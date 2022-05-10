#if WinCopies3
using System;
using System.Collections;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Enumeration.Generic;
using WinCopies.Linq;

namespace WinCopies.Collections.Generic
{
    public interface IGenericEnumerableArray<T> : System.Collections.Generic.IEnumerable<T>,
#if CS5
        IStructuralComparable, IStructuralEquatable, System.Collections.Generic.
#endif
        IReadOnlyCollection<T>,
#if CS5
        System.Collections.Generic.
#endif
        IReadOnlyList<T>, IAsEnumerable<T>, ICloneable, IList, ICollection, ICountable, IIndexable
    {
        // Left empty.
    }

    public class GenericEnumerableArray<T> : IGenericEnumerableArray<T>,
#if CS5
        Generic.
#endif
        IReadOnlyCollection<T>,
#if CS5
        Generic.
#endif
        IReadOnlyList<T>
    {
        protected Array Array { get; }

        public bool IsReadOnly => Array.IsReadOnly;

        public bool IsFixedSize => Array.IsFixedSize;

        public int Count => Array.Length;

        public object SyncRoot => Array.SyncRoot;

        public bool IsSynchronized => Array.IsSynchronized;

        public T this[int index] { get => (T)Array.GetValue(index); set => Array.SetValue(value, index); }

        object IList.this[int index] { get => Array.GetValue(index); set => Array.SetValue(value, index); }

        object IIndexableW.this[int index] { set => this[index] = (T)value; }

        public GenericEnumerableArray(in Array array) => Array = array;

        public System.Collections.Generic.IEnumerable<T> AsEnumerable() => Array.To<T>();

        public System.Collections.Generic.IEnumerator<T> GetEnumerator() => AsEnumerable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public object Clone() => Array.Clone();

        int IList.Add(object value) => ((IList)Array).Add(value);

        public bool Contains(T value) => ((IList)Array).Contains(value);

        bool IList.Contains(object value) => ((IList)Array).Contains(value);

        public void Clear() => ((IList)Array).Clear();

        public int IndexOf(object value) => ((IList)Array).IndexOf(value);

        public void Insert(int index, object value) => ((IList)Array).Insert(index, value);

        public void Remove(object value) => ((IList)Array).Remove(value);

        public void RemoveAt(int index) => ((IList)Array).RemoveAt(index);

        public void CopyTo(Array array, int index) => Array.CopyTo(array, index);

#if CS5
        public int CompareTo(object other, System.Collections.IComparer comparer) => ((IStructuralComparable)Array).CompareTo(other, comparer);

        public bool Equals(object other, IEqualityComparer comparer) => ((IStructuralEquatable)Array).Equals(other, comparer);

        public int GetHashCode(IEqualityComparer comparer) => ((IStructuralEquatable)Array).GetHashCode(comparer);
#else
        object IReadOnlyList.this[int index] => Array.GetValue(index);
#endif

        private CountableEnumerator<T> GetCountableEnumerator() => new
#if !CS9
            CountableEnumerator<T>
#endif
            (GetEnumerator(), () => Array.Length);

        ICountableEnumerator<T> Generic.IReadOnlyList<T>.GetEnumerator() => GetCountableEnumerator();

        ICountableEnumerator<T> ICountableEnumerable<T, ICountableEnumerator<T>>.GetEnumerator() => GetCountableEnumerator();

        public void CopyTo(T[] array, int arrayIndex) => this.CopyTo(array, arrayIndex, Count);

#if !CS8
        object IIndexableR.this[int index] => Array.GetValue(index);

        ICountableEnumerator<T> Enumeration.DotNetFix.IEnumerable<ICountableEnumerator<T>>.GetEnumerator() => GetCountableEnumerator();

        ICountableEnumerator<T> DotNetFix.Generic.IEnumerable<T, ICountableEnumerator<T>>.GetEnumerator() => GetCountableEnumerator();

        DotNetFix.ICountableEnumerator Enumeration.DotNetFix.IEnumerable<DotNetFix.ICountableEnumerator>.GetEnumerator() => GetCountableEnumerator();
#endif
    }
}
#endif
