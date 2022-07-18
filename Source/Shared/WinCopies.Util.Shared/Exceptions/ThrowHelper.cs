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
using System.Collections.Generic;
using System.Globalization;

using WinCopies.Util;

using static WinCopies.
#if !WinCopies3
    Util.
#endif
    Resources.ExceptionMessages;
#if WinCopies3
using static WinCopies.UtilHelpers;
#else
using static WinCopies.Util.Util;
#endif

namespace WinCopies
#if !WinCopies3
    .Util
#endif
{
    /// <summary>
    /// This class contains some helper methods for exception throwing.
    /// </summary>
    public static class ThrowHelper
    {
        public static OverflowException GetOverflowException(in string paramName) => new
#if !CS9
            OverflowException
#endif
            (Format(Overflow, paramName));

        public static void ThrowOverflowException(in string paramName) => throw GetOverflowException(paramName);

        public static ArgumentException GetArgumentException(in string paramName) => new
#if !CS9
            ArgumentException
#endif
            (Format(InvalidArgument, paramName));

        public static void ThrowArgumentException(in string paramName) => throw GetArgumentException(paramName);

        public static IndexOutOfRangeException GetIndexOutOfRangeException(in string paramName) => new
#if !CS9
            IndexOutOfRangeException
#endif
            (Format(IndexOutOfRange, paramName));

        public static void ThrowIndexOutOfRangeException(in string paramName) => throw GetIndexOutOfRangeException(paramName);

        public static InvalidCastException GetInvalidCastException(in Type
#if CS8
            ?
#endif
            typeIn, in Type typeOut) => new
#if !CS9
            InvalidCastException
#endif
            ($"Cannot convert from {typeIn?.ToString() ?? "null"} to {typeOut}.");

        public static InvalidCastException GetInvalidCastException(in object
#if CS8
            ?
#endif
            obj, in Type typeOut) => GetInvalidCastException(obj?.GetType(), typeOut);

        public static InvalidCastException GetInvalidCastException<T>(in Type type) => GetInvalidCastException(type, typeof(T));

        public static InvalidCastException GetInvalidCastException<T>(in object
#if CS8
            ?
#endif
            obj) => GetInvalidCastException(obj, typeof(T));

        public static InvalidCastException GetInvalidCastException<TIn, TOut>() => GetInvalidCastException<TOut>(typeof(TIn));

        public static T
#if CS9
            ?
#endif
             GetOrThrowIfInvalidType<T>(in object
#if CS8
            ?
#endif
            obj, in string argumentName, in bool nullAllowed) where T : class => obj is T _obj ? _obj : (obj == null && nullAllowed) ?
#if !CS9
            (T)
#endif
            null : throw new TypeArgumentException<T>(obj?.GetType(), argumentName);

        public static T? GetValueOrThrowIfInvalidType<T>(in object
#if CS8
            ?
#endif
            obj, in string argumentName, in bool nullAllowed) where T : struct => obj is T _obj ? _obj : (obj == null && nullAllowed) ?
#if !CS9
            (T?)
#endif
            null : throw new TypeArgumentException<T>(obj?.GetType(), argumentName);

        public static T GetOrThrowIfInvalidType<T>(in object
#if CS8
            ?
#endif
            obj, in string argumentName) => obj is T _obj ? _obj : throw new TypeArgumentException<T>(obj?.GetType(), argumentName);

        public static void ThrowIfNull(object
#if CS8
                ?
#endif
                obj, in string paramName)
        {
            if (obj == null)

                throw GetArgumentNullException(paramName);
        }

        public static T GetResultOrThrowIfNull<T>(Func<T
#if CS8
                ?
#endif
                > func, string errorMessage) where T : class => func() ?? throw new InvalidOperationException(errorMessage);

        public static Func<T> FuncAsGetResultOrThrowIfNull<T>(Func<T
#if CS8
                ?
#endif
                > func, string errorMessage) where T : class => () => GetResultOrThrowIfNull(func, errorMessage);

        public static Exception GetExceptionForInvalidType<T>(in object
#if CS8
                ?
#endif
                obj, in string argumentName) => GetExceptionForInvalidType<T>(obj?.GetType(), argumentName);

        public static InvalidEnumArgumentException GetArgumentMustBeFromEnumAndNotValueException(in string argumentName, in string enumName, in Enum value, in Enum forbiddenValue) => new InvalidEnumArgumentException(string.Format(ArgumentMustBeFromEnumAndNotValue, argumentName, enumName, $"{enumName}.{forbiddenValue}"), value);

#if WinCopies3
        #region Enum Throws
#if CS6
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
#endif

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

#if !WinCopies3
        public static void ThrowIfInvalidFlagsEnumValue<T>(in T value, in ComparisonType comparisonType, in string argumentName, params T[] values) where T : Enum
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

#if CS7
        public static void ThrowIfNotFlagsEnum(in Enum value, in string argumentName)
        {
            if (!value.IsFlagsEnum())

                throw GetExceptionForNonFlagsEnum(argumentName);
        }
#endif

        public static TypeArgumentException GetExceptionForNonFlagsEnumType(in string typeArgumentName) => new TypeArgumentException(NonFlagsEnumTypeException, typeArgumentName);

#if CS7
        public static void ThrowIfNotFlagsEnumType<T>(in string typeArgumentName) where T : Enum
        {
            if (!IsFlagsEnum<T>())

                throw GetExceptionForNonFlagsEnum(typeArgumentName);
        }
#endif

        #endregion
#endif

        #region Array Throws

        public static ArgumentException GetArrayWithMoreThanOneDimensionException(in string paramName) => new ArgumentException(ArrayWithMoreThanOneDimension, paramName);

        public static void ThrowIfArrayHasMoreThanOneDimension(in Array array, in string paramName)
        {
            if (array.Rank > 1)

                throw GetArrayWithMoreThanOneDimensionException(paramName);
        }

        #endregion

        #region Reflection Throws

        public static InvalidOperationException GetDeclaringTypesNotCorrespondException(in string propertyName, in string methodName) => new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, DeclaringTypesNotCorrespond, propertyName, methodName));

        public static ArgumentException GetFieldOrPropertyNotFoundException(in string fieldOrPropertyName, in Type objectType) => new ArgumentException(string.Format(CultureInfo.CurrentCulture, FieldOrPropertyNotFound, fieldOrPropertyName, objectType));

        #endregion

