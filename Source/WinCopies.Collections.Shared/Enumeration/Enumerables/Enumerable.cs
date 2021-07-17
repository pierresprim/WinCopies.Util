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

using WinCopies.Collections.DotNetFix.Generic;

#if WinCopies3
using static WinCopies.ThrowHelper;
#endif

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
    namespace DotNetFix.Generic
    {
#if CS8
        public interface IEnumerable<out T> : System.Collections.Generic.IEnumerable<T>
        {
            System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
#endif

        public interface IEnumerable<out TItems, out TEnumerator> :
#if !CS8
            System.Collections.Generic.
#endif
            IEnumerable<TItems> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
        {
            new TEnumerator GetEnumerator();

#if CS8 && WinCopies3
            System.Collections.Generic.IEnumerator<TItems> System.Collections.Generic.IEnumerable<TItems>.GetEnumerator() => GetEnumerator();
#endif
        }
    }

    namespace Enumeration
    {
        namespace DotNetFix
        {
            public interface IEnumerable<out TEnumerator> : System.Collections.IEnumerable where TEnumerator : IEnumerator
            {
                new TEnumerator GetEnumerator();

#if CS8
                IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
#endif
            }
        }

        /// <summary>
        /// A collection that can be enumerated.
        /// </summary>
        public interface IEnumerable : System.Collections.IEnumerable
        {
            /// <summary>
            /// Gets a value indicating whether this collection can be enumerated in the two directions (FIFO and LIFO).
            /// </summary>
            bool SupportsReversedEnumeration { get; }

            /// <summary>
            /// Returns a reversed enumerator for the current collection. See the Remarks section.
            /// </summary>
            /// <returns>A reversed enumerator for the current collection.</returns>
            /// <exception cref="InvalidOperationException"><see cref="SupportsReversedEnumeration"/> is set to <see langword="false"/>.</exception>
            /// <remarks>
            /// This method returns an enumerator that enumerates in the reversed direction that the enumerator returned by the <see cref="System.Collections.Generic.IEnumerable{T}.GetEnumerator"/> method. So, for a queue, the <see cref="System.Collections.Generic.IEnumerable{T}.GetEnumerator"/> method will return an enumerator that will enumerate through the queue using the FIFO direction and the <see cref="GetReversedEnumerator"/> will throw an exception, because any reversed enumerator can be returned while a queue only supports the FIFO direction. However, a stack, which only supports the LIFO direction, will return a LIFO-enumerator as its main enumerator and throw an exception if we ask it to return a reversed enumerator. A linked list that supports the two directions, but which stores items using the FIFO direction by default, will return a FIFO-enumerator as its main enumerator and a LIFO-enumerator as its reversed enumerator.
            /// </remarks>
            System.Collections.IEnumerator GetReversedEnumerator();
        }

        public interface IEnumerable<out TEnumerator> : DotNetFix.IEnumerable<TEnumerator>, IEnumerable where TEnumerator : System.Collections.IEnumerator
        {
            new TEnumerator GetReversedEnumerator();

#if CS8
            System.Collections.IEnumerator IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#endif
        }
    }

    public interface IEnumerableInfo<out TEnumerator> : Enumeration.IEnumerable<TEnumerator> where TEnumerator : IEnumeratorInfo
    {
        // Left empty.
    }

    public interface IEnumerableInfo : IEnumerableInfo<IEnumeratorInfo>
    {
        // Left empty.
    }

    namespace Generic
    {
#endif
        public interface IDisposableEnumerable : System.Collections.IEnumerable, System.IDisposable
        {
            // Left empty.
        }

        public interface IDisposableEnumerable<out T> : System.Collections.Generic.IEnumerable<T>, System.IDisposable
        {
            // Left empty.
        }

        public abstract class EnumerableBase<TItems, TEnumerator> : System.Collections.Generic.IEnumerable<TItems> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
        {
            protected Func<TEnumerator> EnumeratorFunc { get; }

            protected EnumerableBase(in Func<TEnumerator> enumeratorFunc) => EnumeratorFunc = enumeratorFunc
#if WinCopies3
                ?? throw GetArgumentNullException(nameof(enumeratorFunc))
#endif
                ;

            public TEnumerator GetEnumerator() => EnumeratorFunc();

            System.Collections.Generic.IEnumerator<TItems> System.Collections.Generic.IEnumerable<TItems>.GetEnumerator() => EnumeratorFunc();

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public sealed class Enumerable<T> : EnumerableBase<T, System.Collections.Generic.IEnumerator<T>>
        {
            public Enumerable(in Func<System.Collections.Generic.IEnumerator<T>> enumeratorFunc) : base(enumeratorFunc)
            {
                // Left empty.
            }
        }

#if WinCopies3
        public abstract class EnumerableInfoBase<TItems, TEnumerator> : EnumerableBase<TItems, TEnumerator>, IEnumerableInfo<TItems> where TEnumerator : IEnumeratorInfo<TItems>
        {
            protected Func<TEnumerator> ReversedEnumeratorFunc { get; }

            public bool SupportsReversedEnumeration => ReversedEnumeratorFunc != null;

            /// <summary>
            /// Initializes a new instance of the <see cref="EnumerableInfoBase{TItems, TEnumerator}"/> class.
            /// </summary>
            /// <param name="enumeratorFunc">The func that provides the enumerators.</param>
            /// <param name="reversedEnumeratorFunc">The func that provides the reversed enumerators. This parameter can be null.</param>
            /// <exception cref="ArgumentNullException"><paramref name="enumeratorFunc"/> is null.</exception>
            protected EnumerableInfoBase(in Func<TEnumerator> enumeratorFunc, in Func<TEnumerator> reversedEnumeratorFunc) : base(enumeratorFunc) => ReversedEnumeratorFunc = reversedEnumeratorFunc;

            /// <summary>
            /// Returns a reversed enumerator for the current collection.
            /// </summary>
            /// <returns>A reversed enumerator for the current collection.</returns>
            /// <exception cref="InvalidOperationException"><see cref="Enumeration.IEnumerable.SupportsReversedEnumeration"/> is set to <see langword="false"/>.</exception>
            /// <seealso cref="IEnumerable{T}.GetReversedEnumerator"/>
            public TEnumerator GetReversedEnumerator() => (ReversedEnumeratorFunc ?? throw new InvalidOperationException("The current enumerable does not support reversed enumeration."))();

            IEnumeratorInfo<TItems> WinCopies.Collections.DotNetFix.Generic.IEnumerable<TItems, IEnumeratorInfo<TItems>>.GetEnumerator() => EnumeratorFunc();

            System.Collections.Generic.IEnumerator<TItems> IEnumerable<TItems>.GetReversedEnumerator() => GetReversedEnumerator(); // We call this method and not the delegate directly because we have to make a null-check.

            IEnumeratorInfo<TItems> IEnumerable<TItems, IEnumeratorInfo<TItems>>.GetReversedEnumerator() => GetReversedEnumerator();

            IEnumerator Enumeration.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
        }

        public sealed class EnumerableInfo<T> : EnumerableInfoBase<T, IEnumeratorInfo<T>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="EnumerableInfo{T}"/> class.
            /// </summary>
            /// <param name="enumeratorFunc">The func that provides the enumerators.</param>
            /// <param name="reversedEnumeratorFunc">The func that provides the reversed enumerators. This parameter can be null.</param>
            /// <exception cref="ArgumentNullException"><paramref name="enumeratorFunc"/> is null.</exception>
            public EnumerableInfo(in Func<IEnumeratorInfo<T>> enumeratorFunc, in Func<IEnumeratorInfo<T>> reversedEnumeratorFunc) : base(enumeratorFunc, reversedEnumeratorFunc)
            {
                // Left empty.
            }

            public EnumerableInfo(in IEnumerableInfo<T> enumerable) : base(enumerable.GetEnumerator, enumerable.GetReversedEnumerator)
            {
                // Left empty.
            }
        }

        /// <summary>
        /// A collection that can be enumerated.
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        public interface IEnumerable<out T> :
#if CS8
            WinCopies.Collections.DotNetFix
#else
            System.Collections
#endif
            .Generic.IEnumerable<T>
#if WinCopies3
            , Enumeration.IEnumerable
        {
#if CS8
            IEnumerator Enumeration.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#endif
#else
        {
            /// <summary>
            /// Gets a value indicating whether this collection can be enumerated in the two directions (FIFO and LIFO).
            /// </summary>
            bool SupportsReversedEnumeration { get; }
#endif

            /// <summary>
            /// Returns a reversed enumerator for the current collection. See the Remarks section.
            /// </summary>
            /// <returns>A reversed enumerator for the current collection.</returns>
            /// <exception cref="InvalidOperationException"><see cref="SupportsReversedEnumeration"/> is set to <see langword="false"/>.</exception>
            /// <remarks>
            /// This method returns an enumerator that enumerates in the reversed direction that the enumerator returned by the <see cref="System.Collections.Generic.IEnumerable{T}.GetEnumerator"/> method. So, for a queue, the <see cref="System.Collections.Generic.IEnumerable{T}.GetEnumerator"/> method will return an enumerator that will enumerate through the queue using the FIFO direction and the <see cref="GetReversedEnumerator"/> will throw an exception, because any reversed enumerator can be returned while a queue only supports the FIFO direction. However, a stack, which only supports the LIFO direction, will return a LIFO-enumerator as its main enumerator and throw an exception if we ask it to return a reversed enumerator. A linked list that supports the two directions, but which stores items using the FIFO direction by default, will return a FIFO-enumerator as its main enumerator and a LIFO-enumerator as its reversed enumerator.
            /// </remarks>
            System.Collections.Generic.IEnumerator<T> GetReversedEnumerator();
        }

        public interface IEnumerable<out TItems, out TEnumerator> : DotNetFix.Generic.IEnumerable<TItems, TEnumerator>, IEnumerable<TItems> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
        {
            new TEnumerator GetReversedEnumerator();

#if CS8
            System.Collections.Generic.IEnumerator<TItems> IEnumerable<TItems>.GetReversedEnumerator() => GetReversedEnumerator();
#endif
        }

        public interface IEnumerableInfo<out TItems, out TEnumerator> : IEnumerable<TItems, TEnumerator> where TEnumerator : IEnumeratorInfo<TItems>
        {
            // Left empty.
        }

        public interface IEnumerableInfo<out T> : IEnumerableInfo<T, IEnumeratorInfo<T>>
        {
            // Left empty.
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
