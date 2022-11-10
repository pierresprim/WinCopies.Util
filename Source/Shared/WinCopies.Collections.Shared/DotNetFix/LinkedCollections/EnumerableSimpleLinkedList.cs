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

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections.DotNetFix
{
    public interface IEnumerableSimpleLinkedList : ISimpleLinkedList, IEnumerableSimpleLinkedListBase, IEnumerable
    {
        void CopyTo(System.Array array, int index);

        object[] ToArray();
    }

    public abstract class EnumerableSimpleLinkedList : EnumerableSimpleLinkedListBase, IEnumerableSimpleLinkedList
    {
        public abstract object Peek();

        public abstract bool TryPeek(out object result);

        public abstract System.Collections.IEnumerator GetEnumerator();

        public void CopyTo(System.Array array, int arrayIndex) => EnumerableExtensions.CopyTo(this, array, arrayIndex, Count);

        public object[] ToArray()
        {
            object[] result = Count > int.MaxValue ? throw new InvalidOperationException("Too many items in list or collection.") : new object[Count];

            int i = -1;

            foreach (object value in this)

                result[++i] = value;

            return result;
        }
    }

    public abstract class EnumerableSimpleLinkedList<T> : EnumerableSimpleLinkedList, IPeekable, ICollection, ISimpleLinkedListCore, IListCommon where T : IUIntCountable, IPeekable, IClearable, ISimpleLinkedListCore, IListCommon
    {
        [NonSerialized]
        private readonly T _list;

        protected T InnerList => _list;

        public override uint Count => _list.Count;

        int ICollection.Count => (int)Count;

        public override bool HasItems => _list.HasItems;

        protected abstract ISimpleLinkedListNode2
#if CS8
            ?
#endif
            FirstNode
        { get; }

        public EnumerableSimpleLinkedList(in T list) => _list = list;

        public sealed override object Peek() => _list.Peek();
        public sealed override bool TryPeek(out object result) => _list.TryPeek(out result);

        public sealed override void Clear()
        {
            _list.Clear();

            UpdateEnumerableVersion();
        }

        /// <summary>
        /// Returns an <see cref="System.Collections.IEnumerator"/> for this <see cref="EnumerableSimpleLinkedList{T}"/>.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator"/> for this <see cref="EnumerableSimpleLinkedList{T}"/>.</returns>
        public sealed override System.Collections.IEnumerator GetEnumerator()
        {
            var enumerator = new Enumerator(this);

            IncrementEnumeratorCount();

            return enumerator;
        }

        public void Add(object value) => InnerList.Add(value);
        public object Remove() => InnerList.Remove();
        public bool TryRemove(out object result) => InnerList.TryRemove(out result);

        public sealed class Enumerator : Enumerator<object, ISimpleLinkedListNode2, EnumerableSimpleLinkedList<T>>
        {
            /// <summary>
            /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
            /// </summary>
            protected override object CurrentOverride => CurrentNode.Value;

            protected override ISimpleLinkedListNode2 FirstNode => List.FirstNode;
            protected override ISimpleLinkedListNode2 NextNode => CurrentNode.Next;

            public override bool? IsResetSupported => true;

            public Enumerator(in EnumerableSimpleLinkedList<T> list) : base(list) { /* Left empty. */ }
        }
    }
}