#if !CS5
        public
#else
        private
#endif
            static ArgumentException GetValuesMustBeAnInstanceOfTypeException<T>(in object value, in int index, in string paramName) => new
#if !CS9
            ArgumentException
#endif
            ($"{paramName} must all be instances of {typeof(T)}. The value at {index} is {(value == null ? "null" : $"an instance of {value.GetType()}")}.", paramName);

#if !CS5
        public
#else
        private
#endif
            static object GetAtOrThrowIfArrayNull<T>(in
#if CS5
            IReadOnlyList<
#endif
            T
#if CS5
            >
#else
            []
#endif
            values, in int index) => (values ?? throw GetArgumentNullException(nameof(values)))[index];

#if CS5
        public static ArgumentException GetValuesMustBeAnInstanceOfTypeException<TValues, TExpected>(in IReadOnlyList<TValues> values, in int index, in string paramName) => GetValuesMustBeAnInstanceOfTypeException<TExpected>(GetAtOrThrowIfArrayNull(values, index), index, paramName);
#endif

        public static ArgumentException GetArgumentMustBeAnInstanceOfTypeException(in string argumentName, in string typeName) => new
#if !CS9
            ArgumentException
#endif
            (string.Format(ArgumentMustBeAnInstanceOf, argumentName, typeName));

        public static ArgumentException GetArgumentMustBeAnInstanceOfTypeException(in string argumentName, in Type t) => new
#if !CS9
            ArgumentException
