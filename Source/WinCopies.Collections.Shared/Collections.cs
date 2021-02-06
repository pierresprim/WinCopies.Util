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

#if !WinCopies3
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using IDisposable = WinCopies.Util.DotNetFix.IDisposable;
#endif

namespace WinCopies.Collections
{
    public interface ICountable
    {
        int Count { get; }
    }

    public interface IUIntCountable
    {
        uint Count { get; }
    }

    public interface IEnumeratorInfo :
#if !WinCopies3
System.Collections.IEnumerator
    {
        bool? IsResetSupported { get; }

        bool IsStarted { get; }

        bool IsCompleted { get; }
    }
    
    public interface IDisposableEnumeratorInfo : IEnumeratorInfo, WinCopies.Util.DotNetFix.IDisposable
    {
        // Left empty.
    }
#else
        WinCopies.Collections.DotNetFix.IEnumerator, WinCopies.Collections.DotNetFix.IEnumeratorInfo
    {
        // Left empty.
    }

    public interface IDisposableEnumeratorInfo : WinCopies.Collections.DotNetFix.IDisposableEnumeratorInfo, IEnumeratorInfo
    {
        // Left empty.
    }
#endif

#if !WinCopies3
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
    public abstract class UIntIndexedCollectionEnumeratorBase : WinCopies.Util.DotNetFix.IDisposable
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
    #endregion

        public virtual bool MoveNext() => MoveNextMethod();

        public virtual void Reset()
        {
            Index = null;

            MoveNextMethod = () => UIntIndexedCollectionEnumerator.MoveNextMethod(this);
        }
    }

    [Obsolete("This type has been replaced by the types in the WinCopies.Collections.DotNetFix namespace and will be removed in later versions.")]
    public sealed class UIntIndexedCollectionEnumerator : UIntIndexedCollectionEnumeratorBase, System.Collections.IEnumerator
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
    public sealed class UIntIndexedCollectionEnumerator<T> : UIntIndexedCollectionEnumeratorBase, System.Collections.Generic.IEnumerator<T>
    {
        public T Current
        {
            get
            {
                Debug.Assert(Index.HasValue, "_index does not have value.");

                return ((IUIntIndexedCollection<T>)UIntIndexedCollection)[Index.Value];
            }
        }

        object System.Collections.IEnumerator.Current => Current;

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

//    //new System.Collections.Generic.IEnumerator<T> GetEnumerator();

//    //new bool IsReadOnly { get; }

//    new void RemoveAt(int index);

//    new T this[int index] { get; set; }

//}

//public interface IReadOnlyList<T> : System.Collections.Generic.IList<T>, System.Collections.Generic.IReadOnlyList<T>, ICollection, IList

//{

//    new void Clear();

//    new int Count { get; }

//    //new System.Collections.Generic.IEnumerator<T> GetEnumerator();

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

//    // System.Collections.Generic.IEnumerator<T> IReadOnlyCollection<T>.GetEnumerator() => throw new NotImplementedException();

//    //void IReadOnlyList<T>.RemoveAt(int index) => throw new NotSupportedException("This collection is read-only.") ;

//}

//public interface ICollection<T, U> : System.Collections.Generic.ICollection<U> where T : U

//{



//}
