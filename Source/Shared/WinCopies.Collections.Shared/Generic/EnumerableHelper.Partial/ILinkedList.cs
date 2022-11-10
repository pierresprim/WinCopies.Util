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

namespace WinCopies.Collections
{
    public static partial class EnumerableHelper
    {
        public interface ILinkedListBase : IClearable
        {
            void RemoveFirst();
            void RemoveLast();
        }

        internal interface _ILinkedListBase<T> : ILinkedListBase
        {
            void Remove(ILinkedListNode<T> node);
        }

        public interface ILinkedList : ILinkedListBase, ISimpleLinkedListBase, IPeekable, ISimpleLinkedListCore, IClearable, IQueueCore, IStackCore
        {
            object First { get; }
            object Last { get; }

            bool TryGetFirst(out object result);

            bool TryGetLast(out object result);

            void AddFirst(object item);

            void AddLast(object item);

            object GetAndRemoveFirst();

            bool TryGetAndRemoveFirst(out object result);

            object GetAndRemoveLast();

            bool TryGetAndRemoveLast(out object result);
#if CS8
            object IPeekable.Peek() => First;
            bool IPeekable.TryPeek(out object value) => TryGetFirst(out value);
#endif
        }
    }

    namespace Generic
    {
        public static partial class EnumerableHelper<T>
        {
            public interface ILinkedList : EnumerableHelper.ILinkedListBase, ISimpleLinkedListBase, ISimpleLinkedListCore, IClearable, IQueueCommon<T>, IStackCore<T>
            {
                T
#if CS9
                    ?
#endif
                    First
                { get; }
                T
#if CS9
                    ?
#endif
                    Last
                { get; }

                bool TryGetFirst(out T
#if CS9
                    ?
#endif
                    result);
                bool TryGetLast(out T
#if CS9
                    ?
#endif
                    result);

                void AddFirst(T
#if CS9
                    ?
#endif
                    item);
                void AddLast(T
#if CS9
                    ?
#endif
                    item);

                T
#if CS9
                    ?
#endif
                    GetAndRemoveFirst();
                bool TryGetAndRemoveFirst(out T
#if CS9
                    ?
#endif
                    result);

                T
#if CS9
                    ?
#endif
                    GetAndRemoveLast();
                bool TryGetAndRemoveLast(out T
#if CS9
                    ?
#endif
                    result);

                IQueueCommon<T> AsQueue();
                IStackCommon<T> AsStack();
#if CS8
                #region IQueueCore Implementation
                void IQueueCore<T>.Enqueue(T
#if CS9
                    ?
#endif
                    item) => AddLast(item);

                T
#if CS9
                    ?
#endif
                    IQueueCore<T>.Dequeue() => GetAndRemoveFirst();

                bool IQueueCore<T>.TryDequeue(out T
#if CS9
                    ?
#endif
                    result) => TryGetAndRemoveFirst(out result);
                #endregion IQueueCore Implementation
                #region IStackCore Implementation
                void IStackCore<T>.Push(T
#if CS9
                    ?
#endif
                    item) => AddFirst(item);

                T
#if CS9
                    ?
#endif
                    IStackCore<T>.Pop() => GetAndRemoveLast();

                bool IStackCore<T>.TryPop(out T
#if CS9
                    ?
#endif
                    result) => TryGetAndRemoveFirst(out result);
                #endregion IStackCore Implementation

                T
#if CS9
                    ?
#endif
                    IPeekable<T>.Peek() => First;
                bool IPeekable<T>.TryPeek(out T
#if CS9
                    ?
#endif
                    value) => TryGetFirst(out value);
#endif
            }
        }
    }
}
