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

namespace WinCopies.Collections.DotNetFix.Generic
{
    public interface IEnumerableQueue<T> : IQueue<T>, IEnumerableSimpleLinkedList<T>
    {
        // Left empty.
    }

    [Serializable]
    public class EnumerableQueue<T> : EnumerableSimpleLinkedList<T, Queue<T>>, IEnumerableQueue<T>
    {
        protected override ISimpleLinkedListNode<T>
#if CS8
            ?
#endif
            FirstNode => List.FirstItem;

        public EnumerableQueue() : base(new Queue<T>()) { /* Left empty. */ }

        public void Enqueue(T
#if CS9
            ?
#endif
            item) => Add(item);
        public T
#if CS9
            ?
#endif
            Dequeue() => Remove();
        public bool TryDequeue(out T
#if CS9
            ?
#endif
            result) => TryRemove(out result);
#if !CS8
        void IQueueCore.Enqueue(object item) => Enqueue((T)item);
        object IQueueCore.Dequeue() => Dequeue();
        public bool TryDequeue(out object result) => UtilHelpers.TryGetValue<T>(TryDequeue, out result);
#endif
    }
}
