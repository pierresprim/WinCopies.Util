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

namespace WinCopies.Collections.DotNetFix.Generic
{
    public class ArrayEnumerator<T> : Enumerator<T>, ICountableDisposableEnumeratorInfo<T>
    {
        private
#if CS7
            System.Collections.Generic.
#endif
            IReadOnlyList<T> _array;
        private int _currentIndex = -1;
        private readonly bool _reverse;
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
            IReadOnlyList<T> array, in bool reverse = false)
        {
            _array = array ?? throw GetArgumentNullException(nameof(array));

            ResetCurrent();

            if ((_reverse = reverse))
            {
                _condition = () => _currentIndex >= 0;
                _moveNext = () => _currentIndex--;
            }

            else
            {
                _condition = () => _currentIndex < _array.Count;
                _moveNext = () => _currentIndex++;
            }
        }
        protected override T CurrentOverride => _array[_currentIndex];

        public override bool? IsResetSupported => true;

        protected override bool MoveNextOverride()
        {
            _moveNext();

            return _condition();
        }

        protected override void ResetCurrent() => _currentIndex = _reverse ? _array.Count : -1;

        protected override void DisposeManaged()
        {
            _array = null;
            _condition = null;
            _moveNext = null;

            Reset();
        }
    }
}
#endif
