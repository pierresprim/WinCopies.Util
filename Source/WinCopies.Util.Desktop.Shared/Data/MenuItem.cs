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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WinCopies.Util.Data
{
    public class MenuItem : ViewModelBase
    {
        private readonly object _header;

        /// <summary>
        /// Gets or sets a value that represents the header of this item.
        /// </summary>
        public object Header { get => _header; set => Update(nameof(Header), nameof(_header), value, typeof(MenuItem)); }

        private readonly ImageSource _icon;

        public ImageSource Icon { get => _icon; set => Update(nameof(Icon), nameof(_icon), value, typeof(MenuItem)); }

        private readonly ICommand _command;

        /// <summary>
        /// Gets or sets the <see cref="ICommand"/> that represents the action to do when the user clicks on the menu item.
        /// </summary>
        public ICommand Command { get => _command; set => Update(nameof(Command), nameof(_command), value, typeof(MenuItem)); }

        private readonly object _commandParameter;

        /// <summary>
        /// Gets or sets the command parameter for the <see cref="Command"/> property value.
        /// </summary>
        public object CommandParameter { get => _commandParameter; set => Update(nameof(CommandParameter), nameof(_commandParameter), value, typeof(MenuItem)); }

        private readonly IInputElement _commandTarget;

        /// <summary>
        /// Gets or sets the command target for this menu item.
        /// </summary>
        public IInputElement CommandTarget { get => _commandTarget; set => Update(nameof(CommandTarget), nameof(_commandTarget), value, typeof(MenuItem)); }

        private readonly bool _isCheckable = false;

        /// <summary>
        /// Gets or sets a value that indicates whether the menu item can be checked.
        /// </summary>
        public bool IsCheckable { get => _isCheckable; set => Update(nameof(IsCheckable), nameof(_isCheckable), value, typeof(MenuItem)); }

        private readonly bool _isChecked = false;

        /// <summary>
        /// Gets or sets a value that indicates whether the menu item is checked.
        /// </summary>
        public bool IsChecked { get => _isChecked; set => Update(nameof(IsChecked), nameof(_isChecked), value, typeof(MenuItem)); }

        // todo: do not reduce this enumerable to MenuItemData and perform a check for the type of item in xaml. Seeing also TabControl.xaml.cs for the other use of this class in this solution.

        /// <summary>
        /// Gets or sets the items of this menu item.
        /// </summary>
        public ObservableCollection<object> Items { get; set; } = new ObservableCollection<object>();

        /// <summary>
        /// Gets a value that indicates whether this <see cref="MenuItem"/> represents a separator.
        /// </summary>
        public bool IsSeparator { get; private set; } = false;

        public MenuItem() { }

        public MenuItem(bool isSeparator) => IsSeparator = isSeparator;

        public MenuItem(object header) => _header = header;

        public MenuItem(object header, ImageSource icon)
        {
            _header = header;

            _icon = icon;
        }

        public MenuItem(object header, ImageSource icon, ICommand command, object commandParameter, IInputElement commandTarget)
        {
            _header = header;

            _icon = icon;

            _command = command;

            _commandParameter = commandParameter;

            _commandTarget = commandTarget;
        }

        public MenuItem(object header, IEnumerable<MenuItem> items)
        {
            _header = header;

            Items.AddRange(items);
        }
    }

    public class MenuItem<THeader, TChildren> : ViewModelBase
    {
        private readonly THeader _header;

        /// <summary>
        /// Gets or sets a value that represents the header of this item.
        /// </summary>
        public THeader Header { get => _header; set => Update(nameof(Header), nameof(_header), value, typeof(MenuItem)); }

        private readonly ImageSource _icon = null;

        public ImageSource Icon { get => _icon; set => Update(nameof(Icon), nameof(_icon), value, typeof(MenuItem)); }

        private readonly ICommand _command = null;

        /// <summary>
        /// Gets or sets the <see cref="ICommand"/> that represents the action to do when user click on this menu item.
        /// </summary>
        public ICommand Command { get => _command; set => Update(nameof(Command), nameof(_command), value, typeof(MenuItem)); }

        private readonly object _commandParameter = null;

        /// <summary>
        /// Gets or sets the command parameter for the <see cref="CommandParameter"/> property.
        /// </summary>
        public object CommandParameter { get => _commandParameter; set => Update(nameof(CommandParameter), nameof(_commandParameter), value, typeof(MenuItem)); }

        private readonly IInputElement _commandTarget = null;

        public IInputElement CommandTarget { get => _commandTarget; set => Update(nameof(CommandTarget), nameof(_commandTarget), value, typeof(MenuItem)); }

        private readonly bool _isCheckable = false;

        /// <summary>
        /// Gets or sets a value that indicates whether this menu item can be checked.
        /// </summary>
        public bool IsCheckable { get => _isCheckable; set => Update(nameof(IsCheckable), nameof(_isCheckable), value, typeof(MenuItem)); }

        private readonly bool _isChecked = false;

        /// <summary>
        /// Gets or sets a value that indicates whether this menu item is checked.
        /// </summary>
        public bool IsChecked { get => _isChecked; set => Update(nameof(IsChecked), nameof(_isChecked), value, typeof(MenuItem)); }

        // todo: do not reduce this enumerable to MenuItemData and perform a check for the type of item in xaml. Seeing also TabControl.xaml.cs for the other use of this class in this solution.

        /// <summary>
        /// Gets or sets the items of this menu item.
        /// </summary>
        public ObservableCollection<TChildren> Items { get; } = new ObservableCollection<TChildren>();

        /// <summary>
        /// Gets a value that indicates whether this <see cref="MenuItem"/> represents a separator.
        /// </summary>
        public bool IsSeparator { get; private set; } = false;

        public MenuItem() { }

        public MenuItem(bool isSeparator) => IsSeparator = isSeparator;

        public MenuItem(THeader header) => _header = header;

        public MenuItem(THeader header, ImageSource icon)
        {
            _header = header;

            _icon = icon;
        }

        public MenuItem(THeader header, ImageSource icon, ICommand command, object commandParameter, IInputElement commandTarget)
        {
            _header = header;

            _icon = icon;

            _command = command;

            _commandParameter = commandParameter;

            _commandTarget = commandTarget;
        }

        public MenuItem(THeader header, IEnumerable<MenuItem> items)
        {
            _header = header;

            Items.AddRange(items);
        }
    }
}
