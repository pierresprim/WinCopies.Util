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

#if CS5
using System;
using System.Collections;
using System.Collections.Generic;

using WinCopies.Collections.Generic;

namespace WinCopies.Collections
{
    public abstract class CircularArrayBase : IEnumerable, IIndexableR, ICountable
    {
        private int _offset;

        public abstract int Count { get; }

        public int Offset { get => _offset; set => _offset = value % Count; }

        public object
#if CS8
            ?
#endif
            this[int index]
        {
            get
            {
                SetIndex(ref index);

                return GetAt(index);
            }
        }

        protected void SetIndex(ref int index)
        {
            int count = Count;
            int offset = Offset;

            UtilHelpers.SetIndex(ref index, count, ref offset);
        }

        protected int GetIndex(int index)
        {
            SetIndex(ref index);

            return index;
        }

        protected abstract object
#if CS8
            ?
#endif
            GetAt(int index);

        public void UpdateOffset(ref int offset) => Offset = UtilHelpers.GetIndex(Offset, Count, ref offset);

        public abstract IEnumerator GetEnumerator();
    }

    public abstract class CircularArrayBase<T> : CircularArrayBase where T : IEnumerable
    {
        protected T List { get; }

        protected CircularArrayBase(in T list, in int offset)
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
            Offset = offset;
        }
    }

    public class CircularArray : CircularArrayBase<System.Array>, IIndexable
    {
        public new object
#if CS8
            ?
#endif
            this[int index]
        { get => base[index]; set => List.SetValue(value, GetIndex(index)); }

        public override int Count => List.Length;

        public CircularArray(in System.Array array, in int offset) : base(array, offset) { /* Left empty. */ }

        protected override object
#if CS8
            ?
#endif
            GetAt(int index) => List.GetValue(index);

        public override IEnumerator GetEnumerator() => DotNetFix.ArrayEnumerator.Create(List, DotNetFix.ArrayEnumerationOptions.Circular, GetIndex(0));
    }

    public class CircularCharArray : CircularArray<char>
    {
        public CircularCharArray(in char[] array, in int offset) : base(array, offset) { /* Left empty. */ }

        protected bool GetAvailableLength(ref int index, in int length, out int availableLength)
        {
            SetIndex(ref index);

            return (availableLength = Count - Offset) >= length;
        }

        public string Copy(int startIndex, int length)
        {
            if (startIndex.Between(0, Count, true, false) ? length == 0 : throw new ArgumentOutOfRangeException(nameof(startIndex)))

                return string.Empty;

            if (length.Outside(0, Count - startIndex))

                throw new ArgumentOutOfRangeException(nameof(length));

            string getString(in int _startIndex, in int __length) => new
#if !CS9
                string
#endif
                (List, _startIndex, __length);

            if (Offset == 0 || GetAvailableLength(ref startIndex, length, out int _length))

                return getString(startIndex, length);

            string result = getString(startIndex, _length);
            return result + getString(0, length - _length);
        }

        public void Paste(string text, in int sourceIndex, int destinationIndex, in int length)
        {
            if (length == 0)

                return;

            int count = sourceIndex.Between(0, text.Length, true, false) ? Count : throw new ArgumentOutOfRangeException(nameof(sourceIndex));

            if (destinationIndex.Between(0, count, true, false) ? (length > count || length > count - destinationIndex || length.Outside(0, text.Length - sourceIndex)) : throw new ArgumentOutOfRangeException(nameof(destinationIndex)))

                throw new ArgumentOutOfRangeException(nameof(length));

            void copyTo(in int _sourceIndex, in int _destinationIndex, in int __length) => text.CopyTo(_sourceIndex, List, _destinationIndex, __length);

            if (Offset == 0 || GetAvailableLength(ref destinationIndex, length, out int _length))

                copyTo(sourceIndex, destinationIndex, length);

            else
            {
                copyTo(sourceIndex, destinationIndex, _length);
                copyTo(sourceIndex + _length, destinationIndex + _length, length - _length);
            }
        }
        public void Paste(string text, in int sourceIndex, int destinationIndex) => Paste(text, sourceIndex, destinationIndex, text.Length);
        public void Paste(string text) => Paste(text, 0, 0);
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
            public new TItems this[int index]
            {
                get
                {
                    SetIndex(ref index);

                    return GetItemAt(index);
                }
            }

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
            public new T this[int index]
            {
                get => base[index];

                set
                {
                    SetIndex(ref index);

                    List[index] = value;
                }
            }
#if !CS8
            object IIndexableW.this[int index] { set => this[index] = (T)value; }
#endif
            public CircularArray(in T[] array, in int offset) : base(array, offset) { /* Left empty. */ }
        }

        public class CircularList<T> : CircularArrayBase<T, IList<T>>, IIndexable<T>
        {
            public override int Count => List.Count;

            public new T this[int index]
            {
                get => base[index];

                set
                {
                    SetIndex(ref index);

                    List[index] = value;
                }
            }
#if !CS8
            object IIndexableW.this[int index] { set => this[index] = (T)value; }
#endif
            public CircularList(in IList<T> list, in int offset) : base(list, offset) { /* Left empty. */ }

            protected override T GetItemAt(int index) => List[index];

            public override IEnumerator<T> GetEnumeratorGeneric() => DotNetFix.ArrayEnumerator.Create(List, DotNetFix.ArrayEnumerationOptions.Circular, GetIndex(0));
        }
    }
}
#endif
