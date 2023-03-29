/* Copyright © Pierre Sprimont, 2019
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using static WinCopies.ThrowHelper;

namespace WinCopies
{
    public interface IReadOnlyValueObject : IEquatable<IReadOnlyValueObject>, System.IDisposable
    {
        /// <summary>
        /// Gets a value that indicates whether this object is read-only.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        object Value { get; }
    }

    public interface IReadOnlyValueObject<T> : IReadOnlyValueObject, IEquatable<IReadOnlyValueObject<T>>, System.IDisposable
    {
        new T Value { get; }
    }

    /// <summary>
    /// Represents a value container. See the <see cref="IValueObject{T}"/> for a generic version of this class.
    /// </summary>
    public interface IValueObject : IReadOnlyValueObject, DotNetFix.IDisposable
    {
        new object Value { get; set; }
    }

    /// <summary>
    /// Represents a value container. See the <see cref="IValueObject"/> for a non-generic version of this class.
    /// </summary>
    public interface IValueObject<T> : IReadOnlyValueObject<T>, IValueObject, System.IDisposable
    {
        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        new T Value { get; set; }
    }

    public interface INamedObjectBase
    {
        /// <summary>
        /// Gets or sets the name of this object.
        /// </summary>
        string Name { get; }
    }

    public interface INamedObjectBase2 : INamedObjectBase
    {
        /// <summary>
        /// Gets or sets the name of this object.
        /// </summary>
        new string Name { get; set; }
    }

    /// <summary>
    /// Provides an object that defines a value with an associated name and notifies of the name or value change.
    /// </summary>
    public interface INamedObject : INamedObjectBase2, IValueObject
    {
        // Left empty.
    }

    /// <summary>
    /// Provides an object that defines a value with an associated name and notifies of the name or value change.
    /// </summary>
    public interface INamedObject<T> : INamedObject, IValueObject<T> { /* Left empty. */ }

    public class NamedObject : INamedObjectBase
    {
        public string Name { get; }

        public NamedObject(in string name) => Name = name;
    }

    public class NamedObject2 : INamedObjectBase
    {
        public string Name { get; set; }

        public NamedObject2() { /* Left empty. */ }

        public NamedObject2(in string name) => Name = name;
    }

    /// <summary>
    /// Provides an object that defines a value that can be checked and notifies of the checked status or value change. For example, this interface can be used in a view for items that can be selected.
    /// </summary>
    public interface ICheckableObject
    {
        /// <summary>
        /// Gets or sets a value that indicates whether this object is checked.
        /// </summary>
        bool IsChecked { get; set; }
    }

    public interface ICheckableObject2
    {
        bool? IsChecked { get; set; }
    }

    public class CheckableObject : ICheckableObject
    {
        /// <inheritdoc/>
        public bool IsChecked { get; set; }

        public CheckableObject() { /* Left empty. */ }

        public CheckableObject(in bool isChecked) => IsChecked = isChecked;
    }

    public class CheckableObject2 : ICheckableObject2
    {
        /// <inheritdoc/>
        public bool? IsChecked { get; set; }

        public CheckableObject2() { /* Left empty. */ }

        public CheckableObject2(in bool? isChecked) => IsChecked = isChecked;
    }

    public interface ICheckableNamedObject : ICheckableObject, INamedObjectBase2 { }

    public interface ICheckableNamedObject2 : ICheckableObject2, INamedObjectBase2 { }

    public interface ICheckableEnumerable<
#if CS5
        out
#endif
        T> : ICheckableObject, IEnumerable<T>
    {
        // Left empty.
    }

    public interface ICheckableEnumerable2<
#if CS5
    out
#endif
    T> : ICheckableObject2, IEnumerable<T>
    {
        // Left empty.
    }

    public interface ICheckableNamedEnumerable<
#if CS5
        out
#endif
        T> : ICheckableNamedObject, ICheckableEnumerable<T>
    {
        // Left empty.
    }

    public interface ICheckableNamedEnumerable2<
#if CS5
        out
