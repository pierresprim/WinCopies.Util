﻿/* Copyright © Pierre Sprimont, 2020
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

using System.Threading;

using static WinCopies.Collections.ThrowHelper;

#if !WinCopies3
using WinCopies.Util;

using static WinCopies.Util.Util;
using static WinCopies.Util.ThrowHelper;
#endif

namespace WinCopies.Collections.DotNetFix
{
    public abstract class ReadOnlySimpleLinkedListBase
#if WinCopies3
            : ISimpleLinkedListBase2
#endif
    {
        private object _syncRoot;

        public bool IsReadOnly => true;

        public abstract uint Count { get; }

        public bool HasItems => Count != 0u;

        public object SyncRoot
        {
            get
            {
                if (_syncRoot == null)

                    _syncRoot = Interlocked.CompareExchange(ref _syncRoot, new object(), null);

                return _syncRoot;
            }
        }

        public bool IsSynchronized => false;

#if WinCopies3
        public void Clear() => throw GetReadOnlyListOrCollectionException();
#endif
    }

    public abstract class ReadOnlySimpleLinkedList : ReadOnlySimpleLinkedListBase, ISimpleLinkedList
    {
        public abstract object Peek();

#if WinCopies3
        public abstract bool TryPeek(out object result);
#endif
    }
}
