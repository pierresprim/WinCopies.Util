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

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Util;

using static WinCopies.Collections.RecursiveEnumeration;
using static WinCopies.ThrowHelper;

namespace WinCopies.Collections
{
    [Flags]
    public enum RecursiveEnumerationOrder : byte
    {
        ParentThenChildren = 1,

        ChildrenThenParent = 2,

        Both = ParentThenChildren | ChildrenThenParent
    }

    public enum RecursiveEnumerationMode : byte
    {
        ContainersThenItems = 1,

        ItemsThenContainers = 2
    }

    public enum RecursiveEnumerationModeEx : byte
    {
        /// <summary>
        /// Does not sort items.
        /// </summary>
        None = 0,

        ContainersThenItems = RecursiveEnumerationMode.ContainersThenItems,

        ItemsThenContainers = RecursiveEnumerationMode.ItemsThenContainers
    }

    public static class RecursiveEnumeration
    {
        public static void ThrowIfInvalidOrder(in string parameterName, in RecursiveEnumerationOrder enumerationOrder) => ThrowIfInvalidEnumValue(parameterName, enumerationOrder, RecursiveEnumerationOrder.ParentThenChildren, RecursiveEnumerationOrder.Both);
        public static void ThrowIfInvalidMode(in string parameterName, in RecursiveEnumerationMode enumerationMode) => ThrowIfInvalidEnumValue(parameterName, enumerationMode, RecursiveEnumerationMode.ContainersThenItems, RecursiveEnumerationMode.ItemsThenContainers);
        public static void ThrowIfInvalidModeEx(in string parameterName, in RecursiveEnumerationModeEx enumerationMode) => ThrowIfInvalidEnumValue(parameterName, enumerationMode, RecursiveEnumerationModeEx.None, RecursiveEnumerationModeEx.ItemsThenContainers);

        public static RecursiveEnumerator<T> CreateEnumerator<T>(in IRecursiveEnumerable<T> enumerable,
            in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode,
            in IStackBase<IRecursiveEnumeratorStruct<T>> stack) => new
#if !CS9
                RecursiveEnumerator<T>
#endif
            (new SingletonEnumerator<IRecursiveEnumerable<T>>(enumerable), enumerationOrder, enumerationMode, stack);

        public static RecursiveEnumerator<T> CreateEnumerator<T>(in IRecursiveEnumerable<T> enumerable,
            in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode) => new
#if !CS9
                RecursiveEnumerator<T>
#endif
            (new SingletonEnumerator<IRecursiveEnumerable<T>>(enumerable), enumerationOrder, enumerationMode);

        public static DelegateRecursiveEnumerator<T> CreateEnumerator<T>(in IRecursiveEnumerable<T> enumerable,
            in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode,
            in IStackBase<IRecursiveEnumeratorStruct<T>> stack, in Predicate<T> addAsDuplicatePredicate) => new
#if !CS9
                DelegateRecursiveEnumerator<T>
#endif
            (new SingletonEnumerator<IRecursiveEnumerable<T>>(enumerable), enumerationOrder, enumerationMode, stack, addAsDuplicatePredicate);

        public static DelegateRecursiveEnumerator<T> CreateEnumerator<T>(in IRecursiveEnumerable<T> enumerable,
            in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode, in Predicate<T> addAsDuplicatePredicate) => new
#if !CS9
                DelegateRecursiveEnumerator<T>
#endif
            (new SingletonEnumerator<IRecursiveEnumerable<T>>(enumerable), enumerationOrder, enumerationMode, addAsDuplicatePredicate);
    }
    namespace Generic
    {
        public interface IRecursiveEnumeratorStruct<T> :
#if CS8
            DotNetFix
#else
            System.Collections
#endif
            .Generic.IEnumerable<T>
        {
            System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>> Enumerator { get; }

            T Value { get; }
        }

        public abstract class RecursiveEnumeratorAbstract<T> : Enumerator<IRecursiveEnumerable<T>, T>
        {
            private readonly struct RecursiveEnumeratorStruct : IRecursiveEnumeratorStruct<T>
            {
                private readonly IRecursiveEnumerable<T> _enumerable;

                public System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>> Enumerator { get; }

                public T Value => _enumerable.Value;

                public RecursiveEnumeratorStruct(in IRecursiveEnumerable<T> enumerable, in System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>> enumerator)
                {
                    _enumerable = enumerable;
                    Enumerator = enumerator;
                }

                public System.Collections.Generic.IEnumerator<T> GetEnumerator() => _enumerable.GetItemsEnumerator();
#if !CS8
                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
#endif
            }

