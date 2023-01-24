/* Copyright © Pierre Sprimont, 2022
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

namespace WinCopies.Collections.Generic
{
    public class SelectManyEnumerator<TIn, TOut> : Enumerator<TIn, TOut>
    {
        private TOut _current;
        private TIn  _currentEnumerable;
        private Func<bool> _moveNext;
        private Func<bool> __moveNext;
        private Func<bool> ___moveNext;

        public RecursiveEnumerationOrder EnumerationOrder { get; }

        public Func<TIn, System.Collections.Generic.IEnumerable<TOut>> EnumerableConverter { get; }
        public Func<TIn, TOut> ItemConverter { get; }

        public override bool? IsResetSupported => InnerEnumerator is IEnumeratorInfo<TIn> enumerator ? enumerator.IsResetSupported : null;

        protected override TOut CurrentOverride => _current;

        public SelectManyEnumerator(in System.Collections.Generic.IEnumerator<TIn> enumerator,  in Func<TIn, System.Collections.Generic.IEnumerable<TOut>> enumerableConverter, in Func<TIn, TOut> itemConverter,in RecursiveEnumerationOrder enumerationOrder) : base(enumerator)
        {
            EnumerationOrder = enumerationOrder;

            ValidateEnumerationOrder();

            EnumerableConverter = enumerableConverter;
            ItemConverter = itemConverter;
        }
        public SelectManyEnumerator(in System.Collections.Generic.IEnumerable<TIn> enumerable, in Func<TIn, System.Collections.Generic.IEnumerable<TOut>> enumerableConverter,in Func<TIn, TOut> itemConverter,  in RecursiveEnumerationOrder enumerationOrder) : base(enumerable)
        {
            EnumerationOrder = enumerationOrder;

            ValidateEnumerationOrder();

            EnumerableConverter = enumerableConverter;
            ItemConverter = itemConverter;
        }

        private void SetCurrent() => _current = ItemConverter(_currentEnumerable);

        private void ValidateEnumerationOrder()
        {
            bool noFlag = true;

            void setMoveNext(ref Func<bool> func, in RecursiveEnumerationOrder enumerationOrder)
            {
                if (EnumerationOrder.HasFlag(enumerationOrder))
                {
                    func = () =>
                    {
                        SetCurrent();

                        return true;
                    };

                    noFlag = false;
                }

                else

                    func = _moveNext;
            }

            setMoveNext(ref __moveNext, RecursiveEnumerationOrder.ParentThenChildren);
            setMoveNext(ref ___moveNext, RecursiveEnumerationOrder.ChildrenThenParent);

            if (noFlag)

                throw new ArgumentOutOfRangeException("enumerationOrder");

            ResetMoveNext();
        }

        private void ResetMoveNext() => _moveNext = () =>
        {
            while (InnerEnumerator.MoveNext())
            {
                _currentEnumerable = InnerEnumerator.Current;
                System.Collections.Generic.IEnumerator<TOut> currentEnumerator = EnumerableConverter(_currentEnumerable).GetEnumerator();

                _moveNext = () =>
                {
                    if (currentEnumerator.MoveNext())
                    {
                        _current = currentEnumerator.Current;

                        return true;
                    }

                    ResetMoveNext();

                    return ___moveNext();
                };

                if (__moveNext())

                    return true;
            }

            return false;
        };

        protected override bool MoveNextOverride() => _moveNext();

        protected override void ResetOverride2()
        {
            base.ResetOverride2();

            ResetMoveNext();
        }

        protected override void ResetCurrent()
        {
            base.ResetCurrent();

            _current = default;
            _currentEnumerable = default;
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();

            _moveNext = null;
            __moveNext = null;
            ___moveNext = null;
        }
    }
}
