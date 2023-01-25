/* Copyright © Pierre Sprimont, 2022
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

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;

namespace WinCopies.Collections.Abstraction.Generic
{
#if CS7
    public abstract class SimpleLinkedListCommon<TItems, TList> : IUIntCountable, IClearable where TList : IUIntCountable, IClearable
    {
        protected TList InnerList { get; }

        public uint Count => InnerList.Count;

        protected SimpleLinkedListCommon(in TList list) => InnerList = list;

        public void Clear() => InnerList.Clear();
    }
    public abstract class SimpleLinkedListBase<TItems, TList> : SimpleLinkedListCommon<TItems, TList>, IListCommon<TItems>, IPeekable<TItems> where TList : IUIntCountable, IPeekable<TItems>, IClearable
    {
        protected SimpleLinkedListBase(in TList list) : base(list) { /* Left empty. */ }

        public TItems
#if CS9
            ?
#endif
            Peek() => InnerList.Peek();
        public bool TryPeek(out TItems
#if CS9
            ?
#endif
            result) => InnerList.TryPeek(out result);

        public abstract void Add(TItems value);
        public abstract TItems Remove();
        public abstract bool TryRemove(out TItems result);

        void IListCommon.Add(object value) => Add((TItems)value);
        object IListCommon.Remove() => Remove();
        public bool TryRemove(out object result) => UtilHelpers.TryGetValue<TItems>(TryRemove, out result);
#if !CS8
        object IPeekable.Peek() => Peek();
        public bool TryPeek(out object result) => TryPeek(out result);
#endif
    }

    public abstract class SimpleLinkedList<TItems, TList> : SimpleLinkedListBase<TItems, TList> where TList : ISimpleLinkedListCommon<TItems>, IPeekable<TItems>, ISimpleLinkedListBase2
    {
        //protected ISimpleLinkedListCore InnerSimpleLinkedList;

        public object SyncRoot => InnerList.SyncRoot;
        public bool IsSynchronized => InnerList.IsSynchronized;

        public bool IsReadOnly => InnerList.IsReadOnly;

        public bool HasItems => InnerList.HasItems;

        protected SimpleLinkedList(in TList list) : base(list) { /* Left empty. */ }
    }

    public class Queue<TItems, TList> : SimpleLinkedList<TItems, TList>, IQueue<TItems> where TList : IQueueCommon<TItems>, ISimpleLinkedListBase2
#if CS8
        , IListCommon<TItems>
#endif
    {
        public Queue(in TList list) : base(list) { /* Left empty. */ }

        public void Enqueue(TItems
#if CS9
            ?
#endif
            item) => InnerList.Enqueue(item);
        public TItems
#if CS9
            ?
#endif
            Dequeue() => InnerList.Dequeue();
        public bool TryDequeue(out TItems
#if CS9
            ?
#endif
            result) => InnerList.TryDequeue(out result);

        public override void Add(TItems value) => Enqueue(value);
        public override TItems Remove() => Dequeue();
        public override bool TryRemove(out TItems result) => TryDequeue(out result);
#if !CS8
        void IQueueCore.Enqueue(object item) => Enqueue((TItems)item);
        object IQueueCore.Dequeue() => Dequeue();
        public bool TryDequeue(out object result) => UtilHelpers.TryGetValue<TItems>(TryDequeue, out result);
#endif
    }
    public class Stack<TItems, TList> : SimpleLinkedList<TItems, TList>, IStack<TItems> where TList : IStackCommon<TItems>, ISimpleLinkedListBase2
#if CS8
        , IListCommon<TItems>
#endif
    {
        public Stack(in TList list) : base(list) { /* Left empty. */ }

        public void Push(TItems
#if CS9
            ?
#endif
            item) => InnerList.Push(item);
        public TItems
#if CS9
            ?
#endif
            Pop() => InnerList.Pop();
        public bool TryPop(out TItems
#if CS9
            ?
#endif
            result) => InnerList.TryPop(out result);

        public override void Add(TItems value) => Push(value);
        public override TItems Remove() => Pop();
        public override bool TryRemove(out TItems result) => TryPop(out result);
#if !CS8
        void IStackCore.Push(object item) => Push((TItems)item);
        object IStackCore.Pop() => Pop();
        public bool TryPop(out object result) => UtilHelpers.TryGetValue<TItems>(TryPop, out result);
#endif
    }

    public class IndexedSimpleLinkedList<TItems, TList> : SimpleLinkedListCommon<TItems, TList> where TList : Collections.Generic.IIndexable<TItems>, IUIntCountable, IClearable
    {
        public int CurrentIndex { get; private set; }

        public IndexedSimpleLinkedList(in TList items) : base(items) { /* Left empty. */ }
    }
