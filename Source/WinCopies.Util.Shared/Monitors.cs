using System;
using System.Collections.Generic;
using System.Text;

using static WinCopies.
#if !WinCopies3
Util.Util;
#else
ThrowHelper;
#endif

namespace WinCopies
#if !WinCopies3
.Util
#endif
{
    public interface IMonitor : System.IDisposable
    {
        bool IsBusy { get; }

        void Enter();
    }

    public sealed class ActionMonitor : IMonitor
    {
        private readonly Action _enter;
        private readonly Action _dispose;
        private readonly Func<bool> _getIsBusy;

        public bool IsBusy => _getIsBusy();

        public ActionMonitor(Action enter, Action dispose, Func<bool> getIsBusy)
        {
            _enter = enter ?? throw GetArgumentNullException(nameof(enter));

            _dispose = dispose ?? throw GetArgumentNullException(nameof(dispose));

            _getIsBusy = getIsBusy ?? throw GetArgumentNullException(nameof(getIsBusy));
        }

        public void Enter() => _enter();

        public void Dispose() => _dispose();
    }

    public sealed class ActionMonitor2 : IMonitor
    {
        private readonly Action _enter;
        private readonly Action _dispose;

        public bool IsBusy { get; private set; }

        public ActionMonitor2(Action enter, Action dispose)
        {
            _enter = () => { IsBusy = true; (enter ?? throw GetArgumentNullException(nameof(enter)))(); };

            _dispose = dispose ?? throw GetArgumentNullException(nameof(dispose));
        }

        public void Enter() => _enter();

        public void Dispose()
        {
            _dispose();

            IsBusy = false;
        }
    }

    public sealed class CountMonitor : IMonitor
    {
        private uint _count = 0;

        public bool IsBusy => _count != 0;

        public void Enter() => _count++;

        public void Dispose() => _count--;
    }
}
