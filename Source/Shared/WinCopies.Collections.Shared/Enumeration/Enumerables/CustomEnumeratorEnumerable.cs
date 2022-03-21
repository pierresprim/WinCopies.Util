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

#if !WinCopies3
using WinCopies.Collections.DotNetFix;
#endif

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
        uint
#if WinCopies3
            IUIntCountable
#else
            IUIntCountableEnumerable
#endif
            .Count => InnerEnumerator.Count;

        public UIntCountableEnumerable(in TEnumerator enumerator) : base(enumerator) { /* Left empty. */ }

#if WinCopies3
        IUIntCountableEnumerator<TItems> IUIntCountableEnumerable<TItems, IUIntCountableEnumerator<TItems>>.GetEnumerator() => GetEnumerator();

#if !CS8
        IUIntCountableEnumerator<TItems> Enumeration.DotNetFix.IEnumerable<IUIntCountableEnumerator<TItems>>.GetEnumerator() => GetEnumerator();

        IUIntCountableEnumerator<TItems> DotNetFix.Generic.IEnumerable<TItems, IUIntCountableEnumerator<TItems>>.GetEnumerator() => GetEnumerator();
#endif
#endif
    }

    public class UIntCountableProvider<TItems, TEnumerator> : CustomEnumeratorProvider<TItems, TEnumerator>, IUIntCountableEnumerable<TItems> where TEnumerator :
#if WinCopies3
        IEnumeratorInfo2<TItems>
#else
        System.Collections.Generic.IEnumerator<TItems>
#endif
    {
        private Func<uint> CountFunc { get; }

        uint
#if WinCopies3
            IUIntCountable
#else
            IUIntCountableEnumerable
#endif
            .Count => CountFunc();

        public UIntCountableProvider(in Func<TEnumerator> func, in Func<uint> countFunc) : base(func) => CountFunc = countFunc;

#if WinCopies3
        private IUIntCountableEnumerator<TItems> _GetEnumerator() => new UIntCountableEnumeratorInfo<TItems>(GetEnumerator(), CountFunc);

        IUIntCountableEnumerator<TItems> IUIntCountableEnumerable<TItems, IUIntCountableEnumerator<TItems>>.GetEnumerator() => _GetEnumerator();

#if !CS8
        IUIntCountableEnumerator<TItems> Enumeration.DotNetFix.IEnumerable<IUIntCountableEnumerator<TItems>>.GetEnumerator() => _GetEnumerator();

        IUIntCountableEnumerator<TItems> DotNetFix.Generic.IEnumerable<TItems, IUIntCountableEnumerator<TItems>>.GetEnumerator() => _GetEnumerator();
#endif
#endif
    }
}
