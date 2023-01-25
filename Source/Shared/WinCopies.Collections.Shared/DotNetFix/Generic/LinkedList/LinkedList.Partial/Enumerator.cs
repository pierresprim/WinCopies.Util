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

#if CS7
using System;

using WinCopies.Collections.Generic;
using WinCopies.Util;

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public partial class LinkedList<T>
    {
        public class Enumerator : Enumerator<ILinkedListNode<T>>
        {
            private readonly uint _version;

            protected LinkedList<T> List { get; }

            protected IEnumeratorInfo2<ILinkedListNode<T>> InnerEnumerator { get; }
            protected System.Collections.IEnumerator InnerEnumerator2 => InnerEnumerator.AsFromType<System.Collections.IEnumerator>();

            protected override ILinkedListNode<T> CurrentOverride => InnerEnumerator.Current;

            public override bool? IsResetSupported => InnerEnumerator.IsResetSupported;

            public Enumerator(in IEnumeratorInfo2<ILinkedListNode<T>> enumerator, in LinkedList<T> list)
            {
                InnerEnumerator = enumerator ?? throw GetArgumentNullException(nameof(enumerator));
                List = list ?? throw GetArgumentNullException(nameof(list));

                (list._enumeratorsCount == uint.MaxValue ? throw new InvalidOperationException("Too many enumerators are currently active.") : list).IncrementEnumeratorsCount();
                _version = list._enumerableVersion;
            }

            protected override bool MoveNextOverride()
            {
                ThrowIfVersionHasChanged(List._enumerableVersion, _version);

                return InnerEnumerator2.MoveNext();
            }

            protected override void ResetOverride2()
            {
                ThrowIfVersionHasChanged(List._enumerableVersion, _version);

                InnerEnumerator2.Reset();
            }

            protected override void DisposeUnmanaged()
            {
                List.DecrementEnumeratorsCount();

                base.DisposeUnmanaged();
            }
        }

        public class UIntCountableEnumeratorInfo : Enumerator, IUIntCountableEnumeratorInfo<ILinkedListNode<T>>
        {
            public uint Count => List.Count;

            public UIntCountableEnumeratorInfo(in IEnumeratorInfo2<ILinkedListNode<T>> enumerator, in LinkedList<T> list) : base(enumerator, list) { /* Left empty. */ }
        }
    }
}
#endif
