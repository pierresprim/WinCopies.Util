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

#if !WinCopies3 && CS7

using System;
using System.ComponentModel;

namespace WinCopies.Util.Data
{
    /// <summary>
    /// Provides an object that defines a value and notifies of the value change.
    /// </summary>
    [Obsolete("This interface has been replaced by the WinCopies.Util.IValueObject (WinCopies.IValueObject in WinCopies 3) interface and will be removed in later versions.")]
    public interface IValueObject : WinCopies.Util.IValueObject, INotifyPropertyChanged
    {
        // Left empty.
    }

    /// <summary>
    /// Provides an object that defines a value and notifies of the value change.
    /// </summary>
    [Obsolete("This interface has been replaced by the WinCopies.Util.IValueObject (WinCopies.IValueObject in WinCopies 3) interface and will be removed in later versions.")]
    public interface IValueObject<T> :
#if !WinCopies3
        WinCopies.Util.IValueObject<T>
#else
        WinCopies.IValueObject<T>
#endif
        , IValueObject
    {
        // Left empty.
    }

    /// <summary>
    /// Provides an object that defines a value and notifies of the value change.
    /// </summary>
    [Obsolete("This class has been replaced by the ViewModelBase class and will be removed in laters versions.")]
    public class ValueObject : IValueObject
    {
        /// <summary>
        /// Gets a value that indicates whether this object is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        private readonly object _value = null;

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        public object Value { get => _value; set => OnPropertyChanged(nameof(Value), nameof(_value), value, typeof(ValueObject)); }

        /// <summary>
        /// Determines whether this object is equal to a given object.
        /// </summary>
        /// <param name="obj">Object to compare to the current object.</param>
        /// <returns><see langword="true"/> if this object is equal to <paramref name="obj"/>, otherwise <see langword="false"/>.</returns>
        public bool Equals(
#if !WinCopies3
WinCopies.Util.IValueObject
#else
            WinCopies.IValueObject
#endif
        obj) => new ValueObjectEqualityComparer().Equals(this, obj);

        bool IEquatable<IReadOnlyValueObject>.Equals(IReadOnlyValueObject obj) => new ValueObjectEqualityComparer().Equals(this, obj);

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueObject"/> class.
        /// </summary>
        public ValueObject() { }

        /// <summary>
        /// Initilizes a new instance of the <see cref="ValueObject"/> class with the specified value.
        /// </summary>
        /// <param name="value"></param>
        public ValueObject(
#if WinCopies3
in
#endif
object value) => _value = value;

        /// <summary>
        /// Sets a value for a property. If succeeds, then call the <see cref="OnPropertyChanged(string, object, object)"/> method to raise the <see cref="PropertyChanged"/> event.
        /// </summary>// See the Remarks section.
        /// <param name="propertyName">The name of the property for which set a new value</param>
        /// <param name="fieldName">The name of the field to store the new value. This must the field that is called by the property accessors (get and set).</param>
        /// <param name="newValue">The value to set in the property</param>
        /// <param name="declaringType">The declaring type of both the property and its associated field</param>
        // /// <remarks>To use this method, you need to work with the WinCopies Framework Property changed notification pattern. See the website of the WinCopies Framework for more details.</remarks>
        protected virtual void OnPropertyChanged(string propertyName, string fieldName, object newValue, Type declaringType = null)
        {
            (bool propertyChanged, object oldValue) = ((INotifyPropertyChanged)this).SetProperty(propertyName, fieldName, newValue, declaringType);

            if (propertyChanged)

                OnPropertyChanged(propertyName, oldValue, newValue);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property for which set a new value</param>
        /// <param name="oldValue">The old value of the property. This parameter is ignored by default. You can override this method and use the <see cref="PropertyChangedEventArgs"/> if you want for the <see cref="PropertyChanged"/> event to notify for this value.</param>
        /// <param name="newValue">The new value of the property. This parameter is ignored by default. You can override this method and use the <see cref="PropertyChangedEventArgs"/> if you want for the <see cref="PropertyChanged"/> event to notify for this value.</param>
        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));

#region IDisposable Support
        private bool disposedValue = false;

        /// <summary>
        /// Removes the unmanaged resources and the managed resources if needed. If you override this method, you should call this implementation of this method in your override implementation to avoid unexpected results when using this object later.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to dispose managed resources, otherwise <see langword="false"/>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)

                return;

            if (Value is System.IDisposable _value)

                _value.Dispose();

            disposedValue = true;
        }

        ~ValueObject() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
