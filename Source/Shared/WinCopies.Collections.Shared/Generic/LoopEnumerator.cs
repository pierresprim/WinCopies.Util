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

#if CS6
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using WinCopies.Collections.Generic;
using WinCopies.Linq;

namespace WinCopies.Collections
{
    public interface ILoopEnumerator
    {
        object
#if CS8
            ?
#endif
            Current
        { get; }

        void MovePrevious();

        void MoveNext();
    }

    public interface IReadOnlyListLoopEnumerator : ILoopEnumerator
    {
        int CurrentIndex { get; }
    }

    public class ReflectionLoopEnumerator : StringLoopEnumerator<PropertyInfo>
    {
        protected override string CurrentOverride => Current.Name;

        public ReflectionLoopEnumerator(in Type type) : base(type.GetProperties()) { /* Left empty. */ }

        public ReflectionLoopEnumerator(in Type type, in BindingFlags bindingFlags) : base(type.GetProperties(bindingFlags)) { /* Left empty. */ }

        public ReflectionLoopEnumerator(in Type type, in Predicate<PropertyInfo> predicate) : base(GetArray(type.GetProperties(), predicate)) { /* Left empty. */ }

        public ReflectionLoopEnumerator(in Type type, in BindingFlags bindingFlags, in Predicate<PropertyInfo> predicate) : base(GetArray(type.GetProperties(bindingFlags), predicate)) { /* Left empty. */ }

        public static ReflectionLoopEnumerator Create<T>() => new
#if !CS9
            ReflectionLoopEnumerator
#endif
            (typeof(T));

        public static ReflectionLoopEnumerator Create<T>(in BindingFlags bindingFlags) => new
#if !CS9
            ReflectionLoopEnumerator
#endif
            (typeof(T), bindingFlags);

        public static ReflectionLoopEnumerator Create<T>(in Predicate<PropertyInfo> predicate) => new
#if !CS9
            ReflectionLoopEnumerator
#endif
            (typeof(T), predicate);

        public static ReflectionLoopEnumerator Create<T>(in BindingFlags bindingFlags, in Predicate<PropertyInfo> predicate) => new
#if !CS9
            ReflectionLoopEnumerator
#endif
            (typeof(T), bindingFlags, predicate);

        private static System.Collections.Generic.IReadOnlyList<PropertyInfo> GetArray(in IEnumerable<PropertyInfo> properties, in Predicate<PropertyInfo> predicate)
        {
            var builder = new ArrayBuilder<PropertyInfo>();

            foreach (PropertyInfo p in properties.WherePredicate(predicate))

                _ = builder.AddLast(p);

            return builder.ToArray();
        }
    }

    namespace Generic
    {
        public interface ILoopEnumerator<T> : ILoopEnumerator
        {
            new T Current { get; }
        }
    }

    namespace Abstraction.Generic
    {
        public abstract class LoopEnumerator<TEnumerator, TIn, TOut> : ILoopEnumerator<TOut> where TEnumerator : ILoopEnumerator<TIn>
        {
            protected TEnumerator Enumerator { get; }

            public TOut Current => Convert(Enumerator.Current);

            object
#if CS8
                ?
#endif
                ILoopEnumerator.Current => Current;

            protected LoopEnumerator(in TEnumerator enumerator) => Enumerator = enumerator;

            public void MovePrevious() => Enumerator.MovePrevious();
            public void MoveNext() => Enumerator.MoveNext();

            protected abstract TOut Convert(in TIn value);
        }

        public class LoopEnumeratorDelegate<TEnumerator, TIn, TOut> : LoopEnumerator<TEnumerator, TIn, TOut> where TEnumerator : ILoopEnumerator<TIn>
        {
            protected Converter<TIn, TOut> Selector { get; }

            public LoopEnumeratorDelegate(in TEnumerator enumerator, in Converter<TIn, TOut> selector) : base(enumerator) => Selector = selector;