            private T
#if CS9
                ?
#endif
                _current;
            private IRecursiveEnumeratorStruct<T>
#if CS8
                ?
#endif
                _currentEnumerator;
            private System.Collections.Generic.IEnumerator<T>
#if CS8
                ?
#endif
                _currentItemEnumerator;
            private PredicateIn<IRecursiveEnumerable<T>> _push;
            private PredicateIn<bool> _moveNext;
            private Func<bool> _moveNextFunc;
            private Func<bool> _itemsMoveNext;

            protected IStackBase<IRecursiveEnumeratorStruct<T>> InnerStack { get; private set; }

            /// <summary>
            /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
            /// </summary>
            protected override T CurrentOverride => _current;

            protected IRecursiveEnumeratorStruct<T> CurrentEnumerator => GetOrThrowIfDisposed(_currentEnumerator);
            protected System.Collections.Generic.IEnumerator<T> CurrentItemEnumerator => GetOrThrowIfDisposed(_currentItemEnumerator);

            public override bool? IsResetSupported => null;

            public RecursiveEnumerationOrder EnumerationOrder { get; }
            public RecursiveEnumerationModeEx EnumerationMode { get; }

            private RecursiveEnumeratorAbstract(in System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>> enumerator, in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode, in IStackBase<IRecursiveEnumeratorStruct<T>> stack,
#if CS7
(
#else
                ValueTuple<
#endif
                    ConverterIn<IRecursiveEnumerableProvider<T>, System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>>
#if CS7
                    converter
#endif
                    , Converter<RecursiveEnumeratorAbstract<T>, Func<bool>>
#if CS7
                    itemsMoveNext)
#else
                    >
#endif
                    converters) : base(enumerator)
            {
                ThrowIfInvalidOrder(nameof(enumerationOrder), enumerationOrder);
                ThrowIfInvalidModeEx(nameof(enumerationMode), enumerationMode);

                _itemsMoveNext = converters.
#if CS7
                    itemsMoveNext
#else
                    item2
#endif
                    (this);

                InnerStack = stack;
                EnumerationOrder = enumerationOrder;
                EnumerationMode = enumerationMode;

                void push(in IRecursiveEnumerable<T> _enumerable)
                {
                    _currentEnumerator = new RecursiveEnumeratorStruct(_enumerable, converters.
#if CS7
                        converter
#else
                        item1
#endif
                        (_enumerable));

                    InnerStack.Push(_currentEnumerator);
                }

                bool moveNext(in bool value) => value && _push(_currentEnumerator.Enumerator.Current);

                _push = EnumerationOrder.HasFlag(RecursiveEnumerationOrder.ParentThenChildren) ? (in IRecursiveEnumerable<T> _enumerable) =>
                {
                    push(_enumerable);

                    _current = _enumerable.Value;

                    return true;
                }
                :
#if !CS9
                (PredicateIn<IRecursiveEnumerable<T>>)(
#endif
                (in IRecursiveEnumerable<T> _enumerable) =>
                {
                    push(_enumerable);

                    return false;
                }
#if !CS9
                )
#endif
                ;

                if (EnumerationOrder.HasFlag(RecursiveEnumerationOrder.ChildrenThenParent))
                {
                    Func<bool> func = EnumerationOrder.HasFlag(RecursiveEnumerationOrder.ParentThenChildren) ? () =>
                    {
                        if (AddAsDuplicate(_current))
                        {
                            OnCurrentAlreadyParsed();

                            return true;
                        }

                        else

                            return false;
                    }
                    :
#if !CS9
                    (Func<bool>)
#endif
                        Bool.True;

                    _moveNext = (in bool value) =>
                    {
                        if (value)

                            return moveNext(value);

                        _current = (_currentEnumerator = InnerStack.Pop()).Value;
                        _currentEnumerator.Enumerator.Dispose();

                        return func();
                    };
                }

                else

                    _moveNext = moveNext;

                ResetMoveNext();
            }

            public RecursiveEnumeratorAbstract(in System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>> enumerator, in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode, in IStackBase<IRecursiveEnumeratorStruct<T>> stack) : this(enumerator, enumerationOrder, enumerationMode, stack, GetDelegates(enumerationMode)) { /* Left empty. */ }

            public RecursiveEnumeratorAbstract(in System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>> enumerator, in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode) : this(enumerator, enumerationOrder, enumerationMode, EnumerableHelper<IRecursiveEnumeratorStruct<T>>.GetStack()) { /* Left empty. */ }

            public RecursiveEnumeratorAbstract(in System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> enumerable, in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode, in IStackBase<IRecursiveEnumeratorStruct<T>> stack) : this(enumerable.GetEnumerator(), enumerationOrder, enumerationMode, stack, GetDelegates(enumerationMode)) { /* Left empty. */ }

