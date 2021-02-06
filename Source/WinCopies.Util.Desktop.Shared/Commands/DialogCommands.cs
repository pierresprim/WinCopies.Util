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
namespace WinCopies.Util.Commands
#else
namespace WinCopies.Commands
#endif
{
    public static class DialogCommands
    {
        public static RoutedUICommand Ok { get; } = new RoutedUICommand(WinCopies.
#if !WinCopies3
            Util.
#endif
            Desktop.Resources.Commands.WPF.DialogCommands.Ok, nameof(Ok), typeof(DialogCommands));

        public static RoutedUICommand Cancel { get; } = new RoutedUICommand(WinCopies.
#if !WinCopies3
            Util.
#endif
            Desktop.Resources.Commands.WPF.DialogCommands.Cancel, nameof(Cancel), typeof(DialogCommands));

        public static RoutedUICommand Yes { get; } = new RoutedUICommand(WinCopies.
#if !WinCopies3
            Util.
#endif
            Desktop.Resources.Commands.WPF.DialogCommands.YesToAll, nameof(Yes), typeof(DialogCommands));

        public static RoutedUICommand YesToAll { get; } = new RoutedUICommand(WinCopies.
#if !WinCopies3
            Util.
#endif
            Desktop.Resources.Commands.WPF.DialogCommands.YesToAll, nameof(YesToAll), typeof(DialogCommands));

        public static RoutedUICommand No { get; } = new RoutedUICommand(WinCopies.
#if !WinCopies3
            Util.
#endif
            Desktop.Resources.Commands.WPF.DialogCommands.NoToAll, nameof(No), typeof(DialogCommands));

        public static RoutedUICommand NoToAll { get; } = new RoutedUICommand(WinCopies.
#if !WinCopies3
            Util.
#endif
            Desktop.Resources.Commands.WPF.DialogCommands.NoToAll, nameof(NoToAll), typeof(DialogCommands));

        public static RoutedUICommand Apply { get; } = new RoutedUICommand(WinCopies.
#if !WinCopies3
            Util.
#endif
            Desktop.Resources.Commands.WPF.DialogCommands.Apply, nameof(Apply), typeof(DialogCommands));

        public static RoutedUICommand Retry { get; } = new RoutedUICommand(WinCopies.
#if !WinCopies3
            Util.
#endif
            Desktop.Resources.Commands.WPF.DialogCommands.Retry, nameof(Retry), typeof(DialogCommands));

        public static RoutedUICommand Ignore { get; } = new RoutedUICommand(WinCopies.
#if !WinCopies3
            Util.
#endif
            Desktop.Resources.Commands.WPF.DialogCommands.Ignore, nameof(Ignore), typeof(DialogCommands));

        public static RoutedUICommand Abort { get; } = new RoutedUICommand(WinCopies.
#if !WinCopies3
            Util.
#endif
            Desktop.Resources.Commands.WPF.DialogCommands.Abort, nameof(Abort), typeof(DialogCommands));

        public static RoutedUICommand Continue { get; } = new RoutedUICommand(WinCopies.
#if !WinCopies3
            Util.
#endif
            Desktop.Resources.Commands.WPF.DialogCommands.Continue, nameof(Continue), typeof(DialogCommands));
    }
}
