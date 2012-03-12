using System.Collections.Generic;

namespace Kiwi.Fluesent
{
    public interface IEsentTableSearch<T>
    {
        string IndexName { get; set; }
        IEnumerable<T> FindAll();
        IEnumerable<T> FindEq(ITableKey key);
    }
}