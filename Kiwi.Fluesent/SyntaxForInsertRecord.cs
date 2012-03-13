using System;
using System.Text;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public static class SyntaxForInsertRecord
    {
        public static IInsertRecord Int64(this IInsertRecord record, string name, Int64 value)
        {
            record.DefineValue((s,t,m) => Api.SetColumn(s,t,m[name], value));
            return record;
        }

        public static IInsertRecord String(this IInsertRecord record, string name, string value)
        {
            return String(record, name, value, Encoding.Unicode);
        }

        public static IInsertRecord String(this IInsertRecord record, string name, string value, Encoding encoding)
        {
            record.DefineValue((s, t, m) => Api.SetColumn(s, t, m[name], value, encoding));
            return record;
        }
    }
}