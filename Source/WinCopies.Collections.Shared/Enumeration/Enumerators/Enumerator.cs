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
using System.Collections.Generic;
using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using static WinCopies.
#if !WinCopies3
    Util.Util;
#else
ThrowHelper;
#endif

namespace WinCopies.Collections
{
#if WinCopies3
    public abstract class EnumeratorInfoBase : WinCopies.Collections.DotNetFix.IDisposableEnumeratorInfo, IEnumeratorBase
    {
        public bool IsStarted { get; private set; }

        public bool IsCompleted { get; private set; }

        public abstract bool? IsResetSupported { get; }

        public bool IsDisposed { get; private set; }

        protected void ThrowIfDisposed()
        {
            if (IsDisposed)

                throw GetExceptionForDispose(false);
        }

        protected T GetOrThrowIfDisposed<T>(in T value) => WinCopies.ThrowHelper.GetOrThrowIfDisposed(this, value);

        public bool MoveNext()
        {
            ThrowIfDisposed();

            if (IsCompleted)

                return false;

            if (MoveNextOverride())
            {
                IsStarted = true;

                return true;
            }

            OnMoveNextCompleted();

            return false;
        }

        protected abstract bool MoveNextOverride();

        protected virtual void OnMoveNextCompleted()
        {
            IsCompleted = true;

            IsStarted = false;

            ResetCurrent();
        }

        protected virtual void ResetCurrent()
        {
            // Left empty.
        }

        protected virtual void ResetOverride() => OnResetOrDisposed();

        protected virtual void OnResetOrDisposed()
        {
            IsStarted = false;

            IsCompleted = false;

            ResetCurrent();
        }

        public void Reset()
        {
            ThrowIfDisposed();

            if (IsResetSupported == false)

                throw new InvalidOperationException("The current enumerator does not support resetting.");

            if (IsStarted)

                ResetOverride();
        }

        protected virtual void DisposeManaged() => OnResetOrDisposed();

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)

                DisposeManaged();

            IsDisposed = true;
        }

        public void Dispose()
        {
            if (IsDisposed)

                return;

            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
#endif

    public abstract class Enumerator :
#if !WinCopies3
System.Collections.IEnumerator, WinCopies.Util.DotNetFix.IDisposable
#else
        EnumeratorInfoBase, IDisposableEnumerator, IDisposableEnumeratorInfo
#endif
    {
        public object Current
#if !WinCopies3
        {
            get
#endif
            => IsDisposed ? throw GetExceptionForDispose(false) : IsStarted ?
#if !WinCopies3
                _current 
#else
                CurrentOverride
#endif
                : throw new InvalidOperationException("The enumeration has not been started or has completed.");

#if !WinCopies3
            protected set => _current = IsDisposed ? throw GetExceptionForDispose(false) : value;
        }
#endif

#if !WinCopies3
        private object _current;

        public bool IsStarted { get; private set; }

        public bool IsCompleted { get; private set; }

        public bool IsDisposed { get; private set; }

        public bool MoveNext()
        {
            if (IsDisposed ? throw GetExceptionForDispose(false) : MoveNextOverride())
            {
                IsStarted = true;

                return true;
            }

            IsCompleted = true;

            IsStarted = false;

            _current = default;

            return false;
        }

        protected abstract bool MoveNextOverride();

        public void Reset()
        {
            if (IsDisposed)

                throw GetExceptionForDispose(false);

            if (IsStarted)
            {
                ResetOverride();

                IsStarted = false;

                IsCompleted = false;

                _current = default;
            }
        }

        protected virtual void ResetOverride()
        {
            IsStarted = false;

            IsCompleted = false;

            _current = default;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)

                IsDisposed = true;
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                Dispose(disposing: true);

                GC.SuppressFinalize(this);
            }
        }
#else
        /// <summary>
        /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
        /// </summary>
        protected abstract object CurrentOverride { get; }
#endif
    }

