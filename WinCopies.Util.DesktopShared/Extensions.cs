﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WinCopies.Util
{
    public static class Extensions
    {

        public static void Execute(this ICommand command, object commandParameter, IInputElement commandTarget)

        {

            if (command is RoutedCommand)

                ((RoutedCommand)command).Execute(commandParameter, commandTarget);

            else

                command.Execute(commandParameter);

        }

        public static bool TryExecute(this ICommand command, object commandParameter, IInputElement commandTarget) => command is RoutedCommand
                ? ((RoutedCommand)command).TryExecute(commandParameter, commandTarget)
                : command.TryExecute(commandParameter);

        /// <summary>
        /// Check if the command can be executed, and executes it if available. See the remarks section.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="commandParameter">The parameter of your command.</param>
        /// <remarks>
        /// This method only evaluates the commands of the common <see cref="ICommand"/> type. To evaluate a command of the <see cref="RoutedCommand"/> type, consider using the <see cref="TryExecute(RoutedCommand, object, IInputElement)"/> method. If you are not sure of the type of your command, so consider using the <see cref="TryExecute(ICommand, object, IInputElement)"/> method.
        /// </remarks>
        public static bool TryExecute(this ICommand command, object commandParameter)

        {

            if (command != null && command.CanExecute(commandParameter))

            {

                command.Execute(commandParameter);

                return true;

            }

            return false;

        }

        public static bool TryExecute(this RoutedCommand command, object commandParameter, IInputElement commandTarget)

        {

            if (command.CanExecute(commandParameter, commandTarget))

            {

                // try
                // {

                command.Execute(commandParameter, commandTarget);

                // }
                // catch (InvalidOperationException ex)
                // {

                // Debug.WriteLine(ex.Message);

                // }

                return true;

            }

            return false;

        }

        public static bool CanExecute(this ICommand command, object commandParameter, IInputElement commandTarget) => command is RoutedCommand routedCommand
                ? routedCommand.CanExecute(commandParameter, commandTarget)
                : command.CanExecute(commandParameter);

        ///// <summary>
        ///// Checks if an object is a <see cref="FrameworkElement.Parent"/> or a <see cref="FrameworkElement.TemplatedParent"/> of an another object.
        ///// </summary>
        ///// <param name="source">The source object</param>
        ///// <param name="obj">The object to search in</param>
        ///// <returns><see langword="true"/> if 'obj' is a parent of the current object, otherwise <see langword="false"/>.</returns>
        //public static bool IsParent(this DependencyObject source, FrameworkElement obj)

        //{

        //    DependencyObject parent = obj.Parent ?? obj.TemplatedParent;

        //    while (parent != null && parent is FrameworkElement)

        //    {

        //        if (parent == source)

        //            return true;

        //        parent = ((FrameworkElement)parent).Parent ?? ((FrameworkElement)parent).TemplatedParent;

        //    }

        //    return false;

        //}

        /// <summary>
        /// Searches for the first parent of an object which is assignable from a given type.
        /// </summary>
        /// <typeparam name="T">The type to search</typeparam>
        /// <param name="source">The source object</param>
        /// <param name="typeEquality">Indicates whether to check for the exact type equality. <see langword="true"/> to only search for objects with same type than the given type, <see langword="false"/> to search for all objects of type for which the given type is assignable from.</param>
        /// <returns>The first object that was found, if any, otherwise null.</returns>
        public static T GetParent<T>(this DependencyObject source, bool typeEquality) where T : DependencyObject

        {

            Type type = typeof(T);

            //if (!typeof(DependencyObject).IsAssignableFrom(type))

            //    throw new InvalidOperationException($"The DependencyObject type must be assignable from the type parameter.");

            do

                source = (source is FrameworkElement frameworkElement ? frameworkElement.Parent ?? frameworkElement.TemplatedParent : null) ?? VisualTreeHelper.GetParent(source);

            while (source != null && (typeEquality ? source.GetType() != type : !type.IsAssignableFrom(source.GetType())));

            return (T)source;

        }

        internal static FieldInfo GetField(in string fieldName, in Type objectType, in BindingFlags bindingFlags) => objectType.GetField(fieldName, bindingFlags) ?? throw new ArgumentException(string.Format(Resources.ExceptionMessages.FieldOrPropertyNotFound, fieldName, objectType));

        internal static PropertyInfo GetProperty(in string propertyName, in Type objectType, in BindingFlags bindingFlags) => objectType.GetProperty(propertyName, bindingFlags) ?? throw new ArgumentException(string.Format(Resources.ExceptionMessages.FieldOrPropertyNotFound, propertyName, objectType));

        /// <summary>
        /// Sets a value to a property if the new value is different.
        /// </summary>
        /// <param name="obj">The object in which to set the property.</param>
        /// <param name="propertyName">The name of the given property.</param>
        /// <param name="fieldName">The field related to the property.</param>
        /// <param name="newValue">The value to set.</param>
        /// <param name="declaringType">The actual declaring type of the property.</param>
        /// <param name="throwIfBusy">Whether to throw if <paramref name="obj"/> is busy.</param>
        /// <param name="throwIfReadOnly">Whether to throw if the given property is read-only.</param>
        /// <param name="bindingFlags">The <see cref="BindingFlags"/> used to get the property.</param>
        /// <param name="paramName">The parameter from which the value was passed to this method.</param>
        /// <param name="setOnlyIfNotNull">Whether to set only if the given value is not null.</param>
        /// <param name="throwIfNull">Whether to throw if the given value is null.</param>
        /// <param name="validateValueCallback">The callback used to validate the given value. You can leave this parameter to null if you don't want to perform validation.</param>
        /// <param name="throwIfValidationFails">Whether to throw if the validation of <paramref name="validateValueCallback"/> fails.</param>
        /// <param name="valueChangedCallback">The callback used to perform actions after the property is set. You can leave this parameter to null if you don't want to perform actions after the property is set.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the setting succeeded and the old value of the given property (or <see langword="null"/> if the property does not contain any value nor reference).</returns>
        /// <exception cref="InvalidOperationException">The declaring types of the given property and field name doesn't correspond. OR The given property is read-only and <paramref name="throwIfReadOnly"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="ArgumentNullException">The new value is null and <paramref name="throwIfNull"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="Exception"><paramref name="validateValueCallback"/> failed and <paramref name="throwIfValidationFails"/> is set to <see langword="true"/>. This exception is the exception that was returned by <paramref name="validateValueCallback"/> if it was not null or an <see cref="ArgumentException"/> otherwise.</exception>
        public static (bool propertyChanged, object oldValue) SetBackgroundWorkerProperty(this System.ComponentModel.BackgroundWorker obj, string propertyName, string fieldName, object newValue, Type declaringType, bool throwIfBusy, bool throwIfReadOnly = true, BindingFlags bindingFlags = Util.DefaultBindingFlagsForPropertySet, string paramName = null, bool setOnlyIfNotNull = false, bool throwIfNull = false, FieldValidateValueCallback validateValueCallback = null, bool throwIfValidationFails = false, FieldValueChangedCallback valueChangedCallback = null) => obj.IsBusy
                ? throwIfBusy ? throw new InvalidOperationException(WinCopies.Util.Desktop.Resources.ExceptionMessages.BackgroundWorkerIsBusy) : (false, GetField(fieldName, declaringType, bindingFlags).GetValue(obj))
                : obj.SetProperty(propertyName, fieldName, newValue, declaringType, throwIfReadOnly, bindingFlags, paramName, setOnlyIfNotNull, throwIfNull, validateValueCallback, throwIfValidationFails, valueChangedCallback);

        /// <summary>
        /// Sets a value to a property if the new value is different.
        /// </summary>
        /// <param name="obj">The object in which to set the property.</param>
        /// <param name="propertyName">The name of the given property.</param>
        /// <param name="newValue">The value to set.</param>
        /// <param name="declaringType">The actual declaring type of the property.</param>
        /// <param name="throwIfBusy">Whether to throw if <paramref name="obj"/> is busy.</param>
        /// <param name="throwIfReadOnly">Whether to throw if the given property is read-only.</param>
        /// <param name="bindingFlags">The <see cref="BindingFlags"/> used to get the property.</param>
        /// <param name="paramName">The parameter from which the value was passed to this method.</param>
        /// <param name="setOnlyIfNotNull">Whether to set only if the given value is not null.</param>
        /// <param name="throwIfNull">Whether to throw if the given value is null.</param>
        /// <param name="validateValueCallback">The callback used to validate the given value. You can leave this parameter to null if you don't want to perform validation.</param>
        /// <param name="throwIfValidationFails">Whether to throw if the validation of <paramref name="validateValueCallback"/> fails.</param>
        /// <param name="valueChangedCallback">The callback used to perform actions after the property is set. You can leave this parameter to null if you don't want to perform actions after the property is set.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the setting succeeded and the old value of the given property (or <see langword="null"/> if the property does not contain any value nor reference).</returns>
        /// <exception cref="InvalidOperationException">The given property is read-only and <paramref name="throwIfReadOnly"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="ArgumentNullException">The new value is null and <paramref name="throwIfNull"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="Exception"><paramref name="validateValueCallback"/> failed and <paramref name="throwIfValidationFails"/> is set to <see langword="true"/>. This exception is the exception that was returned by <paramref name="validateValueCallback"/> if it was not null or an <see cref="ArgumentException"/> otherwise.</exception>
        public static (bool propertyChanged, object oldValue) SetBackgroundWorkerProperty(this System.ComponentModel.BackgroundWorker obj, string propertyName, object newValue, Type declaringType, bool throwIfBusy, bool throwIfReadOnly = true, BindingFlags bindingFlags = Util.DefaultBindingFlagsForPropertySet, string paramName = null, bool setOnlyIfNotNull = false, bool throwIfNull = false, PropertyValidateValueCallback validateValueCallback = null, bool throwIfValidationFails = false, PropertyValueChangedCallback valueChangedCallback = null) => obj.IsBusy
                ? throwIfBusy ? throw new InvalidOperationException(WinCopies.Util.Desktop.Resources.ExceptionMessages.BackgroundWorkerIsBusy) : (false, GetProperty(propertyName, declaringType, bindingFlags).GetValue(obj))
                : obj.SetProperty(propertyName, newValue, declaringType, throwIfReadOnly, bindingFlags, paramName, setOnlyIfNotNull, throwIfNull, validateValueCallback, throwIfValidationFails, valueChangedCallback);

        /// <summary>
        /// Sets a value to a property if the new value is different.
        /// </summary>
        /// <param name="obj">The object in which to set the property.</param>
        /// <param name="propertyName">The name of the given property.</param>
        /// <param name="fieldName">The field related to the property.</param>
        /// <param name="newValue">The value to set.</param>
        /// <param name="declaringType">The actual declaring type of the property.</param>
        /// <param name="throwIfBusy">Whether to throw if <paramref name="obj"/> is busy.</param>
        /// <param name="throwIfReadOnly">Whether to throw if the given property is read-only.</param>
        /// <param name="bindingFlags">The <see cref="BindingFlags"/> used to get the property.</param>
        /// <param name="paramName">The parameter from which the value was passed to this method.</param>
        /// <param name="setOnlyIfNotNull">Whether to set only if the given value is not null.</param>
        /// <param name="throwIfNull">Whether to throw if the given value is null.</param>
        /// <param name="validateValueCallback">The callback used to validate the given value. You can leave this parameter to null if you don't want to perform validation.</param>
        /// <param name="throwIfValidationFails">Whether to throw if the validation of <paramref name="validateValueCallback"/> fails.</param>
        /// <param name="valueChangedCallback">The callback used to perform actions after the property is set. You can leave this parameter to null if you don't want to perform actions after the property is set.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the setting succeeded and the old value of the given property (or <see langword="null"/> if the property does not contain any value nor reference).</returns>
        /// <exception cref="InvalidOperationException">The declaring types of the given property and field name doesn't correspond.</exception>
        /// <exception cref="ArgumentNullException">The new value is null and <paramref name="throwIfNull"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="Exception"><paramref name="validateValueCallback"/> failed and <paramref name="throwIfValidationFails"/> is set to <see langword="true"/>. This exception is the exception that was returned by <paramref name="validateValueCallback"/> if it was not null or an <see cref="ArgumentException"/> otherwise.</exception>
        public static (bool propertyChanged, object oldValue) SetBackgroundWorkerProperty(this IBackgroundWorker obj, string propertyName, string fieldName, object newValue, Type declaringType, bool throwIfBusy, bool throwIfReadOnly = true, BindingFlags bindingFlags = Util.DefaultBindingFlagsForPropertySet, string paramName = null, bool setOnlyIfNotNull = false, bool throwIfNull = false, FieldValidateValueCallback validateValueCallback = null, bool throwIfValidationFails = false, FieldValueChangedCallback valueChangedCallback = null) => obj.IsBusy
                ? throwIfBusy ? throw new InvalidOperationException(WinCopies.Util.Desktop.Resources.ExceptionMessages.BackgroundWorkerIsBusy) : (false, GetField(fieldName, declaringType, bindingFlags).GetValue(obj))
                : obj.SetProperty(propertyName, fieldName, newValue, declaringType, throwIfReadOnly, bindingFlags, paramName, setOnlyIfNotNull, throwIfNull, validateValueCallback, throwIfValidationFails, valueChangedCallback);

        /// <summary>
        /// Sets a value to a property if the new value is different.
        /// </summary>
        /// <param name="obj">The object in which to set the property.</param>
        /// <param name="propertyName">The name of the given property.</param>
        /// <param name="newValue">The value to set.</param>
        /// <param name="declaringType">The actual declaring type of the property.</param>
        /// <param name="throwIfBusy">Whether to throw if <paramref name="obj"/> is busy.</param>
        /// <param name="throwIfReadOnly">Whether to throw if the given property is read-only.</param>
        /// <param name="bindingFlags">The <see cref="BindingFlags"/> used to get the property.</param>
        /// <param name="paramName">The parameter from which the value was passed to this method.</param>
        /// <param name="setOnlyIfNotNull">Whether to set only if the given value is not null.</param>
        /// <param name="throwIfNull">Whether to throw if the given value is null.</param>
        /// <param name="validateValueCallback">The callback used to validate the given value. You can leave this parameter to null if you don't want to perform validation.</param>
        /// <param name="throwIfValidationFails">Whether to throw if the validation of <paramref name="validateValueCallback"/> fails.</param>
        /// <param name="valueChangedCallback">The callback used to perform actions after the property is set. You can leave this parameter to null if you don't want to perform actions after the property is set.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the setting succeeded and the old value of the given property (or <see langword="null"/> if the property does not contain any value nor reference).</returns>
        /// <exception cref="InvalidOperationException">The given property is read-only and <paramref name="throwIfReadOnly"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="ArgumentNullException">The new value is null and <paramref name="throwIfNull"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="Exception"><paramref name="validateValueCallback"/> failed and <paramref name="throwIfValidationFails"/> is set to <see langword="true"/>. This exception is the exception that was returned by <paramref name="validateValueCallback"/> if it was not null or an <see cref="ArgumentException"/> otherwise.</exception>
        public static (bool propertyChanged, object oldValue) SetBackgroundWorkerProperty(this IBackgroundWorker obj, string propertyName, object newValue, Type declaringType, bool throwIfBusy, bool throwIfReadOnly = true, BindingFlags bindingFlags = Util.DefaultBindingFlagsForPropertySet, string paramName = null, bool setOnlyIfNotNull = false, bool throwIfNull = false, PropertyValidateValueCallback validateValueCallback = null, bool throwIfValidationFails = false, PropertyValueChangedCallback valueChangedCallback = null) => obj.IsBusy
                ? throwIfBusy ? throw new InvalidOperationException(WinCopies.Util.Desktop.Resources.ExceptionMessages.BackgroundWorkerIsBusy) : (false, GetProperty(propertyName, declaringType, bindingFlags).GetValue(obj))
                : obj.SetProperty(propertyName, newValue, declaringType, throwIfReadOnly, bindingFlags, paramName, setOnlyIfNotNull, throwIfNull, validateValueCallback, throwIfValidationFails, valueChangedCallback);

        /// <summary>
        /// Disposes an old value of a property then sets a new value to the given property if the new value is different.
        /// </summary>
        /// <param name="obj">The object in which to set the property.</param>
        /// <param name="propertyName">The name of the given property.</param>
        /// <param name="fieldName">The field related to the property.</param>
        /// <param name="newValue">The value to set.</param>
        /// <param name="declaringType">The actual declaring type of the property.</param>
        /// <param name="throwIfBusy">Whether to throw if <paramref name="obj"/> is busy.</param>
        /// <param name="throwIfReadOnly">Whether to throw if the given property is read-only.</param>
        /// <param name="bindingFlags">The <see cref="BindingFlags"/> used to get the property.</param>
        /// <param name="paramName">The parameter from which the value was passed to this method.</param>
        /// <param name="setOnlyIfNotNull">Whether to set only if the given value is not null.</param>
        /// <param name="throwIfNull">Whether to throw if the given value is null.</param>
        /// <param name="validateValueCallback">The callback used to validate the given value. You can leave this parameter to null if you don't want to perform validation.</param>
        /// <param name="throwIfValidationFails">Whether to throw if the validation of <paramref name="validateValueCallback"/> fails.</param>
        /// <param name="valueChangedCallback">The callback used to perform actions after the property is set. You can leave this parameter to null if you don't want to perform actions after the property is set.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the setting succeeded and the old value of the given property (or <see langword="null"/> if the property does not contain any value nor reference).</returns>
        /// <exception cref="InvalidOperationException">The declaring types of the given property and field name doesn't correspond. OR The given property is read-only and <paramref name="throwIfReadOnly"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="ArgumentNullException">The new value is null and <paramref name="throwIfNull"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="Exception"><paramref name="validateValueCallback"/> failed and <paramref name="throwIfValidationFails"/> is set to <see langword="true"/>. This exception is the exception that was returned by <paramref name="validateValueCallback"/> if it was not null or an <see cref="ArgumentException"/> otherwise.</exception>
        public static (bool propertyChanged, IDisposable oldValue) DisposeAndSetBackgroundWorkerProperty(this System.ComponentModel.BackgroundWorker obj, string propertyName, string fieldName, IDisposable newValue, Type declaringType, bool throwIfBusy, bool throwIfReadOnly = true, BindingFlags bindingFlags = Util.DefaultBindingFlagsForPropertySet, string paramName = null, bool setOnlyIfNotNull = false, bool throwIfNull = false, FieldValidateValueCallback validateValueCallback = null, bool throwIfValidationFails = false, FieldValueChangedCallback valueChangedCallback = null) => obj.IsBusy
                ? throwIfBusy ? throw new InvalidOperationException(WinCopies.Util.Desktop.Resources.ExceptionMessages.BackgroundWorkerIsBusy) : (false, (IDisposable)GetField(fieldName, declaringType, bindingFlags).GetValue(obj))
                : obj.DisposeAndSetProperty(propertyName, fieldName, newValue, declaringType, throwIfReadOnly, bindingFlags, paramName, setOnlyIfNotNull, throwIfNull, validateValueCallback, throwIfValidationFails, valueChangedCallback);

        /// <summary>
        /// Disposes an old value of a property then sets a new value to the given property if the new value is different.
        /// </summary>
        /// <param name="obj">The object in which to set the property.</param>
        /// <param name="propertyName">The name of the given property.</param>
        /// <param name="newValue">The value to set.</param>
        /// <param name="declaringType">The actual declaring type of the property.</param>
        /// <param name="throwIfBusy">Whether to throw if <paramref name="obj"/> is busy.</param>
        /// <param name="throwIfReadOnly">Whether to throw if the given property is read-only.</param>
        /// <param name="bindingFlags">The <see cref="BindingFlags"/> used to get the property.</param>
        /// <param name="paramName">The parameter from which the value was passed to this method.</param>
        /// <param name="setOnlyIfNotNull">Whether to set only if the given value is not null.</param>
        /// <param name="throwIfNull">Whether to throw if the given value is null.</param>
        /// <param name="validateValueCallback">The callback used to validate the given value. You can leave this parameter to null if you don't want to perform validation.</param>
        /// <param name="throwIfValidationFails">Whether to throw if the validation of <paramref name="validateValueCallback"/> fails.</param>
        /// <param name="valueChangedCallback">The callback used to perform actions after the property is set. You can leave this parameter to null if you don't want to perform actions after the property is set.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the setting succeeded and the old value of the given property (or <see langword="null"/> if the property does not contain any value nor reference).</returns>
        /// <exception cref="InvalidOperationException">The given property is read-only and <paramref name="throwIfReadOnly"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="ArgumentNullException">The new value is null and <paramref name="throwIfNull"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="Exception"><paramref name="validateValueCallback"/> failed and <paramref name="throwIfValidationFails"/> is set to <see langword="true"/>. This exception is the exception that was returned by <paramref name="validateValueCallback"/> if it was not null or an <see cref="ArgumentException"/> otherwise.</exception>
        public static (bool propertyChanged, IDisposable oldValue) DisposeAndSetBackgroundWorkerProperty(this System.ComponentModel.BackgroundWorker obj, string propertyName, IDisposable newValue, Type declaringType, bool throwIfBusy, bool throwIfReadOnly = true, BindingFlags bindingFlags = Util.DefaultBindingFlagsForPropertySet, string paramName = null, bool setOnlyIfNotNull = false, bool throwIfNull = false, PropertyValidateValueCallback validateValueCallback = null, bool throwIfValidationFails = false, PropertyValueChangedCallback valueChangedCallback = null) => obj.IsBusy
                ? throwIfBusy ? throw new InvalidOperationException(WinCopies.Util.Desktop.Resources.ExceptionMessages.BackgroundWorkerIsBusy) : (false, (IDisposable)GetProperty(propertyName, declaringType, bindingFlags).GetValue(obj))
                : obj.DisposeAndSetProperty(propertyName, newValue, declaringType, throwIfReadOnly, bindingFlags, paramName, setOnlyIfNotNull, throwIfNull, validateValueCallback, throwIfValidationFails, valueChangedCallback);

        /// <summary>
        /// Disposes an old value of a property then sets a new value to the given property if the new value is different.
        /// </summary>
        /// <param name="obj">The object in which to set the property.</param>
        /// <param name="propertyName">The name of the given property.</param>
        /// <param name="fieldName">The field related to the property.</param>
        /// <param name="newValue">The value to set.</param>
        /// <param name="declaringType">The actual declaring type of the property.</param>
        /// <param name="throwIfBusy">Whether to throw if <paramref name="obj"/> is busy.</param>
        /// <param name="throwIfReadOnly">Whether to throw if the given property is read-only.</param>
        /// <param name="bindingFlags">The <see cref="BindingFlags"/> used to get the property.</param>
        /// <param name="paramName">The parameter from which the value was passed to this method.</param>
        /// <param name="setOnlyIfNotNull">Whether to set only if the given value is not null.</param>
        /// <param name="throwIfNull">Whether to throw if the given value is null.</param>
        /// <param name="validateValueCallback">The callback used to validate the given value. You can leave this parameter to null if you don't want to perform validation.</param>
        /// <param name="throwIfValidationFails">Whether to throw if the validation of <paramref name="validateValueCallback"/> fails.</param>
        /// <param name="valueChangedCallback">The callback used to perform actions after the property is set. You can leave this parameter to null if you don't want to perform actions after the property is set.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the setting succeeded and the old value of the given property (or <see langword="null"/> if the property does not contain any value nor reference).</returns>
        /// <exception cref="InvalidOperationException">The declaring types of the given property and field name doesn't correspond.</exception>
        /// <exception cref="ArgumentNullException">The new value is null and <paramref name="throwIfNull"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="Exception"><paramref name="validateValueCallback"/> failed and <paramref name="throwIfValidationFails"/> is set to <see langword="true"/>. This exception is the exception that was returned by <paramref name="validateValueCallback"/> if it was not null or an <see cref="ArgumentException"/> otherwise.</exception>
        public static (bool propertyChanged, IDisposable oldValue) DisposeAndSetBackgroundWorkerProperty(this IBackgroundWorker obj, string propertyName, string fieldName, IDisposable newValue, Type declaringType, bool throwIfBusy, bool throwIfReadOnly = true, BindingFlags bindingFlags = Util.DefaultBindingFlagsForPropertySet, string paramName = null, bool setOnlyIfNotNull = false, bool throwIfNull = false, FieldValidateValueCallback validateValueCallback = null, bool throwIfValidationFails = false, FieldValueChangedCallback valueChangedCallback = null) => obj.IsBusy
                ? throwIfBusy ? throw new InvalidOperationException(WinCopies.Util.Desktop.Resources.ExceptionMessages.BackgroundWorkerIsBusy) : (false, (IDisposable)GetField(fieldName, declaringType, bindingFlags).GetValue(obj))
                : obj.DisposeAndSetProperty(propertyName, fieldName, newValue, declaringType, throwIfReadOnly, bindingFlags, paramName, setOnlyIfNotNull, throwIfNull, validateValueCallback, throwIfValidationFails, valueChangedCallback);

        /// <summary>
        /// Disposes an old value of a property then sets a new value to the given property if the new value is different.
        /// </summary>
        /// <param name="obj">The object in which to set the property.</param>
        /// <param name="propertyName">The name of the given property.</param>
        /// <param name="newValue">The value to set.</param>
        /// <param name="declaringType">The actual declaring type of the property.</param>
        /// <param name="throwIfBusy">Whether to throw if <paramref name="obj"/> is busy.</param>
        /// <param name="throwIfReadOnly">Whether to throw if the given property is read-only.</param>
        /// <param name="bindingFlags">The <see cref="BindingFlags"/> used to get the property.</param>
        /// <param name="paramName">The parameter from which the value was passed to this method.</param>
        /// <param name="setOnlyIfNotNull">Whether to set only if the given value is not null.</param>
        /// <param name="throwIfNull">Whether to throw if the given value is null.</param>
        /// <param name="validateValueCallback">The callback used to validate the given value. You can leave this parameter to null if you don't want to perform validation.</param>
        /// <param name="throwIfValidationFails">Whether to throw if the validation of <paramref name="validateValueCallback"/> fails.</param>
        /// <param name="valueChangedCallback">The callback used to perform actions after the property is set. You can leave this parameter to null if you don't want to perform actions after the property is set.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the setting succeeded and the old value of the given property (or <see langword="null"/> if the property does not contain any value nor reference).</returns>
        /// <exception cref="InvalidOperationException">The given property is read-only and <paramref name="throwIfReadOnly"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="ArgumentNullException">The new value is null and <paramref name="throwIfNull"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="Exception"><paramref name="validateValueCallback"/> failed and <paramref name="throwIfValidationFails"/> is set to <see langword="true"/>. This exception is the exception that was returned by <paramref name="validateValueCallback"/> if it was not null or an <see cref="ArgumentException"/> otherwise.</exception>
        public static (bool propertyChanged, IDisposable oldValue) DisposeAndSetBackgroundWorkerProperty(this IBackgroundWorker obj, string propertyName, IDisposable newValue, Type declaringType, bool throwIfBusy, bool throwIfReadOnly = true, BindingFlags bindingFlags = Util.DefaultBindingFlagsForPropertySet, string paramName = null, bool setOnlyIfNotNull = false, bool throwIfNull = false, PropertyValidateValueCallback validateValueCallback = null, bool throwIfValidationFails = false, PropertyValueChangedCallback valueChangedCallback = null) => obj.IsBusy
                ? throwIfBusy ? throw new InvalidOperationException(WinCopies.Util.Desktop.Resources.ExceptionMessages.BackgroundWorkerIsBusy) : (false, (IDisposable)GetProperty(propertyName, declaringType, bindingFlags).GetValue(obj))
                : obj.DisposeAndSetProperty(propertyName, newValue, declaringType, throwIfReadOnly, bindingFlags, paramName, setOnlyIfNotNull, throwIfNull, validateValueCallback, throwIfValidationFails, valueChangedCallback);
    }
}
