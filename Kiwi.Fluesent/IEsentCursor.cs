using System.Collections.Generic;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public interface IEsentCursor
    {
        JET_SESID JetSesid { get; }
        JET_TABLEID JetTableid { get; }
        T ReadTo<T>(T instance, IRecordMapper<T> mapper);
        void Delete();
        IEnumerable<T> Scan<T>(IRecordMapper<T> recordMapper) where T: new ();
        IEnumerable<T> ScanEq<T>(IRecordMapper<T> recordMapper, ITableKey key) where T : new();
        IEnumerable<int> EnumerateEq(ITableKey key);
    }
}