using System;

namespace Kiwi.Fluesent
{
    public interface IEsentInstance : IDisposable
    {
        IEsentDatabase Database { get; }
        IEsentSession CreateSession(bool attachAndOpenDatabase = true);
    }
}