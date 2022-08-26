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

#if !WinCopies3
using WinCopies.Util;

using static WinCopies.Util.Util;
#else
using WinCopies;
using WinCopies.Collections.DotNetFix.Generic;
using static WinCopies.ThrowHelper;
#endif

namespace WinCopies.Collections
{
#if WinCopies3
    internal class EmptyCheckEnumeratorHelper
    {
        #region Fields
        private IEnumeratorProvider _enumerator;
        private bool? _hasItems;
        #endregion

        public bool HasItems
        {
            get
            {
                ThrowIfDisposed();

                if (!_hasItems.HasValue)

                    _hasItems = _enumerator.Enumerator.MoveNext();

                return _hasItems.Value;
            }
        }

        public Func<bool> MoveNext { get; private set; }

        public EmptyCheckEnumeratorHelper(in IEnumeratorProvider enumerator)
        {
            _enumerator = enumerator;

            ResetMoveNext();
        }

        private void ResetMoveNext()
        {
            // ResetCurrent();

            void resetMoveNext() => MoveNext = () => false;

            bool enumerate()
            {
                if (_enumerator.Enumerator.MoveNext())

                    return true;

                resetMoveNext();

                return false;
            }

            MoveNext = () =>
            {
                if (_hasItems.HasValue)
                {
                    if (_hasItems.Value)
                    {
                        MoveNext = enumerate;

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
                    MoveNext = enumerate;

                    return enumerate();
                }
            };
        }

        public void Reset()
        {
            _hasItems = null;

            ResetMoveNext();

            _enumerator.Enumerator.Reset();
        }

        private void ThrowIfDisposed()
        {
            if (_enumerator.IsDisposed)

                throw GetExceptionForDispose(false);
        }

        public void DisposeManaged()
        {
            ResetMoveNext();

            _enumerator = null;
        }
    }

    internal interface IEnumeratorProvider : WinCopies.Collections.DotNetFix.IDisposableEnumerator
    {
        System.Collections.IEnumerator Enumerator { get; }
    }
#endif

