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

#if WinCopies2

using System;
using System.Collections;
using System.Collections.Generic;
using IDisposable = WinCopies.Util.DotNetFix.IDisposable;
using WinCopies.Util;
using static WinCopies.Util.Util;
using System.Diagnostics;

namespace WinCopies.Collections
{
    public interface IEnumerableEnumerator
    {
        object Current { get; }

        bool MoveNext();
    }

    public interface IEnumerableEnumerator<T> : IEnumerableEnumerator, System.IDisposable
    {
        T Current { get; }
    }

    public abstract class Enumerator : IEnumerator, WinCopies.
#if WinCopies2
        Util.
#endif
        DotNetFix.IDisposable
    {
        private object _current;
        private bool _enumerationStarted = false;

        public bool IsDisposed { get; private set; }

        public object Current { get => IsDisposed ? throw GetExceptionForDispose(false) : _enumerationStarted ? _current : throw new InvalidOperationException("The enumeration has not been started or has completed."); protected set => _current = IsDisposed ? throw GetExceptionForDispose(false) : value; }

        public bool MoveNext()
        {
            if (IsDisposed ? throw GetExceptionForDispose(false) : MoveNextOverride())
            {
                _enumerationStarted = true;

                return true;
            }

            _current = default;

            _enumerationStarted = false;

            return false;
        }

        protected abstract bool MoveNextOverride();

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
                Dispose(disposing: true);

                GC.SuppressFinalize(this);
            }
        }
    }

    public sealed class EmptyCheckEnumerator : IEnumerator, IDisposable
    {
        #region Fields
        private readonly IEnumerator _enumerator;
        private Func<bool> _moveNext;
        private bool? _hasItems = null;
        private Func _current;
        #endregion

        #region Properties
        public bool IsDisposed { get; private set; }

        public bool HasItems
        {
            get
            {
                ThrowIfDisposed();

                if (!_hasItems.HasValue)

                    _hasItems = _enumerator.MoveNext();

                return _hasItems.Value;
            }
        }

        public object Current
        {
            get
            {
                ThrowIfDisposed();

                return _current();
            }
        }
        #endregion

        public EmptyCheckEnumerator(IEnumerator enumerator)
        {
            _enumerator = enumerator;

            ResetMoveNext();
        }

        #region Methods
        private void ResetCurrent() => _current = () => throw new InvalidOperationException("The enumeration has not been started or has completed.");

        private void ResetMoveNext()
        {
            ResetCurrent();

            void resetMoveNext()
            {
                _moveNext = () => false;

                ResetCurrent();
            }

            bool enumerate()
            {
                if (_enumerator.MoveNext())

                    return true;

                resetMoveNext();

                return false;
            }

            _moveNext = () =>
            {
                if (_hasItems.HasValue)
                {
                    if (_hasItems.Value)
                    {
                        _current = () => _enumerator.Current;

                        _moveNext = enumerate;

                        return true;
                    }

                    else
                    {
                        resetMoveNext();

                        return false;
                    }
                }

                else
                {
                    _moveNext = enumerate;

                    return enumerate();
                }
            };
        }

        private void ThrowIfDisposed()
        {
            if (IsDisposed)

                throw GetExceptionForDispose(false);
        }
        #endregion

        #region Interface implementations
        #region IEnumerator implementation
        public bool MoveNext()
        {
            ThrowIfDisposed();

            return _moveNext();
        }

        public void Reset()
        {
            ThrowIfDisposed();

            _enumerator.Reset();

            _hasItems = null;

            ResetMoveNext();
        }
        #endregion

        #region IDisposable implementation
        public void Dispose()
        {
            if (IsDisposed) return;

            // _enumerator.Dispose();

            ResetMoveNext();

            IsDisposed = true;
        }
        #endregion
        #endregion
    }

    public sealed class EmptyEnumerator : IEnumerator
    {
        private readonly object _current = default;

        object IEnumerator.Current => _current;

        bool IEnumerator.MoveNext() => false;

        void IEnumerator.Reset() { }
    }

    public abstract class Enumerator<T> : IEnumerator<T>, WinCopies.
#if WinCopies2
        Util.
