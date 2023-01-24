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

using WinCopies;
using WinCopies.Collections.DotNetFix.Generic;

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections
{
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

    public sealed class EmptyCheckEnumerator : WinCopies.Collections.EnumeratorInfo, IEnumeratorProvider
    {
        private EmptyCheckEnumeratorHelper _emptyCheckEnumeratorHelper;

        #region Properties

        /// <summary>
        /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
        /// </summary>
        protected override object CurrentOverride => InnerEnumerator.Current;

        public override bool? IsResetSupported => null;

        System.Collections.IEnumerator IEnumeratorProvider.Enumerator => InnerEnumerator;

        #endregion

        public bool HasItems
        {
            get
            {
                ThrowIfDisposed();

                return _emptyCheckEnumeratorHelper.HasItems;
            }
        }

        public EmptyCheckEnumerator(System.Collections.IEnumerator enumerator) : base(enumerator) => _emptyCheckEnumeratorHelper = new EmptyCheckEnumeratorHelper(this);

        #region Interface implementations
        #region System.Collections.IEnumerator implementation
        protected override bool MoveNextOverride() => _emptyCheckEnumeratorHelper.MoveNext();

        protected override void ResetOverride2()
        {
            base.ResetOverride2();

            _emptyCheckEnumeratorHelper.Reset();
        }
        #endregion

        #region IDisposable implementation
        protected override void DisposeManaged()
        {
            base.DisposeManaged();

            _emptyCheckEnumeratorHelper.DisposeManaged();

            _emptyCheckEnumeratorHelper = null;
        }
        #endregion
        #endregion
    }

    namespace Generic
    {
        public sealed class EmptyCheckEnumerator<T> : EnumeratorInfo<T>, IEnumeratorProvider
        {
            private EmptyCheckEnumeratorHelper _emptyCheckEnumeratorHelper;

            System.Collections.IEnumerator IEnumeratorProvider.Enumerator => InnerEnumerator;

            /// <summary>
            /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
            /// </summary>
            protected override T CurrentOverride => InnerEnumerator.Current;

            public override bool? IsResetSupported => null;

            public bool HasItems
            {
                get
                {
                    ThrowIfDisposed();

                    return _emptyCheckEnumeratorHelper.HasItems;
                }
            }

            public EmptyCheckEnumerator(System.Collections.Generic.IEnumerator<T> enumerator) : base(enumerator) => _emptyCheckEnumeratorHelper = new EmptyCheckEnumeratorHelper(this);

            public EmptyCheckEnumerator(System.Collections.Generic.IEnumerable<T> enumerable) : this(enumerable.GetEnumerator()) { /* Left empty. */ }

            #region Interface implementations
            #region System.Collections.IEnumerator implementation
            protected override bool MoveNextOverride() => _emptyCheckEnumeratorHelper.MoveNext();

            protected override void ResetOverride2()
            {
                base.ResetOverride2();

                _emptyCheckEnumeratorHelper.Reset();
            }
            #endregion

            #region IDisposable implementation
            protected override void DisposeManaged()
            {
                base.DisposeManaged();

                _emptyCheckEnumeratorHelper.DisposeManaged();

                _emptyCheckEnumeratorHelper = null;
            }
            #endregion
            #endregion
        }
    }
}
