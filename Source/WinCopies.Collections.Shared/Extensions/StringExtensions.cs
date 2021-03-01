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

#if WinCopies3

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

using WinCopies.Collections.DotNetFix.Generic;

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections
{
    public static class StringExtensions
    {
        #region Split

        private static void Split(this string s, in bool skipEmptyValues, in StringBuilder stringBuilder, in Action<string> action, params char[] separators)
        {
            ThrowIfNull(s, nameof(s));
            ThrowIfNull(stringBuilder, nameof(stringBuilder));
            ThrowIfNull(separators, nameof(separators));

            if (separators.Length == 0)

                throw new ArgumentException($"{nameof(separators)} does not contain values.");

            Debug.Assert(action != null, $"{nameof(action)} must be not null.");

            Predicate<char> getPredicate()
            {
                Predicate<char> predicate;

                if (separators.Length == 1)

                    predicate = __c => __c == separators[0];

                else

                    predicate = __c => separators.Contains(__c);

                return predicate;
            }

            if (skipEmptyValues)

                if (s.Length == 0)

                    return;

                else if (s.Length == 1)

                    if ((separators.Length == 1 && s[0] == separators[0]) || separators.Contains(s[0]))

                        return;

                    else

                        action(s);

                else
                {
                    Predicate<char> predicate = getPredicate();

                    foreach (char _c in s)

                        if (predicate(_c) && stringBuilder.Length > 0)
                        {
                            action(stringBuilder.ToString());

                            _ = stringBuilder.Clear();
                        }

                        else

                            _ = stringBuilder.Append(_c);

                    if (stringBuilder.Length > 0)

                        action(stringBuilder.ToString());
                }

            else if (s.Length == 0)

                action("");

            else if (s.Length == 1)

                if ((separators.Length == 1 && s[0] == separators[0]) || separators.Contains(s[0]))
                {
                    action("");

                    action("");
                }

                else

                    action(s);

            else
            {
                Predicate<char> predicate = getPredicate();

                foreach (char _c in s)

                    if (predicate(_c))

                        if (stringBuilder.Length == 0)

                            action("");

                        else
                        {
                            action(stringBuilder.ToString());

                            _ = stringBuilder.Clear();
                        }

                    else

                        _ = stringBuilder.Append(_c);

                if (stringBuilder.Length > 0)

                    action(stringBuilder.ToString());
            }
        }

        public static WinCopies.Collections.DotNetFix.Generic.IQueue<string> SplitToQueue(this string s, in bool skipEmptyValues, params char[] separators)
        {
            var queue = new WinCopies.Collections.DotNetFix.Generic.Queue<string>();

            SplitToQueue(s, skipEmptyValues, new StringBuilder(), queue, separators);

            return queue;
        }

        public static void SplitToQueue(this string s, in bool skipEmptyValues, in StringBuilder stringBuilder, WinCopies.Collections.DotNetFix.Generic.IQueue<string> queue, params char[] separators)
        {
            ThrowIfNull(queue, nameof(queue));

            Split(s, skipEmptyValues, stringBuilder, _s => queue.Enqueue(_s), separators);
        }

        public static WinCopies.Collections.DotNetFix.Generic.IStack<string> SplitToStack(this string s, in bool splitEmptyValues, params char[] separators)
        {
            var stack = new WinCopies.Collections.DotNetFix.Generic.Stack<string>();

            SplitToStack(s, splitEmptyValues, new StringBuilder(), stack, separators);

            return stack;
        }

        public static void SplitToStack(this string s, in bool splitEmptyValues, in StringBuilder stringBuilder, WinCopies.Collections.DotNetFix.Generic.IStack<string> stack, params char[] separators)
        {
            ThrowIfNull(stack, nameof(stack));

            Split(s, splitEmptyValues, stringBuilder, _s => stack.Push(_s), separators);
        }

        public static System.Collections.Generic.LinkedList<string> SplitToLinkedList(this string s, in bool splitEmptyValues, params char[] separators)
        {
            var list = new System.Collections.Generic.LinkedList<string>();

            SplitToLinkedList(s, splitEmptyValues, new StringBuilder(), list, separators);

            return list;
        }

        public static void SplitToLinkedList(this string s, in bool splitEmptyValues, in StringBuilder stringBuilder, System.Collections.Generic.LinkedList<string> list, params char[] separators)
        {
            ThrowIfNull(list, nameof(list));

            Split(s, splitEmptyValues, stringBuilder, _s => list.AddLast(_s), separators);
        }

#if CS7
        public static ILinkedList<string> SplitToILinkedList(this string s, in bool splitEmptyValues, params char[] separators)
        {
            var list = new DotNetFix.Generic.LinkedList<string>();

            SplitToILinkedList(s, splitEmptyValues, new StringBuilder(), list, separators);

            return list;
        }

        public static void SplitToILinkedList(this string s, in bool splitEmptyValues, in StringBuilder stringBuilder, ILinkedList<string> list, params char[] separators)
        {
            ThrowIfNull(list, nameof(list));

            Split(s, splitEmptyValues, stringBuilder, _s => list.AddLast(_s), separators);
        }
#endif

        #endregion

        public static string Join(this System.Collections.Generic.IEnumerable<string> enumerable, in bool keepEmptyValues, params char[] join) => Join(enumerable, keepEmptyValues, new string(join));

        public static string Join(this System.Collections.Generic.IEnumerable<string> enumerable, in bool keepEmptyValues, in string join, StringBuilder stringBuilder = null)
        {
            System.Collections.Generic.IEnumerator<string> enumerator = (enumerable ?? throw GetArgumentNullException(nameof(enumerable))).GetEnumerator();

#if CS8
            stringBuilder ??= new StringBuilder();
#else
            if (stringBuilder == null)

                stringBuilder = new StringBuilder();
#endif

            try
            {
                void append() => _ = stringBuilder.Append(enumerator.Current);

                bool moveNext() => enumerator.MoveNext();

                if (moveNext())

                    append();

                while (moveNext() && (keepEmptyValues || enumerator.Current.Length > 0))
                {
                    _ = stringBuilder.Append(join);

                    append();
                }
            }
            finally
            {
                enumerator.Dispose();
            }

            return stringBuilder.ToString();
        }

        // todo: add other methods and overloads for StringComparison, IEqualityComparer<char>, Comparer<char>, Comparison<char>, ignore case and CultureInfo parameters

#if !WinCopies3

        [Obsolete("This method has been replaced by the Contains(this string, string, IEqualityComparer<char>) method.")]
        public static bool Contains(this string s, System.Collections.Generic.IEqualityComparer<char> comparer, string value) => s.Contains(value, comparer);

#endif

        public static bool Contains(this string s, string value, System.Collections.Generic.IEqualityComparer<char> comparer)
        {
            bool contains(ref int i)
            {
                for (int j = 0; j < value.Length; j++)

                    if (!comparer.Equals(s[i + j], value[j]))

                        return false;

                return true;
            }

            for (int i = 0; i < s.Length; i++)
            {
                if (value.Length > s.Length - i)

                    return false;

                if (contains(ref i))

                    return true;
            }

            return false;
        }

        // todo: To replace by arrays-common methods

        public static bool Contains(this string s, char value, out int index)
        {
            for (int i = 0; i < s.Length; i++)

                if (s[i] == value)
                {
                    index = i;

                    return true;
                }

            index = default;

            return false;
        }

        public static bool Contains(this string s, string value, out int index)
        {
            bool contains(ref int i)
            {
                for (int j = 0; j < value.Length; j++)

                    if (s[i + j] != value[j])

                        return false;

                return true;
            }

            for (int i = 0; i < s.Length; i++)
            {

                if (value.Length > s.Length - i)
                {
                    index = -1;

                    return false;
                }

                if (contains(ref i))
                {
                    index = i;

                    return true;
                }
            }

            index = -1;

            return false;
        }

        public static bool Contains(this string s, string value, System.Collections.Generic.IEqualityComparer<char> comparer, out int index)
        {
            bool contains(ref int i)
            {
                for (int j = 0; j < value.Length; j++)

                    if (!comparer.Equals(s[i + j], value[j]))

                        return false;

                return true;
            }

            for (int i = 0; i < s.Length; i++)

            {
                if (value.Length > s.Length - i)
                {
                    index = -1;

                    return false;
                }

                if (contains(ref i))
                {
                    index = i;

                    return true;
                }
            }

            index = -1;

            return false;
        }

#if !WinCopies3 || !CS8
        public static bool StartsWith(this string s, char value) => s[0] == value;
        
        public static bool EndsWith(this string s, char value) => s[s.Length - 1] == value;
#endif

        public static bool StartsWithAND(this string s, params char[] values) => s.StartsWith(new string(values));

        public static StringBuilder Append(this StringBuilder stringBuilder, params string[] values)
        {
            foreach (string value in values)

                _ = stringBuilder.Append(value);

            return stringBuilder;
        }

        public static bool StartsWithAND(this string s, params string[] values)
        {
            var sb = new StringBuilder();

            _ = sb.Append(values);

            return s.StartsWith(sb.ToString());
        }

        public static bool StartsWithOR(this string s, params char[] values)
        {
            foreach (char value in values)

                if (s.StartsWith(value))

                    return true;

            return false;
        }

        public static bool StartsWithOR(this string s, params string[] values)
        {
            foreach (string value in values)

                if (s.StartsWith(value))

                    return true;

            return false;
        }

        public static bool EndsWithAND(this string s, params char[] values) => s.EndsWith(new string(values));

        public static bool EndsWithAND(this string s, params string[] values)
        {
            var sb = new StringBuilder();

            _ = sb.Append(values);

            return false;
        }

        public static bool EndsWithOR(this string s, params char[] values)
        {
            foreach (char value in values)

                if (s.EndsWith(value))

                    return true;

            return false;
        }

        public static bool EndsWithOR(this string s, params string[] values)
        {
            foreach (string value in values)

                if (s.EndsWith(value))

                    return true;

            return false;
        }

        public static string RemoveAccents(this string s)
        {
            var stringBuilder = new StringBuilder();

            s = s.Normalize(NormalizationForm.FormD);

            foreach (char c in s)

                if (char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)

                    _ = stringBuilder.Append(c);

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}

#endif
