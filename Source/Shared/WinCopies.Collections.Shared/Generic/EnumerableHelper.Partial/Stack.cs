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

using System.Collections.Generic;
using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;

namespace WinCopies.Collections
{
    public static partial class EnumerableHelper
    {
        public interface IEnumerableStack : IStackBase, IPeekableEnumerableInfo
        {
            // Left empty.
        }

        internal class Stack : SimpleLinkedListBase<IEnumerableStack>, IStackBase
        {
            public Stack() : base(new Enumerable()) { /* Left empty. */ }

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

            public void Push(object
#if CS9
                ?
#endif
                item) => Enumerable.Push(item);

            public object
#if CS9
                ?
#endif
                Pop() => Enumerable.Pop();

            public bool TryPop(out object
#if CS9
                ?
#endif
                item) => Enumerable.TryPop(out item);
        }

        internal class EnumerableStack : Stack, IEnumerableStack
        {
            public bool SupportsReversedEnumeration => Enumerable.SupportsReversedEnumeration;

            public IEnumeratorInfo GetEnumerator() => EnumerableInfo.GetEnumerator();
            System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumeratorInfo GetReversedEnumerator() => EnumerableInfo.GetReversedEnumerator();
#if !CS8
            System.Collections.IEnumerator Extensions.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#endif
        }
    }

    namespace Generic
    {
        public static partial class EnumerableHelper<T>
        {
            public interface IEnumerableStack : IStackBase<T>, DotNetFix.Generic.IPeekableEnumerableInfo<T>
            {
                // Left empty.
            }

            internal class Stack : LinkedListBase<IEnumerableStack>, IStackBase<T>
            {
                public Stack() : base(new Enumerable()) { /* Left empty. */ }

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

                public void Push(T
#if CS9
                    ?
#endif
                    item) => Enumerable.Push(item);

                public T
#if CS9
                    ?
#endif
                    Pop() => Enumerable.Pop();

                public bool TryPop(out T
#if CS9
                    ?
#endif
                    item) => Enumerable.TryPop(out item);
#if !CS8
                public void Push(object item) => Push((T)item);
                object IStackCore.Pop() => Pop();
                public bool TryPop(out object result) => UtilHelpers.TryGetValue<T>(TryPop, out result);
                object IPeekable.Peek() => Peek();
                public bool TryPeek(out object result) => UtilHelpers.TryGetValue<T>(TryPeek, out result);
#endif
            }

            internal class EnumerableStack : Stack, IEnumerableStack
            {
                public bool SupportsReversedEnumeration => Enumerable.SupportsReversedEnumeration;

                public IEnumeratorInfo<T> GetEnumerator() => EnumerableInfo.GetEnumerator();

                public IEnumeratorInfo<T> GetReversedEnumerator() => EnumerableInfo.GetReversedEnumerator();
                System.Collections.IEnumerator Extensions.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#if !CS8
                IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();
                System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
                IEnumerator<T> Extensions.Generic.IEnumerable<T>.GetReversedEnumerator() => GetReversedEnumerator();
#endif
            }
        }
    }
}