#endif
            (string.Format(ArgumentMustBeAnInstanceOf, argumentName, t));

        public static ArgumentException GetArgumentMustBeAnInstanceOfTypeException<T>(in string argumentName) => GetArgumentMustBeAnInstanceOfTypeException(argumentName, typeof(T));

        public static ArgumentException GetInvalidTypeArgumentException(in string argumentName) => new
#if !CS9
            ArgumentException
#endif
            (GivenTypeIsNotSupported, argumentName);

        public static InvalidOperationException GetMoreThanOneOccurencesWereFoundException() => new
#if !CS9
            InvalidOperationException
#endif
            (MoreThanOneOccurencesWereFound);

        public static ArgumentException GetOneOrMoreKeyIsNullException() => new
#if !CS9
            ArgumentException
#endif
            (OneOrMoreKeyIsNull);

        public static InvalidOperationException GetEnumeratorNotStartedOrDisposedException() => new
#if !CS9
            InvalidOperationException
#endif
            (EnumeratorIsNotStartedOrDisposed);

        public static InvalidOperationException GetVersionHasChangedException() => new
#if !CS9
            InvalidOperationException
#endif
            (CollectionChangedDuringEnumeration);

        public static void ThrowIfVersionHasChanged(int currentVersion, int initialVersion)
        {
            if (currentVersion != initialVersion)

                throw GetVersionHasChangedException();
        }

        public static void ThrowIfVersionHasChanged(uint currentVersion, uint initialVersion)
        {
            if (currentVersion != initialVersion)

                throw GetVersionHasChangedException();
        }

        public static void ThrowIfVersionHasChanged(long currentVersion, long initialVersion)
        {
            if (currentVersion != initialVersion)

                throw GetVersionHasChangedException();
        }

        public static void ThrowIfVersionHasChanged(ulong currentVersion, ulong initialVersion)
        {
            if (currentVersion != initialVersion)

                throw GetVersionHasChangedException();
        }

        public static ArgumentException GetArrayHasNotEnoughSpaceException(in string arrayArgumentName) => new
#if !CS9
            ArgumentException
#endif
            (ArrayHasNotEnoughSpace, arrayArgumentName);

        public static void ThrowIfIndexIsLowerThanZero(in int index, in string indexArgumentName)
        {
            if (index < 0)

                throw new
#if WinCopies3
                    IndexOutOfRangeException
#else
                    ArgumentOutOfRangeException
#endif
                    (indexArgumentName);
        }

        public static T GetOrThrowIfDisposed<T>(in DotNetFix.IDisposable obj, in T value)
        {
            ThrowIfDisposed(obj);

            return value;
        }

        public static T GetOrThrowIfDisposed<T>(in DotNetFix.IDisposable obj, in Func<T> func)
        {
            ThrowIfDisposed(obj);

            return func();
        }

#if WinCopies3
        public static ArgumentException GetMultidimensionalArraysNotSupportedException(in string arrayArgumentName) => new ArgumentException(MultidimensionalArraysNotSupported, arrayArgumentName);

        public static void ThrowIfMultidimensionalArray(in Array array, in string arrayArgumentName)
        {
            if (array.Rank > 1)

                throw GetMultidimensionalArraysNotSupportedException(arrayArgumentName);
        }

        public static ArgumentException GetArrayHasNonZeroLowerBoundException(in string arrayArgumentName) => new ArgumentException(ArrayHasNonZeroLowerBound, arrayArgumentName);

        public static void ThrowIfArrayHasNotEnoughSpace(in Array array, in int arrayIndex, in int count, in string arrayArgumentName)
        {
            if (count <= array.Length - arrayIndex)

                throw GetArrayHasNotEnoughSpaceException(arrayArgumentName);
        }

        public static void ThrowIfArrayHasNotEnoughSpace<T>(in T[] array, in int arrayIndex, in int count, in string arrayArgumentName)
        {
            if (count <= array.Length - arrayIndex)

                throw GetArrayHasNotEnoughSpaceException(arrayArgumentName);
        }

