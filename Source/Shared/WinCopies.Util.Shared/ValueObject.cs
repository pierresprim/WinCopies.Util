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

#if !WinCopies3
namespace WinCopies.Util
#else
namespace WinCopies
#endif
{
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
