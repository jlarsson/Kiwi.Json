using System;
using System.Collections.Generic;

namespace Kiwi.Fluesent.Utility
{
    internal static class SyntaxForDictionary
    {
        public static TValue GetOrCreate<TKey,TValue>(this IDictionary<TKey,TValue> dictionary, TKey key, Func<TValue> creator)
        {
            TValue value;
            if (!dictionary.TryGetValue(key, out value))
            {
                value = creator();
                dictionary.Add(key, value);
            }
            return value;
        }
    }
}
