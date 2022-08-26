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

namespace WinCopies.Collections.DotNetFix
{
#if WinCopies3
    namespace Generic
    {
#endif
        public class ReadOnlyStackCollection<
#if WinCopies3
            TStack, TItems
#else
            T
#endif
            > :
#if WinCopies3
            IStack<TItems> where TStack : IStack<TItems>
#else
            IEnumerableStack<T>, System.Collections.Generic.IReadOnlyCollection<T>, ICollection
#endif
        {
            protected
#if !WinCopies3
                System.Collections.Generic.Stack<T>
#else
                TStack
#endif
            InnerStack
            { get; }

            /// <summary>
            /// Gets the number of elements contained in the <see cref="StackCollection{T}"/>.
            /// </summary>
            /// <value>The number of elements contained in the <see cref="StackCollection{T}"/>.</value>
            public
#if WinCopies3
                uint
#else
                int
#endif
                Count => InnerStack.Count;

            public bool IsReadOnly => true;

            /// <summary>
            /// Initializes a new instance of the <see cref="StackCollection{T}"/> class with a custom <see cref="System.Collections.Generic.Stack{T}"/>.
            /// </summary>
            /// <param name="Stack">The inner <see cref="System.Collections.Generic.Stack{T}"/> for this <see cref="StackCollection{T}"/>.</param>
            public ReadOnlyStackCollection(in
#if WinCopies3
                TStack
#else
                System.Collections.Generic.Stack<T>
#endif
                Stack) => InnerStack = Stack;

            public ReadOnlyStackCollection(in StackCollection<
#if WinCopies3
                TStack, TItems
#else
                T
#endif
                > StackCollection) : this(StackCollection.InnerStack) { /* Left empty. */ }

            /// <summary>
            /// Returns the object at the beginning of the <see cref="StackCollection{T}"/> without removing it.
            /// </summary>
            /// <returns>The object at the beginning of the <see cref="StackCollection{T}"/>.</returns>
            /// <exception cref="InvalidOperationException">The <see cref="StackCollection{T}"/> is empty.</exception>
            /// <seealso cref="TryPeek(out T)"/>
            public
#if WinCopies3
                TItems
#else
                T
#endif
                 Peek() => InnerStack.Peek();

#if CS8
            /// <summary>
            /// Tries to peek the object at the beginning of the <see cref="StackCollection{T}"/> without removing it.
            /// </summary>
            /// <param name="result">The object at the beginning of the <see cref="StackCollection{T}"/>. This value can be <see langword="null"/> when the return value is <see langword="false"/>.</param>
            /// <returns>A <see cref="bool"/> value that indicates whether a value has actually been retrieved.</returns>
            /// <seealso cref="Peek"/>
            public bool TryPeek([MaybeNullWhen(false)] out
#if WinCopies3
                TItems
#else
                T
#endif
                result) => InnerStack.TryPeek(out result);
#endif

#if WinCopies3
            void IStackBase<TItems>.Push(TItems item) => throw GetReadOnlyListOrCollectionException();

            TItems IStackBase<TItems>.Pop() => throw GetReadOnlyListOrCollectionException();

            bool IStackBase<TItems>.TryPop(out TItems
#if CS9
                ?
#endif
                result)
            {
                result = default;

                return false;
            }

            bool IPeekable<TItems>.TryPeek(out TItems result) => InnerStack.TryPeek(out result);

            void ISimpleLinkedListBase.Clear() => throw GetReadOnlyListOrCollectionException();

            object ISimpleLinkedListBase2.SyncRoot => InnerStack.SyncRoot;

            bool ISimpleLinkedListBase2.IsSynchronized => InnerStack.IsSynchronized;

            bool ISimpleLinkedListBase.HasItems => InnerStack.HasItems;
#if !CS8
            bool ISimpleLinkedList.TryPeek(out object result) => InnerStack.TryPeek(out result);

            object ISimpleLinkedList.Peek() => InnerStack.Peek();
#endif
#else
        bool ICollection.IsSynchronized => ((ICollection)InnerStack).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerStack).SyncRoot;

        uint IUIntCountable.Count => (uint)Count;

        uint IUIntCountableEnumerable.Count => (uint)Count;

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="StackCollection{T}"/>.
        /// </summary>
        /// <returns>An <see cref="System.Collections.Generic.IEnumerator{T}"/> for the <see cref="StackCollection{T}"/>.</returns>
        public System.Collections.Generic.IEnumerator<T> GetEnumerator() => InnerStack.GetEnumerator();

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerStack).GetEnumerator();

