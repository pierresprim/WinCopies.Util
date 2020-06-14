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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

#if WinCopies2
namespace WinCopies.Util
#else
namespace WinCopies
#endif
{
    [MarkupExtensionReturnType(typeof(Style))]
    [DefaultProperty("Styles")]
    public class MergedStylesExtension : MarkupExtension, INotifyPropertyChanged
    {
        private IEnumerable<Style> _styles;

        private Style _mergedStyle;

        public IEnumerable<Style> Styles { get => _styles; set { _styles = value; OnPropertyChanged(new PropertyChangedEventArgs(nameof(Styles))); } }

        public Style MergedStyle { get => _mergedStyle; private set { _mergedStyle = value; OnPropertyChanged(new PropertyChangedEventArgs(nameof(MergedStyle))); } }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_mergedStyle == null)
            {
                IEnumerator<Style> enumerator = _styles.GetEnumerator();

                if (enumerator.MoveNext())
                {
                    _mergedStyle = new Style();

                    MergeStyles(_mergedStyle, enumerator.Current);

                    // todo: sort styles by target types

                    //    ArrayBuilder<SetterBase> setters = new ArrayBuilder<SetterBase>();

                    //    for (int i = 0; i < _mergedStyle.Setters.Count; i++)

                    //    {

                    //        if (_mergedStyle.Setters[i] is Setter setter)

                    //            foreach (SetterBase item in propertyNames)

                    //                    if (item is Setter _setter && setter.Property == _setter.Property)



                    //            }

                    while (enumerator.MoveNext())

                        MergeStyles(_mergedStyle, enumerator.Current);
                }
            }

            return _mergedStyle;
        }

        public static void MergeStyles(Style s1, Style s2)
        {
            if (s2.BasedOn != null)

                MergeStyles(s1, s2.BasedOn);

            s1.Setters.AddRange(s2.Setters);

            s1.Triggers.AddRange(s2.Triggers);
        }
    }
}
