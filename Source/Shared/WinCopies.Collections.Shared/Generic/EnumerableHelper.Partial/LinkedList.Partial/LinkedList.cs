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

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections
{
    public static partial class EnumerableHelper
    {
        internal abstract class LinkedListBase : ISimpleLinkedListBase
        {
            public abstract bool HasItems { get; }

            bool ISimpleLinkedListCore.IsReadOnly => false;

            public abstract void RemoveFirst();

            public void Clear()
            {
                while (HasItems)

                    RemoveFirst();
            }
        }

        internal class LinkedListBase<TNode> : LinkedListBase, _ILinkedListBase<TNode> where TNode : class, ILinkedListNode<TNode>
        {
            private protected TNode FirstNode { get; set; }
            private protected TNode LastNode { get; set; }

            public override bool HasItems => FirstNode != null;

            public override void RemoveFirst()
            {
                TNode node = (FirstNode ?? throw GetEmptyListOrCollectionException()).Next;

                FirstNode.Next = default;

                FirstNode = node;

                if (node == null)

                    LastNode = default;

                else

                    FirstNode.Previous = default;
            }
            public void RemoveLast()
            {
                TNode node = (LastNode ?? throw GetEmptyListOrCollectionException()).Previous;

                LastNode.Previous = default;

                LastNode = node;

                if (LastNode == null)

                    FirstNode = default;

                else

                    LastNode.Next = default;
            }

            void _ILinkedListBase<TNode>.Remove(ILinkedListNode<TNode> node)
            {
                if (node.Previous == null)

                    FirstNode = node.Next;

                else

                    node.Previous.Next = node.Next;

                if (node.Next == null)

                    LastNode = node.Previous;

                else

                    node.Next.Previous = node.Previous;
            }
        }

        internal partial class LinkedList : LinkedListBase<LinkedList.Node>, ILinkedList, _ILinkedListBase<LinkedList.Node>
        {
            public object First => (FirstNode ?? throw GetEmptyListOrCollectionException()).Value;
            public object Last => (LastNode ?? throw GetEmptyListOrCollectionException()).Value;

            public bool TryGetFirst(out object result)
            {
                if (FirstNode == null)
                {
                    result = default;

                    return false;
                }

                result = FirstNode.Value;

                return true;
            }

            public bool TryGetLast(out object result)
            {
                if (LastNode == null)
                {
                    result = default;

                    return false;
                }

                result = LastNode.Value;

                return true;
            }

            private Node GetNode(in object value) => new
#if !CS9
                    Node
#endif
                (this, value);

            public void AddFirst(object item)
            {
                FirstNode = FirstNode == null ? GetNode(item) : (FirstNode.Previous = new Node(this, item) { Next = FirstNode });

                if (LastNode == null)

                    LastNode = FirstNode;
            }

            public void AddLast(object item)
            {
                LastNode = LastNode == null ? GetNode(item) : (LastNode.Next = new Node(this, item) { Previous = LastNode });

                if (FirstNode == null)

                    FirstNode = LastNode;
            }

            public object GetAndRemoveFirst()
            {
                object value = First;

                RemoveFirst();

                return value;
            }

            public bool TryGetAndRemoveFirst(out object
#if CS8
                ?
#endif
                result)
            {
                if (FirstNode == null)
                {
                    result = default;

                    return false;
                }

                result = GetAndRemoveFirst();

                return true;
            }

            public object GetAndRemoveLast()
            {
                object value = Last;

                RemoveLast();

                return value;
            }

            public bool TryGetAndRemoveLast(out object
#if CS8
                ?
#endif
                result)
            {
                if (LastNode == null)
                {
                    result = default;

                    return false;
                }

                result = GetAndRemoveLast();

                return true;
            }

            #region IQueueCore Implementation
            void IQueueCore.Enqueue(object
#if CS8
                ?
#endif
                item) => AddLast(item);

            object IQueueCore.Dequeue() => GetAndRemoveFirst();

            bool IQueueCore.TryDequeue(out object result) => TryGetAndRemoveFirst(out result);
            #endregion IQueueCore Implementation
            #region IStackCore Implementation
            void IStackCore.Push(object
#if CS8
                ?
#endif
                item) => AddFirst(item);

            object IStackCore.Pop() => GetAndRemoveLast();

            bool IStackCore.TryPop(out object result) => TryGetAndRemoveFirst(out result);
            #endregion IStackCore Implementation
#if !CS8
            object IPeekable.Peek() => First;
            bool IPeekable.TryPeek(out object value) => TryGetFirst(out value);
#endif
        }
    }

    namespace Generic
    {
        public static partial class EnumerableHelper<T>
        {
            internal partial class LinkedList : EnumerableHelper.LinkedListBase<LinkedList.Node>, ILinkedList, EnumerableHelper._ILinkedListBase<LinkedList.Node>
            {
                public T
#if CS9
                    ?
#endif
                    First => (FirstNode ?? throw GetEmptyListOrCollectionException()).Value;
                public T
#if CS9
                    ?
#endif
                    Last => (LastNode ?? throw GetEmptyListOrCollectionException()).Value;

                public bool TryGetFirst(out T
#if CS9
                    ?
#endif
                    result)
                {
                    if (FirstNode == null)
                    {
                        result = default;

                        return false;
                    }

                    result = FirstNode.Value;

                    return true;
                }
                public bool TryGetLast(out T
#if CS9
                    ?
#endif
                    result)
                {
                    if (LastNode == null)
                    {
                        result = default;

                        return false;
                    }

                    result = LastNode.Value;

                    return true;
                }

                private Node GetNode(in T
#if CS9
                    ?
#endif
                    value) => new
#if !CS9
                    Node
#endif
                    (this, value);

                public void AddFirst(T
#if CS9
                    ?
#endif
                    item)
                {
                    FirstNode = FirstNode == null ? GetNode(item) : (FirstNode.Previous = new Node(this, item) { Next = FirstNode });

                    if (LastNode == null)

                        LastNode = FirstNode;
                }
                public void AddLast(T
#if CS9
                    ?
#endif
                    item)
                {
                    LastNode = LastNode == null ? GetNode(item) : (LastNode.Next = new Node(this, item) { Previous = LastNode });

                    if (FirstNode == null)

                        FirstNode = LastNode;
                }

                public T
#if CS9
                    ?
#endif
                    GetAndRemoveFirst()
                {
                    T value = First;

                    RemoveFirst();

                    return value;
                }
                public bool TryGetAndRemoveFirst(out T
#if CS9
                    ?
#endif
                    result)
                {
                    if (FirstNode == null)
                    {
                        result = default;

                        return false;
                    }

                    result = GetAndRemoveFirst();

                    return true;
                }

                public T
#if CS9
                    ?
#endif
                    GetAndRemoveLast()
                {
                    T value = Last;

                    RemoveLast();

                    return value;
                }
                public bool TryGetAndRemoveLast(out T
#if CS9
                    ?
#endif
                    result)
                {
                    if (LastNode == null)
                    {
                        result = default;

                        return false;
                    }

                    result = GetAndRemoveLast();

                    return true;
                }

                public IQueueCommon<T> AsQueue() => new Abstraction.Generic.EnumerableHelper<T>.Queue(this);
                public IStackCommon<T> AsStack() => new Abstraction.Generic.EnumerableHelper<T>.Stack(this);

                void IListCommon<T>.Add(T
#if CS9
                    ?
#endif
                    value) => AddLast(value);
                T
#if CS9
                   ?
#endif
                   IListCommon<T>.Remove() => GetAndRemoveFirst();
                bool IListCommon<T>.TryRemove(out T
#if CS9
                    ?
#endif
                    result) => TryGetAndRemoveFirst(out result);

                #region Implementations
                #region IQueueCore
                public void Enqueue(T
#if CS9
                    ?
#endif
                    item) => AddLast(item);
                public T Dequeue() => GetAndRemoveFirst();
                public bool TryDequeue(out T
#if CS9
                    ?
#endif
                    result) => TryGetAndRemoveFirst(out result);
                #endregion IQueueCore
                #region IStackCore
                public void Push(T
#if CS9
                    ?
#endif
                    item) => AddFirst(item);
                public T
#if CS9
                    ?
#endif
                    Pop() => GetAndRemoveFirst();
                public bool TryPop(out T
#if CS9
                    ?
#endif
                    result) => TryGetAndRemoveFirst(out result);
                #endregion IStackCore
                #endregion Implementations
#if !CS8
                #region Implementations
                #region IQueueCore Implementation
                void IQueueCore.Enqueue(object item) => Enqueue((T)item);
                object IQueueCore.Dequeue() => Dequeue();
                bool IQueueCore.TryDequeue(out object result) => UtilHelpers.TryGetValue<T>(TryDequeue, out result);
                #endregion IQueueCore Implementation
                #region IStackCore Implementation
                void IStackCore.Push(object item) => Push((T)item);
                object IStackCore.Pop() => Pop();
                bool IStackCore.TryPop(out object result) => UtilHelpers.TryGetValue<T>(TryPop, out result);
                #endregion IStackCore Implementation
                #endregion Implementations

                public T Peek() => First;
                public bool TryPeek(out T value) => TryGetFirst(out value);
                
                object IPeekable.Peek() => Peek();
                bool IPeekable.TryPeek(out object result) => UtilHelpers.TryGetValue<T>(TryPeek, out result);

                void IListCommon.Add(object value) => AddLast((T)value);
                object IListCommon.Remove() => GetAndRemoveFirst();
                bool IListCommon.TryRemove(out object result) => UtilHelpers.TryGetValue<T>(TryGetAndRemoveFirst, out result);
#endif
            }
        }
    }
}