#endif
        DotNetFix.IDisposable
    {
        private T _current;
        private bool _enumerationStarted = false;

        public bool IsDisposed { get; private set; }

        public T Current { get => IsDisposed ? throw GetExceptionForDispose(false) : _enumerationStarted ? _current : throw new InvalidOperationException("The enumeration has not been started or has completed."); protected set => _current = IsDisposed ? throw GetExceptionForDispose(false) : value; }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (IsDisposed ? throw GetExceptionForDispose(false) : MoveNextOverride())
            {
                _enumerationStarted = true;

                return true;
            }

            _current = default;

            _enumerationStarted = false;

            return false;
        }

        protected abstract bool MoveNextOverride();

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
                Dispose(disposing: true);

                GC.SuppressFinalize(this);
            }
        }
    }

    public abstract class Enumerator<TSource, TDestination> : IEnumerator<TDestination>, WinCopies
#if WinCopies2
.Util
#endif
        .DotNetFix.IDisposable
    {
        public bool IsDisposed { get; private set; }

        private IEnumerator<TSource> _innerEnumerator;

        protected IEnumerator<TSource> InnerEnumerator => IsDisposed ? throw GetExceptionForDispose(false) : _innerEnumerator;

        private TDestination _current;

        public TDestination Current { get => IsDisposed ? throw GetExceptionForDispose(false) : _current; protected set => _current = IsDisposed ? throw GetExceptionForDispose(false) : value; }

        object IEnumerator.Current => Current;

        public Enumerator(IEnumerable<TSource> enumerable) => _innerEnumerator = (enumerable ?? throw GetArgumentNullException(nameof(enumerable))).GetEnumerator();

        public bool MoveNext()
        {
            if (IsDisposed ? throw GetExceptionForDispose(false) : MoveNextOverride()) return true;

            _current = default;

            return false;
        }

        protected abstract bool MoveNextOverride();

        public void Reset()
        {
            if (IsDisposed)

                throw GetExceptionForDispose(false);

            ResetOverride();
        }

        protected virtual void ResetOverride()
        {
            _current = default;

            InnerEnumerator.Reset();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)

                _innerEnumerator = null;

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
    }

    public sealed class EmptyCheckEnumerator<T> : IEnumerator<T>, WinCopies
