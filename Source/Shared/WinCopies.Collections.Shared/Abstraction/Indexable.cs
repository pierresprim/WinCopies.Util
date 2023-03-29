/* Copyright © Pierre Sprimont, 2023
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

#if CS7
using System;

using WinCopies.Collections.Generic;
using WinCopies.Util;

namespace WinCopies.Collections.Abstraction.Generic.Indexables
{
    public class ReadOnlyIndexableULong<TArray, TItems> : IIndexableR<TItems>, IUIntCountable, ICountable where TArray : IULongIndexableR<TItems>, IULongCountable
    {
        protected TArray Array { get; }

        protected ulong Start { get; private set; }
        public uint Count { get; private set; }

        int ICountable.Count => (int)Count;

        public TItems this[int index] => Array[GetIndex(index)];
#if !CS8
        object IIndexableR.this[int index] => this[index];
#endif
        public ReadOnlyIndexableULong(in TArray array, in ulong start, in int length)
        {
            Array = array
#if CS9
                ??
#else
                == null ?
#endif
                throw new ArgumentNullException(nameof(array))
#if !CS9
                : array
#endif
                ;

            UpdateBounds(start, length);
        }
        public ReadOnlyIndexableULong(in TArray array) : this(array, 0ul, 0) { }

        public void UpdateBounds(in ulong start, in int length)
        {
            if (length == 0)
            {
                Count = 0;

                return;
            }

            ulong count = Array.Count;

            Start = start < count ? start : throw GetError(nameof(start));

            ulong actualLength = length < 0 ? throw GetError(nameof(length)) : start + (ulong)length;

            Count = actualLength > count ? throw GetError(nameof(length)) : (uint)length;
        }
        public void ResetBounds() => UpdateBounds(0ul, 0);

        protected static Exception GetError(in string paramName) => new ArgumentOutOfRangeException(paramName);

        protected ulong GetIndex(in int index)
        {
#if CS8
            static
#endif
            Exception getError() => GetError(nameof(index));

            ulong _index = Count == 0 ? throw ThrowHelper.GetReadOnlyListOrCollectionException() : index < 0 ? throw getError() : (ulong)index;

            return _index < Count ? Start + _index : throw getError();
        }
    }

    public class IndexableULong<TArray, TItems> : ReadOnlyIndexableULong<TArray, TItems>, IIndexable<TItems> where TArray : IULongIndexable<TItems>, IULongCountable
    {
        public new TItems this[int index] { get => base[index]; set => Array.AsFromType<IULongIndexableW<TItems>>()[GetIndex(index)] = value; }
#if !CS8
        object IIndexableW.this[int index] { set => this[index]= (TItems)value; }
#endif
        public IndexableULong(in TArray array, in ulong start, in int length) : base(array, start, length) { }
        public IndexableULong(in TArray array) : base(array, 0ul, 0) { }
    }
}
#endif
