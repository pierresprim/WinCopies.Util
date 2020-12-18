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

using static WinCopies.
#if WinCopies2
    Util.Util
#else
    ThrowHelper
#endif
    ;

namespace WinCopies.Collections.Generic
{
    public class DictionaryBuilder<TKey, TValue> : ArrayBuilder<KeyValuePair<TKey, TValue>>
    {
        public Dictionary<TKey, TValue> ToDictionary(in bool remove = false)
        {
#if !WinCopies2
            ValidateCount();
#endif

            var dic = new Dictionary<TKey, TValue>(
#if !WinCopies2
(int)
#endif
                Count);

            ToDictionaryPrivate(dic, remove);

            return dic;
        }

        public void ToDictionary(in IDictionary<TKey, TValue> dic, in bool remove = false)
        {
#if !WinCopies2
            ValidateCount();
#endif

            ToDictionaryPrivate(dic ?? throw GetArgumentNullException(nameof(dic)), remove);
        }

        private void ToDictionaryPrivate(in IDictionary<TKey, TValue> dic, in bool remove)
        {
            if (remove)

                while (Count != 0)

                    dic.Add(First.Value.Key,
#if WinCopies2
                        InnerList
#else
                        this
#endif
                        .RemoveAndGetFirstValue().Value.Value);

            else

                foreach (KeyValuePair<TKey, TValue> keyValuePair in
#if WinCopies2
                InnerList
#else
                this
#endif
                )

                    dic.Add(keyValuePair.Key, keyValuePair.Value);
        }
    }
}
#endif
