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

#if CS7
using System;
using System.Collections;
using System.Collections.Generic;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public class EnumerableStackCollection<TStack, TItems> : StackCollection<TStack, TItems>, IEnumerableStack<TItems>, System.Collections.Generic.IReadOnlyCollection<TItems>, ICollection where TStack : IEnumerableStack<TItems>
    {
        protected ICollection InnerCollection => InnerList;

        bool ICollection.IsSynchronized => InnerCollection.IsSynchronized;
        object ICollection.SyncRoot => InnerCollection.SyncRoot;

        int ICollection.Count => InnerCollection.Count;

        int System.Collections.Generic.IReadOnlyCollection<TItems>.Count => InnerCollection.Count;

        public EnumerableStackCollection(in TStack stack) : base(stack) { /* Left empty. */ }

        public void CopyTo(TItems[] array, int index) => InnerList.CopyTo(array, index);

        public void CopyTo(Array array, int index) => InnerList.CopyTo(array, index);

        public System.Collections.Generic.IEnumerator<TItems> GetEnumerator() => InnerList.GetEnumerator();
        System.Collections.IEnumerator IEnumerable.GetEnumerator() => InnerList.GetEnumerator();

        public TItems[] ToArray() => InnerList.ToArray();
    }

    public class EnumerableStackCollection<T> : EnumerableStackCollection<IEnumerableStack<T>, T>
    {
        public EnumerableStackCollection(in IEnumerableStack<T> stack) : base(stack) { /* Left empty. */ }
        public EnumerableStackCollection() : this(new EnumerableStack<T>()) { /* Left empty. */ }
    }
}
#endif