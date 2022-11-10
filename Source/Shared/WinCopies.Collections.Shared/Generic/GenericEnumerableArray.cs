using System;
using System.Collections;
using System.Linq;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Enumeration.Generic;
using WinCopies.Linq;
using WinCopies.Util;

namespace WinCopies.Collections.Generic
{
    public interface IGenericEnumerableArray<T> : System.Collections.Generic.IEnumerable<T>,
#if CS5
        IStructuralComparable, IStructuralEquatable, System.Collections.Generic.
#endif
        IReadOnlyCollection<T>,
#if CS5
        System.Collections.
#else
        Extensions.
#endif
        Generic.IReadOnlyList<T>, IAsEnumerable<T>, ICloneable, IList, ICollection, ICountable, IIndexable
    {
        // Left empty.
    }

    public class GenericEnumerableArray<T> : IGenericEnumerableArray<T>,
#if CS5
        Generic.
#endif
        IReadOnlyCollection<T>, Extensions.Generic.IReadOnlyList<T>
    {
        protected System.Array Array { get; }

        protected IList List => Array.AsFromType<IList>();

        public bool IsReadOnly => Array.IsReadOnly;

        public bool IsFixedSize => Array.IsFixedSize;

        public int Count => Array.Length;

        public object SyncRoot => Array.SyncRoot;

        public bool IsSynchronized => Array.IsSynchronized;

        public T this[int index] { get => (T)Array.GetValue(index); set => Array.SetValue(value, index); }

        object IList.this[int index] { get => Array.GetValue(index); set => Array.SetValue(value, index); }

        object IIndexableW.this[int index] { set => this[index] = (T)value; }

        public GenericEnumerableArray(in System.Array array) => Array = array;

        public System.Collections.Generic.IEnumerable<T> AsEnumerable() => Array.Cast<T>();
        public System.Collections.Generic.IEnumerator<T> GetEnumerator() => AsEnumerable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public object Clone() => Array.Clone();

        int IList.Add(object value) => List.Add(value);

        public bool Contains(T value) => List.Contains(value);

        bool IList.Contains(object value) => List.Contains(value);

        public void Clear() => List.Clear();

        public int IndexOf(object value) => List.IndexOf(value);

        public void Insert(int index, object value) => List.Insert(index, value);

        public void Remove(object value) => List.Remove(value);

        public void RemoveAt(int index) => List.RemoveAt(index);

        public void CopyTo(System.Array array, int index) => Array.CopyTo(array, index);
#if CS5
        public int CompareTo(object other, System.Collections.IComparer comparer) => Array.AsFromType<IStructuralComparable>().CompareTo(other, comparer);

        public bool Equals(object other, IEqualityComparer comparer) => Array.AsFromType<IStructuralEquatable>().Equals(other, comparer);

        public int GetHashCode(IEqualityComparer comparer) => Array.AsFromType<IStructuralEquatable>().GetHashCode(comparer);
#else
        object IReadOnlyList.this[int index] => Array.GetValue(index);
#endif

        private CountableEnumerator<T> GetCountableEnumerator() => new
#if !CS9
            CountableEnumerator<T>
#endif
            (GetEnumerator(), () => Array.Length);

        ICountableEnumerator<T> Extensions.Generic.IReadOnlyList<T>.GetEnumerator() => GetCountableEnumerator();

        public void CopyTo(T[] array, int arrayIndex) => this.CopyTo(array, arrayIndex, Count);

        ICountableEnumerator<T> Enumeration.IEnumerable<ICountableEnumerator<T>>.GetEnumerator() => GetCountableEnumerator();
        DotNetFix.ICountableEnumerator Enumeration.IEnumerable<DotNetFix.ICountableEnumerator>.GetEnumerator() => GetCountableEnumerator();
#if !CS8
        object IIndexableR.this[int index] => Array.GetValue(index);
#endif
    }
}
