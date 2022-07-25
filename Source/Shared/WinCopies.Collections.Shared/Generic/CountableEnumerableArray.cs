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

using System;
using System.Collections;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;

using static WinCopies.
#if WinCopies3
    ThrowHelper;
#else
    Util.Util;
#endif

using SystemIEnumerable = System.Collections.IEnumerable;
using IEnumerator = System.Collections.IEnumerator;

namespace WinCopies.Collections
{
#if WinCopies3
    namespace Enumeration
    {
        public abstract class CountableEnumerator<TEnumerator, TCount> : System.Collections.IEnumerator where TEnumerator : System.Collections.IEnumerator
        {
            private readonly Func<TCount> _func;

            protected TEnumerator Enumerator { get; }

            object IEnumerator.Current => Enumerator.Current;

            public TCount Count => _func();

            protected CountableEnumerator(in TEnumerator enumerator, in Func<TCount> func)
            {
                Enumerator = enumerator == null ? throw GetArgumentNullException(nameof(enumerator)) : enumerator;

                _func = func ?? throw GetArgumentNullException(nameof(func));
            }

            protected virtual void OnEnumerationStarting() { /* Left empty. */ }

            protected virtual void OnEnumerationCompleted() { /* Left empty. */ }

            public bool MoveNext()
            {
                OnEnumerationStarting();

                bool result = Enumerator.MoveNext();

                OnEnumerationCompleted();

                return result;
            }

            public void Reset() => Enumerator.Reset();
        }

        public class CountableEnumerator<TEnumerator> : CountableEnumerator<TEnumerator, int>, ICountableEnumerator where TEnumerator : System.Collections.IEnumerator
        {
            public CountableEnumerator(in TEnumerator enumerator, in Func<int> func) : base(enumerator, func) { /* Left empty. */ }
        }

        public class UIntCountableEnumerator<TEnumerator> : CountableEnumerator<TEnumerator, uint>, IUIntCountableEnumerator where TEnumerator : IEnumerator
        {
            public UIntCountableEnumerator(in TEnumerator enumerator, in Func<uint> func) : base(enumerator, func) { /* Left empty. */ }
        }

        public abstract class CountableEnumerableBase<TEnumerable, TEnumerator> : SystemIEnumerable where TEnumerable : SystemIEnumerable where TEnumerator : IEnumerator
        {
            protected TEnumerable Enumerable { get; }

            protected CountableEnumerableBase(in TEnumerable enumerable) => Enumerable = enumerable == null ? throw GetArgumentNullException(nameof(enumerable)) : enumerable;

            public abstract TEnumerator GetEnumerator();

            IEnumerator SystemIEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class CountableEnumerable<TEnumerable> : CountableEnumerableBase<TEnumerable, ICountableEnumerator>, ICountableEnumerable where TEnumerable : ICountable, SystemIEnumerable
        {
            int ICountable.Count => Enumerable.Count;

            public CountableEnumerable(in TEnumerable enumerable) : base(enumerable) { /* Left empty. */ }

            public override ICountableEnumerator GetEnumerator() => new CountableEnumerator<IEnumerator>(Enumerable.GetEnumerator(), () => Enumerable.Count);
        }

        public class UIntCountableEnumerable<TEnumerable> : CountableEnumerableBase<TEnumerable, IUIntCountableEnumerator>, IUIntCountableEnumerable where TEnumerable : IUIntCountable, SystemIEnumerable
        {
            uint IUIntCountable.Count => Enumerable.Count;

            public UIntCountableEnumerable(in TEnumerable enumerable) : base(enumerable) { /* Left empty. */ }

            public override IUIntCountableEnumerator GetEnumerator() => new UIntCountableEnumerator<IEnumerator>(Enumerable.GetEnumerator(), () => Enumerable.Count);
        }

        namespace Generic
        {
            public abstract class CountableEnumerator<TEnumerator, TItems, TCount> : WinCopies.Collections.Enumeration.CountableEnumerator<TEnumerator, TCount>, System.Collections.Generic.IEnumerator<TItems> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
            {
                TItems System.Collections.Generic.IEnumerator<TItems>.Current => Enumerator.Current;

