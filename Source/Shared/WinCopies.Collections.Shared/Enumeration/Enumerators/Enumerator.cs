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

using static WinCopies.
#if WinCopies3
    UtilHelpers;
using static WinCopies.ThrowHelper;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;

using DataEventArgs = WinCopies.Util.Data.EventArgs;
using EnumerationEventArgs = WinCopies.Util.Data.EventArgs<WinCopies.Collections.Generic.EnumerationStatus>;
#else
    Util.Util;

using WinCopies.Util;
#endif

namespace WinCopies.Collections
{
#if WinCopies3
    public enum MoveNextResult : byte
    {
        False = 0,

        True,

        AlreadyCompleted
    }

    public abstract class EnumeratorInfoBase : DotNetFix.IDisposableEnumeratorInfo, IEnumeratorBase
    {
        private Func<MoveNextResult> _moveNext;

        public bool IsStarted { get; private set; }

        public bool IsCompleted { get; private set; }

        public abstract bool? IsResetSupported { get; }

        public bool IsDisposed { get; private set; }

        protected EnumeratorInfoBase() => ResetMoveNext();

        protected void ThrowIfDisposed()
        {
            if (IsDisposed)

                throw GetExceptionForDispose(false);
        }

        protected T GetOrThrowIfDisposed<T>(in T value) => WinCopies.ThrowHelper.GetOrThrowIfDisposed(this, value);

        protected virtual void OnMoveNextStarting() { /* Left empty. */ }

        private void ResetMoveNext() => _moveNext = () =>
        {
            IsStarted = true;

            OnMoveNextStarting();

            return (_moveNext = _MoveNext)();
        };

        private MoveNextResult _MoveNext()
        {
            if (MoveNextOverride())

                return MoveNextResult.True;

            OnMoveNextCompleted();

            return MoveNextResult.False;
        }

        public MoveNextResult MoveNext2()
        {
            ThrowIfDisposed();

            return IsCompleted ? MoveNextResult.AlreadyCompleted : _moveNext();
        }

        public bool MoveNext() => MoveNext2() == MoveNextResult.True;

        protected abstract bool MoveNextOverride();

        protected virtual void OnMoveNextCompleted()
        {
            IsCompleted = true;

            IsStarted = false;

            ResetCurrent();
        }

        protected virtual void ResetCurrent() { /* Left empty. */ }

        protected abstract void ResetOverride2();

        protected virtual void ResetOverride()
        {
            ResetOverride2();

            OnResetOrDisposed();
        }

        protected virtual void OnResetOrDisposed()
        {
            IsStarted = false;
            IsCompleted = false;

            ResetCurrent();

            ResetMoveNext();
        }

        public void Reset()
        {
            ThrowIfDisposed();

            if (IsResetSupported == false
#if WinCopies3
                )
#else
                ?
#endif
                throw new InvalidOperationException("The current enumerator does not support resetting.")
#if WinCopies3
                ;
#else
                : IsStarted)
#endif

            ResetOverride();
        }

        protected virtual void DisposeManaged() => OnResetOrDisposed();

        protected virtual void DisposeUnmanaged() { /* Left empty. */ }

        protected virtual void DisposeManaged2() { /* Left empty. */ }

        public virtual void Dispose()
        {
            if (IsDisposed)

                return;

            DisposeManaged();
            DisposeUnmanaged();

            IsDisposed = true;

            DisposeManaged2();

            GC.SuppressFinalize(this);
        }

        ~EnumeratorInfoBase() => DisposeUnmanaged();
    }
#endif

    public abstract class Enumerator :
#if WinCopies3
        EnumeratorInfoBase, IDisposableEnumerator, IDisposableEnumeratorInfo
#else
        System.Collections.IEnumerator, WinCopies.Util.DotNetFix.IDisposable
