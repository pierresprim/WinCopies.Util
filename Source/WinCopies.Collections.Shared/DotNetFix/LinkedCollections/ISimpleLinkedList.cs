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

namespace WinCopies.Collections.DotNetFix
{
    public interface ISimpleLinkedListNode
    {
        object Value { get; }

#if !WinCopies3
        ISimpleLinkedListNode NextNode { get; }
#else
        ISimpleLinkedListNode Next { get; }
#endif
    }

    public interface ISimpleLinkedListBase
#if !WinCopies3
: IUIntCountable
#else
    {
        bool IsReadOnly { get; }

        bool HasItems { get; }
    }

    public interface ISimpleLinkedListBase2 : ISimpleLinkedListBase, IUIntCountable
#endif
    {
        object SyncRoot { get; }

        bool IsSynchronized { get; }

#if WinCopies3
        void Clear();
#endif
    }

    public interface ISimpleLinkedList :
#if WinCopies3
             ISimpleLinkedListBase2
#else
             IUIntCountable
#endif
    {
#if !WinCopies3
        bool IsReadOnly { get; }
#else
        bool TryPeek(out object result);
#endif

        object Peek();
    }
}