            protected override TOut Convert(in TIn value) => Selector(value);
        }
    }

    namespace AbstractionInterop.Generic
    {
        public abstract class LoopEnumerator<TEnumerator, TIn, TOut> : ILoopEnumerator<TOut> where TEnumerator : ILoopEnumerator<TIn> where TIn : TOut
        {
            protected TEnumerator Enumerator { get; }

            public TOut Current => Enumerator.Current;

            object
#if CS8
                ?
#endif
                ILoopEnumerator.Current => Current;

            public LoopEnumerator(in TEnumerator enumerator) => Enumerator = enumerator;

            public void MovePrevious() => Enumerator.MovePrevious();
            public void MoveNext() => Enumerator.MoveNext();
        }
    }

    public abstract class CircularArrayBase<T> : IEnumerable, IIndexableR, ICountable where T : IEnumerable
    {
        protected T List { get; }

        public object
#if CS8
            ?
#endif
            this[int index] => GetAt(GetIndex(index));

        public abstract int Count { get; }

        public int Offset { get; }

        public CircularArrayBase(in T list, in int offset)
        {
            List = list
#if CS8
                ??
#else
                == null ?
#endif
                throw new ArgumentNullException(nameof(list))
#if !CS8
                : list
#endif
                ;
            Offset = offset % Count;
        }

        protected int GetIndex(int index)
        {
            int count = Count;
            int offset = Offset;

            return
#if WinCopies3
        UtilHelpers
#else
                WinCopies.Util.Util
#endif
            .GetIndex(index % count, count, ref offset);
        }

        protected abstract object
#if CS8
            ?
#endif
            GetAt(int index);

        public abstract IEnumerator GetEnumerator();
    }

    public class CircularArray : CircularArrayBase<Array>, IIndexable
    {
        public new object
#if CS8
            ?
#endif
            this[int index]
        { get => base[index]; set => List.SetValue(value, GetIndex(index)); }

        public override int Count => List.Length;

        public CircularArray(in Array array, in int offset) : base(array, offset) { /* Left empty. */ }

        protected override object
#if CS8
            ?
#endif
            GetAt(int index) => List.GetValue(index);

        public override IEnumerator GetEnumerator() => DotNetFix.ArrayEnumerator.Create(List, DotNetFix.ArrayEnumerationOptions.Circular, GetIndex(0));
    }

    namespace Generic
    {
        public abstract class CircularArrayBase<TItems, TList> : CircularArrayBase<TList>, System.Collections.Generic.IReadOnlyList<TItems>,
#if CS8
            DotNetFix
#else
            System.Collections
#endif
            .Generic.IEnumerable<TItems>, IIndexableR<TItems> where TList : IEnumerable<TItems>
        {
            public new TItems this[int index] => GetItemAt(GetIndex(index));

            protected CircularArrayBase(in TList list, in int offset) : base(list, offset) { /* Left empty. */ }

            protected abstract TItems GetItemAt(int index);
            protected override object
#if CS8
                ?
#endif
                GetAt(int index) => GetItemAt(index);

            public abstract IEnumerator<TItems> GetEnumeratorGeneric();
            public override
#if CS9
                IEnumerator<TItems>
#else
                IEnumerator
#endif
                GetEnumerator() => GetEnumeratorGeneric();
            IEnumerator<TItems> IEnumerable<TItems>.GetEnumerator() => GetEnumeratorGeneric();
        }

        public class CircularArray<TItems, TList> : CircularArrayBase<TItems, TList> where TList : System.Collections.Generic.IReadOnlyList<TItems>
        {
            public override int Count => List.Count;

            public CircularArray(in TList list, in int offset) : base(list, offset) { /* Left empty. */ }

            protected override TItems GetItemAt(int index) => List[index];

            public override IEnumerator<TItems> GetEnumeratorGeneric() => DotNetFix.ArrayEnumerator.Create(List, DotNetFix.ArrayEnumerationOptions.Circular, GetIndex(0));
        }

