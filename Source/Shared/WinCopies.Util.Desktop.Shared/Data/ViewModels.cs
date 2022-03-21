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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Markup;

using static WinCopies.
#if WinCopies3
ThrowHelper;

using WinCopies.Collections.DotNetFix.Generic;
#else
Util.Util;

using WinCopies.Collections;
#endif

namespace WinCopies.Util.Data
{
    public abstract class ViewModelRoot : MarkupExtension, INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual
#if WinCopies3
            bool
#else
            void
#endif
           UpdateValue<T>(ref T value,
#if !WinCopies3
in
#endif
            T newValue, System.ComponentModel.PropertyChangedEventArgs propertyChangedEventArgs)
#if WinCopies3
        {
            T _value = value;

            return UtilHelpers
#else
            => Util
#endif
            .UpdateValue(ref value, newValue, () => OnPropertyChanged(propertyChangedEventArgs
#if WinCopies3
            , _value, newValue
#endif
            ));
#if WinCopies3
        }
#endif

        protected virtual
#if WinCopies3
            bool
#else
            void
#endif
           UpdateValue<T>(ref T value, in T newValue,
#if WinCopies3
            in
#endif
            string propertyName) =>
#if !WinCopies3
        Util.
#endif
            UpdateValue(ref value, newValue,
#if WinCopies3
                new System.ComponentModel.PropertyChangedEventArgs(propertyName)
#else
        () => OnPropertyChanged(propertyName)
#endif
                );

        protected virtual void OnPropertyChanged(in System.ComponentModel.PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected virtual void OnPropertyChanged(
#if WinCopies3
            in
#endif
            string propertyName) => OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(propertyName));

        protected virtual void OnPropertyChanged(
#if WinCopies3
            in
#endif
            System.ComponentModel.PropertyChangedEventArgs e
#if WinCopies3
            , in object oldValue, in object newValue
#endif
            ) => OnPropertyChanged(e);

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property for which to set a new value.</param>
        /// <param name="oldValue">The old value of the property. This parameter is ignored by default. You can override this method and use the <see cref="PropertyChangedEventArgs"/> if you want for the <see cref="PropertyChanged"/> event to notify for this value.</param>
        /// <param name="newValue">The new value of the property. This parameter is ignored by default. You can override this method and use the <see cref="PropertyChangedEventArgs"/> if you want for the <see cref="PropertyChanged"/> event to notify for this value.</param>
        protected virtual void OnPropertyChanged(
#if WinCopies3
            in
#endif
         string propertyName,
#if WinCopies3
            in
#endif
         object oldValue,
#if WinCopies3
            in
#endif
         object newValue) => OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(propertyName)
#if WinCopies3
             , oldValue, newValue
#endif
             );

        /// <summary>
        /// Returns the current instance of this class as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }

    /// <summary>
    /// Provides a base class for direct view models.
    /// </summary>
    public abstract class ViewModelBase :
#if WinCopies3
        ViewModelRoot
#else
        MarkupExtension, INotifyPropertyChanged
