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

using System;

using WinCopies.Collections.DotNetFix.Generic;

namespace WinCopies.Collections.Generic
{
    public class CustomEnumeratorEnumerable<TItems, TEnumerator> : System.Collections.Generic.IEnumerable<TItems> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
    {
        protected TEnumerator InnerEnumerator { get; }

        public CustomEnumeratorEnumerable(in TEnumerator enumerator) => InnerEnumerator = enumerator;

        public TEnumerator GetEnumerator() => InnerEnumerator;

        System.Collections.Generic.IEnumerator<TItems> System.Collections.Generic.IEnumerable<TItems>.GetEnumerator() => InnerEnumerator;

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => InnerEnumerator;
    }

    public class CustomEnumeratorProvider<TItems, TEnumerator> : System.Collections.Generic.IEnumerable<TItems> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
    {
        protected Func<TEnumerator> Func { get; }

        public CustomEnumeratorProvider(in Func<TEnumerator> func) => Func = func;

        public TEnumerator GetEnumerator() => Func();
        System.Collections.Generic.IEnumerator<TItems> System.Collections.Generic.IEnumerable<TItems>.GetEnumerator() => Func();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => Func();
    }

    public class UIntCountableEnumerable<TItems, TEnumerator> : CustomEnumeratorEnumerable<TItems, TEnumerator>, IUIntCountableEnumerable<TItems> where TEnumerator : IUIntCountableEnumerator<TItems>
    {
        uint IUIntCountable.Count => InnerEnumerator.Count;

        public UIntCountableEnumerable(in TEnumerator enumerator) : base(enumerator) { /* Left empty. */ }

        IUIntCountableEnumerator<TItems> Enumeration.IEnumerable<IUIntCountableEnumerator<TItems>>.GetEnumerator() => GetEnumerator ();
    }

    public class UIntCountableProvider<TItems, TEnumerator> : CustomEnumeratorProvider<TItems, TEnumerator>, IUIntCountableEnumerable<TItems> where TEnumerator : IEnumeratorInfo2<TItems>
    {
        private Func<uint> CountFunc { get; }

        uint IUIntCountable.Count => CountFunc();

        public UIntCountableProvider(in Func<TEnumerator> func, in Func<uint> countFunc) : base(func) => CountFunc = countFunc;

        IUIntCountableEnumerator<TItems> Enumeration.IEnumerable<IUIntCountableEnumerator<TItems>>.GetEnumerator() => new UIntCountableEnumeratorInfo<TItems>(GetEnumerator(), CountFunc);
    }
}
