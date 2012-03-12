using System.Text;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public static class SyntaxForInsertRecord
    {
        public static IInsertRecord AddString(this IInsertRecord record, string name, string value)
        {
            return AddString(record, name, value, Encoding.Unicode);
        }

        public static IInsertRecord AddString(this IInsertRecord record, string name, string value, Encoding encoding)
        {
            record.DefineValue((s, t, m) => Api.SetColumn(s, t, m[name], value, encoding));
            return record;
        }
    }
}