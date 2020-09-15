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

#if !WinCopies2

using static WinCopies.UtilHelpers;
using static WinCopies.ThrowHelper;
using static WinCopies.Resources.ExceptionMessages;

#endif

namespace WinCopies.Collections
{
    public static class ThrowHelper
    {
        /// <summary>
        /// Returns an exception indicating that a list or collection is read-only.
        /// </summary>
        /// <returns>An exception indicating that a list or collection is read-only.</returns>
        public static InvalidOperationException GetReadOnlyListOrCollectionException() => new InvalidOperationException("The list or collection is read-only.");

        /// <summary>
        /// Returns an exception indicating that a list or collection is empty.
        /// </summary>
        /// <returns>An exception indicating that a list or collection is empty.</returns>
        public static InvalidOperationException GetEmptyListOrCollectionException() => new InvalidOperationException("The current list or collection is empty.");

        /// <summary>
        /// Throws the exception given by <see cref="GetEmptyListOrCollectionException"/> if the <see cref="ICountable.Count"/> property of a given <see cref="ICountable"/> object is equal to 0.
        /// </summary>
        /// <param name="obj">The <see cref="ICountable"/> object for which to check the <see cref="ICountable.Count"/> property.</param>
        public static void ThrowIfEmpty(in ICountable obj)
        {
            if (obj.Count == 0)

                throw GetEmptyListOrCollectionException();
        }

        /// <summary>
        /// Throws the exception given by <see cref="GetEmptyListOrCollectionException"/> if the <see cref="IUIntCountable.Count"/> property of a given <see cref="IUIntCountable"/> object is equal to 0.
        /// </summary>
        /// <param name="obj">The <see cref="IUIntCountable"/> object for which to check the <see cref="IUIntCountable.Count"/> property.</param>
        public static void ThrowIfEmpty(in IUIntCountable obj)
        {
            if (obj.Count == 0)

                throw GetEmptyListOrCollectionException();
        }

#if !WinCopies2
        public static void ThrowIfEnumeratorNotStartedOrDisposedException(in WinCopies.Collections.IDisposableEnumeratorInfo enumerator)
        {
            if (Extensions.IsEnumeratorNotStartedOrDisposed(enumerator))

                throw GetEnumeratorNotStartedOrDisposedException();
        }

        #region Enum Throws
        ///// <summary>
        ///// Throws an <see cref="InvalidEnumArgumentException"/> if the enum value is not in the required enum value range. See the Remarks section.
        ///// </summary>
        ///// <param name="enum">The enum value to check.</param>
        ///// <param name="argumentName">The parameter name.</param>
        ///// <remarks>This method doesn't read all the enum fields, but only takes care of the first and last numeric enum fields, so if the value is 1, and the enum has only defined fields for 0 and 2, this method still doesn't throw. For a method that actually reads all the enum fields, see the <see cref="ThrowIfNotDefinedEnumValue(Enum)"/> method.</remarks>
        ///// <seealso cref="IsValidEnumValue(Enum)"/>
        ///// <seealso cref="ThrowIfNotValidEnumValue(Enum)"/>
        public static void ThrowIfNotValidEnumValue(in string argumentName, in Enum @enum)
        {
            if (!@enum.IsValidEnumValue()) throw new InvalidEnumArgumentException(argumentName, @enum);
            // .GetType().IsEnumDefined(@enum)
        }

        ///// <summary>
        ///// Throws an <see cref="InvalidEnumArgumentException"/> if the enum value is not in the required enum value range. See the Remarks section.
        ///// </summary>
        ///// <param name="enum">The enum value to check.</param>
        ///// <param name="argumentName">The parameter name.</param>
        ///// <remarks>This method doesn't read all the enum fields, but only takes care of the first and last numeric enum fields, so if the value is 1, and the enum has only defined fields for 0 and 2, this method still doesn't throw. For a method that actually reads all the enum fields, see the <see cref="ThrowIfNotDefinedEnumValue(Enum)"/> method.</remarks>
        ///// <seealso cref="IsValidEnumValue(Enum)"/>
        ///// <seealso cref="ThrowIfNotDefinedEnumValue(Enum)"/>
        public static void ThrowIfNotDefinedEnumValue(in Enum @enum, in string argumentName)
        {
            if (!(@enum ?? throw GetArgumentNullException(nameof(@enum))).GetType().IsEnumDefined(@enum))

                throw new InvalidEnumArgumentException(argumentName, @enum);
        }