                protected CountableEnumerator(in TEnumerator enumerator, in Func<TCount> func) : base(enumerator, func) { /* Left empty. */ }

                protected virtual void Dispose(in bool disposing) => Enumerator.Dispose();

                public void Dispose()
                {
                    Dispose(true);

                    GC.SuppressFinalize(this);
                }
            }

            public abstract class CountableEnumeratorInfo<TEnumerator, TItems, TCount> : CountableEnumerator<TEnumerator, TItems, TCount>, IEnumeratorInfo<TItems> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
            {
                public bool? IsResetSupported => null;

                public bool IsStarted { get; private set; }

                public bool IsCompleted { get; private set; }

                protected CountableEnumeratorInfo(in TEnumerator enumerator, in Func<TCount> func) : base(enumerator, func) { /* Left empty. */ }

                private void UpdateStartStatus(in bool newValue)
                {
                    IsStarted = newValue;

                    IsCompleted = !newValue;
                }

                protected override void OnEnumerationStarting() => UpdateStartStatus(true);

                protected override void OnEnumerationCompleted() => UpdateStartStatus(false);
            }

            public class CountableEnumeratorInfo<TEnumerator, TItems> : CountableEnumeratorInfo<TEnumerator, TItems, int>, ICountableEnumeratorInfo<TItems> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
            {
                public CountableEnumeratorInfo(in TEnumerator enumerator, in Func<int> func) : base(enumerator, func) { /* Left empty. */ }
            }

            public class CountableEnumeratorInfo<T> : CountableEnumeratorInfo<System.Collections.Generic.IEnumerator<T>, T>
            {
                public CountableEnumeratorInfo(in System.Collections.Generic.IEnumerator<T> enumerator, in Func<int> func) : base(enumerator, func) { /* Left empty. */ }
            }

            public class UIntCountableEnumeratorInfo<TEnumerator, TItems> : CountableEnumeratorInfo<TEnumerator, TItems, uint>, IUIntCountableEnumeratorInfo<TItems> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
            {
                public UIntCountableEnumeratorInfo(in TEnumerator enumerator, in Func<uint> func) : base(enumerator, func) { /* Left empty. */ }
            }

            public class UIntCountableEnumeratorInfo<T> : UIntCountableEnumeratorInfo<System.Collections.Generic.IEnumerator<T>, T>
            {
                public UIntCountableEnumeratorInfo(in System.Collections.Generic.IEnumerator<T> enumerator, in Func<uint> func) : base(enumerator, func) { /* Left empty. */ }
            }

            public class CountableEnumerator<TEnumerator, TItems> : CountableEnumerator<TEnumerator, TItems, int>, ICountableEnumerator<TItems> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
            {
                public CountableEnumerator(in TEnumerator enumerator, in Func<int> func) : base(enumerator, func) { /* Left empty. */ }
            }

            public class CountableEnumerator<T> : CountableEnumerator<System.Collections.Generic.IEnumerator<T>, T>
            {
                public CountableEnumerator(in System.Collections.Generic.IEnumerator<T> enumerator, in Func<int> func) : base(enumerator, func) { /* Left empty. */ }
            }

            public class UIntCountableEnumerator<TEnumerator, TItems> : CountableEnumerator<TEnumerator, TItems, uint>, IUIntCountableEnumerator<TItems> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
            {
                public UIntCountableEnumerator(in TEnumerator enumerator, in Func<uint> func) : base(enumerator, func) { /* Left empty. */ }
            }

            public class UIntCountableEnumerator<T> : UIntCountableEnumerator<System.Collections.Generic.IEnumerator<T>, T>
            {
                public UIntCountableEnumerator(in System.Collections.Generic.IEnumerator<T> enumerator, in Func<uint> func) : base(enumerator, func) { /* Left empty. */ }
            }

            public class DisposableEnumerator<TEnumerator, TItems> : IDisposableEnumerator<TItems> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
            {
                private NullableGeneric<TEnumerator> _enumerator;

