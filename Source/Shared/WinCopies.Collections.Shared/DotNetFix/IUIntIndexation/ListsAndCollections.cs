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

namespace WinCopies.Collections.DotNetFix
{
    public interface IUIntIndexedCollection :
#if !WinCopies3
IEnumerable
    {
        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        uint Count { get; }
#else
        IUIntCountableEnumerable
    {
#endif
        object SyncRoot { get; }

        bool IsSynchronized { get; }

        /// <summary>
        /// Copies the items of this collection to a given array starting at a given index.
        /// </summary>
        /// <param name="array">The array to copy the items.</param>
        /// <param name="index">The index in the array from which to start to copy.</param>
        void CopyTo(in Array array, int index);
    }

    public interface IReadOnlyUIntIndexedList : IUIntIndexedCollection
#if !WinCopies3
, IEnumerable
#endif
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

    public interface IUIntIndexedList : IReadOnlyUIntIndexedList
#if !WinCopies3
, IEnumerable
#endif
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

#if !WinCopies3
    public interface IReadOnlyUIntIndexedCollection<
#if CS5
            out
#endif
            T> : System.Collections.Generic.IEnumerable<T>, IEnumerable
    {
        uint Count { get; }
    }
#else
    namespace Generic
    {
#endif
        public interface IUIntIndexedCollection<T> :
#if !WinCopies3
            IReadOnlyUIntIndexedCollection<T>, System.Collections.Generic.IEnumerable<T>, IEnumerable
#else
            IUIntCountableEnumerable<T>
#endif
        {
            void Add(
#if !WinCopies3
in
#endif
                T item);

            void Clear();

            bool Contains(
#if !WinCopies3
in
#endif
                 T item);

            void CopyTo(
#if !WinCopies3
in
#endif
                 T[] array, uint arrayIndex);

            bool Remove(
#if !WinCopies3
in
#endif
                 T item);
        }

        public interface IReadOnlyUIntIndexedList<
#if CS5
            out
#endif
            T> :
#if !WinCopies3
            IReadOnlyUIntIndexedCollection<T>, System.Collections.Generic.IEnumerable<T>, IEnumerable
#else
            IUIntCountableEnumerable<T>
#endif
        {
            T this[uint index] { get; }
        }

        public interface IUIntIndexedList<T> : IUIntIndexedCollection<T>, IReadOnlyUIntIndexedList<T>
#if !WinCopies3
, System.Collections.Generic.IEnumerable<T>, IEnumerable
#endif
        {
            new T this[uint index] { get; set; }

            uint? IndexOf(
#if !WinCopies3
in
#endif
                 T item);

            void Insert(
#if !WinCopies3
in
#endif
                 uint index,
#if !WinCopies3
in
#endif
                 T item);

            void RemoveAt(
#if !WinCopies3
in
#endif
                 uint index);
        }
#if WinCopies3
    }
#endif
}
