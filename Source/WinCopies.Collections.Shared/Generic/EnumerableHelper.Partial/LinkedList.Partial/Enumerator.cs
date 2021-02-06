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

#if WinCopies3

using System;

using WinCopies.Collections.DotNetFix;

namespace WinCopies.Collections.Generic
{
    public static partial class EnumerableHelper<T>
    {
        internal partial class LinkedList
        {
            internal class Enumerator : Enumerator<T>, IEnumeratorInfo2<T>
            {
                private Node _current;
                private Func<Node> _reset;
                private Func<Node> _moveNext;
                private Action __moveNext;

                /// <summary>
                /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
                /// </summary>
                protected override T CurrentOverride => _current.Value;

                public override bool? IsResetSupported => true;

                public Enumerator(Enumerable enumerable, EnumerationDirection enumerationDirection)
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

                protected override void ResetOverride()
                {
                    base.ResetOverride();

                    ResetMoveNext();
                }

                protected override void DisposeManaged()
                {
                    base.DisposeManaged(); // _current is reset to null in the ResetCurrent() method, which is called in the call stack of the base DisposeManaged() method.

                    _reset = null;

                    _moveNext = null;

                    __moveNext = null;
                }
            }
        }
    }
}

#endif
