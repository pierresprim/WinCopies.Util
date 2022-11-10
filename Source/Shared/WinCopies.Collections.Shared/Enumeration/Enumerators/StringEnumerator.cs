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
using System.Collections.Specialized;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Util;

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections
{
    public class StringEnumerator : Enumerator<string>, ICountableEnumerator<string>
    {
        private int _currentIndex = -1;
        private Func<bool> _func;

        protected System.Collections.Specialized.StringCollection StringCollection { get; }

        public EnumerationDirection EnumerationDirection { get; }

        public override bool? IsResetSupported => true;

        public int Count => StringCollection.Count;

        public StringEnumerator(in System.Collections.Specialized.StringCollection stringCollection, in EnumerationDirection enumerationDirection)
        {
            StringCollection = stringCollection ?? throw GetArgumentNullException(nameof(stringCollection));
            EnumerationDirection = enumerationDirection;

            switch (enumerationDirection)
            {
                case EnumerationDirection.FIFO:

                    _func = () => ++_currentIndex < StringCollection.Count;

                    break;

                case EnumerationDirection.LIFO:

                    _currentIndex = stringCollection.Count;

                    _func = () => --_currentIndex > -1;

                    break;

                default:

                    throw GetInvalidEnumArgumentException(nameof(enumerationDirection), enumerationDirection);
            }
        }

        protected override string CurrentOverride => StringCollection[_currentIndex];

        protected override bool MoveNextOverride() => _func();

        protected override void ResetOverride2() { /* Left empty. */ }

        protected override void ResetCurrent() => _currentIndex = -1;

        protected override void DisposeManaged()
        {
            _func = null;

            base.DisposeManaged();
        }
    }
}
