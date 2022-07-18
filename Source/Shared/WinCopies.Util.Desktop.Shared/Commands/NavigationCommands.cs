using System.Windows.Input;

namespace WinCopies.Commands
{
    public static class NavigationCommands
    {
        public static RoutedUICommand Browse { get; } = new RoutedUICommand("Browse...", nameof(Browse), typeof(NavigationCommands));

        public static RoutedUICommand BrowseToParent { get; } = new RoutedUICommand("Browse to parent", nameof(BrowseToParent), typeof(NavigationCommands), new InputGestureCollection() { new KeyGesture(Key.Up, ModifierKeys.Alt) });
    }
}