#if CS7
        public static void ThrowIfArrayHasNotEnoughSpace<T>(in IReadOnlyCollection<T> collection, in int arrayIndex, in int count, in string arrayArgumentName)
        {
            if (count <= collection.Count - arrayIndex)

                throw GetArrayHasNotEnoughSpaceException(arrayArgumentName);
        }
#endif

        public static void ThrowOnInvalidCopyToArrayOperation(in Array array, in int arrayIndex, in int count, in string arrayArgumentName, in string arrayIndexArgumentName)
        {
            ThrowIfNull(array, nameof(array));

            ThrowIfMultidimensionalArray(array, arrayArgumentName);

            ThrowIfIndexIsLowerThanZero(arrayIndex, arrayIndexArgumentName);

            //if (array.GetLowerBound(0) != 0)

            //    throw GetArrayHasNonZeroLowerBoundException(arrayArgumentName);

            ThrowIfArrayHasNotEnoughSpace(array, arrayIndex, count, arrayArgumentName);
        }

        public static void ThrowOnInvalidCopyToArrayOperation<T>(in T[] array, in int arrayIndex, in int count, in string arrayArgumentName, in string arrayIndexArgumentName)
        {
            ThrowIfNull(array, nameof(array));

            ThrowIfMultidimensionalArray(array, arrayArgumentName);

            ThrowIfIndexIsLowerThanZero(arrayIndex, arrayIndexArgumentName);

            //if (array.GetLowerBound(0) != 0)

            //    throw GetArrayHasNonZeroLowerBoundException(arrayArgumentName);

            ThrowIfArrayHasNotEnoughSpace(array, arrayIndex, count, arrayArgumentName);
        }

        public static void ThrowIfArrayHasNotEnoughSpace(in Array array, in int arrayIndex, in uint count, in string arrayArgumentName)
        {
            if (array.Length - arrayIndex < count)

                throw GetArrayHasNotEnoughSpaceException(arrayArgumentName);
        }

        public static void ThrowIfIndexIsLowerThanZero(in uint index, in string indexArgumentName)
        {
            if (index < 0)

                throw new IndexOutOfRangeException(indexArgumentName);
        }

        public static void ThrowOnInvalidCopyToArrayOperation(in Array array, in int arrayIndex, in uint count, in string arrayArgumentName, in string arrayIndexArgumentName)
        {
            ThrowIfNull(array, nameof(array));

            ThrowIfMultidimensionalArray(array, arrayArgumentName);

            ThrowIfIndexIsLowerThanZero(arrayIndex, arrayIndexArgumentName);

            //if (array.GetLowerBound(0) != 0)

            //    throw GetArrayHasNonZeroLowerBoundException(arrayArgumentName);

            ThrowIfArrayHasNotEnoughSpace(array, arrayIndex, count, arrayArgumentName);
        }

#if CS7
        public static void ThrowOnInvalidCopyToArrayOperation<T>(in IReadOnlyCollection<T> collection, in int arrayIndex, in int count, in string arrayArgumentName, in string arrayIndexArgumentName)
        {
            ThrowIfNull(collection, nameof(collection));

            ThrowIfIndexIsLowerThanZero(arrayIndex, arrayIndexArgumentName);

            //if (array.GetLowerBound(0) != 0)

            //    throw GetArrayHasNonZeroLowerBoundException(arrayArgumentName);

            ThrowIfArrayHasNotEnoughSpace(collection, arrayIndex, count, arrayArgumentName);
        }
