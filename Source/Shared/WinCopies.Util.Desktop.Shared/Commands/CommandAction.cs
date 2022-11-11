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

#if CS7
using Microsoft.Xaml.Behaviors;

using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;

namespace WinCopies.Commands
{
    public class CommandAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandAction), new PropertyMetadata(null, OnCommandChanged));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(CommandAction), new PropertyMetadata(null, OnCommandParameterChanged));

        private System.IDisposable canExecuteChanged;

        public ICommand Command { get => (ICommand)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }

        public object CommandParameter { get => GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }

        private static void OnCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is CommandAction ev)
            {
                ev.canExecuteChanged?.Dispose();

                if (e.NewValue is ICommand command)

                    ev.canExecuteChanged = Observable.FromEventPattern(
                        x => command.CanExecuteChanged += x,
                        x => command.CanExecuteChanged -= x).Subscribe
                        (_ => ev.SynchronizeElementState());
            }
        }

        private static void OnCommandParameterChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => (sender as CommandAction)?.SynchronizeElementState();

        private void SynchronizeElementState()
        {
            if (Command != null && AssociatedObject is FrameworkElement associatedObject)

                associatedObject.IsEnabled = Command.CanExecute(CommandParameter);
        }

        protected override void Invoke(object parameter) => Command?.Execute(CommandParameter);
    }
}
#endif