#endif
    {
#if WinCopies3
        /// <summary>
        /// Returns the current instance of this class as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

#if CS7
        /// <summary>
        /// Sets a value for a property. If succeeds, then call the <see cref="OnPropertyChanged(string, object, object)"/> method to raise the <see cref="PropertyChanged"/> event.
        /// </summary>// See the Remarks section.
        /// <param name="propertyName">The name of the property for which to set a new value</param>
        /// <param name="fieldName">The name of the field to store the new value. This has to be the field that is called by the property accessors (get and set).</param>
        /// <param name="newValue">The value to set in the property</param>
        /// <param name="declaringType">The declaring type of both the property and its associated field</param>
        /// <param name="performIntegrityCheck">Whether to throw when the property is not settable or declaring types of the property and the setter method do not correspond</param>
        // /// <remarks>To use this method, you need to work with the WinCopies Framework Property changed notification pattern. See the website of the WinCopies Framework for more details.</remarks>
        protected virtual
#if WinCopies3
            bool
#else
            void
#endif
            Update(string propertyName, string fieldName, object newValue, Type declaringType, bool performIntegrityCheck = true)
        {
            (bool propertyChanged, object oldValue) = performIntegrityCheck ? this.SetProperty(propertyName, fieldName, newValue, declaringType) : ((INotifyPropertyChanged)this).SetField(fieldName, newValue, declaringType);

            if (propertyChanged)

                OnPropertyChanged(propertyName, oldValue, newValue);

#if WinCopies3
            return propertyChanged;
#endif
        }

        protected virtual
#if WinCopies3
            bool
#else
            void
#endif
           Update<T>(string propertyName, string fieldName, object newValue, bool performIntegrityCheck = true) => Update(propertyName, fieldName, newValue, typeof(T), performIntegrityCheck);
#endif

#if CS7
        /// <summary>
        /// Sets a value for a property. If succeeds, then call the <see cref="OnPropertyChanged(string, object, object)"/> method to raise the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property for which to set a new value</param>
        /// <param name="newValue">The value to set in the property</param>
        /// <param name="declaringType">The declaring type of both the property and its associated field</param>
        /// <param name="performIntegrityCheck">Whether to throw when the property is not settable</param>
        protected virtual
#if WinCopies3
            bool
#else
            void
#endif
           Update(string propertyName, object newValue, Type declaringType, bool performIntegrityCheck = true)
        {
            (bool propertyChanged, object oldValue) = this.SetProperty(propertyName, newValue, declaringType);

            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

#if WinCopies3
            return propertyChanged;
#endif
        }

        protected virtual
#if WinCopies3
            bool
#else
            void
#endif
           Update<T>(string propertyName, object newValue, bool performIntegrityCheck = true) => Update(propertyName, newValue, typeof(T), performIntegrityCheck);
#endif
#else
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

#if CS7
        /// <summary>
        /// Sets a value for a property. If succeeds, then call the <see cref="OnPropertyChanged(string, object, object)"/> method to raise the <see cref="PropertyChanged"/> event.
        /// </summary>// See the Remarks section.
        /// <param name="propertyName">The name of the property for which to set a new value</param>
        /// <param name="fieldName">The name of the field to store the new value. This has to be the field that is called by the property accessors (get and set).</param>
        /// <param name="newValue">The value to set in the property</param>
        /// <param name="declaringType">The declaring type of both the property and its associated field</param>
        /// <param name="performIntegrityCheck">Whether to throw when the property is not settable or declaring types of the property and the setter method do not correspond</param>
        // /// <remarks>To use this method, you need to work with the WinCopies Framework Property changed notification pattern. See the website of the WinCopies Framework for more details.</remarks>
        protected virtual
#if WinCopies3
            bool
#else
            void
#endif
            Update(string propertyName, string fieldName, object newValue, Type declaringType, bool performIntegrityCheck = true)
        {
            (bool propertyChanged, object oldValue) = performIntegrityCheck ? this.SetProperty(propertyName, fieldName, newValue, declaringType) : ((INotifyPropertyChanged)this).SetField(fieldName, newValue, declaringType);

            if (propertyChanged)

                OnPropertyChanged(propertyName, oldValue, newValue);

#if WinCopies3
            return propertyChanged;
#endif
        }

        protected virtual
#if WinCopies3
            bool
#else
            void
#endif
           Update<T>(string propertyName, string fieldName, object newValue, bool performIntegrityCheck = true) => Update(propertyName, fieldName, newValue, typeof(T), performIntegrityCheck);
#endif

        protected virtual
#if WinCopies3
            bool
#else
            void
#endif
           UpdateValue<T>(ref T value,
#if !WinCopies3
in
#endif
            T newValue, System.ComponentModel.PropertyChangedEventArgs propertyChangedEventArgs)
#if WinCopies3
        {
            T _value = value;

            return UtilHelpers
#else
            => Util
#endif
            .UpdateValue(ref value, newValue, () => OnPropertyChanged(propertyChangedEventArgs
#if WinCopies3
            , _value, newValue
#endif
            ));
#if WinCopies3
        }
#endif

        protected virtual
#if WinCopies3
            bool
#else
            void
#endif
           UpdateValue<T>(ref T value, in T newValue,
#if WinCopies3
            in
#endif
            string propertyName) =>
#if !WinCopies3
        Util.
#endif
            UpdateValue(ref value, newValue,
#if WinCopies3
                new System.ComponentModel.PropertyChangedEventArgs(propertyName)
#else
        () => OnPropertyChanged(propertyName)
#endif
                );

        protected virtual void
#if WinCopies3
        OnPropertyChanged(in System.ComponentModel.PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);
#else
        OnPropertyChanged(string propertyName) => OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(propertyName));
