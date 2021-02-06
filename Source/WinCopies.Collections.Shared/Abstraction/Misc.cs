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
using System.Collections.Generic;
using WinCopies.Collections.Abstraction.Generic.Abstract;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Linq;

namespace WinCopies.Collections.Abstraction.Generic
{
    namespace Abstract
    {
        public abstract class Countable<TEnumerable, TItems> : ICountable where TEnumerable : IReadOnlyCollection<TItems>
        {
            protected TEnumerable InnerEnumerable { get; }

            public int Count => InnerEnumerable.Count;

            protected Countable(TEnumerable enumerable) => InnerEnumerable = enumerable;
        }

        public abstract class CountableEnumerable<TEnumerable, TSourceItems, TDestinationItems> : Countable<TEnumerable, TSourceItems>, ICountableEnumerable<TDestinationItems> where TEnumerable : IReadOnlyCollection<TSourceItems>
        {
            protected CountableEnumerable(TEnumerable enumerable) : base(enumerable) { /* Left empty. */ }

            public abstract ICountableEnumerator<TDestinationItems> GetEnumerator();

            System.Collections.Generic.IEnumerator<TDestinationItems> System.Collections.Generic.IEnumerable<TDestinationItems>.GetEnumerator() => GetEnumerator();

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public abstract class CountableEnumerable<TEnumerable, TItems> : CountableEnumerable<TEnumerable, TItems, TItems> where TEnumerable : IReadOnlyCollection<TItems>
        {
            protected CountableEnumerable(TEnumerable enumerable) : base(enumerable) { /* Left empty. */ }

            public override ICountableEnumerator<TItems> GetEnumerator() => new CountableEnumeratorInfo<TItems>(new EnumeratorInfo<TItems>(this), () => Count);
        }

        public abstract class CountableEnumerableSelector<TEnumerable, TSourceItems, TDestinationItems> : CountableEnumerable<TEnumerable, TSourceItems, TDestinationItems> where TEnumerable : IReadOnlyCollection<TSourceItems>
        {
            protected Converter<TSourceItems, TDestinationItems> Selector { get; }

            protected CountableEnumerableSelector(TEnumerable enumerable, Converter<TSourceItems, TDestinationItems> selector) : base(enumerable) => Selector = selector;

            public override ICountableEnumerator<TDestinationItems> GetEnumerator() => new CountableEnumeratorInfo<TDestinationItems>(new EnumeratorInfo<TSourceItems>(InnerEnumerable).SelectConverter(Selector), () => Count);
        }

        public interface IList<T> : System.Collections.Generic.IReadOnlyList<T>, System.Collections.Generic.IList<T>, ICountableEnumerable<T>, IReadOnlyCollection<T>
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