    public sealed class EmptyCheckEnumerator :
#if !WinCopies3
System.Collections.IEnumerator, WinCopies.Util.DotNetFix.IDisposable
#else
        WinCopies.Collections.EnumeratorInfo, IEnumeratorProvider
#endif
    {
#if !WinCopies3
        #region Fields
        private IEnumerator _enumerator;
        private Func<bool> _moveNext;
        private bool? _hasItems = null;
        #endregion

        #region Properties
        public bool IsDisposed { get; private set; }

        public object Current
        {
            get
            {
                ThrowIfDisposed();

                return _enumerator.Current;
            }
        }
        #endregion

        #region Methods
        private void ThrowIfDisposed()
        {
            if (IsDisposed)

                throw GetExceptionForDispose(false);
        }

        private void ResetMoveNext()
        {
            // ResetCurrent();

            void resetMoveNext()
            {
                _moveNext = () => false;

                // ResetCurrent();
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
        #endregion
#else
        private EmptyCheckEnumeratorHelper _emptyCheckEnumeratorHelper;

        #region Properties

        /// <summary>
        /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
        /// </summary>
        protected override object CurrentOverride => InnerEnumerator.Current;

        public override bool? IsResetSupported => null;

        System.Collections.IEnumerator IEnumeratorProvider.Enumerator => InnerEnumerator;

        #endregion
#endif

        public bool HasItems
        {
            get
            {
                ThrowIfDisposed();

#if !WinCopies3
                if (!_hasItems.HasValue)

                    _hasItems = _enumerator.MoveNext();

                return _hasItems.Value;
#else
                return _emptyCheckEnumeratorHelper.HasItems;
#endif
            }
        }

        public EmptyCheckEnumerator(System.Collections.IEnumerator enumerator)
#if !WinCopies3
        {
            _enumerator = enumerator;

            ResetMoveNext();
        }
#else
            : base(enumerator) => _emptyCheckEnumeratorHelper = new EmptyCheckEnumeratorHelper(this);
#endif

        #region Interface implementations
        #region System.Collections.IEnumerator implementation
#if !WinCopies3
        public bool MoveNext()
        {
            ThrowIfDisposed();

            return _moveNext();
        }
#else
        protected override bool MoveNextOverride() => _emptyCheckEnumeratorHelper.MoveNext();
#endif

#if WinCopies3
        protected override void ResetOverride2()
        {
            base.ResetOverride2();

            _emptyCheckEnumeratorHelper.Reset();
#else
            public void Reset()
        {
            ThrowIfDisposed();
            
            _hasItems = null;

            ResetMoveNext();
#endif
        }
        #endregion

        #region IDisposable implementation
#if !WinCopies3
        public void Dispose()
        {
            if (IsDisposed) return;

            // _enumerator.Dispose();
#else
        protected override void DisposeManaged()
        {
            base.DisposeManaged();

            _emptyCheckEnumeratorHelper.DisposeManaged();

            _emptyCheckEnumeratorHelper = null;
#endif

#if !WinCopies3
            ResetMoveNext();

            _enumerator = null;

            IsDisposed = true;
#endif
        }
        #endregion
        #endregion
    }

#if WinCopies3
    namespace Generic
    {
#endif
        public sealed class EmptyCheckEnumerator<T> :
#if !WinCopies3
            System.Collections.Generic.IEnumerator<T>, WinCopies.Util.DotNetFix.IDisposable
#else
            EnumeratorInfo<T>, IEnumeratorProvider
#endif
        {
#if !WinCopies3
            #region Fields
        private Func<bool> _moveNext;
        private bool? _hasItems = null;
        private Func<T> _current;
            #endregion

            #region Properties
        public bool IsDisposed { get; private set; }

            public T Current
            {
                get
                {
                    ThrowIfDisposed();

                    return _current();
                }
            }

            protected System.Collections.Generic.IEnumerator<T> InnerEnumerator { get; private set; }
            #endregion
#else
            private EmptyCheckEnumeratorHelper _emptyCheckEnumeratorHelper;

            System.Collections.IEnumerator IEnumeratorProvider.Enumerator => InnerEnumerator;

            /// <summary>
            /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
            /// </summary>
            protected override T CurrentOverride => InnerEnumerator.Current;

            public override bool? IsResetSupported => null;
#endif

            public bool HasItems
            {
                get
                {
                    ThrowIfDisposed();

#if !WinCopies3
                    if (!_hasItems.HasValue)

                        _hasItems = InnerEnumerator.MoveNext();

                    return _hasItems.Value;
#else
                    return _emptyCheckEnumeratorHelper.HasItems;
#endif
                }
            }

            public EmptyCheckEnumerator(System.Collections.Generic.IEnumerator<T> enumerator)
#if !WinCopies3
            {
                InnerEnumerator = enumerator;

                ResetMoveNext();
            }
#else
                : base(enumerator) => _emptyCheckEnumeratorHelper = new EmptyCheckEnumeratorHelper(this);
#endif

            public EmptyCheckEnumerator(System.Collections.Generic.IEnumerable<T> enumerable) : this(enumerable.GetEnumerator()) { /* Left empty. */ }

#if !WinCopies3
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
                if (InnerEnumerator.MoveNext())

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
                        _current = () => InnerEnumerator.Current;

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
#endif

            #region Interface implementations
            #region System.Collections.IEnumerator implementation
#if !WinCopies3
        object System.Collections.IEnumerator.Current => Current;

        public bool MoveNext()
        {
            ThrowIfDisposed();

            return _moveNext();
        }
#else
            protected override bool MoveNextOverride() => _emptyCheckEnumeratorHelper.MoveNext();
#endif

#if WinCopies3
            protected override void ResetOverride2()
            {
                base.ResetOverride2();

                _emptyCheckEnumeratorHelper.Reset();
#else
            public void Reset()
            {
                ThrowIfDisposed();

                InnerEnumerator.Reset();

                _hasItems = null;

                ResetMoveNext();
#endif
            }
            #endregion

            #region IDisposable implementation
#if !WinCopies3
            public void Dispose()
            {
                if (IsDisposed) return;

                InnerEnumerator.Dispose();

                ResetMoveNext();

                IsDisposed = true;

                InnerEnumerator = null;
#else
            protected override void DisposeManaged()
            {
                base.DisposeManaged();

                _emptyCheckEnumeratorHelper.DisposeManaged();

                _emptyCheckEnumeratorHelper = null;
#endif
            }
            #endregion
            #endregion
        }
#if WinCopies3
    }
#endif
}
