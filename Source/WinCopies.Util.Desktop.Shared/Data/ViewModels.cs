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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace WinCopies.Util.Data
{
    /// <summary>
    /// Provides a base class for direct view models.
    /// </summary>
    public abstract class ViewModelBase : MarkupExtension, INotifyPropertyChanged
    {

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Sets a value for a property. If succeeds, then call the <see cref="OnPropertyChanged(string, object, object)"/> method to raise the <see cref="PropertyChanged"/> event.
        /// </summary>// See the Remarks section.
        /// <param name="propertyName">The name of the property for which to set a new value</param>
        /// <param name="fieldName">The name of the field to store the new value. This has to be the field that is called by the property accessors (get and set).</param>
        /// <param name="newValue">The value to set in the property</param>
        /// <param name="declaringType">The declaring type of both the property and its associated field</param>
        /// <param name="performIntegrityCheck">Whether to throw when the property is not settable or declaring types of the property and the setter method do not correspond</param>
        // /// <remarks>To use this method, you need to work with the WinCopies Framework Property changed notification pattern. See the website of the WinCopies Framework for more details.</remarks>
        protected virtual void Update(string propertyName, string fieldName, object newValue, Type declaringType, bool performIntegrityCheck = true)

        {

            (bool propertyChanged, object oldValue) = performIntegrityCheck ? this.SetProperty(propertyName, fieldName, newValue, declaringType) : ((INotifyPropertyChanged)this).SetField(fieldName, newValue, declaringType);

            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

        }

        protected virtual void OnPropertyChanged(string propertyName) => OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(propertyName));

        protected virtual void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property for which to set a new value</param>
        /// <param name="oldValue">The old value of the property. This parameter is ignored by default. You can override this method and use the <see cref="PropertyChangedEventArgs"/> if you want for the <see cref="PropertyChanged"/> event to notify for this value.</param>
        /// <param name="newValue">The new value of the property. This parameter is ignored by default. You can override this method and use the <see cref="PropertyChangedEventArgs"/> if you want for the <see cref="PropertyChanged"/> event to notify for this value.</param>
        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Sets a value for a property. If succeeds, then call the <see cref="OnPropertyChanged(string, object, object)"/> method to raise the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property for which to set a new value</param>
        /// <param name="newValue">The value to set in the property</param>
        /// <param name="declaringType">The declaring type of both the property and its associated field</param>
        /// <param name="performIntegrityCheck">Whether to throw when the property is not settable</param>
        protected virtual void Update(string propertyName, object newValue, Type declaringType, bool performIntegrityCheck = true)

        {

            (bool propertyChanged, object oldValue) = this.SetProperty(propertyName, newValue, declaringType);

            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

        }

        /// <summary>
        /// Returns the current instance of this class as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

    }

    public enum PropertyChangeScope
    {

        Model = 0,

        ViewModel = 1

    }

    public abstract class ViewModelAbstract : MarkupExtension, INotifyPropertyChanged

    {

#if CS7

        public object GetModelFromPropertyChangeScope(PropertyChangeScope propertyChangeScope)

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

#else

        public object GetModelFromPropertyChangeScope(PropertyChangeScope propertyChangeScope) => propertyChangeScope switch
        {
            PropertyChangeScope.Model => Model,
            PropertyChangeScope.ViewModel => this,
            _ => throw new InvalidEnumArgumentException(nameof(propertyChangeScope), propertyChangeScope),
        };

#endif

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The model for this instance of view model.
        /// </summary>
        protected abstract object Model { get; }

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
        protected virtual void Update(string propertyName, string fieldName, object newValue, Type declaringType, bool performIntegrityCheck = true, PropertyChangeScope propertyChangeScope = PropertyChangeScope.ViewModel)

        {

            (bool propertyChanged, object oldValue) = performIntegrityCheck ? GetModelFromPropertyChangeScope(propertyChangeScope). SetProperty(propertyName, fieldName, newValue, declaringType) : ((INotifyPropertyChanged)this).SetField(fieldName, newValue, declaringType);

            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

        }

        /// <summary>
        /// Sets a value for a property. If succeeds, then call the <see cref="OnPropertyChanged(string, object, object)"/> method to raise the <see cref="PropertyChanged"/> event.
        /// </summary>// See the Remarks section.
        /// <param name="propertyName">The name of the property for which to set a new value</param>
        /// <param name="newValue">The value to set in the property</param>
        /// <param name="declaringType">The declaring type of both the property and its associated field</param>
        /// <param name="propertyChangeScope">Whether to reflect on the <see cref="Model"/> object or on the current view model. This value is set to <see cref="PropertyChangeScope.Model"/> by default for this method.</param>
        // /// <remarks>To use this method, you need to work with the WinCopies Framework Property changed notification pattern. See the website of the WinCopies Framework for more details.</remarks>
        protected virtual void Update(string propertyName, object newValue, Type declaringType, PropertyChangeScope propertyChangeScope = PropertyChangeScope.Model)

        {

            (bool propertyChanged, object oldValue) = GetModelFromPropertyChangeScope(propertyChangeScope).SetProperty(propertyName, newValue, declaringType);

            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

        }

        protected virtual void OnPropertyChanged(string propertyName) => OnPropertyChanged(new System.ComponentModel. PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property for which to set a new value</param>
        /// <param name="oldValue">The old value of the property. This parameter is ignored by default. You can override this method and use the <see cref="PropertyChangedEventArgs"/> if you want for the <see cref="PropertyChanged"/> event to notify for this value.</param>
        /// <param name="newValue">The new value of the property. This parameter is ignored by default. You can override this method and use the <see cref="PropertyChangedEventArgs"/> if you want for the <see cref="PropertyChanged"/> event to notify for this value.</param>
        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));

        protected virtual void OnPropertyChanged( System.ComponentModel. PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        /// <summary>
        /// Sets a value for a property. If succeeds, then call the <see cref="OnPropertyChanged(string, object, object)"/> method to raise the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property for which to set a new value</param>
        /// <param name="newValue">The value to set in the property</param>
        /// <param name="declaringType">The declaring type of both the property and its associated field</param>
        /// <param name="performIntegrityCheck">Whether to throw when the property is not settable</param>
        protected virtual void UpdateAutoProperty(string propertyName, object newValue, Type declaringType, bool performIntegrityCheck = true)

        {

            (bool propertyChanged, object oldValue) = this.SetProperty(propertyName, newValue, declaringType, performIntegrityCheck);

            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

        }

        /// <summary>
        /// Returns the current instance of this class as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

    }

    /// <summary>
    /// Provides a base class for view models.
    /// </summary>
    public abstract class ViewModel : ViewModelAbstract
    {

        protected override object Model { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        /// <param name="model">The model to use for this instance of view model.</param>
        public ViewModel(object model) => Model = model;

    }

    /// <summary>
    /// Provides a base class for view models.
    /// </summary>
    public abstract class ViewModel<T> : ViewModelAbstract
    {

        protected override object Model => ModelGeneric ; 

        /// <summary>
        /// The model for this instance of view model.
        /// </summary>
        protected T ModelGeneric { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        /// <param name="model">The model to use for this instance of view model.</param>
        public ViewModel(T model) => ModelGeneric = model;

    }
}

 
