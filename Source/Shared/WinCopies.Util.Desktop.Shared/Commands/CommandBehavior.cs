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
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

using WinCopies.Diagnostics;

using DesktopHelpers = WinCopies.Util.Desktop.UtilHelpers;

namespace WinCopies.    Commands
{
    /// <summary>
    /// Defines the attached properties to create a <see cref="CommandBehaviorBinding"/>.
    /// </summary>
    public static class CommandBehavior
    {
        private static DependencyProperty RegisterAttached<T>(in string propertyName, in T
#if CS8
            ?
#endif
            defaultValue, in PropertyChangedCallback callback) where T : class => DesktopHelpers.RegisterAttached<T>(propertyName, typeof(CommandBehavior), new FrameworkPropertyMetadata(defaultValue, callback));
        private static DependencyProperty RegisterAttached<T>(in string propertyName, in PropertyChangedCallback callback) where T : class => RegisterAttached<T>(propertyName, default, callback);

        private const string Behavior = nameof(Behavior);
        private const string Command = nameof(Command);
        private const string Action = nameof(Action);
        private const string CommandParameter = nameof(CommandParameter);
        private const string Event = nameof(Event);

        #region Behavior
        /// <summary>
        /// Behavior Attached Dependency Property
        /// </summary>
        private static readonly DependencyProperty BehaviorProperty = DesktopHelpers.RegisterAttached<CommandBehaviorBinding>(Behavior, typeof(CommandBehavior));

        /// <summary>
        /// Gets the Behavior property. 
        /// </summary>
        private static CommandBehaviorBinding GetBehavior(DependencyObject d) => (CommandBehaviorBinding)d.GetValue(BehaviorProperty);

        /// <summary>
        /// Sets the Behavior property.  
        /// </summary>
        private static void SetBehavior(DependencyObject d, CommandBehaviorBinding value) => d.SetValue(BehaviorProperty, value);
        #endregion Behavior

        #region Command
        /// <summary>
        /// Command Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandProperty = RegisterAttached<ICommand>(Command, OnCommandChanged);

        /// <summary>
        /// Gets the Command property.  
        /// </summary>
        public static ICommand GetCommand(DependencyObject d) => (ICommand)d.GetValue(CommandProperty);

        /// <summary>
        /// Sets the Command property. 
        /// </summary>
        public static void SetCommand(DependencyObject d, ICommand value) => d.SetValue(CommandProperty, value);

        /// <summary>
        /// Handles changes to the Command property.
        /// </summary>
        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => FetchOrCreateBinding(d).Command = (ICommand)e.NewValue;
        #endregion Command

        #region Action
        /// <summary>
        /// Action Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ActionProperty = RegisterAttached<Action<object>>(Action, OnActionChanged);

        /// <summary>
        /// Gets the Action property.  
        /// </summary>
        public static Action<object> GetAction(DependencyObject d) => (Action<object>)d.GetValue(ActionProperty);

        /// <summary>
        /// Sets the Action property. 
        /// </summary>
        public static void SetAction(DependencyObject d, Action<object> value) => d.SetValue(ActionProperty, value);

        /// <summary>
        /// Handles changes to the Action property.
        /// </summary>
        private static void OnActionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => FetchOrCreateBinding(d).Action = (Action<object>)e.NewValue;
        #endregion Action

        #region CommandParameter
        /// <summary>
        /// CommandParameter Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = RegisterAttached<object>(CommandParameter, OnCommandParameterChanged);

        /// <summary>
        /// Gets the CommandParameter property.  
        /// </summary>
        public static object GetCommandParameter(DependencyObject d) => d.GetValue(CommandParameterProperty);

        /// <summary>
        /// Sets the CommandParameter property. 
        /// </summary>
        public static void SetCommandParameter(DependencyObject d, object value) => d.SetValue(CommandParameterProperty, value);

        /// <summary>
        /// Handles changes to the CommandParameter property.
        /// </summary>
        private static void OnCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => FetchOrCreateBinding(d).CommandParameter = e.NewValue;
        #endregion CommandParameter

        #region Event
        /// <summary>
        /// Event Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty EventProperty = RegisterAttached(Event, string.Empty, OnEventChanged);

        /// <summary>
        /// Gets the Event property.
        /// </summary>
        public static string GetEvent(DependencyObject d) => (string)d.GetValue(EventProperty);

        /// <summary>
        /// Sets the Event property.
        /// </summary>
        public static void SetEvent(DependencyObject d, string value) => d.SetValue(EventProperty, value);

        /// <summary>
        /// Handles changes to the Event property.
        /// </summary>
        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d))

                return;

            CommandBehaviorBinding binding = FetchOrCreateBinding(d);

            //check if the Event is set. If yes we need to rebind the Command to the new event and unregister the old one
            if (Determine.AreNotNull(binding.Event, binding.Owner))

                binding.Dispose();

            //if (string.IsNullOrEmpty((string)e.NewValue))

            //    return;

            //bind the new event to the command
            binding.BindEvent(d, e.NewValue.ToString());
        }
        #endregion Event

        #region Helpers
        //tries to get a CommandBehaviorBinding from the element. Creates a new instance if there is not one attached
        private static CommandBehaviorBinding FetchOrCreateBinding(DependencyObject d)
        {
            CommandBehaviorBinding binding = GetBehavior(d);

            if (binding == null)

                SetBehavior(d, binding = new CommandBehaviorBinding());

            return binding;
        }
        #endregion Helpers
    }
}