#endif
        T> : ICheckableNamedObject2, ICheckableEnumerable2<T>
    {
        // Left empty.
    }

    /// <summary>
    /// Provides an object that defines a value that can be checked and notifies of the checked status or value change. For example, this class can be used in a view for items that can be selected.
    /// </summary>
    public class CheckableNamedObject : CheckableObject, ICheckableNamedObject
    {
        /// <inheritdoc/>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableObject"/> class.
        /// </summary>
        public CheckableNamedObject() { /* Left empty. */ }
        public CheckableNamedObject(in bool isChecked) : base(isChecked) { /* Left empty. */ }
        public CheckableNamedObject(in string name) => Name = name;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableObject"/> class using custom values.
        /// </summary>
        /// <param name="isChecked">A value that indicates if this object is checked by default.</param>
        /// <param name="name">The name of the object.</param>
        public CheckableNamedObject(in bool isChecked, in string name) : base(isChecked) => Name = name;
    }

    /// <summary>
    /// Provides an object that defines a value that can be checked and notifies of the checked status or value change. For example, this class can be used in a view for items that can be selected.
    /// </summary>
    public class CheckableNamedObject2 : CheckableObject2, ICheckableNamedObject2
    {
        /// <inheritdoc/>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableObject"/> class.
        /// </summary>
        public CheckableNamedObject2() { /* Left empty. */ }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableObject"/> class using custom values.
        /// </summary>
        /// <param name="name">The name of the object.</param>
        public CheckableNamedObject2(in bool? isChecked, in string name) : base(isChecked) => Name = name;
    }

    public class CheckableNamedEnumerableBase<TCollection, TItems, TIsChecked> : IEnumerable, INamedObjectBase where TCollection : IEnumerable<TItems>
    {
        public string Name { get; set; }

        public TIsChecked IsChecked { get; set; }

        public TCollection
#if CS9
            ?
#endif
            Items
        { get; set; }

#if !CS8
        string INamedObjectBase.Name => Name;
#endif

        internal CheckableNamedEnumerableBase() { }

        public IEnumerator<TItems> GetEnumerator() => (Items
#if CS8
            ??
#else
            == null ?
#endif
            Enumerable.Empty<TItems>()
#if !CS8
            : Items
#endif
            ).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class CheckableNamedEnumerable<TCollection, TItems> : CheckableNamedEnumerableBase<TCollection, TItems, bool>, ICheckableNamedEnumerable<TItems> where TCollection : IEnumerable<TItems>
    {
        // Left empty.
    }

    public class CheckableNamedEnumerable<T> : CheckableNamedEnumerable<IEnumerable<T>, T>
    {
        // Left empty.
    }

    public class CheckableNamedEnumerable2<TCollection, TItems> : CheckableNamedEnumerableBase<TCollection, TItems, bool?>, ICheckableNamedEnumerable2<TItems> where TCollection : IEnumerable<TItems>
    {
        // Left empty.
    }

    public class CheckableNamedEnumerable2<T> : CheckableNamedEnumerable2<IEnumerable<T>, T>
    {
        // Left empty.
    }

    public sealed class PropertyValueObject : IValueObject, IEquatable<PropertyValueObject>
    {
        private PropertyInfo _property;
        private object _obj;

        public object
#if CS8
            ?
#endif
            Value
        {
            get => _property.GetValue(_obj
#if !CS5
            , null
#endif
            ); set => _property.SetValue(_obj, value
#if !CS5
            , null
#endif
            );
        }

        public bool IsReadOnly => !_property.CanWrite;

        public bool IsDisposed => _property == null;

        public PropertyValueObject(in PropertyInfo property, in object obj)
        {
            _property = property ?? throw GetArgumentNullException(nameof(property));

            _obj = obj ?? throw GetArgumentNullException(nameof(property));
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                _property = null;
                _obj = null;
            }
        }

        public bool Equals(PropertyValueObject other) => _property == other._property && _obj == other._obj;

        public override bool Equals(
#if CS8
            [NotNullWhen(true)]
#endif
        object
#if CS8
            ?
#endif
            obj) => obj is PropertyValueObject other && Equals(other);

        public override int GetHashCode() => _property.GetHashCode() ^ _obj.GetHashCode();

        bool IEquatable<IReadOnlyValueObject>.Equals(IReadOnlyValueObject other) => Equals(other);
    }

    public sealed class PropertyValueObject<T> : IValueObject<T>, IEquatable<PropertyValueObject<T>>
    {
        private PropertyValueObject _innerStruct;

        public T Value { get => (T)_innerStruct.Value; set => _innerStruct.Value = value; }

        public bool IsReadOnly => _innerStruct.IsReadOnly;

        public bool IsDisposed => _innerStruct == null;

        object IValueObject.Value { get => Value; set => Value = (T)value; }

        object IReadOnlyValueObject.Value => Value;

        public PropertyValueObject(in PropertyInfo property, in object obj) => _innerStruct = new PropertyValueObject(property, obj);

        public void Dispose()
        {
            if (!IsDisposed)
            {
                _innerStruct.Dispose();
                _innerStruct = null;
            }
        }

        public bool Equals(PropertyValueObject<T> other) => _innerStruct.Equals(other._innerStruct);

        public override bool Equals(
#if CS8
            [NotNullWhen(true)]
#endif
        object
#if CS8
            ?
#endif
            obj) => obj is PropertyValueObject<T> other && Equals(other);

        bool IEquatable<IReadOnlyValueObject<T>>.Equals(IReadOnlyValueObject<T> other) => Equals(other);

        bool IEquatable<IReadOnlyValueObject>.Equals(IReadOnlyValueObject other) => Equals(other);

        public override int GetHashCode() => _innerStruct.GetHashCode();
    }

    /// <summary>
    /// Represents a default comparer for <see cref="IReadOnlyValueObject"/>s.
    /// </summary>
    public sealed class ValueObjectEqualityComparer : IEqualityComparer<IReadOnlyValueObject>
    {
        /// <summary>
        /// Checks if two <see cref="IReadOnlyValueObject"/>s are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="x"/> and <paramref name="y"/> are equal, otherwise <see langword="false"/>.</returns>
        public bool Equals(IReadOnlyValueObject x, IReadOnlyValueObject y) => x is object && y is object ? EqualityComparer<object>.Default.Equals(x.Value, y.Value) : !(x is object || y is object);

        /// <summary>
        /// Returns the hash code for a given <see cref="IReadOnlyValueObject"/>. If <paramref name="obj"/> has a value, this function returns the hash code of <paramref name="obj"/>'s <see cref="IReadOnlyValueObject.Value"/>, otherwise this function returns the hash code of <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The <see cref="IReadOnlyValueObject"/> for which to return the hash code.</param>
        /// <returns>The hash code of <paramref name="obj"/>'s <see cref="IReadOnlyValueObject.Value"/> if <paramref name="obj"/> has a value, otherwise the <paramref name="obj"/>'s hash code.</returns>
        public int GetHashCode(IReadOnlyValueObject obj) => (obj.Value is null ? obj : obj.Value).GetHashCode();
    }

    /// <summary>
    /// Represents a default comparer for <see cref="IReadOnlyValueObject{T}"/>s.
    /// </summary>
    public class ValueObjectEqualityComparer<T> : IEqualityComparer<IReadOnlyValueObject<T>>
    {
        /// <summary>
        /// Checks if two <see cref="IReadOnlyValueObject{T}"/>s are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="x"/> and <paramref name="y"/> are equal, otherwise <see langword="false"/>.</returns>
        public bool Equals(IReadOnlyValueObject<T> x, IReadOnlyValueObject<T> y) => x is object && y is object ? EqualityComparer<T>.Default.Equals(x.Value, y.Value) : !(x is object || y is object);

        /// <summary>
        /// Returns the hash code for a given <see cref="IReadOnlyValueObject{T}"/>. If <paramref name="obj"/> has a value, this function returns the hash code of <paramref name="obj"/>'s <see cref="IReadOnlyValueObject{T}.Value"/>, otherwise this function returns the hash code of <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The <see cref="IReadOnlyValueObject{T}"/> for which to return the hash code.</param>
        /// <returns>The hash code of <paramref name="obj"/>'s <see cref="IReadOnlyValueObject{T}.Value"/> if <paramref name="obj"/> has a value, otherwise the <paramref name="obj"/>'s hash code.</returns>
        public int GetHashCode(IReadOnlyValueObject<T> obj) => obj.Value is object ? obj.Value.GetHashCode() : obj.GetHashCode();
    }

    [Serializable]
    public struct ValueObjectEnumerator<T> : IEnumerator<T>, IEnumerator
    {
        private IEnumerator<IReadOnlyValueObject<T>> _enumerator;

        public T Current { get; private set; }

        object IEnumerator.Current => Current;

        public ValueObjectEnumerator(IEnumerator<IReadOnlyValueObject<T>> enumerator)
        {
            _enumerator = enumerator;

            Current = default;
        }

        public void Dispose()
        {
            Reset();

            _enumerator = null;
        }

        public bool MoveNext()
        {
            if (_enumerator.MoveNext())
            {
                Current = _enumerator.Current.Value;

                return true;
            }

            else return false;
        }

        public void Reset()
        {
            _enumerator.Reset();

            Current = default;
        }
    }
}
