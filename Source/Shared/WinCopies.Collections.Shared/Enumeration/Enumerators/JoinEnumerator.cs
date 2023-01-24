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
using System.Diagnostics;

using WinCopies.Linq;

#if !DEBUG
using static WinCopies.ThrowHelper;
#endif

namespace WinCopies.Collections.Generic
{
    public sealed class JoinSubEnumerator<T> : Enumerator<T, T>
    {
        private System.Collections.Generic.IEnumerator<T> _joinEnumerator;
        private T _firstValue;
        private Func<bool> _moveNext;

        private T _current;

        /// <summary>
        /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
        /// </summary>
        protected override T CurrentOverride => _current;

        public override bool? IsResetSupported => null;

        public JoinSubEnumerator(System.Collections.Generic.IEnumerable<T> subEnumerable, System.Collections.Generic.IEnumerable<T> joinEnumerable) : base(subEnumerable)
        {
            Debug.Assert(subEnumerable != null && joinEnumerable != null);

            _joinEnumerator =
#if !DEBUG
                (
#endif
            joinEnumerable
#if !DEBUG
                ?? throw GetArgumentNullException(nameof(joinEnumerable)))
#endif
            .GetEnumerator();

            InitDelegate();
        }

        private void InitDelegate() => _moveNext = () =>
        {
            if (InnerEnumerator.MoveNext())
            {
                _firstValue = InnerEnumerator.Current;

                _moveNext = () => _MoveNext();

                return _MoveNext();
            }

            else

                return false;
        };

        protected override void ResetCurrent() => _current = default;

        private bool _MoveNext()
        {
            if (_joinEnumerator.MoveNext())
            {
                _current = _joinEnumerator.Current;

                return true;
            }

            _current = _firstValue;

            _firstValue = default;

            _moveNext = () =>
            {
                if (InnerEnumerator.MoveNext())
                {
                    _current = InnerEnumerator.Current;

                    return true;
                }

                return false;
            };

            return true;
        }

        protected override bool MoveNextOverride()
        {
            if (_moveNext())

                return true;

            _current = default;
            _moveNext = null;

            return false;
        }

        protected override void ResetOverride2()
        {
            _joinEnumerator.Reset();
            _firstValue = default;

            InitDelegate();
        }

        protected override void DisposeUnmanaged()
        {
            base.DisposeUnmanaged();

            _joinEnumerator = null;
            _firstValue = default;
            _moveNext = null;
        }
    }

    public sealed class JoinEnumerator<T> : Enumerator<System.Collections.Generic.IEnumerable<T>, T>
    {
        private System.Collections.Generic.IEnumerator<T> _subEnumerator;
        private System.Collections.Generic.IEnumerable<T> _joinEnumerable;
        private Func<System.Collections.Generic.IEnumerator<T>> _updateEnumerator;
        private Func<bool> _moveNext;
        private readonly bool _keepEmptyEnumerables;
        private T _current;

        /// <summary>
        /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
        /// </summary>
        protected override T CurrentOverride => _current;

        public override bool? IsResetSupported => null;

        public JoinEnumerator(System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> enumerable, bool keepEmptyEnumerables, params T[] join) : base(enumerable)
        {
            _joinEnumerable = join;
            _keepEmptyEnumerables = keepEmptyEnumerables;

            InitDelegates();
        }

        private void InitDelegates()
        {
            _updateEnumerator = () =>
            {
                _updateEnumerator = _keepEmptyEnumerables
                    ? () => _joinEnumerable.AppendValues(InnerEnumerator.Current).GetEnumerator()
                    :
#if !CS9
                        (Func<System.Collections.Generic.IEnumerator<T>>)(
#endif
                    () => new JoinSubEnumerator<T>(InnerEnumerator.Current, _joinEnumerable)
#if !CS9
                        )
#endif
                    ;

                return InnerEnumerator.Current.GetEnumerator();
            };

            _moveNext = () =>
            {
                if (_subEnumerator == null && !_MoveNext())

                    return false;

                _moveNext = () => __MoveNext();

                return __MoveNext();
            };
        }

        private bool __MoveNext()
        {
            bool moveNext()
            {
                if (_subEnumerator.MoveNext())
                {
                    _current = _subEnumerator.Current;

                    return true;
                }

                return false;
            }

            do
            {
                if (moveNext())

                    return true;

            } while (_MoveNext());

            return false;
        }

        private bool _MoveNext()
        {
            if (InnerEnumerator.MoveNext())
            {
                _subEnumerator = _updateEnumerator();

                return true;
            }

            _subEnumerator = null;

            return false;
        }

        protected override bool MoveNextOverride() => _moveNext();

        protected override void ResetOverride2()
        {
            base.ResetOverride2();

            _subEnumerator = null;

            InitDelegates();
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();

            _current = default;

            _joinEnumerable = null;
            _updateEnumerator = null;
            _moveNext = null;
            _subEnumerator = null;
        }
    }
}
