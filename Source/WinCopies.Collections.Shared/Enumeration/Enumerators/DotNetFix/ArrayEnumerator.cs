/* Copyright © Pierre Sprimont, 2020
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

#if WinCopies2
using WinCopies.Collections.Generic;

using static WinCopies.Util.Util;
using static WinCopies.Util.ThrowHelper;
#else
using WinCopies.Collections.Generic;

using static WinCopies.ThrowHelper;
#endif

namespace WinCopies.Collections.DotNetFix.Generic
{
    public class ArrayEnumerator<T> :
#if WinCopies3
        Enumerator<T>,
#endif
        ICountableDisposableEnumeratorInfo<T>
    {
        private T[] _array;
        private int _currentIndex =
#if WinCopies3
            -1
#else
            0
#endif
            ;

        protected T[] Array => IsDisposed ? throw GetExceptionForDispose(false) : _array;

        public int Count => IsDisposed ? throw GetExceptionForDispose(false) : _array.Length;

        protected int CurrentIndex => IsDisposed ? throw GetExceptionForDispose(false) : _currentIndex;

        public ArrayEnumerator(T[] array) => _array = array ?? throw GetArgumentNullException(nameof(array));

#if WinCopies3
        protected override T CurrentOverride => _array[_currentIndex];

        public override bool? IsResetSupported => true;

        protected override bool MoveNextOverride() => ++_currentIndex < _array.Length;
#else
        public bool? IsResetSupported => true;

        public bool MoveNext()
        {
            if (IsDisposed)

                throw GetExceptionForDispose(false);

            if (IsCompleted)

                return false;

            if (_currentIndex < _array.Length)
            {
                IsStarted = true;

                _current = _array[_currentIndex++];

                return true;
            }

            _Reset();

            IsCompleted = true;

            return false;
        }
#endif

#if WinCopies3
        protected override void ResetCurrent() =>
#else
        private void _Reset()
        {
#endif
            _currentIndex = 0;

#if !WinCopies3
            _current = default;

            IsStarted = false;
        }
#endif

#if WinCopies3
        protected override void DisposeManaged()
        {
#else
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)

                return;

            if (disposing)
            {
#endif
                _array = null;

                Reset();

#if !WinCopies3
                IsDisposed = true;
            }
#endif
        }

#if !WinCopies3
        private T _current;

        public bool IsStarted { get; private set; }

        public bool IsCompleted { get; private set; }

        public T Current => IsStarted && !IsDisposed ? _current : throw GetEnumeratorNotStartedOrDisposedException();

        object System.Collections.IEnumerator.Current => Current;

        public bool IsDisposed { get; private set; }

        public void Dispose() => Dispose(true);

        public void Reset()
        {
            if (IsDisposed)

                throw GetExceptionForDispose(false);

            _Reset();

            IsCompleted = false;
        }
#endif
    }
}
