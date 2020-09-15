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
using System.Collections.Generic;

namespace WinCopies.Collections.DotNetFix
{
    public interface ICountableEnumerable : IEnumerable
#if !WinCopies2
, ICountable
#endif
    {
#if WinCopies2
        int Count { get; }
#else
        // Left empty.
#endif
    }

    public interface ICountableEnumerable<out T> : IEnumerable<T>, ICountableEnumerable
    {
        // Left empty.
    }

    public interface IUIntCountableEnumerable : IEnumerable
#if !WinCopies2
, IUIntCountable
#endif
    {
#if WinCopies2
        uint Count { get; }
#else
        // Left empty.
#endif
    }

    public interface IUIntCountableEnumerable<out T> : IEnumerable<T>, IUIntCountableEnumerable
    {
        // Left empty.
    }
}
