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

#if WinCopies3 && CS7

using System.Collections.Generic;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;

namespace WinCopies.Collections.Abstraction.Generic.Abstract
{
    public interface IList<T> : System.Collections.Generic.IReadOnlyList<T>, System.Collections.Generic.IList<T>, ICountableEnumerable<T>, System.Collections.Generic.IReadOnlyCollection<T>
    {
        new T this[int index] { get; set; }
    }

    public class List<T> : IList<T>
    {
#region Properties
        protected System.Collections.Generic.IList<T> InnerList { get; }

        public int Count => InnerList.Count;

        public T this[int index] { get => InnerList[index]; set => InnerList[index] = value; }

        public bool IsReadOnly => InnerList.IsReadOnly;
#endregion

        public List(System.Collections.Generic.IList<T> innerList) => InnerList = innerList;

#region Methods
        public void Add(T item) => InnerList.Add(item);

        public bool Contains(T item) => InnerList.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

        public ICountableEnumerator<T> GetEnumerator() => new CountableEnumeratorInfo<T>(new EnumeratorInfo<T>(InnerList.GetEnumerator()), () => Count);

        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(T item) => InnerList.IndexOf(item);

        public void Insert(int index, T item) => InnerList.Insert(index, item);

        public void RemoveAt(int index) => InnerList.RemoveAt(index);

        public bool Remove(T item) => InnerList.Remove(item);

        public void Clear() => InnerList.Clear();
#endregion
    }
}

#endif
