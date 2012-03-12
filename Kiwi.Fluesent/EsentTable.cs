using System;
using System.Collections.Generic;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public class EsentTable : IEsentTable
    {
        private IDictionary<string, JET_COLUMNID> _columnNames;
        protected readonly Table _table;

        public EsentTable(IEsentTransaction transaction, Table table)
        {
            Transaction = transaction;
            _table = table;
        }

        #region IEsentTable Members

        public JET_SESID JetSesid
        {
            get { return Transaction.JetSesid; }
        }

        public JET_TABLEID JetTableid
        {
            get { return _table.JetTableid; }
        }

        public IEsentSession Session
        {
            get { return Transaction.Session; }
        }

        public IEsentTransaction Transaction { get; protected set; }

        public IDictionary<string, JET_COLUMNID> ColumnNames
        {
            get { return _columnNames ?? (_columnNames = Api.GetColumnDictionary(JetSesid, JetTableid)); }
        }

        public IInsertRecord CreateInsertRecord()
        {
            return new InsertRecord(this);
        }

        public ITableKey CreateKey()
        {
            return new TableKey(this);
        }

        public IEsentCursor CreateCursor(string indexName)
        {
            return new EsentCursor(this, indexName);
        }

        public void Dispose()
        {
            _table.Dispose();
        }

        #endregion

        private T Insert<T>(IEnumerable<Action<JET_SESID, JET_TABLEID, IDictionary<string, JET_COLUMNID>>> adders,
                            Func<JET_SESID, JET_TABLEID, IDictionary<string, JET_COLUMNID>, T> getResult)
        {
            var session = Transaction.Session.JetSesid;
            var table = _table.JetTableid;

            using (var update = new Update(session, table, JET_prep.Insert))
            {
                var result = getResult == null ? default(T) : getResult(session, table, ColumnNames);
                foreach (var adder in adders)
                {
                    adder(session, table, ColumnNames);
                }
                update.Save();

                return result;
            }
        }

        #region Nested type: InsertRecord

        public class InsertRecord : IInsertRecord
        {
            private readonly List<Action<JET_SESID, JET_TABLEID, IDictionary<string, JET_COLUMNID>>> _adders =
                new List<Action<JET_SESID, JET_TABLEID, IDictionary<string, JET_COLUMNID>>>();

            private readonly EsentTable _table;

            public InsertRecord(EsentTable table)
            {
                _table = table;
            }

            #region IInsertRecord Members

            public void DefineValue(Action<JET_SESID, JET_TABLEID, IDictionary<string, JET_COLUMNID>> adder)
            {
                _adders.Add(adder);
            }

            public void Insert()
            {
                _table.Insert<int>(_adders, null);
            }

            public Int64 InsertWithAutoIncrement64(string columnName)
            {
                return _table.Insert(_adders,
                                     (s, t, m) =>
                                     Api.RetrieveColumnAsInt64(s, t, m[columnName], RetrieveColumnGrbit.RetrieveCopy).
                                         Value);
            }

            #endregion
        }

        #endregion

        #region Nested type: TableKey

        public class TableKey : ITableKey
        {
            private readonly List<Action<JET_SESID, JET_TABLEID, MakeKeyGrbit>> _definers =
                new List<Action<JET_SESID, JET_TABLEID, MakeKeyGrbit>>();

            private readonly EsentTable _table;

            public TableKey(EsentTable table)
            {
                _table = table;
            }

            #region ITableKey Members

            public IEnumerable<Action<JET_SESID, JET_TABLEID, MakeKeyGrbit>> Parts
            {
                get { return _definers; }
            }

            public void DefineKeyPart(Action<JET_SESID, JET_TABLEID, MakeKeyGrbit> definer)
            {
                _definers.Add(definer);
            }

            #endregion
        }

        #endregion
    }
}