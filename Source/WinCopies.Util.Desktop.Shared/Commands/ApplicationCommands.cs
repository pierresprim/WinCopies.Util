/* Copyright © Pierre Sprimont, 2019
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

using System.Windows.Input;

#if !WinCopies3
using System;

namespace WinCopies.Util.Commands
#else
namespace WinCopies.Commands
#endif
{
    /// <summary>
    /// Provides some standard commands for application commands.
    /// </summary>
    public static class ApplicationCommands
    {
        private static RoutedUICommand GetCommand(in string text, in string name) => new
#if !CS9
            RoutedUICommand
#endif
            (text, name, typeof(ApplicationCommands));

        private static RoutedUICommand GetCommand(in string text, in string name, in InputGestureCollection inputGestures) => new
#if !CS9
            RoutedUICommand
#endif
            (text, name, typeof(ApplicationCommands), inputGestures);

        public static RoutedUICommand OpenOrLaunch { get; } = GetCommand("Open selected items.", nameof(OpenOrLaunch));

        public static RoutedUICommand OpenInNewTab { get; } = GetCommand("Open in new tab", nameof(OpenInNewTab));

        public static RoutedUICommand OpenInNewWindow { get; } = GetCommand("Open in new window", nameof(OpenInNewWindow));

        /// <summary>
        /// Gets the <b>NewTab</b> command.
        /// </summary>
        public static RoutedUICommand NewTab { get; } = GetCommand(
#if !WinCopies3
            WinCopies.Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.NewTab, nameof(NewTab), new InputGestureCollection() { new KeyGesture(Key.T, ModifierKeys.Control) });

        /// <summary>
        /// Gets the <b>NewWindow</b> command.
        /// </summary>
        public static RoutedUICommand NewWindow { get; } = GetCommand(
#if !WinCopies3
            WinCopies.Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.NewWindow, nameof(NewWindow), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Alt) });

        /// <summary>
        /// Gets the <b>NewWindowInNewInstance</b> command.
        /// </summary>
        public static RoutedUICommand NewWindowInNewInstance { get; } = GetCommand(
#if !WinCopies3
            WinCopies.Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.NewWindowInNewInstance, nameof(NewWindowInNewInstance), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Control | ModifierKeys.Shift) });

        /// <summary>
        /// Gets the <b>CloseTab</b> command.
        /// </summary>
        public static RoutedUICommand CloseTab { get; } = GetCommand(
#if !WinCopies3
            WinCopies.Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.CloseTab, nameof(CloseTab), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Control) });

        public static RoutedUICommand CloseTabsToTheLeftOrRight { get; } = new
#if !CS9
            RoutedUICommand
#endif
            (Desktop.Resources.Commands.WPF.ApplicationCommands.CloseTabsToTheLeftOrRight, nameof(CloseTabsToTheLeftOrRight), typeof(ApplicationCommands));

        public static RoutedUICommand CloseOtherTabs { get; } = GetCommand(
#if !WinCopies3
            WinCopies.Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.CloseOtherTabs, nameof(CloseOtherTabs), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Control | ModifierKeys.Alt) });

        /// <summary>
        /// Gets the <b>CloseAllTabs</b> command.
        /// </summary>
        public static RoutedUICommand CloseAllTabs { get; } = GetCommand(
#if !WinCopies3
            WinCopies.Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.CloseAllTabs, nameof(CloseAllTabs), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Control | ModifierKeys.Shift) });

#if !WinCopies3
        /// <summary>
        /// Gets the <b>CloseWindow</b> command.
        /// </summary>
        [Obsolete("This command is obsolete and will be removed in later versions. Please use the System.Windows.Input.ApplicationCommands.Close command instead.")]
        public static RoutedUICommand CloseWindow { get; } = GetCommand(Desktop.Resources.Commands.WPF.ApplicationCommands.CloseWindow, nameof(CloseWindow), new InputGestureCollection() { new KeyGesture(Key.F4, ModifierKeys.Alt) });
#endif

        public static RoutedUICommand Quit { get; } = GetCommand(
#if !WinCopies3
            WinCopies.Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.Quit, nameof(Quit), new InputGestureCollection() { new KeyGesture(Key.Q, ModifierKeys.Control) });

        /// <summary>
        /// Gets the <b>DeselectAll</b> command.
        /// </summary>
        public static RoutedUICommand DeselectAll { get; } = GetCommand(
#if !WinCopies3
            WinCopies.Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.DeselectAll, nameof(DeselectAll));

        /// <summary>
        /// Gets the <b>ReverseSelection</b> command.
        /// </summary>
        public static RoutedUICommand ReverseSelection { get; } = GetCommand(
#if !WinCopies3
            WinCopies.Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.ReverseSelection, nameof(ReverseSelection));

        public static RoutedUICommand Reset { get; } = GetCommand(
#if !WinCopies3
            WinCopies.Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.Reset, nameof(Reset));

        public static RoutedUICommand Empty { get; } = GetCommand(
#if !WinCopies3
            WinCopies.Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.Empty, nameof(Empty));

        public static RoutedUICommand DeletePermanently { get; } = GetCommand(
#if !WinCopies3
            WinCopies.Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.DeletePermanently, nameof(DeletePermanently), new InputGestureCollection() { new KeyGesture(Key.Delete, ModifierKeys.Alt) });
    }
}
