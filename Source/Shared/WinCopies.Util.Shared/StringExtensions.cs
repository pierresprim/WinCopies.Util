using System;
using System.Text;

using WinCopies.Util;

using static WinCopies.
#if WinCopies3
    ThrowHelper;
#else
    Util.Util;
#endif

namespace WinCopies
{
    public static class StringExtensions
    {
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