#if WinCopies3
    /// <summary>
    /// <see cref="System.Collections.IEnumerator"/> wrapper that inherits from <see cref="Enumerator"/>, a class that extends the info provided by the <see cref="System.Collections.IEnumerator"/> interface.
    /// </summary>
    public class EnumeratorInfo : Enumerator
    {
        private System.Collections.IEnumerator _innerEnumerator;

        /// <summary>
        /// The <see cref="System.Collections.IEnumerator"/> to enumerate.
        /// </summary>
        protected System.Collections.IEnumerator InnerEnumerator => IsDisposed ? throw GetExceptionForDispose(false) : _innerEnumerator;

        /// <summary>
        /// Always returns <see langword="null"/>.
        /// </summary>
        public override bool? IsResetSupported => null;

        /// <summary>
        /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
        /// </summary>
        protected override object CurrentOverride => _innerEnumerator.Current;

        protected override void ResetCurrent()
        {
            // Left empty.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumeratorInfo"/> class.
        /// </summary>
        /// <param name="enumerator">The enumerator to enumerate.</param>
        public EnumeratorInfo(in System.Collections.IEnumerator enumerator) => _innerEnumerator = enumerator;

        /// <summary>
        /// Calls the <see cref="System.Collections.IEnumerator.MoveNext"/> method on <see cref="Enumerator"/>.
        /// </summary>
        /// <returns>A value indicating whether a value has been retrieved.</returns>
        protected override bool MoveNextOverride() => _innerEnumerator.MoveNext();

        /// <summary>
        /// Calls the <see cref="System.Collections.IEnumerator.Reset"/> method on <see cref="Enumerator"/>.
        /// </summary>
        protected override void ResetOverride()
        {
            base.ResetOverride();

            _innerEnumerator.Reset();
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();

            _innerEnumerator = null;
        }
    }
#endif

#if WinCopies3
    namespace Generic
    {
#endif
        public abstract class Enumerator<T> :
#if !WinCopies3
System.Collections.Generic.IEnumerator<T>, WinCopies.Util.DotNetFix.IDisposable
#else
           EnumeratorInfoBase, IDisposableEnumerator<T>, IDisposableEnumeratorInfo, IEnumeratorInfo2<T>
#endif
        {
#if !WinCopies3
            private T _current;

            public bool IsDisposed { get; private set; }
#endif

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <exception cref="InvalidOperationException">The enumerator is disposed.</exception>
            public T Current
#if !WinCopies3
            {
                get
#endif
                => IsDisposed ? throw GetExceptionForDispose(false) :
#if !WinCopies3
_enumerationStarted
#else
                    IsStarted
#endif
                    ? CurrentOverride : throw new InvalidOperationException("The enumeration has not been started or has completed.");
#if !WinCopies3
                protected set => _current = IsDisposed ? throw GetExceptionForDispose(false) : value;
            }
#endif

            /// <summary>
            /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
            /// </summary>
            protected abstract T CurrentOverride { get; }

            object System.Collections.IEnumerator.Current => Current;

#if !WinCopies3
        private bool _enumerationStarted = false;

        protected abstract bool MoveNextOverride();

        public bool MoveNext()
        {
            if (IsDisposed ? throw GetExceptionForDispose(false) : MoveNextOverride())
            {
                _enumerationStarted = true;

                return true;
            }

            _enumerationStarted = false;

            _current = default;

            return false;
        }

        public void Reset()
        {
            if (IsDisposed)

                throw GetExceptionForDispose(false);

            ResetOverride();
        }

        protected virtual void ResetOverride()
        {
            _current = default;

            _enumerationStarted = false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)

                IsDisposed = true;
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                Dispose(true);

                GC.SuppressFinalize(this);
            }
        }
#endif
        }

#if WinCopies3
        public interface IEnumeratorInfo2<out T> : IEnumeratorInfo<T>, WinCopies.Collections.DotNetFix.IDisposableEnumeratorInfo, IDisposableEnumerator<T>, IDisposableEnumeratorInfo
        {
            // Left empty.
        }

        public class EnumeratorInfo<T> : Enumerator<T>
        {
            private System.Collections.Generic.IEnumerator<T> _innerEnumerator;

            protected System.Collections.Generic.IEnumerator<T> InnerEnumerator => IsDisposed ? throw GetExceptionForDispose(false) : _innerEnumerator;

            /// <summary>
            /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
            /// </summary>
            protected override T CurrentOverride => InnerEnumerator.Current; // The disposed check is performed in the InnerEnumerator property.

            public override bool? IsResetSupported => null;

            public EnumeratorInfo(in System.Collections.Generic.IEnumerator<T> enumerator) => _innerEnumerator = enumerator;

            public EnumeratorInfo(in System.Collections.Generic.IEnumerable<T> enumerable) : this(enumerable.GetEnumerator()) { /* Left empty. */ }

            protected override bool MoveNextOverride() => InnerEnumerator.MoveNext();

            protected override void ResetOverride()
            {
                base.ResetOverride();

                _innerEnumerator.Reset();
            }

            protected override void DisposeManaged()
            {
                base.DisposeManaged();

                _innerEnumerator.Dispose();

                _innerEnumerator = null;
            }
        }
