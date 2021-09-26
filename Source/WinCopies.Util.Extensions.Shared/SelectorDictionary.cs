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

using WinCopies.Collections;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Linq;

using static WinCopies.
#if !WinCopies3
    Util.
#endif
    Delegates;
using static
#if WinCopies3
    WinCopies.ThrowHelper;
#else
    WinCopies.Util.Util;
#endif

namespace WinCopies
{
    public interface IActionDictionaryBase<TPredicate, TAction> : WinCopies.
#if !WinCopies3
        Util.
#endif
        DotNetFix.IDisposable
    {
        TAction
#if WinCopies3
            DefaultAction
#else
            DefaultSelector
#endif
        { get; }

        void Push(Predicate<TPredicate> predicate, TAction action);
    }

    public abstract class ActionDictionaryBase<TPredicate, TAction>
#if WinCopies3
        : IActionDictionaryBase<TPredicate, TAction>
#endif
    {
        private
#if WinCopies3
            IEnumerableStack
#else
            EnumerableStack
#endif
            <KeyValuePair<Predicate<TPredicate>, TAction>> _stack;

        protected
#if WinCopies3
            IEnumerableStack
#else
            EnumerableStack
#endif
            <KeyValuePair<Predicate<TPredicate>, TAction>> Stack => IsDisposed ? throw GetExceptionForDispose(false) : _stack;

        protected abstract TAction
#if WinCopies3
            DefaultActionOverride
#else
DefaultSelectorOverride
#endif
        { get; }

        public TAction
#if WinCopies3
            DefaultAction
#else
            DefaultSelector
#endif
            => IsDisposed ? throw GetExceptionForDispose(false) :
#if WinCopies3
            DefaultActionOverride;

#else
        DefaultSelectorOverride;
#endif

        public bool IsDisposed { get; private set; }

        public ActionDictionaryBase(
#if WinCopies3
            IEnumerableStack
#else
            EnumerableStack
#endif
            <KeyValuePair<Predicate<TPredicate>, TAction>> stack) => _stack = stack ?? throw GetArgumentNullException(nameof(stack));

        public ActionDictionaryBase() : this(new EnumerableStack<KeyValuePair<Predicate<TPredicate>, TAction>>()) { }

        public void Push(Predicate<TPredicate> predicate, TAction selector) => Stack.Push(new KeyValuePair<Predicate<TPredicate>, TAction>(predicate ?? throw GetArgumentNullException(nameof(predicate)), selector
#if CS8
            ??
#else
            == null ?
#endif
            throw GetArgumentNullException(nameof(selector))
#if !CS8
            : selector
#endif
            ));

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

        ~ActionDictionaryBase() => DisposeUnmanaged();
    }

    public interface IActionDictionary<T> : IActionDictionaryBase<T, Action<T>>
    {
        void Run(T item);
    }

    public interface IEnumerableActionDictionary<T> : IActionDictionary<T>
    {
        void Run(System.Collections.Generic.IEnumerable<T> items);
    }

    public abstract class ActionDictionary<T> : ActionDictionaryBase<T, Action<T>>, IActionDictionary<T>
    {
        protected void _Run(T item)
        {
            foreach (KeyValuePair<Predicate<T>, Action<T>> _item in Stack)

                if (_item.Key(item))
                {
                    _item.Value(item);

                    return;
                }

#if WinCopies3
            DefaultActionOverride
#else
            DefaultSelectorOverride
#endif
                (item);
        }

        public void Run(T item)
        {
            ThrowIfDisposed(this);

            _Run(item
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
    }

    public abstract class EnumerableActionDictionary<T> : ActionDictionary<T>, IEnumerableActionDictionary<T>
    {
        public void Run(System.Collections.Generic.IEnumerable<T> items)
        {
            ThrowIfDisposed(this);

            foreach (T item in items)

                (((IUIntCountable)Stack).Count == 0 ?
#if WinCopies3
            DefaultActionOverride
#else
            DefaultSelectorOverride
#endif
            : _Run)(item);
        }
    }

    public class DefaultNullableValueActionDictionary<T> : ActionDictionary<T>
    {
        protected override Action<T>
#if WinCopies3
            DefaultActionOverride
#else
            DefaultSelectorOverride
#endif
            => EmptyVoid;
    }

    public class DefaultNullableValueEnumerableActionDictionary<T> : EnumerableActionDictionary<T>
    {
        protected override Action<T>
#if WinCopies3
            DefaultActionOverride
#else
            DefaultSelectorOverride
#endif
            => EmptyVoid;
    }

    public interface ISelectorDictionary<TIn, TOut> :
#if WinCopies3
        IActionDictionaryBase<TIn, Converter<TIn, TOut>>
    {
#else
        Util.DotNetFix.IDisposable
    {
        void Push(Predicate<TIn> predicate, Converter<TIn, TOut> selector);

        Converter<TIn, TOut> DefaultSelector { get; }
#endif
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

    public abstract class SelectorDictionary<TIn, TOut> : ActionDictionaryBase<TIn, Converter<TIn, TOut>>, ISelectorDictionary<TIn, TOut>
    {
        protected TOut _Select(TIn item)
        {
            foreach (KeyValuePair<Predicate<TIn>, Converter<TIn, TOut>> _item in Stack)

                if (_item.Key(item))

                    return _item.Value(item);

            return
#if WinCopies3
                DefaultActionOverride
#else
                DefaultSelectorOverride
#endif
                (item);
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
    }

    public abstract class EnumerableSelectorDictionary<TIn, TOut> : SelectorDictionary<TIn, TOut>, IEnumerableSelectorDictionary<TIn, TOut>
    {
        public System.Collections.Generic.IEnumerable<TOut> Select(System.Collections.Generic.IEnumerable<TIn> items) => IsDisposed ? throw GetExceptionForDispose(false) : items.
#if WinCopies3
            SelectConverter
#else
            Select
#endif
            (((IUIntCountable)Stack).Count == 0 ?
#if WinCopies3
            DefaultActionOverride
#else
            DefaultSelectorOverride
#endif
            : _Select);
    }

    public class DefaultNullableValueSelectorDictionary<TIn, TOut> : SelectorDictionary<TIn, TOut> where TOut : class
    {
        protected override Converter<TIn, TOut>
#if WinCopies3
            DefaultActionOverride
#else
            DefaultSelectorOverride
#endif
            => Null<TIn, TOut>;
    }

    public class DefaultNullableValueEnumerableSelectorDictionary<TIn, TOut> : EnumerableSelectorDictionary<TIn, TOut> where TOut : class
    {
        protected override Converter<TIn, TOut>
#if WinCopies3
            DefaultActionOverride
#else
            DefaultSelectorOverride
#endif
            => Null<TIn, TOut>;
    }
}