            public RecursiveEnumeratorAbstract(in System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> enumerable, in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode) : this(enumerable, enumerationOrder, enumerationMode, EnumerableHelper<IRecursiveEnumeratorStruct<T>>.GetStack()) { /* Left empty. */ }

            private RecursiveEnumeratorAbstract(in IRecursiveEnumerableProvider<T> enumerable, in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode, in IStackBase<IRecursiveEnumeratorStruct<T>> stack,
#if CS7
                (
#else
                ValueTuple<
#endif
                ConverterIn<IRecursiveEnumerableProvider<T>, System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>>
#if CS7
                    converter
#endif
                    , Converter<RecursiveEnumeratorAbstract<T>, Func<bool>>
#if CS7
                    itemsMoveNext)
#else
                    >
#endif
                    converters) : this(converters.
#if CS7
                        converter
#else
                        item1
#endif
                        (enumerable), enumerationOrder, enumerationMode, stack, converters)
            { /* Left empty. */ }

            public RecursiveEnumeratorAbstract(in IRecursiveEnumerableProvider<T> enumerable, in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode, in IStackBase<IRecursiveEnumeratorStruct<T>> stack) : this(enumerable, enumerationOrder, enumerationMode, stack, GetDelegates(enumerationMode)) { /* Left empty. */ }

            public RecursiveEnumeratorAbstract(in IRecursiveEnumerableProvider<T> enumerable, in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode) : this(enumerable, enumerationOrder, enumerationMode, EnumerableHelper<IRecursiveEnumeratorStruct<T>>.GetStack(), GetDelegates(enumerationMode)) { /* Left empty. */ }

            private static
#if CS7
                (
#else
                ValueTuple<
#endif
                ConverterIn<IRecursiveEnumerableProvider<T>, System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>>, Converter<RecursiveEnumeratorAbstract<T>, Func<bool>>
#if CS7
                    )
#else
                    >
#endif
                    GetDelegates(in RecursiveEnumerationModeEx enumerationMode)
            {
                if (enumerationMode == RecursiveEnumerationModeEx.None)

                    return
#if !CS7
                        new ValueTuple<ConverterIn<IRecursiveEnumerableProvider<T>, System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>>, Converter<RecursiveEnumeratorAbstract<T>, Func<bool>>>
#endif
                        ((in IRecursiveEnumerableProvider<T> enumerable) => enumerable.GetRecursiveEnumerator(),

                     enumerator => enumerator.EnumerateContainers);

                Converter<RecursiveEnumeratorAbstract<T>, Func<bool>> itemsMoveNextConverter
#if CS8
                =
#else
                ;

                switch (
#endif
                    enumerationMode
#if CS8
                    switch
#else
                )
#endif
                {
#if !CS8
                    case
#endif
                        RecursiveEnumerationModeEx.ContainersThenItems
#if CS8
                            =>
#else
                            :
                        itemsMoveNextConverter =
#endif
                            enumerator => () => enumerator.EnumerateContainers() || enumerator.EnumerateItems()
#if CS8
                        ,
#else
                        ;
                        break;

                    case
#endif
                        RecursiveEnumerationModeEx.ItemsThenContainers
#if CS8
                            =>
#else
                            :
                        itemsMoveNextConverter =
#endif
                            enumerator => () => enumerator.EnumerateItems() || enumerator.EnumerateContainers()
#if CS8
                        ,
                        _ =>
#else
                        ;
                        break;

                    default:
#endif
                        throw new NotImplementedException()
#if CS8
                };
#else
                    ;
                }
#endif
                return
#if !CS7
                    new ValueTuple<ConverterIn<IRecursiveEnumerableProvider<T>, System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>>, Converter<RecursiveEnumeratorAbstract<T>, Func<bool>>>
#endif
                    ((in IRecursiveEnumerableProvider<T> enumerable) => enumerable.GetContainersEnumerator(), itemsMoveNextConverter);
            }

            protected override void ResetOverride2() => ResetMoveNext();

            protected override void OnResetOrDisposed()
            {
                base.OnResetOrDisposed();

                InnerStack.Clear();
            }

            protected override void ResetCurrent() => _current = default;

            protected abstract bool AddAsDuplicate(T value);

            protected virtual void OnCurrentAlreadyParsed() { /* Left empty. */ }

            private void UpdateCurrent() => _current = _currentItemEnumerator.Current;

            private bool EnumerateContainers() => _moveNext(_currentEnumerator.Enumerator.MoveNext());
            private bool EnumerateItems()
            {
                bool enumerateFromItemsEnumerator()
                {
                    if (_currentItemEnumerator.MoveNext())
                    {
                        UpdateCurrent();

                        return true;
                    }

                    return (_moveNextFunc = MoveNext)();
                }

                if ((_currentItemEnumerator = _currentEnumerator.GetEnumerator()).MoveNext())
                {
                    _moveNextFunc = enumerateFromItemsEnumerator;

                    UpdateCurrent();

                    return true;
                }

                return false;
            }