#endif

        public abstract class Enumerator<TSource, TEnumSource, TDestination
#if !WinCopies3
            , TEnumDestination
#endif
            > :
#if !WinCopies3
            System.Collections.Generic.IEnumerator<TDestination>, WinCopies.Util.DotNetFix.IDisposable
#else
            Enumerator<TDestination>
#endif
            where TEnumSource : System.Collections.Generic.IEnumerator<TSource>
#if !WinCopies3
            where TEnumDestination : System.Collections.Generic.IEnumerator<TDestination>
#endif
        {
            private TEnumSource _innerEnumerator;

            protected TEnumSource InnerEnumerator => IsDisposed ? throw GetExceptionForDispose(false) : _innerEnumerator;

#if !WinCopies3
        private TDestination _current;

        public TDestination Current { get => IsDisposed ? throw GetExceptionForDispose(false) : _current; protected set => _current = IsDisposed ? throw GetExceptionForDispose(false) : value; }

        object System.Collections.IEnumerator.Current => Current;
        
        public bool IsDisposed { get; private set; }

        protected abstract bool MoveNextOverride();

        public bool MoveNext()
        {
            if (IsDisposed ? throw GetExceptionForDispose(false) : MoveNextOverride()) return true;

            _current = default;

            return false;
        }

        public void Reset()
        {
            if (IsDisposed)

                throw GetExceptionForDispose(false);

            ResetOverride();
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                Dispose(disposing: true);

                GC.SuppressFinalize(this);
            }
        }
#endif

            /// <summary>
            /// When overridden in a derived class, initializes a new instance of the <see cref="Enumerator{TSource, TDestination}"/> class.
            /// </summary>
            /// <param name="enumerator">The enumerator to enumerate.</param>
            public Enumerator(TEnumSource enumerator)
#if CS8
                => _innerEnumerator = enumerator ?? throw GetArgumentNullException(nameof(enumerator));
#else
            {
                if (enumerator == null)

                    throw GetArgumentNullException(nameof(enumerator));

                _innerEnumerator = enumerator;
            }
#endif

            protected
#if !WinCopies3
                virtual
#else
                override
#endif
                void ResetOverride()
            {
#if !WinCopies3
            _current = default;
#else
                base.ResetOverride();
#endif
                InnerEnumerator.Reset();
            }

            protected
#if !WinCopies3
                virtual void Dispose(bool disposing) => _innerEnumerator = default;
#else
                override void DisposeManaged()
            {
#if WinCopies3
                base.DisposeManaged();
#endif

                _innerEnumerator.Dispose();

                _innerEnumerator = default;
            }
#endif
        }

        public abstract class Enumerator<TSource, TDestination> : Enumerator<TSource, System.Collections.Generic.IEnumerator<TSource>, TDestination
#if !WinCopies3
            , System.Collections.Generic.IEnumerator<TDestination>
#endif
            >
        {
            /// <summary>
            /// When overridden in a derived class, initializes a new instance of the <see cref="Enumerator{TSource, TDestination}"/> class.
            /// </summary>
            /// <param name="enumerable">An enumerable from which to get an enumerator to enumerate.</param>
            public Enumerator(System.Collections.Generic.IEnumerable<TSource> enumerable) : base((enumerable ?? throw GetArgumentNullException(nameof(enumerable))).GetEnumerator()) { /* Left empty. */ }

            /// <summary>
            /// When overridden in a derived class, initializes a new instance of the <see cref="Enumerator{TSource, TDestination}"/> class.
            /// </summary>
            /// <param name="enumerator">The enumerator to enumerate.</param>
            public Enumerator(System.Collections.Generic.IEnumerator<TSource> enumerator) : base(enumerator ?? throw GetArgumentNullException(nameof(enumerator))) { /* Left empty. */ }
        }

