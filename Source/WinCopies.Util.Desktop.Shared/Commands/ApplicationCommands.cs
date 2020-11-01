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

#if WinCopies2
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
        /// <summary>
        /// Gets the <b>NewTab</b> command.
        /// </summary>
        public static RoutedUICommand NewTab { get; } = new RoutedUICommand(WinCopies.
            #if WinCopies2
            Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.NewTab, nameof(NewTab), typeof(ApplicationCommands), new InputGestureCollection() { new KeyGesture(Key.T, ModifierKeys.Control) });

        /// <summary>
        /// Gets the <b>NewWindow</b> command.
        /// </summary>
        public static RoutedUICommand NewWindow { get; } = new RoutedUICommand(WinCopies.
#if WinCopies2
            Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.NewWindow, nameof(NewWindow), typeof(ApplicationCommands), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Alt) });

        /// <summary>
        /// Gets the <b>NewWindowInNewInstance</b> command.
        /// </summary>
        public static RoutedUICommand NewWindowInNewInstance { get; } = new RoutedUICommand(WinCopies.
#if WinCopies2
            Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.NewWindowInNewInstance, nameof(NewWindowInNewInstance), typeof(ApplicationCommands), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Control | ModifierKeys.Shift) });

        /// <summary>
        /// Gets the <b>CloseTab</b> command.
        /// </summary>
        public static RoutedUICommand CloseTab { get; } = new RoutedUICommand(WinCopies.
#if WinCopies2
            Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.CloseTab, nameof(CloseTab), typeof(ApplicationCommands), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Control) });

        public static RoutedUICommand CloseOtherTabs { get; } = new RoutedUICommand(WinCopies.
#if WinCopies2
            Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.CloseOtherTabs, nameof(CloseOtherTabs), typeof(ApplicationCommands), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Control | ModifierKeys.Alt) });

        /// <summary>
        /// Gets the <b>CloseAllTabs</b> command.
        /// </summary>
        public static RoutedUICommand CloseAllTabs { get; } = new RoutedUICommand(WinCopies.
#if WinCopies2
            Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.CloseAllTabs, nameof(CloseAllTabs), typeof(ApplicationCommands), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Control | ModifierKeys.Shift) });

#if WinCopies2
        /// <summary>
        /// Gets the <b>CloseWindow</b> command.
        /// </summary>
        [Obsolete("This command is obsolete and will be removed in later versions. Please use the System.Windows.Input.ApplicationCommands.Close command instead.")]
        public static RoutedUICommand CloseWindow { get; } = new RoutedUICommand(Desktop.Resources.Commands.WPF.ApplicationCommands.CloseWindow, nameof(CloseWindow), typeof(ApplicationCommands), new InputGestureCollection() { new KeyGesture(Key.F4, ModifierKeys.Alt) });
#endif

        public static RoutedUICommand Quit { get; } = new RoutedUICommand(WinCopies.
#if WinCopies2
            Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.Quit, nameof(Quit), typeof(ApplicationCommands), new InputGestureCollection() { new KeyGesture(Key.Q, ModifierKeys.Control) });

        /// <summary>
        /// Gets the <b>DeselectAll</b> command.
        /// </summary>
        public static RoutedUICommand DeselectAll { get; } = new RoutedUICommand(WinCopies.
#if WinCopies2
            Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.DeselectAll, nameof(DeselectAll), typeof(ApplicationCommands));

        /// <summary>
        /// Gets the <b>ReverseSelection</b> command.
        /// </summary>
        public static RoutedUICommand ReverseSelection { get; } = new RoutedUICommand(WinCopies.
#if WinCopies2
            Util.
#endif
            Desktop.Resources.Commands.WPF.ApplicationCommands.ReverseSelection, nameof(ReverseSelection), typeof(ApplicationCommands));
    }
}
