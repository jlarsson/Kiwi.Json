using System;

namespace Kiwi.Json.Conversion.TypeWriters
{
    public class SystemObjectWriterFactory : ITypeWriterFactory
    {
        #region ITypeWriterFactory Members

        public Func<ITypeWriter> CreateTypeWriter(Type type)
        {
            return type == typeof (object) ? (Func<ITypeWriter>) (() => new SystemObjectWriter()) : null;
        }

        #endregion
    }
}