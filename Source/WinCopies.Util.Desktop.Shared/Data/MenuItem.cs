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

#if WinCopies3
using WinCopies.Collections;
#endif

namespace WinCopies.Util.Data
{
    public class MenuItemBase : ViewModelBase
    {
        private ImageSource _icon;
        private ICommand _command;
        private object _commandParameter;
        private IInputElement _commandTarget;
        private bool _isCheckable = false;
        private bool _isChecked = false;

        public ImageSource Icon { get => _icon; set => UpdateValue(ref _icon, value, nameof(Icon)); }

        /// <summary>
        /// Gets or sets the <see cref="ICommand"/> that represents the action to do when the user clicks on the menu item.
        /// </summary>
        public ICommand Command { get => _command; set => UpdateValue(ref _command, value, nameof(Command)); }

        /// <summary>
        /// Gets or sets the command parameter for the <see cref="Command"/> property value.
        /// </summary>
        public object CommandParameter { get => _commandParameter; set => UpdateValue(ref _commandParameter, value, nameof(CommandParameter)); }

        /// <summary>
        /// Gets or sets the command target for this menu item.
        /// </summary>
        public IInputElement CommandTarget { get => _commandTarget; set => UpdateValue(ref _commandTarget, value, nameof(CommandTarget)); }

        /// <summary>
        /// Gets or sets a value that indicates whether the menu item can be checked.
        /// </summary>
        public bool IsCheckable { get => _isCheckable; set => UpdateValue(ref _isCheckable, value, nameof(IsCheckable)); }

        /// <summary>
        /// Gets or sets a value that indicates whether the menu item is checked.
        /// </summary>
        public bool IsChecked { get => _isChecked; set => UpdateValue(ref _isChecked, value, nameof(IsChecked)); }

        /// <summary>
        /// Gets a value that indicates whether this <see cref="MenuItem"/> represents a separator.
        /// </summary>
        public bool IsSeparator { get; private set; } = false;

        public MenuItemBase() { /* Left empty. */ }

        public MenuItemBase(in bool isSeparator) => IsSeparator = isSeparator;

        public MenuItemBase(in ImageSource icon) => _icon = icon;

        public MenuItemBase(in ImageSource icon, in ICommand command, in object commandParameter, in IInputElement commandTarget) : this(icon)
        {
            _command = command;

            _commandParameter = commandParameter;

            _commandTarget = commandTarget;
        }
    }

    public class MenuItem : MenuItemBase
    {
        private object _header;
        private ObservableCollection<object> _items = new ObservableCollection<object>();

        /// <summary>
        /// Gets or sets a value that represents the header of this item.
        /// </summary>
        public object Header { get => _header; set => UpdateValue(ref _header, value, nameof(Header)); }

        // todo: do not reduce this enumerable to MenuItemData and perform a check for the type of item in xaml. Seeing also TabControl.xaml.cs for the other use of this class in this solution.

        /// <summary>
        /// Gets or sets the items of this menu item.
        /// </summary>
        public ObservableCollection<object> Items { get => _items; set => UpdateValue(ref _items, value, nameof(Items)); }

        public MenuItem() { /* Left empty. */ }

        public MenuItem(in bool isSeparator) : base(isSeparator) { /* Left empty. */ }

        public MenuItem(
#if WinCopies3
            in
#endif
            object header) => _header = header;

        public MenuItem(
#if WinCopies3
            in
#endif
            object header,
#if WinCopies3
            in
#endif
             ImageSource icon) : base(icon) => _header = header;

        public MenuItem(
#if WinCopies3
            in
#endif
            object header,
#if WinCopies3
            in
#endif
            ImageSource icon,
#if WinCopies3
            in
#endif
            ICommand command,
#if WinCopies3
            in
#endif
            object commandParameter,
#if WinCopies3
            in
#endif
            IInputElement commandTarget) : base(icon, command, commandParameter, commandTarget) => _header = header;

        public MenuItem(
#if WinCopies3
            in
#endif
            object header,
#if WinCopies3
            in
#endif
            IEnumerable<MenuItem> items)
        {
            _header = header;

            Items.AddRange(items);
        }
    }

    public class MenuItem<THeader, TChildren> : MenuItemBase
    {
        private THeader _header;

        /// <summary>
        /// Gets or sets a value that represents the header of this item.
        /// </summary>
        public THeader Header { get => _header; set => UpdateValue(ref _header, value, nameof(Header)); }

        // todo: do not reduce this enumerable to MenuItemData and perform a check for the type of item in xaml. Seeing also TabControl.xaml.cs for the other use of this class in this solution.

        /// <summary>
        /// Gets or sets the items of this menu item.
        /// </summary>
        public ObservableCollection<TChildren> Items { get; } = new ObservableCollection<TChildren>();

        public MenuItem() { /* Left empty. */ }

        public MenuItem(
#if WinCopies3
            in
#endif
            bool isSeparator) : base(isSeparator) { /* Left empty. */ }

        public MenuItem(
#if WinCopies3
            in
#endif
            THeader header) => _header = header;

        public MenuItem(
#if WinCopies3
            in
#endif
            THeader header,
#if WinCopies3
            in
#endif
            ImageSource icon) : base(icon) => _header = header;

        public MenuItem(
#if WinCopies3
            in
#endif
         THeader header,
#if WinCopies3
            in
#endif
          ImageSource icon,
#if WinCopies3
            in
#endif
         ICommand command,
#if WinCopies3
            in
#endif
         object commandParameter,
#if WinCopies3
            in
#endif
         IInputElement commandTarget) : base(icon, command, commandParameter, commandTarget) => _header = header;

        public MenuItem(
#if WinCopies3
            in
#endif
         THeader header,
#if WinCopies3
            in
#endif
         IEnumerable<MenuItem> items) : this(header) => Items.AddRange(items);
    }
}
