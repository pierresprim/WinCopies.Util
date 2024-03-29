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
using System.Linq;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Util;

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections
{
    public interface IEnumeratorInfo2 : IEnumeratorInfo, DotNetFix.IDisposableEnumeratorInfo, IDisposableEnumerator
    {
        // Left empty.
    }

    public interface ICountableEnumeratorInfo : IEnumeratorInfo2, ICountableEnumerator
    {
        // Left empty.
    }

    public interface ICountableDisposableEnumeratorInfo : ICountableDisposableEnumerator, ICountableEnumeratorInfo
    {
        // Left empty.
    }

    public interface IUIntCountableEnumeratorInfo : IEnumeratorInfo2, IUIntCountableEnumerator
    {
        // Left empty.
    }

    public interface IUIntCountableDisposableEnumeratorInfo : IUIntCountableDisposableEnumerator, IUIntCountableEnumeratorInfo
    {
        // Left empty.
    }

    namespace Generic
    {
        public class ActionObservableEnumerator<TItems, TEnumerator> : System.Collections.Generic.IEnumerator<TItems> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
        {
            private Func<bool> _moveNext;

            protected TEnumerator InnerEnumerator { get; }
            protected Action<TItems> Entering { get; }
            protected Action<TItems> Exiting { get; }

            public TItems Current => InnerEnumerator.Current;

            object
#if CS8
            ?
#endif
                System.Collections.IEnumerator.Current => Current;

            public ActionObservableEnumerator(in TEnumerator enumerator, in Action<TItems> entering, in Action<TItems> exiting)
            {
                InnerEnumerator = enumerator;
                Entering = entering;
                Exiting = exiting;

                ResetMoveNext();
            }

            private void ResetMoveNext() => _moveNext = () =>
            {
                bool moveNext() => InnerEnumerator.MoveNext();

                if (moveNext())
                {
                    Entering(Current);

                    _moveNext = () =>
                    {
                        if (moveNext())

                            return true;

                        Exiting(Current);

                        return false;
                    };

                    return true;
                }

                return false;
            };

            public bool MoveNext() => _moveNext();
            public void Reset() => ResetMoveNext();

            protected virtual void Dispose(bool disposing) => InnerEnumerator.Dispose();

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            ~ActionObservableEnumerator() => Dispose(false);
        }

        public class ActionObservableEnumerator<T> : ActionObservableEnumerator<T, System.Collections.Generic.IEnumerator<T>>
        {
            public ActionObservableEnumerator(in System.Collections.Generic.IEnumerator<T> enumerator, in Action<T> entering, in Action<T> exiting) : base(enumerator, entering, exiting) { /* Left empty. */ }

            public ActionObservableEnumerator(in System.Collections.Generic.IEnumerable<T> enumerable, in Action<T> entering, in Action<T> exiting) : this(enumerable.GetEnumerator(), entering, exiting) { /* Left empty. */ }
        }

        public abstract class DisposableEnumerableBase<T> : DotNetFix.Generic.IDisposableEnumerable<T>
        {
            public abstract bool IsDisposed { get; }

            protected abstract System.Collections.Generic.IEnumerator<T> GetEnumeratorOverride();

            public System.Collections.Generic.IEnumerator<T> GetEnumerator() => GetOrThrowIfDisposed(this, GetEnumeratorOverride());

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

            protected abstract void Dispose(bool disposing);

            ~DisposableEnumerableBase() => Dispose(false);

            public void Dispose()
            {
                if (IsDisposed)

                    return;

                Dispose(true);

                GC.SuppressFinalize(this);
            }
        }

        public abstract class DisposableEnumerable<T> : DisposableEnumerableBase<T>
        {
            private bool _isDisposed = false;

            public sealed override bool IsDisposed => _isDisposed;

            protected override void Dispose(bool disposing) => _isDisposed = true;
        }

        public class DisposableEnumerable2<T> : DisposableEnumerableBase<T>
        {
            private Action _onDispose;
            protected System.Collections.Generic.IEnumerable<T> InnerEnumerable { get; private set; }

            public override bool IsDisposed => InnerEnumerable == null;

            public DisposableEnumerable2(in System.Collections.Generic.IEnumerable<T> enumerable, in Action
#if CS8
                ?
#endif
                onDispose)
            {
                InnerEnumerable = enumerable ?? throw GetArgumentNullException(nameof(enumerable));
                _onDispose = onDispose ?? Delegates.EmptyVoid;
            }

            protected override System.Collections.Generic.IEnumerator<T> GetEnumeratorOverride() => InnerEnumerable.GetEnumerator();

            protected override void Dispose(bool disposing)
            {
                _onDispose();
                InnerEnumerable = null;
                _onDispose = null;
            }
        }

        public class DisposableEnumerable<TIn, TOut> : DisposableEnumerableBase<TOut> where TIn : TOut
        {
            protected IDisposableEnumerable<TIn> InnerEnumerable { get; private set; }

            public override bool IsDisposed => InnerEnumerable == null;

            public DisposableEnumerable(in IDisposableEnumerable<TIn> enumerable) => InnerEnumerable = enumerable ?? throw GetArgumentNullException(nameof(enumerable));

            protected override System.Collections.Generic.IEnumerator<TOut> GetEnumeratorOverride() => InnerEnumerable.OfType<TOut>().GetEnumerator();

            protected override void Dispose(bool disposing)
            {
                InnerEnumerable.Dispose();
                InnerEnumerable = null;
            }
        }

        public interface IRecursiveEnumerableProvider<
#if CS5
            out
#endif
             T> :
#if CS8
            DotNetFix
#else
            System.Collections
#endif
            .Generic.IEnumerable<T>, IAsEnumerable<IRecursiveEnumerable<T>>, IAsEnumerableAlt<IRecursiveEnumerable<T>>, IAsEnumerableAlt<T>
        {
            // Left empty.
        }

        public interface IRecursiveEnumerable<
#if CS5
            out
#endif
             T> : IRecursiveEnumerableProvider<T>
        {
            T Value { get; }
        }

        public readonly struct RecursiveEnumerationConverters<T>
        {
            public readonly Converter<T, System.Collections.Generic.IEnumerable<T>> ContainersConverter;
            public readonly Converter<T, System.Collections.Generic.IEnumerable<T>> ItemsConverter;

            public RecursiveEnumerationConverters(in Converter<T, System.Collections.Generic.IEnumerable<T>> containersConverter, in Converter<T, System.Collections.Generic.IEnumerable<T>> itemsConverter)
            {
                ContainersConverter = containersConverter;
                ItemsConverter = itemsConverter;
            }
        }

        public readonly struct RecursiveEnumerableConverters<T>
        {
            public readonly Converter<T, System.Collections.Generic.IEnumerable<T>> Converter;
            public readonly RecursiveEnumerationConverters<T> RecursiveEnumerationConverters;

            public RecursiveEnumerableConverters(in Converter<T, System.Collections.Generic.IEnumerable<T>> converter, in RecursiveEnumerationConverters<T> recursiveEnumerationConverters)
            {
                Converter = converter;
                RecursiveEnumerationConverters = recursiveEnumerationConverters;
            }
        }

        public readonly struct RecursiveEnumerable<T> : IRecursiveEnumerable<T>
        {
            private readonly RecursiveEnumerableConverters<T> _converters;

            public T Value { get; }

            public RecursiveEnumerable(in T value, in RecursiveEnumerableConverters<T> converters)
            {
                _converters = converters;

                Value = value;
            }

            private System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> GetEnumerable(in Converter<T, System.Collections.Generic.IEnumerable<T>> converter)
            {
                RecursiveEnumerableConverters<T> converters = _converters;

                return converter(Value).Select(item => new RecursiveEnumerable<T>(item, converters).AsFromType<IRecursiveEnumerable<T>>());
            }

            public System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> AsEnumerable() => GetEnumerable(_converters.Converter);
            System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> IAsEnumerableAlt<IRecursiveEnumerable<T>>.AsEnumerableAlt() => GetEnumerable(_converters.RecursiveEnumerationConverters.ContainersConverter);
            System.Collections.Generic.IEnumerable<T> IAsEnumerableAlt<T>.AsEnumerableAlt() => _converters.RecursiveEnumerationConverters.ItemsConverter(Value);

            public System.Collections.Generic.IEnumerator<T> GetEnumerator() => _converters.Converter(Value).GetEnumerator();
#if !CS8
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
#endif
        }

        public interface IEnumeratorInfo<
#if CS5
            out
#endif
            T> :
#if CS8
            DotNetFix.Generic
#else
            System.Collections.Generic
#endif
            .IEnumerator<T>, IEnumeratorInfo
        {
            // Left empty.
        }

        public interface ICountableEnumeratorInfo<
#if CS5
            out
#endif
             T> : IEnumeratorInfo<T>, ICountableEnumerator<T>
        {
            // Left empty.
        }

        public interface ICountableDisposableEnumeratorInfo<
#if CS5
            out
#endif
             T> : ICountableDisposableEnumerator<T>, ICountableEnumeratorInfo<T>, IEnumeratorInfo<T>
        {
            // Left empty.
        }

        public interface IUIntCountableEnumeratorInfo<
#if CS5
            out
#endif
             T> : IEnumeratorInfo<T>, IUIntCountableEnumerator<T>
        {
            // Left empty.
        }

        public interface IUIntCountableDisposableEnumeratorInfo<
#if CS5
            out
#endif
             T> : IUIntCountableDisposableEnumerator<T>, IUIntCountableEnumeratorInfo<T>, IEnumeratorInfo<T>
        {
            // Left empty.
        }
    }
}
