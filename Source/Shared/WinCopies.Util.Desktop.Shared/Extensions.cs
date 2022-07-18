/* Copyright © Pierre Sprimont, 2020
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
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using WinCopies
#if !WinCopies3
    .Util
#endif
    .Commands;

using static WinCopies.
#if !WinCopies3
    Util.
#endif
    ForLoop;

using
#if WinCopies3
WinCopies.DotNetFix;
using WinCopies.Util;

using static WinCopies.Consts;
using static WinCopies.ThrowHelper;
using static WinCopies.UtilHelpers;

namespace WinCopies.Desktop
#else
static WinCopies.Util.Util;

namespace WinCopies.Util
#endif
{
    public static class Extensions
    {
        public static bool CanExecuteCommand<T>(this ICommand<T> command, in object
#if CS8
            ?
#endif
            parameter) => (parameter is T _parameter && command.CanExecute(_parameter)) || (parameter == null && !typeof(T).IsValueType && command.CanExecute(default));

        public static bool TryExecuteCommand<T>(this ICommand<T> command, in object
#if CS8
            ?
#endif
            parameter)
        {
            if (parameter is T _parameter)
            {
                command.Execute(_parameter);

                return true;
            }

            if (parameter == null && !typeof(T).IsValueType)
            {
                command.Execute(default);

                return true;
            }

            return false;
        }

        public static void OverrideMetadata<T>(this DependencyProperty property, in PropertyMetadata metadata) => property.OverrideMetadata(typeof(T), metadata);

        public static void OverrideMetadata<T>(this DependencyProperty property, in object value) => property.OverrideMetadata<T>(new PropertyMetadata(value));

        public static void OverrideFrameworkPropertyMetadata<T>(this DependencyProperty property, in object value) => property.OverrideMetadata<T>(new FrameworkPropertyMetadata(value));

        public static void
#if WinCopies3
            OverrideDefaultStyleKey
#else
            OverrideFrameworkPropertyMetadata
#endif
            <T>(this DependencyProperty property) => property.OverrideFrameworkPropertyMetadata<T>(typeof(T));

        public static T
#if CS9
                ?
#endif
                GetChild<T>(this DependencyObject parent, in bool lookForDirectChildOnly, out bool isDirectChild) where T : Visual
        {
            T
#if CS9
                ?
#endif
                child = default;
            int count = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count; i++)
            {
                var visual = VisualTreeHelper.GetChild(parent, i) as Visual;
                child = visual as T;

                if (child == null)
                {
                    if (!(lookForDirectChildOnly || visual == null) && child == null)
                    {
                        child = GetChild<T>(visual, false, out _);

                        if (child != null)

                            break;
                    }
                }

                else
                {
                    isDirectChild = true;

                    return child;
                }
            }

            isDirectChild = false;

            return child;
        }

        public static Panel
#if CS8
            ?
#endif
            GetItemsPanel(this DependencyObject itemsControl)
        {
            ItemsPresenter itemsPresenter = itemsControl.GetChild<ItemsPresenter>(false, out _);

            return itemsPresenter == null ? null : VisualTreeHelper.GetChild(itemsPresenter, 0) as Panel;
        }

#if CS5
        public static bool HasHorizontalOrientation(this Panel panel) => panel.HasLogicalOrientationPublic && panel.LogicalOrientationPublic == Orientation.Horizontal;

        public static bool HasVerticalOrientation(this Panel panel) => panel.HasLogicalOrientationPublic && panel.LogicalOrientationPublic == Orientation.Vertical;

        private struct TryGetFirstParams
        {
            public int Index { get; }

            public int Count { get; }

            public TryGetFirstParams(in int index, in int count)
            {
                Index = index;
                Count = count;
            }
        }

        private static bool TryGet<T>(in IList itemCollection, in int i, ref bool checkContent, out T
#if CS9
            ?
#endif
            item) where T : Visual
        {
#if CS8
            static
#endif
            bool onItemFound(in T __item, out T ___item)
            {
                ___item = __item;

                return false;
            }

            object
#if CS8
            ?
#endif
            obj = itemCollection[i];

            if (obj is T _item)

                return onItemFound(_item, out item);

            else if (checkContent && obj is ContentPresenter contentPresenter && (_item = contentPresenter.GetChild<T>(true, out _)) != null)
            {
                checkContent = true;

                return onItemFound(_item, out item);
            }

            item = default;

            return true;
        }

        private delegate T LoopFunc<T>(in int index, in int count, out bool ok, in FuncOut<int, T, bool> func);

        private static T TryGetFirst<T>(IList itemCollection, in TryGetFirstParams initParams, in TryGetFirstParams? rollBackParams, out bool rollBack, ref bool checkContent, LoopFunc<T> loopFunc, out bool found) where T : Visual
        {
            T loop(in TryGetFirstParams @params, ref bool _checkContent, out bool _found)
            {
                bool __checkContent = _checkContent;

                T result = loopFunc(@params.Index, @params.Count, out _found, (int i, out T _item) => TryGet(itemCollection, i, ref __checkContent, out _item));

                _checkContent = __checkContent;

                return result;
            }

            T item = loop(initParams, ref checkContent, out found);

            rollBack = false;

            if (!found && rollBackParams.HasValue)
            {
                item = loop(rollBackParams.Value, ref checkContent, out found);

                rollBack = true;
            }

            return item;
        }

        private struct TGFParams
        {
            public FuncIn<int, TryGetFirstParams> GetInitParams { get; }

            public FuncIn<int, TryGetFirstParams> GetRollBackParams { get; }

            public TGFParams(in int index, in FuncIn<int, int, TryGetFirstParams> getInitParams, in FuncIn<int, int, TryGetFirstParams> getRollBackParams)
            {
                GetInitParams = GetTGFParamsFunc(index, getInitParams);
                GetRollBackParams = GetTGFParamsFunc(index, getRollBackParams);
            }

            private static FuncIn<int, TryGetFirstParams> GetTGFParamsFunc(int index, FuncIn<int, int, TryGetFirstParams> func) => (in int count) => func(index, count);
        }

        private static TOut TryGetFirst<TIn, TOut>(in TIn itemsControl, in FuncIn<TIn, IList> func, in TGFParams @params, ref bool rollBack, ref bool checkContent, in LoopFunc<TOut> loopFunc, out bool found) where TOut : Visual
        {
            if (itemsControl == null)

                throw GetArgumentNullException(nameof(itemsControl));

            IList itemCollection = func(itemsControl);

            int count = itemCollection.Count;

            return TryGetFirst(itemCollection, @params.GetInitParams(count), rollBack ? @params.GetRollBackParams(count) :
#if !CS9
                (TryGetFirstParams?)
#endif
                null, out rollBack, ref checkContent, loopFunc, out found);
        }

        private static TGFParams GetTGFParams(in int index, in FuncIn<int, int, TryGetFirstParams> funcInitParams, in FuncIn<int, int, TryGetFirstParams> funcRollBackParams) => new
#if !CS9
            TGFParams
#endif
            (index, funcInitParams, funcRollBackParams);

        private static TGFParams GetTGFAParams(in int index) => GetTGFParams(index, (in int _index, in int count) => new
#if !CS9
            TryGetFirstParams
#endif
            (_index + 1, count), (in int _index, in int _count) => new
#if !CS9
            TryGetFirstParams
#endif
            (0, _index));

        private static TGFParams GetTGFBParams(in int index) => GetTGFParams(index, (in int _index, in int count) => new
#if !CS9
            TryGetFirstParams
#endif
            (_index - 1, 0), (in int _index, in int _count) => new
#if !CS9
            TryGetFirstParams
#endif
            (_count - 1, _index + 1));

        private static T TryGetFirst<T>(in ItemsControl itemsControl, int index, in FuncIn<int, TGFParams> func, ref bool rollBack, ref bool checkContent, in LoopFunc<T> loopFunc, out bool found) where T : Visual => TryGetFirst(itemsControl, (in ItemsControl _itemsControl) => _itemsControl.Items, func(index), ref rollBack, ref checkContent, loopFunc, out found);

        public static T TryGetFirstAfter<T>(this ItemsControl itemsControl, int index, ref bool rollBack, ref bool checkContent, out bool found) where T : Visual => TryGetFirst<T>(itemsControl, index, GetTGFAParams, ref rollBack, ref checkContent, LoopFuncASC, out found);

        public static T TryGetFirstBefore<T>(this ItemsControl itemsControl, int index, ref bool rollBack, ref bool checkContent, out bool found) where T : Visual => TryGetFirst<T>(itemsControl, index, GetTGFBParams, ref rollBack, ref checkContent, LoopFuncDESC, out found);

        private static T TryGetFirst<T>(in Panel itemsControl, int index, in FuncIn<int, TGFParams> func, ref bool rollBack, ref bool checkContent, in LoopFunc<T> loopFunc, out bool found) where T : Visual => TryGetFirst(itemsControl, (in Panel _itemsControl) => _itemsControl.Children, func(index), ref rollBack, ref checkContent, loopFunc, out found);

        public static T TryGetFirstAfter<T>(this Panel itemsControl, int index, ref bool rollBack, ref bool checkContent, out bool found) where T : Visual => TryGetFirst<T>(itemsControl, index, GetTGFAParams, ref rollBack, ref checkContent, LoopFuncASC, out found);

        public static T TryGetFirstBefore<T>(this Panel itemsControl, int index, ref bool rollBack, ref bool checkContent, out bool found) where T : Visual => TryGetFirst<T>(itemsControl, index, GetTGFBParams, ref rollBack, ref checkContent, LoopFuncDESC, out found);
#endif

        public static void Add(this CommandBindingCollection commandBindings, in ICommand command, in ExecutedRoutedEventHandler executed, in CanExecuteRoutedEventHandler canExecute) => commandBindings.Add(new CommandBinding(command, executed, canExecute));

        public static void Add(this CommandBindingCollection commandBindings, in ICommand command, Action _delegate) => commandBindings.Add(new CommandBinding(command, (object sender, ExecutedRoutedEventArgs e) => _delegate(), (object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true));

        public static void AddCommandBinding(this UIElement obj, in ICommand command, Action _delegate) => obj.CommandBindings.Add(command, _delegate);

        public static void Execute(this ICommand command, object
#if CS8
            ?
#endif
            commandParameter, IInputElement commandTarget)
        {
            if ((command ?? throw GetArgumentNullException(nameof(command))) is RoutedCommand _command)

                _command.Execute(commandParameter, commandTarget);

            else

                command.Execute(commandParameter);
        }

        /// <summary>
        /// Check if the command can be executed, and executes it if available. See the remarks section.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="commandParameter">The parameter of your command.</param>
        /// <remarks>
        /// This method only evaluates the commands of the common <see cref="ICommand"/> type. To evaluate a command of the <see cref="RoutedCommand"/> type, consider using the <see cref="TryExecute(RoutedCommand, object, IInputElement)"/> method. If you are not sure of the type of your command, so consider using the <see cref="TryExecute(ICommand, object, IInputElement)"/> method.
        /// </remarks>
        public static bool TryExecute(this ICommand
#if CS8
            ?
#endif
            command, object
#if CS8
            ?
#endif
            commandParameter)
        {
            if (command?.CanExecute(commandParameter) == true)
            {
                command.Execute(commandParameter);

                return true;
            }

            return false;
        }

        public static bool TryExecute(this RoutedCommand
#if CS8
            ?
#endif
            command, object
#if CS8
            ?
#endif
            commandParameter, IInputElement commandTarget)
        {
            if (command?.CanExecute(commandParameter, commandTarget) == true)
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

        public static bool TryExecute<T>(this IQueryCommand<T>
#if CS8
            ?
#endif
            command, object
#if CS8
            ?
#endif
            commandParameter, out T
#if CS9
            ?
#endif
            result)
        {
            if (command?.CanExecute(commandParameter) == true)
            {
                result = command.Execute(commandParameter);

                return true;
            }

            result = default;

            return false;
        }

        public static bool TryExecute<T>(this IQueryRoutedCommand<T>
#if CS8
            ?
#endif
            command, object
#if CS8
            ?
#endif
            commandParameter, IInputElement commandTarget, out T
#if CS9
            ?
#endif
            result)
        {
            if (command?.CanExecute(commandParameter, commandTarget) == true)
            {
                // try
                // {

                result = command.Execute(commandParameter, commandTarget);

                // }
                // catch (InvalidOperationException ex)
                // {
                // Debug.WriteLine(ex.Message);
                // }

                return true;
            }

            result = default;

            return false;
        }

        public static bool TryExecute(this ICommand
#if CS8
            ?
#endif
            command, object
#if CS8
            ?
#endif
            commandParameter, IInputElement commandTarget) => command is RoutedCommand _command
                ? _command.TryExecute(commandParameter, commandTarget)
                : command.TryExecute(commandParameter);

        public static bool TryExecute2(this ICommand
#if CS8
            ?
#endif
            command, ICommandSource commandSource) => command.TryExecute(commandSource.CommandParameter, commandSource.CommandTarget);

        public static bool TryExecute<T>(this IQueryCommand<T>
#if CS8
            ?
#endif
            command, object
#if CS8
            ?
#endif
            commandParameter, IInputElement commandTarget, out T
#if CS9
            ?
#endif
            result) => command is IQueryRoutedCommand<T> _command
                ? _command.TryExecute(commandParameter, commandTarget, out result)
                : command.TryExecute(commandParameter, out result);

        public static bool TryExecute<T>(this IQueryCommand<T>
#if CS8
            ?
#endif
            command, ICommandSource
#if CS8
            ?
#endif
            commandSource, out T
#if CS9
            ?
#endif
            result)
        {
            if (command == null || commandSource == null)
            {
                result = default;

                return false;
            }

            return command.TryExecute(commandSource.CommandParameter, commandSource.CommandTarget, out result);
        }

        public static bool CanExecute(this ICommand
#if CS8
            ?
#endif
            command, object
#if CS8
            ?
#endif
            commandParameter, IInputElement commandTarget) => command is RoutedCommand routedCommand
                ? routedCommand.CanExecute(commandParameter, commandTarget)
                : command == null
                ? false
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

        internal static FieldInfo GetField(in string fieldName, in Type objectType, in BindingFlags bindingFlags) => objectType.GetField(fieldName, bindingFlags) ?? throw new ArgumentException(string.Format(WinCopies.
#if !WinCopies3
            Util.
#endif
            Resources.ExceptionMessages.FieldOrPropertyNotFound, fieldName, objectType));

        internal static PropertyInfo GetProperty(in string propertyName, in Type objectType, in BindingFlags bindingFlags) => objectType.GetProperty(propertyName, bindingFlags) ?? throw new ArgumentException(string.Format(WinCopies.
#if !WinCopies3
            Util.
#endif
            Resources.ExceptionMessages.FieldOrPropertyNotFound, propertyName, objectType));

#if CS7
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
        public static
#if CS7
            (bool propertyChanged, object oldValue)
#else
            ValueTuple<bool, object>
#endif
             SetBackgroundWorkerProperty(this System.ComponentModel.BackgroundWorker obj, string propertyName, string fieldName, object newValue, Type declaringType, bool throwIfBusy, bool throwIfReadOnly = true, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, string paramName = null, bool setOnlyIfNotNull = false, bool throwIfNull = false, FieldValidateValueCallback validateValueCallback = null, bool throwIfValidationFails = false, FieldValueChangedCallback valueChangedCallback = null) => obj.IsBusy
                ? throwIfBusy ? throw new InvalidOperationException(WinCopies.
#if !WinCopies3
                    Util.
#endif
                    Desktop.Resources.ExceptionMessages.BackgroundWorkerIsBusy) :
#if !CS7
            new ValueTuple<bool, object>
#endif
                (false, GetField(fieldName, declaringType, bindingFlags).GetValue(obj)) : obj.SetProperty(propertyName, fieldName, newValue, declaringType, throwIfReadOnly, bindingFlags, paramName, setOnlyIfNotNull, throwIfNull, validateValueCallback, throwIfValidationFails, valueChangedCallback);

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
        public static
#if CS7
            (bool propertyChanged, object oldValue)
#else
            ValueTuple<bool, object>
#endif
             SetBackgroundWorkerProperty(this System.ComponentModel.BackgroundWorker obj, string propertyName, object newValue, Type declaringType, bool throwIfBusy, bool throwIfReadOnly = true, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, string paramName = null, bool setOnlyIfNotNull = false, bool throwIfNull = false, PropertyValidateValueCallback validateValueCallback = null, bool throwIfValidationFails = false, PropertyValueChangedCallback valueChangedCallback = null) => obj.IsBusy
                ? throwIfBusy ? throw new InvalidOperationException(WinCopies.
#if !WinCopies3
                    Util.
#endif
                    Desktop.Resources.ExceptionMessages.BackgroundWorkerIsBusy) :
#if !CS7
            new ValueTuple<bool, object>
#endif
            (false, GetProperty(propertyName, declaringType, bindingFlags).GetValue(obj
#if !CS7
                , null
#endif
                ))
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
        public static
#if CS7
            (bool propertyChanged, object oldValue)
#else
            ValueTuple<bool, object>
#endif
             SetBackgroundWorkerProperty(this IBackgroundWorker obj, string propertyName, string fieldName, object newValue, Type declaringType, bool throwIfBusy, bool throwIfReadOnly = true, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, string paramName = null, bool setOnlyIfNotNull = false, bool throwIfNull = false, FieldValidateValueCallback validateValueCallback = null, bool throwIfValidationFails = false, FieldValueChangedCallback valueChangedCallback = null) => obj.IsBusy
                ? throwIfBusy ? throw new InvalidOperationException(WinCopies.
#if !WinCopies3
                    Util.
#endif
                    Desktop.Resources.ExceptionMessages.BackgroundWorkerIsBusy) :
#if !CS7
            new ValueTuple<bool, object>
#endif
            (false, GetField(fieldName, declaringType, bindingFlags).GetValue(obj))
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
        public static
#if CS7
            (bool propertyChanged, object oldValue)
#else
            ValueTuple<bool, object>
#endif
             SetBackgroundWorkerProperty(this IBackgroundWorker obj, string propertyName, object newValue, Type declaringType, bool throwIfBusy, bool throwIfReadOnly = true, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, string paramName = null, bool setOnlyIfNotNull = false, bool throwIfNull = false, PropertyValidateValueCallback validateValueCallback = null, bool throwIfValidationFails = false, PropertyValueChangedCallback valueChangedCallback = null) => obj.IsBusy
                ? throwIfBusy ? throw new InvalidOperationException(WinCopies.
#if !WinCopies3
                    Util.
#endif
                    Desktop.Resources.ExceptionMessages.BackgroundWorkerIsBusy) :
#if !CS7
            new ValueTuple<bool, object>
#endif
            (false, GetProperty(propertyName, declaringType, bindingFlags).GetValue(obj
#if !CS7
                , null
#endif
                ))
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
        public static
#if CS7
            (bool propertyChanged, IDisposable oldValue)
#else
            ValueTuple<bool, IDisposable>
#endif
             DisposeAndSetBackgroundWorkerProperty(this System.ComponentModel.BackgroundWorker obj, string propertyName, string fieldName, IDisposable newValue, Type declaringType, bool throwIfBusy, bool throwIfReadOnly = true, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, string paramName = null, bool setOnlyIfNotNull = false, bool throwIfNull = false, FieldValidateValueCallback validateValueCallback = null, bool throwIfValidationFails = false, FieldValueChangedCallback valueChangedCallback = null) => obj.IsBusy
                ? throwIfBusy ? throw new InvalidOperationException(WinCopies.
#if !WinCopies3
                    Util.
#endif
                    Desktop.Resources.ExceptionMessages.BackgroundWorkerIsBusy) :
#if !CS7
            new ValueTuple<bool, IDisposable>
#endif
            (false, (IDisposable)GetField(fieldName, declaringType, bindingFlags).GetValue(obj))
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
        public static
#if CS7
            (bool propertyChanged, IDisposable oldValue)
#else
            ValueTuple<bool, IDisposable>
#endif
            DisposeAndSetBackgroundWorkerProperty(this System.ComponentModel.BackgroundWorker obj, string propertyName, IDisposable newValue, Type declaringType, bool throwIfBusy, bool throwIfReadOnly = true, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, string paramName = null, bool setOnlyIfNotNull = false, bool throwIfNull = false, PropertyValidateValueCallback validateValueCallback = null, bool throwIfValidationFails = false, PropertyValueChangedCallback valueChangedCallback = null) => obj.IsBusy
                ? throwIfBusy ? throw new InvalidOperationException(WinCopies.
#if !WinCopies3
                    Util.
#endif
                    Desktop.Resources.ExceptionMessages.BackgroundWorkerIsBusy) :
#if !CS7
            new ValueTuple<bool, IDisposable>
#endif
            (false, (IDisposable)GetProperty(propertyName, declaringType, bindingFlags).GetValue(obj
#if !CS7
                , null
#endif
                ))
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
        public static
#if CS7
            (bool propertyChanged, IDisposable oldValue)
#else
            ValueTuple<bool, IDisposable>
#endif
             DisposeAndSetBackgroundWorkerProperty(this IBackgroundWorker obj, string propertyName, string fieldName, IDisposable newValue, Type declaringType, bool throwIfBusy, bool throwIfReadOnly = true, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, string paramName = null, bool setOnlyIfNotNull = false, bool throwIfNull = false, FieldValidateValueCallback validateValueCallback = null, bool throwIfValidationFails = false, FieldValueChangedCallback valueChangedCallback = null) => obj.IsBusy
                ? throwIfBusy ? throw new InvalidOperationException(WinCopies.
#if !WinCopies3
                    Util.
#endif
                    Desktop.Resources.ExceptionMessages.BackgroundWorkerIsBusy) :
#if !CS7
            new ValueTuple<bool, IDisposable>
#endif
            (false, (IDisposable)GetField(fieldName, declaringType, bindingFlags).GetValue(obj))
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
        public static
#if CS7
            (bool propertyChanged, IDisposable oldValue)
#else
            ValueTuple<bool, IDisposable>
#endif
             DisposeAndSetBackgroundWorkerProperty(this IBackgroundWorker obj, string propertyName, IDisposable newValue, Type declaringType, bool throwIfBusy, bool throwIfReadOnly = true, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, string paramName = null, bool setOnlyIfNotNull = false, bool throwIfNull = false, PropertyValidateValueCallback validateValueCallback = null, bool throwIfValidationFails = false, PropertyValueChangedCallback valueChangedCallback = null) => obj.IsBusy
                ? throwIfBusy ? throw new InvalidOperationException(WinCopies.
#if !WinCopies3
                    Util.
#endif
                    Desktop.Resources.ExceptionMessages.BackgroundWorkerIsBusy) :
#if !CS7
            new ValueTuple<bool, IDisposable>
#endif
            (false, (IDisposable)GetProperty(propertyName, declaringType, bindingFlags).GetValue(obj
#if !CS7
                , null
#endif
                ))
                : obj.DisposeAndSetProperty(propertyName, newValue, declaringType, throwIfReadOnly, bindingFlags, paramName, setOnlyIfNotNull, throwIfNull, validateValueCallback, throwIfValidationFails, valueChangedCallback);
#endif

        private static BitmapSource _ToImageSource(in Bitmap bitmap)
        {
            bitmap.MakeTransparent();

            IntPtr hBitmap = bitmap.GetHbitmap();

            BitmapSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))

                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            return wpfBitmap;
        }

        //public static BitmapSource ToImageSource2(this Bitmap bitmap)
        //{
        //using (MemoryStream stream = new MemoryStream())
        //{
        //    bitmap.MakeTransparent();
        //    bitmap.Save(stream, ImageFormat.Png); // Was .Bmp, but this did not show a transparent background.

        //    BitmapImage result = new BitmapImage();
        //    result.BeginInit();
        //    // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
        //    // Force the bitmap to load right now so we can dispose the stream.
        //    result.CacheOption = BitmapCacheOption.OnLoad;
        //    result.StreamSource = stream;
        //    result.EndInit();
        //    result.Freeze();
        //    return result;
        //}
        //}

        /// <summary>
        /// Converts a <see cref="Bitmap"/> to an <see cref="ImageSource"/>.
        /// </summary>
        /// <param name="bitmap">The <see cref="Bitmap"/> to convert.</param>
        /// <returns>The <see cref="ImageSource"/> obtained from the given <see cref="Bitmap"/>.</returns>
        public static BitmapSource ToImageSource(this Bitmap bitmap) => _ToImageSource(bitmap ?? throw GetArgumentNullException(nameof(bitmap)));

        public static BitmapSource ToImageSource(this Icon icon) => _ToImageSource((icon ?? throw GetArgumentNullException(nameof(icon))).ToBitmap());

        // https://stackoverflow.com/questions/5689674/c-sharp-convert-wpf-image-source-to-a-system-drawing-bitmap

        public static Bitmap ToBitmap(this BitmapSource bitmapSource)
        {
            ThrowIfNull(bitmapSource, nameof(bitmapSource));

            int width = bitmapSource.PixelWidth;
            int height = bitmapSource.PixelHeight;
            int stride = width * ((bitmapSource.Format.BitsPerPixel + 7) / 8);
            IntPtr ptr = IntPtr.Zero;

            try
            {
                ptr = Marshal.AllocHGlobal(height * stride);
                bitmapSource.CopyPixels(new Int32Rect(0, 0, width, height), ptr, height * stride, stride);

                using (var bitmap = new Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format1bppIndexed, ptr))

                    // Clone the bitmap so that we can dispose it and
                    // release the unmanaged memory at ptr
                    return new Bitmap(bitmap);
            }

            finally
            {
                if (ptr != IntPtr.Zero)

                    Marshal.FreeHGlobal(ptr);
            }
        }

        /// <summary>
        /// Tries to report progress for a given <see cref="IBackgroundWorker"/>.
        /// </summary>
        /// <param name="backgroundWorker">The <see cref="IBackgroundWorker"/> for which to report the progress.</param>
        /// <param name="progressPercentage">The progress percentage of <paramref name="backgroundWorker"/>.</param>
        /// <returns><see langword="true"/> if <see cref="IBackgroundWorker.WorkerReportsProgress"/> is <see langword="true"/> for <paramref name="backgroundWorker"/>; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="backgroundWorker"/> is <see langword="null"/>.</exception>
        public static bool TryReportProgress(this IBackgroundWorker backgroundWorker, int progressPercentage)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerReportsProgress)
            {
                backgroundWorker.ReportProgress(progressPercentage);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to report progress for a given <see cref="IBackgroundWorker"/>.
        /// </summary>
        /// <param name="backgroundWorker">The <see cref="IBackgroundWorker"/> for which to report the progress.</param>
        /// <param name="progressPercentage">The progress percentage of <paramref name="backgroundWorker"/>.</param>
        /// <param name="userState">An object to pass to the <see cref="IBackgroundWorker.ProgressChanged"/> event handler of <paramref name="backgroundWorker"/>.</param>
        /// <returns><see langword="true"/> if <see cref="IBackgroundWorker.WorkerReportsProgress"/> is <see langword="true"/> for <paramref name="backgroundWorker"/>; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="backgroundWorker"/> is <see langword="null"/>.</exception>
        public static bool TryReportProgress(this IBackgroundWorker backgroundWorker, int progressPercentage, object userState)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerReportsProgress)
            {
                backgroundWorker.ReportProgress(progressPercentage, userState);

                return true;
            }

            return false;
        }

        public static bool TryCancelAsync(this IBackgroundWorker backgroundWorker)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerSupportsCancellation)
            {
                backgroundWorker.CancelAsync();

                return true;
            }

            return false;
        }

        public static bool TryCancelAsync(this IBackgroundWorker backgroundWorker, object stateInfo)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerSupportsCancellation)
            {
                backgroundWorker.CancelAsync(stateInfo);

                return true;
            }

            return false;
        }

        public static bool TryCancel(this IBackgroundWorker backgroundWorker)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerSupportsCancellation)
            {
                backgroundWorker.Cancel();

                return true;
            }

            return false;
        }

        public static bool TryCancel(this IBackgroundWorker backgroundWorker, object stateInfo)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerSupportsCancellation)
            {
                backgroundWorker.Cancel(stateInfo);

                return true;
            }

            return false;
        }

#if WinCopies3

        public static bool TryPauseAsync(this IPausableBackgroundWorker backgroundWorker)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerSupportsPausing)
            {
                backgroundWorker.PauseAsync();

                return true;
            }

            return false;
        }

        public static bool TryPauseAsync(this IPausableBackgroundWorker backgroundWorker, object stateInfo)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerSupportsPausing)
            {
                backgroundWorker.PauseAsync(stateInfo);

                return true;
            }

            return false;
        }

        public static bool TryPause(this IPausableBackgroundWorker backgroundWorker)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerSupportsPausing)
            {
                backgroundWorker.Pause();

                return true;
            }

            return false;
        }

        public static bool TryPause(this IPausableBackgroundWorker backgroundWorker, object stateInfo)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerSupportsPausing)
            {
                backgroundWorker.Pause(stateInfo);

                return true;
            }

            return false;
        }

#endif

#if CS7
        /// <summary>
        /// Tries to report progress for a given <see cref="IBackgroundWorker"/>.
        /// </summary>
        /// <param name="backgroundWorker">The <see cref="IBackgroundWorker"/> for which to report the progress.</param>
        /// <param name="progressPercentage">The progress percentage of <paramref name="backgroundWorker"/>.</param>
        /// <returns><see langword="true"/> if <see cref="IBackgroundWorker.WorkerReportsProgress"/> is <see langword="true"/> for <paramref name="backgroundWorker"/>; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="backgroundWorker"/> is <see langword="null"/>.</exception>
        public static bool TryReportProgress(this DotNetFix.IBackgroundWorker backgroundWorker, int progressPercentage)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerReportsProgress)
            {
                backgroundWorker.ReportProgress(progressPercentage);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to report progress for a given <see cref="IBackgroundWorker"/>.
        /// </summary>
        /// <param name="backgroundWorker">The <see cref="IBackgroundWorker"/> for which to report the progress.</param>
        /// <param name="progressPercentage">The progress percentage of <paramref name="backgroundWorker"/>.</param>
        /// <param name="userState">An object to pass to the <see cref="IBackgroundWorker.ProgressChanged"/> event handler of <paramref name="backgroundWorker"/>.</param>
        /// <returns><see langword="true"/> if <see cref="IBackgroundWorker.WorkerReportsProgress"/> is <see langword="true"/> for <paramref name="backgroundWorker"/>; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="backgroundWorker"/> is <see langword="null"/>.</exception>
        public static bool TryReportProgress(this DotNetFix.IBackgroundWorker backgroundWorker, int progressPercentage, object userState)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerReportsProgress)
            {
                backgroundWorker.ReportProgress(progressPercentage, userState);

                return true;
            }

            return false;
        }

        public static bool TryCancelAsync(this DotNetFix.IBackgroundWorker backgroundWorker)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerSupportsCancellation)
            {
                backgroundWorker.CancelAsync();

                return true;
            }

            return false;
        }

        public static bool TryPauseAsync(this DotNetFix.IPausableBackgroundWorker backgroundWorker)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerSupportsPausing)
            {
                backgroundWorker.PauseAsync();

                return true;
            }

            return false;
        }
#endif



        /// <summary>
        /// Tries to report progress for a given <see cref="System.ComponentModel.BackgroundWorker"/>.
        /// </summary>
        /// <param name="backgroundWorker">The <see cref="System.ComponentModel.BackgroundWorker"/> for which to report the progress.</param>
        /// <param name="progressPercentage">The progress percentage of <paramref name="backgroundWorker"/>.</param>
        /// <returns><see langword="true"/> if <see cref="System.ComponentModel.BackgroundWorker.WorkerReportsProgress"/> is <see langword="true"/> for <paramref name="backgroundWorker"/>; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="backgroundWorker"/> is <see langword="null"/>.</exception>
        public static bool TryReportProgress(this System.ComponentModel.BackgroundWorker backgroundWorker, int progressPercentage)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerReportsProgress)
            {
                backgroundWorker.ReportProgress(progressPercentage);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to report progress for a given <see cref="System.ComponentModel.BackgroundWorker"/>.
        /// </summary>
        /// <param name="backgroundWorker">The <see cref="System.ComponentModel.BackgroundWorker"/> for which to report the progress.</param>
        /// <param name="progressPercentage">The progress percentage of <paramref name="backgroundWorker"/>.</param>
        /// <param name="userState">An object to pass to the <see cref="System.ComponentModel.BackgroundWorker.ProgressChanged"/> event handler of <paramref name="backgroundWorker"/>.</param>
        /// <returns><see langword="true"/> if <see cref="System.ComponentModel.BackgroundWorker.WorkerReportsProgress"/> is <see langword="true"/> for <paramref name="backgroundWorker"/>; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="backgroundWorker"/> is <see langword="null"/>.</exception>
        public static bool TryReportProgress(this System.ComponentModel.BackgroundWorker backgroundWorker, int progressPercentage, object userState)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerReportsProgress)
            {
                backgroundWorker.ReportProgress(progressPercentage, userState);

                return true;
            }

            return false;
        }

#if CS7
        public static bool TryCancelAsync(this BackgroundWorker backgroundWorker)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerSupportsCancellation)
            {
                backgroundWorker.CancelAsync();

                return true;
            }

            return false;
        }

        public static bool TryCancelAsync(this BackgroundWorker backgroundWorker, object stateInfo)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerSupportsCancellation)
            {
                backgroundWorker.CancelAsync(stateInfo);

                return true;
            }

            return false;
        }

        public static bool TryCancel(this BackgroundWorker backgroundWorker)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerSupportsCancellation)
            {
                backgroundWorker.Cancel();

                return true;
            }

            return false;
        }

        public static bool TryCancel(this BackgroundWorker backgroundWorker, object stateInfo)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerSupportsCancellation)
            {
                backgroundWorker.Cancel(stateInfo);

                return true;
            }

            return false;
        }
#endif

#if WinCopies3 && CS7
        public static bool TryPauseAsync(this PausableBackgroundWorker backgroundWorker)
        {
            if ((backgroundWorker ?? throw GetArgumentNullException(nameof(backgroundWorker))).WorkerSupportsPausing)
            {
                backgroundWorker.PauseAsync();

                return true;
            }

            return false;
        }
#endif
    }
}
