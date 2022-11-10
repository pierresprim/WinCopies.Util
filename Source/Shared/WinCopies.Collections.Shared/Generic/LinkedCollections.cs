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
using System.Collections;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Util;

namespace WinCopies.Collections.Generic
{
#if !CS8
    internal static class SimpleLinkedListHelper
    {
        internal static bool TryGetValue<T>(this ICollection collection, in System.Func<T> func, out T result)
        {
            if (collection.Count > 0)
            {
                result = func();

                return true;
            }

            result = default;

            return false;
        }

        internal static bool TryPeekItem<TItems, TCollection>(this TCollection collection, out TItems result) where TCollection : IPeekable<TItems>, ICollection => collection.TryGetValue(collection.Peek, out result);
        internal static bool TryPeekValue<T>(this IPeekable<T> collection, out object result) => UtilHelpers.TryGetValue<T>(collection.TryPeek, out result);
    }
#endif
    public class Stack<T> : System.Collections.Generic.Stack<T>, IEnumerableStack<T>
    {
        protected ICollection AsCollection => this;

        public bool IsReadOnly => false;

        bool ISimpleLinkedListBase2.IsSynchronized => AsCollection.IsSynchronized;
        object ISimpleLinkedListBase2.SyncRoot => AsCollection.SyncRoot;

        uint IUIntCountable.Count => (uint)Count;

        public bool HasItems => Count != 0;

        void IListCommon.Add(object value) => Push((T)value);
        object IListCommon.Remove() => Pop();
        bool IListCommon.TryRemove(out object result) => UtilHelpers.TryGetValue<T>(TryPop, out result);
#if !CS8
        public bool TryPeek(out T result) => this.TryPeekItem(out result);
        public bool TryPop(out T result) => this.TryGetValue(Pop, out result);

        object IPeekable.Peek() => Peek();
        public bool TryPeek(out object result) => this.TryPeekValue(out result);

        void IStackCore.Push(object item) => Push((T)item);
        object IStackCore.Pop() => Pop();
        public bool TryPop(out object result) => UtilHelpers.TryGetValue<T>(TryPop, out result);

        void IListCommon<T>.Add(T value) => Push(value);
        T IListCommon<T>.Remove() => Pop();
        bool IListCommon<T>.TryRemove(out T result) => TryPop(out result);
#endif
    }

    public class Queue<T> : System.Collections.Generic.Queue<T>, IEnumerableQueue<T>
    {
        protected ICollection AsCollection => this;

        public bool IsReadOnly => false;

        bool ISimpleLinkedListBase2.IsSynchronized => AsCollection.IsSynchronized;
        object ISimpleLinkedListBase2.SyncRoot => AsCollection.SyncRoot;

        uint IUIntCountable.Count => (uint)Count;

        public bool HasItems => Count != 0;

        void IListCommon.Add(object value) => Enqueue((T)value);
        object IListCommon.Remove() => Dequeue();
        bool IListCommon.TryRemove(out object result) => UtilHelpers.TryGetValue<T>(TryDequeue, out result);
#if !CS8
        public bool TryPeek(out T result) => this.TryPeekItem(out result);
        public bool TryDequeue(out T result) => this.TryGetValue(Dequeue, out result);

        object IPeekable.Peek() => Peek();
        public bool TryPeek(out object result) => this.TryPeekValue(out result);

        void IQueueCore.Enqueue(object item) => Enqueue((T)item);
        object IQueueCore.Dequeue() => Dequeue();
        public bool TryDequeue(out object result) => UtilHelpers.TryGetValue<T>(TryDequeue, out result);

        void IListCommon<T>.Add(T value) => Enqueue(value);
        T IListCommon<T>.Remove() => Dequeue();
        bool IListCommon<T>.TryRemove(out T result) => TryDequeue(out result);
#endif
    }
}
#endif
