using System;

namespace Kiwi.Json.Util
{
    public interface IRegistry<in TKey, TValue>
    {
        TValue Lookup(TKey key, Func<TValue> creator);
    }
}