#endif
    {
        public object Current
#if !WinCopies3
        {
            get
#endif
            => IsDisposed ? throw GetExceptionForDispose(false) : IsStarted ?
#if WinCopies3
                CurrentOverride
#else
                _current
#endif
                : throw new InvalidOperationException("The enumeration has not been started or has completed.");

#if WinCopies3
        /// <summary>
        /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
        /// </summary>
        protected abstract object CurrentOverride { get; }
#else
            protected set => _current = IsDisposed ? throw GetExceptionForDispose(false) : value;
        }

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
#endif

        public static Generic.WhileEnumerator<T> GetNullCheckWhileEnumerator<T>(in T first, in Converter<T, T> converter) => new
#if !CS9
            Generic.WhileEnumerator<T>
#endif
            (first, converter, Delegates.
#if WinCopies4
            NotEqualsNull
#else
            CheckIfNotEqualsNull
#endif
            );
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
        protected override void ResetOverride2() => _innerEnumerator.Reset();

        protected override void DisposeManaged()
        {
            base.DisposeManaged();

            _innerEnumerator = null;
        }
    }

    namespace Generic
    {
#endif
    public abstract class Enumerator<T> :
#if WinCopies3
                EnumeratorInfoBase, IDisposableEnumerator<T>, IDisposableEnumeratorInfo, IEnumeratorInfo2<T>
#else
    System.Collections.Generic.IEnumerator<T>, WinCopies.Util.DotNetFix.IDisposable
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
#if WinCopies3
                        IsStarted
#else
                _enumerationStarted
#endif
                    ?
#if WinCopies3
                    CurrentOverride
#else
            _current
#endif
                : throw new InvalidOperationException("The enumeration has not been started or has completed.");
#if !WinCopies3
            protected set => _current = IsDisposed ? throw GetExceptionForDispose(false) : value;
        }
#endif

        /// <summary>
        /// When overridden in a derived class, gets the item in the collection at the current position of the enumerator.
        /// </summary>
#if !WinCopies3
        [Obsolete("This property is not by this class in WinCopies 2, so it is now virtual. This property is abstract in WinCopies 3.")]
#endif
        protected
#if WinCopies3
    abstract
#else
            virtual
#endif
    T CurrentOverride
        { get; }

        object System.Collections.IEnumerator.Current => Current;

