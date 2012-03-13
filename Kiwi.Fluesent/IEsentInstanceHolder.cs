using System;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public interface IEsentInstanceHolder: IDisposable
    {
        Instance Instance { get; }
    }
}