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

using System;
using System.Windows.Input;
using WinCopies.Util.Commands.Primitives;

namespace WinCopies.Commands
{
    public class CommandModel<T> :  Util.Commands.Primitives.ICommand, System.Windows.Input.ICommand where T : Util.Commands.Primitives.ICommand
    {
        protected T Command { get; }

        public string Name => Command.Name;

        public string Description => Command.Description;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public CommandModel(in T command) => Command = command;

        public bool CanExecute(object parameter) => Command.CanExecute(parameter);

        public void Execute(object parameter) => Command.Execute(parameter);
    }

    public class CommandModel<TCommand, TParameter> : CommandModel<TCommand>, Util.Commands.Primitives.ICommand<TParameter>, WinCopies.
#if !WinCopies3
        Util.
#endif
        Commands.ICommand<TParameter> where TCommand : Util.Commands.Primitives.ICommand<TParameter>
    {
        public CommandModel(in TCommand command) : base(command) { /* Left empty. */ }

        public bool CanExecute(TParameter parameter) => Command.CanExecute(parameter);

        public void Execute(TParameter parameter) => Command.Execute(parameter);
    }
}
