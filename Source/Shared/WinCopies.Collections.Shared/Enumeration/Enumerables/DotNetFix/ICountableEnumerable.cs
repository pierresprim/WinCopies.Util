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

using System.Collections;

using WinCopies.Collections.Generic;
using WinCopies.Util;

namespace WinCopies.Collections.DotNetFix
{
    public interface ICountableEnumerable<out TEnumerator> : Enumeration.IEnumerable<TEnumerator>, ICountable where TEnumerator : ICountableEnumerator
    {
        // Left empty.
    }

    public interface ICountableEnumerable : ICountableEnumerable<ICountableEnumerator>
    {
        // Left empty.
    }

    public interface IUIntCountableEnumerable<out TEnumerator> : Enumeration.IEnumerable<TEnumerator>, IUIntCountable where TEnumerator : IUIntCountableEnumerator
    {
        // Left empty.
    }

    public interface IUIntCountableEnumerable : IUIntCountableEnumerable<IUIntCountableEnumerator>
    {
        // Left empty.
    }

    namespace Generic
    {
        public interface ICountableEnumerable
        <
#if CS5
            out
#endif
            TItems, out TEnumerator> : DotNetFix.ICountableEnumerable<TEnumerator>, Enumeration.Generic.IEnumerable<TItems, TEnumerator>
#if CS7
            , System.Collections.Generic.IReadOnlyCollection<TItems>
#endif
            where TEnumerator : ICountableEnumerator<TItems>
        {
#if CS8
            int System.Collections.Generic.IReadOnlyCollection<TItems>.Count => this.AsFromType<ICountable>().Count;
#endif
        }

        public interface ICountableEnumerable<
#if CS5
            out
#endif
             T> : ICountableEnumerable<T, ICountableEnumerator<T>>
        {
            // Left empty.
        }

        public interface ICountableDisposableEnumerable<
#if CS5
            out
#endif
             TItems, out TEnumerator> : ICountableEnumerable<TItems, TEnumerator> where TEnumerator : ICountableEnumerator<TItems>, WinCopies.DotNetFix.IDisposable
        {
            // Left empty.
        }

        public interface ICountableDisposableEnumerable<
#if CS5
            out
#endif
             T> : ICountableDisposableEnumerable<T, ICountableDisposableEnumerator<T>>
        {
            // Left empty.
        }

        public interface IReadOnlyArray<
#if CS5
                out
#endif
                T> : WinCopies.Collections.DotNetFix.Generic.ICountableEnumerable<T>
        {
            T this[int index] { get; }
        }

        public interface IArray<T> : Collections.Extensions.Generic.IReadOnlyList<T>, IReadOnlyArray<T>
        {
            new T this[int index] { get; set; }
        }

        public interface IUIntCountableEnumerable
            <
#if CS5
            out
#endif
            TItems, out TEnumerator> : DotNetFix.IUIntCountableEnumerable<TEnumerator>, Enumeration.Generic.IEnumerable<TItems, TEnumerator> where TEnumerator : IUIntCountableEnumerator<TItems>
        {
            // Left empty.
        }

        public interface IUIntCountableEnumerable<
#if CS5
            out
#endif
             T> : IUIntCountableEnumerable<T, IUIntCountableEnumerator<T>>
        {
#if CS8
            System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => this.AsFromType<Enumeration.IEnumerable<IUIntCountableEnumerator<T>>>().GetEnumerator();
#endif
        }

        public interface IUIntCountableDisposableEnumerable<
#if CS5
            out
#endif
             TItems, out TEnumerator> : IUIntCountableEnumerable<TItems, TEnumerator> where TEnumerator : IUIntCountableEnumerator<TItems>, WinCopies.DotNetFix.IDisposable
        {
            // Left empty.
        }

        public interface IUIntCountableDisposableEnumerable<
#if CS5
            out
#endif
             T> : IUIntCountableDisposableEnumerable<T, IUIntCountableDisposableEnumerator<T>>
        {
            // Left empty.
        }

        public interface ICountableEnumerableInfo<
#if CS5
            out
#endif
             T> : ICountableEnumerable<T, ICountableEnumeratorInfo<T>>, IEnumerableInfo<T, ICountableEnumeratorInfo<T>>
        {
            // Left empty.
        }

        public interface IUIntCountableEnumerableInfo<
#if CS5
            out
#endif
             T> : IUIntCountableEnumerable<T, IUIntCountableEnumeratorInfo<T>>, IEnumerableInfo<T, IUIntCountableEnumeratorInfo<T>>
        {
            // Left empty.
        }
    }
}
