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
    public interface IClearable
    {
        void Clear();
    }

    public interface ISimpleLinkedListNodeBase
    {
        ISimpleLinkedListNodeBase Next { get; }
    }

    public interface ISimpleLinkedListNodeBase2 : ISimpleLinkedListNodeBase
    {
        object Value { get; }
    }

    public interface ISimpleLinkedListNodeBase<T> : ISimpleLinkedListNodeBase2 where T : ISimpleLinkedListNodeBase2
    {
        new T Next { get; }
#if CS8
        ISimpleLinkedListNodeBase ISimpleLinkedListNodeBase.Next => Next;
#endif
    }

    public interface ISimpleLinkedListNode : IClearable, ISimpleLinkedListNodeBase2
    {
        // Left empty.
    }

    public interface ISimpleLinkedListNode<T> : ISimpleLinkedListNodeBase<T>, ISimpleLinkedListNode where T : ISimpleLinkedListNode<T>
    {
        // Left empty.
    }

    public interface ISimpleLinkedListNode2 : ISimpleLinkedListNode<ISimpleLinkedListNode2>
    {
        // Left empty.
    }

    public interface ISimpleLinkedListCore
    {
        bool IsReadOnly { get; }

        bool HasItems { get; }
    }

    public interface ISimpleLinkedListBase : ISimpleLinkedListCore, IClearable
    {
        // Left empty.
    }

    public interface IListCommon
    {
        void Add(object
#if CS8
            ?
#endif
            value);
        object
#if CS8
            ?
#endif
            Remove();
        bool TryRemove(out object
#if CS8
            ?
#endif
            result);
    }

    public interface ISimpleLinkedListCommon : IListCommon, ISimpleLinkedListBase
    {
        // Left empty.
    }

    namespace Generic
    {
        public interface IListCommon<T> : IListCommon
        {
            void Add(T
#if CS9
                ?
#endif
                value);
            new T
#if CS9
                ?
#endif
                Remove();
            bool TryRemove(out T
#if CS9
                ?
#endif
                result);
#if CS8
            void IListCommon.Add(object? value) => Add((T
#if CS9
                ?
#endif
                )value);
            object? IListCommon.Remove() => Remove();
            bool IListCommon.TryRemove(out object? result) => UtilHelpers.TryGetValue<T>(TryRemove, out result);
#endif
        }

        public interface ISimpleLinkedListCommon<T> : IPeekable<T>, IListCommon<T>, ISimpleLinkedListCommon
        {
            // Left empty.
        }
    }

    public interface ISimpleLinkedListBase2 : ISimpleLinkedListBase, IUIntCountable
    {
        object SyncRoot { get; }
        bool IsSynchronized { get; }
    }

    public interface ISimpleLinkedList : ISimpleLinkedListBase2, IPeekable
    {
        // Left empty.
    }
}
