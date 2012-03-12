using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent.Ddl
{
    public class ColumnDefinition : IColumnDefinition
    {
        public ColumnDefinition(ITableDefinition table, string name)
        {
            Table = table;
            Name = name;
            JetColumnDef = new JET_COLUMNDEF()
                               {
                                   coltyp = JET_coltyp.Nil
                               };
        }

        #region IColumnDefinition Members

        public string Name { get; set; }

        public ITableDefinition Table { get; protected set; }

        public JET_COLUMNDEF JetColumnDef { get; protected set; }

        #endregion
    }
}