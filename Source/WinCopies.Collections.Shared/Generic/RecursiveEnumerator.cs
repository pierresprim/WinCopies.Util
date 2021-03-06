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

#if CS7

using WinCopies.Collections.DotNetFix.Generic;

using static WinCopies
#if !WinCopies3
    .Util.Util;
#else
    .ThrowHelper;
#endif

namespace WinCopies.Collections.Generic
{
    public class RecursiveEnumerator<T> : Enumerator<IRecursiveEnumerable<T>, T>
    {
        protected
#if !WinCopies3
            IStack
#else
IStackBase
#endif
            <System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>> InnerStack
        { get; private set; }

        private
#if !WinCopies3
bool _completed = false;
#else
            T _current;
            
            /// <summary>
            /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
            /// </summary>
        protected override T CurrentOverride => _current;

        public override bool? IsResetSupported => null;

        protected override void ResetOverride()
        {
            base.ResetOverride();

            InnerStack.Clear();
        }

        protected override void ResetCurrent() => _current = default;
#endif

        public RecursiveEnumerator(in System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> enumerable, in IStack<System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>> stack) : base(enumerable ?? throw GetArgumentNullException(nameof(enumerable))) => InnerStack = stack;

        public RecursiveEnumerator(in System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> enumerable) : this(enumerable, new WinCopies.Collections.DotNetFix.Generic.Stack<System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>>())
        {
            // Left empty.
        }

        public RecursiveEnumerator(IRecursiveEnumerableProviderEnumerable<T> enumerable, in IStack<System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>> stack) : base(new Enumerable<IRecursiveEnumerable<T>>(() => (enumerable ?? throw GetArgumentNullException(nameof(enumerable))).GetRecursiveEnumerator())) => InnerStack = stack;

        public RecursiveEnumerator(in IRecursiveEnumerableProviderEnumerable<T> enumerable) : this(enumerable, new WinCopies.Collections.DotNetFix.Generic.Stack<System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>>())
        {
            // Left empty.
        }

        protected override bool MoveNextOverride()
        {
#if !WinCopies3
            if (_completed) return false;
#endif

            System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>> enumerator;

            void push(in IRecursiveEnumerable<T> enumerable)
            {
                enumerator = enumerable.GetRecursiveEnumerator();

#if !WinCopies3
                Current
#else
                _current
#endif
                    = enumerable.Value;

                InnerStack.Push(enumerator);
            }

            while (true)
            {
                if (
#if !WinCopies3
                    InnerStack.Count == 0
#else
                    !InnerStack.HasItems
#endif
                    )
                {
                    if (InnerEnumerator.MoveNext())
                    {
                        push(InnerEnumerator.Current);

                        return true;
                    }

#if !WinCopies3
                    _completed = true;
#endif

                    return false;
                }

                enumerator = InnerStack.Peek();

                if (enumerator.MoveNext())
                {
                    push(enumerator.Current);

                    return true;
                }

                else

                    _ = InnerStack.Pop();
            }
        }

        protected override void
#if !WinCopies3
            Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
#else
DisposeManaged()
        {
            base.DisposeManaged();
#endif
            // {
            InnerStack = null;

            // _enumerateFunc = null;
            // }
        }
    }
}

#endif
