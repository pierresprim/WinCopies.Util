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

    public
#if WinCopies3
    abstract class RecursiveEnumeratorAbstract
#else
    class RecursiveEnumerator
#endif
        <T> : Enumerator<IRecursiveEnumerable<T>, T>
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

        public
#if WinCopies3
            RecursiveEnumeratorAbstract
#else
            RecursiveEnumerator
#endif
            (in System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> enumerable,
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

                    _getEnumeratorStructDelegate = (in IRecursiveEnumerable<T> _enumerable) => new RecursiveEnumeratorStruct<T>(_enumerable);

                    break;

                case RecursiveEnumerationOrder.ChildrenThenParent:
                case RecursiveEnumerationOrder.Both:

                    _getEnumeratorStructDelegate = (in IRecursiveEnumerable<T> _enumerable) => new RecursiveEnumeratorStruct2<T>(_enumerable);

                    break;

                default:

                    throw GetInvalidEnumArgumentException(nameof(enumerationOrder), enumerationOrder);
            }
#endif
        }

        public
#if WinCopies3
            RecursiveEnumeratorAbstract
#else
            RecursiveEnumerator
#endif
            (in System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> enumerable
#if WinCopies3
            , in RecursiveEnumerationOrder enumerationOrder
#endif
            ) : this(enumerable
#if WinCopies3
                , enumerationOrder
#endif
                , new DotNetFix.Generic.Stack<
#if WinCopies3
                    RecursiveEnumeratorStruct<T>
#else
                    System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>
#endif
                    >())
        { /* Left empty. */ }

        public
#if WinCopies3
            RecursiveEnumeratorAbstract
#else
            RecursiveEnumerator
#endif
            (IRecursiveEnumerableProviderEnumerable<T> enumerable
#if WinCopies3
            , in RecursiveEnumerationOrder enumerationOrder
#endif
            , in IStack<
#if WinCopies3
            RecursiveEnumeratorStruct<T>
#else
            System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>
#endif
            > stack) : this(new Enumerable<IRecursiveEnumerable<T>>(() => (enumerable ?? throw GetArgumentNullException(nameof(enumerable))).GetRecursiveEnumerator())
#if WinCopies3
, enumerationOrder
#endif
                , stack)
        { /* Left empty. */ }

        public
#if WinCopies3
            RecursiveEnumeratorAbstract
#else
            RecursiveEnumerator
#endif
            (in IRecursiveEnumerableProviderEnumerable<T> enumerable
#if WinCopies3
            , in RecursiveEnumerationOrder enumerationOrder
#endif
            ) : this(enumerable
#if WinCopies3
                , enumerationOrder
#endif
                , new DotNetFix.Generic.Stack<
#if WinCopies3
            RecursiveEnumeratorStruct<T>
#else
            System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>
#endif
            >())
        { /* Left empty. */ }

#if WinCopies3
        protected abstract bool AddAsDuplicate(T value);
#endif

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
                enumerator =
#if WinCopies3
_getEnumeratorStructDelegate(enumerable);
#else
                    enumerable.GetRecursiveEnumerator();
#endif

                InnerStack.Push(enumerator);

#if WinCopies3
                if (EnumerationOrder.HasFlag(RecursiveEnumerationOrder.ParentThenChildren))
                {
                    _current
#else
                    Current
#endif
                    = enumerable.Value;

#if WinCopies3
                    return true;
                }

                return false;
#endif
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
#if WinCopies3
                        if (
#endif
                        push(InnerEnumerator.Current)
#if WinCopies3
                            )
#else
                            ;
#endif

                            return true;

#if WinCopies3
                        continue;
#endif
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
#if WinCopies3
                    if (
#endif
                    push(enumerator.
#if WinCopies3
                    Enumerator.
#endif
                Current)
#if WinCopies3
                        )
#else
                        ;
#endif

                        return true;

#if WinCopies3
                    continue;
#endif
                }

                else
#if WinCopies3
                {
                    enumerator
#else
                    _
#endif
                        = InnerStack.Pop();

#if WinCopies3
                    T getCurrent() => ((RecursiveEnumeratorStruct2<T>)enumerator).Value;

                    void addCurrent() => _current = getCurrent();

                    if (EnumerationOrder.HasFlag(RecursiveEnumerationOrder.ChildrenThenParent))
                    {
                        if (EnumerationOrder.HasFlag(RecursiveEnumerationOrder.ParentThenChildren))

                            if (AddAsDuplicate(getCurrent()))
                            {
                                addCurrent();

                                OnCurrentAlreadyParsed();

                                return true;
                            }

                            else

                                continue;

                        addCurrent();

                        return true;
                    }
                }
#endif
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

#if WinCopies3
            _getEnumeratorStructDelegate = null;
#endif

            // _enumerateFunc = null;
        }
    }

#if WinCopies3
    public class RecursiveEnumerator<T> : RecursiveEnumeratorAbstract<T>
    {
        public RecursiveEnumerator(in System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> enumerable,
             in RecursiveEnumerationOrder enumerationOrder,
             in IStack<RecursiveEnumeratorStruct<T>> stack) : base(enumerable, enumerationOrder, stack) { /* Left empty. */ }

        public RecursiveEnumerator(in System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> enumerable, in RecursiveEnumerationOrder enumerationOrder) : base(enumerable, enumerationOrder) { /* Left empty. */ }

        public RecursiveEnumerator(IRecursiveEnumerableProviderEnumerable<T> enumerable, in RecursiveEnumerationOrder enumerationOrder, in IStack<RecursiveEnumeratorStruct<T>> stack) : base(enumerable, enumerationOrder, stack) { /* Left empty. */ }

        public RecursiveEnumerator(in IRecursiveEnumerableProviderEnumerable<T> enumerable, in RecursiveEnumerationOrder enumerationOrder) : base(enumerable, enumerationOrder) { /* Left empty. */ }

        protected override bool AddAsDuplicate(T value) => true;
    }
#endif
}

#endif
