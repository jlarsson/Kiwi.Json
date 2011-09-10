using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion
{
    public class FromJsonToClass<TClass> : IFromJson where TClass : new()
    {
        private static Dictionary<string, IMemberSetter> MemberSetters =
            (
                from property in
                    typeof (TClass).GetProperties(BindingFlags.GetProperty |
                                                  BindingFlags.SetProperty |
                                                  BindingFlags.Public |
                                                  BindingFlags.Instance)
                where
                    (property.GetSetMethod().GetParameters().Length == 1)
                select new PropertySetter(property) as IMemberSetter
            ).Union(
                from field in
                    typeof (TClass).GetFields(BindingFlags.GetField |
                                              BindingFlags.SetField |
                                              BindingFlags.Public |
                                              BindingFlags.Instance)
                select new FieldSetter(field) as IMemberSetter
                ).ToDictionary(v => v.MemberName, v => v);

        public FromJsonToClass(IFromJson fromJsonConverter)
        {
            FromJsonConverter = fromJsonConverter;
        }

        public IFromJson FromJsonConverter { get; private set; }

        #region IFromJson Members

        public object FromJson(Type nativeType, IJsonValue value)
        {
            if (value == null)
            {
                return null;
            }
            var jsonObj = value as IJsonObject;
            if (jsonObj == null)
            {
                throw new ArgumentException(string.Format("{0} is not a JsonObject and cant be converted to class {1}",
                                                          value, typeof (TClass)));
            }

            var instance = new TClass();

            foreach (var setter in MemberSetters.Values)
            {
                IJsonValue jsonProperty;
                if (jsonObj.TryGetValue(setter.MemberName, out jsonProperty))
                {
                    setter.SetValue(instance, FromJsonConverter.FromJson(setter.MemberType, jsonProperty));
                }
            }
            return instance;
        }

        #endregion
    }
}