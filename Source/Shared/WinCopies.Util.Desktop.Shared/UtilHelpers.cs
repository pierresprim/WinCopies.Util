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
using System.Windows;
using System.Windows.Input;

using WinCopies.Commands;

namespace WinCopies.Desktop
{
    public static class Delegates
    {
        public static void CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.Handled = true; e.CanExecute = true; }
    }
}

namespace WinCopies.Util.Desktop
{
    public static class UtilHelpers
    {
        public static RoutedEventArgs<BooleanEventArgs> GetRoutedBooleanEventArgs(in RoutedEvent @event, in bool value) => new
#if !CS9
            RoutedEventArgs<BooleanEventArgs>
#endif
            (@event, new BooleanEventArgs(value));

        public static void RegisterClassHandler<T>(in RoutedEvent routedEvent, in Delegate handler) => EventManager.RegisterClassHandler(typeof(T), routedEvent, handler);

        public static void RegisterClassHandler<T>(in RoutedEvent routedEvent, in Delegate handler, in bool handledEventsToo) => EventManager.RegisterClassHandler(typeof(T), routedEvent, handler, handledEventsToo);

        public static DependencyProperty Register<TValue, TOwner>(in string propertyName) => DependencyProperty.Register(propertyName, typeof(TValue), typeof(TOwner));

        public static DependencyProperty Register<TValue, TOwner>(in string propertyName, in PropertyMetadata propertyMetadata) => DependencyProperty.Register(propertyName, typeof(TValue), typeof(TOwner), propertyMetadata);

        public static DependencyPropertyKey RegisterReadOnly<TValue, TOwner>(in string propertyName, in PropertyMetadata propertyMetadata) => DependencyProperty.RegisterReadOnly(propertyName, typeof(TValue), typeof(TOwner), propertyMetadata);

        public static DependencyProperty RegisterAttached<T>(in string propertyName, in Type ownerType) => DependencyProperty.RegisterAttached(propertyName, typeof(T), ownerType);

        public static DependencyProperty RegisterAttached<TValue, TOwner>(in string propertyName) => RegisterAttached<TValue>(propertyName, typeof(TOwner));

        public static RoutedEvent RegisterRoutedEvent<TEventHandler, TOwner>(in string eventName, in RoutingStrategy routingStrategy) => EventManager.RegisterRoutedEvent(eventName, routingStrategy, typeof(TEventHandler), typeof(TOwner));

        public static void RaiseEvent(this UIElement uiElement, RoutedEvent routedEvent) => (uiElement ?? throw WinCopies.
#if WinCopies3
            ThrowHelper
#else
            Util.Util
#endif
            .GetArgumentNullException(nameof(uiElement))).RaiseEvent(new RoutedEventArgs(routedEvent));

        public static void UpdateAttachedRoutedEventHandler(in DependencyObject d, in RoutedEvent @event, in Delegate handler, in Converter<UIElement, Action<RoutedEvent, Delegate>> converter)
        {
            if (d is UIElement uiElement)

                converter(uiElement)(@event, handler);
        }

        public static void AddAttachedRoutedEventHandler(in DependencyObject d, in RoutedEvent @event, in Delegate handler) => UpdateAttachedRoutedEventHandler(d, @event, handler, uiElement => uiElement.AddHandler);

        public static void RemoveAttachedRoutedEventHandler(in DependencyObject d, in RoutedEvent @event, in Delegate handler) => UpdateAttachedRoutedEventHandler(d, @event, handler, uiElement => uiElement.RemoveHandler);
    }
}
