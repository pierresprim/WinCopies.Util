﻿/* Copyright © Pierre Sprimont, 2021
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

#if WinCopies3 && CS5
using System;

using static WinCopies.ThrowHelper;

namespace WinCopies.PropertySystem
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyDescriptionAttribute : Attribute
    {
        public string FriendlyName { get; }

        public string Description { get; }

        public PropertyDescriptionAttribute(string friendlyName, string description)
        {
            ThrowIfNullEmptyOrWhiteSpace(friendlyName);
            ThrowIfNullEmptyOrWhiteSpace(description);

            FriendlyName = friendlyName;

            Description = description;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class PropertyDescriptionFindingAttribute : Attribute
    {
        public Type PropertyDescriptionType { get; }

        public string NameFormat { get; }

        public string DescriptionFormat { get; }

        public PropertyDescriptionFindingAttribute(Type propertyDescriptionType, string nameFormat, string descriptionFormat)
        {
            ThrowIfNullEmptyOrWhiteSpace(nameFormat);
            ThrowIfNullEmptyOrWhiteSpace(descriptionFormat);

            PropertyDescriptionType = propertyDescriptionType;

            NameFormat = nameFormat;

            DescriptionFormat = descriptionFormat;
        }
    }
}
#endif
