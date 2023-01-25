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

using System;
using System.Windows;
using System.Windows.Input;

using WinCopies.Util.Data;

using static WinCopies.ThrowHelper;

using EventArgs = System.EventArgs;

namespace WinCopies.Commands
{
    public interface ICommandSource<T> : ICommandSource
    {
        new ICommand<T> Command { get; }
        new T CommandParameter { get; }
#if CS8
        ICommand ICommandSource.Command => Command;

        object ICommandSource.CommandParameter => CommandParameter;
#endif
    }

    public interface IQueryCommandSource<T> : ICommandSource
    {
        new IQueryCommand<T> Command { get; }
#if CS8
        ICommand ICommandSource.Command => Command;
#endif
    }

    public interface IQueryCommandSource<TParam, TResult> : ICommandSource<TParam>, IQueryCommandSource<TResult>
    {
        new IQueryCommand<TParam, TResult> Command { get; }
    }

    public abstract class CommandSourceBase
    {
        public IInputElement CommandTarget { get; set; }
    }

    public class CommandSource : CommandSourceBase, ICommandSource
    {
        public ICommand Command { get; }

        public object CommandParameter { get; set; }

        public CommandSource(in ICommand command) => Command = command;
    }

    public class CommandSource<T> : CommandSourceBase, ICommandSource<T>
    {
        public ICommand<T> Command { get; }

        public T CommandParameter { get; set; }

        public CommandSource(in ICommand<T> command) => Command = command;
#if !CS8
        ICommand ICommandSource.Command => Command;

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
        public ICommand Command => ModelGeneric.Command;

        public object CommandParameter { get => ModelGeneric.CommandParameter; set { ModelGeneric.CommandParameter = value; OnPropertyChanged(nameof(CommandParameter)); } }

        public CommandSourceViewModel(in CommandSource commandSource) : base(commandSource) { /* Left empty. */ }
    }

    public class CommandSourceViewModel<T> : CommandSourceViewModelBase<CommandSource<T>>, ICommandSource<T>
    {
        public ICommand<T> Command => ModelGeneric.Command;

        public T CommandParameter { get => ModelGeneric.CommandParameter; set { ModelGeneric.CommandParameter = value; OnPropertyChanged(nameof(CommandParameter)); } }

        public CommandSourceViewModel(in CommandSource<T> commandSource) : base(commandSource) { /* Left empty. */ }
#if !CS8
        ICommand ICommandSource.Command => Command;

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

        public static void OnRoutedCommandExecuted(in ExecutedRoutedEventArgs e) => e.Handled = true;

        private static void _RunActionOrThrowIfNull(in EventArgs e, in string eventArgsParamName, in Action action2)
        {
            ThrowIfNull(e, eventArgsParamName);

            action2();
        }

        private static void RunActionOrThrowIfNull(in Action action1, in string actionParamName, in EventArgs e, in string eventArgsParamName, in Action action2)
        {
            ThrowIfNull(action1, actionParamName);

            _RunActionOrThrowIfNull(e, eventArgsParamName, action2);
        }

        private static void RunActionOrThrowIfNull<T>(Action<T> action1, in string actionParamName, in EventArgs e, in string eventArgsParamName, in Action action2)
        {
            ThrowIfNull(action1, actionParamName);

            _RunActionOrThrowIfNull(e, eventArgsParamName, action2);
        }

        private static void RunActionOrThrowIfNull<T>(ActionIn<T> action1, in string actionParamName, in EventArgs e, in string eventArgsParamName, in Action action2)
        {
            ThrowIfNull(action1, actionParamName);

            _RunActionOrThrowIfNull(e, eventArgsParamName, action2);
        }

        public static void TryExecuteRoutedCommand(Action action, ExecutedRoutedEventArgs e) => RunActionOrThrowIfNull(action, nameof(action), e, nameof(e), () =>
            {
                try
                {
                    action();
                }

                finally
                {
                    OnRoutedCommandExecuted(e);
                }
            });

        public static void ExecuteRoutedCommand(Action action, ExecutedRoutedEventArgs e) => RunActionOrThrowIfNull(action, nameof(action), e, nameof(e), () =>
            {
                action();

                OnRoutedCommandExecuted(e);
            });

        public static void TryExecuteRoutedCommand<T>(Action<T> action, T parameter, ExecutedRoutedEventArgs e) => RunActionOrThrowIfNull(action, nameof(action), e, nameof(e), () =>
            {
                try
                {
                    action(parameter);
                }

                finally
                {
                    OnRoutedCommandExecuted(e);
                }
            });

        public static void ExecuteRoutedCommand<T>(Action<T> action, T parameter, ExecutedRoutedEventArgs e) => RunActionOrThrowIfNull(action, nameof(action), e, nameof(e), () =>
            {
                action(parameter);

                OnRoutedCommandExecuted(e);
            });

        public static void TryExecuteRoutedCommand<T>(ActionIn<T> action, T parameter, ExecutedRoutedEventArgs e) => RunActionOrThrowIfNull(action, nameof(action), e, nameof(e), () =>
            {
                try
                {
                    action(parameter);
                }

                finally
                {
                    OnRoutedCommandExecuted(e);
                }
            });

        public static void ExecuteRoutedCommand<T>(ActionIn<T> action, T parameter, ExecutedRoutedEventArgs e) => RunActionOrThrowIfNull(action, nameof(action), e, nameof(e), () =>
            {
                action(parameter);

                OnRoutedCommandExecuted(e);
            });

        public static void CanRunCommand(Action<CanExecuteRoutedEventArgs> action, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;

            action(e);
        }

        public static void CanRunCommand(in bool value, in CanExecuteRoutedEventArgs e) => e.CanExecute = value;

        public static void CanRunCommand(in Func<bool> func, in CanExecuteRoutedEventArgs e) => CanRunCommand(func(), e);

        public static void RunCommand(in Action<ExecutedRoutedEventArgs> action, in ExecutedRoutedEventArgs e)
        {
            if (e != null)

                e.Handled = true;

            action(e);
        }
    }
}
