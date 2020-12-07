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

#if !WinCopies2

using WinCopies.Collections.DotNetFix.Generic;

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.Generic
{
    public static partial class EnumerableHelper<T>
    {
        internal partial class LinkedList : ILinkedList
        {
            private protected Node FirstNode { get; private set; }

            public T First => (FirstNode ?? throw GetEmptyListOrCollectionException()).Value;

            private protected Node LastNode { get; private set; }

            public T Last => (LastNode ?? throw GetEmptyListOrCollectionException()).Value;

            public bool HasItems => FirstNode != null;

            public bool SupportsReversedEnumeration => true;

            public bool TryGetFirst(out T result)
            {
                if (FirstNode == null)
                {
                    result = default;

                    return false;
                }

                result = FirstNode.Value;

                return true;
            }

            public bool TryGetLast(out T result)
            {
                if (LastNode == null)
                {
                    result = default;

                    return false;
                }

                result = LastNode.Value;

                return true;
            }

            public void AddFirst(T item)
            {
                FirstNode = (FirstNode.Previous = new Node(item));

                if (LastNode == null)

                    LastNode = FirstNode;
            }

            public void AddLast(T item)
            {
                LastNode = (LastNode.Next = new Node(item));

                if (FirstNode == null)

                    FirstNode = LastNode;
            }

            public void RemoveFirst()
            {
                (FirstNode = (FirstNode ?? throw GetEmptyListOrCollectionException()).Next).Previous = null;

                if (FirstNode == null)

                    LastNode = null;
            }

            public T GetAndRemoveFirst()
            {
                T value = First;

                RemoveFirst();

                return value;
            }

            public bool TryGetAndRemoveFirst(out T result)
            {
                if (FirstNode == null)
                {
                    result = default;

                    return false;
                }

                result = GetAndRemoveFirst();

                return true;
            }

            public void RemoveLast()
            {
                (LastNode = (LastNode ?? throw GetEmptyListOrCollectionException()).Previous).Next = null;

                if (LastNode == null)

                    FirstNode = null;
            }

            public T GetAndRemoveLast()
            {
                T value = Last;

                RemoveLast();

                return value;
            }

            public bool TryGetAndRemoveLast(out T result)
            {
                if (LastNode == null)
                {
                    result = default;

                    return false;
                }

                result = GetAndRemoveLast();

                return true;
            }

            public void Clear()
            {
                while (HasItems)

                    RemoveFirst();
            }

            #region IQueueBase implementation

            T IQueueBase<T>.Peek() => First;

            bool IQueueBase<T>.TryPeek(out T result) => TryGetFirst(out result);

            void IQueueBase<T>.Enqueue(T item) => AddLast(item);

            T IQueueBase<T>.Dequeue() => GetAndRemoveFirst();

            bool IQueueBase<T>.TryDequeue(out T result) => TryGetAndRemoveFirst(out result);

            #endregion

            #region IStackBase implementation

            T IStackBase<T>.Peek() => Last;

            bool IStackBase<T>.TryPeek(out T result) => TryGetLast(out result);

            void IStackBase<T>.Push(T item) => AddFirst(item);

            T IStackBase<T>.Pop() => GetAndRemoveLast();

            bool IStackBase<T>.TryPop(out T result) => TryGetAndRemoveFirst(out result);

            #endregion
        }
    }
}

#endif
