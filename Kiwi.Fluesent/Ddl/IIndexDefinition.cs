using System.Collections.Generic;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent.Ddl
{
    public interface IIndexDefinition
    {
        string Name { get; }
        ITableDefinition Table { get; }
        List<IndexColumnDefinition> Columns { get; }
        CreateIndexGrbit Grbit { get; set; }
        int Density { get; set; }
        
    }
}