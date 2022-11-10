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
using WinCopies.Collections.Enumeration.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Util;

namespace WinCopies.Collections.DotNetFix
{
    public interface ILongCountableEnumerable<out TEnumerator> : Enumeration.IEnumerable<TEnumerator>, ILongCountable where TEnumerator : ILongCountableEnumerator
    {
        // Left empty.
    }

    public interface ILongCountableEnumerable : ILongCountableEnumerable<ILongCountableEnumerator>
    {
        // Left empty.
    }

    public interface IULongCountableEnumerable<out TEnumerator> : Enumeration.IEnumerable<TEnumerator>, IULongCountable where TEnumerator : IULongCountableEnumerator
    {
        // Left empty.
    }

    public interface IULongCountableEnumerable : IULongCountableEnumerable<IULongCountableEnumerator>
    {
        // Left empty.
    }

    namespace Generic
    {
        public interface ILongCountableEnumerable
            <
#if CS5
            out
#endif
            TItems, out TEnumerator> : DotNetFix.ILongCountableEnumerable<TEnumerator>, IEnumerable<TItems, TEnumerator>
#if CS7
            , System.Collections.Generic.IReadOnlyCollection<TItems>
#endif
            where TEnumerator : ILongCountableEnumerator<TItems>
        {
#if CS8
            int System.Collections.Generic.IReadOnlyCollection<TItems>.Count => (int)this.AsFromType<ILongCountable>().Count;
#endif
        }

        public interface ILongCountableEnumerable<
#if CS5
            out
#endif
             T> : ILongCountableEnumerable<T, ILongCountableEnumerator<T>>
        {
            // Left empty.
        }

        public interface ILongCountableDisposableEnumerable<
#if CS5
            out
#endif
             TItems, out TEnumerator> : ILongCountableEnumerable<TItems, TEnumerator> where TEnumerator : ILongCountableEnumerator<TItems>, WinCopies.DotNetFix.IDisposable
        {
            // Left empty.
        }

        public interface ILongCountableDisposableEnumerable<
#if CS5
            out
#endif
             T> : ILongCountableDisposableEnumerable<T, ILongCountableDisposableEnumerator<T>>
        {
            // Left empty.
        }

        public interface IULongCountableEnumerable<
#if CS5
                out
#endif
                 TItems, out TEnumerator> : DotNetFix.IULongCountableEnumerable<TEnumerator>, IEnumerable<TItems, TEnumerator> where TEnumerator : IULongCountableEnumerator<TItems>
        {
            // Left empty.
        }

        public interface IULongCountableEnumerable<
#if CS5
            out
#endif
             T> : IULongCountableEnumerable<T, IULongCountableEnumerator<T>>
        {
            // Left empty.
        }

        public interface IULongCountableDisposableEnumerable<
#if CS5
            out
#endif
             TItems, out TEnumerator> : IULongCountableEnumerable<TItems, TEnumerator> where TEnumerator : IULongCountableEnumerator<TItems>, WinCopies.DotNetFix.IDisposable
        {
            // Left empty.
        }

        public interface IULongCountableDisposableEnumerable<
#if CS5
            out
#endif
             T> : IULongCountableDisposableEnumerable<T, IULongCountableDisposableEnumerator<T>>
        {
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
    }
}
