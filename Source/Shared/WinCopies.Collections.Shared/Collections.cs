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
    /// <summary>
    /// Represents a countable item.
    /// </summary>
    public interface ICountable
    {
        int Count { get; }
    }

    /// <summary>
    /// Represents a <see cref="uint"/>-countable item.
    /// </summary>
    public interface IUIntCountable
    {
        uint Count { get; }
    }

    /// <summary>
    /// Represents a <see cref="long"/>-countable item.
    /// </summary>
    public interface ILongCountable
    {
        long Count { get; }
    }

    /// <summary>
    /// Represents a <see cref="ulong"/>-countable item.
    /// </summary>
    public interface IULongCountable
    {
        ulong Count { get; }
    }

    public interface IIndexableR
    {
        object this[int index] { get; }
    }

    public interface IIndexableW
    {
        object this[int index] { set; }
    }

    public interface IIndexable : IIndexableR, IIndexableW
    {
        // Left empty.
    }

    public interface IUIntIndexableR
    {
        object this[uint index] { get; }
    }

    public interface IUIntIndexableW
    {
        object this[uint index] { set; }
    }

    public interface IUIntIndexable : IUIntIndexableR, IUIntIndexableW
    {
        // Left empty.
    }

    public interface ILongIndexableR
    {
        object this[long index] { get; }
    }

    public interface ILongIndexableW
    {
        object this[long index] { set; }
    }

    public interface ILongIndexable : ILongIndexableR, ILongIndexableW
    {
        // Left empty.
    }

    public interface IULongIndexableR
    {
        object this[ulong index] { get; }
    }

    public interface IULongIndexableW
    {
        object this[ulong index] { set; }
    }

    public interface IULongIndexable : IULongIndexableR, IULongIndexableW
    {
        // Left empty.
    }

    namespace Generic
    {
        public interface IIndexableR<out T> : IIndexableR
        {
            new T this[int index] { get; }

#if CS8
            object IIndexableR.this[int index] => this[index];
#endif
        }

        public interface IIndexableW<in T> : IIndexableW
        {
            new T this[int index] { set; }

#if CS8
            object IIndexableW.this[int index] { set => this[index] = (T)value; }
#endif
        }

        public interface IIndexable<T> : IIndexableR<T>, IIndexableW<T>, IIndexable
        {
            // Left empty.
        }

        public interface IUIntIndexableR<out T> : IUIntIndexableR
        {
            new T this[uint index] { get; }

#if CS8
            object IUIntIndexableR.this[uint index] => this[index];
#endif
        }

        public interface IUIntIndexableW<in T> : IUIntIndexableW
        {
            new T this[uint index] { set; }

#if CS8
            object IUIntIndexableW.this[uint index] { set => this[index] = (T)value; }
#endif
        }

        public interface IUIntIndexable<T> : IUIntIndexableR<T>, IUIntIndexableW<T>, IUIntIndexable
        {
            // Left empty.
        }

        public interface ILongIndexableR<out T> : ILongIndexableR
        {
            new T this[long index] { get; }

#if CS8
            object ILongIndexableR.this[long index] => this[index];
#endif
        }

        public interface ILongIndexableW<in T> : ILongIndexableW
        {
            new T this[long index] { set; }

#if CS8
            object ILongIndexableW.this[long index] { set => this[index] = (T)value; }
#endif
        }

        public interface ILongIndexable<T> : ILongIndexableR<T>, ILongIndexableW<T>, ILongIndexable
        {
            // Left empty.
        }

        public interface IULongIndexableR<out T> : IULongIndexableR
        {
            new T this[ulong index] { get; }

#if CS8
            object IULongIndexableR.this[ulong index] => this[index];
#endif
        }

        public interface IULongIndexableW<in T> : IULongIndexableW
        {
            new T this[ulong index] { set; }

#if CS8
            object IULongIndexableW.this[ulong index] { set => this[index] = (T)value; }
#endif
        }

        public interface IULongIndexable<T> : IULongIndexableR<T>, IULongIndexableW<T>, IULongIndexable
        {
            // Left empty.
        }
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

//    // int System.Collections.Generic.IReadOnlyCollection<T>.Count => Count ; 

//    // bool IReadOnlyList<T>.IsReadOnly => true ; 

//    //void IReadOnlyList<T>.Clear() => throw new NotSupportedException("This collection is read-only.") ; 

//    // System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IReadOnlyCollection<T>.GetEnumerator() => throw new NotImplementedException();

//    //void IReadOnlyList<T>.RemoveAt(int index) => throw new NotSupportedException("This collection is read-only.") ;

//}

//public interface System.Collections.Generic.ICollection<T, U> : System.Collections.Generic.ICollection<U> where T : U

//{



//}
