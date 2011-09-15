using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kiwi.Json.Conversion.FromJson;
using Kiwi.Json.Conversion.Reflection;
using Kiwi.Json.Conversion.ToJson;
using Kiwi.Json.Untyped;
using Kiwi.Json.Util;

namespace Kiwi.Json.Conversion
{
    public class JsonConverter : IJsonConverter, IToJsonContext, IPrimitiveValueVisitor<IJsonValue>
    {
        public IRegistry<Type, IToJson> ToJsonCache { get; private set; }

        public JsonConverter()
        {
            ToJsonCache = new ThreadsafeRegistry<Type, IToJson>();
            JsonFactory = new JsonFactory();
            FromJsonConverter = new DefaultFromJson();
        }

        public IJsonFactory JsonFactory { get; set; }
        public IFromJson FromJsonConverter { get; set; }

        #region IJsonConverter Members

        public IJsonValue ToJson(object value)
        {
            return PrimitiveType<IJsonValue>.Default.Visit(value, this);
        }

        public object FromJson(Type nativeType, IJsonValue value)
        {
            return FromJsonConverter.FromJson(nativeType, value);
        }

        #endregion

        IJsonValue IToJsonContext.Convert(object value)
        {
            return ToJson(value);
        }

        IJsonObject IJsonFactory.CreateObject()
        {
            return JsonFactory.CreateObject();
        }

        IJsonArray IJsonFactory.CreateArray()
        {
            return JsonFactory.CreateArray();
        }

        IJsonValue IJsonFactory.CreateString(string value)
        {
            return JsonFactory.CreateString(value);
        }

        IJsonValue IJsonFactory.CreateNumber(long value)
        {
            return JsonFactory.CreateNumber(value);
        }

        IJsonValue IJsonFactory.CreateNumber(double value)
        {
            return JsonFactory.CreateNumber(value);
        }

        IJsonValue IJsonFactory.CreateBool(bool value)
        {
            return JsonFactory.CreateBool(value);
        }

        IJsonValue IJsonFactory.CreateDate(DateTime value)
        {
            return JsonFactory.CreateDate(value);
        }

        IJsonValue IJsonFactory.CreateNull()
        {
            return JsonFactory.CreateNull();
        }

        IJsonValue IPrimitiveValueVisitor<IJsonValue>.VisitNull()
        {
            return JsonFactory.CreateNull();
        }

        IJsonValue IPrimitiveValueVisitor<IJsonValue>.VisitBool(bool value)
        {
            return JsonFactory.CreateBool(value);
        }

        IJsonValue IPrimitiveValueVisitor<IJsonValue>.VisitInteger(long value)
        {
            return JsonFactory.CreateNumber(value);
        }

        IJsonValue IPrimitiveValueVisitor<IJsonValue>.VisitString(string value)
        {
            return JsonFactory.CreateString(value);
        }

        IJsonValue IPrimitiveValueVisitor<IJsonValue>.VisitDate(DateTime value)
        {
            return JsonFactory.CreateDate(value);
        }

        IJsonValue IPrimitiveValueVisitor<IJsonValue>.VisitDouble(double value)
        {
            return JsonFactory.CreateNumber(value);
        }

        IJsonValue IPrimitiveValueVisitor<IJsonValue>.VisitObject(object value)
        {
            var type = value.GetType();
            var toJson = ToJsonCache.Lookup(value.GetType(), _ => GetToComplexJson(type));
            if (toJson == null)
            {
                throw new ArgumentException(string.Format("Type {0} is not convertibel to json", type), "value");
            }
            return toJson.ToJson(value, this);
        }

        private IToJson GetToComplexJson(Type type)
        {
            var toJson = TryGetToJsonDictionary(type);
            if (toJson == null)
            {
                toJson = TryGetToJsonEnumerable(type);
            }
            if (toJson == null)
            {
                toJson = TryGetToJsonClass(type);
            }
            return toJson;
        }


        private static IToJson TryGetToJsonDictionary(Type type)
        {
            return (from @interface in type.GetInterfaces()
                    where @interface.IsGenericType
                          && @interface.GetGenericTypeDefinition() == typeof(IDictionary<,>)
                    select typeof(ToJsonDictionary<,>)
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
                          && @interface.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                    select typeof(ToJsonEnumerable<>)
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