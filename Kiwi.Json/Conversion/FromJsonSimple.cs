using System;
using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion
{
    public class FromJsonSimple : IFromJson
    {
        private static readonly object[] EmptyParameters = new object[0];
        private readonly List<FromJsonCase> _cases = new List<FromJsonCase>();

        #region IFromJson Members

        public object FromJson(Type nativeType, IJsonValue value)
        {
            if (value == null)
            {
                if (nativeType.IsClass)
                {
                    return null;
                }
                return nativeType.GetConstructor(Type.EmptyTypes).Invoke(EmptyParameters); // Equivalent of default(T) ?
            }
            var convert = _cases
                .Where(c => c.JsonType.IsAssignableFrom(value.GetType()))
                .Select(c => c.Convert)
                .FirstOrDefault();
            if (convert == null)
            {
                throw new InvalidCastException(string.Format("Cannot convert {0} to {1}", value.GetType(), nativeType));
            }
            return convert(value);
        }

        #endregion

        public FromJsonSimple When<T>(Func<T, object> func)
        {
            _cases.Add(new FromJsonCase {JsonType = typeof (T), Convert = o => func((T) o)});
            return this;
        }

        #region Nested type: FromJsonCase

        private class FromJsonCase
        {
            public Type JsonType { get; set; }
            public Func<object, object> Convert { get; set; }
        }

        #endregion
    }
}