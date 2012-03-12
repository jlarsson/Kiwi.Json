using System;
using System.Collections.Generic;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public class EsentTable : IEsentTable
    {
        private IDictionary<string, JET_COLUMNID> _columnCache;

        public EsentTable(IEsentTransaction transaction, Table table)
        {
            Transaction = transaction;
            Table = table;
        }

        protected IDictionary<string, JET_COLUMNID> ColumnCache
        {
            get { return _columnCache ?? (_columnCache = Api.GetColumnDictionary(Transaction.Session.Session, Table)); }
        }

        #region IEsentTable Members

        public IEsentTransaction Transaction { get; protected set; }
        public Table Table { get; set; }

        public IInsertRecord CreateInsertRecord()
        {
            return new InsertRecord(this);
        }

        public ITableKey CreateKey()
        {
            return new TableKey(this);
        }

        public IEsentTableSearch<T> CreateSearch<T>(IRecordMapper<T> mapper) where T : new()
        {
            return new EsentTableSearch<T>(Transaction.Session.JetSesid, Table.JetTableid, mapper);
        }

        public void Dispose()
        {
            Table.Dispose();
        }

        #endregion

        private void Insert(IEnumerable<Action<JET_SESID, JET_TABLEID, IDictionary<string, JET_COLUMNID>>> adders)
        {
            var session = Transaction.Session.JetSesid;
            var table = Table.JetTableid;

            using (var update = new Update(session, table, JET_prep.Insert))
            {
                foreach (var adder in adders)
                {
                    adder(session, table, ColumnCache);
                }
                update.Save();
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
                _table.Insert(_adders);
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