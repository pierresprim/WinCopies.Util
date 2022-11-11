/* Copyright © Pierre Sprimont, 2021
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
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace WinCopies.Desktop
{
    public abstract class Application : System.Windows.Application
    {
        protected abstract Window GetWindow();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            (MainWindow = GetWindow()).Show();
        }
    }

    public static class Delegates
    {
        public static void CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.Handled = e.CanExecute = true; }
    }
}

namespace WinCopies.Util.Desktop
{
    public static class UtilHelpers
    {
        private static bool TryGetValue(in object item, in string value, in bool inherit, out string? result, ref TypeConverterAttribute
#if CS8
                ?
#endif
                typeConverterAttribute, ref TypeConverterAttribute
#if CS8
                ?
#endif
            _typeConverterAttribute, ref Type
#if CS8
                ?
#endif
            type, ref Type
#if CS8
                ?
#endif
            _type, ref TypeConverter
#if CS8
                ?
#endif
            typeConverter, ref TypeConverter
#if CS8
                ?
#endif
            _typeConverter, ref string tmp)
        {
            if ((typeConverterAttribute = item.GetType().GetCustomAttribute<TypeConverterAttribute>(inherit)) == null)
            {
                result = null;

                return false;
            }

            if (typeConverterAttribute != _typeConverterAttribute)
            {
                _typeConverterAttribute = typeConverterAttribute;

                if ((type = Type.GetType(typeConverterAttribute.ConverterTypeName)) != _type)
                {
                    _type = type;

                    if (typeof(TypeConverter).IsAssignableFrom(type))

                        if ((typeConverter = TypeDescriptor.GetConverter(type)) != _typeConverter)
                        {
                            _typeConverter = typeConverter;

                            if (!typeConverter.CanConvertTo(typeof(string)))
                            {
                                result = null;

                                return false;
                            }
                        }

                        else
                        {
                            result = null;

                            return false;
                        }
                }
            }

            if ((tmp = typeConverter.ConvertToString(item))?.ToLower() == value)
            {
                result = tmp;

                return true;
            }

            result = null;

            return false;
        }

        public static bool TryGetValue(in IEnumerable values, ref string? value, bool inherit, out object
#if CS8
            ?
#endif
            result)
        {
            TypeConverterAttribute
#if CS8
                ?
#endif
                typeConverterAttribute = null;
            TypeConverterAttribute
#if CS8
                ?
#endif
                _typeConverterAttribute = null;
            Type
#if CS8
                ?
#endif
                type = null;
            Type
#if CS8
                ?
#endif
                _type = null;
            TypeConverter
#if CS8
                ?
#endif
                typeConverter = null;
            TypeConverter
#if CS8
                ?
#endif
                _typeConverter = null;

            string
#if CS8
                ?
#endif
                tmp = null;

            foreach (object
#if CS8
                ?
#endif
                item in values)
            {
                bool check(ref string _value)
                {
                    if (item == null && string.IsNullOrEmpty(_value))
                    {
                        _value = null;

                        return true;
                    }

                    tmp = item.ToString();

                    if (item.Equals(_value) || tmp?.ToLower() == _value)
                    {
                        _value = tmp;

                        return true;
                    }

                    if (TryGetValue(item, _value, inherit, out string? _result, ref typeConverterAttribute, ref _typeConverterAttribute, ref type, ref _type, ref typeConverter, ref _typeConverter, ref tmp))
                    {
                        _value = _result;

                        return true;
                    }

                    return false;
                }

                if (check(ref value))
                {
                    result = item;

                    return true;
                }
            }

            value = null;
            result = null;

            return false;
        }

        public static bool TryGetValue(in IEnumerable values, string value, in bool inherit, out object
#if CS8
            ?
#endif
            result) => TryGetValue(values, ref value, inherit, out result);

        public static string
#if CS8
            ?
#endif
            TryGetValue(in object item, string value, bool inherit)
        {
            TypeConverterAttribute
#if CS8
                ?
#endif
                typeConverterAttribute = null;
            TypeConverterAttribute
#if CS8
                ?
#endif
                _typeConverterAttribute = null;
            Type
#if CS8
                ?
#endif
                type = null;
            Type
#if CS8
                ?
#endif
                _type = null;
            TypeConverter
#if CS8
                ?
#endif
                typeConverter = null;
            TypeConverter
#if CS8
                ?
#endif
                _typeConverter = null;

            return TryGetValue(item, value, inherit, out string? result, ref typeConverterAttribute, ref _typeConverterAttribute, ref type, ref _type, ref typeConverter, ref _typeConverter, ref value) ? result : item?.ToString();
        }

        public static RoutedEventArgs<BooleanEventArgs> GetRoutedBooleanEventArgs(in RoutedEvent @event, in bool value) => new
#if !CS9
            RoutedEventArgs<BooleanEventArgs>
#endif
            (@event, new BooleanEventArgs(value));

        public static void RegisterClassHandler<T>(in RoutedEvent routedEvent, in Delegate handler) => EventManager.RegisterClassHandler(typeof(T), routedEvent, handler);

        public static void RegisterClassHandler<T>(in RoutedEvent routedEvent, in Delegate handler, in bool handledEventsToo) => EventManager.RegisterClassHandler(typeof(T), routedEvent, handler, handledEventsToo);

        public static DependencyProperty Register<TValue, TOwner>(in string propertyName) => DependencyProperty.Register(propertyName, typeof(TValue), typeof(TOwner));

        public static DependencyProperty Register<TValue, TOwner>(in string propertyName, in PropertyMetadata propertyMetadata) => DependencyProperty.Register(propertyName, typeof(TValue), typeof(TOwner), propertyMetadata);

        public static DependencyProperty Register<TValue, TOwner>(in string propertyName, in TValue defaultValue) => Register<TValue, TOwner>(propertyName, new PropertyMetadata(defaultValue));

        public static DependencyPropertyKey RegisterReadOnly<TValue, TOwner>(in string propertyName, in PropertyMetadata propertyMetadata) => DependencyProperty.RegisterReadOnly(propertyName, typeof(TValue), typeof(TOwner), propertyMetadata);

        public static DependencyPropertyKey RegisterReadOnly<TValue, TOwner>(in string propertyName, in TValue defaultValue) => RegisterReadOnly<TValue, TOwner>(propertyName, new PropertyMetadata(defaultValue));

        public static DependencyProperty RegisterAttached<T>(in string propertyName, in Type ownerType) => DependencyProperty.RegisterAttached(propertyName, typeof(T), ownerType);

        public static DependencyProperty RegisterAttached<T>(in string propertyName, in Type ownerType, in PropertyMetadata metadata) => DependencyProperty.RegisterAttached(propertyName, typeof(T), ownerType, metadata);

        public static DependencyProperty RegisterAttached<T>(in string propertyName, in Type ownerType, in T defaultValue) => RegisterAttached<T>(propertyName, ownerType, new PropertyMetadata(defaultValue));

        public static DependencyProperty RegisterAttached<TValue, TOwner>(in string propertyName) => RegisterAttached<TValue>(propertyName, typeof(TOwner));

        public static DependencyProperty RegisterAttached<TValue, TOwner>(in string propertyName, in PropertyMetadata metadata) => RegisterAttached<TValue>(propertyName, typeof(TOwner), metadata);

        public static DependencyProperty RegisterAttached<TValue, TOwner>(in string propertyName, in TValue defaultValue) => RegisterAttached(propertyName, typeof(TOwner), defaultValue);

        public static DependencyPropertyKey RegisterAttachedReadOnly<T>(in string propertyName, in Type ownerType, in PropertyMetadata propertyMetadata) => DependencyProperty.RegisterAttachedReadOnly(propertyName, typeof(T), ownerType, propertyMetadata);

        public static DependencyPropertyKey RegisterAttachedReadOnly<TValue, TOwner>(in string propertyName, in PropertyMetadata propertyMetadata) => RegisterAttachedReadOnly<TValue>(propertyName, typeof(TOwner), propertyMetadata);

        public static DependencyPropertyKey RegisterAttachedReadOnly<T>(in string propertyName, in Type ownerType, in T defaultValue) => RegisterAttachedReadOnly<T>(propertyName, ownerType, new PropertyMetadata(defaultValue));

        public static DependencyPropertyKey RegisterAttachedReadOnly<TValue, TOwner>(in string propertyName, in TValue defaultValue) => RegisterAttachedReadOnly(propertyName, typeof(TOwner), defaultValue);

        public static RoutedEvent Register<TEventHandler, TOwner>(in string eventName, in RoutingStrategy routingStrategy) => EventManager.RegisterRoutedEvent(eventName, routingStrategy, typeof(TEventHandler), typeof(TOwner));

        public static void RaiseEvent(this UIElement uiElement, RoutedEvent routedEvent) => (uiElement ?? throw WinCopies.ThrowHelper.GetArgumentNullException(nameof(uiElement))).RaiseEvent(new RoutedEventArgs(routedEvent));

        public static void UpdateAttachedRoutedEventHandler(in DependencyObject d, in RoutedEvent @event, in Delegate handler, in Converter<UIElement, Action<RoutedEvent, Delegate>> converter)
        {
            if (d is UIElement uiElement)

                converter(uiElement)(@event, handler);
        }

        public static void AddAttachedRoutedEventHandler(in DependencyObject d, in RoutedEvent @event, in Delegate handler) => UpdateAttachedRoutedEventHandler(d, @event, handler, uiElement => uiElement.AddHandler);

        public static void RemoveAttachedRoutedEventHandler(in DependencyObject d, in RoutedEvent @event, in Delegate handler) => UpdateAttachedRoutedEventHandler(d, @event, handler, uiElement => uiElement.RemoveHandler);
    }
}
