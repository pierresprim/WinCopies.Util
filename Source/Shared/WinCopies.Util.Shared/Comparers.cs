﻿/* Copyright © Pierre Sprimont, 2019
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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace WinCopies.Collections
{
    /// <summary>
    /// Delegate for a non-generic comparison.
    /// </summary>
    /// <param name="x">First parameter to compare.</param>
    /// <param name="y">Second parameter to compare.</param>
    /// <returns>An <see cref="int"/> which is lesser than 0 if x is lesser than y, 0 if x is equal to y and greater than 0 if x is greater than y.</returns>
    public delegate int Comparison(object x, object y);

    /// <summary>
    /// Delegate for a non-generic equality comparison.
    /// </summary>
    /// <param name="x">First parameter to compare.</param>
    /// <param name="y">Second parameter to compare.</param>
    /// <returns><see langword="true"/> if x is equal to y, otherwise <see langword="false"/>.</returns>
    public delegate bool EqualityComparison(object x, object y);

#if WinCopies3
    namespace Generic
    {
#endif
        /// <summary>
        /// Delegate for a generic equality comparison.
        /// </summary>
        /// <typeparam name="T">The type of the current delegate parameters.</typeparam>
        /// <param name="x">First parameter to compare.</param>
        /// <param name="y">Second parameter to compare.</param>
        /// <returns><see langword="true"/> if x is equal to y, otherwise <see langword="false"/>.</returns>
        public delegate bool EqualityComparison<in T>(T x, T y);

        public delegate void EqualityComparisonIn<T>(in T x, in T y);

        public interface IComparable<T>
        {
            bool LessThan(T other);
            bool LessThanOrEqualTo(T other);
            bool GreaterThan(T other);
            bool GreaterThanOrEqualTo(T other);
        }

        public interface IComparer<
#if CS5
            in
#endif
            T> : System.Collections.Generic.IComparer<T>
        {
            SortingType SortingType { get; set; }
        }

        public abstract class Comparer<T> : System.Collections.Generic.Comparer<T>, IComparer<T>
        {
            public SortingType SortingType { get; set; }

            protected abstract int CompareOverride(T x, T y);

            public sealed override int Compare(T x, T y)
            {
                int result = CompareOverride(x, y);

                return SortingType == SortingType.Ascending ? result : -result;
            }
        }

        public class Comparer2<T> : Comparer<T>
        {
            protected Comparison<T> Comparison { get; }

            public Comparer2(Comparison<T> comparison) => Comparison = comparison ?? throw
#if WinCopies3
            ThrowHelper
#else
            WinCopies.Util.Util
#endif
            .GetArgumentNullException(nameof(comparison));

            protected override int CompareOverride(T x, T y) => Comparison(x, y);
        }

        public interface IEqualityComparer<
#if CS5
            in
#endif
            T> : System.Collections.Generic.IEqualityComparer<T>
        {
            bool Equals(
#if CS8
            [AllowNull]
#endif
        T x,
#if CS8
            [AllowNull]
#endif
        object y);
        }

        public abstract class EqualityComparer<T> : System.Collections.Generic.EqualityComparer<T>, IEqualityComparer<T>
        {
            public bool Equals(
#if CS8
            [AllowNull]
#endif
         T x,
#if CS8
            [AllowNull]
#endif
         object y) => y is T _y && EqualsOverride(x, _y);

            public sealed override bool Equals(
#if CS8
            [AllowNull]
#endif
        T x,
#if CS8
            [AllowNull]
#endif
        T y) => EqualsOverride(x, y);

            protected abstract bool EqualsOverride(
#if CS8
            [AllowNull]
#endif
        T x,
#if CS8
            [AllowNull]
#endif
        T y);
        }
#if WinCopies3
    }
#endif

    public enum SortingType
    {
        Ascending,

        Descending
    }

    public interface IComparer : System.Collections.IComparer
    {
        SortingType SortingType { get; set; }
    }

    public sealed class Comparer : IComparer
    {
        public static readonly Comparer Default = new Comparer(System.Threading.Thread.CurrentThread.CurrentCulture);

        public static readonly Comparer DefaultInvariant = new Comparer(CultureInfo.InvariantCulture);

        private readonly System.Collections.Comparer _comparer;

        public SortingType SortingType { get; set; }

        public Comparer(CultureInfo cultureInfo) => _comparer = new System.Collections.Comparer(cultureInfo);

        public int Compare(object x, object y)
        {
            int result = _comparer.Compare(x, y);

            return SortingType == SortingType.Ascending ? result : -result;
        }
    }

    public sealed class CaseInsensitiveComparer : IComparer
    {
        public static CaseInsensitiveComparer Default => new CaseInsensitiveComparer(System.Threading.Thread.CurrentThread.CurrentCulture);

        public static CaseInsensitiveComparer DefaultInvariant => new CaseInsensitiveComparer(CultureInfo.InvariantCulture);

        private readonly System.Collections.CaseInsensitiveComparer _comparer;

        public SortingType SortingType { get; set; }

        public CaseInsensitiveComparer(CultureInfo cultureInfo) => _comparer = new System.Collections.CaseInsensitiveComparer(cultureInfo);

        public int Compare(object x, object y)
        {
            int result = _comparer.Compare(x, y);

            return SortingType == SortingType.Ascending ? result : -result;
        }
    }
}
