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

using System;

#if !WinCopies3
using WinCopies.Collections.Generic;

using static WinCopies.Util.Util;
using static WinCopies.Util.ThrowHelper;
#else
using WinCopies.Collections.Generic;

using static WinCopies.ThrowHelper;
#endif

namespace WinCopies.Collections.DotNetFix.Generic
{
    public abstract class ArrayEnumeratorBase<TEnumerable, TItems> :
#if WinCopies3
        EnumeratorInfo<TItems>,
#endif
        ICountableDisposableEnumeratorInfo<TItems> where TEnumerable : System.Collections.Generic.IEnumerable<TItems>
    {
        private int _currentIndex;
        private readonly bool _reverse;
        private Func<bool> _condition;
        private Func<int> _moveNext;
        private Func<int, TItems> _getAt;
        private Func<int> _getLength;

#if !WinCopies3
        private T[] _array;

        protected T[] Array => IsDisposed ? throw GetExceptionForDispose(false) : _array;
#endif

        public int Count => IsDisposed ? throw GetExceptionForDispose(false) : _getLength();

        protected int CurrentIndex => IsDisposed ? throw GetExceptionForDispose(false) : _currentIndex;

        public ArrayEnumeratorBase(in TEnumerable enumerable, in Func<int, TItems> getAt, in Func<int> getLength, in bool reverse = false) : base(enumerable)
        {
            _getAt = getAt ?? throw GetArgumentNullException(nameof(getAt));
            _getLength = getLength ?? throw GetArgumentNullException(nameof(getLength));

#if WinCopies3
            ResetCurrent();
#else
            _Reset();
#endif

            if ((_reverse = reverse))
            {
                _condition = () => _currentIndex >= 0;
                _moveNext = () => _currentIndex--;
            }

            else
            {
                _condition = () => _currentIndex < _getLength();
                _moveNext = () => _currentIndex++;
            }
        }

#if WinCopies3
        protected override TItems CurrentOverride => _getAt(_currentIndex);

        public override bool? IsResetSupported => true;

        protected override bool MoveNextOverride()
        {
            _currentIndex = _moveNext();

            return _condition();
        }
#else
        public bool? IsResetSupported => true;

        public bool MoveNext()
        {
            if (IsDisposed)

                throw GetExceptionForDispose(false);

            if (IsCompleted)

                return false;

            _currentIndex = _moveNext();

            if (_condition())
            {
                IsStarted = true;

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
            _currentIndex = _reverse ? _getLength() : -1;

#if !WinCopies3
            IsStarted = false;
        }
#endif

        protected
#if WinCopies3
        override void DisposeManaged()
        {
#else
        virtual void Dispose(bool disposing)
        {
            if (IsDisposed)

                return;

            if (disposing)
            {
#endif
            _condition = null;
            _moveNext = null;
            _getAt = null;
            _getLength = null;

            Reset();

#if !WinCopies3
                IsDisposed = true;
            }
#endif
        }

#if !WinCopies3
        public bool IsStarted { get; private set; }

        public bool IsCompleted { get; private set; }

        public T Current => IsStarted && !IsDisposed ? _getAt(_currentIndex) : throw GetEnumeratorNotStartedOrDisposedException();

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

    public class ArrayEnumerator<T> : ArrayEnumeratorBase<T[], T>
    {
        public ArrayEnumerator(T[] array, in bool reverse = false) : base(array ?? throw GetArgumentNullException(nameof(array)), i => array[i], () => array.Length, reverse)
        {
            // Left empty.
        }
    }

    public class ListEnumerator<T> : ArrayEnumeratorBase<System.Collections.Generic.IReadOnlyList<T>, T>
    {
        public ListEnumerator(System.Collections.Generic.IReadOnlyList<T> list, in bool reverse = false) : base(list ?? throw GetArgumentNullException(nameof(list)), i => list[i], () => list.Count, reverse)
        {
            // Left empty.
        }
    }
}
