using System;
using System.Collections.Generic;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public class RecordMapper<T> : IRecordMapper<T>
    {
        private readonly List<Action<IEsentSession, IEsentTable, IDictionary<string, JET_COLUMNID>, T>> _maps =
            new List<Action<IEsentSession, IEsentTable, IDictionary<string, JET_COLUMNID>, T>>();

        #region IRecordMapper<T> Members

        public void DefineMapping(Action<IEsentSession, IEsentTable, IDictionary<string, JET_COLUMNID>, T> map)
        {
            _maps.Add(map);
        }

        public T Map(IEsentSession session, IEsentTable table, T instance)
        {
            foreach (var action in _maps)
            {
                action(session, table, table.ColumnNames, instance);
            }
            return instance;
        }

        #endregion
    }
}