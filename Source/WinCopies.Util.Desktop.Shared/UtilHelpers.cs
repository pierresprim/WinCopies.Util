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

using System.Windows;

namespace WinCopies.Util.Desktop
{
    public static class UtilHelpers
    {
        public static DependencyProperty Register<TValue, TOwnerType>(in string propertyName) => DependencyProperty.Register(propertyName, typeof(TValue), typeof(TOwnerType));

        public static DependencyProperty Register<TValue, TOwnerType>(in string propertyName, in PropertyMetadata propertyMetadata) => DependencyProperty.Register(propertyName, typeof(TValue), typeof(TOwnerType), propertyMetadata);

        public static DependencyPropertyKey RegisterReadOnly<TValue, TOwnerType>(in string propertyName, in PropertyMetadata propertyMetadata) => DependencyProperty.RegisterReadOnly(propertyName, typeof(TValue), typeof(TOwnerType), propertyMetadata);

        public static RoutedEvent RegisterRoutedEvent<TEventHandler, TOwnerType>(in string eventName, in RoutingStrategy routingStrategy) => EventManager.RegisterRoutedEvent(eventName, routingStrategy, typeof(TEventHandler), typeof(TOwnerType));
    }
}
