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
using System.Globalization;
using WinCopies.Util.Resources;

#if WinCopies2
using static WinCopies.Util.Util;
#else
using static WinCopies.UtilHelpers;

using IfCT = WinCopies.Diagnostics.ComparisonType;
#endif
using static WinCopies.Util.Resources.ExceptionMessages;

namespace WinCopies
#if WinCopies2
    .Util
#endif
{
    public static class ThrowHelper
    {
        #region Array Throws

        public static ArgumentException GetArrayWithMoreThanOneDimensionException(in string paramName) => new ArgumentException(ArrayWithMoreThanOneDimension, paramName);

        [Obsolete("This method has been replaced by GetArrayWithMoreThanOneDimensionException and ThrowIfArrayWithMoreThanOneDimension.")]
        public static void ThrowArrayWithMoreThanOneDimensionException(in string paramName) => throw GetArrayWithMoreThanOneDimensionException(paramName);

        public static void ThrowIfArrayHasMoreThanOneDimension(in Array array, in string paramName)
        {
            if (array.Rank > 1)

                throw GetArrayWithMoreThanOneDimensionException(paramName);
        }

        #endregion

        #region Reflection Throws

        public static InvalidOperationException GetDeclaringTypesNotCorrespondException(in string propertyName, in string methodName) => new InvalidOperationException(string.Format(Culture, DeclaringTypesNotCorrespond, propertyName, methodName));

        [Obsolete("Throw the result of GetDeclaringTypesNotCorrespondException instead of calling this method.")]
        public static void ThrowDeclaringTypesNotCorrespondException(string propertyName, string methodName) => throw GetDeclaringTypesNotCorrespondException(propertyName, methodName);

        public static ArgumentException GetFieldOrPropertyNotFoundException(in string fieldOrPropertyName, in Type objectType) => new ArgumentException(string.Format(CultureInfo.CurrentCulture, FieldOrPropertyNotFound, fieldOrPropertyName, objectType));

        #endregion

        public static InvalidOperationException GetMoreThanOneOccurencesWereFoundException() => new InvalidOperationException(MoreThanOneOccurencesWereFound);

        [Obsolete("Throw the result of GetMoreThanOneOccurencesWereFoundException instead of calling this method.")]
        public static void ThrowMoreThanOneOccurencesWereFoundException() => throw GetMoreThanOneOccurencesWereFoundException();

        [Obsolete("This method has been replaced by GetOneOrMoreKeyIsNullException.")]
        public static ArgumentException GetOneOrMoreKeyIsNull() => GetOneOrMoreKeyIsNullException();

        public static ArgumentException GetOneOrMoreKeyIsNullException() => new ArgumentException(OneOrMoreKeyIsNull);

        [Obsolete("Throw the result of GetOneOrMoreKeyIsNullException instead of calling this method.")]
        public static void ThrowOneOrMoreKeyIsNull() => throw GetOneOrMoreKeyIsNullException();
    }
}
