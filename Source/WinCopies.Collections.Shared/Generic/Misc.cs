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

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;

namespace WinCopies.Collections
{
#if WinCopies3
    public interface IEnumeratorInfo2 : IEnumeratorInfo, WinCopies.Collections.DotNetFix.IDisposableEnumeratorInfo, IDisposableEnumerator
    {
        // Left empty.
    }

    public interface ICountableEnumeratorInfo : IEnumeratorInfo2, ICountableEnumerator
    {
        // Left empty.
    }

    public interface ICountableDisposableEnumeratorInfo : ICountableDisposableEnumerator, ICountableEnumeratorInfo
    {
        // Left empty.
    }

    public interface IUIntCountableEnumeratorInfo : IEnumeratorInfo2, IUIntCountableEnumerator
    {
        // Left empty.
    }

    public interface IUIntCountableDisposableEnumeratorInfo : IUIntCountableDisposableEnumerator, IUIntCountableEnumeratorInfo
    {
        // Left empty.
    }
#endif

    namespace Generic
    {
        public interface IRecursiveEnumerableProviderEnumerable<out T> : System.Collections.Generic.IEnumerable<T>
        {
            System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>> GetRecursiveEnumerator();
        }

        public interface IRecursiveEnumerable<out T> : IRecursiveEnumerableProviderEnumerable<T>
        {
            T Value { get; }
        }

        public interface IEnumeratorInfo<out T> : System.Collections.Generic.IEnumerator<T>, IEnumeratorInfo
#if WinCopies3
        , WinCopies.DotNetFix.IDisposable
#endif
        {
            // Left empty.
        }

#if !WinCopies3
    public interface IDisposableEnumeratorInfo<out T> : IEnumeratorInfo<T>, IDisposableEnumeratorInfo
    {
        // Left empty.
    }
#endif

        public interface ICountableEnumeratorInfo<out T> :
#if WinCopies3
        IEnumeratorInfo2
#else
IEnumeratorInfo
#endif
        <T>, ICountableEnumerator<T>
        {
            // Left empty.
        }

        public interface ICountableDisposableEnumeratorInfo<out T> : ICountableDisposableEnumerator<T>, ICountableEnumeratorInfo<T>
#if !WinCopies3
, IDisposableEnumeratorInfo<T>
#else
        , IEnumeratorInfo<T>
#endif
        {
            // Left empty.
        }

        public interface IUIntCountableEnumeratorInfo<out T> :
#if WinCopies3
        IEnumeratorInfo2
#else
IEnumeratorInfo
#endif
        <T>, IUIntCountableEnumerator<T>
        {
            // Left empty.
        }

        public interface IUIntCountableDisposableEnumeratorInfo<out T> : IUIntCountableDisposableEnumerator<T>, IUIntCountableEnumeratorInfo<T>
#if !WinCopies3
, IDisposableEnumeratorInfo<T>
#else
        , IEnumeratorInfo<T>
#endif
        {
            // Left empty.
        }
    }
}
