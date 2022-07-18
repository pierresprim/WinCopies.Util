using System;
using System.Globalization;
using System.Text;

using WinCopies.Util;

using static WinCopies.
#if WinCopies3
ThrowHelper;

using static WinCopies.UtilHelpers;
#else
Util.Util;
#endif


namespace WinCopies
{
    public static class StringExtensions
    {
        public static string TryPrefix(this string value, in string prefix) => value.StartsWith(prefix) ? value : prefix + value;
        public static string TrySuffix(this string value, in string suffix) => value.EndsWith(suffix) ? value : value + suffix;

        private static string TryPrefix(this string value, in string prefix, in ActionIn<StringBuilder> action)
        {
            var stringBuilder = new StringBuilder();

            void append(in string text) => stringBuilder.Append(text);

            append(prefix);
            action(stringBuilder);

            string toString() => stringBuilder.ToString();

            if (value.StartsWith(toString()))

                return value;

            append(value);

            return toString();
        }

        public static string TryPrefix(this string value, in string prefix, string separator) => value.TryPrefix(prefix, (in StringBuilder stringBuilder) => stringBuilder.Append(separator));
        public static string TryPrefix(this string value, in string prefix, char separator) => value.TryPrefix(prefix, (in StringBuilder stringBuilder) => stringBuilder.Append(separator));

        public static string TrySuffix(this string value, string suffix, in string separator) => value.EndsWith(suffix = separator + suffix) ? value : value + suffix;
        public static string TrySuffix(this string value, string suffix, in char separator) => value.EndsWith(suffix = separator + suffix) ? value : value + suffix;

        public static char[] ToCharArray(this string s, in int start) => (s ?? throw GetArgumentNullException(nameof(s))).ToCharArray(start, s.Length - start);

        public static char[] ToCharArrayL(this string s, in int length) => (s ?? throw GetArgumentNullException(nameof(s))).ToCharArray(0, length > s.Length ? s.Length : length);

        public static
#if CS5
            (string left, string right)
#else
            ValueTuple<string, string>
#endif
            Split(this string s, in int index)
        {
            return
#if !CS5
                new ValueTuple<string, string>
#endif
                (new string((s ?? throw GetArgumentNullException(nameof(s))).ToCharArrayL(index)), new string(s.ToCharArray(index + 1)));
        }

#if CS8
        public static ReadOnlySpanTuple<char, char> SplitAsReadOnlySpan(this string s, in int index)
        {
            char[] array = (s ?? throw GetArgumentNullException(nameof(s))).ToCharArray();

            return new
#if !CS9
            ReadOnlySpanTuple<char, char>
#endif
            (GetReadOnlySpanL(array, index), GetReadOnlySpan(array, index + 1));
        }
#endif

        public static string Surround(this string
#if CS8
                ?
#endif
                value, in char left, in char right) => Surround(value, left.ToString(), right.ToString());
        public static string Surround(this string
#if CS8
                ?
#endif
                value, in char decorator) => Surround(value, decorator.ToString());
        public static string Surround(this string
#if CS8
                ?
#endif
                value, in string
#if CS8
                ?
#endif
                left, in string
#if CS8
                ?
#endif
                right) => $"{left}{value}{right}";
        public static string Surround(this string
#if CS8
                ?
#endif
                value, in string
#if CS8
                ?
#endif
                decorator) => Surround(value, decorator, decorator);

        private static string
#if CS8
                ?
#endif
                FirstCharTo(this string
#if CS8
                ?
#endif
                value, Converter<char, char> charConverter, Converter<string, string> stringConverter) => value == null ? null : value.Length > 1 ? charConverter(value[0]) + value
#if CS8
            [1..]
#else
            .Substring(1)
#endif
            : stringConverter(value);
        public static string
#if CS8
                ?
#endif
                FirstCharToLower(this string
#if CS8
                ?
#endif
                value) => value.FirstCharTo(c => char.ToLower(c), s => s.ToLower());
        public static string
#if CS8
                ?
#endif
                FirstCharToLowerInvariant(this string
#if CS8
                ?
#endif
                value) => value.FirstCharTo(c => char.ToLowerInvariant(c), s => s.ToLowerInvariant());
        public static string
#if CS8
                ?
#endif
                FirstCharToLower(this string
#if CS8
                ?
#endif
                value, CultureInfo culture) => value.FirstCharTo(c => char.ToLower(c, culture), s => s.ToLower(culture));