        public static void ThrowIfInvalidEnumValue(in Enum value, in bool valuesAreExpected, in string argumentName, params Enum[] values)
        {
            if (!value.IsValidEnumValue(valuesAreExpected, values))

                throw GetInvalidEnumArgumentException(argumentName, value);
        }

        public static void ThrowIfInvalidEnumValue<T>(in T value, in bool valuesAreExpected, in string argumentName, params T[] values) where T : Enum
        {
            if (!value.IsValidEnumValue(valuesAreExpected, values))

                throw GetInvalidEnumArgumentException(argumentName, value);
        }

#if WinCopies2
        public static void ThrowIfInvalidFlagsEnumValue<T>(in T value, in IfCT comparisonType, in string argumentName, params T[] values) where T : Enum
        {
            if (!value.IsValidFlagsEnumValue(comparisonType, argumentName, values))

                throw GetInvalidEnumArgumentException(argumentName, value);
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the enum value is not in the required enum value range.
        /// </summary>
        /// <param name="enum">The enum value to check.</param>
        /// <param name="throwIfNotFlagsEnum">Whether to throw if the given enum does not have the <see cref="FlagsAttribute"/> attribute.</param>
        /// <param name="throwIfZero">Whether to throw if the given enum is zero.</param>
        /// <exception cref="ArgumentException"><paramref name="enum"/> does not have the <see cref="FlagsAttribute"/> and <paramref name="throwIfNotFlagsEnum"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="enum"/> is equal to zero and the <paramref name="throwIfZero"/> parameter is set to true or <paramref name="enum"/> is lesser than zero.</exception>
        /// <seealso cref="Extensions.IsValidEnumValue(Enum)"/>
        /// <seealso cref="Extensions.ThrowIfNotValidEnumValue(Enum, in string)"/>
        public static void ThrowIfNotValidFlagsEnumValue(in Enum @enum, in bool throwIfNotFlagsEnum, in bool throwIfZero)
        {
            if (!@enum.IsValidFlagsEnumValue(throwIfNotFlagsEnum, throwIfZero))

                throw GetInvalidEnumArgumentException(nameof(@enum), @enum);
        }

        /// <summary>
        /// Throws an <see cref="InvalidEnumArgumentException"/> if the enum value is not in the required enum value range.
        /// </summary>
        /// <param name="enum">The enum value to check.</param>
        /// <param name="argumentName">The parameter name.</param>
        /// <param name="throwIfNotFlagsEnum">Whether to throw if the given enum does not have the <see cref="FlagsAttribute"/> attribute.</param>
        /// <param name="throwIfZero">Whether to throw if the given enum is zero.</param>
        /// <exception cref="ArgumentException"><paramref name="enum"/> does not have the <see cref="FlagsAttribute"/> and <paramref name="throwIfNotFlagsEnum"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="enum"/> is equal to zero and the <paramref name="throwIfZero"/> parameter is set to true or <paramref name="enum"/> is lesser than zero.</exception>
        /// <seealso cref="Extensions.IsValidEnumValue(Enum)"/>
        /// <seealso cref="Extensions.ThrowIfNotValidEnumValue(Enum)"/>
        public static void ThrowIfNotValidFlagsEnumValue(in Enum @enum, in string argumentName, in bool throwIfNotFlagsEnum, in bool throwIfZero)
        {
            if (!@enum.IsValidFlagsEnumValue(throwIfNotFlagsEnum, throwIfZero))

                throw new InvalidEnumArgumentException(argumentName, (int)Convert.ChangeType(@enum, TypeCode.Int32), @enum.GetType());
        }
#endif

        public static InvalidEnumArgumentException GetInvalidEnumArgumentException(in string argumentName, in Enum value) => new InvalidEnumArgumentException(argumentName, value);

        public static ArgumentException GetExceptionForNonFlagsEnum(in string argumentName) => new ArgumentException(NonFlagsEnumException, argumentName);

        public static void ThrowIfNotFlagsEnum(in Enum value, in string argumentName)
        {
            if (!value.IsFlagsEnum())

                throw GetExceptionForNonFlagsEnum(argumentName);
        }

        public static TypeArgumentException GetExceptionForNonFlagsEnumType(in string typeArgumentName) => new TypeArgumentException(NonFlagsEnumTypeException, typeArgumentName);

        public static void ThrowIfNotFlagsEnumType<T>(in string typeArgumentName) where T : Enum
        {
            if (!IsFlagsEnum<T>())

                throw GetExceptionForNonFlagsEnum(typeArgumentName);
        }

#endregion
#endif
    }
}