#endif

        protected virtual void OnPropertyChanged(
#if WinCopies3
            in
#endif
            System.ComponentModel.PropertyChangedEventArgs e
#if WinCopies3
            , in object oldValue, in object newValue
#endif
            ) => OnPropertyChanged(e);

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property for which to set a new value.</param>
        /// <param name="oldValue">The old value of the property. This parameter is ignored by default. You can override this method and use the <see cref="PropertyChangedEventArgs"/> if you want for the <see cref="PropertyChanged"/> event to notify for this value.</param>
        /// <param name="newValue">The new value of the property. This parameter is ignored by default. You can override this method and use the <see cref="PropertyChangedEventArgs"/> if you want for the <see cref="PropertyChanged"/> event to notify for this value.</param>
        protected virtual void OnPropertyChanged(
#if WinCopies3
            in
#endif
         string propertyName,
#if WinCopies3
            in
#endif
         object oldValue,
#if WinCopies3
            in
#endif
         object newValue) => OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(propertyName)
#if WinCopies3
             , oldValue, newValue
#endif
             );

#if CS7
        /// <summary>
        /// Sets a value for a property. If succeeds, then call the <see cref="OnPropertyChanged(string, object, object)"/> method to raise the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property for which to set a new value</param>
        /// <param name="newValue">The value to set in the property</param>
        /// <param name="declaringType">The declaring type of both the property and its associated field</param>
        /// <param name="performIntegrityCheck">Whether to throw when the property is not settable</param>
        protected virtual
#if WinCopies3
            bool
#else
            void
#endif
           Update(string propertyName, object newValue, Type declaringType, bool performIntegrityCheck = true)
        {
            (bool propertyChanged, object oldValue) = this.SetProperty(propertyName, newValue, declaringType);

            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

#if WinCopies3
            return propertyChanged;
#endif
        }

        protected virtual
#if WinCopies3
            bool
#else
            void
#endif
           Update<T>(string propertyName, object newValue, bool performIntegrityCheck = true) => Update(propertyName, newValue, typeof(T), performIntegrityCheck);
#endif

        /// <summary>
        /// Returns the current instance of this class as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
#endif
    }

    public enum PropertyChangeScope
    {
        Model = 0,

        ViewModel = 1
    }

    public abstract class ViewModelAbstract :
#if WinCopies3
        ViewModelRoot
#else
        MarkupExtension, INotifyPropertyChanged
