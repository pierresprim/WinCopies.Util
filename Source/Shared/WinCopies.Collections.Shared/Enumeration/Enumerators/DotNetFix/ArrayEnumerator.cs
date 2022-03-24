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

#if WinCopies3
using System;

using WinCopies.Collections.Generic;

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections.DotNetFix
{
    public class ArrayEnumerator : Enumerator<object
#if CS8
        ?
#endif
        >
    {
        protected Array Array { get; }

        public int CurrentIndex { get; private set; }

        public override bool? IsResetSupported => true;

        protected override object
#if CS8
            ?
#endif
            CurrentOverride => Array.GetValue(CurrentIndex);

        public ArrayEnumerator(in Array array)
        {
            Array = array ?? throw GetArgumentNullException(nameof(array));

            ResetIndex();
        }

        protected void ResetIndex() => CurrentIndex = -1;

        protected override void ResetCurrent()
        {
            base.ResetCurrent();

            ResetIndex();
        }

        protected override bool MoveNextOverride() => ++CurrentIndex < Array.Length;

        protected override void ResetOverride2() => ResetIndex();
    }

    namespace Generic
    {
        public class ArrayEnumerator<T> : Enumerator<T>, ICountableDisposableEnumeratorInfo<T>
        {
            private
#if CS7
            System.Collections.Generic.
#endif
            IReadOnlyList<T> _array;
            private int _currentIndex;
            private readonly int _startIndex;
            private Func<bool> _condition;
            private Action _moveNext;

            protected
#if CS7
            System.Collections.Generic.
#endif
            IReadOnlyList<T> Array => IsDisposed ? throw GetExceptionForDispose(false) : _array;

            public int Count => IsDisposed ? throw GetExceptionForDispose(false) : _array.Count;

            protected int CurrentIndex => IsDisposed ? throw GetExceptionForDispose(false) : _currentIndex;

            public ArrayEnumerator(in
#if CS7
            System.Collections.Generic.
#endif
            IReadOnlyList<T> array, in bool reverse = false, in int? startIndex = null)
            {
                _array = array ?? throw GetArgumentNullException(nameof(array));

                if (startIndex.HasValue && (startIndex.Value < 0 || startIndex.Value >= array.Count))

                    throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex, $"The given index is less than zero or greater than or equal to {nameof(array.Count)}.");

                if (reverse)
                {
                    _startIndex = startIndex.HasValue ? startIndex.Value + 1 : _array.Count;
                    _condition = () => _currentIndex >= 0;
                    _moveNext = () => _currentIndex--;
                }

                else
                {
                    _startIndex = startIndex.HasValue ? startIndex.Value - 1 : -1;
                    _condition = () => _currentIndex < _array.Count;
                    _moveNext = () => _currentIndex++;
                }

                ResetCurrent();
            }

            protected override T CurrentOverride => _array[_currentIndex];

            public override bool? IsResetSupported => true;

            protected override bool MoveNextOverride()
            {
                _moveNext();

                return _condition();
            }

            protected override void ResetCurrent() => _currentIndex = _startIndex;

            protected override void ResetOverride2() { /* Left empty. */ }

            protected override void DisposeManaged()
            {
                _array = null;
                _condition = null;
                _moveNext = null;

                Reset();
            }
        }
    }
}
#endif
