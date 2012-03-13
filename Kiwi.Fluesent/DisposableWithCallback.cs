using System;
using System.Diagnostics;

namespace Kiwi.Fluesent
{
    internal class DisposableWithCallback: IDisposable
    {
        private readonly Action _disposing;
        private bool _isDisposed;

        public DisposableWithCallback(Action disposing)
        {
            _disposing = disposing;
        }

        public void Dispose()
        {
            Debug.Assert(!_isDisposed);
            if (!_isDisposed)
            {
                _isDisposed = true;
                _disposing();
            }
        }
    }
}