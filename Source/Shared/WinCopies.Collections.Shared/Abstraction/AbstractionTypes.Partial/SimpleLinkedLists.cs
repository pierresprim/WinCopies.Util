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

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;

namespace WinCopies.Collections.AbstractionInterop.Generic
{
    public static partial class AbstractionTypes<TSource, TDestination>
    {
        public class SimpleLinkedCollection<T> : SimpleLinkedCollectionBase<T>, ISimpleLinkedList<TDestination>, IListCommon<TDestination> where T : ISimpleLinkedList<TSource>, IListCommon<TSource>
        {
            public SimpleLinkedCollection(in T list) : base(list) { }

            public TDestination
#if CS9
                ?
#endif
                Peek() => InnerList.Peek();
            public bool TryPeek(out TDestination
#if CS9
                ?
#endif
                result) => UtilHelpers.TryGetValue<TSource, TDestination>(InnerList.TryPeek, out result);

            void IListCommon<TDestination>.Add(TDestination
#if CS9
                ?
#endif
                value) => InnerList.Add((TSource
#if CS9
                ?
#endif
                )value);
            TDestination
#if CS9
                ?
#endif
                IListCommon<TDestination>.Remove() => InnerList.Remove();
            public bool TryRemove(out TDestination
#if CS9
                ?
#endif
                result) => UtilHelpers.TryGetValue<TSource, TDestination>(InnerList.TryRemove, out result);

            void IListCommon.Add(object
#if CS8
                ?
#endif
                value) => InnerList.Add(value);
            object
#if CS8
                ?
#endif
                IListCommon.Remove() => InnerList.Remove();
            bool IListCommon.TryRemove(out object
#if CS8
                ?
#endif
                result) => InnerList.TryRemove(out result);
#if !CS8
            object IPeekable.Peek() => InnerList.Peek();
            public bool TryPeek(out object result) => InnerList.TryPeek(out result);
#endif
        }

        public class Queue<TQueue> : SimpleLinkedCollection<TQueue>, IQueue<TDestination> where TQueue : IQueue<TSource>
        {
            public Queue(in TQueue queue) : base(queue) { /* Left empty. */ }

            public void Enqueue(TDestination
#if CS9
                ?
#endif
                item) => InnerList.Enqueue(item);
            public TDestination
#if CS9
                ?
#endif
                Dequeue() => InnerList.Dequeue();
            public bool TryDequeue(out TDestination
#if CS9
                ?
#endif
                result) => UtilHelpers.TryGetValue<TSource, TDestination>(InnerList.TryDequeue, out result);
#if !CS8
            void IQueueCore.Enqueue(object item) => InnerList.Enqueue(item);
            object IQueueCore.Dequeue() => InnerList.Dequeue();
            public bool TryDequeue(out object result) => InnerList.TryDequeue(out result);
#endif
        }

        public class Queue : Queue<IQueue<TSource>>
        {
            public Queue(in IQueue<TSource> queue) : base(queue) { /* Left empty. */ }
        }

        public class Stack<TStack> : SimpleLinkedCollection<TStack>, IStack<TDestination> where TStack : IStack<TSource>
        {
            public Stack(in TStack stack) : base(stack) { /* Left empty. */ }

            public void Push(TDestination
#if CS9
                ?
#endif
                item) => InnerList.Push((TSource)item);
            public TDestination
#if CS9
                ?
#endif
                Pop() => InnerList.Pop();
            public bool TryPop(out TDestination
#if CS9
                ?
#endif
                result) => UtilHelpers.TryGetValue<TSource, TDestination>(InnerList.TryPop, out result);
#if !CS8
            void IStackCore.Push(object item) => InnerList.Push(item);
            object IStackCore.Pop() => InnerList.Pop();
            public bool TryPop(out object result) => InnerList.TryPop(out result);
#endif
        }

        public class Stack : Stack<IStack<TSource>>
        {
            public Stack(in IStack<TSource> stack) : base(stack) { /* Left empty. */ }
        }
    }
}
