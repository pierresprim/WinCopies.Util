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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
#if WinCopies2
using IDisposable = WinCopies.Util.DotNetFix.IDisposable;
using WinCopies.Util;
using static WinCopies.Util.Util;
#else
using IDisposable = WinCopies.DotNetFix.IDisposable;
using WinCopies;
using static WinCopies.UtilHelpers;
#endif

namespace WinCopies.Collections
{
    public sealed class Enumerable<T> : IEnumerable<T>
    {
        private readonly Func<IEnumerator<T>> _enumeratorFunc;

        public Enumerable(Func<IEnumerator<T>> enumeratorFunc) => _enumeratorFunc = enumeratorFunc;

        public IEnumerator<T> GetEnumerator() => _enumeratorFunc();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public abstract class Enumerator<TSource, TDestination> : IEnumerator<TDestination>, WinCopies.
#if WinCopies2
        Util.
#endif
        DotNetFix.IDisposable
    {
        public bool IsDisposed { get; private set; }

        private IEnumerator<TSource> _innerEnumerator;

        protected IEnumerator<TSource> InnerEnumerator => IsDisposed ? throw GetExceptionForDispose(false) : _innerEnumerator;

        private TDestination _current;

        public TDestination Current { get => IsDisposed ? throw GetExceptionForDispose(false) : _current; protected set => _current = IsDisposed ? throw GetExceptionForDispose(false) : value; }

        object IEnumerator.Current => Current;

        public Enumerator(IEnumerable<TSource> enumerable) => _innerEnumerator = (enumerable ?? throw GetArgumentNullException(nameof(enumerable))).GetEnumerator();

        public bool MoveNext() => IsDisposed ? throw GetExceptionForDispose(false) : MoveNextOverride();

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
            if (!IsDisposed)
            {
                if (disposing)

                    _innerEnumerator = null;

                IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);

            GC.SuppressFinalize(this);
        }
    }

    public interface IEnumerableEnumerator
    {
        object Current { get; }

        bool MoveNext();
    }

    public interface IEnumerableEnumerator<T> : IEnumerableEnumerator, System.IDisposable
    {
        T Current { get; }
    }

    public sealed class JoinSubEnumerator<T> : Enumerator<T, T>
    {
        private IEnumerator<T> _joinEnumerator;
        private T _firstValue;
        private bool _completed = false;
        private Func<bool> _moveNext;

        public JoinSubEnumerator(IEnumerable<T> subEnumerable, IEnumerable<T> joinEnumerable) : base(subEnumerable)
        {
#if DEBUG
            Debug.Assert(subEnumerable != null && joinEnumerable != null);
#endif

            _joinEnumerator =
#if !DEBUG
                (
#endif
                joinEnumerable
#if !DEBUG
                ?? throw GetArgumentNullException())
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

#if WinCopies2
    [Obsolete("This type has been replaced by the types in the WinCopies.Collections.DotNetFix namespace and will be removed in later versions.")]
    public interface IUIntIndexedCollection
    {
        object this[uint index] { get; }

        uint Count { get; }
    }

    [Obsolete("This type has been replaced by the types in the WinCopies.Collections.DotNetFix namespace and will be removed in later versions.")]
    public interface IUIntIndexedCollection<T> : IUIntIndexedCollection
    {
        T this[uint index] { get; }
    }

    [Obsolete("This type has been replaced by the types in the WinCopies.Collections.DotNetFix namespace and will be removed in later versions.")]
    public abstract class UIntIndexedCollectionEnumeratorBase : IDisposable
    {
        protected internal IUIntIndexedCollection UIntIndexedCollection { get; private set; }
        protected internal uint? Index { get; set; } = null;
        protected internal Func<bool> MoveNextMethod { get; set; }

        public UIntIndexedCollectionEnumeratorBase(IUIntIndexedCollection uintIndexedCollection)
        {
            UIntIndexedCollection = uintIndexedCollection;

            MoveNextMethod = () => UIntIndexedCollectionEnumerator.MoveNextMethod(this);
        }

        #region IDisposable Support
        public bool IsDisposed { get; private set; } = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    UIntIndexedCollection = null;

                    Index = null;
                }

                IsDisposed = true;
            }
        }

