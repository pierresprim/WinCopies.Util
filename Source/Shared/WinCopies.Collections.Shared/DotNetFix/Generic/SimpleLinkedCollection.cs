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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using WinCopies.Util;

namespace WinCopies.Collections.DotNetFix
{
    namespace Generic
    {
        [Serializable, DebuggerDisplay("Count = {Count}")]
        public class SimpleLinkedCollection<TList, TItems> : SimpleLinkedCollectionBase<TList>, ISimpleLinkedList<TItems>, ISimpleLinkedListCommon<TItems> where TList : ISimpleLinkedList<TItems>, ISimpleLinkedListCommon<TItems>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SimpleLinkedCollection{TList, TItems}"/> class with a custom queue.
            /// </summary>
            /// <param name="list">The inner queue for this <see cref="SimpleLinkedCollection{TList, TItems}"/>.</param>
            public SimpleLinkedCollection(in TList list) : base(list) { /* Left empty. */ }

            /// <summary>
            /// Returns the object at the beginning of the <see cref="SimpleLinkedCollection{T}"/> without removing it.
            /// </summary>
            /// <returns>The object at the beginning of the <see cref="SimpleLinkedCollection{T}"/>.</returns>
            /// <exception cref="InvalidOperationException">The <see cref="SimpleLinkedCollection{T}"/> is empty.</exception>
            /// <seealso cref="TryPeek(out TItems)"/>
            public TItems
#if CS9
                ?
#endif
                Peek() => InnerList.Peek();

            /// <summary>
            /// Tries to peek the object at the beginning of the <see cref="SimpleLinkedCollection{TList, TItems}"/> without removing it.
            /// </summary>
            /// <param name="result">The object at the beginning of the <see cref="SimpleLinkedCollection{TList, TItems}"/>. This value can be <see langword="null"/> when the return value is <see langword="false"/>.</param>
            /// <returns>A <see cref="bool"/> value that indicates whether a value has actually been retrieved.</returns>
            /// <seealso cref="Peek"/>
            public bool TryPeek(
#if CS8
                [MaybeNullWhen(false)]
#endif
                out TItems result) => InnerList.TryPeek(out result);

            /// <summary>
            /// Adds an object to the end of the <see cref="SimpleLinkedCollection{TList, TItems}"/>.
            /// </summary>
            /// <param name="item">The object to add to the <see cref="SimpleLinkedCollection{TList, TItems}"/>. The value can be <see langword="null"/> for reference types.</param>
            public virtual void Add(TItems
#if CS9
                ?
#endif
                item) => InnerList.Add(item);

            /// <summary>
            /// Removes and returns the object at the beginning of the <see cref="SimpleLinkedCollection{TList, TItems}"/>.
            /// </summary>
            /// <returns>The object that is removed from the beginning of the <see cref="SimpleLinkedCollection{TList, TItems}"/>.</returns>
            /// <exception cref="InvalidOperationException">The <see cref="SimpleLinkedCollection{TList, TItems}"/> is empty.</exception>
            /// <seealso cref="TryRemove(out TItems)"/>
            public virtual TItems
#if CS9
                ?
#endif
                Remove() => InnerList.Remove();

            public virtual bool TryRemove(
#if CS8
                [MaybeNullWhen(false)]
#endif
                out TItems result) => InnerList.TryRemove(out result);

            void IListCommon.Add(object value) => Add((TItems)value);
            object IListCommon.Remove() => Remove();
            bool IListCommon.TryRemove(out object result) => UtilHelpers.TryGetValue<TItems>(TryRemove, out result);
#if !CS8
            object IPeekable.Peek() => InnerList.Peek();
            bool IPeekable.TryPeek(out object result) => InnerList.TryPeek(out result);
#endif
        }
    }
}
#endif
