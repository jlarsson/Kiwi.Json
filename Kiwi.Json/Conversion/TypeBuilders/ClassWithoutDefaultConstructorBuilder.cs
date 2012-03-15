using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class ClassWithoutDefaultConstructorBuilder : AbstractTypeBuilder
    {
        private readonly Type _classType;

        public ClassWithoutDefaultConstructorBuilder(Type classType)
        {
            _classType = classType;
        }

        public override IObjectBuilder CreateObjectBuilder(ITypeBuilderRegistry registry)
        {
            throw new InvalidClassForDeserializationException(
                string.Format("Cannot deserialize instance of {0} since it has no public default constructor defined",
                              _classType));
        }

        public override object CreateNull(ITypeBuilderRegistry registry)
        {
            return null;
        }
    }
}