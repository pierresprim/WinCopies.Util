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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using static WinCopies
#if WinCopies3
    .ThrowHelper;
#else
    .Util.Util;
#endif

namespace WinCopies
#if !WinCopies3
    .Util
#endif
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
    public interface IValueObject : IReadOnlyValueObject,
#if WinCopies3
        WinCopies.DotNetFix
#else
        System
#endif
        .IDisposable
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

    public sealed class PropertyValueObject : IValueObject, IEquatable<PropertyValueObject>
    {
        private PropertyInfo _property;
        private object _obj;

        public object Value
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
        public int GetHashCode(IReadOnlyValueObject obj) => (obj.Value is object ? obj.Value : obj).GetHashCode();
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
    public struct ValueObjectEnumerator<T> : System.Collections.Generic.IEnumerator<T>, System.Collections.IEnumerator
    {
        private System.Collections.Generic.IEnumerator<IReadOnlyValueObject<T>> _enumerator;

        public T Current { get; private set; }

        object System.Collections.IEnumerator.Current => Current;

        public ValueObjectEnumerator(System.Collections.Generic.IEnumerator<IReadOnlyValueObject<T>> enumerator)
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
