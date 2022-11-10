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

using System.Threading;

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix
{
    public abstract class ReadOnlySimpleLinkedListCore : ISimpleLinkedListBase2
    {
        public bool IsReadOnly => true;

        public abstract uint Count { get; }

        public virtual bool HasItems => Count != 0u;

        protected abstract object SyncRoot { get; }

        bool ISimpleLinkedListBase2.IsSynchronized => false;
        object ISimpleLinkedListBase2.SyncRoot => SyncRoot;

        public void Clear() => throw GetReadOnlyListOrCollectionException();
    }

    public abstract class ReadOnlySimpleLinkedListBase : ReadOnlySimpleLinkedListCore, ISimpleLinkedList, ISimpleLinkedListCommon
    {
        public abstract object Peek();
        public abstract bool TryPeek(out object result);

        void IListCommon.Add(object value) => throw GetReadOnlyListOrCollectionException();
        object IListCommon.Remove() => throw GetReadOnlyListOrCollectionException();
        bool IListCommon.TryRemove(out object result)
        {
            result = null;

            return false;
        }
    }

    public abstract class ReadOnlySimpleLinkedList : ReadOnlySimpleLinkedListBase
    {
        private object _syncRoot;

        protected override object SyncRoot
        {
            get
            {
                if (_syncRoot == null)

                    _syncRoot = Interlocked.CompareExchange(ref _syncRoot, new object(), null);

                return _syncRoot;
            }
        }
    }

    public abstract class ReadOnlySimpleLinkedList<T> : ReadOnlySimpleLinkedListBase, IUIntCountable, IPeekable, ISimpleLinkedListCore where T : IUIntCountable, IPeekable, ISimpleLinkedListBase2
    {
        protected T InnerList { get; }

        public override uint Count => InnerList.Count;

        public override bool HasItems => InnerList.HasItems;

        protected override object SyncRoot => InnerList.SyncRoot;

        public ReadOnlySimpleLinkedList(in T list) => InnerList = list;

        public override object Peek() => InnerList.Peek();
        public override bool TryPeek(out object result) => InnerList.TryPeek(out result);
    }
}
