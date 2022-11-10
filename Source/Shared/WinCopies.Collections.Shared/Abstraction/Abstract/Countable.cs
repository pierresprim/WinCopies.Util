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
using System.Collections.Generic;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Linq;

namespace WinCopies.Collections.Abstraction.Generic.Abstract
{
    public abstract class Countable<TEnumerable, TItems> : ICountable where TEnumerable : System.Collections.Generic.IEnumerable<TItems>
    {
        protected TEnumerable InnerEnumerable { get; }

        public abstract int Count { get; }

        protected Countable(TEnumerable enumerable) => InnerEnumerable = enumerable;
    }

    public abstract class CountableEnumerable<TEnumerable, TSourceItems, TDestinationItems> : Countable<TEnumerable, TSourceItems>, ICountableEnumerable<TDestinationItems> where TEnumerable : System.Collections.Generic.IEnumerable<TItems>
    {
        protected CountableEnumerable(TEnumerable enumerable) : base(enumerable) { /* Left empty. */ }

        public abstract ICountableEnumerator<TDestinationItems> GetEnumerator();

        System.Collections.Generic.IEnumerator<TDestinationItems> System.Collections.Generic.IEnumerable<TDestinationItems>.GetEnumerator() => GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public abstract class CountableEnumerable<TEnumerable, TItems> : CountableEnumerable<TEnumerable, TItems, TItems> where TEnumerable : System.Collections.Generic.IEnumerable<TItems>
    {
        protected CountableEnumerable(TEnumerable enumerable) : base(enumerable) { /* Left empty. */ }

        public override ICountableEnumerator<TItems> GetEnumerator() => new CountableEnumeratorInfo<TItems>(new EnumeratorInfo<TItems>(this), () => Count);
    }

    public abstract class CountableEnumerableSelector<TEnumerable, TSourceItems, TDestinationItems> : CountableEnumerable<TEnumerable, TSourceItems, TDestinationItems> where TEnumerable : System.Collections.Generic.IEnumerable<TSourceItems>
    {
        protected Converter<TSourceItems, TDestinationItems> Selector { get; }

        protected CountableEnumerableSelector(TEnumerable enumerable, Converter<TSourceItems, TDestinationItems> selector) : base(enumerable) => Selector = selector;

        public override ICountableEnumerator<TDestinationItems> GetEnumerator() => new CountableEnumeratorInfo<TDestinationItems>(new EnumeratorInfo<TSourceItems>(InnerEnumerable).SelectConverter(Selector), () => Count);
    }
}
#endif
