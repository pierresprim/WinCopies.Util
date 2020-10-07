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

namespace WinCopies.Util.Data
{
    public class MultiValueConversionAttribute : Attribute
    {
        public Type[] SourceTypes { get; }

        public Type TargetType { get; }

        public Type ParameterType { get; set; }

        public MultiValueConversionAttribute(Type[] sourceTypes, Type targetType)
        {
            SourceTypes = sourceTypes;

            TargetType = targetType;
        }

        public override object TypeId => this;

        public override int GetHashCode()
        {
            int sourceTypesHashCode = 0;

            foreach (Type t in SourceTypes)

                sourceTypesHashCode += t.GetHashCode();

            return sourceTypesHashCode + TargetType.GetHashCode();
        }
    }
}
