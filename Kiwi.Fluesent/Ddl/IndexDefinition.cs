using System.Collections.Generic;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent.Ddl
{
    public class IndexDefinition : IIndexDefinition
    {
        public IndexDefinition(ITableDefinition table, string name)
        {
            Table = table;
            Name = name;
            Columns = new List<IndexColumnDefinition>();
            Grbit = CreateIndexGrbit.None;
            Density = 100;
        }

        #region IIndexDefinition Members

        public string Name { get; set; }
        public ITableDefinition Table { get; protected set; }

        public List<IndexColumnDefinition> Columns { get; protected set; }

        public CreateIndexGrbit Grbit { get; set; }
        public int Density { get; set; }

        #endregion
    }
}