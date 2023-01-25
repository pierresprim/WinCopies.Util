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

#if CS7
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using WinCopies.Collections.Generic;
using WinCopies.Extensions;
using WinCopies.Linq;
using WinCopies.Util;

namespace WinCopies
{
    public class InterfaceDataTemplateSelector : DataTemplateSelector
    {
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
        public class Ignore : Attribute
        {
            // Left empty.
        }

        private byte _bools = 0b111;

        public bool IgnoreGenerics { get => GetBit(0); set => SetBit(0, value); }
        public bool IgnoreFirstTypesWithoutInterfaces { get => GetBit(1); set => SetBit(1, value); }
        public bool DirectInterfacesOnly { get => GetBit(2); set => SetBit(2, value); }
        public bool DirectTypeOnly { get => GetBit(3); set => SetBit(3, value); }
        public bool IgnoreClassType { get => GetBit(4); set => SetBit(4, value); }

        public InterfaceDataTemplateSelector() { /* Left empty. */ }

        private bool GetBit(in byte pos) => _bools.GetBit(pos);
        private void SetBit(in byte pos, in bool value) => UtilHelpers.SetBit(ref _bools, pos, value);

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null ||
#if !CS9
                !(
#endif
                container is
#if CS9
                not
#endif
                FrameworkElement containerElement
#if !CS9
                )
#endif
                )

                return base.SelectTemplate(item, container);

            Type itemType = item.GetType();

            System.Collections.Generic.IEnumerable<Type> types = DirectInterfacesOnly ? itemType.GetDirectInterfaces(IgnoreGenerics, DirectTypeOnly, IgnoreFirstTypesWithoutInterfaces, predicate: t => t.CustomAttributes.FirstOrDefault(_t => typeof(Ignore).IsAssignableFrom(_t.AttributeType)) == null) : itemType.GetInterfaces();

            return (IgnoreClassType ? types : types.Prepend(itemType)).FirstOrDefault<DataTemplate>(t => containerElement.TryFindResource(new DataTemplateKey(t))) ?? base.SelectTemplate(item, container);
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class TypeForDataTemplateAttribute : Attribute
    {
        public Type Type { get; }

        public TypeForDataTemplateAttribute(Type type) => Type = type;
    }

    public class AttributeDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container) => item == null ||
#if !CS9
            !(
#endif
            container is
#if CS9
            not
#endif
            FrameworkElement containerElement
#if !CS9
        )
#endif
                ? base.SelectTemplate(item, container)
                : ((TypeForDataTemplateAttribute[])item.GetType().GetCustomAttributes(typeof(TypeForDataTemplateAttribute), false))
                    .FirstOrDefault<TypeForDataTemplateAttribute, DataTemplate>(t => containerElement.TryFindResource(new DataTemplateKey(t.Type))) ?? base.SelectTemplate(item, container);
    }
}
#endif
