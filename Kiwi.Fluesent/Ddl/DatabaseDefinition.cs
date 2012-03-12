using System.Collections.Generic;
using System.Linq;
using Kiwi.Fluesent.Utility;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent.Ddl
{
    public class DatabaseDefinition : IDatabaseDefinition
    {
        private readonly Dictionary<string, ITableDefinition> _tables = new Dictionary<string, ITableDefinition>();

        #region IDatabaseDefinition Members

        public IEnumerable<ITableDefinition> Tables
        {
            get { return _tables.Values; }
        }

        public ITableDefinition Table(string name)
        {
            return _tables.GetOrCreate(name, () => new TableDefinition(this, name));
        }

        public void Create(IEsentTransaction transaction)
        {
            var session = transaction.Session.JetSesid;
            var dbid = transaction.Session.JetDbid;
            foreach (var table in Tables)
            {
                JET_TABLEID tableid;
                Api.JetCreateTable(session, dbid, table.Name, table.Pages, table.Density, out tableid);

                foreach (var column in table.Columns)
                {
                    JET_COLUMNID columnid;
                    Api.JetAddColumn(session, tableid, column.Name, column.JetColumnDef, null, 0, out columnid);
                }

                foreach (var index in table.Indexes)
                {
                    var keyDescription = GetKeyDescription(index);
                    Api.JetCreateIndex(session, tableid, index.Name, index.Grbit, keyDescription,
                                       keyDescription.Length, index.Density);
                }
            }
        }

        #endregion

        private string GetKeyDescription(IIndexDefinition index)
        {
            return index.Columns.Aggregate("", (a, c) => a + (c.SortAscending ? '+' : '-') + c.ColumnName + '\0') + '\0';
        }
    }
}