#endif
    public static class EnumerableHelper<T>
    {
        public abstract class SimpleLinkedList : IPeekable<T>, ISimpleLinkedListBase
        {
            protected Collections.Generic.EnumerableHelper<T>.ILinkedList InnerList { get; }

            public bool IsReadOnly => InnerList.IsReadOnly;

            public bool HasItems => InnerList.HasItems;

            protected SimpleLinkedList(in Collections.Generic.EnumerableHelper<T>.ILinkedList list) => InnerList = list;

            public T
#if CS9
                ?
#endif
                Peek() => InnerList.Peek();
            public bool TryPeek(out T
#if CS9
                ?
#endif
                result) => InnerList.TryPeek(out result);

            public void Clear() => InnerList.Clear();
#if !CS8
            object IPeekable.Peek() => InnerList.Peek();
            bool IPeekable.TryPeek(out object result) => InnerList.TryPeek(out result);
#endif
        }

        public class Queue : SimpleLinkedList, IQueueCommon<T>
        {
            public Queue(in Collections.Generic.EnumerableHelper<T>.ILinkedList list) : base(list) { /* Left empty. */ }

            void IQueueCore<T>.Enqueue(T
#if CS9
                ?
#endif
                item) => InnerList.Enqueue(item);
            T
#if CS9
                ?
#endif
                IQueueCore<T>.Dequeue() => InnerList.Dequeue();
            bool IQueueCore<T>.TryDequeue(out T
#if CS9
                ?
#endif
                result) => InnerList.TryDequeue(out result);

            void IListCommon<T>.Add(T
#if CS9
                 ?
#endif
                 value) => InnerList.Enqueue(value);
            T
#if CS9
                 ?
#endif
                 IListCommon<T>.Remove() => InnerList.Dequeue();
            bool IListCommon<T>.TryRemove(out T
#if CS9
                 ?
#endif
                 result) => InnerList.TryDequeue(out result);
#if !CS8
            void IQueueCore.Enqueue(object value) => InnerList.Enqueue(value);
            object IQueueCore.Dequeue() => InnerList.Dequeue();
            bool IQueueCore.TryDequeue(out object result) => InnerList.TryDequeue(out result);

            void IListCommon.Add(object value) => InnerList.Enqueue(value);
            object IListCommon.Remove() => InnerList.Dequeue();
            bool IListCommon.TryRemove(out object result) => InnerList.TryDequeue(out result);
#endif
        }

        public class Stack : SimpleLinkedList, IStackCommon<T>
        {
            public Stack(in Collections.Generic.EnumerableHelper<T>.ILinkedList list) : base(list) { /* Left empty. */ }

            void IStackCore<T>.Push(T
#if CS9
                ?
#endif
                item) => InnerList.Push(item);
            T
#if CS9
                ?
#endif
                IStackCore<T>.Pop() => InnerList.Pop();
            bool IStackCore<T>.TryPop(out T
#if CS9
                ?
#endif
                result) => InnerList.TryPop(out result);

            void IListCommon<T>.Add(T
#if CS9
                 ?
#endif
                 value) => InnerList.Push(value);
            T
#if CS9
                 ?
#endif
                 IListCommon<T>.Remove() => InnerList.Pop();
            bool IListCommon<T>.TryRemove(out T
#if CS9
                 ?
#endif
                 result) => InnerList.TryPop(out result);
#if !CS8
            void IStackCore.Push(object value) => InnerList.Push(value);
            object IStackCore.Pop() => InnerList.Pop();
            bool IStackCore.TryPop(out object result) => InnerList.TryPop(out result);

            void IListCommon.Add(object value) => InnerList.Push(value);
            object IListCommon.Remove() => InnerList.Pop();
            bool IListCommon.TryRemove(out object result) => InnerList.TryPop(out result);
#endif
        }
    }
}
