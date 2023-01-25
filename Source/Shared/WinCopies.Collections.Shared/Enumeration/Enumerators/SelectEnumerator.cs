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

using System;

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections.Generic
{
    public sealed class SelectEnumerator<TSource, TDestination> : Enumerator<TSource, TDestination>
    {
        private Converter<TSource, TDestination> _func;
        private TDestination _current;

        /// <summary>
        /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
        /// </summary>
        protected override TDestination CurrentOverride => _current;

        public override bool? IsResetSupported => null;

        public SelectEnumerator(in System.Collections.Generic.IEnumerable<TSource> enumerable, in Converter<TSource, TDestination> func) : this(enumerable.GetEnumerator(), func) { /* Left empty. */ }

        public SelectEnumerator(in System.Collections.Generic.IEnumerator<TSource> enumerator, in Converter<TSource, TDestination> func) : base(enumerator) => _func = func ?? throw GetArgumentNullException(nameof(func));

        protected override bool MoveNextOverride()
        {
            if (InnerEnumerator.MoveNext())
            {
                _current = _func(InnerEnumerator.Current);

                return true;
            }

            return false;
        }

        protected override void ResetOverride2()
        {
            base.ResetOverride2();

            InnerEnumerator.Reset();
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();

            _func = null;
        }
    }
}
