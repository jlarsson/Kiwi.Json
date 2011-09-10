using System;
using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.Untyped;
using Kiwi.Json.Util;

namespace Kiwi.Json.Conversion
{
    public class DefaultFromJson : IFromJson
    {
        private static readonly Dictionary<Type, IFromJson> FromJsonToSimpleTypes =
            new Dictionary<Type, IFromJson>
                {
                    {typeof (bool), new FromJsonSimple().When<IJsonBool>(o => o.Value)},
                    {typeof (Byte), new FromJsonSimple().When<IJsonInteger>(o => (Byte) o.Value)},
                    {typeof (SByte), new FromJsonSimple().When<IJsonInteger>(o => (SByte) o.Value)},
                    {typeof (Int16), new FromJsonSimple().When<IJsonInteger>(o => (Int16) o.Value)},
                    {typeof (UInt16), new FromJsonSimple().When<IJsonInteger>(o => (UInt16) o.Value)},
                    {typeof (Int32), new FromJsonSimple().When<IJsonInteger>(o => o.Value)},
                    {typeof (UInt32), new FromJsonSimple().When<IJsonInteger>(o => (UInt32) o.Value)},
                    {typeof (Int64), new FromJsonSimple().When<IJsonInteger>(o => (Int64) o.Value)},
                    {typeof (UInt64), new FromJsonSimple().When<IJsonInteger>(o => (UInt64) o.Value)},
                    {
                        typeof (double), new FromJsonSimple()
                        .When<IJsonInteger>(o => (double) (o.Value))
                        .When<IJsonDouble>(o => (o).Value)
                        },
                    {
                        typeof (Single), new FromJsonSimple()
                        .When<IJsonInteger>(o => (Single) (o.Value))
                        .When<IJsonDouble>(o => (Single) (o.Value))
                        },
                    {
                        typeof (string), new FromJsonSimple().When<IJsonString>(o => o.Value)
                        },
                    {typeof (DateTime), new FromJsonSimple().When<IJsonDate>(o => o.Value)},
                    {typeof (object), new FromJsonSimple().When<IJsonValue>(o => o.ToObject())}
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

            var converter = GetFromJson(nativeType);
            if (converter == null)
            {
                throw new InvalidCastException(string.Format("No conversion between {0} and {1} exists.",
                                                             value.GetType(),
                                                             nativeType));
            }
            return converter.FromJson(nativeType, value);
        }

        #endregion

        public T FromJson<T>(IJsonValue value)
        {
            return (T) FromJson(typeof (T), value);
        }

        public IFromJson GetFromJson(Type nativeType)
        {
            return JsonToNativeConverterCache.Lookup(
                nativeType,
                () => GetFromJson(nativeType,
                                  TryGetFromJsonToSimpleType,
                                  TryGetFromJsonToToArrayOfT,
                                  TryGetFromJsonToListOfT,
                                  TryGetFromJsonToClass));
        }

        private static IFromJson GetFromJson(Type nativeType, params Func<Type, IFromJson>[] factories)
        {
            return factories.Select(f => f(nativeType)).Where(c => c != null).FirstOrDefault();
        }

        private IFromJson TryGetFromJsonToSimpleType(Type nativeType)
        {
            IFromJson fromJson;
            return FromJsonToSimpleTypes.TryGetValue(nativeType, out fromJson) ? fromJson : null;
        }

        public IFromJson TryGetFromJsonToToArrayOfT(Type nativeType)
        {
            if (nativeType.IsArray && nativeType.GetArrayRank() == 1)
            {
                var elementType = nativeType.GetElementType();
                return typeof (FromJsonToArray<>).
                           MakeGenericType(elementType).
                           GetConstructor(new[] {typeof (IFromJson)})
                           //.Invoke(new object[] { this }) as IFromJson;
                           .Invoke(new object[] {GetFromJson(elementType)}) as IFromJson;
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
                return null;
            }
            var elementType = interfaceType.GetGenericArguments()[0];

            // Determine concrete IList<T> to instantiate
            var listType = nativeType.IsInterface
                               ? typeof (List<>).MakeGenericType(elementType)
                               : nativeType;

            // List must have default constructor
            if (listType.GetConstructor(Type.EmptyTypes) == null)
            {
                return null;
            }


            return typeof (FromJsonToList<,>)
                       .MakeGenericType(listType, interfaceType.GetGenericArguments()[0])
                       .GetConstructor(new[] {typeof (IFromJson)})
                       .Invoke(new object[] {GetFromJson(elementType)}) as IFromJson;
        }

        private IFromJson TryGetFromJsonToClass(Type nativeType)
        {
            if (!nativeType.IsClass)
            {
                return null;
            }
            var ctor = nativeType.GetConstructor(Type.EmptyTypes);
            if (ctor == null)
            {
                throw new ArgumentException(string.Format("{0} contains no public parameterless constructor", nativeType));
            }
            return typeof (FromJsonToClass<>).MakeGenericType(nativeType)
                       .GetConstructor(new[] {typeof (IFromJson)})
                       .Invoke(new object[] {this}) as IFromJson;
        }
    }
}