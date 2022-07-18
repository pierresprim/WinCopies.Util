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

namespace WinCopies.Util.Data
{
#if !WinCopies3
    /// <summary>
    /// Provides an object that defines a value with an associated name and notifies of the name or value change.
    /// </summary>
    public interface INamedObject : IValueObject
    {
        /// <summary>
        /// Gets or sets the name of this object.
        /// </summary>
        string Name { get; set; }
    }

    /// <summary>
    /// Provides an object that defines a value with an associated name and notifies of the name or value change.
    /// </summary>
    public interface INamedObject<T> : INamedObject, IValueObject<T> { }
#endif

    public abstract class NamedObjectBase : ViewModelBase,
#if WinCopies3
        WinCopies.DotNetFix
#else
        System
#endif
        .IDisposable
    {
        private string _name = null;

        /// <summary>
        /// Gets or sets the name of the object.
        /// </summary>
        public string Name { get => _name; set => UpdateValue(ref _name, value, nameof(Name)); }

        public bool IsReadOnly => false;

        public NamedObjectBase() { /* Left empty. */ }

        public NamedObjectBase(in string name) => _name = name;

        public override string ToString() => Name;

        #region IDisposable Support
        public bool IsDisposed
        {
            get; private
#if !WinCopies3
                protected
#endif
                set;
        }

        /// <summary>
        /// Removes the unmanaged resources and the managed resources if needed. If you override this method, you should call this implementation of this method in your override implementation to avoid unexpected results when using this object laater.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to dispose managed resources, otherwise <see langword="false"/>.</param>
        protected
#if !WinCopies3
            abstract
#endif
            void Dispose(bool disposing)
#if WinCopies3
        {
            DisposeOverride(disposing);

            IsDisposed = true;
        }

        protected abstract void DisposeOverride(in bool disposing)
#endif
            ;

        public void Dispose()
        {
#if WinCopies3
            if (IsDisposed)

                return;
#endif

            Dispose(true);

            GC.SuppressFinalize(this);
        }

        ~NamedObjectBase() => Dispose(false);
        #endregion
    }

    /// <summary>
    /// Provides an object that defines a value with an associated name and notifies of the name or value change.
    /// </summary>
    public class NamedObject : NamedObjectBase, INamedObject
    {
        private object _value;

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        public object Value { get => _value; set => UpdateValue(ref _value, value, nameof(Value)); }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedObject"/> class.
        /// </summary>
        public NamedObject() { /* Left empty. */ }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedObject"/> class using custom values.
        /// </summary>
        public NamedObject(
#if WinCopies3
in
#endif
            string name,
#if WinCopies3
in
#endif
            object value) : base(name) => _value = value;

        /// <summary>
        /// Determines whether this object is equal to a given object.
        /// </summary>
        /// <param name="obj">Object to compare to the current object.</param>
        /// <returns><see langword="true"/> if this object is equal to <paramref name="obj"/>, otherwise <see langword="false"/>.</returns>
        public bool Equals(WinCopies.
#if !WinCopies3
            Util.
#endif
            IValueObject obj) => new ValueObjectEqualityComparer().Equals(this, obj);

        /// <summary>
        /// Determines whether this object is equal to a given object.
        /// </summary>
        /// <param name="obj">Object to compare to the current object.</param>
        /// <returns><see langword="true"/> if this object is equal to <paramref name="obj"/>, otherwise <see langword="false"/>.</returns>
        public bool Equals(IReadOnlyValueObject obj) => new ValueObjectEqualityComparer().Equals(this, obj);

        protected override void
#if WinCopies3
            DisposeOverride
#else
            Dispose
#endif
            (
#if WinCopies3
in
#endif
            bool disposing)
        {
#if !WinCopies3
            if (IsDisposed)

                return;
#endif

            if (Value is System.IDisposable _value)

                _value.Dispose();

#if !WinCopies3
            IsDisposed = true;
#endif
        }
    }

    /// <summary>
    /// Provides an object that defines a generic value with an associated name and notifies of the name or value change.
    /// </summary>
    /// <typeparam name="T">The type of the value of this object.</typeparam>
    public class NamedObject<T> : NamedObjectBase, INamedObject<T>
    {
        private T _value;

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        public T Value { get => _value; set => UpdateValue(ref _value, value, nameof(Value)); }

        object IReadOnlyValueObject.Value => _value;

        object WinCopies.
#if !WinCopies3
            Util.
#endif
            IValueObject.Value
        { get => _value; set => Value = value is T _value ? _value : throw new ArgumentException("Invalid type.", nameof(value)); }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedObject{T}"/> class.
        /// </summary>
        public NamedObject() { /* Left empty. */ }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedObject{T}"/> class using custom values.
        /// </summary>
        public NamedObject(
#if WinCopies3
            in
#endif
            string name,
#if WinCopies3
            in
#endif
            T value) : base(name) => _value = value;

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
        public bool Equals(WinCopies
#if !WinCopies3
            .Util
#endif
            .IValueObject<T> obj) => new ValueObjectEqualityComparer<T>().Equals(this, obj);

        /// <summary>
        /// Determines whether this object is equal to a given object.
        /// </summary>
        /// <param name="obj">Object to compare to the current object.</param>
        /// <returns><see langword="true"/> if this object is equal to <paramref name="obj"/>, otherwise <see langword="false"/>.</returns>
        public bool Equals(IReadOnlyValueObject<T> obj) => new ValueObjectEqualityComparer().Equals(this, obj);

        /// <summary>
        /// Removes the unmanaged resources and the managed resources if needed. If you override this method, you should call this implementation of this method in your override implementation to avoid unexpected results when using this object laater.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to dispose managed resources, otherwise <see langword="false"/>.</param>
        protected override void
#if WinCopies3
            DisposeOverride
#else
            Dispose
#endif
            (
#if WinCopies3
in
#endif
           bool disposing)
        {
#if !WinCopies3
            if (IsDisposed)

                return;
#endif

            if (Value is System.IDisposable _value)

                _value.Dispose();

#if !WinCopies3
            IsDisposed = true;
#endif
        }
    }
}
