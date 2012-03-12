using System.Collections.Generic;

namespace Kiwi.Fluesent.Ddl
{
    public interface ITableDefinition
    {
        string Name { get; }
        IDatabaseDefinition Database { get; }
        int Pages { get; set; }
        int Density { get; set; }
        IEnumerable<IColumnDefinition> Columns { get; }
        IEnumerable<IIndexDefinition> Indexes { get; }
        IColumnDefinition Column(string name);
        IIndexDefinition Index(string name);
    }
}
