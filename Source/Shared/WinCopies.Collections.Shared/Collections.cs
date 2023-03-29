/* Copyright © Pierre Sprimont, 2020
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

namespace WinCopies.Collections
{
    /// <summary>
    /// Represents a countable item.
    /// </summary>
    public interface ICountable
    {
        int Count { get; }
    }

    /// <summary>
    /// Represents a <see cref="uint"/>-countable item.
    /// </summary>
    public interface IUIntCountable
    {
        uint Count { get; }
    }

    /// <summary>
    /// Represents a <see cref="long"/>-countable item.
    /// </summary>
    public interface ILongCountable
    {
        long Count { get; }
    }

    /// <summary>
    /// Represents a <see cref="ulong"/>-countable item.
    /// </summary>
    public interface IULongCountable
    {
        ulong Count { get; }
    }

    public interface IIndexableR
    {
        object
#if CS8
            ?
#endif
            this[int index]
        { get; }
    }

    public interface IIndexableW
    {
        object
#if CS8
            ?
#endif
            this[int index]
        { set; }
    }

    public interface IIndexable : IIndexableR, IIndexableW
    {
        // Left empty.
    }

    public interface IUIntIndexableR
    {
        object this[uint index] { get; }
    }

    public interface IUIntIndexableW
    {
        object this[uint index] { set; }
    }

    public interface IUIntIndexable : IUIntIndexableR, IUIntIndexableW
    {
        // Left empty.
    }

    public interface ILongIndexableR
    {
        object this[long index] { get; }
    }

    public interface ILongIndexableW
    {
        object this[long index] { set; }
    }

    public interface ILongIndexable : ILongIndexableR, ILongIndexableW
    {
        // Left empty.
    }

    public interface IULongIndexableR
    {
        object this[ulong index] { get; }
    }

    public interface IULongIndexableW
    {
        object this[ulong index] { set; }
    }

    public interface IULongIndexable : IULongIndexableR, IULongIndexableW
    {
        // Left empty.
    }

    namespace Generic
    {
        public interface IIndexableR<out T> : IIndexableR
        {
            new T this[int index] { get; }
#if CS8
            object IIndexableR.this[int index] => this[index];
#endif
        }

        public interface IIndexableW<in T> : IIndexableW
        {
            new T this[int index] { set; }
#if CS8
            object IIndexableW.this[int index] { set => this[index] = (T)value; }
#endif
        }

        public interface IIndexable<T> : IIndexableR<T>, IIndexableW<T>, IIndexable
        {
            // Left empty.
        }

        public interface IUIntIndexableR<out T> : IUIntIndexableR
        {
            new T this[uint index] { get; }
#if CS8
            object IUIntIndexableR.this[uint index] => this[index];
#endif
        }

        public interface IUIntIndexableW<in T> : IUIntIndexableW
        {
            new T this[uint index] { set; }
#if CS8
            object IUIntIndexableW.this[uint index] { set => this[index] = (T)value; }
#endif
        }

        public interface IUIntIndexable<T> : IUIntIndexableR<T>, IUIntIndexableW<T>, IUIntIndexable
        {
            // Left empty.
        }

        public interface ILongIndexableR<out T> : ILongIndexableR
        {
            new T this[long index] { get; }
#if CS8
            object ILongIndexableR.this[long index] => this[index];
#endif
        }

        public interface ILongIndexableW<in T> : ILongIndexableW
        {
            new T this[long index] { set; }
#if CS8
            object ILongIndexableW.this[long index] { set => this[index] = (T)value; }
#endif
        }

        public interface ILongIndexable<T> : ILongIndexableR<T>, ILongIndexableW<T>, ILongIndexable
        {
            // Left empty.
        }

        public interface IULongIndexableR<out T> : IULongIndexableR
        {
            new T this[ulong index] { get; }
#if CS8
            object IULongIndexableR.this[ulong index] => this[index];
#endif
        }

        public interface IULongIndexableW<in T> : IULongIndexableW
        {
            new T this[ulong index] { set; }
#if CS8
            object IULongIndexableW.this[ulong index] { set => this[index] = (T)value; }
#endif
        }

        public interface IULongIndexable<T> : IULongIndexableR<T>, IULongIndexableW<T>, IULongIndexable
        {
            // Left empty.
        }
    }

    public interface IEnumeratorInfo : DotNetFix.IEnumerator, DotNetFix.IEnumeratorInfo
    {
        // Left empty.
    }

    public interface IDisposableEnumeratorInfo : DotNetFix.IDisposableEnumeratorInfo, IEnumeratorInfo
    {
        // Left empty.
    }
}