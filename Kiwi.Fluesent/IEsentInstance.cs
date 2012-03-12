using System;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public interface IEsentInstance : IDisposable
    {
        Instance Instance { get; }
        IEsentDatabase Database { get; }
        IEsentSession CreateSession(bool attachAndOpenDatabase = true);
    }
}