                protected TEnumerator Enumerator => IsDisposed ? throw GetExceptionForDispose(false) : _enumerator.Value;

                TItems System.Collections.Generic.IEnumerator<TItems>.Current => IsDisposed ? throw GetExceptionForDispose(false) : _enumerator.Value.Current;

                object IEnumerator.Current => IsDisposed ? throw GetExceptionForDispose(false) : ((IEnumerator)_enumerator.Value).Current;

                public bool IsDisposed { get; private set; }

                public DisposableEnumerator(in TEnumerator enumerator) => _enumerator = new NullableGeneric<TEnumerator>(enumerator == null ? throw GetArgumentNullException(nameof(enumerator)) : enumerator);

                bool IEnumerator.MoveNext() => IsDisposed ? throw GetExceptionForDispose(false) : _enumerator.Value.MoveNext();

                void IEnumerator.Reset()
                {
                    if (IsDisposed)

                        throw GetExceptionForDispose(false);

                    else

                        _enumerator.Value.Reset();
                }

                protected virtual void Dispose(in bool disposing)
                {
                    if (!IsDisposed)
                    {
                        if (disposing)
                        {
                            _enumerator.Value.Dispose();

                            _enumerator = null;

                            IsDisposed = true;
                        }
                    }
                }

                public void Dispose()
                {
                    Dispose(true);

                    GC.SuppressFinalize(this);
                }
            }

            public class DisposableEnumerator<T> : DisposableEnumerator<System.Collections.Generic.IEnumerator<T>, T>
            {
                public DisposableEnumerator(in System.Collections.Generic.IEnumerator<T> enumerator) : base(enumerator) { /* Left empty. */ }
            }

            public class CountableEnumerable<TEnumerable, TItems> : CountableEnumerableBase<TEnumerable, ICountableEnumerator<TItems>>, Collections.DotNetFix.Generic.ICountableEnumerable<TItems> where TEnumerable : ICountable, System.Collections.Generic.IEnumerable<TItems>
            {
#if !CS8
                int ICountable.Count => Enumerable.Count;
#endif

#if CS7
                int ICountableEnumerable<TItems, ICountableEnumerator<TItems>>.Count => Enumerable.Count;

#if !CS8
                int System.Collections.Generic.IReadOnlyCollection<TItems>.Count => Enumerable.Count;
#endif
#endif

                public CountableEnumerable(in TEnumerable enumerable) : base(enumerable) { /* Left empty. */ }

                public override ICountableEnumerator<TItems> GetEnumerator() => new CountableEnumerator<DisposableEnumerator<System.Collections.Generic.IEnumerator<TItems>, TItems>, TItems>(new DisposableEnumerator<System.Collections.Generic.IEnumerator<TItems>, TItems>(Enumerable.GetEnumerator()), () => Enumerable.Count);

#if !CS8
                System.Collections.Generic.IEnumerator<TItems> System.Collections.Generic.IEnumerable<TItems>.GetEnumerator() => GetEnumerator();
#endif
            }

            public class UIntCountableEnumerable<TEnumerable, TItems> : CountableEnumerableBase<TEnumerable, IUIntCountableEnumerator<TItems>>, Collections.DotNetFix.Generic.IUIntCountableEnumerable<TItems> where TEnumerable : IUIntCountable, System.Collections.Generic.IEnumerable<TItems>
            {
                uint IUIntCountable.Count => Enumerable.Count;

                public UIntCountableEnumerable(in TEnumerable enumerable) : base(enumerable) { /* Left empty. */ }

                public override IUIntCountableEnumerator<TItems> GetEnumerator() => new UIntCountableEnumerator<DisposableEnumerator<System.Collections.Generic.IEnumerator<TItems>, TItems>, TItems>(new DisposableEnumerator<System.Collections.Generic.IEnumerator<TItems>, TItems>(Enumerable.GetEnumerator()), () => Enumerable.Count);

#if !CS8
                System.Collections.Generic.IEnumerator<TItems> System.Collections.Generic.IEnumerable<TItems>.GetEnumerator() => GetEnumerator();
#endif
            }

