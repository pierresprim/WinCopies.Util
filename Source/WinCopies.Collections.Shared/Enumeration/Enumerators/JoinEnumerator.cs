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

#if !WinCopies3
using System.Collections.Generic;

using WinCopies.Util;

using static WinCopies.Util.Util;
#else
using WinCopies.Linq;

#if !DEBUG
using static WinCopies.ThrowHelper;
#endif
#endif

namespace WinCopies.Collections
{
#if WinCopies3
    namespace Generic
    {
#endif
        public sealed class JoinSubEnumerator<T> :
#if WinCopies3
        WinCopies.Collections.Generic.
#endif
        Enumerator<T, T>
        {
            private System.Collections.Generic.IEnumerator<T> _joinEnumerator;
            private T _firstValue;
            private Func<bool> _moveNext;

#if !WinCopies3
        private bool _completed = false;
#else
            private T _current;

            /// <summary>
            /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
            /// </summary>
            protected override T CurrentOverride => _current;

            public override bool? IsResetSupported => null;
#endif

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

#if WinCopies3
            protected override void ResetCurrent() => _current = default;
#endif

            private bool _MoveNext()
            {
                if (_joinEnumerator.MoveNext())
                {
#if !WinCopies3
Current
#else
                    _current
#endif
                    = _joinEnumerator.Current;

                    return true;
                }

#if !WinCopies3
Current
#else
                _current
#endif
                = _firstValue;

                _firstValue = default;

                _moveNext = () =>
                {
                    if (InnerEnumerator.MoveNext())
                    {
#if !WinCopies3
Current
#else
                    _current
#endif
                    = InnerEnumerator.Current;

                        return true;
                    }

                    return false;
                };

                return true;
            }

            protected override bool MoveNextOverride()
            {
#if !WinCopies3
            if (_completed) return false;
#endif

                if (_moveNext()) return true;

#if !WinCopies3
Current
#else
                _current
#endif
                = default;

                _moveNext = null;

#if !WinCopies3
            _completed = true;
#endif

                return false;
            }

            protected override void
#if WinCopies3
                ResetOverride2
#else
                ResetOverride
#endif
                ()
            {
#if !WinCopies3
                base.ResetOverride();
#endif

                _joinEnumerator.Reset();

                _firstValue = default;

#if !WinCopies3
            _completed = false;
#endif

                InitDelegate();
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                _joinEnumerator = null;

                _firstValue = default;

                _moveNext = null;
            }
        }

        public sealed class JoinEnumerator<T> :
#if WinCopies3
        WinCopies.Collections.Generic.
#endif
        Enumerator<System.Collections.Generic.IEnumerable<T>, T>
        {
            private System.Collections.Generic.IEnumerator<T> _subEnumerator;
            private System.Collections.Generic.IEnumerable<T> _joinEnumerable;
            private Action _updateEnumerator;
            private Func<bool> _moveNext;
            private readonly bool _keepEmptyEnumerables;

#if !WinCopies3
        private bool _completed = false;
#else
            private T _current;

            /// <summary>
            /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
            /// </summary>
            protected override T CurrentOverride => _current;

            public override bool? IsResetSupported => null;
#endif

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
                    _subEnumerator = InnerEnumerator.Current.GetEnumerator();

                    if (_keepEmptyEnumerables)

                        _updateEnumerator = () => _subEnumerator = _joinEnumerable.AppendValues(InnerEnumerator.Current).GetEnumerator();

                    else

                        _updateEnumerator = () => _subEnumerator = new JoinSubEnumerator<T>(InnerEnumerator.Current, _joinEnumerable);
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
#if !WinCopies3
Current 
#else
                        _current
#endif
                        = _subEnumerator.Current;

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
                    _updateEnumerator();

                    return true;
                }

                _subEnumerator = null;

                return false;
            }

            protected override bool MoveNextOverride() =>
#if !WinCopies3
            _completed ? false : 
#endif
            _moveNext();

            protected override void ResetOverride()
            {
                base.ResetOverride();

                _subEnumerator = null;

                InitDelegates();
            }

            protected override void
#if !WinCopies3
Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
            Current
#else
            DisposeManaged()
            {
                base.DisposeManaged();

                _current
#endif
                = default;

                _joinEnumerable = null;

                _updateEnumerator = null;

                _moveNext = null;

                _subEnumerator = null;
#if !WinCopies3
        }
#endif
            }
        }
#if WinCopies3
    }
#endif
}