#endif

    {
        public object GetModelFromPropertyChangeScope(PropertyChangeScope propertyChangeScope)
#if CS8
            => propertyChangeScope switch
            {
                PropertyChangeScope.Model => Model,
                PropertyChangeScope.ViewModel => this,
                _ => throw new InvalidEnumArgumentException(nameof(propertyChangeScope), propertyChangeScope),
            };
#else
        {
            switch (propertyChangeScope)
            {
                case PropertyChangeScope.Model:

                    return Model;

                case PropertyChangeScope.ViewModel:

                    return this;

                default:

                    throw new InvalidEnumArgumentException(nameof(propertyChangeScope), propertyChangeScope);
            }
        }
#endif

#if !WinCopies3
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
#endif

        /// <summary>
        /// The model for this instance of view model.
        /// </summary>
        protected abstract object Model { get; }

#if CS7
        /// <summary>
        /// Sets a value for a property. If succeeds, then call the <see cref="OnPropertyChanged(string, object, object)"/> method to raise the <see cref="PropertyChanged"/> event.
        /// </summary>// See the Remarks section.
        /// <param name="propertyName">The name of the property for which to set a new value</param>
        /// <param name="fieldName">The name of the field to store the new value. This has to be the field that is called by the property accessors (get and set).</param>
        /// <param name="newValue">The value to set in the property</param>
        /// <param name="declaringType">The declaring type of both the property and its associated field</param>
        /// <param name="performIntegrityCheck">Whether to throw when the property is not settable or declaring types of the property and the setter method do not correspond</param>
        /// <param name="propertyChangeScope">Whether to reflect on the <see cref="Model"/> object or on the current view model. This value is set to <see cref="PropertyChangeScope.ViewModel"/> by default for this method.</param>
        // /// <remarks>To use this method, you need to work with the WinCopies Framework Property changed notification pattern. See the website of the WinCopies Framework for more details.</remarks>
        protected virtual
#if WinCopies3
            bool
#else
            void
#endif
            Update(string propertyName, string fieldName, object newValue, Type declaringType, bool performIntegrityCheck = true, PropertyChangeScope propertyChangeScope = PropertyChangeScope.ViewModel)
        {
            (bool propertyChanged, object oldValue) = performIntegrityCheck ? GetModelFromPropertyChangeScope(propertyChangeScope).SetProperty(propertyName, fieldName, newValue, declaringType) : ((INotifyPropertyChanged)this).SetField(fieldName, newValue, declaringType);

            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

#if WinCopies3
            return propertyChanged;
#endif
        }

        protected virtual
#if WinCopies3
            bool
#else
            void
#endif
             Update<T>(string propertyName, string fieldName, object newValue, bool performIntegrityCheck = true, PropertyChangeScope propertyChangeScope = PropertyChangeScope.ViewModel) => Update(propertyName, fieldName, newValue, typeof(T), performIntegrityCheck, propertyChangeScope);

        /// <summary>
        /// Sets a value for a property. If succeeds, then call the <see cref="OnPropertyChanged(string, object, object)"/> method to raise the <see cref="PropertyChanged"/> event.
        /// </summary>// See the Remarks section.
        /// <param name="propertyName">The name of the property for which to set a new value</param>
        /// <param name="newValue">The value to set in the property</param>
        /// <param name="declaringType">The declaring type of both the property and its associated field</param>
        /// <param name="propertyChangeScope">Whether to reflect on the <see cref="Model"/> object or on the current view model. This value is set to <see cref="PropertyChangeScope.Model"/> by default for this method.</param>
        // /// <remarks>To use this method, you need to work with the WinCopies Framework Property changed notification pattern. See the website of the WinCopies Framework for more details.</remarks>
        protected virtual
#if WinCopies3
            bool
#else
            void
#endif
             Update(string propertyName, object newValue, Type declaringType, PropertyChangeScope propertyChangeScope = PropertyChangeScope.Model)
        {
            (bool propertyChanged, object oldValue) = GetModelFromPropertyChangeScope(propertyChangeScope).SetProperty(propertyName, newValue, declaringType);

            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

#if WinCopies3
            return propertyChanged;
#endif
        }

        protected virtual
#if WinCopies3
            bool
#else
            void
#endif
             Update<T>(string propertyName, object newValue, PropertyChangeScope propertyChangeScope = PropertyChangeScope.Model) => Update(propertyName, newValue, typeof(T), propertyChangeScope);
#endif

#if !WinCopies3

        protected virtual void OnPropertyChanged(string propertyName) => OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property for which to set a new value</param>
        /// <param name="oldValue">The old value of the property. This parameter is ignored by default. You can override this method and use the <see cref="PropertyChangedEventArgs"/> if you want for the <see cref="PropertyChanged"/> event to notify for this value.</param>
        /// <param name="newValue">The new value of the property. This parameter is ignored by default. You can override this method and use the <see cref="PropertyChangedEventArgs"/> if you want for the <see cref="PropertyChanged"/> event to notify for this value.</param>
        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));

        protected virtual void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);
#endif

