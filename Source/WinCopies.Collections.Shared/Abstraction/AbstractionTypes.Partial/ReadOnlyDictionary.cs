/* Copyright © Pierre Sprimont, 2021
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

#if CS7
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WinCopies.Collections.AbstractionInterop.Generic
{
    public static partial class AbstractionTypes<TSource, TDestination>
    {
        public class ReadOnlyDictionary<TSourceKey, TDestinationKey, TDictionary> : IReadOnlyDictionary<TDestinationKey, TDestination> where TSourceKey : TDestinationKey where TDictionary : System.Collections.Generic.IReadOnlyDictionary<TSourceKey, TSource>
        {
            protected TDictionary InnerDictionary { get; }

            TDestination IReadOnlyDictionary<TDestinationKey, TDestination>.this[TDestinationKey key] => InnerDictionary[(TSourceKey)key];

            IEnumerable<TDestinationKey> IReadOnlyDictionary<TDestinationKey, TDestination>.Keys => InnerDictionary.Keys.Select<TSourceKey, TDestinationKey>(item => item);

            IEnumerable<TDestination> IReadOnlyDictionary<TDestinationKey, TDestination>.Values => InnerDictionary.Values.Select<TSource, TDestination>(item => item);

            int System.Collections.Generic.IReadOnlyCollection<KeyValuePair<TDestinationKey, TDestination>>.Count => InnerDictionary.Count;

            public ReadOnlyDictionary(in TDictionary dictionary) => InnerDictionary = dictionary;

            bool IReadOnlyDictionary<TDestinationKey, TDestination>.ContainsKey(TDestinationKey key) => InnerDictionary.ContainsKey((TSourceKey)key);

            IEnumerator<KeyValuePair<TDestinationKey, TDestination>> IEnumerable<KeyValuePair<TDestinationKey, TDestination>>.GetEnumerator() => InnerDictionary.Select(item => new KeyValuePair<TDestinationKey, TDestination>(item.Key, item.Value)).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerDictionary).GetEnumerator();

            bool IReadOnlyDictionary<TDestinationKey, TDestination>.TryGetValue(TDestinationKey key, out TDestination value)
            {
                bool result = InnerDictionary.TryGetValue((TSourceKey)key, out TSource _value);

                value = _value;

                return result;
            }
        }
    }
}
#endif
