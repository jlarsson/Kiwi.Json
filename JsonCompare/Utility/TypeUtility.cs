using System;
using System.Linq;

namespace JsonCompare.Utility
{
    public class TypeUtility
    {
        public static string GetTypeName<T>()
        {
            return GetTypeName(typeof (T));
        }
        public static string GetTypeName(Type type)
        {
            if (type.IsGenericType)
            {
                return new string(
                           type.Name.TakeWhile(c => char.IsLetterOrDigit(c) || (c == '_')).ToArray())
                       + '<' + string.Join(",", type.GetGenericArguments().Select(GetTypeName)) + '>';
            }
            return type.Name;
        }
    }
}
