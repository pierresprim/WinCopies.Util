/* Copyright © Pierre Sprimont, 2019
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
using System.Linq;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Linq;
using WinCopies.Util;

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections
{
    namespace Generic
    {
        public interface IMultiTypeEnumerable<T, U> : System.Collections.Generic.IEnumerable<T>, IAsEnumerable<U> where T : U
        {
#if CS8
            System.Collections.Generic.IEnumerable<U> IAsEnumerable<U>.AsEnumerable() => this.ToEnumerable<T, U>();
#else
            // Left empty.
#endif
        }
    }

    /// <summary>
    /// Collection-oriented helper methods.
    /// </summary>
    public static class Util
    {
#if CS5
        private readonly struct RecursiveEnumerable<T> : IRecursiveEnumerable<T>
        {
            private readonly Converter<T, System.Collections.Generic.IEnumerable<T>> _converter;

            public T Value { get; }

            public RecursiveEnumerable(in T value, in Converter<T, System.Collections.Generic.IEnumerable<T>> converter)
            {
                _converter = converter;

                Value = value;
            }

            public System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> AsEnumerable()
            {
                Converter<T, System.Collections.Generic.IEnumerable<T>> converter = _converter;

                return converter(Value).Select(item => new RecursiveEnumerable<T>(item, converter).AsFromType<IRecursiveEnumerable<T>>());
            }

            System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> IAsEnumerableAlt<IRecursiveEnumerable<T>>.AsEnumerableAlt() => throw new NotSupportedException();
            System.Collections.Generic.IEnumerable<T> IAsEnumerableAlt<T>.AsEnumerableAlt() => throw new NotSupportedException();

            System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => throw new NotSupportedException();
#if !CS8
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.AsFromType<IEnumerable<T>>().GetEnumerator();
#endif
        }
        private readonly struct RecursiveEnumerable2<T> : IRecursiveEnumerable<T>
        {
            private readonly RecursiveEnumerationConverters<T> _converters;

            public T Value { get; }

            public RecursiveEnumerable2(in T value, in RecursiveEnumerationConverters<T> converters)
            {
                _converters = converters;

                Value = value;
            }

            System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> IAsEnumerable<IRecursiveEnumerable<T>>.AsEnumerable() => throw new NotSupportedException();

            System.Collections.Generic.IEnumerable<IRecursiveEnumerable<T>> IAsEnumerableAlt<IRecursiveEnumerable<T>>.AsEnumerableAlt()
            {
                Converter<T, System.Collections.Generic.IEnumerable<T>> converter = _converters.ContainersConverter;

                return converter(Value).Select(item => new RecursiveEnumerable<T>(item, converter).AsFromType<IRecursiveEnumerable<T>>());
            }
            System.Collections.Generic.IEnumerable<T> IAsEnumerableAlt<T>.AsEnumerableAlt() => _converters.ItemsConverter(Value);

            System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => throw new NotSupportedException();
#if !CS8
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.AsFromType<IEnumerable<T>>().GetEnumerator();
#endif
        }

        public static System.Collections.Generic.IEnumerable<T> EnumerateRecursively<T>(in IRecursiveEnumerable<T> item, in RecursiveEnumerationOrder enumerationOrder = RecursiveEnumerationOrder.ParentThenChildren, in RecursiveEnumerationModeEx enumerationMode = RecursiveEnumerationModeEx.ContainersThenItems) => Enumerable.FromEnumerator(RecursiveEnumeration.CreateEnumerator(item, enumerationOrder, enumerationMode));
        public static System.Collections.Generic.IEnumerable<T> EnumerateRecursively<T>(in IRecursiveEnumerable<T> item, in IStackBase<IRecursiveEnumeratorStruct<T>> stack, in RecursiveEnumerationOrder enumerationOrder = RecursiveEnumerationOrder.ParentThenChildren, in RecursiveEnumerationModeEx enumerationMode = RecursiveEnumerationModeEx.ContainersThenItems) => Enumerable.FromEnumerator(RecursiveEnumeration.CreateEnumerator(item, enumerationOrder, enumerationMode, stack));

        private class RecursiveEnumerationStack<T> : StackBaseCollection<IRecursiveEnumeratorStruct<T>>, IStackBase<IRecursiveEnumeratorStruct<T>>
        {
            private readonly Action<T> _onEntering;

            public RecursiveEnumerationStack(in Action<T> onEntering):base() => _onEntering = onEntering;

            public override void Add(IRecursiveEnumeratorStruct<T> item)
            {
                base.Add(item);

                _onEntering(item.Value);
            }
        }
        private class RecursiveEnumerationStack2<T> : RecursiveEnumerationStack<T>
        {
            private readonly Action<T> _onExiting;

            public RecursiveEnumerationStack2(in Action<T> onEntering, in Action<T> onExiting) : base(onEntering) => _onExiting = onExiting;

            public override IRecursiveEnumeratorStruct<T> Remove()
            {
                IRecursiveEnumeratorStruct<T> result = base.Remove();

                _onExiting(result.Value);

                return result;
            }

            public override bool TryRemove(out IRecursiveEnumeratorStruct<T>
#if CS8
                ?
#endif
                result)
            {
                if (base.TryRemove(out result))
                {
                    _onExiting(result.Value);

                    return true;
                }

                return false;
            }
        }

        public static System.Collections.Generic.IEnumerable<T> EnumerateRecursively<T>(in IRecursiveEnumerable<T> item, in Action<T> enteringLevel, in Action<T> exitingLevel, in RecursiveEnumerationOrder enumerationOrder = RecursiveEnumerationOrder.ParentThenChildren, in RecursiveEnumerationModeEx enumerationMode = RecursiveEnumerationModeEx.ContainersThenItems) => EnumerateRecursively(item, new RecursiveEnumerationStack2<T>(enteringLevel, exitingLevel), enumerationOrder, enumerationMode);
        public static System.Collections.Generic.IEnumerable<T> EnumerateRecursively<T>(in IRecursiveEnumerable<T> item, in Action<T> enteringLevel, in Predicate<T> exitingLevel, in RecursiveEnumerationModeEx enumerationMode = RecursiveEnumerationModeEx.ContainersThenItems) => Enumerable.FromEnumerator(RecursiveEnumeration.CreateEnumerator(item, RecursiveEnumerationOrder.Both, enumerationMode, new RecursiveEnumerationStack<T>(enteringLevel), exitingLevel));



        public static System.Collections.Generic.IEnumerable<T> EnumerateRecursively<T>(in T item, in Converter<T, System.Collections.Generic.IEnumerable<T>> converter, in RecursiveEnumerationOrder enumerationOrder = RecursiveEnumerationOrder.ParentThenChildren) => EnumerateRecursively(new RecursiveEnumerable<T>(item, converter), enumerationOrder, RecursiveEnumerationModeEx.None);
        public static System.Collections.Generic.IEnumerable<T> EnumerateRecursively<T>(in T item, in Converter<T, System.Collections.Generic.IEnumerable<T>> converter, in IStackBase<IRecursiveEnumeratorStruct<T>> stack, in RecursiveEnumerationOrder enumerationOrder = RecursiveEnumerationOrder.ParentThenChildren) => EnumerateRecursively(new RecursiveEnumerable<T>(item, converter), stack, enumerationOrder, RecursiveEnumerationModeEx.None);

        public static System.Collections.Generic.IEnumerable<T> EnumerateRecursively<T>(in T item, in Converter<T, System.Collections.Generic.IEnumerable<T>> converter, in Action<T> enteringLevel, in Action<T> exitingLevel, in RecursiveEnumerationOrder enumerationOrder = RecursiveEnumerationOrder.ParentThenChildren) => EnumerateRecursively(item, converter, new RecursiveEnumerationStack2<T>(enteringLevel, exitingLevel), enumerationOrder);
        public static System.Collections.Generic.IEnumerable<T> EnumerateRecursively<T>(in T item, in Converter<T, System.Collections.Generic.IEnumerable<T>> converter, in Action<T> enteringLevel, in Predicate<T> exitingLevel) => EnumerateRecursively(new RecursiveEnumerable<T>(item, converter), enteringLevel, exitingLevel, RecursiveEnumerationModeEx.None);



        public static System.Collections.Generic.IEnumerable<T> EnumerateRecursively<T>(in T item, in RecursiveEnumerationConverters<T> converters, in RecursiveEnumerationOrder enumerationOrder = RecursiveEnumerationOrder.ParentThenChildren, in RecursiveEnumerationMode enumerationMode = RecursiveEnumerationMode.ContainersThenItems)
        {
            RecursiveEnumeration.ThrowIfInvalidMode(nameof(enumerationMode), enumerationMode);

            return EnumerateRecursively(new RecursiveEnumerable2<T>(item, converters), enumerationOrder, (RecursiveEnumerationModeEx)enumerationMode);
        }
        public static System.Collections.Generic.IEnumerable<T> EnumerateRecursively<T>(in T item, in RecursiveEnumerationConverters<T> converters, in IStackBase<IRecursiveEnumeratorStruct<T>> stack, in RecursiveEnumerationOrder enumerationOrder = RecursiveEnumerationOrder.ParentThenChildren, in RecursiveEnumerationMode enumerationMode = RecursiveEnumerationMode.ContainersThenItems)
        {
            RecursiveEnumeration.ThrowIfInvalidMode(nameof(enumerationMode), enumerationMode);

            return EnumerateRecursively(new RecursiveEnumerable2<T>(item, converters), stack, enumerationOrder, (RecursiveEnumerationModeEx)enumerationMode);
        }

        public static System.Collections.Generic.IEnumerable<T> EnumerateRecursively<T>(in T item, in RecursiveEnumerationConverters<T> converters, in Action<T> enteringLevel, in Action<T> exitingLevel, in RecursiveEnumerationOrder enumerationOrder = RecursiveEnumerationOrder.ParentThenChildren, in RecursiveEnumerationMode enumerationMode = RecursiveEnumerationMode.ContainersThenItems) => EnumerateRecursively(item, converters, new RecursiveEnumerationStack2<T>(enteringLevel, exitingLevel), enumerationOrder, enumerationMode);
        public static System.Collections.Generic.IEnumerable<T> EnumerateRecursively<T>(in T item, in RecursiveEnumerationConverters<T> converters, in Action<T> enteringLevel, in Predicate<T> exitingLevel, in RecursiveEnumerationMode enumerationMode = RecursiveEnumerationMode.ContainersThenItems)
        {
            RecursiveEnumeration.ThrowIfInvalidMode(nameof(enumerationMode), enumerationMode);

            return EnumerateRecursively(new RecursiveEnumerable2<T>(item, converters), enteringLevel, exitingLevel, (RecursiveEnumerationModeEx)enumerationMode);
        }
#endif
        public static bool Equals<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in System.Collections.Generic.IEnumerable<T> compareWith, in T delimiter)
        {
            using
#if !CS8
                (
#endif
                System.Collections.Generic.IEnumerator<T> enumerator = compareWith.GetEnumerator()
#if CS8
                ;
#else
                )
#endif

                foreach (T value in Enumerable.FromEnumerator(new PredicateEnumerator<T>(enumerable, Delegates.GetAreNotEqualPredicate(delimiter))))

                    if (!(enumerator.MoveNext() && Equals(enumerator.Current, value)))

                        return false;

            return true;
        }

        public static void PerformAction<TIn, TOut>(in System.Collections.Generic.IEnumerable<TOut> parameters, in Action<System.Collections.Generic.IEnumerable<TIn>> action) => action(parameters.Cast<TIn>());
#if CS5
        public static ILinkedListNode<KeyValuePair<TKey, TValue>> Find<TDictionary, TKey, TValue>(in TDictionary dictionary, in TKey key) where TDictionary : DotNetFix.Generic.IDictionary<TKey, TValue>, ILinkedList3<KeyValuePair<TKey, TValue>>
        {
            var enumerable = new Enumerable<ILinkedListNode<KeyValuePair<TKey, TValue>>>(dictionary.GetNodeEnumerator);

            foreach (ILinkedListNode<KeyValuePair<TKey, TValue>> node in enumerable)

                if (Delegates.CompareHashCodeGenericIn(node.Value.Key, key))

                    return node;

            return null;
        }

        public static bool Remove<TDictionary, TKey, TValue>(in TDictionary dictionary, in TKey key) where TDictionary : DotNetFix.Generic.IDictionary<TKey, TValue>, ILinkedList3<KeyValuePair<TKey, TValue>>
        {
            ILinkedListNode<KeyValuePair<TKey, TValue>> node = Find<TDictionary, TKey, TValue>(dictionary, key);

            if (node == null)

                return false;

            dictionary.Remove(node);

            return true;
        }
#endif
        private static bool _HasItems<T>(in System.Collections.Generic.IEnumerable<T> enumerable)
        {
            bool result;

            using (var enumerator = new EmptyCheckEnumerator(enumerable.GetEnumerator()))

                result = enumerator.HasItems;

            return result;
        }

        public static bool TryGetHasItems<T>(in System.Collections.Generic.IEnumerable<T> enumerable) => enumerable == null ? false : _HasItems(enumerable);

        public static bool HasItems<T>(in System.Collections.Generic.IEnumerable<T> enumerable) => _HasItems(enumerable ?? throw GetArgumentNullException(nameof(enumerable)));
#if CS7
        public static System.Collections.Generic.IEnumerator<Collections.DotNetFix.Generic.ILinkedListNode<T>> GetNodeEnumerator<T>(in ILinkedList<T> list, in EnumerationDirection enumerationDirection, in DotNetFix.Generic.ILinkedListNode<T> start, DotNetFix.Generic.ILinkedListNode<T> end) => new LinkedListEnumerator<T>(list, enumerationDirection, start, end);
#endif
        /// <summary>
        /// Returns a value obtained by a <see cref="Func"/>, depending on the result of a comparison.
        /// </summary>
        /// <param name="x">The value to compare to <paramref name="y"/>.</param>
        /// <param name="y">The value to compare to <paramref name="x"/>.</param>
        /// <param name="comparison">The comparison delegate.</param>
        /// <param name="lower">The <see cref="Func"/> that provides the value for <paramref name="x"/> is lower than <paramref name="y"/>.</param>
        /// <param name="equals">The <see cref="Func"/> that provides the value for <paramref name="x"/> is equal to <paramref name="y"/>.</param>
        /// <param name="greater">The <see cref="Func"/> that provides the value for <paramref name="x"/> is greater than <paramref name="y"/>.</param>
        /// <returns>A value obtained by a <see cref="Func"/>, depending on the result of a comparison.</returns>
        /// <exception cref="ArgumentNullException">One or more of the given <see cref="Func"/>s are <see langword="null"/>.</exception>
        /// <remarks>See <see cref="GetIf{TValues, TResult}(in TValues, in TValues, in Comparison{TValues}, in Func{TResult}, in Func{TResult}, in Func{TResult})"/> for the generic version.</remarks>
        public static object GetIf(in object x, in object y, in WinCopies.Collections.Comparison comparison, in Func lower, in Func equals, in Func greater)
        {
            ThrowIfNull(lower, nameof(lower));
            ThrowIfNull(greater, nameof(greater));
            ThrowIfNull(equals, nameof(equals));

            int result = comparison(x, y);

            return result < 0 ? lower() : result > 0 ? greater() : equals();
        }

        /// <summary>
        /// Returns a value obtained by a <see cref="Func"/>, depending on the result of a comparison.
        /// </summary>
        /// <param name="x">The value to compare to <paramref name="y"/>.</param>
        /// <param name="y">The value to compare to <paramref name="x"/>.</param>
        /// <param name="comparison">The comparison delegate.</param>
        /// <param name="lower">The <see cref="Func"/> that provides the value for <paramref name="x"/> is lower than <paramref name="y"/>.</param>
        /// <param name="equals">The <see cref="Func"/> that provides the value for <paramref name="x"/> is equal to <paramref name="y"/>.</param>
        /// <param name="greater">The <see cref="Func"/> that provides the value for <paramref name="x"/> is greater than <paramref name="y"/>.</param>
        /// <returns>A value obtained by a <see cref="Func"/>, depending on the result of a comparison.</returns>
        /// <exception cref="ArgumentNullException">One or more of the given <see cref="Func"/>s are <see langword="null"/>.</exception>
        /// <remarks>See <see cref="GetIf(in object, in object, in Collections.Comparison, in Func, in Func, in Func)"/> for the non-generic version.</remarks>
        public static TResult GetIf<TValues, TResult>(in TValues x, in TValues y, in Comparison<TValues> comparison, in Func<TResult> lower, in Func<TResult> equals, in Func<TResult> greater)
        {
            ThrowIfNull(lower, nameof(lower));
            ThrowIfNull(greater, nameof(greater));
            ThrowIfNull(equals, nameof(equals));

            int result = comparison(x, y);

            return result < 0 ? lower() : result > 0 ? greater() : equals();
        }

        public static object GetIf(in object x, in object y, in IComparer comparer, in Func lower, in Func equals, in Func greater)
        {
            ThrowIfNull(lower, nameof(lower));
            ThrowIfNull(greater, nameof(greater));
            ThrowIfNull(equals, nameof(equals));

            int result = comparer.Compare(x, y);

            return result < 0 ? lower() : result > 0 ? greater() : equals();
        }

        public static TResult GetIf<TValues, TResult>(in TValues x, in TValues y, in WinCopies.Collections.Comparison comparison, in Func<TResult> lower, in Func<TResult> equals, in Func<TResult> greater)
        {
            ThrowIfNull(lower, nameof(lower));
            ThrowIfNull(greater, nameof(greater));
            ThrowIfNull(equals, nameof(equals));

            int result = comparison(x, y);

            return result < 0 ? lower() : result > 0 ? greater() : equals();
        }
    }
}
