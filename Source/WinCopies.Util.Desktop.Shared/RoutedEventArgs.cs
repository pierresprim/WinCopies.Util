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
using System.Windows.Controls.Primitives;

#if !WinCopies3
namespace WinCopies.Util
#else
namespace WinCopies
#endif
{
    /// <summary>
    /// Encapsulates a common <see cref="EventArgs"/> into a <see cref="RoutedEventArgs"/> in an event delegate.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="EventArgs"/> to encapsulate.</typeparam>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    /// <seealso cref="RoutedEventArgs{T}"/>
    public delegate void RoutedEventHandler<T>(object sender, RoutedEventArgs<T> e) where T : EventArgs;

    /// <summary>
    /// Encapsulates a common <see cref="EventArgs"/> into a <see cref="RoutedEventArgs"/>.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="EventArgs"/> to encapsulate.</typeparam>
    /// <seealso cref="RoutedEventArgs"/>
    /// <seealso cref="RoutedEventArgs.RoutedEvent"/>
    /// <seealso cref="EventManager"/>
    /// <seealso cref="UIElement.RaiseEvent(RoutedEventArgs)"/>
    public class RoutedEventArgs<T> : RoutedEventArgs where T : EventArgs
    {
        /// <summary>
        /// The original <see cref="EventArgs"/>.
        /// </summary>
        public T OriginalEventArgs { get; } = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEventArgs{T}"/> class.
        /// </summary>
        /// <param name="originalEventArgs">The original <see cref="EventArgs"/> to encapsulate in this <see cref="RoutedEventArgs"/>.</param>
        /// <remarks>When using this <see cref="RoutedEventArgs"/>-parameterless constructor, all public properties of the new <see cref="RoutedEventArgs"/> instance assume the following default values:
        /// <ul><li><see cref="RoutedEventArgs.RoutedEvent"/> defaults to <see langword="null"/>.</li>
        /// <li><see cref="RoutedEventArgs.Handled"/> defaults to <see langword="false"/>.</li>
        /// <li><see cref="RoutedEventArgs.Source"/> defaults to <see langword="null"/>.</li>
        /// <li><see cref="RoutedEventArgs.OriginalSource"/> defaults to <see langword="null"/>.</li></ul>
        /// Null values for <see cref="RoutedEventArgs.Source"/> and <see cref="RoutedEventArgs.OriginalSource"/> only mean that the <see cref="RoutedEventArgs"/> data makes no attempt to specify the source. When this instance is used in a call to <see cref="UIElement.RaiseEvent(RoutedEventArgs)"/>, the <see cref="RoutedEventArgs.Source"/> and <see cref="RoutedEventArgs.OriginalSource"/> values are populated based on the element that raised the event and are passed on to listeners through the routing.
        /// </remarks>
        /// <seealso cref="RoutedEventArgs.RoutedEvent"/>
        public RoutedEventArgs(T originalEventArgs) => OriginalEventArgs = originalEventArgs;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEventArgs{T}"/> class, using the supplied routed event identifier.
        /// </summary>
        /// <param name="routedEvent">The routed event identifier for this instance of the <see cref="RoutedEventArgs"/> class.</param>
        /// <param name="originalEventArgs">The original <see cref="EventArgs"/> to encapsulate in this <see cref="RoutedEventArgs"/>.</param>
        /// <example>void RaiseTapEvent()
        /// {
        ///     RoutedEventArgs newEventArgs = new RoutedEventArgs(MyButtonSimple.TapEvent);
        ///     RaiseEvent(newEventArgs);
        /// }</example>
        /// <remarks>When using this overloaded constructor, unspecified properties of the new RoutedEventArgs instance assume the following default values:
        /// <ul><li><see cref="RoutedEventArgs.Handled"/> defaults to <see langword="false"/>.</li>
        /// <li><see cref="RoutedEventArgs.Source"/> defaults to <see langword="null"/>.</li>
        /// <li><see cref="RoutedEventArgs.OriginalSource"/> defaults to <see langword="null"/>.</li></ul>
        /// Null values for <see cref="RoutedEventArgs.Source"/> and <see cref="RoutedEventArgs.OriginalSource"/> only mean that this <see cref="RoutedEventArgs"/> makes no attempt to specify the source.When this instance is used in a call to <see cref="UIElement.RaiseEvent(RoutedEventArgs)"/>, the <see cref="RoutedEventArgs.Source"/> and <see cref="RoutedEventArgs.OriginalSource"/> values are populated based on the element that raised the event and are passed on to listeners through the routing.</remarks>
        /// <seealso cref="UIElement.RaiseEvent(RoutedEventArgs)"/>
        /// <seealso cref="RoutedEventArgs.RoutedEvent"/>
        public RoutedEventArgs(RoutedEvent routedEvent, T originalEventArgs) : base(routedEvent) => OriginalEventArgs = originalEventArgs;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEventArgs{T}"/> class, using the supplied routed event identifier, and providing the opportunity to declare a different source for the event.
        /// </summary>
        /// <param name="routedEvent">The routed event identifier for this instance of the <see cref="RoutedEventArgs"/> class.</param>
        /// <param name="source">An alternate source that will be reported when the event is handled. This pre-populates the <see cref="RoutedEventArgs.Source"/> property.</param>
        /// <param name="originalEventArgs">The original <see cref="EventArgs"/> to encapsulate in this <see cref="RoutedEventArgs"/>.</param>When using this overloaded constructor, unspecified properties of the new RoutedEventArgs instance assume the following default values:
        /// <ul><li><see cref="RoutedEventArgs.Handled"/> defaults to <see langword="false"/>.</li>
        /// <li><see cref="RoutedEventArgs.OriginalSource"/> defaults to <see langword="null"/>.</li></ul>
        /// <para>Null values for <see cref="RoutedEventArgs.OriginalSource"/> are populated based on the element that raised the event and passed on through the routing, but will read <see langword="null"/> prior to invocation.</para>
        /// <para>Use this signature when passing <see cref="RoutedEventArgs"/> to virtuals such as <see cref="TextBoxBase.OnSelectionChanged"/>, where the arguments are used to call <see cref="UIElement.RaiseEvent(RoutedEventArgs)"/> internally.</para>
        public RoutedEventArgs(RoutedEvent routedEvent, object source, T originalEventArgs) : base(routedEvent, source) => OriginalEventArgs = originalEventArgs;
    }
}
