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

#if CS7
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Util;

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections.Generic
{
    /// <summary>
    /// Builds arrays and lists by sizing them only when required. This class can be used to initialize your array or list before adding or removing values to it.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public class ArrayMerger<T> : WinCopies.Collections.DotNetFix.Generic.LinkedCollection<IUIntCountableEnumerable<T>>
    {
        public ulong RealCount { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayMerger{T}"/> class.
        /// </summary>
        public ArrayMerger() { /* Left empty. */ }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayMerger{T}"/> class with a given <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="enumerable">An enumerable from which to add values.</param>
        public ArrayMerger(in System.Collections.Generic.IEnumerable<IUIntCountableEnumerable<T>> enumerable) : this(new WinCopies.Collections.DotNetFix.Generic.LinkedList<IUIntCountableEnumerable<T>>(enumerable)) { /* Left empty. */ }

        public ArrayMerger(in ILinkedList3<IUIntCountableEnumerable<T>> linkedList) : base(linkedList) { /* Left empty. */ }

        protected void ValidateParameters(in T[] array, in int? startIndex) => ValidateParameters((array ?? throw GetArgumentNullException(nameof(array))).Length, startIndex, true);

        protected void ValidateParameters(in ArrayList arrayList, in int? startIndex) => ValidateParameters((arrayList ?? throw GetArgumentNullException(nameof(arrayList))).Count, startIndex, false);

        protected void ValidateParameters(in IList<T> list, in int? startIndex) => ValidateParameters((list ?? throw GetArgumentNullException(nameof(list))).Count, startIndex, false);

        protected void ValidateRealCount()
        {
            if (RealCount > int.MaxValue)

                throw new IndexOutOfRangeException($"{nameof(RealCount)} is greater than {nameof(Int32)}.{nameof(Int32.MaxValue)}.");
        }

        protected void ValidateParameters(in int count, in int? startIndex, in bool isFixedSize)
        {
            if (RealCount > int.MaxValue || (startIndex.HasValue && ((isFixedSize ? startIndex + (int)RealCount : startIndex) > count)))

                throw new IndexOutOfRangeException($"{nameof(startIndex)} is not in the value range or {nameof(RealCount)} is greater than {nameof(Int32)}.{nameof(Int32.MaxValue)}.");
        }

        /// <summary>
        /// Returns an array with the items of this <see cref="ArrayMerger{T}"/>.
        /// </summary>
        /// <param name="remove">Indicates whether items have to be removed from the current <see cref="ArrayMerger{T}"/> after they has been added to the new list.</param>
        /// <returns>An array with the items of this <see cref="ArrayMerger{T}"/>.</returns>
        public T[] ToArray(in bool remove = false)
        {
            ValidateRealCount();

            var array = new T[RealCount];

            ToArrayPrivate(array, remove, 0);

            return array;
        }

        public void ToArray(in T[] array, in bool remove = false, in int startIndex = 0)
        {
            ValidateParameters(array, startIndex);

            ToArrayPrivate(array, remove, startIndex);
        }

        private void ToArrayPrivate(in T[] array, in bool remove, int startIndex)
        {
            if (remove)

                while (Count != 0)
                {
                    foreach (T item in First.Value)

                        array[startIndex++] = item;

                    RemoveFirst();
                }

            else

                foreach (IUIntCountableEnumerable<T> _array in this.AsFromType<System.Collections.Generic.IEnumerable<IUIntCountableEnumerable<T>>>())

                    foreach (T item in _array)

                        array[startIndex++] = item;
        }

        /// <summary>
        /// Returns an <see cref="ArrayList"/> with the items of this <see cref="ArrayMerger{T}"/>.
        /// </summary>
        /// <param name="remove">Indicates whether items have to be removed from the current <see cref="ArrayMerger{T}"/> after they has been added to the new list.</param>
        /// <returns>An <see cref="ArrayList"/> with the items of this <see cref="ArrayMerger{T}"/>.</returns>
        public ArrayList ToArrayList(in bool remove = false)
        {
            ValidateRealCount();

            var arrayList = new ArrayList((int)RealCount);

            ToArrayListPrivate(arrayList, remove, null);

            return arrayList;
        }

        public void ToArrayList(in ArrayList arrayList, in bool remove = false, in int? startIndex = null)
        {
            ValidateParameters(arrayList, startIndex);

            ToArrayListPrivate(arrayList, remove, startIndex);
        }

        private void ToArrayListPrivate(ArrayList arrayList, in bool remove, in int? startIndex)
        {
            if (remove)
            {
                Action<IUIntCountableEnumerable<T>> action;

                if (startIndex.HasValue)
                {
                    int index = startIndex.Value;

                    action = array =>
                    {
                        foreach (T item in array)

                            arrayList.Insert(index++, item);
                    };
                }

                else

                    action = array =>
                    {
                        foreach (T item in array)

                            _ = arrayList.Add(item);
                    };

                while (Count != 0)

                    action(this.RemoveAndGetFirst().Value);
            }

            else
            {
                Action<T> action;

                if (startIndex.HasValue)
                {
                    int index = startIndex.Value;

                    action = item => arrayList.Insert(index++, item);
                }

                else

                    action = item => _ = arrayList.Add(item);

                foreach (IUIntCountableEnumerable<T> array in this.AsFromType<System.Collections.Generic.IEnumerable<IUIntCountableEnumerable<T>>>())

                    foreach (T item in array)

                        action(item);
            }
        }

        /// <summary>
        /// Returns a <see cref="List{T}"/> with the items of this <see cref="ArrayMerger{T}"/>.
        /// </summary>
        /// <param name="remove">Indicates whether items have to be removed from the current <see cref="ArrayMerger{T}"/> after they has been added to the new list.</param>
        /// <returns>A <see cref="List{T}"/> with the items of this <see cref="ArrayMerger{T}"/>.</returns>
        public List<T> ToList(in bool remove = false)
        {
            ValidateRealCount();

            var list = new List<T>((int)RealCount);

            ToListPrivate(list, remove, null);

            return list;
        }

        public void ToList(in IList<T> list, in bool remove = false, in int? startIndex = null)
        {
            ValidateParameters(list, startIndex);

            ToListPrivate(list, remove, startIndex);
        }

        private void ToListPrivate(IList<T> list, in bool remove, in int? startIndex)
        {
            if (remove)
            {
                Action<IUIntCountableEnumerable<T>> action;

                if (startIndex.HasValue)
                {
                    int index = startIndex.Value;

                    action = array =>
                    {
                        foreach (T item in array)

                            list.Insert(index++, item);
                    };
                }

                else

                    action = array =>
                    {
                        foreach (T item in array)

                            list.Add(item);
                    };

                while (Count != 0)

                    action(this.RemoveAndGetFirst().Value);
            }

            else
            {
                Action<T> action;

                if (startIndex.HasValue)
                {
                    int index = startIndex.Value;

                    action = item => list.Insert(index++, item);
                }

                else

                    action = item => list.Add(item);

                foreach (IUIntCountableEnumerable<T> array in this.AsFromType<System.Collections.Generic.IEnumerable<IUIntCountableEnumerable<T>>>())

                    foreach (T item in array)

                        action(item);
            }
        }

        #region Overrides
        protected override void OnNodeAdded(ILinkedListNode<IUIntCountableEnumerable<T>> node) => RealCount += node.Value.Count;
        protected override void OnNodeRemoved(ILinkedListNode<IUIntCountableEnumerable<T>> node) => RealCount -= node.Value.Count;

        protected override void ClearItems()
        {
            base.ClearItems();

            RealCount = 0ul;
        }
        #endregion
    }
}
#endif
