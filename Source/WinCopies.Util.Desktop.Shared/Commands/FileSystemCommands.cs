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
namespace WinCopies.Util.Commands
#else
namespace WinCopies.Commands
#endif
{
    /// <summary>
    /// Provides some standard commands for file system gesture.
    /// </summary>
    public static class FileSystemCommands
    {
        /// <summary>
        /// Gets the <b>NewFolder</b> command.
        /// </summary>
        public static RoutedUICommand NewFolder { get; } = new RoutedUICommand(WinCopies.
#if WinCopies2
            Util.
#endif
            Desktop.Resources.Commands.WPF.FileSystemCommands.NewFolder, nameof(NewFolder), typeof(FileSystemCommands), new InputGestureCollection() { new KeyGesture(Key.N, ModifierKeys.Control) });

        /// <summary>
        /// Gets the <b>NewArchive</b> command.
        /// </summary>
        public static RoutedUICommand NewArchive { get; } = new RoutedUICommand(WinCopies.
#if WinCopies2
            Util.
#endif
            Desktop.Resources.Commands.WPF.FileSystemCommands.NewArchive, nameof(NewArchive), typeof(FileSystemCommands), new InputGestureCollection() { new KeyGesture(Key.N, ModifierKeys.Control | ModifierKeys.Alt) });

        /// <summary>
        /// Gets the <b>FileProperties</b> command.
        /// </summary>
        public static RoutedUICommand FileProperties { get; } = new RoutedUICommand(WinCopies.
#if WinCopies2
            Util.
#endif
            Desktop.Resources.Commands.WPF.FileSystemCommands.ShowFileProperties, nameof(FileProperties), typeof(FileSystemCommands), new InputGestureCollection() { new KeyGesture(Key.Enter, ModifierKeys.Alt) });

        /// <summary>
        /// Gets the <b>Rename</b> command.
        /// </summary>
        public static RoutedUICommand Rename { get; } = new RoutedUICommand(WinCopies.
#if WinCopies2
            Util.
#endif
            Desktop.Resources.Commands.WPF.FileSystemCommands.Rename, nameof(Rename), typeof(FileSystemCommands), new InputGestureCollection() { new KeyGesture(Key.R, ModifierKeys.Control) });

        /// <summary>
        /// Gets the <b>DeletePermanently</b> command.
        /// </summary>
        public static RoutedUICommand DeletePermanently { get; } = new RoutedUICommand(WinCopies.
#if WinCopies2
            Util.
#endif
            Desktop.Resources.Commands.WPF.FileSystemCommands.DeletePermanently, nameof(DeletePermanently), typeof(FileSystemCommands), new InputGestureCollection() { new KeyGesture(Key.Delete, ModifierKeys.Shift) });
    }
}
