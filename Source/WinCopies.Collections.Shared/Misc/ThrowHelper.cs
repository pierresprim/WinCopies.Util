/* Copyright © Pierre Sprimont, 2019
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

#if WinCopies2
using WinCopies.Collections.DotNetFix;

using static WinCopies.Util.Resources.ExceptionMessages;
#else
using WinCopies.Collections.DotNetFix.Generic;

using static WinCopies.UtilHelpers;
using static WinCopies.ThrowHelper;
using static WinCopies.Resources.ExceptionMessages;
using static WinCopies.Collections.Resources.ExceptionMessages;
#endif

namespace WinCopies.Collections
{
    public static class ThrowHelper
    {
        /// <summary>
        /// Returns an exception indicating that a list or collection is read-only.
        /// </summary>
        /// <returns>An exception indicating that a list or collection is read-only.</returns>
        public static InvalidOperationException GetReadOnlyListOrCollectionException() => new InvalidOperationException(
#if WinCopies2
            ReadOnlyCollection
#else
            ReadOnlyListOrCollection
#endif
            );

        /// <summary>
        /// Returns an exception indicating that a list or collection is empty.
        /// </summary>
        /// <returns>An exception indicating that a list or collection is empty.</returns>
        public static InvalidOperationException GetEmptyListOrCollectionException() => new InvalidOperationException("The current list or collection is empty.");

        /// <summary>
        /// Throws the exception given by <see cref="GetEmptyListOrCollectionException"/> if the <see cref="ICountable.Count"/> property of a given <see cref="ICountable"/> object is equal to 0.
        /// </summary>
        /// <param name="obj">The <see cref="ICountable"/> object for which to check the <see cref="ICountable.Count"/> property.</param>
        public static void
#if WinCopies2
            ThrowIfEmpty
#else
ThrowIfEmptyListOrCollection
#endif
            (in ICountable obj)
        {
            if (obj.Count == 0)

                throw GetEmptyListOrCollectionException();
        }

        /// <summary>
        /// Throws the exception given by <see cref="GetEmptyListOrCollectionException"/> if the <see cref="IUIntCountable.Count"/> property of a given <see cref="IUIntCountable"/> object is equal to 0.
        /// </summary>
        /// <param name="obj">The <see cref="IUIntCountable"/> object for which to check the <see cref="IUIntCountable.Count"/> property.</param>
        public static void
#if WinCopies2
            ThrowIfEmpty
#else
ThrowIfEmptyListOrCollection
#endif
            (in IUIntCountable obj)
        {
            if (obj.Count == 0)

                throw GetEmptyListOrCollectionException();
        }

        public static ArgumentException GetNotContainedLinkedListNodeException(in string argumentName) => new ArgumentException("The given node is not contained in the current list.", argumentName);

#if CS7
        public static void ThrowIfNotContainedNode<T>(in ILinkedListNode<T> node, in string argumentName, in ILinkedList<T> list)
        {
            if (node.List != list)

                throw GetNotContainedLinkedListNodeException(argumentName);
        }

        public static void ThrowIfNodesAreEqual<T>(in ILinkedListNode<T> x, in ILinkedListNode<T> y)
        {
            if (x == y)

                throw GetNodesAreEqualException();
        }
#endif

        public static ArgumentException GetNodesAreEqualException() => new ArgumentException("The given nodes are equal.");

#if WinCopies3
        public static void ThrowIfEnumeratorNotStartedOrDisposedException(in WinCopies.Collections.IDisposableEnumeratorInfo enumerator)
        {
            if (Extensions.IsEnumeratorNotStartedOrDisposed(enumerator))

                throw GetEnumeratorNotStartedOrDisposedException();
        }
#endif
    }
}
