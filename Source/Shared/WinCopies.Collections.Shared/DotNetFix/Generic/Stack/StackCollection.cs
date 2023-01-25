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

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;

#if CS8
using System.Diagnostics.CodeAnalysis;
#endif

namespace WinCopies.Collections.DotNetFix.Generic
{
    public class StackCollection<TStack, TItems> : SimpleLinkedCollection<TStack, TItems>, IStack<TItems> where TStack : IStack<TItems>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StackCollection{T}"/> class with a custom queue.
        /// </summary>
        /// <param name="stack">The inner queue for this <see cref="StackCollection{T}"/>.</param>
        public StackCollection(in TStack stack) : base(stack) { /* Left empty. */ }

        /// <summary>
        /// Adds an object to the end of the <see cref="StackCollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="StackCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
        public void Push(TItems item) => Add(item);

        /// <summary>
        /// Removes and returns the object at the beginning of the <see cref="StackCollection{T}"/>.
        /// </summary>
        /// <returns>The object that is removed from the beginning of the <see cref="StackCollection{T}"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="StackCollection{T}"/> is empty.</exception>
        /// <seealso cref="TryPop(out TItems)"/>
        public TItems Pop() => Remove();

        /// <summary>
        /// Tries to remove and return the object at the beginning of the <see cref="StackCollection{T}"/>.
        /// </summary>
        /// <param name="result">The object at the beginning of the <see cref="StackCollection{T}"/>. This value can be <see langword="null"/> when the return value is <see langword="false"/>.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether a value has actually been removed and retrieved.</returns>
        /// <seealso cref="Pop"/>
        public bool TryPop(
#if CS8
                [MaybeNullWhen(false)]
#endif
            out TItems result) => TryRemove(out result);

        public override void Add(TItems item) => InnerList.Push(item);
        public override TItems Remove() => InnerList.Pop();
        public override bool TryRemove(out TItems result) => InnerList.TryPop(out result);
#if !CS8
        void IStackCore.Push(object item) => Push((TItems)item);
        object IStackCore.Pop() => Pop();
        public bool TryPop(out object result) => UtilHelpers.TryGetValue<TItems>(TryPop, out result);
#endif
    }

    public class StackCollection<T> : StackCollection<IStack<T>, T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StackCollection{T}"/> class with a custom Stack.
        /// </summary>
        /// <param name="stack">The inner stack for this <see cref="StackCollection{T}"/>.</param>
        public StackCollection(in IStack<T> stack) : base(stack) { /* Left empty. */ }

        public StackCollection() : this(new Stack<T>()) { /* Left empty. */ }
    }

    public class StackBaseCollection<TStack, TItems> : SimpleLinkedListBaseCollection<TStack, TItems>, IStackBase<TItems> where TStack : IStackBase<TItems>
    {
        public StackBaseCollection(in TStack list) : base(list) { /* Left empty. */ }

        public void Push(TItems item) => Add(item);
        public TItems Pop() => Remove();
        public bool TryPop(out TItems result) => TryRemove(out result);

        void IStackCore.Push(object item) => Add((TItems)item);
        object IStackCore.Pop() => Remove();
        public bool TryPop(out object result) => TryRemove(out result);
    }

    public class StackBaseCollection<T> : StackBaseCollection<IStackBase<T>, T>
    {
        public StackBaseCollection(in IStackBase<T> list) : base(list) { /* Left empty. */ }
        public StackBaseCollection() : this(EnumerableHelper<T>.GetStack()) { /* Left empty. */ }
    }
}
#endif
