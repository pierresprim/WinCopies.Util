﻿/* Source: https://dzone.com/articles/how-get-eventargs ; Author of original code: Marlon Grech
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

#if CS6
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

#if !WinCopies3
namespace WinCopies.Util.Commands
#else
namespace WinCopies.Commands
#endif
{
    public class EventArgsHandler<T> : Behavior where T : class
    {
        #region LastEventArgs

        /// <summary>
        /// LastEventArgs dependency property
        /// </summary>
        public static readonly DependencyProperty LastEventArgsProperty =
            DependencyProperty.RegisterAttached("LastEventArgs", typeof(T), typeof(EventArgsHandler<T>),
                new FrameworkPropertyMetadata((T)null));

        /// <summary>
        /// Gets the last EventArgs
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> from which one get the EventArgs</param>
        /// <returns>The last EventArgs of the specified <see cref="DependencyObject"/></returns>
        public static T GetLastEventArgs(DependencyObject d) => (T)d.GetValue(LastEventArgsProperty);

        public static void SetLastEventArgs(DependencyObject d, EventArgs value) => d.SetValue(LastEventArgsProperty, value);

        #endregion

        #region HandleEventHandler 

        private static Dictionary<object, Dictionary<string, Delegate>> dico = null;

        public static readonly DependencyProperty HandleEventHandlerProperty =
            DependencyProperty.RegisterAttached("HandleEventHandler", typeof(IEnumerable<string>), typeof(EventArgsHandler<T>),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnEventHandlerChanged)));

        public static IEnumerable<string> GetHandleEventHandler(DependencyObject d) => (IEnumerable<string>)d.GetValue(HandleEventHandlerProperty);

        public static void SetHandleEventHandler(DependencyObject d, IEnumerable<string> value) => d.SetValue(HandleEventHandlerProperty, value);

        /// <summary>
        /// Handles changes to the HandleEventHandler property.
        /// </summary>
        private static void OnEventHandlerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {

                if (dico != null && dico.ContainsKey(d))

                    _ = dico.Remove(d);

                if (dico.Count == 0)

                    dico = null;
            }

            else
            {
#if CS8
                dico ??= new Dictionary<object, Dictionary<string, Delegate>>();
#else
                if (dico == null)

                    dico = new Dictionary<object, Dictionary<string, Delegate>>();
#endif

                if (d is Control control)
                {
                    if (e.NewValue is IEnumerable<string> _e)
                    {
                        if (!dico.ContainsKey(control))

                            dico.Add(control, new Dictionary<string, Delegate>());

                        foreach (System.Reflection.EventInfo _event in control.GetType().GetEvents())

                            if (_e.Contains(_event.Name))
                            {
                                var new_delegate = Delegate.CreateDelegate(_event.EventHandlerType, typeof(EventArgsHandler<T>).GetMethod(nameof(ControlEventHandler)));

                                dico[control].Add(_event.Name, new_delegate);

                                _event.AddEventHandler(control, new_delegate);
                            }

                            else if (dico.ContainsKey(_event.Name))
                            {
                                _event.RemoveEventHandler(control, dico[control][_event.Name]);

                                dico[control].Remove(_event.Name);
                            }
                    }
                }
            }
        }

        public static void ControlEventHandler(dynamic a, dynamic b)
        {
#if DEBUG

            Debug.WriteLine(nameof(ControlEventHandler));

#endif

            if (a is DependencyObject c && b is EventArgs d)

                SetLastEventArgs(c, d);
        }

        protected override Freezable CreateInstanceCore() => throw new NotImplementedException();

        protected override void ResetBehavior()
        {
            // Nothing to do here ...
        }

        #endregion
    }

    public class EventArgsHandler : EventArgsHandler<EventArgs> { }
}
#endif
