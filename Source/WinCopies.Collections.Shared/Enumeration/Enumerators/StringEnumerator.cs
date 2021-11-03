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
using WinCopies.Collections.Generic;
using WinCopies.Util;

using static WinCopies.
#if WinCopies3
    ThrowHelper
#else
    Util.Util
#endif
    ;

namespace WinCopies.Collections
{
    public class StringEnumerator : Enumerator<string>
    {
        private int _currentIndex = -1;
        private Func<bool> _func;

        protected System.Collections.Specialized.StringCollection StringCollection { get; }

        public EnumerationDirection EnumerationDirection { get; }

#if WinCopies3
        public override bool? IsResetSupported => true;
#endif

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

#if WinCopies3
        protected override void ResetOverride2() { /* Left empty. */ }

        protected override void ResetCurrent() =>
#else
        protected override void ResetOverride()
        {
#endif
            _currentIndex = -1;

#if !WinCopies3
            base.ResetOverride();
        }

        private static InvalidEnumArgumentException GetInvalidEnumArgumentException(in string argumentName, in Enum value) => new InvalidEnumArgumentException(argumentName, value);
#endif

#if WinCopies3
        protected override void DisposeManaged()
#else
        protected override void Dispose(bool disposing)
#endif
        {
            _func = null;

#if WinCopies3
            base.DisposeManaged();
#else
            base.Dispose(disposing);
#endif
        }
    }
}
