#if !WinCopies4
using System;

namespace WinCopies.Util.IO
{
    public class Path
    {
        public static string GetAbsolutePath(string path) => Uri.IsWellFormedUriString(path, UriKind.Absolute) ? path : System.IO.Path.GetFullPath(path);
    }
}
#endif