#if CS7
        /// <summary>
        /// Sets a value for a property. If succeeds, then call the <see cref="OnPropertyChanged(string, object, object)"/> method to raise the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property for which to set a new value</param>
        /// <param name="newValue">The value to set in the property</param>
        /// <param name="declaringType">The declaring type of both the property and its associated field</param>
        /// <param name="performIntegrityCheck">Whether to throw when the property is not settable</param>
        protected virtual
#if WinCopies3
            bool
#else
            void
#endif
             UpdateAutoProperty(string propertyName, object newValue, Type declaringType, bool performIntegrityCheck = true)
        {
            (bool propertyChanged, object oldValue) = this.SetProperty(propertyName, newValue, declaringType, performIntegrityCheck);

            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

#if WinCopies3
            return propertyChanged;
#endif
        }

        protected virtual
#if WinCopies3
            bool
#else
            void
#endif
             UpdateAutoProperty<T>(string propertyName, object newValue, bool performIntegrityCheck = true) => UpdateAutoProperty(propertyName, newValue, typeof(T), performIntegrityCheck);
#endif

#if !WinCopies3

        /// <summary>
        /// Returns the current instance of this class as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
#endif
    }

    /// <summary>
    /// Provides a base class for view models.
    /// </summary>
    public abstract class ViewModel : ViewModelAbstract
    {
        protected
#if WinCopies3
            sealed
#endif
            override object Model
        { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        /// <param name="model">The model to use for this instance of view model.</param>
        protected ViewModel(object model) => Model = model;
    }

    /// <summary>
    /// Provides a base class for view models.
    /// </summary>
    public abstract class ViewModel<T> : ViewModelAbstract
    {
        protected
#if WinCopies3
            sealed
#endif
            override object Model => ModelGeneric;

        /// <summary>
        /// The model for this instance of view model.
        /// </summary>
        protected T ModelGeneric { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        /// <param name="model">The model to use for this instance of view model.</param>
        protected ViewModel(T model) => ModelGeneric = model;
    }

    /// <summary>
    /// Provides a base class for view models.
    /// </summary>
    public class CollectionViewModel<T> :
#if WinCopies3
        ViewModelRoot
#else
        MarkupExtension
#endif
#if CS7
, IObservableCollection<T>
#endif
    {
        private readonly CountMonitor _monitor = new
#if !CS9
            CountMonitor
#endif
            ();

        #region Properties
        protected Collection<T> Collection { get; }

        /// <summary>
        /// Gets the number of elements actually contained in the <see cref="CollectionViewModel{T}"/>.
        /// </summary>
        public int Count => Collection.Count;

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero. -or- <paramref name="index"/> is equal to or greater than <see cref="Count"/>.</exception>
        public T this[int index] { get => Collection[index]; set => SetItem(index, value); }

        #region Interface implementations
#if CS7
        object IList.this[int index] { get => this[index]; set => this[index] = (T)value; }

        bool System.Collections.Generic.ICollection<T>.IsReadOnly => ((System.Collections.Generic.ICollection<T>)Collection).IsReadOnly;

        bool ICollection.IsSynchronized => ((ICollection)Collection).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)Collection).SyncRoot;

        bool IList.IsReadOnly => ((IList)Collection).IsReadOnly;

        bool IList.IsFixedSize => ((IList)Collection).IsFixedSize;
#endif
        #endregion
        #endregion

        #region Events
#if !WinCopies3
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        protected event PropertyChangedEventHandler PropertyChanged;
#endif

        /// <summary>
        /// Occurs when an item is added, removed, changed, moved, or the entire list is refreshed.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

#if CS7
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged { add => PropertyChanged += value; remove => PropertyChanged -= value; }
#endif
#endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionViewModel{T}"/> class.
        /// </summary>
        public CollectionViewModel(Collection<T> collection) => Collection = collection ?? throw GetArgumentNullException(nameof(collection));

        #region Methods
#region Protected Methods
#region 'On-' Methods
#if !WinCopies3
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
#endif

        protected void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged == null)

                return;

            using (BlockReentrancy())

                CollectionChanged?.Invoke(this, e);
        }

        protected void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index) => OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(action, item, index));

        protected void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index) => OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(action, oldItem, newItem, index));

        protected void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int newIndex, int oldIndex) => OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(action, item, newIndex, oldIndex));

        protected void OnCollectionReset() => OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