#endif

        #region IndexOutOfRange throws
        public static void ThrowIfBetween(in sbyte i, in sbyte x, in sbyte y, in string argumentName)
        {
            if (i.Between(x, y))

                throw GetIndexOutOfRangeException(argumentName);
        }

        public static void ThrowIfBetween(in short i, in short x, in short y, in string argumentName)
        {
            if (i.Between(x, y))

                throw GetIndexOutOfRangeException(argumentName);
        }

        public static void ThrowIfBetween(in int i, in int x, in int y, in string argumentName)
        {
            if (i.Between(x, y))

                throw GetIndexOutOfRangeException(argumentName);
        }

        public static void ThrowIfBetween(in long i, in long x, in long y, in string argumentName)
        {
            if (i.Between(x, y))

                throw GetIndexOutOfRangeException(argumentName);
        }

        public static void ThrowIfBetween(in byte i, in byte x, in byte y, in string argumentName)
        {
            if (i.Between(x, y))

                throw GetIndexOutOfRangeException(argumentName);
        }

        public static void ThrowIfBetween(in ushort i, in ushort x, in ushort y, in string argumentName)
        {
            if (i.Between(x, y))

                throw GetIndexOutOfRangeException(argumentName);
        }

        public static void ThrowIfBetween(in uint i, in uint x, in uint y, in string argumentName)
        {
            if (i.Between(x, y))

                throw GetIndexOutOfRangeException(argumentName);
        }

        public static void ThrowIfBetween(in ulong i, in ulong x, in ulong y, in string argumentName)
        {
            if (i.Between(x, y))

                throw GetIndexOutOfRangeException(argumentName);
        }



        public static void ThrowIfNOTBetween(in sbyte i, in sbyte x, in sbyte y, in string argumentName)
        {
            if (!i.Between(x, y))

                throw GetIndexOutOfRangeException(argumentName);
        }

        public static void ThrowIfNOTBetween(in short i, in short x, in short y, in string argumentName)
        {
            if (!i.Between(x, y))

                throw GetIndexOutOfRangeException(argumentName);
        }

        public static void ThrowIfNOTBetween(in int i, in int x, in int y, in string argumentName)
        {
            if (!i.Between(x, y))

                throw GetIndexOutOfRangeException(argumentName);
        }

        public static void ThrowIfNOTBetween(in long i, in long x, in long y, in string argumentName)
        {
            if (!i.Between(x, y))

                throw GetIndexOutOfRangeException(argumentName);
        }

        public static void ThrowIfNOTBetween(in byte i, in byte x, in byte y, in string argumentName)
        {
            if (!i.Between(x, y))

                throw GetIndexOutOfRangeException(argumentName);
        }

        public static void ThrowIfNOTBetween(in ushort i, in ushort x, in ushort y, in string argumentName)
        {
            if (!i.Between(x, y))

                throw GetIndexOutOfRangeException(argumentName);
        }

        public static void ThrowIfNOTBetween(in uint i, in uint x, in uint y, in string argumentName)
        {
            if (!i.Between(x, y))

                throw GetIndexOutOfRangeException(argumentName);
        }

        public static void ThrowIfNOTBetween(in ulong i, in ulong x, in ulong y, in string argumentName)
        {
            if (!i.Between(x, y))

                throw GetIndexOutOfRangeException(argumentName);
        }
        #endregion

        #region String Throws
        private static string GetNullEmptyOrWhiteSpaceStringFormattedExceptionMessage(in string value) => string.Format(Culture, StringIsNullEmptyOrWhiteSpace, value ?? string.Empty);

        public static InvalidOperationException GetNullEmptyOrWhiteSpaceStringException(in string value) => new InvalidOperationException(GetNullEmptyOrWhiteSpaceStringFormattedExceptionMessage(value));

#if CS5
        public static void ThrowIfNullEmptyOrWhiteSpace(in string value)
        {
            if (IsNullEmptyOrWhiteSpace(value))

                throw GetNullEmptyOrWhiteSpaceStringException(value);
        }

        public static void ThrowIfNullEmptyOrWhiteSpace(in string value, in string argumentName)
        {
            if (IsNullEmptyOrWhiteSpace(value))

                throw GetNullEmptyOrWhiteSpaceStringException(GetNullEmptyOrWhiteSpaceStringFormattedExceptionMessage(value), argumentName);
        }
#endif

        public static ArgumentException GetNullEmptyOrWhiteSpaceStringException(in string value, in string argumentName) => new ArgumentException(value, argumentName);
        #endregion



        public static IndexOutOfRangeException GetIndexOutOfRangeException(string argumentName) => new
#if !CS9
            IndexOutOfRangeException