        /// <summary>
        /// Determines whether an element is in the <see cref="StackCollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="StackCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is found in the Stack; otherwise, <see langword="false"/>.</returns>
        public bool Contains(T item) => InnerStack.Contains(item);

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerStack).CopyTo(array, index);

        /// <summary>
        /// Copies the <see cref="StackCollection{T}"/> elements to an existing one-dimensional <see cref="System.Array"/>, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="StackCollection{T}"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than zero.</exception>
        /// <exception cref="ArgumentException">The number of elements in the source <see cref="StackCollection{T}"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination array.</exception>
        public void CopyTo(in T[] array, in int arrayIndex) => InnerStack.CopyTo(array, arrayIndex);

        void IStack<T>.Push(T item) => throw GetReadOnlyListOrCollectionException();

        T IStack<T>.Pop() => throw GetReadOnlyListOrCollectionException();
#endif
        }
#if WinCopies3
        public class ReadOnlyStackCollection<T> : ReadOnlyStackCollection<IStack<T>, T>
        {
            public ReadOnlyStackCollection(in IStack<T> Stack) : base(Stack)
            {
                // Left empty.
            }

            public ReadOnlyStackCollection(in StackCollection<IStack<T>, T> StackCollection) : this(StackCollection.InnerStack)
            {
                // Left empty.
            }
        }

        public class ReadOnlyEnumerableStackCollection<TStack, TItems> : ReadOnlyStackCollection<TStack, TItems>, IEnumerableStack<TItems>, IReadOnlyCollection<TItems>, ICollection where TStack : IEnumerableStack<TItems>
        {
            int ICollection.Count => (int)Count;

            int IReadOnlyCollection<TItems>.Count => (int)Count;

            protected ICollection InnerCollection => InnerStack.AsFromType<ICollection>();

            bool ICollection.IsSynchronized => InnerCollection.IsSynchronized;

            object ICollection.SyncRoot => InnerCollection.SyncRoot;

            public ReadOnlyEnumerableStackCollection(in TStack Stack) : base(Stack)
            {
                // Left empty.
            }

            public ReadOnlyEnumerableStackCollection(in StackCollection<TStack, TItems> StackCollection) : this(StackCollection.InnerStack)
            {
                // Left empty.
            }

            void IEnumerableSimpleLinkedList<TItems>.CopyTo(TItems[] array, int index) => InnerStack.CopyTo(array, index);

            TItems[] IEnumerableSimpleLinkedList<TItems>.ToArray() => InnerStack.ToArray();

            public
#if WinCopies3
                System.Collections.Generic.IEnumerator
#else
                IUIntCountableEnumerator
#endif
                <TItems> GetEnumerator() => InnerStack.GetEnumerator();

            System.Collections.Generic.IEnumerator<TItems> System.Collections.Generic.IEnumerable<TItems>.GetEnumerator() => GetEnumerator();

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerStack).GetEnumerator();

            /// <summary>
            /// Determines whether an element is in the <see cref="StackCollection{T}"/>.
            /// </summary>
            /// <param name="item">The object to locate in the <see cref="StackCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
            /// <returns><see langword="true"/> if <paramref name="item"/> is found in the Stack; otherwise, <see langword="false"/>.</returns>
            public bool Contains(TItems item) => InnerStack.Contains(item);

            void ICollection.CopyTo(Array array, int index) => InnerStack.CopyTo(array, index);

            /// <summary>
            /// Copies the <see cref="StackCollection{T}"/> elements to an existing one-dimensional <see cref="System.Array"/>, starting at the specified array index.
            /// </summary>
            /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="StackCollection{T}"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
            /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
            /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than zero.</exception>
            /// <exception cref="ArgumentException">The number of elements in the source <see cref="StackCollection{T}"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination array.</exception>
            public void CopyTo(in TItems[] array, in int arrayIndex) => InnerStack.CopyTo(array, arrayIndex);
        }

        public class ReadOnlyEnumerableStackCollection<T> : ReadOnlyEnumerableStackCollection<IEnumerableStack<T>, T>
        {
            public ReadOnlyEnumerableStackCollection(in IEnumerableStack<T> Stack) : base(Stack) { /* Left empty. */ }

            public ReadOnlyEnumerableStackCollection(in StackCollection<IEnumerableStack<T>, T> StackCollection) : this(StackCollection.InnerStack) { /* Left empty. */ }
        }
    }
#endif
}
#endif
