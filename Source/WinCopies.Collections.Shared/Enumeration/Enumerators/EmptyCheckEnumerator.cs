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
using WinCopies.Collections.DotNetFix.Generic;
using static WinCopies.ThrowHelper;
#endif

namespace WinCopies.Collections
{
#if !WinCopies2
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
#if WinCopies2
System.Collections.IEnumerator, WinCopies.Util.DotNetFix.IDisposable
#else
        WinCopies.Collections.Enumerator, IEnumeratorProvider
#endif
    {
        protected System.Collections.IEnumerator InnerEnumerator { get; private set; }

#if WinCopies2
        #region Fields
        private Func<bool> _moveNext;
        private bool? _hasItems;
        #endregion

        #region Properties
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

        public void ThrowIfDisposed()
        {
            if (_enumerator.IsDisposed)

                throw GetExceptionForDispose(false);
        }

        public bool IsDisposed { get; private set; }

        public object Current
        {
            get
            {
                ThrowIfDisposed();

                return InnerEnumerator.Current;
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

        System.Collections.IEnumerator IEnumeratorProvider.Enumerator => InnerEnumerator;

        protected override object CurrentOverride => InnerEnumerator.Current;

        public override bool? IsResetSupported => null;
#endif

        public EmptyCheckEnumerator(System.Collections.IEnumerator enumerator)
        {
            InnerEnumerator = enumerator;

            _emptyCheckEnumeratorHelper = new EmptyCheckEnumeratorHelper(this);

#if WinCopies2
            ResetMoveNext();
#endif
        }

        #region Interface implementations
        #region System.Collections.IEnumerator implementation
#if WinCopies2
        public bool MoveNext()
        {
            ThrowIfDisposed();

            return
#else
        protected override bool MoveNextOverride() =>
#endif
           _emptyCheckEnumeratorHelper.MoveNext();
#if WinCopies2
        }

        public void Reset()
        {
            ThrowIfDisposed();
            
            _hasItems = null;

            ResetMoveNext();
#else
        protected override void ResetOverride()
        {
            base.ResetOverride();

            _emptyCheckEnumeratorHelper.Reset();
#endif
        }
        #endregion

        #region IDisposable implementation
#if WinCopies2
        public void Dispose()
        {
            if (IsDisposed) return;

            // _enumerator.Dispose();
#else
        protected override void DisposeManaged()
        {
            base.DisposeManaged();
#endif
            _emptyCheckEnumeratorHelper.DisposeManaged();

            _emptyCheckEnumeratorHelper = null;

            InnerEnumerator = null;

#if WinCopies2
            IsDisposed = true;
#endif
        }
        #endregion
        #endregion
    }

#if !WinCopies2
    namespace Generic
    {
#endif
        public sealed class EmptyCheckEnumerator<T> :
#if WinCopies2
            System.Collections.Generic.IEnumerator<T>, WinCopies.Util.DotNetFix.IDisposable
#else
            Enumerator<T>, IEnumeratorProvider
#endif
        {
#if WinCopies2
            #region Fields
            private Func<bool> _moveNext;
            private bool? _hasItems = null;
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
#else
            private EmptyCheckEnumeratorHelper _emptyCheckEnumeratorHelper;

            System.Collections.IEnumerator IEnumeratorProvider.Enumerator => InnerEnumerator;

            protected override T CurrentOverride => InnerEnumerator.Current;

            public override bool? IsResetSupported => null;
#endif

            protected System.Collections.Generic.IEnumerator<T> InnerEnumerator { get; private set; }

            public EmptyCheckEnumerator(System.Collections.Generic.IEnumerator<T> enumerator)
            {
                InnerEnumerator = enumerator;

                _emptyCheckEnumeratorHelper = new EmptyCheckEnumeratorHelper(this);

#if WinCopies2
                ResetMoveNext();
#endif
            }

            #region Interface implementations
            #region System.Collections.IEnumerator implementation
#if WinCopies2
            object System.Collections.IEnumerator.Current => Current;

            public bool MoveNext()
            {
                ThrowIfDisposed();

                return
#else
            protected override bool MoveNextOverride() => _emptyCheckEnumeratorHelper.MoveNext();
#endif

#if WinCopies2
            public void Reset()
            {
                ThrowIfDisposed();

                _enumerator.Reset();

                _hasItems = null;

                ResetMoveNext();
#else
            protected override void ResetOverride()
            {
                base.ResetOverride();

                _emptyCheckEnumeratorHelper.Reset();
#endif
            }
            #endregion

            #region IDisposable implementation
#if WinCopies2
            public void Dispose()
            {
                if (IsDisposed) return;

                _enumerator.Dispose();

                ResetMoveNext();

                IsDisposed = true;
#else
            protected override void DisposeManaged()
            {
                base.DisposeManaged();

                _emptyCheckEnumeratorHelper.DisposeManaged();

                _emptyCheckEnumeratorHelper = null;

                InnerEnumerator.Dispose();

                InnerEnumerator = null;
#endif
            }
            #endregion
            #endregion
        }
#if !WinCopies2
    }
#endif
}