            private new bool MoveNext()
            {
            LOOP:
                if (InnerStack.HasItems)
                {
                    _currentEnumerator = InnerStack.Peek();

                    if (_itemsMoveNext())

                        return true;

                    goto LOOP;
                }

                if (InnerEnumerator.MoveNext())
                {
                    if (_push(InnerEnumerator.Current))

                        return true;

                    goto LOOP;
                }

                return false;
            }

            private void ResetMoveNext() => _moveNextFunc = MoveNext;

            protected override bool MoveNextOverride() => _moveNextFunc();

            protected override void DisposeManaged()
            {
                base.DisposeManaged();

                InnerStack = null;

                _push = null;
                _moveNext = null;
                _itemsMoveNext = null;
            }
        }

        public class RecursiveEnumerator<T> : RecursiveEnumeratorAbstract<T>
        {
            public RecursiveEnumerator(in System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>> enumerator,
                 in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode,
                 in IStackBase<IRecursiveEnumeratorStruct<T>> stack) : base(enumerator, enumerationOrder, enumerationMode, stack) { /* Left empty. */ }

            public RecursiveEnumerator(in System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>> enumerator,
                in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode) : base(enumerator, enumerationOrder, enumerationMode) { /* Left empty. */ }

            public RecursiveEnumerator(in System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> enumerable,
                 in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode,
                 in IStackBase<IRecursiveEnumeratorStruct<T>> stack) : base(enumerable, enumerationOrder, enumerationMode, stack) { /* Left empty. */ }

            public RecursiveEnumerator(in System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> enumerable,
                in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode) : base(enumerable, enumerationOrder, enumerationMode) { /* Left empty. */ }

            public RecursiveEnumerator(in IRecursiveEnumerableProvider<T> enumerable,
                in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode,
                in IStackBase<IRecursiveEnumeratorStruct<T>> stack) : base(enumerable, enumerationOrder, enumerationMode, stack) { /* Left empty. */ }

            public RecursiveEnumerator(in IRecursiveEnumerableProvider<T> enumerable,
                in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode) : base(enumerable, enumerationOrder, enumerationMode) { /* Left empty. */ }

            protected override bool AddAsDuplicate(T value) => true;
        }

        public class DelegateRecursiveEnumerator<T> : RecursiveEnumeratorAbstract<T>
        {
            private readonly Predicate<T> _addAsDuplicatePredicate;

            public DelegateRecursiveEnumerator(in System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>> enumerator,
                 in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode,
                 in IStackBase<IRecursiveEnumeratorStruct<T>> stack, in Predicate<T> addAsDuplicatePredicate) : base(enumerator, enumerationOrder, enumerationMode, stack) => _addAsDuplicatePredicate = addAsDuplicatePredicate;

            public DelegateRecursiveEnumerator(in System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>> enumerator,
                in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode,
                in Predicate<T> addAsDuplicatePredicate) : base(enumerator, enumerationOrder, enumerationMode) => _addAsDuplicatePredicate = addAsDuplicatePredicate;

            public DelegateRecursiveEnumerator(in System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> enumerable,
                 in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode,
                 in IStackBase<IRecursiveEnumeratorStruct<T>> stack, in Predicate<T> addAsDuplicatePredicate) : base(enumerable, enumerationOrder, enumerationMode, stack) => _addAsDuplicatePredicate = addAsDuplicatePredicate;

            public DelegateRecursiveEnumerator(in System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> enumerable,
                in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode,
                in Predicate<T> addAsDuplicatePredicate) : base(enumerable, enumerationOrder, enumerationMode) => _addAsDuplicatePredicate = addAsDuplicatePredicate;

            public DelegateRecursiveEnumerator(in IRecursiveEnumerableProvider<T> enumerable,
                in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode,
                in IStackBase<IRecursiveEnumeratorStruct<T>> stack, in Predicate<T> addAsDuplicatePredicate) : base(enumerable, enumerationOrder, enumerationMode, stack) => _addAsDuplicatePredicate = addAsDuplicatePredicate;

            public DelegateRecursiveEnumerator(in IRecursiveEnumerableProvider<T> enumerable,
                in RecursiveEnumerationOrder enumerationOrder, in RecursiveEnumerationModeEx enumerationMode,
                in Predicate<T> addAsDuplicatePredicate) : base(enumerable, enumerationOrder, enumerationMode) => _addAsDuplicatePredicate = addAsDuplicatePredicate;

            protected override bool AddAsDuplicate(T value) => _addAsDuplicatePredicate(value);
        }
    }
}
