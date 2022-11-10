#if CS7
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using WinCopies;
using WinCopies.Collections.Abstraction.Generic.Abstract;
using WinCopies.Linq;
using WinCopies.Util;

namespace WinCopies.Collections.Abstraction.Generic
{
    public struct DictionarySelector<TSourceKey, TSourceValue, TDestinationKey, TDestinationValue>
    {
        public Converter<TSourceKey, TDestinationKey> KeySelector { get; }
        public Converter<TSourceValue, TDestinationValue> ValueSelector { get; }

        public DictionarySelector(in Converter<TSourceKey, TDestinationKey> keySelector, in Converter<TSourceValue, TDestinationValue> valueSelector)
        {
            KeySelector = keySelector;
            ValueSelector = valueSelector;
        }
    }

    public struct DictionarySelectors<TSourceKey, TSourceValue, TDestinationKey, TDestinationValue>
    {
        public DictionarySelector<TSourceKey, TSourceValue, TDestinationKey, TDestinationValue> Selector { get; }
        public DictionarySelector<TDestinationKey, TDestinationValue, TSourceKey, TSourceValue> ReversedSelector { get; }

        public DictionarySelectors(in DictionarySelector<TSourceKey, TSourceValue, TDestinationKey, TDestinationValue> selector, in DictionarySelector<TDestinationKey, TDestinationValue, TSourceKey, TSourceValue> reversedSelector)
        {
            Selector = selector;
            ReversedSelector = reversedSelector;
        }
    }

    public abstract class ReadOnlyDictionaryBase<TEnumerable, TSourceKey, TSourceValue, TDestinationKey, TDestinationValue> : ReadOnlyListBase<TEnumerable, KeyValuePair<TSourceKey, TSourceValue>, KeyValuePair<TDestinationKey, TDestinationValue>> where TEnumerable : IEnumerable<KeyValuePair<TSourceKey, TSourceValue>>
    {
        #region Selectors
        protected DictionarySelectors<TSourceKey, TSourceValue, TDestinationKey, TDestinationValue> Selectors { get; }
        protected DictionarySelector<TSourceKey, TSourceValue, TDestinationKey, TDestinationValue> ItemSelector => Selectors.Selector;
        public Converter<TSourceKey, TDestinationKey> KeySelector => ItemSelector.KeySelector;
        public Converter<TSourceValue, TDestinationValue> ValueSelector => ItemSelector.ValueSelector;
        protected DictionarySelector<TDestinationKey, TDestinationValue, TSourceKey, TSourceValue> ReversedSelector => Selectors.ReversedSelector;
        public Converter<TDestinationKey, TSourceKey> ReversedKeySelector => ReversedSelector.KeySelector;
        public Converter<TDestinationValue, TSourceValue> ReversedValueSelector => ReversedSelector.ValueSelector;
        #endregion Selectors

        public ReadOnlyDictionaryBase(in TEnumerable innerList, DictionarySelectors<TSourceKey, TSourceValue, TDestinationKey, TDestinationValue> selectors) : base(innerList, item =>
                {
                    var selector = selectors.Selector;

                    return UtilHelpers.GetKeyValuePair(selector.KeySelector(item.Key), selector.ValueSelector(item.Value));
                })
        { /* Left empty. */ }
    }

    public class ReadOnlyDictionary<TEnumerable, TSourceKey, TSourceValue, TDestinationKey, TDestinationValue> : ReadOnlyDictionaryBase<TEnumerable, TSourceKey, TSourceValue, TDestinationKey, TDestinationValue>, IReadOnlyDictionary<TDestinationKey, TDestinationValue> where TEnumerable : IReadOnlyDictionary<TSourceKey, TSourceValue>
    {
        public override int Count => InnerEnumerable.Count;

        public  IEnumerable<TDestinationKey> Keys => InnerEnumerable.Keys.SelectConverter(KeySelector);

        public  IEnumerable<TDestinationValue> Values => InnerEnumerable.Values.SelectConverter(ValueSelector);

        public  TDestinationValue this[TDestinationKey key] => ValueSelector(InnerEnumerable[ReversedKeySelector(key)]);

        public ReadOnlyDictionary(in TEnumerable innerList, DictionarySelectors<TSourceKey, TSourceValue, TDestinationKey, TDestinationValue> selectors) : base(innerList, selectors) { /* Left empty. */ }

        public  bool ContainsKey(TDestinationKey key) => InnerEnumerable.ContainsKey(ReversedKeySelector(key));
        public  bool TryGetValue(TDestinationKey key,
#if CS8
            [MaybeNullWhen(false)]
#endif
        out TDestinationValue value)
        {
            if (InnerEnumerable.TryGetValue(ReversedKeySelector(key), out TSourceValue
#if CS9
                ?
#endif
                _value))
            {
                value = ValueSelector(_value);

                return true;
            }

            value = default;

            return false;
        }
    }

