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

using WinCopies.Util;

using static WinCopies.ThrowHelper;
using static WinCopies.UtilHelpers;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;

using DataEventArgs = WinCopies.Util.Data.EventArgs;
using EnumerationEventArgs = WinCopies.Util.Data.EventArgs<WinCopies.Collections.Generic.EnumerationStatus>;

namespace WinCopies.Collections
{
    public enum MoveNextResult : sbyte
    {
        AlreadyCompleted = -1,

        False,

        True
    }

    public abstract class EnumeratorInfoBase : DotNetFix.IDisposableEnumeratorInfo, IEnumeratorBase
    {
        private Func<MoveNextResult>
#if CS8
            ?
#endif
            _moveNext;

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

        protected void ThrowIfStartedOrDisposed()
        {
            if (IsStarted || IsDisposed)

                throw new InvalidOperationException("The current enumerator is started or disposed.");
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

            ResetMoveNext();
        }

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

            ResetOverride();
        }

        protected virtual void DisposeManaged()
        {
            OnResetOrDisposed();

            _moveNext = null;
        }

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

    public abstract class Enumerator : EnumeratorInfoBase, IDisposableEnumerator, IDisposableEnumeratorInfo
    {
        public object Current => IsDisposed ? throw GetExceptionForDispose(false) : IsStarted ? CurrentOverride : throw new InvalidOperationException("The enumeration has not been started or has completed.");

        /// <summary>
        /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
        /// </summary>
        protected abstract object CurrentOverride { get; }

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

        protected override void ResetCurrent() { /* Left empty. */ }

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
        public abstract class Enumerator<T> : EnumeratorInfoBase, IDisposableEnumerator<T>, IDisposableEnumeratorInfo, IEnumeratorInfo2<T>
#if CS8
                , IEnumerator<T>
#endif
        {
            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <exception cref="InvalidOperationException">The enumerator is disposed.</exception>
            public T Current => IsDisposed ? throw GetExceptionForDispose(false) : IsStarted ? CurrentOverride : throw new InvalidOperationException("The enumeration has not been started or has completed.");
            /// <summary>
            /// When overridden in a derived class, gets the item in the collection at the current position of the enumerator.
            /// </summary>
            protected abstract T CurrentOverride { get; }
#if !CS8
            object System.Collections.IEnumerator.Current => Current;
#endif
        }

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
            public EnumeratorInfo(in Enumeration.Generic.IEnumerable<TItems, TEnumerator> enumerable) : this(GetEnumerator(enumerable, nameof(enumerable))) { /* Left empty. */ }

            protected static TEnumerator GetEnumerator(in Enumeration.Generic.IEnumerable<TItems, TEnumerator> enumerable, in string paramName) => GetOrThrowIfNull(enumerable, paramName).AsFromType<Enumeration.IEnumerable<TEnumerator>>().GetEnumerator();

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
            public DefaultEnumeratorInfo(in Enumeration.Generic.IEnumerable<TItems, TEnumerator> enumerable) : base(GetEnumerator(enumerable, nameof(enumerable))) { /* Left empty. */ }
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
            public EnumeratorInfo2(in Enumeration.Generic.IEnumerable<T, IEnumeratorInfo<T>> enumerable) : base(GetEnumerator(enumerable, nameof(enumerable))) { /* Left empty. */ }
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
            public ObservableEnumerator(in Enumeration.Generic.IEnumerable<TItems, TEnumerator> enumerable) : base(enumerable) { /* Left empty. */ }

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
            public DefaultObservableEnumerator(in Enumeration.Generic.IEnumerable<TItems, TEnumerator> enumerable) : base(GetEnumerator(enumerable, nameof(enumerable))) { /* Left empty. */ }
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
            public ObservableEnumerator2(in Enumeration.Generic.IEnumerable<T, IEnumeratorInfo<T>> enumerable) : base(GetEnumerator(enumerable, nameof(enumerable))) { /* Left empty. */ }
        }

        public abstract class Enumerator<TSource, TEnumSource, TDestination> : Enumerator<TDestination> where TEnumSource : System.Collections.Generic.IEnumerator<TSource>
        {
            private TEnumSource _innerEnumerator;

            protected TEnumSource InnerEnumerator => IsDisposed ? throw GetExceptionForDispose(false) : _innerEnumerator;

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

            protected override void ResetOverride2() => InnerEnumerator.Reset();

            protected override void DisposeManaged()
            {
                base.DisposeManaged();

                _innerEnumerator.Dispose();
                _innerEnumerator = default;
            }
        }

        public abstract class Enumerator<TSource, TDestination> : Enumerator<TSource, System.Collections.Generic.IEnumerator<TSource>, TDestination>
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

        public abstract class ExtensionEnumerator<TItems, TEnumerator> : DotNetFix.IDisposableEnumeratorInfo, IEnumeratorBase, IDisposableEnumerator<TItems>, IDisposableEnumeratorInfo, IEnumeratorInfo2<TItems> where TEnumerator : IEnumeratorInfo<TItems>
        {
            protected TEnumerator InnerEnumerator { get; }

            protected System.Collections.IEnumerator Enumerator => InnerEnumerator;

            public TItems Current => InnerEnumerator.Current;

            public bool IsDisposed { get; private set; }

            public bool? IsResetSupported => InnerEnumerator.IsResetSupported;

            public bool IsStarted => InnerEnumerator.IsStarted;

            public bool IsCompleted => InnerEnumerator.IsCompleted;
#if !CS8
            object System.Collections.IEnumerator.Current => Current;
#endif
            public bool MoveNext() => Enumerator.MoveNext();

            public void Reset() => Enumerator.Reset();

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

            public CountableEnumeratorInfo(in IEnumerableInfo<T> enumerable, in Func<int> countableFunc) : base(enumerable.AsFromType<Enumeration.IEnumerable<IEnumeratorInfo<T>>>().GetEnumerator(), countableFunc) { /* Left empty. */ }
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

            public override bool? IsResetSupported => true;

            protected override T CurrentOverride => _current;

            public bool TryUpdateCurrent(in T value)
            {
                if (IsResetSupported != false)
                {
                    Reset();

                    _current = IsDisposed ? throw GetExceptionForDispose(false) : value;

                    _move = true;

                    return true;
                }

                return false;
            }

            public void UpdateCurrent(in T value)
            {
                if (!TryUpdateCurrent(value))

                    throw new InvalidOperationException("The current enumerator does not support resetting.");
            }

            protected override bool MoveNextOverride() => UpdateValue(ref _move);

            protected override void ResetCurrent()
            {
                base.ResetCurrent();

                _move = false;
                _current = default;
            }

            protected override void ResetOverride2() => _move = true;

            /// <summary>
            /// Disposing is disabled for the <see cref="SingletonEnumerator{T}"/> class because this enumerator does not have to be disposed and does not support it. Calling this method from this class will not do anything, as it was left empty.
            /// </summary>
            public override void Dispose() { /* Left empty. */ }
        }

        public class WhileEnumerator<T> : Enumerator<T>
        {
            private T _current;
            private Func<bool> _moveNext;

            protected Converter<T, T> Converter { get; }

            protected Predicate<T> Predicate { get; }

            public override bool? IsResetSupported => false;

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

            protected override void ResetCurrent()
            {
                _current = default;

                base.ResetCurrent();
            }

            protected override void ResetOverride2() { /* Left empty. */ }
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

            public override bool? IsResetSupported => true;

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

            protected override void ResetCurrent()
            {
                _current = default;

                ResetCurrentIndex();

                base.ResetCurrent();
            }

            protected override void ResetOverride2() { /* Left empty. */ }
        }
    }
}
