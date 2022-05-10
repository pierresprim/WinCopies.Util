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

#if CS7 && WinCopies3
using System.Text;

using WinCopies.Collections;
using WinCopies.Collections.Generic;
using WinCopies.Util;

namespace WinCopies.Extensions // To avoid name conflicts.
{
    public static class StringExtensions
    {
        public static string[] Split(this string s, in bool keepEmpty, params char[] chars)
        {
            var sb = new StringBuilder();
            var result = new ArrayBuilder<string>();

            foreach (char c in s)

                if (chars.Contains(c))
                {
                    if (!(keepEmpty && sb.Length == 0))

                        _ = result.AddLast(sb.ToString());

                    _ = sb.Clear();
                }

                else

                    _ = sb.Append(c);

            return result.ToArray();
        }

        public static (string left, string
#if CS8
            ?
#endif
            right) SplitF(this string s, char c)
        {
            for (int i = 0; i < s.Length; i++)

                if (s[i] == c)

                    return s.Split(i);

            return (s, null);
        }

        public static (string left, string
#if CS8
            ?
#endif
            right) SplitL(this string s, char c)
        {
            for (int i = s.Length - 1; i >= 0; i--)

                if (s[i] == c)

                    return s.Split(i);

            return (s, null);
        }

        public static StringCharArray AsCharArray(this string s) => new
#if !CS9
            StringCharArray
#endif
            (s);

        public static bool Validate(this string s, in string startsWith, in int skipLength, in int? length, in int? lowerBound, in int? upperBound, in string value) => s.AsCharArray().Validate(startsWith.AsCharArray(), (char x, char y) => x == y, skipLength, length, lowerBound, upperBound, value.AsCharArray());

        public static bool Validate(this string s, in char[] startsWith, in int skipLength, in int? length, in int? lowerBound, in int? upperBound, params char[] chars) => s.AsCharArray().Validate(startsWith, (char x, char y) => x == y, skipLength, length, lowerBound, upperBound, chars);

        public static bool Validate(this string s, in char startsWith, in int skipLength, in int? length, in int? lowerBound, in int? upperBound, params char[] chars) => s.AsCharArray().Validate(new char[] { startsWith }, (char x, char y) => x == y, skipLength, length, lowerBound, upperBound, chars);
    }
}
#endif
