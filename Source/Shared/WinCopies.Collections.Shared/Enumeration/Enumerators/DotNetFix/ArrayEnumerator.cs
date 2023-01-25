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
using System.Collections;

using WinCopies.Collections.Generic;
using WinCopies.Util;

using static WinCopies.Collections.DotNetFix.ArrayEnumerationOptions;
using static WinCopies.ThrowHelper;

namespace WinCopies.Collections.DotNetFix
{
    [Flags]
    public enum ArrayEnumerationOptions : byte
    {
        None = 0,
        Reverse = 1,
        Circular = 2,
        Reserved = 4,
        Infinite = Circular | Reserved
    }

    public class ListEnumerator<T> : Generic.ArrayEnumeratorBase<object
#if CS8
        ?
#endif
        , T> where T : System.Collections.IList
    {
        protected override object
#if CS8
            ?
#endif
            CurrentOverride => Array[CurrentIndex];

        public override int Count => Array.Count;

        public ListEnumerator(in T array, in ArrayEnumerationOptions options = None, in int? startIndex = null) : base(array, options, startIndex) { /* Left empty. */ }
    }

    public class ListEnumerator : ListEnumerator<System.Collections.IList>
    {
        public ListEnumerator(in System.Collections.IList array, in ArrayEnumerationOptions options = None, in int? startIndex = null) : base(array, options, startIndex) { /* Left empty. */ }
    }

    public class ArrayEnumerator<T> : Generic.ArrayEnumeratorBase<object
#if CS8
        ?
#endif
        , T> where T : ICountable, IIndexableR
    {
        protected override object
#if CS8
            ?
#endif
            CurrentOverride => Array[CurrentIndex];

        public override int Count => Array.Count;

        public ArrayEnumerator(in T array, in ArrayEnumerationOptions options = None, in int? startIndex = null) : base(array, options, startIndex) { /* Left empty. */ }
    }

    public static class ArrayEnumerator
    {
        public static ListEnumerator Create(in System.Collections.IList array, ArrayEnumerationOptions options = None, in int? startIndex = null) => new
#if !CS9
            ListEnumerator
#endif
            (array, options, startIndex);

        public static Generic.ArrayEnumerator<T> Create<T>(in
#if CS7
            System.Collections
#else
            Extensions
#endif
            .Generic.IReadOnlyList<T> array, in ArrayEnumerationOptions options = None, in int? startIndex = null) => new
#if !CS9
            Generic.ArrayEnumerator<T>
#endif
            (array, options, startIndex);

        public static Generic.ListEnumerator<T> Create<T>(in System.Collections.Generic.IList<T> array, in ArrayEnumerationOptions options = None, in int? startIndex = null) => new
#if !CS9
            Generic.ListEnumerator<T>
#endif
            (array, options, startIndex);

        public static ArrayEnumerator<T> Create<T>(in T array, in ArrayEnumerationOptions options = None, in int? startIndex = null) where T : ICountable, IIndexableR => new
#if !CS9
            ArrayEnumerator<T>
#endif
            (array, options, startIndex);
    }

    namespace Generic
    {
        public abstract class ArrayEnumeratorBase<TItems, TList> : Enumerator<TItems>, ICountableDisposableEnumeratorInfo<TItems>
        {
            private TList _array;
            private int _currentIndex;
            private int? _defaultStartIndex;
            private ArrayEnumerationOptions _options;
            private Func<bool> _condition;
            private Action _moveNext;

            protected TList Array => IsDisposed ? throw GetExceptionForDispose(false) : _array;

            public abstract int Count { get; }

            protected int CurrentIndex => IsDisposed ? throw GetExceptionForDispose(false) : _currentIndex;

            public ArrayEnumerationOptions Options
            {
                get => _options;

                set
                {
                    ThrowIfStartedOrDisposed();

                    _options = value;
                }
            }

            public override bool? IsResetSupported => true;

            public int StartIndex { get; private set; }

            public bool IsCircular => _options.HasFlag(Circular);

            public int? DefaultStartIndex { get => _defaultStartIndex; set => UpdateStartIndex(value); }

            public ArrayEnumeratorBase(in TList array, ArrayEnumerationOptions options = None, in int? startIndex = null)
            {
                _array = array
#if CS8
                    ??
#else
                    == null ?
#endif
                    throw GetArgumentNullException(nameof(array))
#if !CS8
                    : array
#endif
                    ;

                UpdateStartIndex(startIndex);

                _options = options;

                ResetMoveNext();
                ResetCurrent();
            }

