using System.Windows.Input;

namespace WinCopies.Commands
{
    public static class NotificationIconCommands
    {
        public static RoutedUICommand ShowWindow { get; } = GetRoutedUICommand(string.Empty, "ShowWindow");

        public static RoutedUICommand Close { get; } = GetRoutedUICommand("Quit", "Quit");

        private static RoutedUICommand GetRoutedUICommand(in string text, in string name) => new
#if !CS9
            RoutedUICommand
#endif
            (text, name, typeof(NotificationIconCommands));
    }
}
