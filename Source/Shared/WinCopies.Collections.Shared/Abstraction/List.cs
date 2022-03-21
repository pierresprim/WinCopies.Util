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

using System;

using WinCopies.Collections.Abstraction.Generic.Abstract;

namespace WinCopies.Collections.Abstraction.Generic
{
    public class ReadOnlyList<TEnumerable, TSource, TDestination> : CountableEnumerableSelector<TEnumerable, TSource, TDestination>, System.Collections.Generic.IReadOnlyList<TDestination> where TEnumerable : System.Collections.Generic.IReadOnlyList<TSource>
    {
        public TDestination this[int index] => Selector(InnerEnumerable[index]);

        public ReadOnlyList(TEnumerable innerList, Converter<TSource, TDestination> selector) : base(innerList, selector) { /* Left empty. */ }
    }

    public class ReadOnlyList<TSource, TDestination> : ReadOnlyList<System.Collections.Generic.IReadOnlyList<TSource>, TSource, TDestination>
    {
        public ReadOnlyList(System.Collections.Generic.IReadOnlyList<TSource> innerList, Converter<TSource, TDestination> selector) : base(innerList, selector) { /* Left empty. */ }
    }

    public class List<TSource, TDestination> : ReadOnlyList<Abstract.IList<TSource>, TSource, TDestination>, System.Collections.Generic.IList<TDestination>
    {
        protected Converter<TDestination, TSource> ReversedSelector { get; }

        public new TDestination this[int index] { get => base[index]; set => InnerEnumerable[index] = ReversedSelector(value); }

        public bool IsReadOnly => InnerEnumerable.IsReadOnly;

        public List(Abstract.IList<TSource> innerList, Converter<TSource, TDestination> selector, Converter<TDestination, TSource> reversedSelector) : base(innerList, selector) => ReversedSelector = reversedSelector;

        public void Add(TDestination item) => InnerEnumerable.Add(ReversedSelector(item));

        public void Clear() => InnerEnumerable.Clear();

        public bool Contains(TDestination item) => InnerEnumerable.Contains(ReversedSelector(item));

        public void CopyTo(TDestination[] array, int arrayIndex) => InnerEnumerable.ToArray(arrayIndex, Count, Selector);

        public int IndexOf(TDestination item) => InnerEnumerable.IndexOf(ReversedSelector(item));

        public void Insert(int index, TDestination item) => InnerEnumerable.Insert(index, ReversedSelector(item));

        public bool Remove(TDestination item) => InnerEnumerable.Remove(ReversedSelector(item));

        public void RemoveAt(int index) => InnerEnumerable.RemoveAt(index);
    }
}

#endif