    public class ReadOnlyDictionary<TSourceKey, TSourceValue, TDestinationKey, TDestinationValue> : ReadOnlyDictionary<IReadOnlyDictionary<TSourceKey, TSourceValue>, TSourceKey, TSourceValue, TDestinationKey, TDestinationValue>
    {
        public ReadOnlyDictionary(in IReadOnlyDictionary<TSourceKey, TSourceValue> innerList, in DictionarySelectors<TSourceKey, TSourceValue, TDestinationKey, TDestinationValue> selectors) : base(innerList, selectors) { /* Left empty. */ }
    }

    public class Dictionary<TEnumerable, TSourceKey, TSourceValue, TDestinationKey, TDestinationValue> : ReadOnlyDictionaryBase<TEnumerable, TSourceKey, TSourceValue, TDestinationKey, TDestinationValue>, IDictionary<TDestinationKey, TDestinationValue> where TEnumerable : IDictionary<TSourceKey, TSourceValue>
    {
        private ICollection<TDestinationKey>
#if CS8
            ?
#endif
            _keys;
        private ICollection<TDestinationValue>
#if CS8
            ?
#endif
            _values;

        protected IDictionary<TSourceKey, TSourceValue> InnerDictionary => InnerEnumerable.AsFromType<IDictionary<TSourceKey, TSourceValue>>();

        public override int Count => InnerEnumerable.Count;

        public  TDestinationValue this[TDestinationKey key] { get => ValueSelector(InnerEnumerable[ReversedKeySelector(key)]); set => InnerDictionary[ReversedKeySelector(key)] = ReversedValueSelector(value); }

        public bool IsReadOnly => InnerEnumerable.IsReadOnly;

        public  ICollection<TDestinationKey> Keys => GetCollection(ref _keys, InnerDictionary.Keys, KeySelector, ReversedKeySelector);

        public  ICollection<TDestinationValue> Values => GetCollection(ref _values, InnerDictionary.Values, ValueSelector, ReversedValueSelector);

        public Dictionary(in TEnumerable innerList, DictionarySelectors<TSourceKey, TSourceValue, TDestinationKey, TDestinationValue> selectors) : base(innerList, selectors) { /* Left empty. */ }

        protected ICollection<TDestination> GetCollection<TSource, TDestination>(ref ICollection<TDestination>
#if CS8
            ?
#endif
            collection, in ICollection<TSource> source, in Converter<TSource, TDestination> selector, in Converter<TDestination, TSource> reversedSelector) => collection
#if CS8
            ??=
#else
            ?? (collection =
#endif
            new Collection<TSource, TDestination>(new Extensions.Generic.Collection<TSource>(source), selector, reversedSelector)
#if !CS8
            )
#endif
            ;

        protected KeyValuePair<TSourceKey, TSourceValue> ReversedSelection(in KeyValuePair<TDestinationKey, TDestinationValue> item) => new
#if !CS9
            KeyValuePair<TSourceKey, TSourceValue>
#endif
            (ReversedKeySelector(item.Key), ReversedValueSelector(item.Value));

        public void Add(TDestinationKey key, TDestinationValue value) => InnerEnumerable.Add(ReversedKeySelector(key), ReversedValueSelector(value));
        public void Add(KeyValuePair<TDestinationKey, TDestinationValue> item) => InnerEnumerable.Add(ReversedSelection(item));
        public void Clear() => InnerEnumerable.Clear();
        public bool Contains(KeyValuePair<TDestinationKey, TDestinationValue> item) => InnerEnumerable.Contains(ReversedSelection(item));
        public void CopyTo(KeyValuePair<TDestinationKey, TDestinationValue>[] array, int arrayIndex) => InnerEnumerable.ToArray(array, arrayIndex, Count, Selector);
        public bool Remove(TDestinationKey key) => InnerEnumerable.Remove(ReversedKeySelector(key));
        public bool Remove(KeyValuePair<TDestinationKey, TDestinationValue> item) => InnerEnumerable.Remove(ReversedSelection(item));
        public  bool ContainsKey(TDestinationKey key) => InnerEnumerable.ContainsKey(ReversedKeySelector(key));
        public  bool TryGetValue(TDestinationKey key,
#if CS8
            [MaybeNullWhen(false)]
#endif
        out TDestinationValue value)
        {
            if (InnerEnumerable.TryGetValue(ReversedKeySelector(key), out TSourceValue
#if CS9
                ?
#endif
                _value))
            {
                value = ValueSelector(_value);

                return true;
            }

            value = default;

            return false;
        }
    }

    public class Dictionary<TSourceKey, TSourceValue, TDestinationKey, TDestinationValue> : Dictionary<Extensions.Generic.IDictionary<TSourceKey, TSourceValue>, TSourceKey, TSourceValue, TDestinationKey, TDestinationValue>
    {
        public Dictionary(in Extensions.Generic.IDictionary<TSourceKey, TSourceValue> innerList, DictionarySelectors<TSourceKey, TSourceValue, TDestinationKey, TDestinationValue> selectors) : base(innerList, selectors) { /* Left empty. */ }
    }
}
#endif
