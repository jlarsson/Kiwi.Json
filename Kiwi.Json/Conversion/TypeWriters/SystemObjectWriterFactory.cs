using System;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class SystemObjectWriterFactory : ITypeWriterFactory
    {
        #region ITypeWriterFactory Members

        public ITypeWriter CreateTypeWriter(Type type)
        {
            return type == typeof (object) ? new SystemObjectWriter() : null;
        }

        #endregion
    }
}