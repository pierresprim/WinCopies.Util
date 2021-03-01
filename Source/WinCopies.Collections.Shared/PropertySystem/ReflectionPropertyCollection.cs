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

#if WinCopies3 && CS7

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using WinCopies.Collections.Abstraction.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Linq;

using static WinCopies.UtilHelpers;
using static WinCopies.ThrowHelper;

namespace WinCopies.PropertySystem
{
    public class ReflectionPropertyCollection : PropertySystemCollection<ReflectionPropertyId, object>
    {
        #region Fields
        private ArrayBuilder<KeyValuePair<ReflectionPropertyId, IProperty>> _reflectionProperties;
        private System.Collections.Generic.IReadOnlyList<KeyValuePair<ReflectionPropertyId, IProperty>> _properties;
        private ReadOnlyList<KeyValuePair<ReflectionPropertyId, IProperty>, ReflectionPropertyId> _keys;
        private ReadOnlyList<KeyValuePair<ReflectionPropertyId, IProperty>, IProperty> _values;
        #endregion

        #region Properties
        private System.Collections.Generic.IReadOnlyList<KeyValuePair<ReflectionPropertyId, IProperty>> _Properties
        {
            get
            {
                if (_properties == null)

                    PopulateDictionary();

                return _properties;
            }
        }

        public override IProperty this[int index] => _Properties[index].Value;

        public override IProperty this[ReflectionPropertyId key] => _Properties.First(keyValuePair => keyValuePair.Key == key).Value;

        public override int Count => _reflectionProperties == null ? _properties.Count : (int)_reflectionProperties.Count;

        public override System.Collections.Generic.IReadOnlyList<ReflectionPropertyId> Keys => _keys
#if CS8
            ??=
#else
            ?? (_keys =
#endif
            new ReadOnlyList<KeyValuePair<ReflectionPropertyId, IProperty>, ReflectionPropertyId>(_Properties, keyValuePair => keyValuePair.Key)
#if !CS8
            )
#endif
            ;

        public override System.Collections.Generic.IReadOnlyList<IProperty> Values => _values
#if CS8
            ??=
#else
            ?? (_values =
#endif
            new ReadOnlyList<KeyValuePair<ReflectionPropertyId, IProperty>, IProperty>(_Properties, keyValuePair => keyValuePair.Value)
#if !CS8
            )
#endif
            ;
        #endregion

        public ReflectionPropertyCollection(object obj) => _reflectionProperties = new ArrayBuilder<KeyValuePair<ReflectionPropertyId, IProperty>>((obj ?? throw GetArgumentNullException(nameof(obj))).GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).WhereSelect(property => property.GetMethod != null && IsNullOrEmpty(property.GetIndexParameters()), property => new KeyValuePair<ReflectionPropertyId, IProperty>(new ReflectionPropertyId(property), new ReflectionProperty(property, obj))));

        #region Methods
        private void PopulateDictionary()
        {
            _properties = _reflectionProperties.ToList().AsReadOnly();

            _reflectionProperties = null;
        }

        public override IEnumeratorInfo2<KeyValuePair<ReflectionPropertyId, IProperty>> GetKeyValuePairEnumerator() => new Enumerator(_Properties);

        public override IEnumeratorInfo2<KeyValuePair<ReflectionPropertyId, IProperty>> GetReversedKeyValuePairEnumerator() => new ReversedEnumerator(_Properties);
        #endregion
    }
}
#endif
