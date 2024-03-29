﻿/* Copyright © Pierre Sprimont, 2020
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

#if !WinCopies3
namespace WinCopies.Collections.Generic
{
    public class TypeConverterEnumerator<T> : Enumerator<T>
    {
        private T _current;

        protected System.Collections.IEnumerator InnerEnumerator { get; }

        protected override T CurrentOverride => _current;

#if WinCopies3
        public override bool? IsResetSupported => null;

        protected override void ResetOverride2() { /* Left empty. */ }
#endif

        public TypeConverterEnumerator(in System.Collections.IEnumerator enumerator) => InnerEnumerator = enumerator;

        public TypeConverterEnumerator(in System.Collections.IEnumerable enumerable) : this(enumerable.GetEnumerator()) { /* Left empty. */ }

        protected override bool MoveNextOverride()
        {
            while (InnerEnumerator.MoveNext())

                if (InnerEnumerator.Current is T current)
                {
                    _current = current;

                    return true;
                }

            return false;
        }
    }
}
#endif