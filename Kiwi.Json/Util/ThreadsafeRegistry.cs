using System;
using System.Collections.Generic;
using System.Threading;

namespace Kiwi.Json.Util
{
    public class ThreadsafeRegistry<TKey, TValue> : IRegistry<TKey, TValue> where TKey : class
    {
        private readonly object _sync = new object();

        private Dictionary<TKey, TValue> _dict;

        public ThreadsafeRegistry()
        {
            _dict = new Dictionary<TKey, TValue>();
        }

        public ThreadsafeRegistry(IDictionary<TKey, TValue> initial)
        {
            _dict = new Dictionary<TKey, TValue>(initial);
        }

        #region IRegistry<TKey,TValue> Members

        public TValue Lookup(TKey key, Func<TKey, TValue> creator)
        {
            var d = _dict;
            TValue value;
            if (d.TryGetValue(key, out value))
            {
                return value;
            }

            lock(_sync)
            {
                if (d.TryGetValue(key, out value))
                {
                    return value;
                }
                value = creator(key);
                Interlocked.Exchange(ref _dict, new Dictionary<TKey, TValue>(_dict) { { key, value } });
                return value;
            }
        }

        #endregion
    }
}