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

#if WinCopies3
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using WinCopies.Linq;

using static WinCopies.UtilHelpers;
using static WinCopies.Collections.Util;

namespace WinCopies.Collections.Generic
{
    public interface IPrependableExtensibleEnumerable<T> : IEnumerable<T>
    {
        void Prepend(T item);

        void PrependRange(IEnumerable<T> items);
    }

    public interface IAppendableExtensibleEnumerable<T>
    {
        void Append(T item);

        void AppendRange(IEnumerable<T> items);
    }

    public interface IExtensibleEnumerable<T> : IPrependableExtensibleEnumerable<T>, IAppendableExtensibleEnumerable<T>
    {
        // Left empty.
    }

    public abstract class ExtensibleEnumerableBase<TIn, TOut> : IExtensibleEnumerable<TOut>
    {
        protected IExtensibleEnumerable<TIn> InnerEnumerable { get; }

        protected ExtensibleEnumerableBase(IExtensibleEnumerable<TIn> enumerable) => InnerEnumerable = enumerable;

        public abstract void Prepend(TOut item);
        public abstract void PrependRange(IEnumerable<TOut> items);

        public abstract void Append(TOut item);
        public abstract void AppendRange(IEnumerable<TOut> items);

        public abstract IEnumerator<TOut> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class ExtensibleEnumerable<TIn, TOut> : ExtensibleEnumerableBase<TIn, TOut>
    {
        public ExtensibleEnumerable(IExtensibleEnumerable<TIn> enumerable) : base(enumerable) { /* Left empty. */ }

        protected void PerformAction(in TOut parameter, in string paramName, in Action<TIn> action) => PerformAction<TIn, TOut>(parameter, paramName, action);

        protected void PerformAction(in IEnumerable<TOut> parameters, in Action<System.Collections.Generic.IEnumerable<TIn>> action) => PerformAction<TIn, TOut>(parameters, action);

        public override void Append(TOut item) => PerformAction(item, nameof(item), InnerEnumerable.Append);

        public override void AppendRange(IEnumerable<TOut> items) => PerformAction(items, InnerEnumerable.AppendRange);

        public override void Prepend(TOut item) => PerformAction(item, nameof(item), InnerEnumerable.Prepend);

        public override void PrependRange(IEnumerable<TOut> items) => PerformAction(items, InnerEnumerable.PrependRange);

        public override IEnumerator<TOut> GetEnumerator() => InnerEnumerable.Cast<TOut>().GetEnumerator();
    }
}
#endif
