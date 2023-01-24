/* Copyright © Pierre Sprimont, 2022
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

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Linq
{
    public static class Enumerable
    {
        public static IEnumerable<TResult> ForEachIfNotNull<TIn, TOut, TResult>(TIn value, Converter<TIn, IEnumerable<TOut>> converter, ExtendedConverter<TOut, TResult> extendedConverter)
        {
            if (value != null)

                foreach (TOut item in converter(value))

                    if (extendedConverter(item, out TResult result))

                        yield return result;
        }
    }

    public static class Enumerator
    {
        private class EmptyEnumerator : IEnumerator
        {
            private static EmptyEnumerator
#if CS8
                ?
#endif
                _instance;
            private static Func<EmptyEnumerator> _instanceFunc = () =>
            {
                _instance = new EmptyEnumerator();

                return (_instanceFunc = () => _instance)();
            };

            public static EmptyEnumerator Instance => _instanceFunc();

            object IEnumerator.Current => throw GetEmptyListOrCollectionException();

            bool IEnumerator.MoveNext() => false;

            void IEnumerator.Reset() { /* Left empty. */ }
        }

        private sealed class EmptyEnumerator<T> : EmptyEnumerator,
#if CS8
            Collections.DotNetFix
#else
            System.Collections
#endif
            .Generic.IEnumerator<T>
        {
            private static EmptyEnumerator<T>
#if CS8
                ?
#endif
                _instance;
            private static Func<EmptyEnumerator<T>> _instanceFunc = () =>
            {
                _instance = new EmptyEnumerator<T>();

                return (_instanceFunc = () => _instance)();
            };

            public static new EmptyEnumerator<T> Instance => _instanceFunc();

            T IEnumerator<T>.Current => throw GetEmptyListOrCollectionException();

            public bool MoveNext() => false;

            public void Reset() { /* Left empty. */ }

            void System.IDisposable.Dispose() { /* Left empty. */ }
        }

        public static IEnumerator Empty() => EmptyEnumerator.Instance;
        public static IEnumerator<T> Empty<T>() => EmptyEnumerator<T>.Instance;
    }
}
