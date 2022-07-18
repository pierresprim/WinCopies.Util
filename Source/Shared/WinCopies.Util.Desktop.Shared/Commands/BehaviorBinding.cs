/* Source: https://dzone.com/articles/how-get-eventargs ; Author of original code: Marlon Grech
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
using System.Windows;
using System.Windows.Input;

using WinCopies.Diagnostics;

using static WinCopies.Util.Desktop.UtilHelpers;

namespace WinCopies.
#if !WinCopies3
    Util.
#endif
    Commands
{
    /// <summary>
    /// Provides a base class for behaviors.
    /// This inherits from freezable so that it gets inheritance context for DataBinding to work
    /// </summary>
    public abstract class Behavior : Freezable
    {
        internal int Id { get; set; }

        DependencyObject _owner;

        /// <summary>
        /// Gets or sets the Owner of the binding
        /// </summary>
        public DependencyObject Owner
        {
            get => _owner;

            set
            {
                _owner = value;
                ResetBehavior();
            }
        }

        // static void OwnerReset(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((BehaviorBinding)d).ResetBehavior();

        /// <summary>
        /// When overriden in a derived class, resets the behavior.
        /// </summary>
        protected abstract void ResetBehavior();

        /// <summary>
        /// This is not actually used. This is just a trick so that this object gets WPF Inheritance Context
        /// </summary>
        /// <returns></returns>
        protected override Freezable CreateInstanceCore() => throw new NotImplementedException();
    }

    /// <summary>
    /// Defines a Command Binding
    /// </summary>
    public class BehaviorBinding : Behavior
    {
        private static DependencyProperty Register<T>(in string propertyName, in PropertyChangedCallback callback) => Register<T, BehaviorBinding>(propertyName, new FrameworkPropertyMetadata(null, callback));

        CommandBehaviorBinding _behavior;

        /// <summary>
        /// Stores the <see cref="CommandBehaviorBinding"/>.
        /// </summary>
        internal CommandBehaviorBinding Behavior => _behavior
#if CS8
            ??=
#else
            ?? (_behavior =
#endif
            new CommandBehaviorBinding()
#if !CS8
        )
#endif
        ;

        #region Command
        /// <summary>
        /// <see cref="Command"/> Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandProperty = Register<ICommand>(nameof(Command), OnCommandChanged);

        /// <summary>
        /// Gets or sets the Command property.  
        /// </summary>
        public ICommand Command { get => (ICommand)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }

        /// <summary>
        /// Handles changes to the Command property.
        /// </summary>
        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((BehaviorBinding)d).OnCommandChanged(e);

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Command property.
        /// </summary>
        protected virtual void OnCommandChanged(DependencyPropertyChangedEventArgs e) => Behavior.Command = Command;
        #endregion Command

        #region Action
        /// <summary>
        /// <see cref="Action"/> Dependency Property
        /// </summary>
        public static readonly DependencyProperty ActionProperty = Register<Action<object>>(nameof(Action), OnActionChanged);

        /// <summary>
        /// Gets or sets the Action property. 
        /// </summary>
        public Action<object> Action { get => (Action<object>)GetValue(ActionProperty); set => SetValue(ActionProperty, value); }

        /// <summary>
        /// Handles changes to the Action property.
        /// </summary>
        private static void OnActionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((BehaviorBinding)d).OnActionChanged(e);

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Action property.
        /// </summary>
        protected virtual void OnActionChanged(DependencyPropertyChangedEventArgs e) => Behavior.Action = Action;
        #endregion Action

        #region CommandParameter
        /// <summary>
        /// <see cref="CommandParameter"/> Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = Register<object>(nameof(CommandParameter), OnCommandParameterChanged);

        /// <summary>
        /// Gets or sets the CommandParameter property.  
        /// </summary>
        public object CommandParameter { get => GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }

        /// <summary>
        /// Handles changes to the CommandParameter property.
        /// </summary>
        private static void OnCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((BehaviorBinding)d).OnCommandParameterChanged(e);

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the CommandParameter property.
        /// </summary>
        protected virtual void OnCommandParameterChanged(DependencyPropertyChangedEventArgs e) => Behavior.CommandParameter = CommandParameter;
        #endregion CommandParameter

        #region Event
        /// <summary>
        /// <see cref="Event"/> Dependency Property
        /// </summary>
        public static readonly DependencyProperty EventProperty = Register<string>(nameof(Event), OnEventChanged);

        /// <summary>
        /// Gets or sets the Event property.
        /// </summary>
        public string Event { get => (string)GetValue(EventProperty); set => SetValue(EventProperty, value); }

        /// <summary>
        /// Handles changes to the Event property.
        /// </summary>
        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((BehaviorBinding)d).OnEventChanged(e);

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Event property.
        /// </summary>
        protected virtual void OnEventChanged(DependencyPropertyChangedEventArgs e) => ResetBehavior();

        /// <summary>
        /// Resets the behavior.
        /// </summary>
        protected override void ResetBehavior()
        {
            if (Owner == null) return; //only do this when the Owner is set

            //check if the Event is set. If yes we need to rebind the Command to the new event and unregister the old one
            if (Determine.AreNotNull(Behavior.Event, Behavior.Owner))

                Behavior.Dispose();

            //bind the new event to the command
            Behavior.BindEvent(Owner, Event);
        }

        protected override
#if WinCopies3 && CS10
            BehaviorBinding
#else
            Freezable
#endif
            CreateInstanceCore() => new
#if !CS10
            BehaviorBinding
#endif
            ();
        #endregion Event
    }
}
