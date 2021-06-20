/*
 * Source: http://wpftutorial.net/DelegateCommand.html and https://marlongrech.wordpress.com/2008/12/04/attachedcommandbehavior-aka-acb/
* 
* This is free and unencumbered software released into the public domain.
* 
* Anyone is free to copy, modify, publish, use, compile, sell, or
* distribute this software, either in source code form or as a compiled
* binary, for any purpose, commercial or non-commercial, and by any
* means.
*
* In jurisdictions that recognize copyright laws, the author or authors
* of this software dedicate any and all copyright interest in the
* software to the public domain. We make this dedication for the benefit
* of the public at large and to the detriment of our heirs and
* successors. We intend this dedication to be an overt act of
* relinquishment in perpetuity of all present and future rights to this
* software under copyright law.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
* MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
* IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
* OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
* ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
* OTHER DEALINGS IN THE SOFTWARE.
*
* For more information, please refer to <http://unlicense.org> */

using System;
using System.Windows.Input;

#if !WinCopies3
namespace WinCopies.Util.Commands
#else
namespace WinCopies.Commands
#endif
{
    /// <summary>
    /// Provides a base class for WPF commands.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the Predicate to execute when the CanExecute of the command gets called
        /// </summary>
        public Predicate CanExecuteDelegate { get; set; }

        /// <summary>
        /// Gets or sets the action to be called when the Execute method of the command gets called
        /// </summary>
        public Action<object> ExecuteDelegate { get; set; }

        public DelegateCommand(Predicate canExecuteDelegate, Action<object> executeDelegate)
        {
            CanExecuteDelegate = canExecuteDelegate;

            ExecuteDelegate = executeDelegate;
        }

        #region ICommand Members
        /// <summary>
        /// Checks if the command Execute method can run
        /// </summary>
        /// <param name="parameter">THe command parameter to be passed</param>
        /// <returns>Returns true if the command can execute. By default true is returned so that if the user of SimpleCommand does not specify a CanExecuteCommand delegate the command still executes.</returns>
        public bool CanExecute(object parameter) => CanExecuteDelegate == null || CanExecuteDelegate(parameter); // if there is no can execute default to true

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Executes the actual command
        /// </summary>
        /// <param name="parameter">THe command parameter to be passed</param>
        public void Execute(object parameter) => ExecuteDelegate?.Invoke(parameter);
        #endregion
    }

    public interface ICommand<T> : ICommand
    {
        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns><see langword="true"/> if this command can be executed; otherwise, <see langword="false"/>.</returns>
        bool CanExecute(T parameter);

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        void Execute(T parameter);

#if CS8
        bool ICommand.CanExecute(object parameter) => parameter is T _parameter && CanExecute(_parameter);

        void ICommand.Execute(object parameter)
        {
            if (parameter is T _parameter)

                Execute(_parameter);
        }
#endif
    }

    /// <summary>
    /// Provides a base class for WPF commands.
    /// </summary>
    public class DelegateCommand<T> : ICommand
#if WinCopies3
        <T>
#endif
    {
        /// <summary>
        /// Gets or sets the Predicate to execute when the CanExecute of the command gets called
        /// </summary>
        public Predicate<T> CanExecuteDelegate { get; set; }

        /// <summary>
        /// Gets or sets the action to be called when the Execute method of the command gets called
        /// </summary>
        public Action<T> ExecuteDelegate { get; set; }

        public DelegateCommand(Predicate<T> canExecuteDelegate, Action<T> executeDelegate)
        {
            CanExecuteDelegate = canExecuteDelegate;

            ExecuteDelegate = executeDelegate;
        }

        #region ICommand Members
        /// <summary>
        /// Checks if the command Execute method can run
        /// </summary>
        /// <param name="parameter">THe command parameter to be passed</param>
        /// <returns>Returns true if the command can execute. By default true is returned so that if the user of SimpleCommand does not specify a CanExecuteCommand delegate the command still executes.</returns>
        public bool CanExecute(T parameter) => CanExecuteDelegate == null || CanExecuteDelegate(parameter); // if there is no can execute default to true

        bool ICommand.CanExecute(object parameter) => CanExecute((T)parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Executes the actual command
        /// </summary>
        /// <param name="parameter">The command parameter to be passed</param>
        public void Execute(T parameter) => ExecuteDelegate?.Invoke(parameter);

        void ICommand.Execute(object parameter) => Execute((T)parameter);
        #endregion
    }

    /// <summary>
    /// Provides a base class for WPF commands.
    /// </summary>
    public class DelegateCommand2 : ICommand
    {
        /// <summary>
        /// Gets or sets the action to be called when the Execute method of the command gets called.
        /// </summary>
        public Action ExecuteDelegate { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public DelegateCommand2(in Action executeDelegate) => ExecuteDelegate = executeDelegate;

        /// <summary>
        /// Checks if the command Execute method can run.
        /// </summary>
        /// <param name="parameter">THe command parameter to be passed.</param>
        /// <returns>Returns true if the command can execute. By default true is returned so that if the user of SimpleCommand does not specify a CanExecuteCommand delegate the command still executes.</returns>
        public bool CanExecute(object parameter) => true;

        /// <summary>
        /// Executes the actual command.
        /// </summary>
        /// <param name="parameter">The command parameter to be passed.</param>
        public void Execute(object parameter) => ExecuteDelegate?.Invoke();
    }

    /// <summary>
    /// Provides a base class for WPF commands.
    /// </summary>
    public class DelegateCommand3 : ICommand<object>
    {
        /// <summary>
        /// Gets or sets the action to be called when the Execute method of the command gets called.
        /// </summary>
        public Action<object> ExecuteDelegate { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public DelegateCommand3(in Action<object> executeDelegate) => ExecuteDelegate = executeDelegate;

        /// <summary>
        /// Checks if the command Execute method can run.
        /// </summary>
        /// <param name="parameter">THe command parameter to be passed.</param>
        /// <returns>Returns true if the command can execute. By default true is returned so that if the user of SimpleCommand does not specify a CanExecuteCommand delegate the command still executes.</returns>
        public bool CanExecute(object parameter) => true;

        /// <summary>
        /// Executes the actual command.
        /// </summary>
        /// <param name="parameter">The command parameter to be passed.</param>
        public void Execute(object parameter) => ExecuteDelegate?.Invoke(parameter);
    }
}
