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

namespace WinCopies.Util.Data
{
    public class CheckableNamedObjectViewModel<T> : ViewModel<T>, ICheckableNamedObject where T : ICheckableNamedObject
    {
        public bool IsChecked { get => ModelGeneric.IsChecked; set { ModelGeneric.IsChecked = value; OnStatusChanged(); } }
        public string Name { get => ModelGeneric.Name; set { ModelGeneric.Name = value; OnNameChanged(); } }

        public CheckableNamedObjectViewModel(in T checkableNamedObject) : base(checkableNamedObject) { /* Left empty. */ }

        protected virtual void OnNameChanged() => OnPropertyChanged(nameof(Name));

        protected virtual void OnStatusChanged() => OnPropertyChanged(nameof(IsChecked));
    }

    public class CheckableNamedObjectViewModel : CheckableNamedObjectViewModel<ICheckableNamedObject>, ICheckableNamedObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableObject"/> class.
        /// </summary>
        public CheckableNamedObjectViewModel() : base(new CheckableNamedObject()) { /* Left empty. */ }
        public CheckableNamedObjectViewModel(in bool isChecked) : base(new CheckableNamedObject(isChecked)) { /* Left empty. */ }
        public CheckableNamedObjectViewModel(in string name) : base(new CheckableNamedObject(name)) { /* Left empty. */ }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableObject"/> class using custom values.
        /// </summary>
        /// <param name="isChecked">A value that indicates if this object is checked by default.</param>
        /// <param name="name">The name of the object.</param>
        public CheckableNamedObjectViewModel(in bool isChecked, in string name) : base(new CheckableNamedObject(isChecked, name)) { /* Left empty. */ }

        public CheckableNamedObjectViewModel(in ICheckableNamedObject checkableNamedObject) : base(checkableNamedObject) { /* Left empty. */ }
    }
}
