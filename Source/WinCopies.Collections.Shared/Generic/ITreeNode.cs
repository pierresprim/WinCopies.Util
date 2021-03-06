﻿/* Copyright © Pierre Sprimont, 2019
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

using System.Collections.Generic;
using System.ComponentModel;

namespace WinCopies.Collections
{
    /// <summary>
    /// Represents a tree node.
    /// </summary>
    public interface IReadOnlyTreeNode
    {
        /// <summary>
        /// Gets the parent of the current node.
        /// </summary>
        IReadOnlyTreeNode Parent { get; }
    }

    ///// <summary>
    ///// Represents a tree node.
    ///// </summary>
    //public interface ITreeNode<T> : ITreeNode, IValueObject<T>

    //{

    //}

    /// <summary>
    /// Represents a tree node.
    /// </summary>
    public interface ITreeNode<T> : IReadOnlyTreeNode, IReadOnlyTreeNode<T>, WinCopies.
#if !WinCopies3
        Util.
#endif
        IValueObject<T>, WinCopies.
#if !WinCopies3
        Util.
#endif
        IValueObject, System.Collections.Generic.IList<ITreeNode<T>>, System.Collections.Generic.ICollection<ITreeNode<T>>, IEnumerable<ITreeNode<T>>, /*IEnumerable, System.Collections.IList, System.Collections.ICollection,*/ System.Collections.Generic.IReadOnlyList<ITreeNode<T>>, System.Collections.Generic.IReadOnlyCollection<ITreeNode<T>>, System.IDisposable // where TNode : ITreeNode<TNode, TItem>
    {

    }

    /// <summary>
    /// Represents a tree node.
    /// </summary>
    public interface IReadOnlyTreeNode<T> : IReadOnlyTreeNode, WinCopies.
#if !WinCopies3
        Util.
#endif
        IReadOnlyValueObject<T>, WinCopies.
#if !WinCopies3
        Util.
#endif
        IReadOnlyValueObject, System.Collections.Generic.IReadOnlyList<IReadOnlyTreeNode<T>>, System.Collections.Generic.IReadOnlyCollection<IReadOnlyTreeNode<T>>, IEnumerable<IReadOnlyTreeNode<T>>, /*IEnumerable,*/ System.IDisposable // where TNode : ITreeNode<TNode, TItem>
    {

        bool Contains(T item);

    }

    public interface IObservableTreeNode<T> : IReadOnlyTreeNode, ITreeNode<T>, IReadOnlyObservableTreeNode<T>, IReadOnlyTreeNode<T>, WinCopies.
#if !WinCopies3
        Util.
#endif
        IValueObject<T>, WinCopies.
#if !WinCopies3
        Util.
#endif
        IValueObject, System.Collections.Generic.IList<ITreeNode<T>>, System.Collections.Generic.ICollection<T>, System.Collections.Generic.IEnumerable<T>, /*IEnumerable, System.Collections.IList, System.Collections.ICollection,*/ System.Collections.Generic.IReadOnlyList<ITreeNode<T>>, System.Collections.Generic.IReadOnlyCollection<ITreeNode<T>>, System.IDisposable, System.Collections.Specialized.INotifyCollectionChanged, INotifyPropertyChanged
    { }

    public interface IReadOnlyObservableTreeNode<T> : IReadOnlyTreeNode, IReadOnlyTreeNode<T>, WinCopies.
#if !WinCopies3
        Util.
#endif
        IReadOnlyValueObject<T>, WinCopies.
#if !WinCopies3
        Util.
#endif
        IReadOnlyValueObject, System.Collections.Generic.IReadOnlyList<IReadOnlyTreeNode<T>>, System.Collections.Generic.IReadOnlyCollection<IReadOnlyTreeNode<T>>, IEnumerable<IReadOnlyTreeNode<T>>, /*IEnumerable,*/ System.IDisposable, System.Collections.Specialized.INotifyCollectionChanged, INotifyPropertyChanged
    { }

}

#endif
