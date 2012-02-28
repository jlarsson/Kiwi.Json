using System;

namespace Kiwi.Json
{
    public class MissingDefaultConstructorException : JsonException
    {
        public MissingDefaultConstructorException(Type type)
            : base(
                string.Format("Cannot create instance of {0} since it has no public default constructor defined", type))
        {
        }
    }
}