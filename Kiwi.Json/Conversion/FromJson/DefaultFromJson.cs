using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kiwi.Json.Untyped;
using Kiwi.Json.Util;

namespace Kiwi.Json.Conversion.FromJson
{
    public class DefaultFromJson : IFromJson
    {
        private static readonly ToBase _toString = new ToString();

        private static readonly ToBase[] _to = new ToBase[]
                                                   {
                                                       new ToString(),
                                                       new ToInt32(),
                                                       new ToBool(),
                                                       new ToDateTime(),
                                                       new ToDouble(),
                                                       new ToByte(),
                                                       new ToChar(),
                                                       new ToInt16(),
                                                       new ToInt64(),
                                                       new ToSByte(),
                                                       new ToSingle(),
                                                       new ToUInt16(),
                                                       new ToUInt32(),
                                                       new ToUInt64(),
                                                       new ToDecimal()
                                                   };

        public DefaultFromJson()
        {
            JsonToNativeConverterCache = new ThreadsafeRegistry<Type, IFromJson>();
        }

        public IRegistry<Type, IFromJson> JsonToNativeConverterCache { get; set; }

        #region IFromJson Members

        public object FromJson(Type nativeType, IJsonValue value)
        {
            if (value == null)
            {
                return null;
            }
            if (nativeType == typeof (string))
            {
                return value.Visit(_toString);
            }

            if (nativeType.IsValueType)
            {
                foreach (var to in _to)
                {
                    if (to.Type == nativeType)
                    {
                        return value.Visit(to);
                    }
                }
            }

            if (nativeType == typeof(object))
            {
                return value.ToObject();
            }
            var fromJson = JsonToNativeConverterCache.Lookup(nativeType,
                                                             type => (TryGetFromJsonToToArrayOfT(type) ??
                                                                      TryGetFromJsonToListOfT(type)) ??
                                                                     TryGetFromJsonToClass(type));
            if (fromJson != null)
            {
                return fromJson.FromJson(nativeType, value);
            }

            ThrowInvalidCast(nativeType, value);
            return null;
        }

        #endregion

        private static void ThrowInvalidCast(Type nativeType, IJsonValue value)
        {
            throw new InvalidCastException(string.Format("No conversion between {0} and {1} exists.",
                                                         value.GetType(),
                                                         nativeType));
        }

        public IFromJson TryGetFromJsonToToArrayOfT(Type nativeType)
        {
            if (nativeType.IsArray && nativeType.GetArrayRank() == 1)
            {
                var elementType = nativeType.GetElementType();
                return typeof (FromJsonToArray<>).
                           MakeGenericType(elementType).
                           GetConstructor(new[] {typeof (IFromJson)})
                           .Invoke(new object[] {this}) as IFromJson;
            }
            return null;
        }

        public IFromJson TryGetFromJsonToListOfT(Type nativeType)
        {
            // Check which IList<T> is implemented
            var interfaceType = (from @interface in new[] {nativeType}.Concat(nativeType.GetInterfaces())
                                 where
                                     @interface.IsGenericType &&
                                     @interface.GetGenericTypeDefinition() == typeof (IList<>)
                                 select @interface)
                .FirstOrDefault();

            if (interfaceType == null)
            {
                // Check if it is IEnumerable<T>
                interfaceType = (from @interface in new[] {nativeType}.Concat(nativeType.GetInterfaces())
                                 where
                                     @interface.IsGenericType &&
                                     @interface.GetGenericTypeDefinition() == typeof (IEnumerable<>)
                                 select @interface)
                    .FirstOrDefault();
            }
            if (interfaceType == null)
            {
                return null;
            }

            var elementType = interfaceType.GetGenericArguments()[0];

            // Determine concrete IList<T> to instantiate
            var listType = nativeType.IsInterface
                               ? typeof (List<>).MakeGenericType(elementType)
                               : nativeType;

            if (!nativeType.IsAssignableFrom(listType))
            {
                return null;
            }

            // List must have default constructor
            if (listType.GetConstructor(Type.EmptyTypes) == null)
            {
                return null;
            }


            return typeof (FromJsonToList<,>)
                       .MakeGenericType(listType, interfaceType.GetGenericArguments()[0])
                       .GetConstructor(new[] {typeof (IFromJson)})
                       //.Invoke(new object[] {GetFromJson(elementType)}) as IFromJson;
                       .Invoke(new object[] {this}) as IFromJson;
        }

        private IFromJson TryGetFromJsonToClass(Type nativeType)
        {
            if (!nativeType.IsClass)
            {
                return null;
            }

            //if (nativeType.IsClass)
            {
                var ctor = nativeType.GetConstructor(Type.EmptyTypes);

                if (ctor == null)
                {
                    throw new ArgumentException(string.Format("{0} contains no public parameterless constructor",
                                                              nativeType));
                }
            }

            return typeof (FromJsonToClass<>).MakeGenericType(nativeType)
                       .GetConstructor(new[] {typeof (IFromJson)})
                       .Invoke(new object[] {this}) as IFromJson;
        }

        #region Nested type: ToBase

        protected abstract class ToBase : IJsonValueVisitor<object>
        {
            public abstract Type Type { get; }

            #region IJsonValueVisitor<object> Members

            public virtual object VisitArray(IJsonArray value)
            {
                return InvalidCast(value);
            }

            public virtual object VisitBool(IJsonBool value)
            {
                return InvalidCast(value);
            }

            public virtual object VisitDate(IJsonDate value)
            {
                return InvalidCast(value);
            }

