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

using WinCopies.Extensions;

#if WinCopies3
using WinCopies.Collections.Generic;
using WinCopies.Linq;
#else
using WinCopies.Collections;
#endif

namespace WinCopies
#if !WinCopies3
    .Util
#endif
{
    public class InterfaceDataTemplateSelector : DataTemplateSelector
    {
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
        public class Ignore : Attribute
        {
            // Left empty.
        }

        public bool IgnoreGenerics { get; set; } = true;

        public bool DirectTypeOnly { get; set; } = true;

        public bool IgnoreFirstTypesWithoutInterfaces { get; } = true;

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

            return
#if !WinCopies3
                System.Linq.
#endif
                Enumerable.Repeat(itemType, 1).Concat(itemType.GetDirectInterfaces(IgnoreGenerics, DirectTypeOnly, IgnoreFirstTypesWithoutInterfaces, t => t.CustomAttributes.FirstOrDefault(_t => typeof(Ignore).IsAssignableFrom(_t.AttributeType)) == null))
                .FirstOrDefault<DataTemplate>(t => containerElement.TryFindResource(new DataTemplateKey(t))) ?? base.SelectTemplate(item, container);
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public
#if WinCopies3
        sealed
#endif
        class TypeForDataTemplateAttribute : Attribute
    {
        public Type Type { get; }

        public TypeForDataTemplateAttribute(Type type) => Type = type;
    }

    public class AttributeDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container) => item == null || !(container is FrameworkElement containerElement)
                ? base.SelectTemplate(item, container)
                : ((TypeForDataTemplateAttribute[])item.GetType().GetCustomAttributes(typeof(TypeForDataTemplateAttribute), false))
                    .FirstOrDefault<TypeForDataTemplateAttribute, DataTemplate>(t => containerElement.TryFindResource(new DataTemplateKey(t.Type))) ?? base.SelectTemplate(item, container);
    }
}
#endif
