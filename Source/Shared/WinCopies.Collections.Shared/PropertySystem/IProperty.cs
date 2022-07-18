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

#if WinCopies3
using System;

namespace WinCopies.PropertySystem
{
    public interface IPropertyId<T> : IEquatable<IPropertyId<T>>
    {
        string Name { get; }

        T PropertyGroup { get; }
    }

    public interface IReadOnlyProperty
    {
        bool IsEnabled { get; }

        string Name { get; }

        string DisplayName { get; }

        string Description { get; }

        string EditInvitation { get; }

        object PropertyGroup { get; }

        object Value { get; }

        Type Type { get; }

        // string GetDisplayGroupName();
    }

    public interface IProperty : IReadOnlyProperty
    {
        bool IsReadOnly { get; }
    }

    public interface IReadOnlyProperty<T> : IReadOnlyProperty
    {
        new T PropertyGroup { get; }
    }

    public interface IProperty<T> : IReadOnlyProperty<T>, IProperty
    {
        new T PropertyGroup { get; }
    }

    public struct PropertyInfo<T>
    {
        public string Name { get; }

        public string DisplayName { get; }

        public string Description { get; }

        public T PropertyGroup { get; }

        public object Value { get; }

        public PropertyInfo(in string name, in string displayName, in string description, in T propertyGroup, in object value)
        {
            Name = name;
            DisplayName = displayName;
            Description = description;
            PropertyGroup = propertyGroup;
            Value = value;
        }

        public ReadOnlyProperty<T> GetProperty() => new
#if !CS9
            ReadOnlyProperty<T>
#endif
            (this);
    }

    public struct ReadOnlyProperty<T> : IProperty<T>
    {
        private readonly PropertyInfo<T> _propertyInfo;

        public bool IsReadOnly => true;

        public bool IsEnabled => true;

        public string Name => _propertyInfo.Name;

        public string DisplayName => _propertyInfo.DisplayName;

        public string Description => _propertyInfo.Description;

        public string EditInvitation => null;

        public T PropertyGroup => _propertyInfo.PropertyGroup;

        object IReadOnlyProperty.PropertyGroup => PropertyGroup;

        public object Value => _propertyInfo.Value;

        public Type Type => Value?.GetType();

        public ReadOnlyProperty(in PropertyInfo<T> propertyInfo) => _propertyInfo = propertyInfo;
    }
}
#endif
