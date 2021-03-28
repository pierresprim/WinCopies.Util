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

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public class ReadOnlyStack
#if WinCopies3
        <TStack, TItems>
#else
<T>
#endif
        : ReadOnlySimpleLinkedList<
#if WinCopies3
            TItems
#else
            T
#endif
            >, IStack<
#if WinCopies3
            TItems
#else
            T
#endif
            >
#if WinCopies3
        where TStack : IStack<TItems>
#endif
    {
        private readonly
#if WinCopies3
            TStack
#else
IStack<T>
#endif
            _stack;

        public sealed override uint Count => _stack.Count;

#if WinCopies3
        public bool HasItems => _stack.HasItems;
#endif

        public ReadOnlyStack(
#if WinCopies3
            TStack
#else
            IStack<T>
#endif
            stack) => _stack = stack;

        public sealed override
#if WinCopies3
            TItems
#else
            T
#endif
             Peek() => _stack.Peek();

        void
#if !WinCopies3
IStack
#else
            IStackBase
#endif
            <
#if WinCopies3
            TItems
#else
            T
#endif
            >.Push(
#if WinCopies3
            TItems
#else
            T
#endif
             item) => throw GetReadOnlyListOrCollectionException();


#if WinCopies3
        TItems
#else
            T
#endif
#if !WinCopies3
IStack
#else
            IStackBase
#endif
            <
#if WinCopies3
            TItems
#else
            T
#endif
            >.Pop() => throw GetReadOnlyListOrCollectionException();

#if WinCopies3
        public sealed override bool TryPeek(out TItems result) => _stack.TryPeek(out result);

        bool IStackBase<TItems>.TryPop(out TItems result)
        {
            result = default;

            return false;
        }
#endif
    }

#if WinCopies3
    public class ReadOnlyStack<T> : ReadOnlyStack<IStack<T>, T>
    {
        public ReadOnlyStack(in IStack<T> stack) : base(stack)
        {
            // Left empty.
        }
    }
#endif
}
