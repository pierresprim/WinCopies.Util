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
using WinCopies.Util.Data;

#if !WinCopies3
namespace WinCopies.Util.Commands
#else
namespace WinCopies.Commands
#endif
{
    public interface ICommandSource<T> : ICommandSource
    {
#if WinCopies3
        new ICommand<T> Command { get; }
#endif

        new T CommandParameter { get; }

#if CS8
        ICommand ICommandSource.Command => Command;

        object ICommandSource.CommandParameter => CommandParameter;
#endif
    }

    public abstract class CommandSourceBase
    {
        public IInputElement CommandTarget { get; set; }
    }

    public class CommandSource : CommandSourceBase, ICommandSource
    {
        public System.Windows.Input.ICommand Command { get; }

        public object CommandParameter { get; set; }

        public CommandSource(in System.Windows.Input.ICommand command) => Command = command;
    }

    public class CommandSource<T> : CommandSourceBase, ICommandSource<T>
    {
        public ICommand<T> Command { get; }

        public T CommandParameter { get; set; }

        public CommandSource(in ICommand<T> command) => Command = command;

#if !CS8
        System.Windows.Input.ICommand ICommandSource.Command => Command;

        object ICommandSource.CommandParameter => CommandParameter;
#endif
    }

    public abstract class CommandSourceViewModelBase<T> : ViewModel<T> where T : CommandSourceBase
    {
        public IInputElement CommandTarget { get => ModelGeneric.CommandTarget; set { ModelGeneric.CommandTarget = value; OnPropertyChanged(nameof(CommandTarget)); } }

        public CommandSourceViewModelBase(in T commandSource) : base(commandSource) { /* Left empty. */ }
    }

    public class CommandSourceViewModel : CommandSourceViewModelBase<CommandSource>, ICommandSource
    {
        public System.Windows.Input.ICommand Command => ModelGeneric.Command;

        public object CommandParameter { get => ModelGeneric.CommandParameter; set { ModelGeneric.CommandParameter = value; OnPropertyChanged(nameof(CommandParameter)); } }

        public CommandSourceViewModel(in CommandSource commandSource) : base(commandSource) { /* Left empty. */ }
    }

    public class CommandSourceViewModel<T> : CommandSourceViewModelBase<CommandSource<T>>, ICommandSource<T>
    {
        public ICommand<T> Command => ModelGeneric.Command;

        public T CommandParameter { get => ModelGeneric.CommandParameter; set { ModelGeneric.CommandParameter = value; OnPropertyChanged(nameof(CommandParameter)); } }

        public CommandSourceViewModel(in CommandSource<T> commandSource) : base(commandSource) { /* Left empty. */ }

#if !CS8
        System.Windows.Input.ICommand ICommandSource.Command => Command;

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
