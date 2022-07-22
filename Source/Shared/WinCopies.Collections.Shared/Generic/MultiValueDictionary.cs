#if CS5 && WinCopies3
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using WinCopies.Linq;
using WinCopies.Util;

using static WinCopies.UtilHelpers;

namespace WinCopies.Collections.Generic
{
    public interface IReadOnlyIndexableDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IReadOnlyList<TValue>, IIndexable<TValue>, IReadOnlyList<KeyValuePair<TKey, TValue>>
    {
        // Left empty.
    }

    public interface IIndexableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyIndexableDictionary<TKey, TValue>
    {
        // Left empty.
    }

    public interface IMultiValueDictionary<TKey, TValue> : IReadOnlyList<IReadOnlyDictionary<TKey, TValue>>
    {
        IReadOnlyList<TKey> Keys { get; }
    }

    public class MultiValueDictionary<TKey, TValue> : IMultiValueDictionary<TKey, TValue>
    {
        public class Dictionary : IReadOnlyIndexableDictionary<TKey, TValue>, IAsEnumerable<KeyValuePair<TKey, TValue>>
        {
            protected MultiValueDictionary<TKey, TValue> InnerDictionary { get; }

            protected int Index { get; }

            public TValue this[TKey key] { get => this[GetIndex(key)]; set => this[GetIndex(key)] = value; }

            public TValue this[int index] { get => InnerDictionary.Values[Index, index]; set => InnerDictionary.Values[Index, index] = value; }

            KeyValuePair<TKey, TValue> System.Collections.Generic.IReadOnlyList<KeyValuePair<TKey, TValue>>.this[int index] => new
#if !CS9
                KeyValuePair<TKey, TValue>
#endif
                (InnerDictionary.InnerKeys[index], this[index]);

            public int Count => InnerDictionary.InnerKeys.Length;

            public IEnumerable<TKey> Keys => InnerDictionary.InnerKeys.AsReadOnlyEnumerable();

            public IEnumerable<TValue> Values => GetValueEnumerable();

            object IIndexableW.this[int index] { set => this[index] = (TValue)value; }

            object IIndexableR.this[int index] => this[index];

            public Dictionary(in MultiValueDictionary<TKey, TValue> dictionary, in int index)
            {
                InnerDictionary = dictionary;
                Index = index;
            }

            public int TryGetIndex(in TKey key)
            {
                int? _hashCode = key?.GetHashCode();

                if (_hashCode.HasValue)
                {
                    int hashCode = _hashCode.Value;
                    TKey[] keys = InnerDictionary.InnerKeys;

                    for (int i = 0; i < keys.Length; i++)

                        if (keys[i]?.GetHashCode() == hashCode)

                            return i;
                }

                return -1;
            }

            public int GetIndex(in TKey key) => GetValue(TryGetIndex(key), index => index >= 0, () => new KeyNotFoundException());

            public bool Contains(KeyValuePair<TKey, TValue> item)
            {
                int index = TryGetIndex(item.Key);

                if (index < 0)

                    return false;

                if (item.Value is IEquatable<TValue> equatable)

                    return equatable.Equals(this[index]);

                TValue value = this[index];

                return (equatable = value as IEquatable<TValue>) == null ? Equals(item.Value, value) : equatable.Equals(item.Value);
            }

            public bool ContainsKey(TKey key) => TryGetIndex(key) >= 0;

            public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            {
                foreach (KeyValuePair<TKey, TValue> keyValuePair in this)

                    array[arrayIndex++] = keyValuePair;
            }

            public bool TryGetValue(TKey key,
#if CS8
                [MaybeNullWhen(false)]
#endif
                out TValue value)
            {
                int index = TryGetIndex(key);

                if (index < 0)
                {
                    value = default;

                    return false;
                }

                value = this[index];

                return true;
            }

            public IEnumerable<TValue> GetValueEnumerable()
            {
                int length = InnerDictionary.InnerKeys.Length;
                TValue[,] values = InnerDictionary.Values;

                for (int i = 0; i < length; i++)

                    yield return values[Index, i];
            }

            public IEnumerable<KeyValuePair<TKey, TValue>> AsEnumerable()
            {
                TKey[] keys = InnerDictionary.InnerKeys;
                int length = keys.Length;
                TValue[,] values = InnerDictionary.Values;

                for (int i = 0; i < length; i++)

                    yield return new KeyValuePair<TKey, TValue>(keys[i], values[Index, i]);
            }

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => AsEnumerable().GetEnumerator();
            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() => Values.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        protected TKey[] InnerKeys { get; }
        protected TValue[,] Values { get; }
        protected Dictionary[] Items { get; }

        public int Count => Items.Length;

        public IReadOnlyList<TKey> Keys { get; }

        public Dictionary this[int index] => Items[index]
#if CS8
            ??=
#else
            ?? (Items[index] =
#endif
            (new Dictionary(this, index))
#if !CS8
            )
#endif
            ;

        IReadOnlyDictionary<TKey, TValue> IReadOnlyList<IReadOnlyDictionary<TKey, TValue>>.this[int index] => this[index];

        public MultiValueDictionary(in TKey[] keys, in int count)
        {
            foreach (TKey
#if CS9
                ?
#endif
                key in keys)

                if (key == null)

                    throw new ArgumentException("One or more keys are null.");

            Keys = new ReadOnlyArray<TKey>(keys);
            Values = new TValue[count, (InnerKeys = keys).Length];
            Items = new Dictionary[count];
        }

        public IEnumerator<Dictionary> GetEnumerator() => Items.AsEnumerable().GetEnumerator();
        IEnumerator<IReadOnlyDictionary<TKey, TValue>> IEnumerable<IReadOnlyDictionary<TKey, TValue>>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
#endif
