using System;
using System.Collections.Generic;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public class RecordMapper<T> : IRecordMapper<T>
    {
        private readonly List<Action<JET_SESID, JET_TABLEID, IDictionary<string, JET_COLUMNID>, T>> _maps =
            new List<Action<JET_SESID, JET_TABLEID, IDictionary<string, JET_COLUMNID>, T>>();

        #region IRecordMapper<T> Members

        public void DefineMapping(Action<JET_SESID, JET_TABLEID, IDictionary<string, JET_COLUMNID>, T> map)
        {
            _maps.Add(map);
        }

        public T Map(JET_SESID session, JET_TABLEID table, T instance)
        {
            IDictionary<string, JET_COLUMNID> columns = null;
            foreach (var action in _maps)
            {
                var c = columns ?? (columns = Api.GetColumnDictionary(session, table));
                action(session, table, c, instance);
            }
            return instance;
        }

        #endregion
    }
}