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

namespace WinCopies.Util.Commands.Primitives
{
    public interface ICommand
    {
        string Name { get; }

        string Description { get; }

        bool CanExecute(object value);

        void Execute(object value);
    }

    public interface ICommand<T> : ICommand
    {
        new bool CanExecute(T item);

        new void Execute(T item);

#if CS8
        bool ICommand.CanExecute(object value) => CanExecute((T)value);

        void ICommand.Execute(object value) => Execute((T)value);
#endif
    }

    public abstract class Command : ICommand
    {
        public string Name { get; }

        public string Description { get; }

        protected Command(in string name, in string description)
        {
            Name = name;

            Description = description;
        }

        public abstract bool CanExecute(object value);

        public abstract void Execute(object value);
    }

    public abstract class Command<T> : Command, ICommand<T>
    {
        public abstract bool CanExecute(T value);

        public abstract void Execute(T value);

        protected Command(in string name, in string description) : base(name, description) { /* Left empty. */ }

        public sealed override bool CanExecute(object value) => CanExecute((T)value);

        public sealed override void Execute(object value) => Execute((T)value);
    }

    public sealed class DelegateCommand : Command
    {
        private readonly Predicate _predicate;
        private readonly Action<object> _action;

        public DelegateCommand(in string name, in string description, in Predicate predicate, in Action<object> action) : base(name, description)
        {
            _predicate = predicate;

            _action = action;
        }

        public override bool CanExecute(object value) => _predicate(value);

        public override void Execute(object value) => _action(value);
    }

    public sealed class DelegateCommand<T> : Command<T>
    {
        private readonly Predicate<T> _predicate;
        private readonly Action<T> _action;

        public DelegateCommand(in string name, in string description, in Predicate<T> predicate, in Action<T> action) : base(name, description)
        {
            _predicate = predicate;

            _action = action;
        }

        public override bool CanExecute(T value) => _predicate(value);

        public override void Execute(T value) => _action(value);
    }

    public sealed class Command<TSource, TDestination> : ICommand<TDestination> where TSource : TDestination
    {
        private readonly ICommand<TSource> _command;

        public string Name => _command.Name;

        public string Description => _command.Description;

        public Command(in ICommand<TSource> command) => _command = command;

        public bool CanExecute(TDestination item) => _command.CanExecute(item);

        public void Execute(TDestination item) => _command.Execute(item);

#if !CS8
        bool ICommand.CanExecute(object value) => _command.CanExecute(value);

        void ICommand.Execute(object value) => _command.Execute(value);
#endif
    }
}
