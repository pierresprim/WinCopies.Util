/* Copyright © Pierre Sprimont, 2021
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

using WinCopies.Collections.DotNetFix.Generic;

using static WinCopies.Collections.ThrowHelper;

#if WinCopies3
using WinCopies.Collections.DotNetFix;
#endif

namespace WinCopies.Collections.AbstractionInterop.Generic
{
    public static partial class AbstractionTypes<TSource, TDestination> where TSource : TDestination
    {
        public class ReadOnlyQueue<TQueue> : IQueue<TDestination> where TQueue : IQueue<TSource>
        {
            protected TQueue InnerQueue { get; }

#if WinCopies3
            object ISimpleLinkedListBase2.SyncRoot => InnerQueue.SyncRoot;

            bool ISimpleLinkedListBase2.IsSynchronized => InnerQueue.IsSynchronized;

            bool ISimpleLinkedListBase.IsReadOnly => true;

            bool ISimpleLinkedListBase.HasItems => InnerQueue.HasItems;

            bool IQueue<TDestination>.TryPeek(out TDestination result) => TryPeek(out result);

            void IQueue<TDestination>.Clear() => throw GetReadOnlyListOrCollectionException();

            void ISimpleLinkedListBase2.Clear() => throw GetReadOnlyListOrCollectionException();

            TDestination ISimpleLinkedListBase<TDestination>.Peek() => InnerQueue.Peek();

            bool ISimpleLinkedListBase<TDestination>.TryPeek(out TDestination result) => TryPeek(out result);

            void IQueueBase<TDestination>.Enqueue(TDestination item) => throw GetReadOnlyListOrCollectionException();

            TDestination IQueueBase<TDestination>.Dequeue() => throw GetReadOnlyListOrCollectionException();

            bool IQueueBase<TDestination>.TryDequeue(out TDestination result) => throw GetReadOnlyListOrCollectionException();

            TDestination IQueueBase<TDestination>.Peek() => InnerQueue.Peek();

            bool IQueueBase<TDestination>.TryPeek(out TDestination result) => TryPeek(out result);

            void IQueueBase<TDestination>.Clear() => throw GetReadOnlyListOrCollectionException();

            TDestination IQueue<TDestination>.Peek() => InnerQueue.Peek();

            private bool TryPeek(out TDestination result)
            {
                if (InnerQueue.TryPeek(out TSource _result))
                {
                    result = _result;

                    return true;
                }

                result = default;

                return false;
            }

            TDestination ISimpleLinkedList<TDestination>.Peek() => ((ISimpleLinkedList<TDestination>)InnerQueue).Peek();

#if !CS8
            bool ISimpleLinkedList.TryPeek(out object result) => InnerQueue.TryPeek(out result);

            object ISimpleLinkedList.Peek() => ((ISimpleLinkedList)InnerQueue).Peek();
#endif
#else
            public void Enqueue(TDestination item) => throw GetReadOnlyListOrCollectionException();

            public TDestination Dequeue() => throw GetReadOnlyListOrCollectionException();

            public TDestination Peek() => throw GetReadOnlyListOrCollectionException();
#endif

            uint IUIntCountable.Count => InnerQueue.Count;

            public ReadOnlyQueue(in TQueue queue) => InnerQueue = queue;
        }

        public class ReadOnlyQueue : ReadOnlyQueue<IQueue<TSource>>
        {
            public ReadOnlyQueue(in IQueue<TSource> queue) : base(queue)
            {
                // Left empty.
            }
        }
    }
}
