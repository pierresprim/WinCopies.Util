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

#if WinCopies2
using static WinCopies.Util.Util;
#else
using static WinCopies.ThrowHelper;
#endif

namespace WinCopies.Collections.Generic
{
    public interface IRecursiveEnumerableProviderEnumerable<out T> : System.Collections.Generic.IEnumerable<T>
    {
        System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>> GetRecursiveEnumerator();
    }

    public interface IRecursiveEnumerable<out T> : IRecursiveEnumerableProviderEnumerable<T>
    {
        T Value { get; }
    }

    public class RecursiveEnumerator<T> : Enumerator<IRecursiveEnumerable<T>, T>
    {
        protected IStack<System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>> InnerStack { get; private set; }

        private
#if WinCopies2
bool _completed = false;
#else
            T _current;

        protected override T CurrentOverride => _current;

        public override bool? IsResetSupported => null;

        protected override void ResetOverride()
        {
            base.ResetOverride();

            InnerStack.Clear();
        }

        protected override void ResetCurrent() => _current = default;
#endif

        public RecursiveEnumerator(in IEnumerable<IRecursiveEnumerable<T>> enumerable, in IStack<System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>> stack) : base(enumerable ?? throw GetArgumentNullException(nameof(enumerable))) => InnerStack = stack;

        public RecursiveEnumerator(IEnumerable<IRecursiveEnumerable<T>> enumerable) : this(enumerable, new WinCopies.Collections.Generic.Stack<System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>>())
        {
            // Left empty.
        }

        public RecursiveEnumerator(IRecursiveEnumerableProviderEnumerable<T> enumerable, in IStack<System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>> stack) : base(new Enumerable<IRecursiveEnumerable<T>>(() => (enumerable ?? throw GetArgumentNullException(nameof(enumerable))).GetRecursiveEnumerator())) => InnerStack = stack;

        public RecursiveEnumerator(in IRecursiveEnumerableProviderEnumerable<T> enumerable) : this(enumerable, new WinCopies.Collections.Generic.Stack<System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>>>())
        {
            // Left empty.
        }

        protected override bool MoveNextOverride()
        {
#if WinCopies2
            if (_completed) return false;
#endif

            System.Collections.Generic.IEnumerator<IRecursiveEnumerable<T>> enumerator;

            void push(in IRecursiveEnumerable<T> enumerable)
            {
                enumerator = enumerable.GetRecursiveEnumerator();

#if WinCopies2
                Current
#else
                _current
#endif
                    = enumerable.Value;

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

#if WinCopies2
                    _completed = true;
#endif

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

        protected override void
#if WinCopies2
            Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
#else
DisposeManaged()
{
base.DisposeManaged();
#endif
                // {
                InnerStack = null;

            // _enumerateFunc = null;
            // }
        }
    }

    public interface IEnumeratorInfo<out T> : System.Collections.Generic.IEnumerator<T>, IEnumeratorInfo
#if !WinCopies2
        , WinCopies.DotNetFix.IDisposable
#endif
    {
        // Left empty.
    }

#if WinCopies2
    public interface IDisposableEnumeratorInfo<out T> : IEnumeratorInfo<T>, IDisposableEnumeratorInfo
    {
        // Left empty.
    }
#endif

    public interface ICountableEnumeratorInfo<out T> : IEnumeratorInfo<T>, ICountableEnumerator<T>
    {
        // Left empty.
    }

    public interface ICountableDisposableEnumeratorInfo<out T> : ICountableDisposableEnumerator<T>, ICountableEnumeratorInfo<T>
#if WinCopies2
, IDisposableEnumeratorInfo<T>
#else
        , IEnumeratorInfo<T>
#endif
    {
        // Left empty.
    }
}
