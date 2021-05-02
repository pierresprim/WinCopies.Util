///* Copyright © Pierre Sprimont, 2021
// *
// * This file is part of the WinCopies Framework.
// *
// * The WinCopies Framework is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * The WinCopies Framework is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//#if CS7 && WinCopies3

//using System;
//using System.Collections;

//using WinCopies.Collections.DotNetFix.Generic;

//using static WinCopies.ThrowHelper;

//namespace WinCopies.Collections.Generic
//{
//    public class LinkedListEnumerable<TItems, TList> : System.Collections.Generic.IEnumerable<TItems> where TList : ILinkedList<TItems>
//    {
//        private ILinkedListNode<TItems> _current;

//        protected TList InnerList { get; }

//        public ILinkedListNode<TItems> First { get; }

//        public ILinkedListNode<TItems> Current { get => _current; set => _current = value == null ? throw GetArgumentNullException(nameof(value)) : InnerList.Equals(value.List) ? value : throw new ArgumentException("The given node is not in this list."); }

//        public ILinkedListNode<TItems> Last { get; }

//        public LinkedListEnumerable(in TList list, in ILinkedListNode<TItems> first, in ILinkedListNode<TItems> current, in ILinkedListNode<TItems> last)
//        {
//            if (list == null)

//                throw GetArgumentNullException(nameof(list));

//            InnerList = list;

//            First = first;

//            Last = last;

//            _current = current ?? first;
//        }

//        public bool MovePrevious()
//        {
//            if (_current.Previous == null)

//                return false;

//            _current = _current.Previous;

//            return true;
//        }

//        public bool MoveNext()
//        {
//            if (_current.Next == null)

//                return false;

//            _current = _current.Next;

//            return true;
//        }

//        public System.Collections.Generic.IEnumerator<ILinkedListNode<TItems>> GetEnumeratorToCurrent(in bool keepCurrent) => InnerList.GetNodeEnumerator(First, keepCurrent ? Current : Current.Previous);

//        public System.Collections.Generic.IEnumerator<ILinkedListNode<TItems>> GetEnumeratorFromCurrent(in bool keepCurrent) => InnerList.GetNodeEnumerator(keepCurrent ? Current : Current.Next, Last);

//        public System.Collections.Generic.IEnumerator<TItems> GetEnumerator() => InnerList.GetEnumerator(First, Last);

//        System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
//    }
//}

//#endif
