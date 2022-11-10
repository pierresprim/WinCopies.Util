using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using WinCopies.Util;

namespace WinCopies.Collections.DotNetFix
{
    public interface IPeekable
    {
        object
#if CS8
            ?
#endif
            Peek();
        bool TryPeek(out object
#if CS8
            ?
#endif
            result);
    }

    public interface IPeekableEnumerable : IPeekable, IEnumerable
    {
        // Left empty.
    }

    public interface IPeekableEnumerableInfo<T> : IPeekableEnumerable, IEnumerableInfo<T> where T : Collections.IEnumeratorInfo
    {

    }

    public interface IPeekableEnumerableInfo : IPeekableEnumerableInfo<Collections.IEnumeratorInfo>, IEnumerableInfo
    {
        // Left empty.
    }

    namespace Generic
    {
        public interface IPeekable<T> : IPeekable
        {
            new T
#if CS9
                ?
#endif
                Peek();

            bool TryPeek(out T
#if CS9
                ?
#endif
                result);
#if CS8
            object? IPeekable.Peek() => Peek();

            bool IPeekable.TryPeek(out object? result)
            {
                if (TryPeek(out T
#if CS9
                    ?
#endif
                    _result))
                {
                    result = _result;

                    return true;
                }

                result = null;

                return false;
            }
#endif
        }

        public interface IPeekableEnumerable<T> : IPeekable<T>, IEnumerable<T>, IPeekableEnumerable
        {
            // Left empty.
        }

        public interface IPeekableEnumerableInfo<TItems, TEnumerator> : IPeekableEnumerable<TItems>, DotNetFix.IPeekableEnumerableInfo<TEnumerator>, Collections.Generic.IEnumerableInfo<TItems, TEnumerator> where TEnumerator : Collections.Generic.IEnumeratorInfo<TItems>
        {
            // Left empty.
        }

        public interface IPeekableEnumerableInfo<T> : IPeekableEnumerableInfo<T, Collections.Generic.IEnumeratorInfo<T>>, Collections.Generic.IEnumerableInfo<T>, DotNetFix.IPeekableEnumerableInfo<Collections.Generic.IEnumeratorInfo<T>>
        {
#if CS8
            System.Collections.IEnumerator IEnumerable.GetEnumerator() => this.AsFromType<System.Collections.Generic.IEnumerable<T>>().GetEnumerator();
#endif
        }
    }
}
