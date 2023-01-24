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

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.AbstractionInterop.Generic
{
    public static partial class AbstractionTypes<TSource, TDestination>
    {
        public class ReadOnlyQueue<TQueue> : SimpleLinkedCollection<TQueue>, IQueue<TDestination> where TQueue : IQueue<TSource>
        {
            public ReadOnlyQueue(in TQueue queue) : base(queue) { /* Left empty. */ }

            void IQueueCore<TDestination>.Enqueue(TDestination
#if CS9
                ?
#endif
                item) => throw GetReadOnlyListOrCollectionException();
            TDestination
#if CS9
                ?
#endif
                IQueueCore<TDestination>.Dequeue() => throw GetReadOnlyListOrCollectionException();
            bool IQueueCore<TDestination>.TryDequeue(out TDestination
#if CS9
                ?
#endif
               result)
            {
                result = default;

                return false;
            }
#if !CS8
            void IQueueCore.Enqueue(object item) => throw GetReadOnlyListOrCollectionException();
            object IQueueCore.Dequeue() => throw GetReadOnlyListOrCollectionException();
            bool IQueueCore.TryDequeue(out object result)
            {
                result = null;

                return false;
            }
#endif
        }

        public class ReadOnlyQueue : ReadOnlyQueue<IQueue<TSource>>
        {
            public ReadOnlyQueue(in IQueue<TSource> queue) : base(queue) { /* Left empty. */ }
        }

        public class ReadOnlyStack<TStack> : SimpleLinkedCollection<TStack>, IStack<TDestination> where TStack : IStack<TSource>
        {
            public ReadOnlyStack(in TStack stack) : base(stack) { /* Left empty. */ }

            void IStackCore<TDestination>.Push(TDestination
#if CS9
                ?
#endif
                item) => throw GetReadOnlyListOrCollectionException();
            TDestination
#if CS9
                ?
#endif
                IStackCore<TDestination>.Pop() => throw GetReadOnlyListOrCollectionException();
            bool IStackCore<TDestination>.TryPop(out TDestination
#if CS9
                ?
#endif
                result)
            {
                result = default;

                return false;
            }
#if !CS8
            void IStackCore.Push(object item) => throw GetReadOnlyListOrCollectionException();
            object IStackCore.Pop() => throw GetReadOnlyListOrCollectionException();
            bool IStackCore.TryPop(out object result)
            {
                result = null;

                return false;
            }
#endif
        }

        public class ReadOnlyStack : Stack<IStack<TSource>>
        {
            public ReadOnlyStack(in IStack<TSource> stack) : base(stack) { /* Left empty. */ }
        }
    }
}
