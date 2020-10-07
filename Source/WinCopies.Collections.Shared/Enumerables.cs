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

using System;
using System.Collections;
using System.Collections.Generic;

namespace WinCopies.Collections
{
    public sealed class Enumerable : IEnumerable
    {
        private readonly Func<IEnumerator> _enumeratorFunc;

        public Enumerable(Func<IEnumerator> enumeratorFunc) => _enumeratorFunc = enumeratorFunc;

        public IEnumerator GetEnumerator() => _enumeratorFunc();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public sealed class Enumerable<T> : IEnumerable<T>
    {
        private readonly Func<IEnumerator<T>> _enumeratorFunc;

        public Enumerable(Func<IEnumerator<T>> enumeratorFunc) => _enumeratorFunc = enumeratorFunc;

        public IEnumerator<T> GetEnumerator() => _enumeratorFunc();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    //public sealed class EmptyCheckEnumerable : IEnumerable
    //{
    //    private Func<EmptyCheckEnumerator> _func;

    //    public EmptyCheckEnumerable(Func<EmptyCheckEnumerator> func) => _func = func;

    //    public IEnumerator GetEnumerator() => _func();
    //}

    //public sealed class EmptyCheckEnumerable<T> : IEnumerable<T>
    //{
    //    private readonly Func<EmptyCheckEnumerator<T>> _func;

    //    public EmptyCheckEnumerable(Func<EmptyCheckEnumerator<T>> func) => _func = func;

    //    public IEnumerator<T> GetEnumerator() => _func();

    //    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    //}
}
