/* Copyright © Pierre Sprimont, 2021
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

namespace WinCopies.Collections.Generic
{
    public class MultipleEnumerableEnumerator<T> : Enumerator<System.Collections.Generic.IEnumerable<T>, T>
    {
        private System.Collections.Generic.IEnumerator<T> _currentEnumerator;
        private Func<bool> _moveNext;

        public override bool? IsResetSupported => null;

        protected override T CurrentOverride => _currentEnumerator.Current;

        public MultipleEnumerableEnumerator(System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> enumerable) : base(enumerable) => ResetMoveNext();

        protected override bool MoveNextOverride() => _moveNext();

        protected override void ResetCurrent()
        {
            base.ResetCurrent();

            _currentEnumerator = null;
        }

        private bool _MoveNext()
        {
            while (InnerEnumerator.MoveNext())

                if ((_currentEnumerator = InnerEnumerator.Current.GetEnumerator()).MoveNext())
                {
                    _moveNext = () => _currentEnumerator.MoveNext() || _MoveNext();

                    return true;
                }

            _moveNext = null;

            return false;
        }

        protected virtual void ResetMoveNext() => _moveNext = _MoveNext;

        protected override void ResetOverride2() => ResetMoveNext();

        protected override void DisposeManaged()
        {
            base.DisposeManaged();

            _moveNext = null;
        }
    }
}

#endif