#endregion
    }

    /// <summary>
    /// Provides an object that defines a generic value and notifies of the value change.
    /// </summary>
    /// <typeparam name="T">The type of the value of this object.</typeparam>
    [Obsolete("This class has been replaced by the ViewModelBase class and will be removed in laters versions.")]
    public class ValueObject<T> : IValueObject<T>
    {
        /// <summary>
        /// Gets a value that indicates whether this object is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        private readonly T _value = default;

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        public T Value { get => _value; set => OnPropertyChanged(nameof(Value), nameof(_value), value, typeof(ValueObject<T>)); }

        object
#if !WinCopies3
WinCopies.Util.IValueObject
#else
            WinCopies.IValueObject
#endif
            .Value
        { get => _value; set => Value = (T)value; }

        object IReadOnlyValueObject.Value => _value;

        /// <summary>
        /// Determines whether this object is equal to a given object.
        /// </summary>
        /// <param name="obj">Object to compare to the current object.</param>
        /// <returns><see langword="true"/> if this object is equal to <paramref name="obj"/>, otherwise <see langword="false"/>.</returns>
        public bool Equals(
#if !WinCopies3
            WinCopies.Util.IValueObject
#else
            WinCopies.IValueObject
#endif
            obj) => new ValueObjectEqualityComparer().Equals(this, obj);

        /// <summary>
        /// Determines whether this object is equal to a given object.
        /// </summary>
        /// <param name="obj">Object to compare to the current object.</param>
        /// <returns><see langword="true"/> if this object is equal to <paramref name="obj"/>, otherwise <see langword="false"/>.</returns>
        public bool Equals(
#if !WinCopies3
WinCopies.Util.IValueObject<T>
#else
            WinCopies.IValueObject<T>
#endif
            obj) => new ValueObjectEqualityComparer<T>().Equals(this, obj);

        bool IEquatable<IReadOnlyValueObject>.Equals(IReadOnlyValueObject obj) => new ValueObjectEqualityComparer().Equals(this, obj);

        bool IEquatable<IReadOnlyValueObject<T>>.Equals(IReadOnlyValueObject<T> obj) => new ValueObjectEqualityComparer<T>().Equals(this, obj);

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueObject"/> class.
        /// </summary>
        public ValueObject() { /* Left empty. */ }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueObject"/> class using a custom value.
        /// </summary>
        /// <param name="value">The value with which to initialize this object.</param>
        public ValueObject(
#if WinCopies3
in
#endif
T value) => _value = value;

        /// <summary>
        /// Sets a value for a property. If succeeds, then call the <see cref="OnPropertyChanged(string, object, object)"/> method to raise the <see cref="PropertyChanged"/> event.
        /// </summary>// See the Remarks section.
        /// <param name="propertyName">The name of the property for which set a new value</param>
        /// <param name="fieldName">The name of the field to store the new value. This must the field that is called by the property accessors (get and set).</param>
        /// <param name="newValue">The value to set in the property</param>
        /// <param name="declaringType">The declaring type of both the property and its associated field</param>
        // /// <remarks>To use this method, you need to work with the WinCopies Framework Property changed notification pattern. See the website of the WinCopies Framework for more details.</remarks>
        protected virtual void OnPropertyChanged(string propertyName, string fieldName, object newValue, Type declaringType = null)

        {
            (bool propertyChanged, object oldValue) = ((INotifyPropertyChanged)this).SetProperty(propertyName, fieldName, newValue, declaringType);

            if (propertyChanged)

                OnPropertyChanged(propertyName, oldValue, newValue);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property for which set a new value</param>
        /// <param name="oldValue">The old value of the property. This parameter is ignored by default. You can override this method and use the <see cref="PropertyChangedEventArgs"/> if you want for the <see cref="PropertyChanged"/> event to notify for this value.</param>
        /// <param name="newValue">The new value of the property. This parameter is ignored by default. You can override this method and use the <see cref="PropertyChangedEventArgs"/> if you want for the <see cref="PropertyChanged"/> event to notify for this value.</param>
        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));

#region IDisposable Support
        private bool disposedValue = false;

        /// <summary>
        /// Removes the unmanaged resources and the managed resources if needed. If you override this method, you should call this implementation of this method in your override implementation to avoid unexpected results when using this object laater.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to dispose managed resources, otherwise <see langword="false"/>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)

                return;

            if (Value is System.IDisposable _value)

                _value.Dispose();

            disposedValue = true;
        }

        ~ValueObject() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
#endregion
    }
}

#endif
