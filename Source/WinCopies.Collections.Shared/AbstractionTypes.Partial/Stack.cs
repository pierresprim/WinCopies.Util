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
    public static partial class AbstractionTypes<TSource, TDestination> where TSource : TDestination
    {
        public class ReadOnlyStack<TStack> : IStack<TDestination> where TStack : IStack<TSource>
        {
            protected TStack InnerStack { get; }

#if WinCopies3
            object ISimpleLinkedListBase2.SyncRoot => InnerStack.SyncRoot;

            bool ISimpleLinkedListBase2.IsSynchronized => InnerStack.IsSynchronized;

            bool ISimpleLinkedListBase.IsReadOnly => true;

            bool ISimpleLinkedListBase.HasItems => InnerStack.HasItems;

            TDestination IStack<TDestination>.Peek() => InnerStack.Peek();

            private bool TryPeek(out TDestination result)
            {
                if (InnerStack.TryPeek(out TSource _result))
                {
                    result = _result;

                    return true;
                }

                result = default;

                return false;
            }

            bool IStack<TDestination>.TryPeek(out TDestination result) => TryPeek(out result);

            void IStack<TDestination>.Clear() => throw GetReadOnlyListOrCollectionException();

            void ISimpleLinkedListBase2.Clear() => throw GetReadOnlyListOrCollectionException();

            TDestination ISimpleLinkedListBase<TDestination>.Peek() => InnerStack.Peek();

            bool ISimpleLinkedListBase<TDestination>.TryPeek(out TDestination result) => TryPeek(out result);

            void IStackBase<TDestination>.Push(TDestination item) => throw GetReadOnlyListOrCollectionException();

            TDestination IStackBase<TDestination>.Pop() => throw GetReadOnlyListOrCollectionException();

            bool IStackBase<TDestination>.TryPop(out TDestination result) => throw GetReadOnlyListOrCollectionException();

            TDestination IStackBase<TDestination>.Peek() => InnerStack.Peek();

            bool IStackBase<TDestination>.TryPeek(out TDestination result) => TryPeek(out result);

            void IStackBase<TDestination>.Clear() => throw GetReadOnlyListOrCollectionException();
#else
            public void Push(TDestination item) => throw GetReadOnlyListOrCollectionException();

            public TDestination Pop() => throw GetReadOnlyListOrCollectionException();

            public TDestination Peek() => throw GetReadOnlyListOrCollectionException();
#endif

            uint IUIntCountable.Count => InnerStack.Count;

            public ReadOnlyStack(in TStack stack) => InnerStack = stack;
        }

        public class ReadOnlyStack : ReadOnlyStack<IStack<TSource>>
        {
            public ReadOnlyStack(in IStack<TSource> stack) : base(stack)
            {
                // Left empty.
            }
        }
    }
}
