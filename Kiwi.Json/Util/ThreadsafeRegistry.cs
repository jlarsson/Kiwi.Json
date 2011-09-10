using System;
using System.Collections.Generic;
using System.Threading;

namespace Kiwi.Json.Util
{
    public class ThreadsafeRegistry<TKey, TValue> : IRegistry<TKey, TValue> where TKey : class
    {
        private Dictionary<TKey, TValue> _dict = new Dictionary<TKey, TValue>();

        #region IRegistry<TKey,TValue> Members

        public TValue Lookup(TKey key, Func<TValue> creator)
        {
            var d = _dict;
            TValue value;
            if (d.TryGetValue(key, out value))
            {
                return value;
            }
            value = creator();
            Interlocked.Exchange(ref _dict, new Dictionary<TKey, TValue>(_dict) {{key, value}});
            return value;
        }

        #endregion
    }
}