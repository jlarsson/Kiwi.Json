using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class DictionaryWithWrongKeyTypeBuilder : AbstractTypeBuilder
    {
        private readonly Type _dictionaryType;

        public DictionaryWithWrongKeyTypeBuilder(Type dictionaryType)
        {
            _dictionaryType = dictionaryType;
        }

        public override IObjectBuilder CreateObjectBuilder(ITypeBuilderRegistry registry)
        {
            throw new InvalidClassForDeserializationException(
                string.Format(
                    "Cannot deserialize json into dictionary of type {0}. Dictionary key type must be string.",
                    _dictionaryType));
        }

        public override object CreateNull(ITypeBuilderRegistry registry)
        {
            return null;
        }
    }
}