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
using System.Collections;
using System.Collections.Generic;

namespace WinCopies.Collections
{
    public sealed class Enumerable : IEnumerable
    {
        private readonly Func<IEnumerator> _enumeratorFunc;

        public Enumerable(
#if WinCopies3
            in
#endif
            Func<IEnumerator> enumeratorFunc) => _enumeratorFunc = enumeratorFunc;

        public IEnumerator GetEnumerator() => _enumeratorFunc();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static IEnumerator<T> GetEnumerator<T>(params T[] items)
        {
            foreach (T item in items)

                yield return item;
        }

        public static IEnumerable<T> GetEnumerable<T>(params T[] items) => new
#if WinCopies3
            Generic.
#endif
            Enumerable<T>(() => GetEnumerator(items));

        public static IEnumerable<T> FromEnumeratorFunc<T>(in Func<IEnumerator<T>> func) => new Generic.Enumerable<T>(func);

        public static IEnumerable<T> FromEnumerator<T>(IEnumerator<T> enumerator) => new
#if WinCopies3
            Generic.
#endif
            Enumerable<T>(() => enumerator);

#if WinCopies3
        public static IEnumerable<T> GetNullCheckWhileEnumerable<T>(T first, Converter<T, T> converter) => new Generic.Enumerable<T>(() => Enumerator.GetNullCheckWhileEnumerator(first, converter));

        public static IEnumerable<T> GetNullCheckWhileEnumerableC<T>(T first, Converter<T, T> converter) => FromEnumerator(Enumerator.GetNullCheckWhileEnumerator(first, converter));
#endif
    }
}