            public virtual object VisitDouble(IJsonDouble value)
            {
                return InvalidCast(value);
            }

            public virtual object VisitInteger(IJsonInteger value)
            {
                return InvalidCast(value);
            }

            public virtual object VisitNull(IJsonNull value)
            {
                return InvalidCast(value);
            }

            public virtual object VisitObject(IJsonObject value)
            {
                return InvalidCast(value);
            }

            public virtual object VisitString(IJsonString value)
            {
                return InvalidCast(value);
            }

            #endregion

            protected object InvalidCast(IJsonValue value)
            {
                ThrowInvalidCast(Type, value);
                return null;
            }
        }

        protected class ToBase<T> : ToBase
        {
            public override Type Type
            {
                get { return typeof (T); }
            }
        }

        #endregion

        #region Nested type: ToBool

        protected class ToBool : ToBase<bool>
        {
            public override object VisitBool(IJsonBool value)
            {
                return value.Value;
            }
        }

        #endregion

        #region Nested type: ToByte

        protected class ToByte : ToBase<byte>
        {
            public override object VisitInteger(IJsonInteger value)
            {
                return Convert.ToByte(value.Value);
            }

            public override object VisitDouble(IJsonDouble value)
            {
                return Convert.ToByte(value.Value);
            }
        }

        #endregion

        #region Nested type: ToChar

        protected class ToChar : ToBase<char>
        {
            public override object VisitInteger(IJsonInteger value)
            {
                return Convert.ToChar(value.Value);
            }

            public override object VisitDouble(IJsonDouble value)
            {
                return Convert.ToChar(value.Value);
            }
        }

        #endregion

        #region Nested type: ToDateTime

        protected class ToDateTime : ToBase<DateTime>
        {
            public override object VisitDate(IJsonDate value)
            {
                return value.Value;
            }
        }

        #endregion

        #region Nested type: ToDecimal

        protected class ToDecimal : ToBase<Decimal>
        {
            public override object VisitInteger(IJsonInteger value)
            {
                return Convert.ToDecimal(value.Value);
            }

            public override object VisitDouble(IJsonDouble value)
            {
                return Convert.ToDecimal(value.Value);
            }
        }

        #endregion

        #region Nested type: ToDouble

        protected class ToDouble : ToBase<double>
        {
            public override object VisitInteger(IJsonInteger value)
            {
                return Convert.ToDouble(value.Value);
            }

            public override object VisitDouble(IJsonDouble value)
            {
                return Convert.ToDouble(value.Value);
            }
        }

        #endregion

        #region Nested type: ToInt16

        protected class ToInt16 : ToBase<Int16>
        {
            public override object VisitInteger(IJsonInteger value)
            {
                return Convert.ToInt16(value.Value);
            }

            public override object VisitDouble(IJsonDouble value)
            {
                return Convert.ToInt16(value.Value);
            }
        }

        #endregion

        #region Nested type: ToInt32

        protected class ToInt32 : ToBase<Int32>
        {
            public override object VisitInteger(IJsonInteger value)
            {
                return Convert.ToInt32(value.Value);
            }

            public override object VisitDouble(IJsonDouble value)
            {
                return Convert.ToInt32(value.Value);
            }
        }

        #endregion

        #region Nested type: ToInt64

        protected class ToInt64 : ToBase<Int64>
        {
            public override object VisitInteger(IJsonInteger value)
            {
                return Convert.ToInt64(value.Value);
            }

            public override object VisitDouble(IJsonDouble value)
            {
                return Convert.ToInt64(value.Value);
            }
        }

        #endregion

        #region Nested type: ToSByte

        protected class ToSByte : ToBase<sbyte>
        {
            public override object VisitInteger(IJsonInteger value)
            {
                return Convert.ToSByte(value.Value);
            }

            public override object VisitDouble(IJsonDouble value)
            {
                return Convert.ToSByte(value.Value);
            }
        }

        #endregion

        #region Nested type: ToSingle

        protected class ToSingle : ToBase<Single>
        {
            public override object VisitInteger(IJsonInteger value)
            {
                return Convert.ToSingle(value.Value);
            }

            public override object VisitDouble(IJsonDouble value)
            {
                return Convert.ToSingle(value.Value);
            }
        }

        #endregion

        #region Nested type: ToString

        protected new class ToString : ToBase<string>
        {
            public override object VisitString(IJsonString value)
            {
                return value.Value;
            }
        }

        #endregion

        #region Nested type: ToUInt16

        protected class ToUInt16 : ToBase<UInt16>
        {
            public override object VisitInteger(IJsonInteger value)
            {
                return Convert.ToUInt16(value.Value);
            }

            public override object VisitDouble(IJsonDouble value)
            {
                return Convert.ToUInt16(value.Value);
            }
        }

        #endregion

        #region Nested type: ToUInt32

        protected class ToUInt32 : ToBase<UInt32>
        {
            public override object VisitInteger(IJsonInteger value)
            {
                return Convert.ToUInt32(value.Value);
            }

            public override object VisitDouble(IJsonDouble value)
            {
                return Convert.ToUInt32(value.Value);
            }
        }

        #endregion

        #region Nested type: ToUInt64

        protected class ToUInt64 : ToBase<UInt64>
        {
            public override object VisitInteger(IJsonInteger value)
            {
                return Convert.ToUInt64(value.Value);
            }

            public override object VisitDouble(IJsonDouble value)
            {
                return Convert.ToUInt64(value.Value);
            }
        }

        #endregion
    }
}