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

using System.Collections;
using WinCopies.Collections.Generic;

namespace WinCopies.Collections.DotNetFix
{
    public interface ICountableEnumerable : IEnumerable
#if WinCopies3
, ICountable
#endif
    {
#if !WinCopies3
        int Count { get; }
#else
        // Left empty.
#endif
    }

    public interface IUIntCountableEnumerable : IEnumerable
#if WinCopies3
, IUIntCountable
#endif
    {
#if !WinCopies3
        uint Count { get; }
#else
        // Left empty.
#endif
    }

#if WinCopies3
    namespace Generic
    {
#endif
        public interface ICountableEnumerable<out T> : System.Collections.Generic.IEnumerable<T>, ICountableEnumerable
#if WinCopies3 && CS7
, System.Collections.Generic.IReadOnlyCollection<T>
#endif
        {
#if WinCopies3
            new ICountableEnumerator<T> GetEnumerator();
#else
        // Left empty.
#endif
        }

        public interface IUIntCountableEnumerable<out T> : System.Collections.Generic.IEnumerable<T>, IUIntCountableEnumerable
        {
#if WinCopies3
            new IUIntCountableEnumerator<T> GetEnumerator();
#else
        // Left empty.
#endif
        }

#if WinCopies3
        public interface ICountableEnumerableInfo<out T> : ICountableEnumerable<T>, IEnumerableInfo<T>
        {
            // Left empty.
        }

        public interface IUIntCountableEnumerableInfo<out T> : IUIntCountableEnumerable<T>, IEnumerableInfo<T>
        {
            // Left empty.
        }
#endif
#if WinCopies3
    }
#endif
}
