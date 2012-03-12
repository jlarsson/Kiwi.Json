using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public static class SyntaxForEsentSession
    {
        public static void CreateDatabase(this IEsentSession session, CreateDatabaseGrbit grbit)
        {
            session.CreateDatabase(null, grbit);
        }

        public static void CreateDatabase(this IEsentSession session, string connect)
        {
            session.CreateDatabase(connect, CreateDatabaseGrbit.None);
        }

        public static void CreateDatabase(this IEsentSession session)
        {
            session.CreateDatabase(null, CreateDatabaseGrbit.None);
        }

        public static void OpenDatabase(this IEsentSession session, OpenDatabaseGrbit grbit)
        {
            session.OpenDatabase(null,grbit);
        }
        public static void OpenDatabase(this IEsentSession session, string connect)
        {
            session.OpenDatabase(connect, OpenDatabaseGrbit.None);
        }
        public static void OpenDatabase(this IEsentSession session)
        {
            session.OpenDatabase(null, OpenDatabaseGrbit.None);

        }

        public static void AttachDatabase(this IEsentSession session)
        {
            session.AttachDatabase(AttachDatabaseGrbit.None);
        }
        
    }
}