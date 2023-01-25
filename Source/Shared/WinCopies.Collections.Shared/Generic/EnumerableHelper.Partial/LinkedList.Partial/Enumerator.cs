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

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.Generic;

namespace WinCopies.Collections
{
    public static partial class EnumerableHelper
    {
        internal partial class LinkedList
        {
            internal class Enumerator<TInterface, TClass> : Enumerator<TInterface>, IEnumeratorInfo2<TInterface> where TClass : class, TInterface, ILinkedListNodeBase<TClass>
            {
                private TClass _current;
                private Func<TClass> _reset;
                private Func<TClass> _moveNext;
                private Action __moveNext;

                protected override TInterface CurrentOverride => _current;

                public override bool? IsResetSupported => true;

                public Enumerator(ILinkedListBase<TClass> enumerable, in EnumerationDirection enumerationDirection)
                {
                    switch (enumerationDirection)
                    {
                        case EnumerationDirection.FIFO:

                            _reset = () => enumerable.FirstNode;
                            _moveNext = () => _current.Next;

                            break;

                        case EnumerationDirection.LIFO:

                            _reset = () => enumerable.LastNode;
                            _moveNext = () => _current.Previous;

                            break;

                        default:

                            throw new ArgumentOutOfRangeException(nameof(enumerationDirection));
                    }

                    ResetMoveNext();
                }

                private void ResetMoveNext() => __moveNext = () =>
                {
                    _current = _reset();

                    __moveNext = () => _current = _moveNext();
                };

                protected override bool MoveNextOverride()
                {
                    __moveNext();

                    return _current != null;
                }

                protected override void ResetCurrent() => _current = null;

                protected override void ResetOverride2() => ResetMoveNext();

                protected override void DisposeManaged()
                {
                    base.DisposeManaged(); // _current is reset to null in the ResetCurrent() method, which is called in the call stack of the base DisposeManaged() method.

                    _reset = null;
                    _moveNext = null;
                    __moveNext = null;
                }
            }

            internal static Enumerator<ILinkedListNode, Node> GetNodeEnumerator(in Enumerable enumerable, in EnumerationDirection enumerationDirection) => new Enumerator<ILinkedListNode, Node>(enumerable, enumerationDirection);
        }
    }

    namespace Generic
    {
        public static partial class EnumerableHelper<T>
        {
            internal partial class LinkedList
            {
                internal static EnumerableHelper.LinkedList.Enumerator<ILinkedListNode, Node> GetNodeEnumerator(in Enumerable enumerable, in EnumerationDirection enumerationDirection) => new EnumerableHelper.LinkedList.Enumerator<ILinkedListNode, Node>(enumerable, enumerationDirection);
            }
        }
    }
}
