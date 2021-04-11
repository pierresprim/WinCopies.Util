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

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;

namespace WinCopies.Collections.DotNetFix
{
    public interface ICountableEnumerable
#if WinCopies3
<out TEnumerator> : Enumeration.DotNetFix.IEnumerable<TEnumerator>, ICountable where TEnumerator : ICountableEnumerator
#else
        : IEnumerable
#endif
    {
#if WinCopies3
        // Left empty.
    }

    public interface ICountableEnumerable : ICountableEnumerable<ICountableEnumerator>
    {
        // Left empty.
#else
        int Count { get; }
#endif
    }

    public interface IUIntCountableEnumerable
#if WinCopies3
    <out TEnumerator> : Enumeration.DotNetFix.IEnumerable<TEnumerator>, IUIntCountable where TEnumerator : IUIntCountableEnumerator
#else
: IEnumerable
#endif
    {
#if WinCopies3
        // Left empty.
    }

    public interface IUIntCountableEnumerable : IUIntCountableEnumerable<IUIntCountableEnumerator>
    {
        // Left empty.
#else
        uint Count { get; }
#endif
    }

#if WinCopies3
    namespace Generic
    {
#endif
        public interface ICountableEnumerable
#if WinCopies3
        <out TItems, out TEnumerator> : DotNetFix.ICountableEnumerable<TEnumerator>, IEnumerable<TItems, TEnumerator>
#if CS7
, System.Collections.Generic.IReadOnlyCollection<TItems>
#endif
             where TEnumerator : ICountableEnumerator<TItems>
#else
        <out T> : System.Collections.Generic.IEnumerable<T>, ICountableEnumerable
#endif
        {
#if WinCopies3
#if CS7
            new int Count { get; }
#endif

            new TEnumerator GetEnumerator();

#if CS8
            int System.Collections.Generic.IReadOnlyCollection<TItems>.Count => Count;

            int ICountable.Count => Count;

            TEnumerator IEnumerable<TItems, TEnumerator>.GetEnumerator() => GetEnumerator();

            TEnumerator Enumeration.DotNetFix.IEnumerable<TEnumerator>.GetEnumerator() => GetEnumerator();

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
#endif
        }

        public interface ICountableEnumerable<out T> : ICountableEnumerable<T, ICountableEnumerator<T>>
        {
            // Left empty.
        }

        public interface ICountableDisposableEnumerable<out TItems, out TEnumerator> : ICountableEnumerable<TItems, TEnumerator> where TEnumerator : ICountableEnumerator<TItems>, WinCopies.DotNetFix.IDisposable
        {
            // Left empty.
        }

        public interface ICountableDisposableEnumerable<out T> : ICountableDisposableEnumerable<T, ICountableDisposableEnumerator<T>>
        {
#endif
            // Left empty.
        }

        public interface IUIntCountableEnumerable
#if WinCopies3
       <out TItems, out TEnumerator> : DotNetFix.IUIntCountableEnumerable<TEnumerator>, IEnumerable<TItems, TEnumerator> where TEnumerator : IUIntCountableEnumerator<TItems>
#else
<out T> : System.Collections.Generic.IEnumerable<T>, IUIntCountableEnumerable
#endif
        {
#if WinCopies3
            new TEnumerator GetEnumerator();

#if CS8
            TEnumerator IEnumerable<TItems, TEnumerator>.GetEnumerator() => GetEnumerator();

            TEnumerator Enumeration.DotNetFix.IEnumerable<TEnumerator>.GetEnumerator() => GetEnumerator();

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
#endif
        }

        public interface IUIntCountableEnumerable<out T> : IUIntCountableEnumerable<T, IUIntCountableEnumerator<T>>
        {
            // Left empty.
        }

        public interface IUIntCountableDisposableEnumerable<out TItems, out TEnumerator> : IUIntCountableEnumerable<TItems, TEnumerator> where TEnumerator : IUIntCountableEnumerator<TItems>, WinCopies.DotNetFix.IDisposable
        {
            // Left empty.
        }

        public interface IUIntCountableDisposableEnumerable<out T> : IUIntCountableDisposableEnumerable<T, IUIntCountableDisposableEnumerator<T>>
        {
#endif
            // Left empty.
        }

#if WinCopies3
        public interface ICountableEnumerableInfo<out T> : ICountableEnumerable<T, ICountableEnumeratorInfo<T>>, IEnumerableInfo<T, ICountableEnumeratorInfo<T>>
        {
            // Left empty.
        }

        public interface IUIntCountableEnumerableInfo<out T> : IUIntCountableEnumerable<T, IUIntCountableEnumeratorInfo<T>>, IEnumerableInfo<T, IUIntCountableEnumeratorInfo<T>>
        {
            // Left empty.
        }
    }
#endif
}
