/* Copyright © Pierre Sprimont, 2020
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
using System.Collections;
using System.Collections.Generic;

#if WinCopies2

using static WinCopies.Util.Util;
using static WinCopies.Util.ThrowHelper;

#else

using WinCopies;

using static WinCopies.ThrowHelper;

using IDisposable = WinCopies.DotNetFix.IDisposable;

#endif

namespace WinCopies.Collections.Generic
{
    public sealed class SelectEnumerator<TSource, TDestination> : Enumerator<TSource, TDestination>
    {
        private Func<TSource, TDestination> _func;

#if !WinCopies2
        private TDestination _current;

        protected override TDestination CurrentOverride => _current;

        public override bool? IsResetSupported => null;
#endif

        public SelectEnumerator(in System.Collections.Generic.IEnumerator<TSource> enumerator, in Func<TSource, TDestination> func) : base(enumerator) => _func = func ?? throw GetArgumentNullException(nameof(func));

        protected override bool MoveNextOverride()
        {
            if (InnerEnumerator.MoveNext())
            {
#if WinCopies2
Current 
#else
                _current
#endif
                    = _func(InnerEnumerator.Current);

                return true;
            }

            return false;
        }

        protected override void ResetOverride()
        {
            base.ResetOverride();

            InnerEnumerator.Reset();
        }

        protected
#if WinCopies2
            override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if(disposing)

                _func = null;
        }
#else
override void DisposeManaged()
        {
            base.DisposeManaged();

            _func = null;
        }
#endif
    }
}
