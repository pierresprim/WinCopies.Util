#if CS5
using static WinCopies.
#if WinCopies3
    UtilHelpers;
#else
    Util.Util;
#endif

namespace WinCopies
{
    public static class Regex
    {
        public const string LikeSnippet = ".*?";

        public static string FromPattern(in string filter, in char c, in string pattern) => filter == null
                ? ""
                : $"^{filter.Format(c, value => System.Text.RegularExpressions.Regex.Escape(value)).Replace(c.ToString(), pattern)}$";

        public static string FromPathFilter(in string filter) => FromPattern(filter, PathFilterChar, LikeSnippet);

        public static string FromLikeStatement(in string filter) => FromPattern(filter, LikeStatementChar, LikeSnippet);
    }
}
#endif
