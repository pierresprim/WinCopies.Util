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
    public interface ICommandSource<T> : ICommandSource
    {
        new T CommandParameter { get; }

#if CS8
        object ICommandSource.CommandParameter => CommandParameter;
#endif
    }

    public static class Commands
    {
        public static RoutedCommand CommonCommand { get; } = new RoutedCommand(nameof(CommonCommand), typeof(Commands));

        /// <summary>
        /// A static <see cref="System.Windows.Input.CanExecuteRoutedEventHandler"/> that sets the <see cref="CanExecuteRoutedEventArgs.CanExecute"/> to true. This handler can be used for commands that can always be executed.
        /// </summary>
        public static CanExecuteRoutedEventHandler CanExecuteRoutedEventHandler { get; } = (object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
    }
}
