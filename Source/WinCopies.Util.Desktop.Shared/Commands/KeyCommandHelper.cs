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

using System.Windows;
using System.Windows.Input;

#if !WinCopies3
namespace WinCopies.Util.Commands
#else
using WinCopies.Desktop;

namespace WinCopies.Commands
#endif
{
    public static class KeyCommandHelper
    {
        public static bool CanRaiseCommand(ICommandSource commandSource, KeyEventArgs e) => CanRaiseCommand(commandSource.Command, commandSource.CommandParameter, commandSource.CommandTarget, e);

        public static bool CanRaiseCommand(ICommand command, object commandParameter, IInputElement commandTarget, KeyEventArgs e)
        {
            if (command is RoutedCommand routedCommand)
            {
                if (routedCommand.InputGestures == null) return false;

                foreach (object inputGesture in routedCommand.InputGestures)

                    if (inputGesture is KeyGesture keyGesture && e.Key == keyGesture.Key && e.KeyboardDevice.Modifiers == keyGesture.Modifiers)

                        return command.CanExecute(commandParameter, commandTarget);
            }

            return false;
        }
    }
}
