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

#if CS7
using System.Collections.Generic;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Util;

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections.Generic
{
    public class DictionaryMerger<TKey, TValue> : ArrayMerger<KeyValuePair<TKey, TValue>>
    {
        public Dictionary<TKey, TValue> ToDictionary(in bool remove = false)
        {
            ValidateRealCount();

            var dic = new Dictionary<TKey, TValue>((int)Count);

            ToDictionaryPrivate(dic, remove);

            return dic;
        }

        public void ToDictionary(in System.Collections.Generic.IDictionary<TKey, TValue> dic, in bool remove = false)
        {
            ValidateRealCount();

            ToDictionaryPrivate(dic ?? throw GetArgumentNullException(nameof(dic)), remove);
        }

        private void ToDictionaryPrivate(in System.Collections.Generic.IDictionary<TKey, TValue> dic, in bool remove)
        {
            if (remove)

                while (Count != 0)
                {
                    foreach (KeyValuePair<TKey, TValue> item in First.Value)

                        dic.Add(item.Key, item.Value);

                    RemoveFirst();
                }

            else

                foreach (IUIntCountableEnumerable<KeyValuePair<TKey, TValue>> _array in this.AsFromType<System.Collections.Generic.IEnumerable<IUIntCountableEnumerable<KeyValuePair<TKey, TValue>>>>())

                    foreach (KeyValuePair<TKey, TValue> item in _array)

                        dic.Add(item.Key, item.Value);
        }
    }
}
#endif
