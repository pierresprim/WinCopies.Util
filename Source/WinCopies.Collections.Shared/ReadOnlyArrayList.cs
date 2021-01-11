/* Copyright © Pierre Sprimont, 2019
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

#if WinCopies2 // Removed in WinCopies 3

using System;
using System.Collections;

namespace WinCopies.Collections
{
    [Obsolete("This interface is obsolete and will be removed in later versions.")]
    public interface IReadOnlyList : IList
    {
        object this[int index] { get; }
    }

    [Obsolete("This class is obsolete and will be removed in later versions. Please use the ArrayList.ReadOnly method instead.")]
    public class ReadOnlyArrayList : IEnumerable, IList, ICollection, IReadOnlyList
    {
        private IList innerList = null;

        public ReadOnlyArrayList(IList list) => innerList = list;

        public object this[int index] { get => innerList[index]; }

        object IList.this[int index] { get => this[index]; set => throw new NotImplementedException(); }

        public int Count => innerList.Count;

        public object SyncRoot => innerList.SyncRoot;

        public bool IsSynchronized => innerList.IsSynchronized;

        public bool IsReadOnly => true;

        public bool IsFixedSize => true;

        int IList.Add(object value) => throw new NotImplementedException();

        void IList.Clear() => throw new NotImplementedException();

        public bool Contains(object value) => innerList.Contains(value);

        public void CopyTo(Array array, int index) => innerList.CopyTo(array, index);

        public System.Collections.IEnumerator GetEnumerator() => innerList.GetEnumerator();

        public int IndexOf(object value) => innerList.IndexOf(value);

        void IList.Insert(int index, object value) => throw new NotImplementedException();

        void IList.Remove(object value) => throw new NotImplementedException();

        void IList.RemoveAt(int index) => throw new NotImplementedException();
    }
}

#endif
