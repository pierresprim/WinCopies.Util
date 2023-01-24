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

using System.Collections;

using WinCopies.Util;

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public interface ISimpleLinkedList<T> : IPeekable<T>, ISimpleLinkedList
    {
        // Left empty.
    }

    public abstract class SimpleLinkedList<TNode, TValue> : SimpleLinkedListBase<TNode>, ISimpleLinkedList<TValue>, IListCommon where TNode : ISimpleLinkedListNode<TNode, TValue>
    {
        private TValue OnRemove()
        {
            TNode firstItem = FirstItem;

            TValue result = firstItem.Value;

            Decrement();

            firstItem.Clear();

            return result;
        }

        protected TValue PeekItem() => FirstItem.Value;

        protected abstract TNode AddItem(TValue item, out bool actionAfter);

        public TValue Peek() => HasItems ? PeekItem() : throw GetEmptyListOrCollectionException();

        public bool TryPeek(out TValue
#if CS9
            ?
#endif
            result)
        {
            if (HasItems)
            {
                result = PeekItem();

                return true;
            }

            result = default;

            return false;
        }

        public void Add(TValue
#if CS9
            ?
#endif
            item)
        {
            FirstItem = IsReadOnly ? throw GetReadOnlyListOrCollectionException() : AddItem(item, out bool actionAfter);

            Increment(actionAfter);
        }

        public TValue Remove()
        {
            ThrowIfEmptyListOrCollection(IsReadOnly ? throw GetReadOnlyListOrCollectionException() : this);

            return OnRemove();
        }

        public bool TryRemove(out TValue
#if CS9
            ?
#endif
            result)
        {
            if (IsReadOnly || Count == 0)
            {
                result = default;

                return false;
            }

            result = OnRemove();

            return true;
        }

        public void Add(object value) => Add((TValue)value);
        object IListCommon.Remove() => Remove();
        public bool TryRemove(out object result) => UtilHelpers.TryGetValue<TValue>(TryRemove, out result);
#if !CS8
            object IPeekable.Peek() => Peek();

            bool IPeekable.TryPeek(out object result) => UtilHelpers.TryGetValue<TValue>(TryPeek, out result);
#endif
    }

    public abstract class SimpleLinkedList<T> : SimpleLinkedList<SimpleLinkedListNode<T>, T>
    {
        // Left empty.
    }

    /*public interface ILinkedListNode<T>
    {
        ILinkedList<T> List { get; }

        ILinkedListNode<T> Next { get; }

        ILinkedListNode<T> Previous { get; }

        T Value { get; set; }
    }

    public interface ILinkedList<T> : System.Collections.Generic.ICollection<T>, System.Collections.Generic.IEnumerable<T>, IEnumerable, System.Collections.Generic.IReadOnlyCollection<T>, ICollection, IDeserializationCallback, ISerializable
    {
        ILinkedListNode<T> Last { get; }

        ILinkedListNode<T> First { get; }

        int Count { get; }

        void AddAfter(ILinkedListNode<T> node, ILinkedListNode<T> newNode);

        ILinkedListNode<T> AddAfter(ILinkedListNode<T> node, T value);

        void AddBefore(ILinkedListNode<T> node, ILinkedListNode<T> newNode);

        ILinkedListNode<T> AddBefore(ILinkedListNode<T> node, T value);

        void AddFirst(ILinkedListNode<T> node);

        ILinkedListNode<T> AddFirst(T value);

        void AddLast(ILinkedListNode<T> node);

        ILinkedListNode<T> AddLast(T value);

        void Clear();

        bool Contains(T value);

        void CopyTo(T[] array, int index);

        ILinkedListNode<T> Find(T value);

        ILinkedListNode<T> FindLast(T value);

        void Remove(ILinkedListNode<T> node);

        bool Remove(T value);

        void RemoveFirst();

        void RemoveLast();
    }*/

    public abstract class ReadOnlySimpleLinkedListBase<T> : ReadOnlySimpleLinkedListCore, ISimpleLinkedList<T>
    {
        public abstract T Peek();
        public abstract bool TryPeek(out T result);
#if !CS8
            object IPeekable.Peek() => Peek();

            bool IPeekable.TryPeek(out object result)
            {
                if (TryPeek(out T _result))
                {
                    result = _result;

                    return true;
                }

                result = null;

                return false;
            }
#endif
    }

    public abstract class ReadOnlySimpleLinkedList<TList, TItems> : ReadOnlySimpleLinkedListBase<TItems>, IListCommon<TItems> where TList : ISimpleLinkedListCore, IUIntCountable, IPeekable<TItems>
    {
        protected TList InnerList { get; }

        public override uint Count => InnerList.AsFromType<IUIntCountable>().Count;

        public override bool HasItems => InnerList.HasItems;

        protected override object SyncRoot => null;

        protected ReadOnlySimpleLinkedList(in TList list) => InnerList = list;

        public override TItems Peek() => InnerList.Peek();
        public override bool TryPeek(out TItems result) => InnerList.TryPeek(out result);

        void IListCommon<TItems>.Add(TItems item) => throw GetReadOnlyListOrCollectionException();
        TItems IListCommon<TItems>.Remove() => throw GetReadOnlyListOrCollectionException();
        bool IListCommon<TItems>.TryRemove(out TItems result) => throw GetReadOnlyListOrCollectionException();

        void IListCommon.Add(object item) => throw GetReadOnlyListOrCollectionException();
        object IListCommon.Remove() => throw GetReadOnlyListOrCollectionException();
        bool IListCommon.TryRemove(out object result) => throw GetReadOnlyListOrCollectionException();
    }

    public interface IEnumerableSimpleLinkedList<T> : ISimpleLinkedList<T>, IUIntCountable, IEnumerableSimpleLinkedListBase, ICollection,
#if CS7
        System.Collections.Generic.IReadOnlyCollection<T>
#if CS8
        , DotNetFix.Generic.IEnumerable<T>
#endif
#else
            System.Collections.Generic.IEnumerable<T>
#endif
    {
        void CopyTo(T[] array, int index);

        T[] ToArray();
    }
}