#endif
            ($"{argumentName} is out of range.");

        /// <summary>
        /// Returns an <see cref="ArgumentNullException"/> for a given argument name.
        /// </summary>
        /// <param name="argumentName">The name of the <see langword="null"/> argument.</param>
        /// <returns>An <see cref="ArgumentNullException"/> with the given argument name.</returns>
        public static ArgumentNullException GetArgumentNullException(in string argumentName) => new ArgumentNullException(argumentName);

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if a given object is null.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="obj"/>. This must be a class type.</typeparam>
        /// <param name="obj">The object to check.</param>
        /// <param name="argumentName">The argument name for the <see cref="ArgumentNullException"/> that is thrown.</param>
        public static void ThrowIfNull<T>(in T obj, in string argumentName) where T : class
        {
            if (obj is null)

                throw GetArgumentNullException(argumentName);
        }

        /// <summary>
        /// Returns <paramref name="obj"/> if it is not null, otherwise throws the <see cref="ArgumentNullException"/> that is returned by the <see cref="GetArgumentNullException(in string)"/> method.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <param name="argumentName">The argument name for the <see cref="ArgumentNullException"/> that is thrown.</param>
        public static object GetOrThrowIfNull(in object obj, in string argumentName) => GetOrThrowIfNull<object>(obj, argumentName);

        public static object GetOrThrowIfNull(in object obj, in string argumentName, in Func func) => GetOrThrowIfNull(obj, argumentName, func.AsObjectFunc());

        /// <summary>
        /// Returns <paramref name="obj"/> if it is not null, otherwise throws the <see cref="ArgumentNullException"/> that is returned by the <see cref="GetArgumentNullException(in string)"/> method.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="obj"/>. This must be a class type.</typeparam>
        /// <param name="obj">The object to check.</param>
        /// <param name="argumentName">The argument name for the <see cref="ArgumentNullException"/> that is thrown.</param>
        public static T GetOrThrowIfNull<T>(in T obj, in string argumentName)
#if !WinCopies3
            where T : class
#endif
            => GetOrThrowIfNull(obj, argumentName, Delegates.Self);

        public static TOut GetOrThrowIfNull<TIn, TOut>(in TIn obj, in string argumentName, in Func<TOut> func) => obj == null ? throw GetArgumentNullException(argumentName) : GetOrThrowIfNull(func, nameof(func))();

        private static TOut GetOrThrowIfNull<TIn, TOut>(in TIn obj, in string argumentName, in Func<TIn, TOut> func) => (func ?? throw GetArgumentNullException(nameof(func)))(obj
#if CS8
            ??
#else
            == null ?
#endif
            throw GetArgumentNullException(argumentName)
#if !CS8
            : obj
#endif
            );

        public static T
#if CS9
            ?
#endif
            TryGetIfTypeOrThrowIfNull<T>(in object obj, in string argumentName) where T : class => GetOrThrowIfNull(obj, argumentName) is T _obj ? _obj : null;

        public static T GetIfType<T>(in object obj, in string argumentName) where T : class => TryGetIfTypeOrThrowIfNull<T>(obj, argumentName) ?? throw new ArgumentException($"{argumentName} must be {typeof(T)}");

        public static bool TryGetIfTypeOrDefault<T>(in object obj, out T
#if CS9
            ?
#endif
            result)
        {
            if (obj is T _obj)
            {
                result = _obj;

                return true;
            }

            result = default;

            return false;
        }

        public static T
#if CS9
            ?
