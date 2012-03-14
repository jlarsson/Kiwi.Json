using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace Kiwi.Json.PerformanceTests
{
    [TestFixture, Explicit]
    public class ScratchPad
    {
        private void RunTest(ITypeSwitch<object> typeSwitch)
        {
            var values = new object[]
                             {
                                 (byte) 1,
                                 (sbyte) 2,
                                 (short) 1,
                                 (short) 3,
                                 4,
                                 (uint) 5,
                                 (long) 6,
                                 (ulong) 7,
                                 "hello", "world", null,
                                 DateTime.Now,
                                 Math.PI,
                                 1.234f,
                                 0.5d
                             };

            var visitor = new NullValueVisitor<object>();
            for (var j = 0; j < 1000; ++j)
            {
                for (var i = 0; i < values.Length; ++i)
                {
                    typeSwitch.Visit(values[i], visitor);
                }
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (var j = 0; j < 1000000; ++j)
            {
                for (var i = 0; i < values.Length; ++i)
                {
                    typeSwitch.Visit(values[i], visitor);
                }
            }
            stopwatch.Stop();


            Console.Out.WriteLine("{0}: {1}", typeSwitch.GetType(), stopwatch.Elapsed);
        }

        private class NullValueVisitor<T> : IValueVisitor<object>
        {
            #region IValueVisitor<object> Members

            public object VisitNull()
            {
                return null;
            }

            public object VisitBool(bool value)
            {
                return value;
            }

            public object VisitInteger(long value)
            {
                return value;
            }

            public object VisitString(string value)
            {
                return value;
            }

            public object VisitDate(DateTime value)
            {
                return value;
            }

            public object VisitDouble(double value)
            {
                return value;
            }

            public object VisitObject(object value)
            {
                return value;
            }

            #endregion
        }

        [Test]
        public void Test()
        {
            RunTest(new TypeSwitch<object>());
            RunTest(new DictTypeSwitch<object>());
            RunTest(new ListTypeSwitch<object>());
        }
    }

    public class ListTypeSwitch<T> : ITypeSwitch<T>
    {
        private readonly List<Tuple<Type, Func<object, IValueVisitor<T>, T>>>
            _map = new List<Tuple<Type, Func<object, IValueVisitor<T>, T>>>
                       {
                           new Tuple<Type, Func<object, IValueVisitor<T>, T>>(typeof (byte),
                                                                              (o, v) =>
                                                                              v.VisitInteger(
                                                                                  ((IConvertible) o).ToInt64(null))),
                           new Tuple<Type, Func<object, IValueVisitor<T>, T>>(typeof (sbyte),
                                                                              (o, v) =>
                                                                              v.VisitInteger(
                                                                                  ((IConvertible) o).ToInt64(null))),
                           new Tuple<Type, Func<object, IValueVisitor<T>, T>>(typeof (short),
                                                                              (o, v) =>
                                                                              v.VisitInteger(
                                                                                  ((IConvertible) o).ToInt64(null))),
                           new Tuple<Type, Func<object, IValueVisitor<T>, T>>(typeof (ushort),
                                                                              (o, v) =>
                                                                              v.VisitInteger(
                                                                                  ((IConvertible) o).ToInt64(null))),
                           new Tuple<Type, Func<object, IValueVisitor<T>, T>>(typeof (int),
                                                                              (o, v) =>
                                                                              v.VisitInteger(
                                                                                  ((IConvertible) o).ToInt64(null))),
                           new Tuple<Type, Func<object, IValueVisitor<T>, T>>(typeof (uint),
                                                                              (o, v) =>
                                                                              v.VisitInteger(
                                                                                  ((IConvertible) o).ToInt64(null))),
                           new Tuple<Type, Func<object, IValueVisitor<T>, T>>(typeof (long),
                                                                              (o, v) =>
                                                                              v.VisitInteger(
                                                                                  ((IConvertible) o).ToInt64(null))),
                           new Tuple<Type, Func<object, IValueVisitor<T>, T>>(typeof (ulong),
                                                                              (o, v) =>
                                                                              v.VisitInteger(
                                                                                  ((IConvertible) o).ToInt64(null))),
                           new Tuple<Type, Func<object, IValueVisitor<T>, T>>(typeof (string),
                                                                              (o, v) => v.VisitString((string) o)),
                           new Tuple<Type, Func<object, IValueVisitor<T>, T>>(typeof (DateTime),
                                                                              (o, v) => v.VisitDate((DateTime) o)),
                           new Tuple<Type, Func<object, IValueVisitor<T>, T>>(typeof (double),
                                                                              (o, v) =>
                                                                              v.VisitDouble(
                                                                                  ((IConvertible) o).ToDouble(null))),
                           new Tuple<Type, Func<object, IValueVisitor<T>, T>>(typeof (float),
                                                                              (o, v) =>
                                                                              v.VisitDouble(
                                                                                  ((IConvertible) o).ToDouble(null))),
                           new Tuple<Type, Func<object, IValueVisitor<T>, T>>(typeof (decimal),
                                                                              (o, v) =>
                                                                              v.VisitDouble(
                                                                                  ((IConvertible) o).ToDouble(null))),
                           new Tuple<Type, Func<object, IValueVisitor<T>, T>>(typeof (bool),
                                                                              (o, v) => v.VisitBool((bool) o))
                       };

        #region ITypeSwitch<T> Members

        public T Visit(object value, IValueVisitor<T> visitor)
        {
            if (value == null)
            {
                return visitor.VisitNull();
            }
            foreach (var t in _map)
            {
                if (t.Item1 == value.GetType())
                {
                    return t.Item2(value, visitor);
                }
            }

            return visitor.VisitObject(value);
        }

        #endregion
    }


    public class DictTypeSwitch<T> : ITypeSwitch<T>
    {
        private readonly Dictionary<Type, Func<object, IValueVisitor<T>, T>> _map =
            new Dictionary<Type, Func<object, IValueVisitor<T>, T>>
                {
                    {typeof (byte), (o, v) => v.VisitInteger(((IConvertible) o).ToInt64(null))},
                    {typeof (sbyte), (o, v) => v.VisitInteger(((IConvertible) o).ToInt64(null))},
                    {typeof (short), (o, v) => v.VisitInteger(((IConvertible) o).ToInt64(null))},
                    {typeof (ushort), (o, v) => v.VisitInteger(((IConvertible) o).ToInt64(null))},
                    {typeof (int), (o, v) => v.VisitInteger(((IConvertible) o).ToInt64(null))},
                    {typeof (uint), (o, v) => v.VisitInteger(((IConvertible) o).ToInt64(null))},
                    {typeof (long), (o, v) => v.VisitInteger(((IConvertible) o).ToInt64(null))},
                    {typeof (ulong), (o, v) => v.VisitInteger(((IConvertible) o).ToInt64(null))},
                    {typeof (string), (o, v) => v.VisitString((string) o)},
                    {typeof (DateTime), (o, v) => v.VisitDate((DateTime) o)},
                    {typeof (double), (o, v) => v.VisitDouble(((IConvertible) o).ToDouble(null))},
                    {typeof (float), (o, v) => v.VisitDouble(((IConvertible) o).ToDouble(null))},
                    {typeof (decimal), (o, v) => v.VisitDouble(((IConvertible) o).ToDouble(null))},
                    {typeof (bool), (o, v) => v.VisitBool((bool) o)},
                };

        #region ITypeSwitch<T> Members

        public T Visit(object value, IValueVisitor<T> visitor)
        {
            if (value == null)
            {
                return visitor.VisitNull();
            }
            Func<object, IValueVisitor<T>, T> f;
            return _map.TryGetValue(value.GetType(), out f) ? f(value, visitor) : visitor.VisitObject(value);
        }

        #endregion
    }


    public interface ITypeSwitch<T>
    {
        T Visit(object value, IValueVisitor<T> visitor);
    }

    public class TypeSwitch<T> : ITypeSwitch<T>
    {
        #region ITypeSwitch<T> Members

        public T Visit(object value, IValueVisitor<T> visitor)
        {
            if (value == null)
            {
                return visitor.VisitNull();
            }
            if (value is string)
            {
                return visitor.VisitString((string) value);
            }
            if ((value is int) || (value is long) || (value is uint) || (value is ulong) || (value is short) ||
                (value is ushort) || (value is byte) || (value is sbyte))
            {
                return visitor.VisitInteger(((IConvertible) value).ToInt64(null));
            }
            if ((value is double) || (value is float) || (value is decimal))
            {
                return visitor.VisitDouble(((IConvertible) value).ToDouble(null));
            }
            if (value is DateTime)
            {
                return visitor.VisitDate((DateTime) value);
            }
            return visitor.VisitObject(value);
        }

        #endregion
    }


    public interface IValueVisitor<T>
    {
        T VisitNull();
        T VisitBool(bool value);
        T VisitInteger(long value);
        T VisitString(string value);
        T VisitDate(DateTime value);
        T VisitDouble(double value);
        T VisitObject(object value);
    }
}