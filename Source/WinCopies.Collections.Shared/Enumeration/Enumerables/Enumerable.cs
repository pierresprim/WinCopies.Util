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
        private readonly Func<System.Collections.IEnumerator> _enumeratorFunc;

        public Enumerable(Func<System.Collections.IEnumerator> enumeratorFunc) => _enumeratorFunc = enumeratorFunc;

        public System.Collections.IEnumerator GetEnumerator() => _enumeratorFunc();

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

#if WinCopies3
    namespace Generic
    {
#endif
        public sealed class Enumerable<T> : System.Collections.Generic.IEnumerable<T>
        {
            private readonly Func<System.Collections.Generic.IEnumerator<T>> _enumeratorFunc;

            public Enumerable(Func<System.Collections.Generic.IEnumerator<T>> enumeratorFunc) => _enumeratorFunc = enumeratorFunc;

            public System.Collections.Generic.IEnumerator<T> GetEnumerator() => _enumeratorFunc();

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

#if WinCopies3
        /// <summary>
        /// A collection that can be enumerated.
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        public interface IEnumerable<out T> : System.Collections.Generic.IEnumerable<T>
        {
            /// <summary>
            /// Gets a value indicating whether this collection can be enumerated in the two directions (FIFO and LIFO).
            /// </summary>
            bool SupportsReversedEnumeration { get; }

            /// <summary>
            /// Returns a reversed enumerator for the current collection. See the Remarks section.
            /// </summary>
            /// <returns>A reversed enumerator for the current collection.</returns>
            /// <remarks>
            /// This method returns an enumerator which enumerate in the reversed direction that the enumerator returned by the <see cref="System.Collections.Generic.IEnumerable{T}.GetEnumerator"/> method. So, for a queue, the <see cref="System.Collections.Generic.IEnumerable{T}.GetEnumerator"/> method will return an enumerator that will enumerate through the queue using the FIFO direction and the <see cref="GetReversedEnumerator"/> will throw an exception, because any reversed enumerator can be returned while a queue only supports the FIFO direction. However, a stack, which only supports the LIFO direction, will return a LIFO-enumerator as its main enumerator and throw an exception if we ask it to return a reversed enumerator. A linked list that supports the two directions, but which stores items using the FIFO direction by default, will return a FIFO-enumerator as its main enumerator and a LIFO-enumerator as its reversed enumerator.
            /// </remarks>
            System.Collections.Generic.IEnumerator<T> GetReversedEnumerator();
        }

        public interface IEnumerableInfo<out T> : IEnumerable<T>
        {
            new IEnumeratorInfo2<T> GetEnumerator();

            new IEnumeratorInfo2<T> GetReversedEnumerator();
        }
    }
#endif

    //public sealed class EmptyCheckEnumerable : IEnumerable
    //{
    //    private Func<EmptyCheckEnumerator> _func;

    //    public EmptyCheckEnumerable(Func<EmptyCheckEnumerator> func) => _func = func;

    //    public System.Collections.IEnumerator GetEnumerator() => _func();
    //}

    //public sealed class EmptyCheckEnumerable<T> : System.Collections.Generic.IEnumerable<T>
    //{
    //    private readonly Func<EmptyCheckEnumerator<T>> _func;

    //    public EmptyCheckEnumerable(Func<EmptyCheckEnumerator<T>> func) => _func = func;

    //    public System.Collections.Generic.IEnumerator<T> GetEnumerator() => _func();

    //    System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    //}
}