#endif
            GetIfTypeOrDefault<T>(in object obj) => obj is T _obj ? _obj : default;

        /// <summary>
        /// Returns an <see cref="ArgumentException"/> for the given object and argument name.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <param name="objTypeName">The type name of the object of the exception.</param>
        /// <param name="argumentName">The argument name for the <see cref="Exception"/> that is returned.</param>
        /// <returns>An <see cref="ArgumentException"/> with the given argument name.</returns>
        public static Exception GetExceptionForInvalidType<T>(in string objTypeName, in string argumentName) => new ArgumentException($"{argumentName} must be an instance of {typeof(T)}. {argumentName} is an instance of {objTypeName}", argumentName);

        /// <summary>
        /// Returns an <see cref="ArgumentException"/> for the given object and argument name.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <param name="objType">The type of the object of the exception.</param>
        /// <param name="argumentName">The argument name for the <see cref="Exception"/> that is returned.</param>
        /// <returns>An <see cref="ArgumentException"/> with the given argument name.</returns>
        public static Exception GetExceptionForInvalidType<T>(in Type objType, in string argumentName) => new ArgumentException($"{argumentName} must be an instance of {typeof(T)}. {argumentName} is an instance of {objType}", argumentName);

        /// <summary>
        /// Returns an <see cref="ArgumentException"/> for the given object and argument name.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <param name="objTypeName">The type name of the object of the exception.</param>
        /// <param name="argumentName">The argument name for the <see cref="Exception"/> that is returned.</param>
        /// <returns>An <see cref="ArgumentException"/> with the given argument name.</returns>
        public static Exception GetExceptionForInvalidType(in string objTypeName, in string argumentName, in Type t) => new ArgumentException($"{argumentName} must be an instance of {t}. {argumentName} is an instance of {objTypeName}", argumentName);

        /// <summary>
        /// Returns an <see cref="ArgumentException"/> for the given object and argument name.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <param name="objType">The type of the object of the exception.</param>
        /// <param name="argumentName">The argument name for the <see cref="Exception"/> that is returned.</param>
        /// <returns>An <see cref="ArgumentException"/> with the given argument name.</returns>
        public static Exception GetExceptionForInvalidType(in Type objType, in string argumentName, in Type t) => new ArgumentException($"{argumentName} must be an instance of {t}. {argumentName} is an instance of {objType}", argumentName);

        /// <summary>
        /// If <paramref name="obj"/> is not <typeparamref name="T"/>, throws the exception that is returned by the <see cref="GetExceptionForInvalidType{T}(in string, in string)"/> method.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <param name="obj">The object to check.</param>
        /// <param name="argumentName">The argument name for the <see cref="Exception"/> that is thrown.</param>
        public static void ThrowIfNotType<T>(in object obj, in string argumentName) where T : struct
        {
            if (
#if !CS8
                !(
#endif
                obj is
#if CS8        
                not
#endif
                T
#if !CS8
                )
#endif
                )

                throw GetExceptionForInvalidType<T>(obj.GetType().ToString(), argumentName);
        }

        /// <summary>
        /// If <paramref name="obj"/> is not <typeparamref name="T"/>, throws the exception that is returned by the <see cref="GetExceptionForInvalidType{T}(in string, in string)"/> method.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <param name="obj">The object to check.</param>
        /// <param name="argumentName">The argument name for the <see cref="Exception"/> that is thrown.</param>
        public static void ThrowIfNotTypeOrNull<T>(in object obj, in string argumentName) where T : class
        {
            if (obj is null)

                throw GetArgumentNullException(argumentName);

            else if (
#if !CS9
                !(
#endif
                obj is
#if CS9
                not
#endif
                T
#if !CS9
                )
#endif
                )

                throw GetExceptionForInvalidType<T>(obj.GetType().ToString(), argumentName);
        }

        /// <summary>
        /// Returns a given object when it is an instance of a given type, otherwise throws an <see cref="ArgumentException"/> with a given argument name.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <param name="obj">The object to check.</param>
        /// <param name="argumentName">The argument name for the <see cref="ArgumentException"/>.</param>
        /// <returns><paramref name="obj"/> when it is an instance of <typeparamref name="T"/>, otherwise throws an <see cref="ArgumentException"/> with <paramref name="argumentName"/> for the argument name.</returns>
        public static T GetOrThrowIfNotType<T>(in object obj, in string argumentName) where T : struct => obj is T _obj ? _obj : throw GetExceptionForInvalidType<T>(obj.GetType().ToString(), argumentName);

        /// <summary>
        /// Returns a given object when it is an instance of a given type, otherwise throws an <see cref="ArgumentNullException"/> if <paramref name="obj"/> is <see langword="null"/> or an <see cref="ArgumentException"/> with a given argument name otherwise.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <param name="obj">The object to check.</param>
        /// <param name="argumentName">The argument name for the <see cref="ArgumentException"/>.</param>
        /// <returns><paramref name="obj"/> when it is an instance of <typeparamref name="T"/>, otherwise throws an <see cref="ArgumentNullException"/> if <paramref name="obj"/> is <see langword="null"/> or an <see cref="ArgumentException"/> with <paramref name="argumentName"/> for the argument name otherwise.</returns>
        public static T GetOrThrowIfNotTypeOrNull<T>(in object obj, in string argumentName) where T : class => (obj ?? throw GetArgumentNullException(argumentName)) is T _obj ? _obj : throw GetExceptionForInvalidType<T>(obj.GetType().ToString(), argumentName);

        public static InvalidOperationException GetExceptionForDispose(in string objectName, in bool forDisposing) => forDisposing
            ? new ObjectDisposingException(objectName)
            :
