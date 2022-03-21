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
using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;

namespace WinCopies.Collections
{
#if WinCopies3
    public interface IEnumeratorInfo2 : IEnumeratorInfo, DotNetFix.IDisposableEnumeratorInfo, IDisposableEnumerator
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
        public abstract class DisposableEnumerable<T> : IDisposableEnumerable<T>, WinCopies.
#if !WinCopies3
            Util.
#endif
            DotNetFix.IDisposable
        {
            public bool IsDisposed { get; private set; }

            protected abstract System.Collections.Generic.IEnumerator<T> GetEnumeratorOverride();

            public System.Collections.Generic.IEnumerator<T> GetEnumerator() => WinCopies.
#if !WinCopies3
                Util.
#endif
                ThrowHelper.GetOrThrowIfDisposed(this, GetEnumeratorOverride());

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

            protected virtual void Dispose(bool disposing) => IsDisposed = true;

            ~DisposableEnumerable() => Dispose(false);

            public void Dispose()
            {
                if (IsDisposed)

                    return;

                Dispose(true);

                GC.SuppressFinalize(this);
            }
        }

        public interface IRecursiveEnumerableProviderEnumerable<
#if CS5
            out
#endif
             T> : System.Collections.Generic.IEnumerable<T>
        {
            System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>> GetRecursiveEnumerator();
        }

        public interface IRecursiveEnumerable<
#if CS5
            out
#endif
             T> : IRecursiveEnumerableProviderEnumerable<T>
        {
            T Value { get; }
        }

        public interface IEnumeratorInfo<
#if CS5
            out
#endif
            T> : System.Collections.Generic.IEnumerator<T>, IEnumeratorInfo
        {
            // Left empty.
        }

#if !WinCopies3
        public interface IDisposableEnumeratorInfo<
#if CS5
            out
#endif
             T> : IEnumeratorInfo<T>, IDisposableEnumeratorInfo
        {
            // Left empty.
        }
#endif

        public interface ICountableEnumeratorInfo<
#if CS5
            out
#endif
             T> : IEnumeratorInfo<T>, ICountableEnumerator<T>
        {
            // Left empty.
        }

        public interface ICountableDisposableEnumeratorInfo<
#if CS5
            out
#endif
             T> : ICountableDisposableEnumerator<T>, ICountableEnumeratorInfo<T>
#if !WinCopies3
, IDisposableEnumeratorInfo<T>
#else
        , IEnumeratorInfo<T>
#endif
        {
            // Left empty.
        }

        public interface IUIntCountableEnumeratorInfo<
#if CS5
            out
#endif
             T> : IEnumeratorInfo<T>, IUIntCountableEnumerator<T>
        {
            // Left empty.
        }

        public interface IUIntCountableDisposableEnumeratorInfo<
#if CS5
            out
#endif
             T> : IUIntCountableDisposableEnumerator<T>, IUIntCountableEnumeratorInfo<T>
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
