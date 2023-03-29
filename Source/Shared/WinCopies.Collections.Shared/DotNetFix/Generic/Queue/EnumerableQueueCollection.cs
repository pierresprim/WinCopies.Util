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
using System.Linq;
using WinCopies.Util;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public class EnumerableQueueCollection<TQueue, TItems> : QueueCollection<TQueue, TItems>, IEnumerableQueue<TItems>, System.Collections.Generic.IReadOnlyCollection<TItems>, ICollection where TQueue : IEnumerableQueue<TItems>
    {
        protected ICollection InnerCollection => InnerList;

        bool ICollection.IsSynchronized => InnerCollection.IsSynchronized;
        object ICollection.SyncRoot => InnerCollection.SyncRoot;

        int ICollection.Count => InnerCollection.Count;
        int IReadOnlyCollection<TItems>.Count => InnerList.AsFromType<IReadOnlyCollection<TItems>>().Count;

        public EnumerableQueueCollection(in TQueue queue) : base(queue) { /* Left empty. */ }

        public void CopyTo(TItems[] array, int index) => InnerList.CopyTo(array, index);
        public void CopyTo(System.Array array, int index) => InnerList.CopyTo(array, index);

        public System.Collections.Generic.IEnumerator<TItems> GetEnumerator() => InnerList.AsEnumerable().GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        public TItems[] ToArray() => InnerList.ToArray();
    }

    public class EnumerableQueueCollection<T> : EnumerableQueueCollection<IEnumerableQueue<T>, T>, IEnumerableQueue<T>
    {
        public EnumerableQueueCollection(in IEnumerableQueue<T> queue) : base(queue) { /* Left empty. */ }
        public EnumerableQueueCollection() : this(new EnumerableQueue<T>()) { /* Left empty. */ }
    }
}
#endif
