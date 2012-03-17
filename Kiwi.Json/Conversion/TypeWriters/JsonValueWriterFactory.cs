using System;
using Kiwi.Json.Untyped;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class JsonValueWriterFactory : ITypeWriterFactory
    {
        #region ITypeWriterFactory Members

        public ITypeWriter CreateTypeWriter(Type type)
        {
            if (typeof (IJsonValue).IsAssignableFrom(type))
            {
                return new JsonValueWriter();
            }
            return null;
        }

        #endregion
    }
}