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

using WinCopies.Collections.DotNetFix.Generic;

using static WinCopies
#if !WinCopies3
    .Util.Util;
#else
    .ThrowHelper;
#endif

namespace WinCopies.Collections.Generic
{
#if WinCopies3
    [Flags]
    public enum RecursiveEnumerationOrder
    {
        ParentThenChildren = 1,

        ChildrenThenParent = 2,

        Both = ParentThenChildren | ChildrenThenParent
    }

    public class RecursiveEnumeratorStruct<T>
    {
        public System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>> Enumerator { get; }

        public RecursiveEnumeratorStruct(in IRecursiveEnumerable<T> enumerable) => Enumerator = enumerable.GetRecursiveEnumerator();
    }

    public class RecursiveEnumeratorStruct2<T> : RecursiveEnumeratorStruct<T>
    {
        public T Value { get; }

        public RecursiveEnumeratorStruct2(in IRecursiveEnumerable<T> enumerable) : base(enumerable) => Value = enumerable.Value;
    }
#endif

    public class RecursiveEnumerator<T> : Enumerator<IRecursiveEnumerable<T>, T>
    {
        protected
#if !WinCopies3
            IStack
#else
IStackBase
#endif
            <
#if WinCopies3
    RecursiveEnumeratorStruct<T>
#else
    System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>
#endif
    > InnerStack
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

#if WinCopies3
        private FuncIn<IRecursiveEnumerable<T>, RecursiveEnumeratorStruct<T>> _getEnumeratorStructDelegate;

        public RecursiveEnumerationOrder EnumerationOrder { get; }
#endif

        protected override void ResetOverride()
        {
            base.ResetOverride();

            InnerStack.Clear();
        }

        protected override void ResetCurrent() => _current = default;
#endif

        public RecursiveEnumerator(in System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> enumerable,
#if WinCopies3
             in RecursiveEnumerationOrder enumerationOrder,
#endif
             in IStack<
#if WinCopies3
            RecursiveEnumeratorStruct<T>
#else
            System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>
#endif
            > stack) : base(enumerable ?? throw GetArgumentNullException(nameof(enumerable)))
        {
            InnerStack = stack;

#if WinCopies3
            EnumerationOrder = enumerationOrder;

            switch (enumerationOrder)
            {
                case RecursiveEnumerationOrder.ParentThenChildren:

                    _getEnumeratorStructDelegate = (in IRecursiveEnumerable<T> enumerable) => new RecursiveEnumeratorStruct<T>(enumerable);

                    break;

                case RecursiveEnumerationOrder.ChildrenThenParent:
                case RecursiveEnumerationOrder.Both:

                    _getEnumeratorStructDelegate = (in IRecursiveEnumerable<T> enumerable) => new RecursiveEnumeratorStruct2<T>(enumerable);

                    break;

                default:

                    throw GetInvalidEnumArgumentException(nameof(enumerationOrder), enumerationOrder);
            }
#endif
        }

        public RecursiveEnumerator(in System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> enumerable
#if WinCopies3
            , in RecursiveEnumerationOrder enumerationOrder
#endif
            ) : this(enumerable
#if WinCopies3
                , enumerationOrder
#endif
                , new DotNetFix.Generic.Stack<RecursiveEnumeratorStruct<T>>())
        {
            // Left empty.
        }

        public RecursiveEnumerator(IRecursiveEnumerableProviderEnumerable<T> enumerable
#if WinCopies3
            , in RecursiveEnumerationOrder enumerationOrder
#endif
            , in IStack<
#if WinCopies3
            RecursiveEnumeratorStruct<T>
#else
            System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>
#endif
            > stack) : this(new Enumerable<IRecursiveEnumerable<T>>(() => (enumerable ?? throw GetArgumentNullException(nameof(enumerable))).GetRecursiveEnumerator()), enumerationOrder, stack)
        {
            // Left empty.
        }

        public RecursiveEnumerator(in IRecursiveEnumerableProviderEnumerable<T> enumerable, in RecursiveEnumerationOrder enumerationOrder) : this(enumerable, enumerationOrder, new DotNetFix.Generic.Stack<
#if WinCopies3
            RecursiveEnumeratorStruct<T>
#else
            System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>
#endif
            >())
        {
            // Left empty.
        }

        protected virtual void OnCurrentAlreadyParsed() { /* Left empty. */ }

        protected override bool MoveNextOverride()
        {
#if !WinCopies3
            if (_completed) return false;

            System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>
#else
            RecursiveEnumeratorStruct<T>
#endif

enumerator;

#if WinCopies3
            bool
#else
            void
#endif
                push(in IRecursiveEnumerable<T> enumerable)
            {
                enumerator = _getEnumeratorStructDelegate(enumerable);

                InnerStack.Push(enumerator);

                if (EnumerationOrder.HasFlag(RecursiveEnumerationOrder.ParentThenChildren))
                {
#if !WinCopies3
                    Current
#else
                    _current
#endif
                    = enumerable.Value;

                    return true;
                }

                return false;
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
                        if (push(InnerEnumerator.Current))

                            return true;

                        continue;
                    }

#if !WinCopies3
                    _completed = true;
#endif

                    return false;
                }

                enumerator = InnerStack.Peek();

                if (enumerator.
#if WinCopies3
                    Enumerator.
#endif
                    MoveNext())
                {
                    if (push(enumerator.
#if WinCopies3
                    Enumerator.
#endif
                Current))

                        return true;

                    else

                        continue;
                }

                else
                {
                    enumerator = InnerStack.Pop();

                    if (EnumerationOrder.HasFlag(RecursiveEnumerationOrder.ChildrenThenParent))
                    {
                        _current = ((RecursiveEnumeratorStruct2<T>)enumerator).Value;

                        if (EnumerationOrder.HasFlag(RecursiveEnumerationOrder.ParentThenChildren))

                            OnCurrentAlreadyParsed();

                        return true;
                    }
                }
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
            InnerStack = null;

            _getEnumeratorStructDelegate = null;

            // _enumerateFunc = null;
        }
    }
}

#endif