#if WinCopies3
        public abstract class ExtensionEnumerator<TItems, TEnumerator> : WinCopies.Collections.DotNetFix.IDisposableEnumeratorInfo, IEnumeratorBase, IDisposableEnumerator<TItems>, IDisposableEnumeratorInfo, IEnumeratorInfo2<TItems> where TEnumerator : IEnumeratorInfo2<TItems>
        {
            protected TEnumerator InnerEnumerator { get; }

            public TItems Current => InnerEnumerator.Current;

            public bool IsDisposed { get; private set; }

            public bool? IsResetSupported => InnerEnumerator.IsResetSupported;

            public bool IsStarted => InnerEnumerator.IsStarted;

            public bool IsCompleted => InnerEnumerator.IsCompleted;

            object System.Collections.IEnumerator.Current => ((System.Collections.IEnumerator)InnerEnumerator).Current;

            public bool MoveNext() => InnerEnumerator.MoveNext();

            public void Reset() => InnerEnumerator.Reset();

            protected ExtensionEnumerator(in TEnumerator enumerator) => InnerEnumerator = enumerator == null ? throw GetArgumentNullException(nameof(enumerator)) : enumerator;

            protected virtual void DisposeManaged()
            {
                InnerEnumerator.Dispose();

                IsDisposed = true;
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)

                    DisposeManaged();
            }

            ~ExtensionEnumerator() => Dispose(false);

            public void Dispose()
            {
                if (IsDisposed)

                    return;

                Dispose(true);

                GC.SuppressFinalize(this);
            }
        }

        public abstract class CountableEnumeratorInfoBase<TItems, TEnumerator> : ExtensionEnumerator<TItems, TEnumerator>, ICountableEnumeratorInfo<TItems> where TEnumerator : IEnumeratorInfo2<TItems>
        {
            private Func<int> _countableFunc;

            protected Func<int> CountableFunc => IsDisposed ? throw GetEnumeratorNotStartedOrDisposedException() : _countableFunc;

            public int Count => CountableFunc();

            protected CountableEnumeratorInfoBase(in TEnumerator enumerator, in Func<int> countableFunc) : base(enumerator) => _countableFunc = countableFunc ?? throw GetArgumentNullException(nameof(countableFunc));

            protected override void DisposeManaged()
            {
                _countableFunc = null;

                base.DisposeManaged();
            }
        }

        public sealed class CountableEnumeratorInfo<T> : CountableEnumeratorInfoBase<T, IEnumeratorInfo2<T>>
        {
            public CountableEnumeratorInfo(in IEnumeratorInfo2<T> enumerator, in Func<int> countableFunc) : base(enumerator, countableFunc)
            {
                // Left empty.
            }
        }

        public abstract class UIntCountableEnumeratorInfoBase<TItems, TEnumerator> : ExtensionEnumerator<TItems, TEnumerator>, IUIntCountableEnumeratorInfo<TItems> where TEnumerator : IEnumeratorInfo2<TItems>
        {
            private Func<uint> _countableFunc;

            protected Func<uint> CountableFunc => IsDisposed ? throw GetEnumeratorNotStartedOrDisposedException() : _countableFunc;

            public uint Count => CountableFunc();

            protected UIntCountableEnumeratorInfoBase(in TEnumerator enumerator, in Func<uint> countableFunc) : base(enumerator) => _countableFunc = countableFunc ?? throw GetArgumentNullException(nameof(countableFunc));

            protected override void DisposeManaged()
            {
                _countableFunc = null;

                base.DisposeManaged();
            }
        }

        public sealed class UIntCountableEnumeratorInfo<T> : UIntCountableEnumeratorInfoBase<T, IEnumeratorInfo2<T>>
        {
            public UIntCountableEnumeratorInfo(in IEnumeratorInfo2<T> enumerator, in Func<uint> countableFunc) : base(enumerator, countableFunc)
            {
                // Left empty.
            }
        }
    }
#endif
}
