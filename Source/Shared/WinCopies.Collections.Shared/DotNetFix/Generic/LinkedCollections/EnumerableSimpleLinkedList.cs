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

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public abstract class EnumerableSimpleLinkedList<T> : EnumerableSimpleLinkedListBase, IEnumerableSimpleLinkedList<T>, ISimpleLinkedListCommon<T>
    {
#if CS7
        int IReadOnlyCollection<T>.Count => (int)Count;
#endif
        int ICollection.Count => (int)Count;

        public abstract T
#if CS9
            ?
#endif
            Peek();
        public abstract bool TryPeek(out T
#if CS9
            ?
#endif
            result);

        public void CopyTo(T[] array, int arrayIndex) => WinCopies.Collections.EnumerableExtensions.CopyTo(this, array, arrayIndex, Count);

        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();

        public abstract IUIntCountableEnumerator<T> GetEnumerator();

        public T[] ToArray()
        {
            var result = Count > int.MaxValue ? throw new OverflowException("Too many items in list or collection.") : new T[Count];

            int i = -1;

            foreach (T value in this)

                result[++i] = value;

            return result;
        }

        public void CopyTo(System.Array array, int index) => WinCopies.Collections.EnumerableExtensions.CopyTo(this, array, index, Count);

        public abstract void Add(T
#if CS9
            ?
#endif
            item);
        public abstract T
#if CS9
            ?
#endif
            Remove();
        public abstract bool TryRemove(out T
#if CS9
            ?
#endif
            result);

        void IListCommon.Add(object value) => Add((T)value);
        object IListCommon.Remove() => Remove();
        public bool TryRemove(out object result) => UtilHelpers.TryGetValue<T>(TryRemove, out result);
#if !CS8
        System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        object IPeekable.Peek() => Peek();
        bool IPeekable.TryPeek(out object result) => UtilHelpers.TryGetValue<T>(TryPeek, out result);
#endif
    }

    public abstract class EnumerableSimpleLinkedList<TItems, TList> : EnumerableSimpleLinkedList<TItems> where TList : IPeekable<TItems>, ISimpleLinkedListBase, IUIntCountable, ISimpleLinkedListCommon<TItems>
    {
        [NonSerialized]
        private readonly TList _list;

        protected TList List => _list;

        public override uint Count => _list.Count;

        public override bool HasItems => _list.HasItems;

        protected abstract ISimpleLinkedListNode<TItems>
#if CS8
            ?
#endif
            FirstNode
        { get; }

        public EnumerableSimpleLinkedList(in TList list) => _list = list;

        public sealed override TItems
#if CS9
            ?
#endif
            Peek() => _list.Peek();
        public sealed override bool TryPeek(out TItems
#if CS9
            ?
#endif
            result) => _list.TryPeek(out result);

        public override void Add(TItems
#if CS9
            ?
#endif
            item)
        {
            _list.Add(item);

            UpdateEnumerableVersion();
        }
        public override TItems
#if CS9
            ?
#endif
            Remove()
        {
            TItems
#if CS9
                ?
#endif
                result = _list.Remove();

            UpdateEnumerableVersion();

            return result;
        }
        public override bool TryRemove(out TItems
#if CS9
            ?
#endif
            result)
        {
            if (_list.TryRemove(out result))
            {
                UpdateEnumerableVersion();

                return true;
            }

            return false;
        }

        public sealed override void Clear()
        {
            _list.Clear();

            UpdateEnumerableVersion();
        }

        public sealed override IUIntCountableEnumerator<TItems> GetEnumerator()
        {
            IncrementEnumeratorCount();

            return new Enumerator(this);
        }

        public sealed class Enumerator : Enumerator<TItems, ISimpleLinkedListNode<TItems>, EnumerableSimpleLinkedList<TItems, TList>>, IUIntCountableEnumerator<TItems>
        {
            protected override TItems CurrentOverride => CurrentNode.Value;

            protected override ISimpleLinkedListNode<TItems> FirstNode => List.FirstNode;
            protected override ISimpleLinkedListNode<TItems> NextNode => CurrentNode.Next;

            public Enumerator(in EnumerableSimpleLinkedList<TItems, TList> list) : base(list) { /* Left empty. */ }
        }
    }
}