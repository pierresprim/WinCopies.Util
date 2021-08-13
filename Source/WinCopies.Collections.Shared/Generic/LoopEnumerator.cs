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
using System.Collections.Generic;

namespace WinCopies.Collections
{
#if !WinCopies3
    namespace Generic
    {
#endif
        public interface ILoopEnumerator
        {
            object Current { get; }

            void MovePrevious();

            void MoveNext();
        }

        public interface IReadOnlyListLoopEnumerator : ILoopEnumerator
        {
            int CurrentIndex { get; }
        }

#if WinCopies3
    namespace Generic
    {
#endif
        public interface ILoopEnumerator<T> : ILoopEnumerator
        {
            new T Current { get; }
        }

        public class ListLoopEnumerator<T> : IReadOnlyListLoopEnumerator, ILoopEnumerator<T>
        {
            protected
#if CS6
            System.Collections.Generic.
#else
            
#endif
            IReadOnlyList<T> InnerArray
            { get; }

            public T Current => InnerArray[CurrentIndex];

            object ILoopEnumerator.Current => Current;

            public int CurrentIndex { get; protected set; }

            public ListLoopEnumerator(in System.Collections.Generic.IReadOnlyList<T> array) => InnerArray = array;

            public void MovePrevious() => CurrentIndex = (CurrentIndex == 0 ? InnerArray.Count : CurrentIndex) - 1;

            public void MoveNext() => CurrentIndex = CurrentIndex == InnerArray.Count - 1 ? 0 : CurrentIndex + 1;
        }

        public class EnumLoopEnumerator<T> : ListLoopEnumerator<T>, ILoopEnumerator<string>, IReadOnlyListLoopEnumerator where T : Enum
        {
            string ILoopEnumerator<string>.Current => Current.ToString();

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
