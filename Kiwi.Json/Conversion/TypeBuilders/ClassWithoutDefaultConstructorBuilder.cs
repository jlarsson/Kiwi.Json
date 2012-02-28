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

        public override IObjectBuilder CreateObject()
        {
            throw new MissingDefaultConstructorException(_classType);
        }

        public override object CreateNull()
        {
            return null;
        }
    }
}