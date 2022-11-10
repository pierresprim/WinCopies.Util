#if WinCopies3 && CS5
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using WinCopies.Collections.Generic;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public interface IReadOnlyDictionary<TKey, TValue> : System.Collections.Generic.IEnumerable<KeyValuePair<TKey, TValue>>, IReadOnlyUIntCollection<KeyValuePair<TKey, TValue>>
    {
        TValue this[TKey key] { get; }

        IReadOnlyUIntCollection<TKey> Keys { get; }

        IReadOnlyUIntCollection<TValue> Values { get; }

        bool Contains(KeyValuePair<TKey, TValue> item);

        bool ContainsKey(TKey key);

        bool ContainsValue(TValue value);

        bool TryGetValue(TKey key, out TValue value);
    }

    public interface IReadOnlyLinkedDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        IReadOnlyLinkedListNodeBase<KeyValuePair<TKey, TValue>> First { get; }

        IReadOnlyLinkedListNodeBase<KeyValuePair<TKey, TValue>> Last { get; }
    }

    public interface IDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IUIntCollection<KeyValuePair<TKey, TValue>>, System.Collections.Generic.IEnumerable<KeyValuePair<TKey, TValue>>
    {
#if !WinCopies4
        new TValue this[TKey key] { get; set; }

        new void Add(TKey key, TValue value);

        new bool Remove(TKey key);
#endif
    }

    public interface ILinkedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyLinkedDictionary<TKey, TValue>
    {
        new ILinkedListNodeBase<KeyValuePair<TKey, TValue>> First { get; }
        new ILinkedListNodeBase<KeyValuePair<TKey, TValue>> Last { get; }
#if CS8
        IReadOnlyLinkedListNodeBase<KeyValuePair<TKey, TValue>> IReadOnlyLinkedDictionary<TKey, TValue>.First => First;
        IReadOnlyLinkedListNodeBase<KeyValuePair<TKey, TValue>> IReadOnlyLinkedDictionary<TKey, TValue>.Last => Last;
#endif
        KeyValuePair<TKey, TValue> RemoveFirst();
        KeyValuePair<TKey, TValue> RemoveLast();
    }
}
#endif
