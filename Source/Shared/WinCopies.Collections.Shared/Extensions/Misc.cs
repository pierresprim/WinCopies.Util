#if CS5
using System.Collections;

namespace WinCopies.Collections.Extensions.Generic
{
    public interface ICollection<T> : System.Collections.Generic.ICollection<T>, System.Collections.Generic.IReadOnlyCollection<T>
    {
        // Left empty.
    }

    public class Collection<TItems, TCollection> : ICollection<TItems>
#if CS8
        , DotNetFix.Generic.IEnumerable<TItems>
#endif
        where TCollection : System.Collections.Generic.ICollection<TItems>
    {
        protected TCollection InnerCollection { get; }

        public int Count => InnerCollection.Count;
        public bool IsReadOnly => InnerCollection.IsReadOnly;

        public Collection(in TCollection collection) => InnerCollection = collection;

        public void Add(TItems item) => InnerCollection.Add(item);
        public void Clear() => InnerCollection.Clear();
        public bool Contains(TItems item) => InnerCollection.Contains(item);
        public void CopyTo(TItems[] array, int arrayIndex) => InnerCollection.CopyTo(array, arrayIndex);
        public bool Remove(TItems item) => InnerCollection.Remove(item);

        public System.Collections.Generic.IEnumerator<TItems> GetEnumerator() => InnerCollection.GetEnumerator();
#if !CS8
        IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
#endif
    }

    public class Collection<T> : Collection<T, System.Collections.Generic.ICollection<T>>
    {
        public Collection(in System.Collections.Generic.ICollection<T> collection) : base(collection) { /* Left empty. */ }
    }

    public interface IDictionary<TKey, TValue> : System.Collections.Generic.IDictionary<TKey, TValue>, System.Collections.Generic.IReadOnlyDictionary<TKey, TValue>
    {
#if CS8
        System.Collections.Generic.IEnumerable<TKey> System.Collections.Generic.IReadOnlyDictionary<TKey, TValue>.Keys => this.AsDictionary().Keys;
        System.Collections.Generic.IEnumerable<TValue> System.Collections.Generic.IReadOnlyDictionary<TKey, TValue>.Values => this.AsDictionary().Values;
#endif
    }
}
#endif
