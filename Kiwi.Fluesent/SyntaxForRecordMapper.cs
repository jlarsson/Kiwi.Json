using System;
using System.Text;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public static class SyntaxForRecordMapper
    {
        public static IRecordMapper<T> String<T>(this IRecordMapper<T> mapper, string columnName, Action<T,string> map)
        {
            return String<T>(mapper, columnName, Encoding.Unicode, map);
        }

        public static IRecordMapper<T> String<T>(this IRecordMapper<T> mapper, string columnName, Encoding encoding, Action<T,string> map)
        {
            mapper.DefineMapping((s,t,c,o) => map(o, Api.RetrieveColumnAsString(s,t,c[columnName])));
            return mapper;
        }
    }
}