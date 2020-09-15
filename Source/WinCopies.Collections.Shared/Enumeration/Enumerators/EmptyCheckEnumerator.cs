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
using System.Collections;
using System.Collections.Generic;

#if WinCopies2
using WinCopies.Util;

using static WinCopies.Util.Util;
#else
using WinCopies;

using static WinCopies.ThrowHelper;
#endif

namespace WinCopies.Collections
{
    public sealed class EmptyCheckEnumerator : System.Collections.IEnumerator, WinCopies
#if WinCopies2
.Util
#endif
        .DotNetFix.IDisposable
    {
        #region Fields
        private readonly System.Collections.IEnumerator _enumerator;
        private Func<bool> _moveNext;
        private bool? _hasItems = null;
        private Func _current;
        #endregion

        #region Properties
        public bool IsDisposed { get; private set; }

        public bool HasItems
        {
            get
            {
                ThrowIfDisposed();

                if (!_hasItems.HasValue)

                    _hasItems = _enumerator.MoveNext();

                return _hasItems.Value;
            }
        }

        public object Current
        {
            get
            {
                ThrowIfDisposed();

                return _current();
            }
        }
        #endregion

        public EmptyCheckEnumerator(System.Collections.IEnumerator enumerator)
        {
            _enumerator = enumerator;

            ResetMoveNext();
        }

        #region Methods
        private void ResetCurrent() => _current = () => throw new InvalidOperationException("The enumeration has not been started or has completed.");

        private void ResetMoveNext()
        {
            ResetCurrent();

            void resetMoveNext()
            {
                _moveNext = () => false;

                ResetCurrent();
            }

            bool enumerate()
            {
                if (_enumerator.MoveNext())

                    return true;

                resetMoveNext();

                return false;
            }

            _moveNext = () =>
            {
                if (_hasItems.HasValue)
                {
                    if (_hasItems.Value)
                    {
                        _current = () => _enumerator.Current;

                        _moveNext = enumerate;

                        return true;
                    }

                    else
                    {
                        resetMoveNext();

                        return false;
                    }
                }

                else
                {
                    _moveNext = enumerate;

                    return enumerate();
                }
            };
        }

        private void ThrowIfDisposed()
        {
            if (IsDisposed)

                throw GetExceptionForDispose(false);
        }
        #endregion

        #region Interface implementations
        #region System.Collections.IEnumerator implementation
        public bool MoveNext()
        {
            ThrowIfDisposed();

            return _moveNext();
        }

        public void Reset()
        {
            ThrowIfDisposed();

            _enumerator.Reset();

            _hasItems = null;

            ResetMoveNext();
        }
        #endregion

        #region IDisposable implementation
        public void Dispose()
        {
            if (IsDisposed) return;

            // _enumerator.Dispose();

            ResetMoveNext();

            IsDisposed = true;
        }
        #endregion
        #endregion
    }

#if !WinCopies2
namespace Generic
{
#endif
    public sealed class EmptyCheckEnumerator<T> : System.Collections.Generic.IEnumerator<T>, WinCopies
#if WinCopies2
.Util
#endif
        .DotNetFix.IDisposable
    {
#region Fields
        private System.Collections.Generic.IEnumerator<T> _enumerator;
        private Func<bool> _moveNext;
        private bool? _hasItems = null;
        private Func<T> _current;
#endregion

#region Properties
        public bool IsDisposed { get; private set; }

        public bool HasItems
        {
            get
            {
                ThrowIfDisposed();

                if (!_hasItems.HasValue)

                    _hasItems = _enumerator.MoveNext();

                return _hasItems.Value;
            }
        }

        public T Current
        {
            get
            {
                ThrowIfDisposed();

                return _current();
            }
        }
#endregion

        public EmptyCheckEnumerator(System.Collections.Generic.IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;

            ResetMoveNext();
        }

#region Methods
        private void ResetCurrent() => _current = () => throw new InvalidOperationException("The enumeration has not been started or has completed.");

        private void ResetMoveNext()
        {
            ResetCurrent();

            void resetMoveNext()
            {
                _moveNext = () => false;

                ResetCurrent();
            }

            bool enumerate()
            {
                if (_enumerator.MoveNext())

                    return true;

                resetMoveNext();

                return false;
            }

            _moveNext = () =>
            {
                if (_hasItems.HasValue)
                {
                    if (_hasItems.Value)
                    {
                        _current = () => _enumerator.Current;

                        _moveNext = enumerate;

                        return true;
                    }

                    else
                    {
                        resetMoveNext();

                        return false;
                    }
                }

                else
                {
                    _moveNext = enumerate;

                    return enumerate();
                }
            };
        }

        private void ThrowIfDisposed()
        {
            if (IsDisposed)

                throw GetExceptionForDispose(false);
        }
#endregion

#region Interface implementations
#region System.Collections.IEnumerator implementation
        object System.Collections.IEnumerator.Current => Current;

        public bool MoveNext()
        {
            ThrowIfDisposed();

            return _moveNext();
        }

        public void Reset()
        {
            ThrowIfDisposed();

            _enumerator.Reset();

            _hasItems = null;

            ResetMoveNext();
        }
#endregion

#region IDisposable implementation
        public void Dispose()
        {
            if (IsDisposed) return;

            _enumerator.Dispose();

            ResetMoveNext();

            IsDisposed = true;
        }
#endregion
#endregion
    }
#if !WinCopies2
}
#endif
}
