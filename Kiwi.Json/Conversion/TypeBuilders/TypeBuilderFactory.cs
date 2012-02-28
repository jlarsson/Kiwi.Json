using System;

namespace Kiwi.Json.Conversion.TypeBuilders
{
    public class TypeBuilderFactory : AbstractTypeBuilder
    {
        public Func<object> OnCreateNull { get; set; }
        public Func<IObjectBuilder> OnCreateObject { get; set; }
        public Func<IArrayBuilder> OnCreateArray { get; set; }

        public override object CreateNull()
        {
            return OnCreateNull == null ? base.CreateNull() : OnCreateNull();
        }

        public override IObjectBuilder CreateObject()
        {
            return OnCreateObject == null ? base.CreateObject() : OnCreateObject();
        }

        public override IArrayBuilder CreateArray()
        {
            return OnCreateArray == null ? base.CreateArray() : OnCreateArray();
        }
    }
}