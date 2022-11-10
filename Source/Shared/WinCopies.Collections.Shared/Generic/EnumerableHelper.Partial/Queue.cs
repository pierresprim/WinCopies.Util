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

using System.Collections;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Enumeration;
using WinCopies.Util;

namespace WinCopies.Collections
{
    public static partial class EnumerableHelper
    {
        internal class _LinkedListBase<TItems, TEnumerator> where TItems : DotNetFix.IPeekableEnumerableInfo<TEnumerator>, ISimpleLinkedListBase where TEnumerator : IEnumeratorInfo
        {
            protected TItems Enumerable { get; }

            public bool IsReadOnly => false;

            public bool HasItems => Enumerable.HasItems;

            public _LinkedListBase(in TItems enumerable) => Enumerable = enumerable;

            public void Clear() => Enumerable.Clear();
        }

        internal class SimpleLinkedListBase<U> : _LinkedListBase<U, IEnumeratorInfo>, IListCommon where U : IPeekableEnumerableInfo, ISimpleLinkedListBase, IListCommon
        {
            protected IEnumerableInfo EnumerableInfo => Enumerable;

            public SimpleLinkedListBase(in U enumerable) : base(enumerable) { /* Left empty. */ }

            public void Add(object
#if CS8
                ?
#endif
                value) => Enumerable.Add(value);
            public object
#if CS8
                ?
#endif
                Remove() => Enumerable.Remove();
            public bool TryRemove(out object
#if CS8
                ?
#endif
                result) => Enumerable.TryRemove(out result);
        }

        public interface IEnumerableQueue : IQueueBase, IPeekableEnumerableInfo
        {
            // Left empty.
        }

        internal class Queue : SimpleLinkedListBase<IEnumerableQueue>, IQueueBase
        {
            public Queue() : base(new Enumerable()) { /* Left empty. */ }

            public object
#if CS9
                ?
#endif
                Peek() => Enumerable.Peek();

            public bool TryPeek(out object
#if CS9
                ?
#endif
                result) => Enumerable.TryPeek(out result);

            public void Enqueue(object
#if CS9
                ?
#endif
                item) => Add(item);

            public object
#if CS9
                ?
#endif
                Dequeue() => Remove();

            public bool TryDequeue(out object
#if CS9
                ?
#endif
                item) => TryRemove(out item);
        }

        internal class EnumerableQueue : Queue, IEnumerableQueue
        {
            public bool SupportsReversedEnumeration => Enumerable.SupportsReversedEnumeration;

            public IEnumeratorInfo GetEnumerator() => EnumerableInfo.GetEnumerator();
            public IEnumeratorInfo GetReversedEnumerator() => EnumerableInfo.GetReversedEnumerator();
#if !CS8
            System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            System.Collections.IEnumerator Extensions.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#endif
        }
    }

    namespace Generic
    {
        public static partial class EnumerableHelper<T>
        {
            internal class LinkedListBase<U> : EnumerableHelper._LinkedListBase<U, IEnumeratorInfo<T>>, IListCommon<T> where U : DotNetFix.Generic.IPeekableEnumerableInfo<T>, ISimpleLinkedListBase, IListCommon<T>
            {
                protected Extensions.IEnumerable<IEnumeratorInfo<T>> EnumerableInfo => Enumerable;

                public LinkedListBase(in U enumerable) : base(enumerable) { /* Left empty. */ }

                public void Add(T
#if CS9
                    ?
#endif
                    value) => Enumerable.Add(value);
                public T
#if CS9
                    ?
#endif
                    Remove() => Enumerable.Remove();
                public bool TryRemove(out T
#if CS9
                    ?
#endif
                    result) => Enumerable.TryRemove(out result);

                void IListCommon.Add(object
#if CS8
                    ?
#endif
                    value) => Enumerable.Add(value);
                object
#if CS8
                    ?
#endif
                    IListCommon.Remove() => Enumerable.Remove();
                public bool TryRemove(out object
#if CS8
                    ?
#endif
                    result) => Enumerable.TryRemove(out result);
            }

            public interface IEnumerableQueue : IQueueBase<T>, DotNetFix.Generic.IPeekableEnumerableInfo<T>
            {
                // Left empty.
            }

            internal class Queue : LinkedListBase<IEnumerableQueue>, IQueueBase<T>
            {
                public Queue() : base(new Enumerable()) { /* Left empty. */ }

                public T
#if CS9
                    ?
#endif
                    Peek() => Enumerable.Peek();

                public bool TryPeek(out T
#if CS9
                    ?
#endif
                    result) => Enumerable.TryPeek(out result);

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
                    item) => TryRemove(out item);
#if !CS8
                object IPeekable.Peek() => Peek();
                public bool TryPeek(out object result) => UtilHelpers.TryGetValue<T>(TryPeek, out result);

                void IQueueCore.Enqueue(object item) => Enqueue((T)item);
                object IQueueCore.Dequeue() => Dequeue();
                public bool TryDequeue(out object result) => UtilHelpers.TryGetValue<T>(TryDequeue, out result);
#endif
            }

            internal class EnumerableQueue : Queue, IEnumerableQueue
            {
                public bool SupportsReversedEnumeration => Enumerable.SupportsReversedEnumeration;

                public IEnumeratorInfo<T> GetEnumerator() => EnumerableInfo.GetEnumerator();
                public IEnumeratorInfo<T> GetReversedEnumerator() => EnumerableInfo.GetReversedEnumerator();

                System.Collections.IEnumerator Extensions.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#if !CS8
                System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();
                System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
                System.Collections.Generic.IEnumerator<T> Extensions.Generic.IEnumerable<T>.GetReversedEnumerator() => GetReversedEnumerator();
#endif
            }
        }
    }
}
