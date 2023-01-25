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

namespace WinCopies.Collections.DotNetFix.Generic
{
    public interface ISimpleLinkedListNode<T> : ISimpleLinkedListNode
    {
        new T Value { get; }

        new ISimpleLinkedListNode<T> Next { get; }
#if CS8
        object ISimpleLinkedListNodeBase2.Value => Value;

        ISimpleLinkedListNodeBase ISimpleLinkedListNodeBase.Next => Next;
#endif
    }

    public interface ISimpleLinkedListNode<TNode, TValue> : ISimpleLinkedListNode<TValue>, DotNetFix.ISimpleLinkedListNode<TNode> where TNode : DotNetFix.ISimpleLinkedListNode<TNode>
    {
        // Left empty.
    }

    public class SimpleLinkedListNode<T> : SimpleLinkedListNodeBase<SimpleLinkedListNode<T>>, ISimpleLinkedListNode<SimpleLinkedListNode<T>, T>
    {
        private T _value;

        public T Value => IsCleared ? throw SimpleLinkedListNodeHelper.GetIsClearedException() : _value;

        internal SimpleLinkedListNode(T value) => _value = value;

        protected override void ResetValue() => _value = default;

        ISimpleLinkedListNode<T> ISimpleLinkedListNode<T>.Next => Next;
#if !CS8
        object ISimpleLinkedListNodeBase2.Value => Value;
#endif
    }
}
