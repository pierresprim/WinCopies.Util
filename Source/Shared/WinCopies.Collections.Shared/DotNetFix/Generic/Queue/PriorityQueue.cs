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

#if CS5
using System;
using System.Collections.Generic;
using System.Threading;

using WinCopies.Util;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public class PriorityQueue<TKey, TValue> : IQueue<KeyValuePair<TKey, TValue>>
    {
        private object _syncRoot;

        protected SortedLinkedDictionary<TKey, KeyValuePair<ILinkedListNode<TValue>, ILinkedListNode<TValue>>> InnerDictionary { get; } = new SortedLinkedDictionary<TKey, KeyValuePair<ILinkedListNode<TValue>, ILinkedListNode<TValue>>>(true);

        protected ILinkedList<TValue> InnerLinkedList { get; } = new LinkedList<TValue>();

        object ISimpleLinkedListBase2.SyncRoot => _syncRoot
#if CS8
            ??=
#else
            ?? (_syncRoot =
#endif
            Interlocked.CompareExchange(ref _syncRoot, new object(), null)
#if !CS8
            )
#endif
            ;

        bool ISimpleLinkedListBase2.IsSynchronized => false;

        public uint Count => InnerLinkedList.AsFromType<IUIntCountable>().Count;

        public bool IsReadOnly => false;

        public bool HasItems => Count > 0;

        public bool TryPeek(out KeyValuePair<TKey, TValue> result)
        {
            if (InnerDictionary.Count == 0u)
            {
                result = default;

                return false;
            }

            result = new KeyValuePair<TKey, TValue>(InnerDictionary.First.Value.Key, InnerDictionary.First.Value.Value.Key.Value);

            return true;
        }

        public bool TryPeek(out object result)
        {
            if (TryPeek(out KeyValuePair<TKey, TValue> _result))
            {
                result = _result;

                return true;
            }

            result = null;

            return false;
        }

        protected KeyValuePair<TKey, TValue> PeekOrDequeue(in FuncOut<KeyValuePair<TKey, TValue>, bool> func) => (func ?? throw WinCopies.ThrowHelper.GetArgumentNullException(nameof(func)))(out KeyValuePair<TKey, TValue> result) ? result : throw new InvalidOperationException("No item.");

        public KeyValuePair<TKey, TValue> Peek() => PeekOrDequeue(TryPeek);

        object IPeekable.Peek() => Peek();

        public void Enqueue(KeyValuePair<TKey, TValue> item)
        {
            ILinkedListNode<TValue> node;

            KeyValuePair<ILinkedListNode<TValue>, ILinkedListNode<TValue>> getKey() => new
#if !CS9
                KeyValuePair<ILinkedListNode<TValue>, ILinkedListNode<TValue>>
#endif
                (node, node);

            void addLast()
            {
                node = InnerLinkedList.AddLast(item.Value);

                InnerDictionary.Add(item.Key, getKey());
            }

            if (InnerDictionary.Count == 0u)
            {
                addLast();

                return;
            }

            if (InnerDictionary.TryGetValue(item.Key, out KeyValuePair<ILinkedListNode<TValue>, ILinkedListNode<TValue>> nodes))
            {
                InnerDictionary[item.Key] = new KeyValuePair<ILinkedListNode<TValue>, ILinkedListNode<TValue>>(nodes.Key, InnerLinkedList.AddAfter(nodes.Value, item.Value));

                return;
            }

            if (!InnerDictionary.TryAddBeforeNearest(item.Key, nearestFound =>
            {
                node = InnerLinkedList.AddBefore(nearestFound.Value.Key, item.Value);

                return getKey();
            }, out _))

                addLast();
        }

        public bool TryDequeue(out KeyValuePair<TKey, TValue> result)
        {
            if (TryPeek(out result))
            {
                ILinkedListNodeBase<KeyValuePair<TKey, KeyValuePair<ILinkedListNode<TValue>, ILinkedListNode<TValue>>>> first = InnerDictionary.First;

                KeyValuePair<ILinkedListNode<TValue>, ILinkedListNode<TValue>> nodes = first.Value.Value;

                ILinkedListNode<TValue> keyNode = nodes.Key;

                if (nodes.Key == nodes.Value)

                    _ = InnerDictionary.RemoveFirst();

                else

                    first.Value = new KeyValuePair<TKey, KeyValuePair<ILinkedListNode<TValue>, ILinkedListNode<TValue>>>(first.Value.Key, new KeyValuePair<ILinkedListNode<TValue>, ILinkedListNode<TValue>>(keyNode.AsFromType<DotNetFix.IReadOnlyLinkedListNode<ILinkedListNode<TValue>>>().Next, nodes.Value));

                InnerLinkedList.Remove(keyNode);

                return true;
            }

            return false;
        }

        public KeyValuePair<TKey, TValue> Dequeue() => PeekOrDequeue(TryDequeue);

        public void Clear()
        {
            InnerLinkedList.AsFromType<ICollection<TValue>>().Clear();

            InnerDictionary.Clear();
        }

        void IQueueCore.Enqueue(object item) => Enqueue((KeyValuePair<TKey, TValue>)item);
        object IQueueCore.Dequeue() => Dequeue();
        public bool TryDequeue(out object result) => UtilHelpers.TryGetValue<KeyValuePair<TKey, TValue>>(TryDequeue, out result);
#if !CS8
        void IListCommon<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> value) => Enqueue(value);
        KeyValuePair<TKey, TValue> IListCommon<KeyValuePair<TKey, TValue>>.Remove() => Dequeue();
        bool IListCommon<KeyValuePair<TKey, TValue>>.TryRemove(out KeyValuePair<TKey, TValue> result) => TryDequeue(out result);
#endif
        void IListCommon.Add(object value) => this.AsFromType<IQueueCore>().Enqueue(value);
        object IListCommon.Remove() => Dequeue();
        bool IListCommon.TryRemove(out object result) => TryDequeue(out result);
    }
}
#endif
