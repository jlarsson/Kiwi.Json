using System;

namespace Kiwi.Json.Serialization.TypeBuilders
{
    public class InvalidClassBuilder : AbstractTypeBuilder
    {
        private readonly Type _classType;

        public InvalidClassBuilder(Type classType)
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