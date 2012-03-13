using System;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public interface IEsentInstanceHolder: IWriteLockable, IDisposable
    {
        Instance Instance { get; }
    }
}