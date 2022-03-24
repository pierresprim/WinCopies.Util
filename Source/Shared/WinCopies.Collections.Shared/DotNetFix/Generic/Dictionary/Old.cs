/*namespace WinCopies.Collections.DotNetFix.Generic
{
    public abstract class LinkedListDictionaryBase<TDictionary, TKey, TValue> : LinkedList<TDictionary, LinkedListDictionaryBase<TDictionary, TKey, TValue>.LinkedListNode, KeyValuePair<TKey, TValue>, TValue>, ILinkedListBase<LinkedListDictionaryBase<TDictionary, TKey, TValue>.LinkedListNode> where TDictionary : LinkedListDictionaryBase<TDictionary, TKey, TValue>
    {
        public class LinkedListNode : LinkedListNodeBase2
        {
            public TKey Key { get; private set; }

            public LinkedListNode(in KeyValuePair<TKey, TValue> value) : base(value.Value) => Key = value.Key;

            protected override KeyValuePair<TKey, TValue> Convert() => new
#if !CS9
                KeyValuePair<TKey, TValue>
#endif
                (Key, ValueW);
        }

        public abstract class DictionaryItemCollectionBase<T> : IReadOnlyUIntCollection<T>
        {
            protected LinkedListDictionaryBase<TDictionary, TKey, TValue> InnerDictionary { get; }

            public uint Count => InnerDictionary.Count;

            public DictionaryItemCollectionBase(in LinkedListDictionaryBase<TDictionary, TKey, TValue> dictionary) => InnerDictionary = dictionary;

            protected abstract T Convert(KeyValuePair<TKey, TValue> value);

            protected abstract bool CompareEquality(in T x, in T y);

            public bool Contains(T item)
            {
                foreach (KeyValuePair<TKey, TValue> _item in InnerDictionary)

                    if (CompareEquality(Convert(_item), item))

                        return true;

                return false;
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                WinCopies.ThrowHelper.ThrowOnInvalidCopyToArrayOperation(array, arrayIndex, Count, nameof(array), nameof(arrayIndex));

                void defaultAction(in T value) => array[arrayIndex] = value;

                Action<T> action = value =>
                {
                    action = _value =>
                    {
                        ++arrayIndex;

                        defaultAction(_value);
                    };

                    defaultAction(value);
                };

                foreach (T item in this)

                    action(item);
            }

            public IUIntCountableEnumerator<T> GetEnumerator() => new UIntCountableEnumerator<SelectEnumerator<KeyValuePair<TKey, TValue>, T>, T>(new SelectEnumerator<KeyValuePair<TKey, TValue>, T>(InnerDictionary, Convert), () => Count);

#if !CS8
            System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();
#endif

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            void IReadOnlyCollectionBase<T, T, T>.CopyToOut(T[] array, int arrayIndex) => CopyTo(array, arrayIndex);
        }

        public IReadOnlyUIntCollection<TKey> Keys { get; }

        public LinkedListDictionaryBase() => Keys = new KeyCollection(this);

        public void Add(KeyValuePair<TKey, TValue> item) => AddLast(item);

        public void Add(TKey key, TValue value) => Add(new KeyValuePair<TKey, TValue>(key, value));

        public bool ContainsKey(TKey key) => Keys.Contains(key);

        public abstract bool Remove(TKey key);

        public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);

        protected override LinkedListNode GetNewNode(in KeyValuePair<TKey, TValue> value) => new
#if !CS9
            LinkedListNode
#endif
            (value);
    }

    public class LinkedListDictionaryBase<TKey, TValue> : LinkedListDictionaryBase<LinkedListDictionaryBase<TKey, TValue>, TKey, TValue>, IDictionary<TKey, TValue>
    {
        public TValue this[TKey key]
        {
            get => TryGetValue(key, out TValue value) ? value : throw GetKeyNotFoundException(nameof(key));

            set
            {
                if (!TrySetValue(key, value))

                    ThrowKeyNotFoundException(nameof(key));
            }
        }

        public IReadOnlyUIntCollection<TValue> Values { get; }

        public LinkedListDictionaryBase() => Values = new ValueCollection(this);

        public bool TrySetValue(TKey key, TValue value)
        {
            var enumerable = new Enumerable<LinkedListNode>(GetNodeEnumerator);

            foreach (LinkedListNode item in enumerable)

                if (CompareHashCodeGenericIn(key, item.Key))
                {
                    item.ValueW = value;

                    return true;
                }

            return false;
        }

        public bool TryGetValue(in TKey key, in EqualityComparison<TKey> comparison,
#if CS8
            [MaybeNullWhen(false)]
#endif
            out TValue value)
        {
            WinCopies.ThrowHelper.ThrowIfNull(comparison, nameof(comparison));

            foreach (KeyValuePair<TKey, TValue> item in this)

                if (comparison(item.Key, key))
                {
                    value = item.Value;

                    return true;
                }

            value = default;

            return false;
        }

        public bool TryGetValue(TKey key,
#if CS8
            [MaybeNullWhen(false)] 
#endif
            out TValue value) => TryGetValue(key, CompareHashCodeGeneric, out value);

        protected override LinkedListDictionaryBase<TKey, TValue> AsTList() => this;

        protected override LinkedListEnumerator<LinkedListNode, LinkedListDictionaryBase<TKey, TValue>, KeyValuePair<TKey, TValue>, TValue> GetLinkedListEnumerator(in EnumerationDirection enumerationDirection) => new
#if !CS9
            LinkedListEnumerator<LinkedListNode, LinkedListDictionaryBase<TKey, TValue>, KeyValuePair<TKey, TValue>, TValue>
#endif
            (this, enumerationDirection);

        public override bool Remove(TKey key) => Util.Remove<LinkedListDictionaryBase<TKey, TValue>, TKey, TValue>(this, key);

        public bool ContainsValue(TValue value) => Values.Contains(value);

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            foreach (var _item in this)

                if (CompareHashCodeGenericIn(item.Key, _item.Key) && object.Equals(item.Value, _item.Value))

                    return true;

            return false;
        }

        public IReadOnlyDictionary<TKey, TValue> AsReadOnly() => new ReadOnlyLinkedDictionary<TKey, TValue>(new LinkedListDictionary<TKey, TValue>(this));
    }

    public class LinkedListDictionary<TKey, TValue> : LinkedCollection<KeyValuePair<TKey, TValue>, TValue, LinkedListDictionaryBase<TKey, TValue>>, IDictionary<TKey, TValue>
    {
        public TValue this[TKey key] { get => InnerList[key]; set => InnerList[key] = value; }

        TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key] => this[key];

        public IReadOnlyUIntCollection<TKey> Keys => InnerList.Keys;

        public IReadOnlyUIntCollection<TValue> Values => InnerList.Values;

        public LinkedListDictionary(in LinkedListDictionaryBase<TKey, TValue> list) : base(list)
        {
        }

        public void Add(TKey key, TValue value) => AddLast(new KeyValuePair<TKey, TValue>(key, value));

        public bool ContainsKey(TKey key) => InnerList.ContainsKey(key);

        public bool Remove(TKey key) => Util.Remove<LinkedListDictionaryBase<TKey, TValue>, TKey, TValue>(InnerList, key);

        public bool TryGetValue(TKey key,
#if CS9
            [MaybeNullWhen(false)]
#endif
            out TValue value) => InnerList.TryGetValue(key, out value);

        public ILinkedListNode<KeyValuePair<TKey, TValue>, TValue> Find(TKey key) => Util.Find<LinkedListDictionaryBase<TKey, TValue>, TKey, TValue>(InnerList, key);

        protected override bool OnAddItem(KeyValuePair<TKey, TValue> item) => InnerList.Keys.Contains(item.Key)
                ? throw new InvalidOperationException("The given key is already registered.")
                : base.OnAddItem(item);

        public bool ContainsValue(TValue value) => InnerList.ContainsValue(value);

        public bool Contains(KeyValuePair<TKey, TValue> item) => InnerList.Contains(item);

        public void Add(KeyValuePair<TKey, TValue> item) => InnerList.Add(item);

        public IReadOnlyDictionary<TKey, TValue> AsReadOnly() => AsReadOnly();
    }
}*/
