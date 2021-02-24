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
using System.Threading;

namespace WinCopies.Collections.DotNetFix
{
#if WinCopies3
    public interface IEnumerableSimpleLinkedListBase : ISimpleLinkedListBase2
    {
        // Left empty.
    }
#endif

    public abstract class EnumerableSimpleLinkedListBase
#if WinCopies3
: IEnumerableSimpleLinkedListBase
#endif
    {
        [NonSerialized]
        private uint _enumeratorsCount = 0;
        [NonSerialized]
        private uint _enumerableVersion = 0;
        [NonSerialized]
        private object _syncRoot;

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

        private protected uint EnumerableVersion => _enumerableVersion;

        public abstract uint Count { get; }

        public bool HasItems => Count != 0u;

        public bool IsReadOnly => false;

        public abstract void Clear();

        protected void UpdateEnumerableVersion()
        {
            if (_enumeratorsCount != 0)

                _enumerableVersion++;
        }

        protected void IncrementEnumeratorCount() => _enumeratorsCount++;

        protected void DecrementEnumeratorCount()
        {
            if (--_enumeratorsCount == 0)

                _enumerableVersion = 0;
        }
    }
}
