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
using System.Linq;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;

using static WinCopies.
#if WinCopies3
    ThrowHelper
#else
    Util.Util;
using static WinCopies.Util.ThrowHelper;

using WinCopies.Util
#endif
    ;

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
        public abstract class DisposableEnumerableBase<T> : DotNetFix.Generic.IDisposableEnumerable<T>
        {
            public abstract bool IsDisposed { get; }

            protected abstract System.Collections.Generic.IEnumerator<T> GetEnumeratorOverride();

            public System.Collections.Generic.IEnumerator<T> GetEnumerator() => GetOrThrowIfDisposed(this, GetEnumeratorOverride());

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

            protected abstract void Dispose(bool disposing);

            ~DisposableEnumerableBase() => Dispose(false);

            public void Dispose()
            {
                if (IsDisposed)

                    return;

                Dispose(true);

                GC.SuppressFinalize(this);
            }
        }

        public abstract class DisposableEnumerable<T> : DisposableEnumerableBase<T>
        {
            private bool _isDisposed = false;

            public sealed override bool IsDisposed => _isDisposed;

            protected override void Dispose(bool disposing) => _isDisposed = true;
        }

        public class DisposableEnumerable2<T> : DisposableEnumerableBase<T>
        {
            private Action _onDispose;
            protected System.Collections.Generic.IEnumerable<T> InnerEnumerable { get; private set; }

            public override bool IsDisposed => InnerEnumerable == null;

            public DisposableEnumerable2(in System.Collections.Generic.IEnumerable<T> enumerable, in Action
#if CS8
                ?
#endif
                onDispose)
            {
                InnerEnumerable = enumerable ?? throw GetArgumentNullException(nameof(enumerable));
                _onDispose = onDispose ?? Delegates.EmptyVoid;
            }

            protected override System.Collections.Generic.IEnumerator<T> GetEnumeratorOverride() => InnerEnumerable.GetEnumerator();

            protected override void Dispose(bool disposing)
            {
                _onDispose();
                InnerEnumerable = null;
                _onDispose = null;
            }
        }

        public class DisposableEnumerable<TIn, TOut> : DisposableEnumerableBase<TOut> where TIn : TOut
        {
            protected IDisposableEnumerable<TIn> InnerEnumerable { get; private set; }

            public override bool IsDisposed => InnerEnumerable == null;

            public DisposableEnumerable(in IDisposableEnumerable<TIn> enumerable) => InnerEnumerable = enumerable ?? throw GetArgumentNullException(nameof(enumerable));

            protected override System.Collections.Generic.IEnumerator<TOut> GetEnumeratorOverride() => InnerEnumerable.OfType<TOut>().GetEnumerator();

            protected override void Dispose(bool disposing)
            {
                InnerEnumerable.Dispose();
                InnerEnumerable = null;
            }
        }

        public interface IRecursiveEnumerableProviderEnumerable<
#if CS5
            out
#endif
             T> :
#if WinCopies3 && CS8
            DotNetFix
#else
            System.Collections
#endif
            .Generic.IEnumerable<T>
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
