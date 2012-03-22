using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Kiwi.Json.Util
{
    public class ThreadSafeList<T> : IEnumerable<T>
    {
        private readonly object _sync = new object();
        private List<T> _list = new List<T>();

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        public void Add(params T[] items)
        {
            lock (_sync)
            {
                var newList = new List<T>(_list.Capacity + items.Length);
                newList.AddRange(_list);
                newList.AddRange(items);

                Interlocked.Exchange(ref _list, newList);
            }
        }
    }
}