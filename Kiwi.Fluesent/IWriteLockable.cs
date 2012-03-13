using System;

namespace Kiwi.Fluesent
{
    public interface IWriteLockable: IDisposable
    {
        IDisposable CreateWriteLock();
    }
}