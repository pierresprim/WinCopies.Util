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

using WinCopies.Collections.DotNetFix.Generic;

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections.DotNetFix
{
    public interface IEnumerableSimpleLinkedListBase : ISimpleLinkedListBase2
    {
        // Left empty.
    }

    [Serializable]
    public abstract class EnumerableSimpleLinkedListBase : IEnumerableSimpleLinkedListBase
    {
        [NonSerialized]
        private uint _enumeratorsCount = 0;
        [NonSerialized]
        private uint _enumerableVersion = 0;
        [NonSerialized]
        private object
#if CS8
            ?
#endif
            _syncRoot;

        public object SyncRoot => _syncRoot
#if CS8
            ??=
#else
            ?? (_syncRoot =
#endif
            _syncRoot = Interlocked.CompareExchange(ref _syncRoot, new object(), null)
#if !CS8
            )
#endif
            ;

        public bool IsSynchronized => false;

        private protected uint EnumerableVersion => _enumerableVersion;

        public abstract uint Count { get; }

        public abstract bool HasItems { get; }

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

        public abstract class Enumerator<TItems, TNode, TList> : Collections.Generic.Enumerator<TItems>, IUIntCountableEnumerator<TItems> where TList : EnumerableSimpleLinkedListBase, IUIntCountable
        {
            private readonly uint _version;
            private bool _first = true;

            protected TList List { get; private set; }

            public override bool? IsResetSupported => true;

            protected abstract TNode FirstNode { get; }
            public TNode CurrentNode { get; private set; }
            protected abstract TNode NextNode { get; }

            public uint Count => List.Count;

            public Enumerator(in TList list)
            {
                _version = (List = list).EnumerableVersion;
                ResetOverride();
            }

            protected override void ResetOverride2()
            {
                ThrowIfVersionHasChanged(List.EnumerableVersion, _version);

                _first = true;
                CurrentNode = FirstNode;
            }

            protected override bool MoveNextOverride()
            {
                ThrowIfVersionHasChanged(List.EnumerableVersion, _version);

                if (_first)
                {
                    _first = false;

                    return CurrentNode != null;
                }

                if (NextNode == null)
                {
                    CurrentNode = default;

                    return false;
                }

                CurrentNode = NextNode;

                return true;
            }

            protected override void DisposeUnmanaged()
            {
                List.DecrementEnumeratorCount();
                List = default;

                base.DisposeUnmanaged();
            }

            protected override void DisposeManaged()
            {
                base.DisposeManaged();

                CurrentNode = default;
            }
        }
    }
}