            public abstract class CountableEnumerableInfoBase<TEnumerable, TEnumerator, TItems> : CountableEnumerableBase<TEnumerable, TEnumerator> where TEnumerable : ICountable, Collections.DotNetFix.Generic.IEnumerable<TItems, TEnumerator>, IEnumerableInfo<TEnumerator> where TEnumerator : IEnumerator<TItems>, IEnumeratorInfo2<TItems>
            {
                public bool SupportsReversedEnumeration => Enumerable.SupportsReversedEnumeration;

                protected CountableEnumerableInfoBase(in TEnumerable enumerable) : base(enumerable) { /* Left empty.*/ }
            }
        }
    }
#endif

#if !WinCopies3
    namespace Generic
    {
#endif
    public interface IReadOnlyList : ICountableEnumerable
#if WinCopies3
    , IIndexableR
#endif
    {
#if !(WinCopies3 && CS7)
        object this[int index] { get; }
#endif
    }
#if !WinCopies3
    }
#endif

    namespace Generic
    {
#if WinCopies3
        public class UIntCountableEnumerator<T> : Enumerator<T, ICountableEnumeratorInfo<T>, T>, IUIntCountableEnumeratorInfo<T>
        {
            public uint Count => (uint)InnerEnumerator.Count;

            protected override T CurrentOverride => InnerEnumerator.Current;

            public override bool? IsResetSupported => InnerEnumerator.IsResetSupported;

            public UIntCountableEnumerator(ICountableEnumeratorInfo<T> enumerator) : base(enumerator) { /* Left empty. */ }

            protected override bool MoveNextOverride() => InnerEnumerator.MoveNext();

            protected override void ResetOverride2() { /* Left empty. */ }
        }

        public interface IReadOnlyCollectionBase<T> : System.Collections.Generic.IEnumerable<T>
        {
            bool Contains(T item);

            void CopyTo(T[] array, int arrayIndex);
        }

        public interface IReadOnlyCollection<T> : IReadOnlyCollectionBase<T>, DotNetFix.Generic.ICountableEnumerable<T>
#if CS5
            , System.Collections.Generic.IReadOnlyCollection<T>
#endif
        {
            // Left empty.
        }

        public interface IReadOnlyUIntCollection<T> : IReadOnlyCollectionBase<T>, DotNetFix.Generic.IUIntCountableEnumerable<T>
        {
            // Left empty.
        }

        public interface ICollectionBase<T> : IReadOnlyCollectionBase<T>
        {
            bool IsReadOnly { get; }

            void Add(T item);

            void Clear();

            bool Remove(T item);
        }

        public interface ICollection<T> : ICollectionBase<T>, IReadOnlyCollection<T>,
#if CS5
            System.Collections.Generic.IReadOnlyCollection<T>,
#endif
            DotNetFix.Generic.ICountableEnumerable<T>, System.Collections.Generic.ICollection<T>
        {
            // Left empty.
        }

        public interface IUIntCollection<T> : ICollectionBase<T>, IReadOnlyUIntCollection<T>, DotNetFix.Generic.IUIntCountableEnumerable<T>
        {
            // Left empty.
        }
#endif

#if WinCopies3
    }

    namespace Extensions.Generic
    {
#endif
        public interface IReadOnlyList<
#if CS5
            out
#endif
            T> : DotNetFix.
#if WinCopies3
            Generic.
#endif
            ICountableEnumerable<T>
#if WinCopies3
        , IReadOnlyList, IIndexableR<T>
#endif
#if CS7
, System.Collections.Generic.IReadOnlyList<T>
#endif
        {
#if WinCopies3
            new int Count { get; }

            new ICountableEnumerator<T> GetEnumerator();

#if !CS7
            T this[int index] { get; }
#elif CS8
            ICountableEnumerator<T> DotNetFix.Generic.IEnumerable<T, ICountableEnumerator<T>>.GetEnumerator() => GetEnumerator();

            ICountableEnumerator Enumeration.DotNetFix.IEnumerable<ICountableEnumerator>.GetEnumerator() => GetEnumerator();
#endif
#endif
        }

#if !(WinCopies3 || CS7)
        public interface IReadOnlyList2<
#if CS5
            out
#endif
            T> : IReadOnlyList<T>
        {
            T this[int index] { get; }
        }
#endif

