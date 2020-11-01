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

using System.Collections.Generic;

using WinCopies.Collections.DotNetFix.Generic;

using static WinCopies.Util.Util;

namespace WinCopies.Collections.Generic
{
    public interface IRecursiveEnumerableProviderEnumerable<out T> : IEnumerable<T>
    {
        IEnumerator<IRecursiveEnumerable<T>> GetRecursiveEnumerator();
    }

    public interface IRecursiveEnumerable<out T> : IRecursiveEnumerableProviderEnumerable<T>
    {
        T Value { get; }
    }

    public class RecursiveEnumerator<T> : Enumerator<IRecursiveEnumerable<T>, T>
    {
        protected IStack<IEnumerator<IRecursiveEnumerable<T>>> InnerStack { get; private set; }

        private bool _completed = false;

        public RecursiveEnumerator(in IEnumerable<IRecursiveEnumerable<T>> enumerable, in IStack<IEnumerator<IRecursiveEnumerable<T>>> stack) : base(enumerable ?? throw GetArgumentNullException(nameof(enumerable))) => InnerStack = stack;

        public RecursiveEnumerator(IEnumerable<IRecursiveEnumerable<T>> enumerable) : this(enumerable, new WinCopies.Collections.Generic.Stack<IEnumerator<IRecursiveEnumerable<T>>>())
        {
            // Left empty.
        }

        public RecursiveEnumerator(IRecursiveEnumerableProviderEnumerable<T> enumerable, in IStack<IEnumerator<IRecursiveEnumerable<T>>> stack) : base(new Enumerable<IRecursiveEnumerable<T>>(() => (enumerable ?? throw GetArgumentNullException(nameof(enumerable))).GetRecursiveEnumerator())) => InnerStack = stack;

        public RecursiveEnumerator(in IRecursiveEnumerableProviderEnumerable<T> enumerable) : this(enumerable, new WinCopies.Collections.Generic.Stack<IEnumerator<IRecursiveEnumerable<T>>>())
        {
            // Left empty.
        }

        protected override bool MoveNextOverride()
        {
            if (_completed) return false;

            IEnumerator<IRecursiveEnumerable<T>> enumerator;

            void push(in IRecursiveEnumerable<T> enumerable)
            {
                enumerator = enumerable.GetRecursiveEnumerator();

                Current = enumerable.Value;

                InnerStack.Push(enumerator);
            }

            while (true)
            {
                if (InnerStack.Count == 0)
                {
                    if (InnerEnumerator.MoveNext())
                    {
                        push(InnerEnumerator.Current);

                        return true;
                    }

                    _completed = true;

                    return false;
                }

                enumerator = InnerStack.Peek();

                if (enumerator.MoveNext())
                {
                    push(enumerator.Current);

                    return true;
                }

                else

                    _ = InnerStack.Pop();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                // {
                InnerStack = null;

            // _enumerateFunc = null;
            // }
        }
    }
    public interface IEnumeratorInfo<out T> : IEnumerator<T>, IEnumeratorInfo
    {
        // Left empty.
    }

    public interface IDisposableEnumeratorInfo<out T> : IEnumeratorInfo<T>, IDisposableEnumeratorInfo
    {
        // Left empty.
    }

    public interface ICountableEnumeratorInfo<out T> : IEnumeratorInfo<T>, ICountableEnumerator<T>
    {
        // Left empty.
    }

    public interface ICountableDisposableEnumeratorInfo<out T> : ICountableDisposableEnumerator<T>, IDisposableEnumeratorInfo<T>, ICountableEnumeratorInfo<T>
    {
        // Left empty.
    }
}
