using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public static class SyntaxForEsentCursor
    {
        public static void DeleteEq(this IEsentCursor cursor, ITableKey key)
        {
            foreach (var i in cursor.EnumerateEq(key))
            {
                Api.JetDelete(cursor.JetSesid, cursor.JetTableid);
            }
        }
    }
}