using System.Collections.Generic;

namespace Kiwi.Fluesent.Ddl
{
    public interface IDatabaseDefinition
    {
        IEnumerable<ITableDefinition> Tables { get; }
        ITableDefinition Table(string name);

        void Create(IEsentTransaction transaction);
    }
}