            private void UpdateStartIndex(in int? index)
            {
                ThrowIfStartedOrDisposed();

                _defaultStartIndex = index.HasValue && (index.Value < 0 || index.Value >= Count) ? throw new ArgumentOutOfRangeException(nameof(index), index, $"The given index is less than zero or greater than or equal to the length of {nameof(Array)}.") : index;
            }

            protected virtual void ResetMoveNext()
            {
                int count = Count;

                if (count == 0)
                {
                    _moveNext = Delegates.EmptyVoid;
                    _condition = Bool.False;
                }

                else if (count == 1)
                {
                    _moveNext = Delegates.EmptyVoid;
                    _condition = () =>
                    {
                        _condition = Bool.False;

                        return true;
                    };
                }

                else
                {
                    bool isInfinite(in Action action)
                    {
                        if (_options.HasFlag(Infinite))
                        {
                            _condition = Bool.True;

                            action();

                            return true;
                        }

                        return false;
                    }

                    void whenCircular(in Action action)
                    {
                        _condition = () =>
                        {
                            _condition = () =>
                            {
                                if (_currentIndex == StartIndex)

                                    _condition = Bool.False;

                                return true;
                            };

                            return true;
                        };

                        action();
                    }

                    if (_options.HasFlag(Reverse))
                    {
                        StartIndex = _defaultStartIndex.HasValue ? _defaultStartIndex.Value + 1 : count;

                        void updateIndex() => _currentIndex--;

                        void setMoveNext() => _moveNext = () =>
                        {
                            if (_currentIndex == 0)

                                _currentIndex = count;

                            else

                                updateIndex();
                        };

                        if (isInfinite(setMoveNext))

                            return;

                        else if (IsCircular)

                            whenCircular(setMoveNext);

                        else
                        {
                            _condition = () => _currentIndex >= 0;
                            _moveNext = updateIndex;
                        }
                    }

                    else
                    {
                        StartIndex = _defaultStartIndex.HasValue ? _defaultStartIndex.Value - 1 : -1;

                        void updateIndex() => _currentIndex++;

                        void setMoveNext()
                        {
                            count--;

                            _moveNext = () =>
                            {
                                if (_currentIndex == count)

                                    _currentIndex = 0;

                                else

                                    updateIndex();
                            };
                        }

                        if (isInfinite(setMoveNext))

                            return;

                        else if (IsCircular)

                            whenCircular(setMoveNext);

                        else
                        {
                            _condition = () => _currentIndex < count;
                            _moveNext = updateIndex;
                        }
                    }
                }
            }

            private void DisposeDelegates()
            {
                _condition = null;
                _moveNext = null;
            }

            protected override bool MoveNextOverride()
            {
                _moveNext();

                if (_condition())

                    return true;

                DisposeDelegates();

                return false;
            }

            protected virtual void ResetIndex() => _currentIndex = StartIndex;

            protected override void ResetCurrent()
            {
                base.ResetCurrent();

                ResetIndex();
            }

            protected override void ResetOverride2() => ResetMoveNext();

            protected override void DisposeManaged()
            {
                _array = default;

                DisposeDelegates();

                StartIndex = -1;

                base.DisposeManaged();
            }
        }

        public class ArrayEnumerator<TItems, TArray> : ArrayEnumeratorBase<TItems, TArray> where TArray : IIndexableR<TItems>, ICountable
        {
            protected override TItems CurrentOverride => Array[CurrentIndex];

            public override int Count => Array.Count;

            public ArrayEnumerator(in TArray array, in ArrayEnumerationOptions options = None, in int? startIndex = null) : base(array, options, startIndex) { /* Left empty. */ }
        }

        public class ArrayEnumerator<T> : ArrayEnumeratorBase<T,
#if CS7
            System.Collections.Generic
#else
            Extensions.Generic
#endif
            .IReadOnlyList<T>>
        {
            protected override T CurrentOverride => Array[CurrentIndex];

            public override int Count => Array.Count;

            public ArrayEnumerator(in
#if CS7
                System.Collections.Generic
#else
                Extensions.Generic
#endif
                .IReadOnlyList<T> array, in ArrayEnumerationOptions options = None, in int? startIndex = null) : base(array, options, startIndex) { /* Left empty. */ }
        }

        public class ListEnumerator<T> : ArrayEnumeratorBase<T, System.Collections.Generic.IList<T>>
        {
            protected override T CurrentOverride => Array[CurrentIndex];

            public override int Count => Array.Count;

            public ListEnumerator(in System.Collections.Generic.IList<T> list, in ArrayEnumerationOptions options = None, in int? startIndex = null) : base(list, options, startIndex) { /* Left empty. */ }
        }
    }
}
