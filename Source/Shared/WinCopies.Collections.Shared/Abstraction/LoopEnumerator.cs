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

#if CS5
using System;
using WinCopies.Collections.Generic;

namespace WinCopies.Collections.Abstraction.Generic
{
    public abstract class LoopEnumerator<TEnumerator, TIn, TOut> : ILoopEnumerator<TOut> where TEnumerator : ILoopEnumerator<TIn>
    {
        protected TEnumerator Enumerator { get; }

        public TOut Current => Convert(Enumerator.Current);

        object
#if CS8
                ?
#endif
            ILoopEnumerator.Current => Current;

        protected LoopEnumerator(in TEnumerator enumerator) => Enumerator = enumerator;

        public void MovePrevious() => Enumerator.MovePrevious();
        public void MoveNext() => Enumerator.MoveNext();

        protected abstract TOut Convert(in TIn value);
    }

    public class LoopEnumeratorDelegate<TEnumerator, TIn, TOut> : LoopEnumerator<TEnumerator, TIn, TOut> where TEnumerator : ILoopEnumerator<TIn>
    {
        protected Converter<TIn, TOut> Selector { get; }

        public LoopEnumeratorDelegate(in TEnumerator enumerator, in Converter<TIn, TOut> selector) : base(enumerator) => Selector = selector;

        protected override TOut Convert(in TIn value) => Selector(value);
    }
}
#endif