        public interface IReadOnlyList3<T>
#if WinCopies3 || CS5
            :
#endif
#if !(WinCopies3 || CS7)
#if WinCopies3
            IReadOnlyList<T>
#elif CS5
            IReadOnlyLinkedList2<T>
#endif
#else
#if WinCopies3
            IReadOnlyList<T>
#else
            IReadOnlyLinkedList2<T>
#endif
#endif
#if CS5
            , System.Collections.Generic.IReadOnlyCollection<T>
#endif
        {
            // Left empty.
        }
#if WinCopies3
    }

    namespace Generic
    {
#endif
        public class CountableEnumerableArray<T> :
#if WinCopies3
            Extensions.Generic.
#endif
            IReadOnlyList<T>
        {
            protected T[] Array { get; }

            public int Count => Array.Length;

            public T this[int index] => Array[index];

#if WinCopies3
#if !CS8
            object IIndexableR.this[int index] => this[index];
#if !CS7
            object IReadOnlyList.this[int index] => this[index];
#endif
#endif
#endif

            public CountableEnumerableArray(in T[] array) => Array = array;

            public
#if WinCopies3
            ICountableEnumeratorInfo<T>
#else
            System.Collections.Generic.IEnumerator<T>
#endif
            GetEnumerator() => new ArrayEnumerator<T>(
#if WinCopies3 && !CS7
                this
#else
                Array
#endif
                );

            IEnumerator SystemIEnumerable.GetEnumerator() => GetEnumerator();

#if WinCopies3
            ICountableEnumerator<T> ICountableEnumerable<T, ICountableEnumerator<T>>.GetEnumerator() => GetEnumerator();

            System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();

            ICountableEnumerator<T>
#if WinCopies3
            Extensions.Generic.
#endif
            IReadOnlyList<T>.GetEnumerator() => GetEnumerator();

#if !CS8
            ICountableEnumerator<T> Enumeration.DotNetFix.IEnumerable<ICountableEnumerator<T>>.GetEnumerator() => GetEnumerator();

            ICountableEnumerator<T> DotNetFix.Generic.IEnumerable<T, ICountableEnumerator<T>>.GetEnumerator() => GetEnumerator();

            ICountableEnumerator Enumeration.DotNetFix.IEnumerable<ICountableEnumerator>.GetEnumerator() => GetEnumerator();
#endif
#endif
        }

        public class UIntCountableEnumerableArray<T> : DotNetFix.
#if WinCopies3
            Generic.
#endif
            IUIntCountableEnumerable<T>
        {
            private readonly CountableEnumerableArray<T> _array;

            public UIntCountableEnumerableArray(in T[] array) : this(new CountableEnumerableArray<T>(array)) { /* Left empty. */ }

            public UIntCountableEnumerableArray(in CountableEnumerableArray<T> array) => _array = array;

            public uint Count => (uint)_array.Count;

            public
#if WinCopies3
            IUIntCountableEnumeratorInfo<T>
#else
        System.Collections.Generic.IEnumerator<T>
#endif
            GetEnumerator() =>
#if WinCopies3
            new UIntCountableEnumerator<T>(
#endif
                _array.GetEnumerator()
#if WinCopies3
                )
#endif
            ;

            IEnumerator SystemIEnumerable.GetEnumerator() => GetEnumerator();

#if WinCopies3
            IUIntCountableEnumerator<T> IUIntCountableEnumerable<T, IUIntCountableEnumerator<T>>.GetEnumerator() => GetEnumerator();

            System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();

#if !CS8
            IUIntCountableEnumerator<T> Enumeration.DotNetFix.IEnumerable<IUIntCountableEnumerator<T>>.GetEnumerator() => GetEnumerator();

            IUIntCountableEnumerator<T> DotNetFix.Generic.IEnumerable<T, IUIntCountableEnumerator<T>>.GetEnumerator() => GetEnumerator();
#endif
#endif
        }
    }
}