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

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using WinCopies.Linq;

using static WinCopies.UtilHelpers;

namespace WinCopies.PropertySystem
{

    public struct ReflectionPropertyId : IPropertyId<object>
    {
        private readonly PropertyInfo _property;

        public string Name => _property.Name;

        public object PropertyGroup => null;

        public ReflectionPropertyId(in PropertyInfo property) => _property = property;

        public bool Equals(
#if CS8
            [AllowNull]
        #endif
        IPropertyId<object> other) => other is ReflectionPropertyId _other && Equals(_other);

        public bool Equals(
#if CS8
            [AllowNull]
#endif
        ReflectionPropertyId other) => other.PropertyGroup == PropertyGroup && other.Name == Name;

        public override bool Equals(object obj) => obj is ReflectionPropertyId _obj && Equals(_obj);

        public override int GetHashCode() => PropertyGroup.GetHashCode() ^ Name.GetHashCode(
#if CS8
             StringComparison.CurrentCulture
#endif
            );

        public override string ToString() => Name;

        public static bool operator ==(ReflectionPropertyId x, ReflectionPropertyId y) => x.Equals(y);

        public static bool operator !=(ReflectionPropertyId x, ReflectionPropertyId y) => !(x == y);
    }

    public struct ReflectionProperty : IProperty
    {
        private readonly PropertyInfo _property;
        private readonly object _obj;
        private PropertyDescriptionFindingAttribute _descriptionFindingAttribute;
        private NullableReference<PropertyDescriptionAttribute> _descriptionAttribute;
        private string _name;
        private string _description;

        private NullableReference<PropertyDescriptionAttribute> DescriptionAttribute => _descriptionAttribute
#if CS8
            ??=
#else
            ?? (_descriptionAttribute =
#endif
            new NullableReference<PropertyDescriptionAttribute>(_property.PropertyType.GetCustomAttributes().FirstOrDefault<PropertyDescriptionAttribute>())
#if !CS8
            )
#endif
            ;

        public
#if CS8
            readonly
#endif
            bool IsReadOnly => _property.SetMethod == null;

        public
#if CS8
            readonly
#endif
            bool IsEnabled => true;

        public
#if CS8
            readonly
#endif
            string Name => _property.Name;

        private string GetResource(string format)
        {
            PropertyInfo[] properties = _descriptionFindingAttribute?.PropertyDescriptionType?.GetProperties(BindingFlags.Public | BindingFlags.Static);

            if (IsNullOrEmpty(properties))

                return null;

            else
            {
                PropertyInfo _property = this._property;

                return (_property = properties.FirstOrDefault(property => property.Name == (format == null ? _property.Name : string.Format(format, _property.Name)) && property.CanRead && property.PropertyType == typeof(string))) == null
                    ? null
                    : (string)_property.GetValue(null);
            }
        }

        public string DisplayName
        {
            get
            {
                if (_name == null)
                {
                    _name = DescriptionAttribute?.Value?.FriendlyName ?? GetResource(_descriptionFindingAttribute?.NameFormat) ?? _property.Name;

                    if (_description != null)

                        _descriptionAttribute = null;
                }

                return _name;
            }
        }

        public string Description
        {
            get
            {
                if (_description == null)
                {
                    _description = DescriptionAttribute?.Value?.Description ?? GetResource(_descriptionFindingAttribute?.DescriptionFormat) ?? string.Empty;

                    if (_name != null)

                        _descriptionAttribute = null;
                }

                return _description;
            }
        }

        public
#if CS8
            readonly
#endif
          string EditInvitation => null;

        public
#if CS8
            readonly
#endif
          object PropertyGroup => null;

        public
#if CS8
            readonly
#endif
          object Value => _property.GetValue(_obj);

        public
#if CS8
            readonly
#endif
          Type Type => _property.PropertyType;

        public ReflectionProperty(in PropertyInfo property, in object obj)
        {
            _property = property;

            _obj = obj;

            _descriptionFindingAttribute = obj.GetType().GetCustomAttributes().FirstOrDefault<PropertyDescriptionFindingAttribute>();

            _descriptionAttribute = null;
            _name = null;
            _description = null;
        }

        public override string ToString() => Name;
    }
}

#endif
