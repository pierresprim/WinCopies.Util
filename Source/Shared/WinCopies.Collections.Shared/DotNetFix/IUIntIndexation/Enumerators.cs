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

using static WinCopies
#if !WinCopies3
    .Util.Util;

using System.Collections;
using System.Collections.Generic;
#else
    .ThrowHelper;

using WinCopies.Collections.DotNetFix.Generic;
#endif

namespace WinCopies.Collections.DotNetFix
{
    // todo: check if the given collection implements the WinCopies.DotNetFix.IDisposable (or WinCopies.IDisposable) interface and, if yes, check the given collection is not disposed (or disposing) in the Current property and in the MoveNext method.

    public abstract class UIntIndexedListEnumeratorBase : WinCopies.
#if !WinCopies3
        Util.
#endif
        DotNetFix.IDisposable
    {
        private uint? index = null;

        protected internal uint? Index { get { ThrowIfDisposed(this); return index; } set { ThrowIfDisposed(this); index = value; } }

        private Func<bool> _moveNextMethod;

        protected internal Func<bool> MoveNextMethod { get { ThrowIfDisposed(this); return _moveNextMethod; } set { ThrowIfDisposed(this); _moveNextMethod = value; } }

        #region IDisposable Support
        public bool IsDisposed { get; private set; } = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                Reset();

                IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        ~UIntIndexedListEnumeratorBase() => Dispose(false);

        public virtual bool MoveNext()
        {
            ThrowIfDisposed(this);

            return MoveNextMethod();
        }

        public virtual void Reset() => Index = null;

        #endregion
    }

    public sealed class UIntIndexedListEnumerator : UIntIndexedListEnumeratorBase, System.Collections.IEnumerator
    {
        private IReadOnlyUIntIndexedList innerList;

        internal IReadOnlyUIntIndexedList InnerList { get { ThrowIfDisposed(this); return innerList; } private set { ThrowIfDisposed(this); innerList = value; } }

        private Func<bool> moveNextToReset;

        public static Func<UIntIndexedListEnumerator, bool> DefaultMoveNextMethod => (UIntIndexedListEnumerator e) =>
        {
            if (e.InnerList.Count > 0)
            {
                e.Index = 0;

                e.MoveNextMethod = () =>
                {
                    if (e.Index < e.InnerList.Count - 1)
                    {
                        e.Index++;

                        return true;
                    }

                    else return false;
                };

                return true;
            }

            else return false;
        };

        public object Current
        {
            get
            {
                Debug.Assert(Index.HasValue, "_index does not have value.");

                return InnerList[Index.Value];
            }
        }

        public UIntIndexedListEnumerator(IReadOnlyUIntIndexedList uintIndexedList)
        {
            MoveNextMethod = moveNextToReset = () => DefaultMoveNextMethod(this);

            innerList = uintIndexedList;
        }

        public UIntIndexedListEnumerator(IReadOnlyUIntIndexedList uintIndexedList, Func<bool> moveNextMethod)
        {
            MoveNextMethod = moveNextMethod;

            innerList = uintIndexedList;
        }

#if WinCopies2
        public UIntIndexedListEnumerator(IUIntIndexedList uintIndexedList) : this((IReadOnlyUIntIndexedList)uintIndexedList)
        {
            // Left empty.
        }

        public UIntIndexedListEnumerator(IUIntIndexedList uintIndexedList, Func<bool> moveNextMethod) : this((IReadOnlyUIntIndexedList)uintIndexedList, moveNextMethod)
        {
            // Left empty.
        }
#endif

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                InnerList = null;

                moveNextToReset = null;

                base.Dispose(disposing);
            }
        }

        public override void Reset()
        {
            base.Reset();

            MoveNextMethod = moveNextToReset;
        }
    }

#if WinCopies3
    namespace Generic
    {
#endif
    public sealed class UIntIndexedListEnumerator<T> : UIntIndexedListEnumeratorBase, System.Collections.Generic.IEnumerator<T>
    {
        private IReadOnlyUIntIndexedList<T> innerList;

        internal IReadOnlyUIntIndexedList<T> InnerList { get { ThrowIfDisposed(this); return innerList; } private set { ThrowIfDisposed(this); innerList = value; } }

        private Func<bool> moveNextMethodToReset;

        public static Func<UIntIndexedListEnumerator<T>, bool> DefaultMoveNextMethod => (UIntIndexedListEnumerator<T> e) =>
        {
            if (e.InnerList.Count > 0)
            {
                e.Index = 0;

                e.MoveNextMethod = () =>
                {
                    if (e.Index < e.InnerList.Count - 1)
                    {
                        e.Index++;

                        return true;
                    }

                    else return false;
                };

                return true;
            }

            else return false;
        };

        public T Current
        {
            get
            {
                Debug.Assert(Index.HasValue, "_index does not have value.");

                return InnerList[Index.Value];
            }
        }

        object System.Collections.IEnumerator.Current => Current;

        public UIntIndexedListEnumerator(IUIntIndexedList<T> uintIndexedList)
        {
            MoveNextMethod = moveNextMethodToReset = () => DefaultMoveNextMethod(this);

            innerList = uintIndexedList;
        }

        public UIntIndexedListEnumerator(IUIntIndexedList<T> uintIndexedList, Func<bool> moveNextMethod)
        {
            MoveNextMethod = moveNextMethod;

            innerList = uintIndexedList;
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                InnerList = null;

                moveNextMethodToReset = null;

                base.Dispose(disposing);
            }
        }

        public override void Reset()
        {
            base.Reset();

            MoveNextMethod = moveNextMethodToReset;
        }
    }
#if WinCopies3
    }
#endif
}