#endregion

#region Reentrancy Checks
        /// <summary>
        /// Checks for reentrant attempts to change this collection.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">If there was a call to <see cref="BlockReentrancy"/> of which the <see cref="System.IDisposable"/> return value has not yet been disposed of. Typically, this means when there are additional attempts to change this collection during a <see cref="CollectionChanged"/> event. However, it depends on when derived classes choose to call <see cref="BlockReentrancy"/>.</exception>
        protected void CheckReentrancy()
        {
            if (CollectionChanged == null)

                return;

            if (_monitor.IsBusy && CollectionChanged.GetInvocationList().Length != 0)

                throw new InvalidOperationException("Reentrancy not allowed.");
        }

        /// <summary>
        /// Disallows reentrant attempts to change this collection.
        /// </summary>
        /// <returns>An <see cref="System.IDisposable"/> object that can be used to dispose of the object.</returns>
        protected System.IDisposable BlockReentrancy()
        {
            _monitor.Enter();

            return _monitor;
        }
#endregion

#region Collection Update Methods
        protected void AddOrInsert(int index, T item)
        {
            if (index == Collection.Count)

                Collection.Add(item);

            else

                Collection.Insert(index, item);
        }

        /// <summary>
        /// Inserts an element into the <see cref="CollectionViewModel{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero. -or- <paramref name="index"/> is greater than <see cref="Count"/>.</exception>
        protected void InsertItem(int index, T item)
        {
            CheckReentrancy();

            if (index > Collection.Count || index < 0)

                throw new IndexOutOfRangeException();

            AddOrInsert(index, item);

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(Binding.IndexerName);

            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index. The value can be null for reference types.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero. -or- <paramref name="index"/> is greater than <see cref="Count"/>.</exception>
        protected void SetItem(int index, T item)
        {
            CheckReentrancy();

            T oldItem = Collection[index];
            Collection[index] = item;

            OnPropertyChanged(Binding.IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, oldItem, item, index);
        }

        /// <summary>
        /// Moves the item at the specified index to a new location in the collection.
        /// </summary>
        /// <param name="oldIndex">The zero-based index specifying the location of the item to be moved.</param>
        /// <param name="newIndex">The zero-based index specifying the new location of the item.</param>
        protected virtual void MoveItem(int oldIndex, int newIndex)
        {
            CheckReentrancy();

            T item = Collection[oldIndex];

            Collection.RemoveAt(oldIndex);
            AddOrInsert(newIndex, item);

            OnPropertyChanged(Binding.IndexerName);

            OnCollectionChanged(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex);
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="CollectionViewModel{T}"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero. -or- <paramref name="index"/> is equal to or greater than <see cref="Count"/>.</exception>
        protected virtual void RemoveItem(int index)
        {
            CheckReentrancy();

            T oldItem = Collection[index];

            Collection.RemoveAt(index);

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(Binding.IndexerName);

            OnCollectionChanged(NotifyCollectionChangedAction.Remove, oldItem, index);
        }

        /// <summary>
        /// Removes all elements from the <see cref="CollectionViewModel{T}"/>.
        /// </summary>
        protected virtual void ClearItems()
        {
            CheckReentrancy();

            Collection.Clear();

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(Binding.IndexerName);

            OnCollectionReset();
        }
        #endregion
#endregion

#region Public Methods
#if !WinCopies3
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
#endif

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire <see cref="CollectionViewModel{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="CollectionViewModel{T}"/>. The value can be null for reference types.</param>
        /// <returns>The zero-based index of the first occurrence of <paramref name="item"/> within the entire <see cref="CollectionViewModel{T}"/>, if found; otherwise, -1.</returns>
        public int IndexOf(T item) => Collection.IndexOf(item);

        /// <summary>
        /// Determines whether an element is in the <see cref="CollectionViewModel{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="CollectionViewModel{T}"/>. The value can be null for reference types.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is found in the <see cref="CollectionViewModel{T}"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(T item) => Collection.Contains(item);

        /// <summary>
        /// Returns an enumerator that iterates through the System.Collections.ObjectModel.Collection`1.
        /// </summary>
        /// <returns>An <see cref="System.Collections.Generic.IEnumerator{T}"/> for the <see cref="CollectionViewModel{T}"/>.</returns>
        public System.Collections.Generic.IEnumerator<T> GetEnumerator() => Collection.GetEnumerator();

        /// <summary>
        /// Copies the entire <see cref="CollectionViewModel{T}"/> to a compatible one-dimensional <see cref="Array"/>, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="CollectionViewModel{T}"/>. The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.</exception>
        /// <exception cref="System.ArgumentException">The number of elements in the source <see cref="CollectionViewModel{T}"/> is greater than the available space from <paramref name="index"/> to the end of the destination array.</exception>
        public void CopyTo(T[] array, int index) => Collection.CopyTo(array, index);

#if !WinCopies3
        private static void ThrowOnInvalidCopyToArrayParameters(in IEnumerable enumerable, in Array array)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(array, nameof(array));
        }

        private static void CopyTo(IEnumerable enumerable, in Array array, in int arrayIndex, in int count)
        {
            ThrowOnInvalidCopyToArrayParameters(enumerable, array);
            ThrowOnInvalidCopyToArrayOperation(array, arrayIndex, count, nameof(array), nameof(arrayIndex));

            int i = -1;

            foreach (object value in enumerable)

                array.SetValue(value, ++i);
        }
#endif

#if CS7
        bool IList.Contains(object value) => value is T _value && Contains(_value);

        int IList.IndexOf(object value) => value is T _value ? IndexOf(_value) : -1;

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection.CopyTo(Array array, int index) =>
#if WinCopies3
            WinCopies.Collections.EnumerableExtensions.
#endif
            CopyTo(this, array, index, Count);

        int IList.Add(object value)
        {
            if (value is T _value)
            {
                Add(_value);

                return Count - 1;
            }

            return -1;
        }

        void IList.Remove(object value) => Remove((T)value);

        void IList.Insert(int index, object value) => Insert(index, (T)value);
#endif

        /// <summary>
        /// Adds an object to the end of the <see cref="CollectionViewModel{T}"/>.
        /// </summary>
        /// <param name="item">The object to be added to the end of the <see cref="CollectionViewModel{T}"/>. The value can be null for reference types.</param>
        public void Add(T item) => InsertItem(Count, item);

        /// <summary>
        /// Inserts an element into the <see cref="CollectionViewModel{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero. -or- <paramref name="index"/> is greater than <see cref="Count"/>.</exception>
        public void Insert(int index, T item) => InsertItem(index, item);

        /// <summary>
        /// Moves the item at the specified index to a new location in the collection.
        /// </summary>
        /// <param name="oldIndex">The zero-based index specifying the location of the item to be moved.</param>
        /// <param name="newIndex">The zero-based index specifying the new location of the item.</param>
        public void Move(int oldIndex, int newIndex) => MoveItem(oldIndex, newIndex);

        /// <summary>
        /// Removes the element at the specified index of the <see cref="CollectionViewModel{T}"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero. -or- <paramref name="index"/> is equal to or greater than <see cref="Count"/>.</exception>
        public void RemoveAt(int index) => RemoveItem(index);

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="CollectionViewModel{T}"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="CollectionViewModel{T}"/>. The value can be null for reference types.</param>
        /// <returns><see langword="true"/> if item is successfully removed; otherwise, <see langword="false"/>. This method also returns <see langword="false"/> if <paramref name="item"/> was not found in the <see cref="CollectionViewModel{T}"/>.</returns>
        public bool Remove(T item)
        {
            int index = Collection.IndexOf(item);

            if (index > -1)
            {
                RemoveItem(index);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes all elements from the <see cref="CollectionViewModel{T}"/>.
        /// </summary>
        public void Clear() => ClearItems();
#endregion
#endregion
    }
}
