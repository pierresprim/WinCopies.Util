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

namespace WinCopies.Util.Commands
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
        public bool CanExecute(object parameter)
        {
            if (CanExecuteDelegate == null)

                return true;// if there is no can execute default to true

            return CanExecuteDelegate(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Executes the actual command
        /// </summary>
        /// <param name="parameter">THe command parameter to be passed</param>
        public void Execute(object parameter) => ExecuteDelegate?.Invoke(parameter);

        #endregion
    }

    /// <summary>
    /// Provides a base class for WPF commands.
    /// </summary>
    public class DelegateCommand<T> : ICommand
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
        public bool CanExecute(T parameter)
        {
            if (CanExecuteDelegate == null)
                return true;// if there is no can execute default to true
            return CanExecuteDelegate(parameter);
        }

        bool ICommand.CanExecute(object parameter) => CanExecute((T)parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Executes the actual command
        /// </summary>
        /// <param name="parameter">THe command parameter to be passed</param>
        public void Execute(T parameter) => ExecuteDelegate?.Invoke(parameter);

        void ICommand.Execute(object parameter) => Execute((T)parameter);

        #endregion
    }
}
