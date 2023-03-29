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
using System.Diagnostics.CodeAnalysis;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Util;

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public class ReadOnlyStackCollection<TStack, TItems> : ReadOnlySimpleLinkedCollection<TStack, TItems>, IStack<TItems> where TStack : IStack<TItems>
    {
        public ReadOnlyStackCollection(in TStack stack) : base(stack) { /* Left empty. */ }
        public ReadOnlyStackCollection(in StackCollection<TStack, TItems> stackCollection) : this(stackCollection.InnerList) { /* Left empty. */ }

        void IStackCore<TItems>.Push(TItems item) => throw GetReadOnlyListOrCollectionException();
        TItems IStackCore<TItems>.Pop() => throw GetReadOnlyListOrCollectionException();
        bool IStackCore<TItems>.TryPop(out TItems
#if CS9
            ?
#endif
            result)
        {
            result = default;

            return false;
        }
#if !CS8
        void IStackCore.Push(object item) => throw GetReadOnlyListOrCollectionException();
        object IStackCore.Pop() => throw GetReadOnlyListOrCollectionException();
        bool IStackCore.TryPop(out object result) => throw GetReadOnlyListOrCollectionException();
#endif
    }
    public class ReadOnlyStackCollection<T> : ReadOnlyStackCollection<IStack<T>, T>
    {
        public ReadOnlyStackCollection(in IStack<T> stack) : base(stack) { /* Left empty. */ }
        public ReadOnlyStackCollection(in StackCollection<IStack<T>, T> stackCollection) : this(stackCollection.InnerList) { /* Left empty. */ }
    }

    public class ReadOnlyEnumerableStackCollection<TStack, TItems> : ReadOnlyStackCollection<TStack, TItems>, IEnumerableStack<TItems>, IReadOnlyCollection<TItems>, ICollection where TStack : IEnumerableStack<TItems>
    {
        protected ICollection InnerCollection => InnerList;

        int ICollection.Count => (int)Count;
        int IReadOnlyCollection<TItems>.Count => (int)Count;

        bool ICollection.IsSynchronized => InnerCollection.IsSynchronized;
        object ICollection.SyncRoot => InnerCollection.SyncRoot;

        public ReadOnlyEnumerableStackCollection(in TStack stack) : base(stack) { /* Left empty. */ }
        public ReadOnlyEnumerableStackCollection(in StackCollection<TStack, TItems> stackCollection) : this(stackCollection.InnerList) { /* Left empty. */ }

        void IEnumerableSimpleLinkedList<TItems>.CopyTo(TItems[] array, int index) => InnerList.CopyTo(array, index);

        TItems[] IEnumerableSimpleLinkedList<TItems>.ToArray() => InnerList.ToArray();

        public System.Collections.Generic.IEnumerator<TItems> GetEnumerator() => InnerList.GetEnumerator();

        /// <summary>
        /// Determines whether an element is in the <see cref="StackCollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="StackCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is found in the Stack; otherwise, <see langword="false"/>.</returns>
        public bool Contains(TItems item) => InnerList.Contains(item);

        void ICollection.CopyTo(System.Array array, int index) => InnerList.CopyTo(array, index);

        /// <summary>
        /// Copies the <see cref="StackCollection{T}"/> elements to an existing one-dimensional <see cref="System.Array"/>, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="StackCollection{T}"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than zero.</exception>
        /// <exception cref="ArgumentException">The number of elements in the source <see cref="StackCollection{T}"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination array.</exception>
        public void CopyTo(in TItems[] array, in int arrayIndex) => InnerList.CopyTo(array, arrayIndex);
#if !CS8
        System.Collections.IEnumerator IEnumerable.GetEnumerator() => InnerList.GetEnumerator();
#endif
    }

    public class ReadOnlyEnumerableStackCollection<T> : ReadOnlyEnumerableStackCollection<IEnumerableStack<T>, T>
    {
        public ReadOnlyEnumerableStackCollection(in IEnumerableStack<T> stack) : base(stack) { /* Left empty. */ }
        public ReadOnlyEnumerableStackCollection(in StackCollection<IEnumerableStack<T>, T> stackCollection) : this(stackCollection.InnerList) { /* Left empty. */ }
    }
}
#endif
