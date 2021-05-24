/* Copyright © Pierre Sprimont, 2021
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
using System.Collections.Generic;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Linq;

using static
#if WinCopies3
    WinCopies.ThrowHelper;
#else
    WinCopies.Util.Util;
#endif

namespace WinCopies
{
    public interface ISelectorDictionary<TIn, TOut> :
#if !WinCopies3
        Util.
#endif
        DotNetFix.IDisposable
    {
        void Push(Predicate<TIn> predicate, Converter<TIn, TOut> selector);

        Converter<TIn, TOut> DefaultSelector { get; }

        TOut Select(TIn item);
    }

    public interface IEnumerableSelectorDictionary<TIn, TOut> : ISelectorDictionary<TIn, TOut>
    {
        System.Collections.Generic.IEnumerable<TOut> Select(System.Collections.Generic.IEnumerable<TIn> items);
    }

    public static class SelectorDictionary
    {
        public static ArgumentException GetInvalidItemException() => new ArgumentException("The given item or its current configuration is not supported.");
    }

    public abstract class SelectorDictionary<TIn, TOut> : ISelectorDictionary<TIn, TOut>
    {
        private EnumerableStack<KeyValuePair<Predicate<TIn>, Converter<TIn, TOut>>> _stack = new EnumerableStack<KeyValuePair<Predicate<TIn>, Converter<TIn, TOut>>>();

        protected EnumerableStack<KeyValuePair<Predicate<TIn>, Converter<TIn, TOut>>> Stack => IsDisposed ? throw GetExceptionForDispose(false) : _stack;

        protected abstract Converter<TIn, TOut> DefaultSelectorOverride { get; }

        public Converter<TIn, TOut> DefaultSelector => IsDisposed ? throw GetExceptionForDispose(false) : DefaultSelectorOverride;

        public bool IsDisposed { get; private set; }

        public void Push(Predicate<TIn> predicate, Converter<TIn, TOut> selector)
        {
            ThrowIfDisposed(this);

            _stack.Push(new KeyValuePair<Predicate<TIn>, Converter<TIn, TOut>>(predicate ?? throw GetArgumentNullException(nameof(predicate)), selector ?? throw GetArgumentNullException(nameof(selector))));
        }

        protected TOut _Select(TIn item)
        {
            foreach (KeyValuePair<Predicate<TIn>, Converter<TIn, TOut>> _item in _stack)

                if (_item.Key(item))

                    return _item.Value(item);

            return DefaultSelectorOverride(item);
        }

        public TOut Select(TIn item)
        {
            ThrowIfDisposed(this);

            return _Select(item
#if CS8
                ??
#else
                == null ?
#endif
                throw GetArgumentNullException(nameof(item))
#if !CS8
                : item
#endif
                );
        }

        /// <summary>
        /// This method is called by <see cref="Dispose"/> and by the deconstructor. This overload does nothing and it is not necessary to call this base overload in derived classes.
        /// </summary>
        protected virtual void DisposeUnmanaged() { /* Left empty. */ }

        protected virtual void DisposeManaged()
        {
            _stack.Clear();

            _stack = null;

            IsDisposed = true;
        }

        public void Dispose()
        {
            if (IsDisposed)

                return;

            DisposeUnmanaged();

            DisposeManaged();

            GC.SuppressFinalize(this);
        }

        ~SelectorDictionary() => DisposeUnmanaged();
    }

    public abstract class EnumerableSelectorDictionary<TIn, TOut> : SelectorDictionary<TIn, TOut>, IEnumerableSelectorDictionary<TIn, TOut>
    {
        public System.Collections.Generic.IEnumerable<TOut> Select(System.Collections.Generic.IEnumerable<TIn> items) => IsDisposed ? throw GetExceptionForDispose(false) : items.
#if WinCopies3
            SelectConverter
#else
            Select
#endif
            (Stack.Count == 0 ? DefaultSelectorOverride : _Select);
    }

    public class DefaultNullableValueSelectorDictionary<TIn, TOut> : SelectorDictionary<TIn, TOut> where TOut : class
    {
        protected override Converter<TIn, TOut> DefaultSelectorOverride =>
            #if !WinCopies3
            Util.
#endif
            Delegates
            
            .Null<TIn, TOut>;
    }
}
