using System;

namespace Kiwi.Json.Util
{
    public interface IRegistry<TKey, TValue>
    {
        TValue Lookup(TKey key, Func<TKey,TValue> creator);
    }
}