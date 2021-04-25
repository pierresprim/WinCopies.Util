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

using System.Collections;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;

namespace WinCopies.Collections.DotNetFix
{
    public interface ILongCountableEnumerable
#if WinCopies3
<out TEnumerator> : Enumeration.DotNetFix.IEnumerable<TEnumerator>, ILongCountable where TEnumerator : ILongCountableEnumerator
#else
        : IEnumerable
#endif
    {
#if WinCopies3
        // Left empty.
    }

    public interface ILongCountableEnumerable : ILongCountableEnumerable<ILongCountableEnumerator>
    {
        // Left empty.
#else
        int Count { get; }
#endif
    }

    public interface IULongCountableEnumerable
#if WinCopies3
    <out TEnumerator> : Enumeration.DotNetFix.IEnumerable<TEnumerator>, IULongCountable where TEnumerator : IULongCountableEnumerator
#else
: IEnumerable
#endif
    {
#if WinCopies3
        // Left empty.
    }

    public interface IULongCountableEnumerable : IULongCountableEnumerable<IULongCountableEnumerator>
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
    public interface ILongCountableEnumerable
#if WinCopies3
        <out TItems, out TEnumerator> : DotNetFix.ILongCountableEnumerable<TEnumerator>, IEnumerable<TItems, TEnumerator>
#if CS7
, System.Collections.Generic.IReadOnlyCollection<TItems>
#endif
             where TEnumerator : ILongCountableEnumerator<TItems>
#else
        <out T> : System.Collections.Generic.IEnumerable<T>, ILongCountableEnumerable
#endif
    {
#if WinCopies3
#if CS7
            new int Count { get; }
#endif

            new TEnumerator GetEnumerator();

#if CS8
            int System.Collections.Generic.IReadOnlyCollection<TItems>.Count => Count;

            int ILongCountable.Count => Count;

            TEnumerator IEnumerable<TItems, TEnumerator>.GetEnumerator() => GetEnumerator();

            TEnumerator Enumeration.DotNetFix.IEnumerable<TEnumerator>.GetEnumerator() => GetEnumerator();

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
#endif
        }

        public interface ILongCountableEnumerable<out T> : ILongCountableEnumerable<T, ILongCountableEnumerator<T>>
        {
            // Left empty.
        }

        public interface ILongCountableDisposableEnumerable<out TItems, out TEnumerator> : ILongCountableEnumerable<TItems, TEnumerator> where TEnumerator : ILongCountableEnumerator<TItems>, WinCopies.DotNetFix.IDisposable
        {
            // Left empty.
        }

        public interface ILongCountableDisposableEnumerable<out T> : ILongCountableDisposableEnumerable<T, ILongCountableDisposableEnumerator<T>>
        {
#endif
        // Left empty.
    }

    public interface IULongCountableEnumerable
#if WinCopies3
       <out TItems, out TEnumerator> : DotNetFix.IULongCountableEnumerable<TEnumerator>, IEnumerable<TItems, TEnumerator> where TEnumerator : IULongCountableEnumerator<TItems>
#else
<out T> : System.Collections.Generic.IEnumerable<T>, IULongCountableEnumerable
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

        public interface IULongCountableEnumerable<out T> : IULongCountableEnumerable<T, IULongCountableEnumerator<T>>
        {
            // Left empty.
        }

        public interface IULongCountableDisposableEnumerable<out TItems, out TEnumerator> : IULongCountableEnumerable<TItems, TEnumerator> where TEnumerator : IULongCountableEnumerator<TItems>, WinCopies.DotNetFix.IDisposable
        {
            // Left empty.
        }

        public interface IULongCountableDisposableEnumerable<out T> : IULongCountableDisposableEnumerable<T, IULongCountableDisposableEnumerator<T>>
        {
#endif
        // Left empty.
    }

    // TODO:
#if ILongCountableEnumerableInfo
        public interface ILongCountableEnumerableInfo<out T> : ILongCountableEnumerable<T, ILongCountableEnumeratorInfo<T>>, IEnumerableInfo<T, ILongCountableEnumeratorInfo<T>>
        {
            // Left empty.
        }

        public interface IULongCountableEnumerableInfo<out T> : IULongCountableEnumerable<T, IULongCountableEnumeratorInfo<T>>, IEnumerableInfo<T, IULongCountableEnumeratorInfo<T>>
        {
            // Left empty.
        }
#endif
#if WinCopies3
    }
#endif
}
