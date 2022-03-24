/* Copyright © Pierre Sprimont, 2021
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

#if WinCopies3
using System;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;

namespace WinCopies.Collections.Abstraction.Generic
{
    public interface IReadOnlyArrayEnumerable<T> : IReadOnlyList<T>
    {
        bool Check();
    }

    public interface IArrayEnumerable<T> : IReadOnlyArrayEnumerable<T>, IArray<T>
    {
        new T this[int index] { get; set; }
    }

    public sealed class Enumerator : System.Collections.Generic.IEnumerator<object>
    {
        private readonly System.Collections.IEnumerator _enumerator;

        object System.Collections.Generic.IEnumerator<object>.Current => _enumerator.Current;

        object System.Collections.IEnumerator.Current => _enumerator.Current;

        public Enumerator(in System.Collections.IEnumerator enumerator) => _enumerator = enumerator;

        bool System.Collections.IEnumerator.MoveNext() => _enumerator.MoveNext();

        void System.Collections.IEnumerator.Reset() => _enumerator.Reset();

        void System.IDisposable.Dispose() { /* Left empty. */ }
    }

    public sealed class Enumerable : System.Collections.Generic.IEnumerable<object>
    {
        private System.Collections.IEnumerable _enumerable;

        public Enumerable(in System.Collections.IEnumerable enumerable) => _enumerable = enumerable;

        public System.Collections.Generic.IEnumerator<object> GetEnumerator() => new Enumerator(_enumerable.GetEnumerator());

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public abstract class ReadOnlyArrayEnumerableBase<TSourceItems, TDestinationItems> : System.Collections.Generic.IEnumerable<TDestinationItems>, IReadOnlyList
    {
        protected TSourceItems[] Array { get; }

        public int Count => Array.Length;

#if !(WinCopies3 && CS7)
        object IReadOnlyList.this[int index] => Array[index];
#endif

        public ReadOnlyArrayEnumerableBase(in TSourceItems[] array) => Array = array;

        public bool Check()
        {
            foreach (TSourceItems item in Array)

                if (!(item is TDestinationItems))

                    return false;

            return true;
        }

        public abstract ICountableEnumerator<TDestinationItems> GetEnumerator();

        System.Collections.Generic.IEnumerator<TDestinationItems> System.Collections.Generic.IEnumerable<TDestinationItems>.GetEnumerator() => GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        ICountableEnumerator Enumeration.DotNetFix.IEnumerable<ICountableEnumerator>.GetEnumerator() => GetEnumerator();
    }

    public abstract class ReadOnlyArrayEnumerableBase2<TSourceItems, TDestinationItems> : ReadOnlyArrayEnumerableBase<TSourceItems, TDestinationItems>, IReadOnlyArrayEnumerable<TDestinationItems>
    {
        public abstract TDestinationItems this[int index] { get; }

        public ReadOnlyArrayEnumerableBase2(in TSourceItems[] array) : base(array) { /* Left empty. */ }

        public sealed override ICountableEnumerator<TDestinationItems> GetEnumerator() => new ArrayEnumerator<TDestinationItems>(this);
    }

    public abstract class ArrayEnumerableBase<TSourceItems, TDestinationItems> : ReadOnlyArrayEnumerableBase<TSourceItems, TDestinationItems>, IArrayEnumerable<TDestinationItems>
    {
        public abstract TDestinationItems this[int index] { get; set; }

        public ArrayEnumerableBase(in TSourceItems[] array) : base(array) { /* Left empty. */ }

        public sealed override ICountableEnumerator<TDestinationItems> GetEnumerator() => new ArrayEnumerator<TDestinationItems>(this);
    }

    public abstract class ReadOnlyArrayEnumerable<TSourceItems, TDestinationItems> : ReadOnlyArrayEnumerableBase2<TSourceItems, TDestinationItems>
    {
        public override TDestinationItems this[int index] => Convert(Array[index]);

        public ReadOnlyArrayEnumerable(in TSourceItems[] array) : base(array) { /* Left empty. */ }

        protected abstract TDestinationItems Convert(TSourceItems item);
    }

    public class ReadOnlyArrayEnumerableSelector<TSourceItems, TDestinationItems> : ReadOnlyArrayEnumerableBase2<TSourceItems, TDestinationItems>
    {
        private readonly Converter<TSourceItems, TDestinationItems> _selector;

        public override TDestinationItems this[int index] => _selector(Array[index]);

        public ReadOnlyArrayEnumerableSelector(in TSourceItems[] array, in Converter<TSourceItems, TDestinationItems> selector) : base(array) => _selector = selector;
    }

    public abstract class ArrayEnumerable<TSourceItems, TDestinationItems> : ArrayEnumerableBase<TSourceItems, TDestinationItems>
    {
        public override TDestinationItems this[int index] { get => Convert(Array[index]); set => Array[index] = ConvertBack(value); }

        public ArrayEnumerable(in TSourceItems[] array) : base(array) { /* Left empty. */ }

        protected abstract TDestinationItems Convert(TSourceItems item);

        protected abstract TSourceItems ConvertBack(TDestinationItems item);
    }

    public class ArrayEnumerableSelector<TSourceItems, TDestinationItems> : ArrayEnumerableBase<TSourceItems, TDestinationItems>
    {
        private readonly Conversion<TSourceItems, TDestinationItems> _selectors;

        public override TDestinationItems this[int index] { get => _selectors.Converter(Array[index]); set => Array[index] = _selectors.BackConverter(value); }

        public ArrayEnumerableSelector(in TSourceItems[] array, in Conversion<TSourceItems, TDestinationItems> selectors) : base(array) => _selectors = selectors;
    }
}
#endif
