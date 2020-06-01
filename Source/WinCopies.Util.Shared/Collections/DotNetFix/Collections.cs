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
using System.Diagnostics;
using static WinCopies.Util.Util;

namespace WinCopies.Collections.DotNetFix
{
    public interface ICountableEnumerable : IEnumerable
    {
        int Count { get; }
    }

    public interface ICountableEnumerable<out T> : IEnumerable<T>, ICountableEnumerable { }

    public interface IUIntIndexedCollection : IEnumerable
    {
        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        uint Count { get; }

        object SyncRoot { get; }

        bool IsSynchronized { get; }

        /// <summary>
        /// Copies the items of this collection to a given array starting at a given index.
        /// </summary>
        /// <param name="array">The array to copy the items.</param>
        /// <param name="index">The index in the array from which to start to copy.</param>
        void CopyTo(in Array array, int index);
    }

    public interface IUIntIndexedList : IReadOnlyUIntIndexedList, IEnumerable
    {
        /// <summary>
        /// Gets the item at a given index in the list.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>The item at <paramref name="index"/>.</returns>
        new object this[uint index] { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the current list is fixed size.
        /// </summary>
        bool IsFixedSize { get; }

        /// <summary>
        /// Adds a value to the list.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <returns>The index of the new value in the list.</returns>
        uint Add(in object value);

        /// <summary>
        /// Removes all items from the list.
        /// </summary>
        void Clear();

        /// <summary>
        /// Inserts a value to the list.
        /// </summary>
        /// <param name="index">The index at which to add the value.</param>
        /// <param name="value">The value to add.</param>
        void Insert(in uint index, in object value);

        /// <summary>
        /// Removes a given value from the list.
        /// </summary>
        /// <param name="value">The value to remove.</param>
        void Remove(in object value);

        /// <summary>
        /// Removes a value from the list at a given index.
        /// </summary>
        /// <param name="index">The index of the value to remove.</param>
        void RemoveAt(in uint index);
    }

    public interface IReadOnlyUIntIndexedList : IUIntIndexedCollection, IEnumerable
    {
        /// <summary>
        /// Gets or sets an item at a given index in the list.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>The item at <paramref name="index"/>.</returns>
        object this[uint index] { get; }

        /// <summary>
        /// Checks if the list contains a given value.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/> if the list contains <paramref name="value"/>, otherwise <see langword="false"/>.</returns>
        bool Contains(in object value);

        /// <summary>
        /// Returns the index of a given value in the list.
        /// </summary>
        /// <param name="value">The value for which return the index.</param>
        /// <returns>The index of <paramref name="value"/> if it was found, or <see langword="null"/> otherwise.</returns>
        uint? IndexOf(in object value);
    }

    public interface IUIntIndexedCollection<T> : IReadOnlyUIntIndexedCollection<T>, IEnumerable<T>, IEnumerable
    {
        void Add(in T item);

        void Clear();

        bool Contains(in T item);

        void CopyTo(in T[] array, uint arrayIndex);

        bool Remove(in T item);
    }

    public interface IReadOnlyUIntIndexedCollection<out T> : IEnumerable<T>, IEnumerable
    {
        uint Count { get; }
    }

    public interface IUIntIndexedList<T> : IUIntIndexedCollection<T>, IReadOnlyUIntIndexedList<T>, IEnumerable<T>, IEnumerable
    {
        new T this[uint index] { get; set; }

        uint? IndexOf(in T item);

        void Insert(in uint index, in T item);

        void RemoveAt(in uint index);
    }

    public interface IReadOnlyUIntIndexedList<out T> : IReadOnlyUIntIndexedCollection<T>, IEnumerable<T>, IEnumerable
    {
        T this[uint index] { get; }
    }

    // todo: check if the given collection implements the WinCopies.DotNetFix.IDisposable (or WinCopies.IDisposable) interface and, if yes, check the given collection is not disposed (or disposing) in the Current property and in the MoveNext method.

    public abstract class UIntIndexedListEnumeratorBase : Util.DotNetFix.IDisposable

    {
        private uint? index = null;

        protected internal uint? Index { get { ThrowIfDisposed(this); return index; } set { ThrowIfDisposed(this); index = value; } }

        private readonly Func<bool> moveNextMethod;

        protected internal Func<bool> MoveNextMethod { get { ThrowIfDisposed(this); return moveNextMethod; } set { ThrowIfDisposed(this); MoveNextMethod = value; } }

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

        ~UIntIndexedListEnumeratorBase()
        {

            Dispose(false);

        }

        public virtual bool MoveNext()
        {
            ThrowIfDisposed(this);

            return MoveNextMethod();
        }

        public virtual void Reset() => Index = null;
        #endregion
    }

    public sealed class UIntIndexedListEnumerator : UIntIndexedListEnumeratorBase, IEnumerator

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

        public UIntIndexedListEnumerator(IUIntIndexedList uintIndexedList)
        {

            MoveNextMethod = moveNextToReset = () => DefaultMoveNextMethod(this);

            innerList = uintIndexedList;

        }

        public UIntIndexedListEnumerator(IUIntIndexedList uintIndexedList, Func<bool> moveNextMethod)
        {
            MoveNextMethod = moveNextMethod;

            innerList = uintIndexedList;
        }

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

    public sealed class UIntIndexedListEnumerator<T> : UIntIndexedListEnumeratorBase, IEnumerator<T>

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

        object IEnumerator.Current => Current;

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
}