        public static string
#if CS8
                ?
#endif
                FirstCharToUpper(this string
#if CS8
                ?
#endif
                value) => value.FirstCharTo(c => char.ToLower(c), s => s.ToUpper());
        public static string
#if CS8
                ?
#endif
                FirstCharToUpperInvariant(this string
#if CS8
                ?
#endif
                value) => value.FirstCharTo(c => char.ToUpperInvariant(c), s => s.ToUpperInvariant());
        public static string
#if CS8
                ?
#endif
                FirstCharToUpper(this string
#if CS8
                ?
#endif
                value, CultureInfo culture) => value.FirstCharTo(c => char.ToUpper(c, culture), s => s.ToUpper(culture));

        private static string FirstCharOfEachWordToUpper(this string s, in Converter<char, char> converter, params char[] separators)
        {
            string[] text = s.Split(separators);

            char[] c = new char[s.Length];

            int _j;

            string _text;

            for (int i = 0, j = 0; i < text.Length; i++)
            {
                _text = text[i];

                c[j] = converter(_text[0]);

                for (j++, _j = 1; _j < _text.Length; j++, _j++)

                    c[j] = _text[_j];
            }

            return new string(c);
        }
        public static string FirstCharOfEachWordToUpper(this string s, params char[] separators) => s.FirstCharOfEachWordToUpper(c => char.ToUpper(c), separators);
        public static string FirstCharOfEachWordToUpperInvariant(this string s, params char[] separators) => s.FirstCharOfEachWordToUpper(c => char.ToUpperInvariant(c), separators);
        public static string FirstCharOfEachWordToUpper(this string s, CultureInfo culture, params char[] separators) => s.FirstCharOfEachWordToUpper(c => char.ToUpper(c, culture), separators);

        public static string Reverse(this string s)
        {
            char[] c = new char[s.Length];

            for (int i = 0; i < s.Length; i++)

                c[i] = s[s.Length - i - 1];

            return new string(c);
        }
#if CS5
#if !CS8
        public static StringBuilder AppendJoin(this StringBuilder stringBuilder, in string s, in System.Collections.Generic.IEnumerable<string> values)
        {
            if (values is System.Collections.Generic.IReadOnlyList<string> collection)

                return stringBuilder.AppendJoin(s, collection);

            foreach (string value in values)

                _ = stringBuilder.Append(value).Append(s);

            _ = stringBuilder.Remove(stringBuilder.Length - s.Length, s.Length);

            return stringBuilder;
        }

        public static StringBuilder AppendJoin(this StringBuilder stringBuilder, in char c, in System.Collections.Generic.IEnumerable<string> values) => stringBuilder.AppendJoin(c.ToString(), values);

        public static StringBuilder AppendJoin(this StringBuilder stringBuilder, in char c, in System.Collections.Generic.IReadOnlyList<string> values) => stringBuilder.AppendJoin(c.ToString(), values);

        public static StringBuilder AppendJoin(this StringBuilder stringBuilder, in string s, in System.Collections.Generic.IReadOnlyList<string> values)
        {
            for (int i = 0; i < values.Count - 1; i++)

                _ = stringBuilder.Append(values[i]).Append(s);

            _ = stringBuilder.Append(values.Last());

            return stringBuilder;
        }

        public static StringBuilder AppendJoin(this StringBuilder stringBuilder, in char c, params string[] values) => stringBuilder.AppendJoin(c.ToString(), values);

        public static StringBuilder AppendJoin(this StringBuilder stringBuilder, in string s, params string[] values) => stringBuilder.AppendJoin(s, (System.Collections.Generic.IReadOnlyList<string>)values);
#endif

        public static string Format(this string value, in char except, in Func<string, string> func)
        {
            string[] split = (value ?? throw GetArgumentNullException(nameof(value))).Split(except);

            for (int i = 0; i < split.Length; i++)

                split[i] = func(split[i]);

            var result = new StringBuilder();

            return result.AppendJoin(except, split).ToString();
        }
#endif
    }
}
