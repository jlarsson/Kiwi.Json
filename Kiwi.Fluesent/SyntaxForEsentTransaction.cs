using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public static class SyntaxForEsentTransaction
    {
        public static void Commit(this IEsentTransaction transaction)
        {
            transaction.Commit(CommitTransactionGrbit.None);
        }

        public static void Pulse(this IEsentTransaction transaction)
        {
            transaction.Pulse(CommitTransactionGrbit.LazyFlush);
        }

        public static IEsentTable OpenTable(this IEsentTransaction transaction, string name)
        {
            return transaction.OpenTable(name, OpenTableGrbit.None);
        }
    }
}