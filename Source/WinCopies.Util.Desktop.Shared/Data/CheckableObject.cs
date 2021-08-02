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

#if !WinCopies3

using System;

namespace WinCopies.Util.Data
{
    /// <summary>
    /// Provides an object that defines a value that can be checked and notifies of the checked status or value change. For example, this interface can be used in a view for items that can be selected.
    /// </summary>
    [Obsolete("This interface has been replaced by the ICheckBoxModel interface of the WinCopies.GUI.Models assembly, and will be removed in later versions.")]
    public interface ICheckableObject : IValueObject
    {
        /// <summary>
        /// Gets or sets a value that indicates whether this object is checked.
        /// </summary>
        bool IsChecked { get; set; }
    }

    /// <summary>
    /// Provides an object that defines a value that can be checked and notifies of the checked status or value change. For example, this interface can be used in a view for items that can be selected.
    /// </summary>
    [Obsolete("This interface has been replaced by the ICheckBoxModel interface of the WinCopies.GUI.Models assembly, and will be removed in later versions.")]
    public interface ICheckableObject<T> : ICheckableObject, IValueObject<T> { }

    public abstract class CheckableObjectBase : ViewModelBase, System.IDisposable
    {
        private bool _isChecked = false;

        public bool IsReadOnly => false;

        /// <summary>
        /// Gets or sets a value that indicates whether the object is checked.
        /// </summary>
        public bool IsChecked { get => _isChecked; set => UpdateValue(ref _isChecked, value, nameof(IsChecked)); }

        public CheckableObjectBase() { }

        public CheckableObjectBase(in bool isChecked) => _isChecked = isChecked;

        #region IDisposable Support
        public bool IsDisposed { get; private protected set; }

        protected abstract void Dispose(bool disposing);

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        ~CheckableObjectBase() => Dispose(false);
        #endregion
    }

    /// <summary>
    /// Provides an object that defines a value that can be checked and notifies of the checked status or value change. For example, this class can be used in a view for items that can be selected.
    /// </summary>
    [Obsolete("This class has been replaced by the CheckBoxModel class of the WinCopies.GUI.Models assembly, and will be removed in later versions.")]
    public class CheckableObject : CheckableObjectBase, ICheckableObject
    {
        private object _value;

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        public object Value { get => _value; set => UpdateValue(ref _value, value, nameof(Value)); }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableObject"/> class.
        /// </summary>
        public CheckableObject() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableObject"/> class using custom values.
        /// </summary>
        /// <param name="isChecked">A value that indicates if this object is checked by default.</param>
        /// <param name="value">The value of the object.</param>
        public CheckableObject(bool isChecked, object value) : base(isChecked) => _value = value;

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
        public bool Equals(IReadOnlyValueObject obj) => new ValueObjectEqualityComparer().Equals(this, obj);

        /// <summary>
        /// Removes the unmanaged resources and the managed resources if needed. If you override this method, you should call this implementation of this method in your override implementation to avoid unexpected results when using this object laater.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to dispose managed resources, otherwise <see langword="false"/>.</param>
        protected override void Dispose(bool disposing)
        {
            if (IsDisposed)

                return;

            if (Value is System.IDisposable _value)

                _value.Dispose();

            IsDisposed = true;
        }
    }

    /// <summary>
    /// Provides an object that defines a generic value that can be checked and notifies of the checked status or value change. For example, this class can be used in a view for items that can be selected.
    /// </summary>
    /// <typeparam name="T">The type of the value of this object.</typeparam>
    [Obsolete("This class has been replaced by the CheckBoxModel class of the WinCopies.GUI.Models assembly, and will be removed in later versions.")]
    public class CheckableObject<T> : CheckableObject, ICheckableObject<T>
    {
        private T _value;

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        public T Value { get => _value; set => UpdateValue(ref _value, value, nameof(Value)); }

        object IReadOnlyValueObject.Value => _value;

        object
#if !WinCopies3
            WinCopies.Util.IValueObject
#else
            WinCopies.IValueObject
#endif
            .Value
        { get => _value; set => Value = value is T _value ? _value : throw new ArgumentException("Invalid type.", nameof(value)); }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableObject{T}"/> class.
        /// </summary>
        public CheckableObject() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableObject{T}"/> class using custom values.
        /// </summary>
        /// <param name="isChecked">A value that indicates if this object is checked by default.</param>
        /// <param name="value">The value of the object.</param>
        public CheckableObject(bool isChecked, T value) : base(isChecked, value) => _value = value;

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
        public bool Equals(IReadOnlyValueObject obj) => new ValueObjectEqualityComparer().Equals(this, obj);

        /// <summary>
        /// Determines whether this object is equal to a given object.
        /// </summary>
        /// <param name="obj">Object to compare to the current object.</param>
        /// <returns><see langword="true"/> if this object is equal to <paramref name="obj"/>, otherwise <see langword="false"/>.</returns>
        public bool Equals(IReadOnlyValueObject<T> obj) => new ValueObjectEqualityComparer().Equals(this, obj);

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

        /// <summary>
        /// Removes the unmanaged resources and the managed resources if needed. If you override this method, you should call this implementation of this method in your override implementation to avoid unexpected results when using this object laater.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to dispose managed resources, otherwise <see langword="false"/>.</param>
        protected override void Dispose(bool disposing)
        {
            if (IsDisposed)

                return;

            if (Value is System.IDisposable _value)

                _value.Dispose();

            IsDisposed = true;
        }

        //private void SetProperty(string propertyName, string fieldName, object newValue)

        //{

        //    //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
        //    //             BindingFlags.Static | BindingFlags.Instance |
        //    //             BindingFlags.DeclaredOnly;
        //    //this.GetType().GetField(fieldName, flags).SetValue(this, newValue);

        //    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName, previousValue, newValue));

        //    BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
        //                 BindingFlags.Static | BindingFlags.Instance |
        //                 BindingFlags.DeclaredOnly;

        //    object previousValue = null;

        //    FieldInfo field = GetType().GetField(fieldName, flags);

        //    previousValue = field.GetValue(this);

        //    if (!newValue.Equals(previousValue))

        //    {

        //        field.SetValue(this, newValue);

        //        //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
        //        //             BindingFlags.Static | BindingFlags.Instance |
        //        //             BindingFlags.DeclaredOnly;
        //        //this.GetType().GetField(fieldName, flags).SetValue(this, newValue);

        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName, previousValue, newValue));

        //    } 

        //}
    }
}

#endif
