#if CS7
using System;

using WinCopies.Collections;
using WinCopies.Collections.Generic;

namespace WinCopies.Util
{
    public static class Array
    {
        public static int TryCopy<TArrayIn, TArrayOut, TItems>(in TArrayIn source, int sourceIndex, in TArrayOut destination, int destinationIndex, int length, out NullableBool overflowInSource) where TArrayIn : ICountable, IIndexableR<TItems> where TArrayOut : ICountable, IIndexableW<TItems>
        {
            bool assert(in ICountable items, in int index) => index.Between(0, items.Count, true, false);

            overflowInSource = (assert(source, sourceIndex) ? assert(destination, destinationIndex) ? (length < 0) : throw new ArgumentOutOfRangeException(nameof(destination)) : throw new ArgumentOutOfRangeException(nameof(sourceIndex))) ? throw new ArgumentOutOfRangeException(nameof(length)) : NullableBool.None;

            if (length == 0)

                return 0;

            int actualLength;

            bool checkLength(in ICountable items, in int index) => length > (actualLength = items.Count - index);

            if (checkLength(source, sourceIndex))
            {
                int sourceActualLength = actualLength;

                if (checkLength(destination, destinationIndex))
                {
                    if (actualLength < sourceActualLength)

                        sourceActualLength = actualLength;

                    overflowInSource = NullableBool.None;
                }

                else

                    overflowInSource = NullableBool.True;

                length = actualLength = sourceActualLength;
            }

            else if (checkLength(destination, destinationIndex))
            {
                length = actualLength;
                overflowInSource = NullableBool.False;
            }

            else

                actualLength = length;

            length += sourceIndex;

            for (; sourceIndex < length; sourceIndex++, destinationIndex++)

                destination[destinationIndex] = source[sourceIndex];

            return actualLength;
        }

        public static void Copy<TArrayIn, TArrayOut, TItems>(in TArrayIn source, in int sourceIndex, in TArrayOut destination, in int destinationIndex, in int length) where TArrayIn : ICountable, IIndexableR<TItems> where TArrayOut : ICountable, IIndexableW<TItems>
        {
            if (TryCopy<TArrayIn, TArrayOut, TItems>(source, sourceIndex, destination, destinationIndex, length, out _) != length)

                throw new ArgumentOutOfRangeException(nameof(length));
        }
    }
}
#endif
