using System.Collections.Generic;
using Kiwi.Fluesent.Utility;

namespace Kiwi.Fluesent.Ddl
{
    public class TableDefinition : ITableDefinition
    {
        private readonly Dictionary<string, IColumnDefinition> _columns = new Dictionary<string, IColumnDefinition>();
        private readonly Dictionary<string, IIndexDefinition> _indexes = new Dictionary<string, IIndexDefinition>();

        public TableDefinition(IDatabaseDefinition database, string name)
        {
            Name = name;
            Database = database;
            Pages = 0;
            Density = 100;
        }

        #region ITableDefinition Members

        public string Name { get; protected set; }

        public IDatabaseDefinition Database { get; protected set; }

        public int Pages { get; set; }
        public int Density { get; set; }

        public IEnumerable<IColumnDefinition> Columns
        {
            get { return _columns.Values; }
        }

        public IEnumerable<IIndexDefinition> Indexes
        {
            get { return _indexes.Values; }
        }


        public IColumnDefinition Column(string name)
        {
            return _columns.GetOrCreate(name, () => new ColumnDefinition(this, name));
        }

        public IIndexDefinition Index(string name)
        {
            return _indexes.GetOrCreate(name, () => new IndexDefinition(this, name));
        }

        #endregion
    }
}