#if !CS9
            (InvalidOperationException)
#endif
            new ObjectDisposedException(objectName, "The current object or value is disposed.");

        public static InvalidOperationException GetExceptionForDispose(in bool forDisposing) => new InvalidOperationException($"The current object or value is {(forDisposing ? "disposing" : "disposed")}.");

        internal static void ThrowIfDisposedInternal(DotNetFix.IDisposable obj)
        {
            if (obj.IsDisposed)

                throw GetExceptionForDispose(false);
        }

        public static void ThrowIfDisposed(DotNetFix.IDisposable obj) => ThrowIfDisposedInternal(obj ?? throw GetArgumentNullException(nameof(obj)));

        internal static void ThrowIfDisposingInternal(IDisposable obj)
        {
            if (obj.IsDisposing)

                throw GetExceptionForDispose(true);
        }

        public static void ThrowIfDisposing(IDisposable obj) => ThrowIfDisposingInternal(obj ?? throw GetArgumentNullException(nameof(obj)));

        public static void ThrowIfDisposingOrDisposed(IDisposable obj)
        {
            ThrowIfDisposedInternal(obj ?? throw GetArgumentNullException(nameof(obj)));

            ThrowIfDisposingInternal(obj);
        }
#else
        public static void ThrowIfEnumeratorNotStartedOrDisposedException(in WinCopies.Collections.IDisposableEnumeratorInfo enumerator)
        {
            if (Extensions.IsEnumeratorNotStartedOrDisposed(enumerator))

                throw GetEnumeratorNotStartedOrDisposedException();
        }

        [Obsolete("This method has been replaced by GetArrayWithMoreThanOneDimensionException and ThrowIfArrayWithMoreThanOneDimension.")]
        public static void ThrowArrayWithMoreThanOneDimensionException(in string paramName) => throw GetArrayWithMoreThanOneDimensionException(paramName);

        [Obsolete("Throw the result of GetDeclaringTypesNotCorrespondException instead of calling this method.")]
        public static void ThrowDeclaringTypesNotCorrespondException(string propertyName, string methodName) => throw GetDeclaringTypesNotCorrespondException(propertyName, methodName);

        [Obsolete("Throw the result of GetMoreThanOneOccurencesWereFoundException instead of calling this method.")]
        public static void ThrowMoreThanOneOccurencesWereFoundException() => throw GetMoreThanOneOccurencesWereFoundException();

        [Obsolete("This method has been replaced by GetOneOrMoreKeyIsNullException.")]
        public static ArgumentException GetOneOrMoreKeyIsNull() => GetOneOrMoreKeyIsNullException();

        [Obsolete("Throw the result of GetOneOrMoreKeyIsNullException instead of calling this method.")]
        public static void ThrowOneOrMoreKeyIsNull() => throw GetOneOrMoreKeyIsNullException();
#endif
    }
}
