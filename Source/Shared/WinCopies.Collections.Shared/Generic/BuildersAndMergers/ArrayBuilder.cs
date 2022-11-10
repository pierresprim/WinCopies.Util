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

using WinCopies;
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
    public class ArrayBuilder<T> : DotNetFix.Generic.LinkedList<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        public ArrayBuilder() { /* Left empty. */ }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayBuilder{T}"/> class with a given <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="enumerable">An enumerable from which to add values.</param>
        public ArrayBuilder(in System.Collections.Generic.IEnumerable<T> enumerable) : base(enumerable) { /* Left empty. */ }

        protected void ValidateParameters(in T[] array, in int? startIndex) => ValidateParameters((array ?? throw GetArgumentNullException(nameof(array))).Length, startIndex, true);

        protected void ValidateParameters(in ArrayList arrayList, in int? startIndex) => ValidateParameters((arrayList ?? throw GetArgumentNullException(nameof(arrayList))).Count, startIndex, false);

        protected void ValidateParameters(in IList<T> list, in int? startIndex) => ValidateParameters((list ?? throw GetArgumentNullException(nameof(list))).Count, startIndex, false);

        protected void ValidateParameters(in int count, in int? startIndex, in bool isFixedSize)
        {
            ValidateCount();

            if (startIndex.HasValue && ((isFixedSize ? startIndex + Count : startIndex) > count))

                throw new IndexOutOfRangeException($"{nameof(startIndex)} is not in the value range.");
        }

        protected void ValidateCount()
        {
            if (Count > int.MaxValue)

                throw new IndexOutOfRangeException($"{nameof(Count)} is greater than {nameof(Int32)}.{nameof(Int32.MaxValue)}.");
        }

        /// <summary>
        /// Returns an array with the items of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="remove">Indicates whether items have to be removed from the current <see cref="ArrayBuilder{T}"/> after they has been added to the new list.</param>
        /// <returns>An array with the items of this <see cref="ArrayBuilder{T}"/>.</returns>
        public T[] ToArray(in bool remove = false)
        {
            ValidateCount();

            var array = new T[Count];

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
                    array[startIndex++] = First.Value;

                    RemoveFirst();
                }

            else

                foreach (T item in this.AsFromType<System.Collections.Generic.IEnumerable<T>>())

                    array[startIndex++] = item;
        }

        /// <summary>
        /// Returns an <see cref="ArrayList"/> with the items of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="remove">Indicates whether items have to be removed from the current <see cref="ArrayBuilder{T}"/> after they has been added to the new list.</param>
        /// <returns>An <see cref="ArrayList"/> with the items of this <see cref="ArrayBuilder{T}"/>.</returns>
        public ArrayList ToArrayList(in bool remove = false)
        {
            ValidateCount();

            var arrayList = new ArrayList((int)Count);

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
                Action action;

                if (startIndex.HasValue)
                {
                    int index = startIndex.Value;

                    action = () => arrayList.Insert(index++, this.RemoveAndGetFirst().Value);
                }

                else

                    action = () => _ = arrayList.Add(this.RemoveAndGetFirst().Value);

                while (Count != 0)

                    action();
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

                foreach (T item in this.AsFromType<System.Collections.Generic.IEnumerable<T>>())

                    action(item);
            }
        }

        /// <summary>
        /// Returns a <see cref="List{T}"/> with the items of this <see cref="ArrayBuilder{T}"/>.
        /// </summary>
        /// <param name="remove">Indicates whether items have to be removed from the current <see cref="ArrayBuilder{T}"/> after they has been added to the new list.</param>
        /// <returns>A <see cref="List{T}"/> with the items of this <see cref="ArrayBuilder{T}"/>.</returns>
        public List<T> ToList(in bool remove = false)
        {
            ValidateCount();

            var list = new List<T>((int)Count);

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
                Action action;

                if (startIndex.HasValue)
                {
                    int index = startIndex.Value;

                    action = () => list.Insert(index++, this.RemoveAndGetFirst().Value);
                }

                else

                    action = () => list.Add(this.RemoveAndGetFirst().Value);

                while (Count != 0)

                    action();
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

                foreach (T item in this.AsFromType<System.Collections.Generic.IEnumerable<T>>())

                    action(item);
            }
        }
    }
}
#endif
