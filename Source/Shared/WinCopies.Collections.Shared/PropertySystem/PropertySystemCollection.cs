﻿/* Copyright © Pierre Sprimont, 2021
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

#if WinCopies3 && CS7
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Linq;

using static WinCopies.UtilHelpers;
using static WinCopies.ThrowHelper;

namespace WinCopies.PropertySystem
{
    public interface IPropertySystemCollection<TPropertyId, TPropertyGroup> : System.Collections.Generic.IReadOnlyCollection<IProperty>, IReadOnlyList<IProperty>, System.Collections.Generic.IReadOnlyDictionary<TPropertyId, IProperty>, IEnumerableInfo<KeyValuePair<TPropertyId, IProperty>>
#if CS8
        , Collections.DotNetFix.Generic.IEnumerable<IProperty>, Collections.DotNetFix.Generic.IEnumerable<KeyValuePair<TPropertyId, IProperty>>
#endif
        where TPropertyId : IPropertyId<TPropertyGroup>
    {
        // Left empty.
    }

    public abstract class PropertySystemCollection<TPropertyId, TPropertyGroup> : IPropertySystemCollection<TPropertyId, TPropertyGroup> where TPropertyId : IPropertyId<TPropertyGroup>
    {
        public abstract IProperty this[int index] { get; }

        public abstract IProperty this[TPropertyId key] { get; }

        public abstract int Count { get; }

        public abstract IReadOnlyList<TPropertyId> Keys { get; }

        public abstract IReadOnlyList<IProperty> Values { get; }

        System.Collections.Generic.IEnumerable<TPropertyId> System.Collections.Generic.IReadOnlyDictionary<TPropertyId, IProperty>.Keys => Keys;

        System.Collections.Generic.IEnumerable<IProperty> System.Collections.Generic.IReadOnlyDictionary<TPropertyId, IProperty>.Values => Values;

        public bool ContainsKey(TPropertyId key) => Keys.Contains(key);

        public bool SupportsReversedEnumeration => true;

        public abstract IEnumeratorInfo2<KeyValuePair<TPropertyId, IProperty>> GetKeyValuePairEnumerator();

        public abstract IEnumeratorInfo2<KeyValuePair<TPropertyId, IProperty>> GetReversedKeyValuePairEnumerator();

        public ICountableEnumeratorInfo<IProperty> GetEnumerator() => new CountableEnumeratorInfo<IProperty>(GetKeyValuePairEnumerator().SelectConverter(GetValue), () => Count);

        public IEnumeratorInfo<IProperty> GetReversedEnumerator() => GetReversedKeyValuePairEnumerator().SelectConverter(GetValue);

        System.Collections.Generic.IEnumerator<KeyValuePair<TPropertyId, IProperty>> System.Collections.Generic.IEnumerable<KeyValuePair<TPropertyId, IProperty>>.GetEnumerator() => GetKeyValuePairEnumerator();

        System.Collections.Generic.IEnumerator<KeyValuePair<TPropertyId, IProperty>> Collections.Extensions.Generic.IEnumerable<KeyValuePair<TPropertyId, IProperty>>.GetReversedEnumerator() => GetReversedKeyValuePairEnumerator();

        System.Collections.Generic.IEnumerator<IProperty> System.Collections.Generic.IEnumerable<IProperty>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumeratorInfo<KeyValuePair<TPropertyId, IProperty>> Collections.Extensions.IEnumerable<IEnumeratorInfo<KeyValuePair<TPropertyId, IProperty>>>.GetReversedEnumerator() => GetReversedKeyValuePairEnumerator();
        IEnumerator Collections.Extensions.IEnumerable.GetReversedEnumerator() => GetReversedKeyValuePairEnumerator();
#if !CS8
        IEnumeratorInfo<KeyValuePair<TPropertyId, IProperty>> Collections.Enumeration.IEnumerable<IEnumeratorInfo<KeyValuePair<TPropertyId, IProperty>>>.GetEnumerator() => GetKeyValuePairEnumerator();
#endif
        public bool TryGetValue(TPropertyId key,
#if CS8
            [MaybeNullWhen(false)]
#endif
        out IProperty value) => (value = new Enumerable<KeyValuePair<TPropertyId, IProperty>>(GetKeyValuePairEnumerator).FirstOrDefault(keyValuePair => key == null ? keyValuePair.Key == null : key.Equals(keyValuePair.Key)).Value) != null;

        public class Enumerator : EnumeratorInfo<KeyValuePair<TPropertyId, IProperty>>
        {
            public override bool? IsResetSupported => true;

            public Enumerator(in IReadOnlyList<KeyValuePair<TPropertyId, IProperty>> properties) : base(properties) { /* Left empty. */ }
        }

        public class ReversedEnumerator : Enumerator<KeyValuePair<TPropertyId, IProperty>>
        {
            private IReadOnlyList<KeyValuePair<TPropertyId, IProperty>> _properties;
            private int _index = -1;
            private Func<bool> _moveNext;

            public override bool? IsResetSupported => true;

            protected override KeyValuePair<TPropertyId, IProperty> CurrentOverride => _properties[_index];

            public ReversedEnumerator(in IReadOnlyList<KeyValuePair<TPropertyId, IProperty>> properties)
            {
                _properties = properties ?? throw GetArgumentNullException(nameof(properties));

                ResetMoveNext();
            }

            private void ResetMoveNext() => _moveNext = () =>
            {
                _index = _properties.Count - 1;

                if (_index < 0)

                    return false;

                _moveNext = () =>
                {
                    _index--;

                    return _index >= 0;
                };

                return true;
            };

            protected override bool MoveNextOverride() => _moveNext();

            protected override void ResetOverride2() => ResetMoveNext();

            protected override void ResetCurrent() => _index = -1;

            protected override void DisposeManaged()
            {
                base.DisposeManaged();

                _properties = null;
                _moveNext = null;
            }
        }
    }
}
#endif
