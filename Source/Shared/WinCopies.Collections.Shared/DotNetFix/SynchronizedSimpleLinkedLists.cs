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

namespace WinCopies.Collections.DotNetFix
{
#if WinCopies3
    public class SynchronizedSimpleLinkedList<T> : ISimpleLinkedList, ISimpleLinkedListCommon where T : ISimpleLinkedList, ISimpleLinkedListCommon
    {
        private readonly T _list;

        protected T List
        {
            get
            {
                lock (SyncRoot)

                    return _list;
            }
        }

        public bool IsReadOnly => false;

        public bool HasItems => List.HasItems;

        public bool IsSynchronized => true;
        public object SyncRoot => _list.SyncRoot;

        public uint Count => List.Count;

        public SynchronizedSimpleLinkedList(in T list) => _list = list;

        public object
#if CS8
            ?
#endif
            Peek() => List.Peek();

        public bool TryPeek(out object
#if CS8
            ?
#endif
            result) => List.TryPeek(out result);

        public void Add(object
#if CS8
            ?
#endif
            value) => List.Add(value);
        public object
#if CS8
            ?
#endif
            Remove() => List.Remove();
        public bool TryRemove(out object
#if CS8
            ?
#endif
            result) => List.TryRemove(out result);

        public void Clear() => List.Clear();
    }

    public class SynchronizedQueue : SynchronizedSimpleLinkedList<IQueue>, IQueue
    {
        public SynchronizedQueue(in IQueue queue) : base(queue) { /* Left empty. */ }

        public void Enqueue(object
#if CS8
            ?
#endif
            value) => Add(value);

        public object
#if CS8
            ?
#endif
            Dequeue() => Remove();

        public bool TryDequeue(out object
#if CS8
            ?
#endif
            result) => TryRemove(out result);
    }

    public class SynchronizedStack : SynchronizedSimpleLinkedList<IStack>, IStack
    {
        public SynchronizedStack(in IStack stack) : base(stack) { /* Left empty. */ }

        public void Push(object
#if CS8
            ?
#endif
            value) => Add(value);

        public object
#if CS8
            ?
#endif
            Pop() => Remove();

        public bool TryPop(out object
#if CS8
            ?
#endif
            result) => TryRemove(out result);
    }
#endif
}
