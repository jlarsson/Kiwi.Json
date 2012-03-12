using System.Collections.Generic;
using System.Linq;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public class EsentCursor : IEsentCursor
    {
        private readonly string _indexName;
        private readonly IEsentTable _table;

        public EsentCursor(IEsentTable table, string indexName)
        {
            _table = table;
            _indexName = indexName;
        }

        #region IEsentCursor Members

        public JET_SESID JetSesid
        {
            get { return _table.JetSesid; }
        }

        public JET_TABLEID JetTableid
        {
            get { return _table.JetTableid; }
        }

        public T ReadTo<T>(T instance, IRecordMapper<T> mapper)
        {
            return mapper.Map(_table.Session, _table, instance);
        }

        public void Delete()
        {
            Api.JetDelete(JetSesid, JetTableid);
        }

        public IEnumerable<T> Scan<T>(IRecordMapper<T> recordMapper) where T : new()
        {
            Api.JetSetCurrentIndex(JetSesid, JetTableid, _indexName);
            Api.MoveBeforeFirst(JetSesid, JetTableid);

            while (Api.TryMoveNext(JetSesid, JetTableid))
            {
                yield return recordMapper.Map(_table.Session,_table,new T());
            }
        }

        public IEnumerable<T> ScanEq<T>(IRecordMapper<T> recordMapper, ITableKey key) where T : new()
        {
            return EnumerateEq(key).Select(i => recordMapper.Map(_table.Session, _table, new T()));
        }

        public IEnumerable<int> EnumerateEq(ITableKey key)
        {
            Api.JetSetCurrentIndex(JetSesid, JetTableid, _indexName);
            SetKey(key);

            if (Api.TrySeek(JetSesid, JetTableid, SeekGrbit.SeekEQ))
            {
                SetKey(key);
                if (Api.TrySetIndexRange(JetSesid, JetTableid, SetIndexRangeGrbit.RangeInclusive | SetIndexRangeGrbit.RangeUpperLimit))
                {
                    var i = 0;
                    do
                    {
                        yield return i++;
                    } while (Api.TryMoveNext(JetSesid, JetTableid));
                }
            }
        }

        #endregion

        private void SetKey(ITableKey key)
        {
            var mask = MakeKeyGrbit.FullColumnEndLimit | MakeKeyGrbit.FullColumnStartLimit |
                       MakeKeyGrbit.KeyDataZeroLength | MakeKeyGrbit.NewKey | MakeKeyGrbit.NormalizedKey |
                       MakeKeyGrbit.PartialColumnEndLimit | MakeKeyGrbit.PartialColumnStartLimit | MakeKeyGrbit.StrLimit |
                       MakeKeyGrbit.SubStrLimit;
            foreach (var part in key.Parts)
            {
                part(JetSesid, JetTableid, mask);
                mask ^= MakeKeyGrbit.NewKey;
            }
        }
    }
}