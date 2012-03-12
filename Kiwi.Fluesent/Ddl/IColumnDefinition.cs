using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent.Ddl
{
    public interface IColumnDefinition
    {
        string Name { get; }
        ITableDefinition Table { get; }
        JET_COLUMNDEF JetColumnDef { get; }
        
    }
}