#if !WinCopies3
        private bool _enumerationStarted = false;

        protected TValue GetOrThrowIfDisposed<TValue>(in TValue value) => WinCopies.Util.ThrowHelper.GetOrThrowIfDisposed(this, value);

        protected abstract bool MoveNextOverride();

        public bool MoveNext()
        {
            if (IsDisposed ? throw GetExceptionForDispose(false) : MoveNextOverride())
            {
                _enumerationStarted = true;

                return true;
            }

            _enumerationStarted = false;

            Current = default;

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
            Current = default;

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
        public interface IEnumeratorInfo2<
#if CS5
            out
#endif
             T> : IEnumeratorInfo<T>, DotNetFix.IDisposableEnumeratorInfo, IDisposableEnumerator<T>, IDisposableEnumeratorInfo
        {
            // Left empty.
        }

        public abstract class EnumeratorInfo<TItems, TEnumerator> : Enumerator<TItems> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
        {
            private TEnumerator _innerEnumerator;

            protected TEnumerator InnerEnumerator => IsDisposed ? throw GetExceptionForDispose(false) : _innerEnumerator;

            /// <summary>
            /// When overridden in a derived class, gets the item in the collection at the current position of the enumerator.
            /// </summary>
            protected override TItems CurrentOverride => _innerEnumerator.Current; // The disposed check is performed in the Current property.

            public EnumeratorInfo(in TEnumerator enumerator) => SetOrThrowIfNull(ref _innerEnumerator, enumerator, nameof(enumerator));
            public EnumeratorInfo(in DotNetFix.Generic.IEnumerable<TItems, TEnumerator> enumerable) : this(GetEnumerator(enumerable, nameof(enumerable))) { /* Left empty. */ }

            protected static TEnumerator GetEnumerator(in DotNetFix.Generic.IEnumerable<TItems, TEnumerator> enumerable, in string paramName) => GetOrThrowIfNull(enumerable, paramName).GetEnumerator();

            protected static System.Collections.Generic.IEnumerator<TItems> GetEnumerator(in System.Collections.Generic.IEnumerable<TItems> enumerable, in string paramName) => GetOrThrowIfNull(enumerable, paramName).GetEnumerator();

            protected override bool MoveNextOverride() => _innerEnumerator.MoveNext();

            protected override void ResetOverride2() => _innerEnumerator.Reset();

            protected override void DisposeManaged()
            {
                base.DisposeManaged();

                _innerEnumerator.Dispose();
                _innerEnumerator = default;
            }
        }

        public class DefaultEnumeratorInfo<TItems, TEnumerator> : EnumeratorInfo<TItems, TEnumerator> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
        {
            public override bool? IsResetSupported => null;

            public DefaultEnumeratorInfo(in TEnumerator enumerator) : base(enumerator) { /* Left empty. */ }
            public DefaultEnumeratorInfo(in DotNetFix.Generic.IEnumerable<TItems, TEnumerator> enumerable) : base(GetEnumerator(enumerable, nameof(enumerable))) { /* Left empty. */ }
        }

        public class EnumeratorInfo<T> : EnumeratorInfo<T, System.Collections.Generic.IEnumerator<T>>
        {
            public override bool? IsResetSupported => null;

            public EnumeratorInfo(in System.Collections.Generic.IEnumerator<T> enumerator) : base(enumerator) { /* Left empty. */ }
            public EnumeratorInfo(in System.Collections.Generic.IEnumerable<T> enumerable) : base(GetEnumerator(enumerable, nameof(enumerable))) { /* Left empty. */ }
        }

        public class EnumeratorInfo2<T> : EnumeratorInfo<T, IEnumeratorInfo<T>>
        {
            public override bool? IsResetSupported => InnerEnumerator.IsResetSupported;

            public EnumeratorInfo2(in IEnumeratorInfo<T> enumerator) : base(enumerator) { /* Left empty. */ }
            public EnumeratorInfo2(in DotNetFix.Generic.IEnumerable<T, IEnumeratorInfo<T>> enumerable) : base(GetEnumerator(enumerable, nameof(enumerable))) { /* Left empty. */ }
        }

        public enum EnumerationStatus : byte
        {
            None = 0,

            Starting,

            MovedNext,

            Completed,

            ResetCurrent,

            Reset,

            Disposing,

            Disposed
        }

        public abstract class ObservableEnumerator<TItems, TEnumerator> : EnumeratorInfo<TItems, TEnumerator> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
        {
            public event WinCopies.Util.Data.EventHandler<EnumerationStatus> StatusChanged;

            public ObservableEnumerator(in TEnumerator enumerator) : base(enumerator) { /* Left empty. */ }
            public ObservableEnumerator(in DotNetFix.Generic.IEnumerable<TItems, TEnumerator> enumerable) : base(enumerable) { /* Left empty. */ }

            protected void RaiseEvent(in EnumerationEventArgs e) => StatusChanged?.Invoke(this, e);
            protected void RaiseEvent(in EnumerationStatus enumerationStatus) => RaiseEvent(DataEventArgs.Construct(enumerationStatus));

            protected void RaiseEvent(in Action action, in EnumerationEventArgs e)
            {
                action();

                RaiseEvent(e);
            }

            protected void RaiseEvent(in Action action, in EnumerationStatus enumerationStatus) => RaiseEvent(action, DataEventArgs.Construct(enumerationStatus));

            protected override void OnMoveNextStarting() => RaiseEvent(base.OnMoveNextStarting, EnumerationStatus.Starting);

            protected override bool MoveNextOverride()
            {
                bool result = false;

                RaiseEvent(() => result = base.MoveNextOverride(), DataEventArgs.Construct2(EnumerationStatus.MovedNext, result));

                return result;
            }

            protected override void OnMoveNextCompleted() => RaiseEvent(base.OnMoveNextCompleted, EnumerationStatus.Completed);

            protected override void ResetCurrent() => RaiseEvent(base.ResetCurrent, EnumerationStatus.ResetCurrent);

            protected override void ResetOverride() => RaiseEvent(base.ResetOverride, EnumerationStatus.Reset);

            protected override void DisposeManaged() => RaiseEvent(base.DisposeManaged, EnumerationStatus.Disposing);

            protected override void DisposeManaged2() => RaiseEvent(base.DisposeManaged2, EnumerationStatus.Disposed);
        }

        public class DefaultObservableEnumerator<TItems, TEnumerator> : ObservableEnumerator<TItems, TEnumerator> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
        {
            public override bool? IsResetSupported => null;

            public DefaultObservableEnumerator(in TEnumerator enumerator) : base(enumerator) { /* Left empty. */ }
            public DefaultObservableEnumerator(in DotNetFix.Generic.IEnumerable<TItems, TEnumerator> enumerable) : base(GetEnumerator(enumerable, nameof(enumerable))) { /* Left empty. */ }
        }

        public class ObservableEnumerator<T> : ObservableEnumerator<T, System.Collections.Generic.IEnumerator<T>>
        {
            public override bool? IsResetSupported => null;

            public ObservableEnumerator(in System.Collections.Generic.IEnumerator<T> enumerator) : base(enumerator) { /* Left empty. */ }
            public ObservableEnumerator(in System.Collections.Generic.IEnumerable<T> enumerable) : base(GetEnumerator(enumerable, nameof(enumerable))) { /* Left empty. */ }
        }

        public class ObservableEnumerator2<T> : ObservableEnumerator<T, IEnumeratorInfo<T>>
        {
            public override bool? IsResetSupported => InnerEnumerator.IsResetSupported;

            public ObservableEnumerator2(in IEnumeratorInfo<T> enumerator) : base(enumerator) { /* Left empty. */ }
            public ObservableEnumerator2(in DotNetFix.Generic.IEnumerable<T, IEnumeratorInfo<T>> enumerable) : base(GetEnumerator(enumerable, nameof(enumerable))) { /* Left empty. */ }
        }
#endif

    public abstract class Enumerator<TSource, TEnumSource, TDestination
#if !WinCopies3
    , TEnumDestination
#endif
        > :
#if WinCopies3
                Enumerator<TDestination>
#else
    System.Collections.Generic.IEnumerator<TDestination>, WinCopies.Util.DotNetFix.IDisposable

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
#if WinCopies3
                    override void ResetOverride2
#else
        virtual void ResetOverride
#endif
        ()
        {
#if !WinCopies3
            _current = default;
#endif
            InnerEnumerator.Reset();
        }

        protected
#if WinCopies3
                    override void DisposeManaged()
            {
                base.DisposeManaged();

                _innerEnumerator.Dispose();

                _innerEnumerator = default;
            }
#else
    virtual void Dispose(bool disposing) => _innerEnumerator = default;
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
        public abstract class ExtensionEnumerator<TItems, TEnumerator> : DotNetFix.IDisposableEnumeratorInfo, IEnumeratorBase, IDisposableEnumerator<TItems>, IDisposableEnumeratorInfo, IEnumeratorInfo2<TItems> where TEnumerator : IEnumeratorInfo<TItems>
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

        public abstract class CountableEnumeratorInfoBase<TItems, TEnumerator> : ExtensionEnumerator<TItems, TEnumerator>, ICountableEnumeratorInfo<TItems> where TEnumerator : IEnumeratorInfo<TItems>
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

        public sealed class CountableEnumeratorInfo<T> : CountableEnumeratorInfoBase<T, IEnumeratorInfo<T>>
        {
            public CountableEnumeratorInfo(in IEnumeratorInfo<T> enumerator, in Func<int> countableFunc) : base(enumerator, countableFunc) { /* Left empty. */ }
            public CountableEnumeratorInfo(in System.Collections.Generic.IEnumerator<T> enumerator, in Func<int> countableFunc) : base(new EnumeratorInfo<T>(enumerator), countableFunc) { /* Left empty. */ }

            public CountableEnumeratorInfo(in IEnumerableInfo<T> enumerable, in Func<int> countableFunc) : base(enumerable.GetEnumerator(), countableFunc) { /* Left empty. */ }
            public CountableEnumeratorInfo(in System.Collections.Generic.IEnumerable<T> enumerable, in Func<int> countableFunc) : this(enumerable.GetEnumerator(), countableFunc) { /* Left empty. */ }
        }

        public abstract class UIntCountableEnumeratorInfoBase<TItems, TEnumerator> : ExtensionEnumerator<TItems, TEnumerator>, IUIntCountableEnumeratorInfo<TItems> where TEnumerator : IEnumeratorInfo<TItems>
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

        public sealed class UIntCountableEnumeratorInfo<T> : UIntCountableEnumeratorInfoBase<T, IEnumeratorInfo<T>>
        {
            public UIntCountableEnumeratorInfo(in IEnumeratorInfo<T> enumerator, in Func<uint> countableFunc) : base(enumerator, countableFunc) { /* Left empty. */ }
        }
#else
    namespace Generic
    {
#endif
        public sealed class SingletonEnumerable<T> : System.Collections.Generic.IEnumerable<T>
        {
            private readonly SingletonEnumerator<T> _enumerator = new
#if !CS9
                SingletonEnumerator<T>
#endif
                ();

            public System.Collections.Generic.IEnumerator<T> GetEnumerator() => _enumerator;

            public void UpdateCurrent(T value) => _enumerator.UpdateCurrent(value);

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public sealed class SingletonEnumerator<T> : Enumerator<T>
        {
            private bool _move;
            private T _current;

#if WinCopies3
            public override bool? IsResetSupported => true;
#endif

            protected override T CurrentOverride => _current;

            public bool TryUpdateCurrent(in T value)
            {
#if WinCopies3
                if (IsResetSupported != false)
                {
#endif
                Reset();

                _current = IsDisposed ? throw GetExceptionForDispose(false) : value;

                _move = true;

                return true;
#if WinCopies3
                }

                return false;
#endif
            }

            public void UpdateCurrent(in T value)
            {
                if (!TryUpdateCurrent(value))

                    throw new InvalidOperationException("The current enumerator does not support resetting.");
            }

            protected override bool MoveNextOverride() => UpdateValue(ref _move);

            protected
#if WinCopies3
                override
#endif
                void ResetCurrent()
            {
#if WinCopies3
                base.ResetCurrent();
#endif

                _move = false;
                _current = default;
            }

            protected override void

#if WinCopies3
                ResetOverride2
#else
                ResetOverride
#endif
                ()
#if WinCopies3
                =>
#else
            {
                ResetCurrent();

#endif
                _move = true;
#if !WinCopies3
                base.ResetOverride();

            }
#endif

            /// <summary>
            /// Disposing is disabled for the <see cref="SingletonEnumerator{T}"/> class because this enumerator does not have to be disposed and does not support it. Calling this method from this class will not do anything, as it was left empty.
            /// </summary>
#if WinCopies3
            public
#else
            protected
#endif
            override void Dispose(
#if !WinCopies3
                bool disposing
#endif
                )
            { /* Left empty. */ }
        }

        public class WhileEnumerator<T> : Enumerator<T>
        {
            private T _current;
            private Func<bool> _moveNext;

            protected Converter<T, T> Converter { get; }

            protected Predicate<T> Predicate { get; }

#if WinCopies3
            public override bool? IsResetSupported => false;
#endif

            protected override T CurrentOverride => _current;

            public WhileEnumerator(T first, Converter<T, T> converter, Predicate<T> predicate)
            {
                Converter = converter;

                Predicate = predicate;

                _moveNext = () => PerformActionIf(first, predicate, () =>
                {
                    _current = first;

                    _moveNext = () => predicate(_current = converter(_current));
                });
            }

            protected override bool MoveNextOverride() => _moveNext();

            protected
#if WinCopies3
                override
#endif
                void ResetCurrent()
#if WinCopies3
            {
#else
                =>
#endif
                _current = default;

#if WinCopies3
                base.ResetCurrent();
            }
#endif

            protected
#if WinCopies3
                override
#endif
                void
#if WinCopies3
                ResetOverride2
#else
                ResetOverride
#endif
                ()
#if WinCopies3
            { /* Left empty. */ }
#else
                => ResetCurrent();
#endif
        }

        public class SkipEnumerator<T> : Enumerator<T>
        {
            private T _current;

            public int Start { get; }

            public int Length { get; }

            public
#if CS5
                System.Collections.Generic.IReadOnlyList<T>
#else
                T[]
#endif
                InnerArray
            { get; }

            public int CurrentIndex { get; private set; }

#if WinCopies3
            public override bool? IsResetSupported => true;
#endif

            protected override T CurrentOverride => _current;

            public SkipEnumerator(in
#if CS5
                System.Collections.Generic.IReadOnlyList<T>
#else
                T[]
#endif
                innerArray, in int start, in int length)
            {
                int arrayLength = (innerArray ?? throw GetArgumentNullException(nameof(innerArray))).
#if CS5
                    Count
#else
                    Length
#endif
                    ;

                if (start >= arrayLength)

                    throw new ArgumentOutOfRangeException(nameof(start));

                if (length > arrayLength - start)

                    throw new ArgumentOutOfRangeException(nameof(length));

                InnerArray = innerArray;

                Start = start;

                Length = length;

                ResetCurrentIndex();
            }

            public SkipEnumerator(in
#if CS5
                System.Collections.Generic.IReadOnlyList<T>
#else
                T[]
#endif
                innerArray, in int start) : this(innerArray, start, (innerArray ?? throw GetArgumentNullException(nameof(innerArray))).
#if CS5
                    Count
#else
                    Length
#endif
                    - start)
            { /* Left empty. */ }

            protected override bool MoveNextOverride() => PerformActionIf(CurrentIndex < Length, () => _current = InnerArray[CurrentIndex++]);

            protected virtual void ResetCurrentIndex() => CurrentIndex = Start;

            protected
#if WinCopies3
                override
#endif
                void ResetCurrent()
            {
                _current = default;

                ResetCurrentIndex();

#if WinCopies3
                base.ResetCurrent();
#endif
            }

            protected
#if WinCopies3
                override
#endif
                void ResetOverride2()
#if WinCopies3
            { /* Left empty. */ }
#else
                => ResetCurrent();
#endif
        }
    }
}
