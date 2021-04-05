/* Copyright © Pierre Sprimont, 2021
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

using System;
using System.Collections.Generic;

using WinCopies.Collections.DotNetFix
#if WinCopies3
    .Generic
#endif
    ;
using WinCopies.Util;

namespace WinCopies
#if !WinCopies3
.Util
#endif
{
    public abstract class ValueManager<T> : DotNetFix.IDisposable
    {
        private ILinkedList<T> _values;

        protected ILinkedList<T> Values => this.GetIfNotDisposed(_values);

        public bool IsDisposed { get; private set; }

        public void Add(in T action) => _values.Add(action);

        public void Remove(in T action)
        {
#if WinCopies3
ILinkedListNode
#else
            LinkedListNode
#endif
            <T> node = _values.First;

            if (node == null)

                return;

            do
            {
                if (Equals(node.Value, action))
                {
                    _values.Remove(node);

                    break;
                }
            } while ((node = node.Next) != null);
        }

        protected virtual void Dispose(in bool disposing)
        {
            if (IsDisposed)

                return;

            if (disposing)
            {
                _values.Clear();

                _values = null;
            }

            IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }

    public class EventDelegate<T> : ValueManager<Action<T>>
    {
        public void RaiseEvent(in T param)
        {
            foreach (Action<T> action in Values)

                action(param);
        }
    }

    public interface IQueryDelegateDelegate<TIn, TOut>
    {
        Func<TIn, TOut> FirstAction { get; }

        Func<TIn, TOut, TOut> OtherAction { get; }
    }

    public class QueryDelegateDelegate<TIn, TOut> : IQueryDelegateDelegate<TIn, TOut>
    {
        public Func<TIn, TOut> FirstAction { get; }

        public Func<TIn, TOut, TOut> OtherAction { get; }

        public QueryDelegateDelegate(in Func<TIn, TOut> firstAction, in Func<TIn, TOut, TOut> otherAction)
        {
            FirstAction = firstAction;

            OtherAction = otherAction;
        }
    }

    public abstract class EventAndQueryDelegate<TIn, TOut> : ValueManager<IQueryDelegateDelegate<TIn, TOut>>
    {
        protected abstract bool Check(in TOut outParam);

        protected abstract bool Merge(in TOut x, in TOut y, out TOut result);

        public TOut RaiseEvent(TIn param)
        {
            NullableGeneric<TOut> result = null;
            bool mergeResult;

            Values.ForEach<IQueryDelegateDelegate<TIn, TOut>>(func =>
            {
                result = new NullableGeneric<TOut>(func.FirstAction(param));

                return Check(result.Value);
            }, func =>
            {
                mergeResult = Merge(result.Value, func.OtherAction(param, result.Value), out TOut mergeResultParam);

                result = new NullableGeneric<TOut>(mergeResultParam);

                return mergeResult;
            });

            return result == null ? default : result.Value;
        }
    }

    public abstract class EventAndQueryDelegate2<TIn, TOut> : EventAndQueryDelegate<TIn, TOut>
    {
        protected abstract TOut Merge(in TOut x, in TOut y);

        protected sealed override bool Merge(in TOut x, in TOut y, out TOut result)
        {
            result = Merge(x, y);

            return Check(result);
        }
    }

    public class AND_EventAndQueryDelegate<TIn> : EventAndQueryDelegate2<TIn, bool>
    {
        protected override bool Check(in bool outParam) => true;

        protected override bool Merge(in bool x, in bool y) => x && y;
    }

    public class ANDALSO_EventAndQueryDelegate<TIn> : EventAndQueryDelegate2<TIn, bool>
    {
        protected override bool Check(in bool outParam) => outParam;

        protected override bool Merge(in bool x, in bool y) => x && y;
    }

    public class OR_EventAndQueryDelegate<TIn> : EventAndQueryDelegate2<TIn, bool>
    {
        protected override bool Check(in bool outParam) => true;

        protected override bool Merge(in bool x, in bool y) => x || y;
    }

    public class ORELSE_EventAndQueryDelegate<TIn> : EventAndQueryDelegate2<TIn, bool>
    {
        protected override bool Check(in bool outParam) => !outParam;

        protected override bool Merge(in bool x, in bool y) => x || y;
    }

    public class XOR_EventAndQueryDelegate<TIn> : EventAndQueryDelegate2<TIn, bool>
    {
        protected override bool Check(in bool outParam) => true;

        protected override bool Merge(in bool x, in bool y) => x ^ y;
    }

    public class XORELSE_EventAndQueryDelegate<TIn> : EventAndQueryDelegate<TIn, bool>
    {
        protected override bool Check(in bool outParam) => true;

        protected override bool Merge(in bool x, in bool y, out bool result)
        {
            if (x && y)
            {
                result = false;

                return false;
            }

            result = x ^ y;

            return true;
        }
    }
}

#endif
