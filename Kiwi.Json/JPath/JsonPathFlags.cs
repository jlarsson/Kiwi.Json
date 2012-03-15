using System;

namespace Kiwi.Json.JPath
{
    [Flags]
    public enum JsonPathFlags
    {
        None = 0,
        HasArrayIndex = 0x01,
        HasArraySlice = 0x02,

        HasWildcardIndex = 0x10,
        HasWildCardMember = 0x20,
        HasWilcard = HasWildcardIndex,
    }
}