﻿/* Copyright © Pierre Sprimont, 2021
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
                System.Array values = typeof(T).GetEnumValues();

                var list = new List<T>(values.Length);

                foreach (object value in values)

                    list.Add((T)value);

                list.Sort();

                return list.AsReadOnly();
            }

            public EnumLoopEnumerator() : base(GetList()) { /* Left empty. */ }
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
}
#endif
