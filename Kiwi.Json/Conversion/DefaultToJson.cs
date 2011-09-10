using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kiwi.Json.Untyped;
using Kiwi.Json.Util;

namespace Kiwi.Json.Conversion
{
    public class DefaultToJson : IToJson
    {
        private static readonly object[] EmptyParameters = new object[0];

        private static readonly Dictionary<Type, IToJson> ToJsonSimpleTypes =
            new Dictionary<Type, IToJson>
                {
                    {typeof (string), new ToJsonString()},
                    {typeof (bool), new ToJsonBool()},
                    {typeof (Byte), new ToJsonInt()},
                    {typeof (SByte), new ToJsonInt()},
                    {typeof (Int16), new ToJsonInt()},
                    {typeof (UInt16), new ToJsonInt()},
                    {typeof (Int32), new ToJsonInt()},
                    {typeof (UInt32), new ToJsonInt()},
                    {typeof (Int64), new ToJsonInt()},
                    {typeof (UInt64), new ToJsonInt()},
                    {typeof (double), new ToJsonDouble()},
                    {typeof (float), new ToJsonDouble()},
                    {typeof (DateTime), new ToJsonDateTime()}
                };

        public DefaultToJson()
        {
            ToJsonCache = new ThreadsafeRegistry<Type, IToJson>();
        }


        public IRegistry<Type, IToJson> ToJsonCache { get; set; }

        #region IToJson Members

        public IJsonValue ToJson(object value, IToJsonContext context)
        {
            var jsonValue = context.TryCreatePrimitiveValue(value);
            return jsonValue ?? GetToJson(value.GetType()).ToJson(value, context);
        }

        #endregion

        protected IToJson GetToJson(Type type)
        {
            return ToJsonCache.Lookup(type,
                                      () =>
                                      GetToJson(type, TryGetToJsonSimpleType,
                                                TryGetToJsonDictionary,
                                                TryGetToJsonEnumerable,
                                                TryGetToJsonClass));
        }

        private static IToJson GetToJson(Type type, params Func<Type, IToJson>[] factories)
        {
            return factories.Select(f => f(type)).Where(c => c != null).FirstOrDefault();
        }

        private static IToJson TryGetToJsonSimpleType(Type type)
        {
            IToJson converter;
            return ToJsonSimpleTypes.TryGetValue(type, out converter) ? converter : null;
        }

        private static IToJson TryGetToJsonDictionary(Type type)
        {
            return (from @interface in type.GetInterfaces()
                    where @interface.IsGenericType
                          && @interface.GetGenericTypeDefinition() == typeof (IDictionary<,>)
                    select typeof (ToJsonDictionary<,>)
                               .MakeGenericType(@interface.GetGenericArguments()[0], @interface.GetGenericArguments()[1])
                               .GetConstructor(Type.EmptyTypes)
                               .Invoke(new object[0]) as IToJson
                   )
                .FirstOrDefault();
        }

        private static IToJson TryGetToJsonEnumerable(Type type)
        {
            return (from @interface in type.GetInterfaces()
                    where @interface.IsGenericType
                          && @interface.GetGenericTypeDefinition() == typeof (IEnumerable<>)
                    select typeof (ToJsonEnumerable<>)
                               .MakeGenericType(@interface.GetGenericArguments()[0])
                               .GetConstructor(Type.EmptyTypes)
                               .Invoke(new object[0]) as IToJson
                   )
                .FirstOrDefault();
        }

        private static IToJson TryGetToJsonClass(Type type)
        {
            var propertyGetters =
                from property in
                    type.GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance)
                where (property.GetGetMethod().GetParameters().Length == 0)
                select new PropertyGetter(property) as IMemberGetter;

            var fieldGetters =
                from field in
                    type.GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Instance)
                select new FieldGetter(field) as IMemberGetter;


            return new ToJsonClass
                       {
                           Getters = propertyGetters.Concat(fieldGetters).ToArray()
                       };
        }
    }
}