        public class CircularReadOnlyList<T> : CircularArray<T, System.Collections.Generic.IReadOnlyList<T>>
        {
            public CircularReadOnlyList(in System.Collections.Generic.IReadOnlyList<T> list, in int offset) : base(list, offset) { /* Left empty. */ }
        }

        public class CircularArray<T> : CircularArray<T, T[]>, IIndexable<T>
        {
            public new T this[int index] { get => base[index]; set => List[GetIndex(index)] = value; }
#if !CS8
            object IIndexableW.this[int index] { set => this[index] = (T)value; }
#endif
            public CircularArray(in T[] array, in int offset) : base(array, offset) { /* Left empty. */ }
        }

        public class CircularList<T> : CircularArrayBase<T, IList<T>>, IIndexable<T>
        {
            public override int Count => List.Count;

            public new T this[int index] { get => base[index]; set => List[GetIndex(index)] = value; }
#if !CS8
            object IIndexableW.this[int index] { set => this[index] = (T)value; }
#endif
            public CircularList(in IList<T> list, in int offset) : base(list, offset) { /* Left empty. */ }

            protected override T GetItemAt(int index) => List[index];

            public override IEnumerator<T> GetEnumeratorGeneric() => DotNetFix.ArrayEnumerator.Create(List, DotNetFix.ArrayEnumerationOptions.Circular, GetIndex(0));
        }

        public class ListLoopEnumerator<T> : IReadOnlyListLoopEnumerator, ILoopEnumerator<T>
        {
            protected System.Collections.Generic.IReadOnlyList<T> InnerArray { get; }

            public T Current => InnerArray[CurrentIndex];

            object
#if CS8
                ?
#endif
                ILoopEnumerator.Current => Current;

            public int CurrentIndex { get; protected set; }

            public ListLoopEnumerator(in System.Collections.Generic.IReadOnlyList<T> array, in int startIndex = 0)
            {
                InnerArray = array;
                CurrentIndex = startIndex;
            }

            public void MovePrevious() => CurrentIndex = (CurrentIndex == 0 ? InnerArray.Count : CurrentIndex) - 1;

            public void MoveNext() => CurrentIndex = CurrentIndex == InnerArray.Count - 1 ? 0 : CurrentIndex + 1;
        }

        public abstract class StringLoopEnumerator<T> : ListLoopEnumerator<T>, ILoopEnumerator<string>, IReadOnlyListLoopEnumerator
        {
            protected class StringEnumerator : ILoopEnumerator<string>
            {
                protected ILoopEnumerator<string> Enumerator { get; }

                public string Current => Enumerator.Current;

                object
#if CS8
                    ?
#endif
                    ILoopEnumerator.Current => Current;

                public StringEnumerator(in StringLoopEnumerator<T> enumerator) => Enumerator = enumerator;

                public void MovePrevious() => Enumerator.MovePrevious();
                public void MoveNext() => Enumerator.MoveNext();
            }

            protected abstract string CurrentOverride { get; }

            string ILoopEnumerator<string>.Current => CurrentOverride;

            public StringLoopEnumerator(in System.Collections.Generic.IReadOnlyList<T> array, in int startIndex = 0) : base(array, startIndex) { /* Left empty. */ }

            public ILoopEnumerator<string> ToStringEnumerator() => new StringEnumerator(this);
        }

        public class EnumLoopEnumerator<T> : StringLoopEnumerator<T> where T : Enum
        {
            protected override string CurrentOverride => Current.ToString();

            public static System.Collections.Generic.IReadOnlyList<T> GetList()
            {
                Array values = typeof(T).GetEnumValues();

                var list = new List<T>(values.Length);

                foreach (object value in values)

                    list.Add((T)value);

                list.Sort();

                return list.AsReadOnly();
            }

            public EnumLoopEnumerator() : base(GetList()) { /* Left empty. */ }
        }
    }
}
#endif
