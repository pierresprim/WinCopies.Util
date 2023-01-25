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

namespace WinCopies.Collections.DotNetFix
{
    public abstract class SimpleLinkedCollectionCore<T> : IUIntCountable, ISimpleLinkedListCore where T : IUIntCountable, ISimpleLinkedListCore
    {
        protected internal T InnerList { get; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="SimpleLinkedCollectionCore{T}"/>.
        /// </summary>
        public uint Count => InnerList.Count;

        public bool HasItems => InnerList.HasItems;

        public abstract bool IsReadOnly { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleLinkedCollectionCore{T}"/> class with a custom queue.
        /// </summary>
        /// <param name="queue">The inner queue for this <see cref="SimpleLinkedCollectionCore{T}"/>.</param>
        public SimpleLinkedCollectionCore(in T list) => InnerList = list;
    }

    [Serializable]
    public abstract class SimpleLinkedCollectionBase<T> : SimpleLinkedCollectionCore<T>, ISimpleLinkedListBase2 where T : IUIntCountable, ISimpleLinkedListBase2
    {
        public override bool IsReadOnly => InnerList.IsReadOnly;

        bool ISimpleLinkedListBase2.IsSynchronized => InnerList.IsSynchronized;
        object ISimpleLinkedListBase2.SyncRoot => InnerList.SyncRoot;

        public SimpleLinkedCollectionBase(in T list) : base(list) { /* Left empty. */ }

        /// <summary>
        /// Removes all objects from the <see cref="SimpleLinkedCollectionBase{T}"/>.
        /// </summary>
        public virtual void Clear() => InnerList.Clear();
    }

    public abstract class SimpleLinkedCollection<T> : SimpleLinkedCollectionBase<T>, ISimpleLinkedListCommon where T : IUIntCountable, ISimpleLinkedListBase2, IPeekable, ISimpleLinkedListCommon
    {
        public SimpleLinkedCollection(in T list) : base(list) { /* Left empty. */ }

        /// <summary>
        /// Returns the object at the beginning of the <see cref="SimpleLinkedCollection{T}"/> without removing it.
        /// </summary>
        /// <returns>The object at the beginning of the <see cref="SimpleLinkedCollection{T}"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="SimpleLinkedCollection{T}"/> is empty.</exception>
        public object
#if CS8
            ?
#endif
            Peek() => InnerList.Peek();
        public bool TryPeek(out object
#if CS8
            ?
#endif
            result) => InnerList.TryPeek(out result);

        public abstract void Add(object
#if CS8
            ?
#endif
            value);
        public abstract object
#if CS8
            ?
#endif
            Remove();
        public abstract bool TryRemove(out object
#if CS8
            ?
#endif
            result);
    }

    public class SimpleLinkedListBaseCollectionBase<T> : ISimpleLinkedListBase where T : ISimpleLinkedListBase, IPeekable, ISimpleLinkedListCommon
    {
        protected T InnerList { get; }

        public bool IsReadOnly => InnerList.IsReadOnly;
        public bool HasItems => InnerList.HasItems;

        protected SimpleLinkedListBaseCollectionBase(in T list) => InnerList = list;

        public virtual void Clear() => InnerList.Clear();
    }

    public class SimpleLinkedListBaseCollection<T> : SimpleLinkedListBaseCollectionBase<T>, IPeekable, ISimpleLinkedListCommon where T : ISimpleLinkedListBase, IPeekable, ISimpleLinkedListCommon
    {
        protected SimpleLinkedListBaseCollection(in T list) : base(list) { /* Left empty. */ }

        public object Peek() => InnerList.Peek();
        public bool TryPeek(out object result) => InnerList.TryPeek(out result);

        public virtual void Add(object value) => InnerList.Add(value);
        public virtual object Remove() => InnerList.Remove();
        public virtual bool TryRemove(out object result) => InnerList.TryRemove(out result);
    }
}
