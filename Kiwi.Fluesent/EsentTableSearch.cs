using System.Collections.Generic;
using System.Linq;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public class EsentTableSearch<T> : IEsentTableSearch<T> where T : new()
    {
        private readonly IRecordMapper<T> _mapper;
        private readonly JET_SESID _session;
        private readonly JET_TABLEID _table;

        public EsentTableSearch(JET_SESID session, JET_TABLEID table, IRecordMapper<T> mapper)
        {
            _session = session;
            _table = table;
            _mapper = mapper;
        }

        #region IEsentTableSearch<T> Members

        public string IndexName { get; set; }

        public IEnumerable<T> FindAll()
        {
            Api.JetSetCurrentIndex(_session, _table, IndexName);
            if (Api.TryMoveFirst(_session,_table))
            {
                return EnumerateRecords(_session, _table, _mapper);
            }
            return Enumerable.Empty<T>();
        }

        public IEnumerable<T> FindEq(ITableKey key)
        {
            Api.JetSetCurrentIndex(_session, _table, IndexName);
            SetKey(_session, _table, key);

            if (Api.TrySeek(_session, _table, SeekGrbit.SeekEQ))
            {
                SetKey(_session, _table, key);
                if (Api.TrySetIndexRange(_session, _table,
                                         SetIndexRangeGrbit.RangeInclusive | SetIndexRangeGrbit.RangeUpperLimit))
                {
                    return EnumerateRecords(_session, _table, _mapper);
                }
            }
            return Enumerable.Empty<T>();
        }

        #endregion

        private IEnumerable<T> EnumerateRecords<T>(JET_SESID session, JET_TABLEID table, IRecordMapper<T> mapper)
            where T : new()
        {
            do
            {
                yield return mapper.Map(session, table, new T());
            } while (Api.TryMoveNext(session, table));
        }

        private static void SetKey(JET_SESID session, JET_TABLEID table, ITableKey key)
        {
            var mask = MakeKeyGrbit.FullColumnEndLimit | MakeKeyGrbit.FullColumnStartLimit |
                       MakeKeyGrbit.KeyDataZeroLength | MakeKeyGrbit.NewKey | MakeKeyGrbit.NormalizedKey |
                       MakeKeyGrbit.PartialColumnEndLimit | MakeKeyGrbit.PartialColumnStartLimit | MakeKeyGrbit.StrLimit |
                       MakeKeyGrbit.SubStrLimit;
            foreach (var part in key.Parts)
            {
                part(session, table, mask);
                mask ^= MakeKeyGrbit.NewKey;
            }
        }
    }
}