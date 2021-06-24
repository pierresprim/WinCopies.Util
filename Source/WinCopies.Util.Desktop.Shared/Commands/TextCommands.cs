/* Copyright © Pierre Sprimont, 2021
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

#if WinCopies3
namespace WinCopies.Commands
#else
namespace WinCopies.Util.Commands
#endif
{ 
    public static class TextCommands
    {
        public static RoutedUICommand Upper { get; } = new RoutedUICommand("Upper", nameof(Upper), typeof(TextCommands), new InputGestureCollection() { new KeyGesture(Key.U, ModifierKeys.Control | ModifierKeys.Shift) });

        public static RoutedUICommand FirstCharUpper { get; } = new RoutedUICommand("First char upper", nameof(FirstCharUpper), typeof(TextCommands));

        public static RoutedUICommand FirstCharOfEachWordUpper { get; } = new RoutedUICommand("First char of each word upper", nameof(FirstCharOfEachWordUpper), typeof(TextCommands));

        public static RoutedUICommand Lower { get; } = new RoutedUICommand("Lower", nameof(Lower), typeof(TextCommands), new InputGestureCollection() { new KeyGesture(Key.U, ModifierKeys.Control) });

        public static RoutedUICommand Reverse { get; } = new RoutedUICommand("Reverse ", nameof(Reverse), typeof(TextCommands));
    }
}