        public void Dispose() => Dispose(true);

        public virtual bool MoveNext() => MoveNextMethod();

        public virtual void Reset()
        {
            Index = null;

            MoveNextMethod = () => UIntIndexedCollectionEnumerator.MoveNextMethod(this);
        }
        #endregion
    }

    [Obsolete("This type has been replaced by the types in the WinCopies.Collections.DotNetFix namespace and will be removed in later versions.")]
    public sealed class UIntIndexedCollectionEnumerator : UIntIndexedCollectionEnumeratorBase, IEnumerator
    {
        public static Func<UIntIndexedCollectionEnumeratorBase, bool> MoveNextMethod => (UIntIndexedCollectionEnumeratorBase e) =>
        {
            if (e.UIntIndexedCollection.Count > 0)
            {
                e.Index = 0;

                e.MoveNextMethod = () =>
                {
                    if (e.Index < e.UIntIndexedCollection.Count - 1)
                    {
                        e.Index++;

                        return true;
                    }

                    else return false;
                };

                return true;
            }

            else return false;
        };

        public object Current
        {
            get
            {
                Debug.Assert(Index.HasValue, "_index does not have value.");

                return UIntIndexedCollection[Index.Value];
            }
        }

        public UIntIndexedCollectionEnumerator(IUIntIndexedCollection uintIndexedCollection) : base(uintIndexedCollection) { }
    }

    [Obsolete("This type has been replaced by the types in the WinCopies.Collections.DotNetFix namespace and will be removed in later versions.")]
    public sealed class UIntIndexedCollectionEnumerator<T> : UIntIndexedCollectionEnumeratorBase, IEnumerator<T>
    {
        public T Current
        {
            get
            {
                Debug.Assert(Index.HasValue, "_index does not have value.");

                return ((IUIntIndexedCollection<T>)UIntIndexedCollection)[Index.Value];
            }
        }

        object IEnumerator.Current => Current;

        public UIntIndexedCollectionEnumerator(IUIntIndexedCollection<T> uintIndexedCollection) : base(uintIndexedCollection) { }
    }

#endif

}

//public interface IList : System.Collections. IList, ICollection, IEnumerable

//{



//}

//public interface IList<T> : System.Collections.Generic.IList<T>, System.Collections.Generic.IReadOnlyList<T>, ICollection, IList

//{

//    new void Clear();

//    new int Count { get; }

//    //new IEnumerator<T> GetEnumerator();

//    //new bool IsReadOnly { get; }

//    new void RemoveAt(int index);

//    new T this[int index] { get; set; }

//}

//public interface IReadOnlyList<T> : System.Collections.Generic.IList<T>, System.Collections.Generic.IReadOnlyList<T>, ICollection, IList

//{

//    new void Clear();

//    new int Count { get; }

//    //new IEnumerator<T> GetEnumerator();

//    //new bool IsReadOnly { get; }

//    new void RemoveAt(int index);

//    new T this[int index] { get; set; }

//}

//public class ReadOnlyCollection<T> : System.Collections.ObjectModel.ReadOnlyCollection<T>, IReadOnlyList<T>

//{

//    public ReadOnlyCollection(System.Collections.Generic.IList<T> list) : base(list) { } 

//    T IReadOnlyList<T>.this[int index] { get => this[index]; /*set => throw new NotSupportedException("This collection is read-only.") ;*/ } 

//    // int IReadOnlyCollection<T>.Count => Count ; 

//    // bool IReadOnlyList<T>.IsReadOnly => true ; 

//    //void IReadOnlyList<T>.Clear() => throw new NotSupportedException("This collection is read-only.") ; 

//    // IEnumerator<T> IReadOnlyCollection<T>.GetEnumerator() => throw new NotImplementedException();

//    //void IReadOnlyList<T>.RemoveAt(int index) => throw new NotSupportedException("This collection is read-only.") ;

//}

//public interface ICollection<T, U> : System.Collections.Generic.ICollection<U> where T : U

//{



//}