#if WinCopies2
.Util
#endif
        .DotNetFix.IDisposable
    {
        #region Fields
        private IEnumerator<T> _enumerator;
        private Func<bool> _moveNext;
        private bool? _hasItems = null;
        private Func<T> _current;
        #endregion

        #region Properties
        public bool IsDisposed { get; private set; }

        public bool HasItems
        {
            get
            {
                ThrowIfDisposed();

                if (!_hasItems.HasValue)

                    _hasItems = _enumerator.MoveNext();

                return _hasItems.Value;
            }
        }

        public T Current
        {
            get
            {
                ThrowIfDisposed();

                return _current();
            }
        }
        #endregion

        public EmptyCheckEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;

            ResetMoveNext();
        }

        #region Methods
        private void ResetCurrent() => _current = () => throw new InvalidOperationException("The enumeration has not been started or has completed.");

        private void ResetMoveNext()
        {
            ResetCurrent();

            void resetMoveNext()
            {
                _moveNext = () => false;

                ResetCurrent();
            }

            bool enumerate()
            {
                if (_enumerator.MoveNext())

                    return true;

                resetMoveNext();

                return false;
            }

            _moveNext = () =>
            {
                if (_hasItems.HasValue)
                {
                    if (_hasItems.Value)
                    {
                        _current = () => _enumerator.Current;

                        _moveNext = enumerate;

                        return true;
                    }

                    else
                    {
                        resetMoveNext();

                        return false;
                    }
                }

                else
                {
                    _moveNext = enumerate;

                    return enumerate();
                }
            };
        }

        private void ThrowIfDisposed()
        {
            if (IsDisposed)

                throw GetExceptionForDispose(false);
        }
        #endregion

        #region Interface implementations
        #region IEnumerator implementation
        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            ThrowIfDisposed();

            return _moveNext();
        }

        public void Reset()
        {
            ThrowIfDisposed();

            _enumerator.Reset();

            _hasItems = null;

            ResetMoveNext();
        }
        #endregion

        #region IDisposable implementation
        public void Dispose()
        {
            if (IsDisposed) return;

            _enumerator.Dispose();

            ResetMoveNext();

            IsDisposed = true;
        }
        #endregion
        #endregion
    }

    public sealed class EmptyEnumerator<T> : IEnumerator<T>
    {
        private readonly T _current = default;

        T IEnumerator<T>.Current => _current;

        object IEnumerator.Current => _current;

        bool IEnumerator.MoveNext() => false;

        void IEnumerator.Reset() { }

        void System.IDisposable.Dispose() { }
    }

    public sealed class JoinSubEnumerator<T> : Enumerator<T, T>
    {
        private IEnumerator<T> _joinEnumerator;
        private T _firstValue;
        private bool _completed = false;
        private Func<bool> _moveNext;

        public JoinSubEnumerator(IEnumerable<T> subEnumerable, IEnumerable<T> joinEnumerable) : base(subEnumerable)
        {
            Debug.Assert(subEnumerable != null && joinEnumerable != null);

            _joinEnumerator =
#if !DEBUG
                (
#endif
                joinEnumerable
#if !DEBUG
                ?? throw GetArgumentNullException(nameof(joinEnumerable)))
#endif
                .GetEnumerator();

            InitDelegate();
        }

        private void InitDelegate() => _moveNext = () =>
                                       {
                                           if (InnerEnumerator.MoveNext())
                                           {
                                               _firstValue = InnerEnumerator.Current;

                                               _moveNext = () => _MoveNext();

                                               return _MoveNext();
                                           }

                                           else

                                               return false;
                                       };

        private bool _MoveNext()
        {
            if (_joinEnumerator.MoveNext())
            {
                Current = _joinEnumerator.Current;

                return true;
            }

            Current = _firstValue;

            _firstValue = default;

            _moveNext = () =>
            {
                if (InnerEnumerator.MoveNext())
                {
                    Current = InnerEnumerator.Current;

                    return true;
                }

                return false;
            };

            return true;
        }

        protected override bool MoveNextOverride()
        {
            if (_completed) return false;

            if (_moveNext()) return true;

            Current = default;

            _moveNext = null;

            _completed = true;

            return false;
        }

        protected override void ResetOverride()
        {
            base.ResetOverride();

            _joinEnumerator.Reset();

            _firstValue = default;

            _completed = false;

            InitDelegate();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _joinEnumerator = null;

            _firstValue = default;

            _moveNext = null;
        }
    }

    public sealed class JoinEnumerator<T> : Enumerator<IEnumerable<T>, T>
    {
        private IEnumerator<T> _subEnumerator;
        private IEnumerable<T> _joinEnumerable;
        private bool _completed = false;
        private Action _updateEnumerator;
        private Func<bool> _moveNext;
        private readonly bool _keepEmptyEnumerables;

        public JoinEnumerator(IEnumerable<IEnumerable<T>> enumerable, bool keepEmptyEnumerables, params T[] join) : base(enumerable)
        {
            _joinEnumerable = join;

            _keepEmptyEnumerables = keepEmptyEnumerables;

            InitDelegates();
        }

        private void InitDelegates()
        {
            _updateEnumerator = () =>
            {
                _subEnumerator = InnerEnumerator.Current.GetEnumerator();

                if (_keepEmptyEnumerables)

                    _updateEnumerator = () => _subEnumerator = _joinEnumerable.AppendValues(InnerEnumerator.Current).GetEnumerator();

                else

                    _updateEnumerator = () => _subEnumerator = new JoinSubEnumerator<T>(InnerEnumerator.Current, _joinEnumerable);
            };

            _moveNext = () =>
            {

                if (_subEnumerator == null)
                {
                    _MoveNext();

                    if (_completed)

                        return false;
                }

                _moveNext = () => __MoveNext();

                return __MoveNext();
            };
        }

        private bool __MoveNext()
        {
            bool moveNext()
            {
                if (_subEnumerator.MoveNext())
                {
                    Current = _subEnumerator.Current;

                    return true;
                }

                return false;
            }

            while (!_completed)
            {
                if (moveNext())

                    return true;

                _MoveNext();
            }

            return false;
        }

        private void _MoveNext()
        {
            if (InnerEnumerator.MoveNext())

                _updateEnumerator();

            _completed = true;

            _subEnumerator = null;
        }

        protected override bool MoveNextOverride() => _completed ? false : _moveNext();

        protected override void ResetOverride()
        {
            base.ResetOverride();

            _subEnumerator = null;

            InitDelegates();

            _completed = false;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Current = default;

            _joinEnumerable = null;

            _updateEnumerator = null;

            _moveNext = null;

            _subEnumerator = null;
        }
    }
}

#endif
