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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using WinCopies.Collections;
using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.Generic;
using WinCopies.Extensions.Util;

namespace WinCopies.Extensions
{
    public static class Array
    {
        public static Action<int, int> GetLosslessShiftDelegate(this IList array, IQueueBase queue) => (int @in, int @out) =>
            {
                queue.Enqueue(array[@in]);

                array[@in] = array[@out];
            };

        #region Lossless
        private sealed class LosslessShiftOverlapDelegate
        {
            private int _i;
            private Action<int, int> _action;

            public LosslessShiftOverlapDelegate(IList array, in IQueueBase queue, uint offset)
            {
                Action<int, int> action = array.GetLosslessShiftDelegate(queue);

                _action = (int @out, int @in) =>
                {
                    action(@out, @in);

                    if (++_i == offset)

                        _action = array.GetShiftDelegate();
                };
            }

            public void Move(int @out, int @in) => _action(@out, @in);
        }

        private delegate bool LosslessShiftDelegate(in Action<int, int> action, out int lOut, out int rOut);

        private static bool LosslessShift(this IList array, int start, int length, int offset, out int rOut, in LosslessShiftDelegate losslessShiftDelegate)
        {
            uint _offset = (uint)System.Math.Abs(offset);
            int i = 0;
            Action postWork;
            Func<int> func = null;
            Action<int, int> action;

            void setIndex()
            {
                int _length = length - 1;

                if (length > _offset)

                    _length -= length - (int)_offset;

                i = WinCopies.UtilHelpers.GetIndex(start, array.Count, ref _length);
            }

            if (_offset == 1 || length == 1)
            {
                object
#if CS8
                    ?
#endif
                    value = null;

                Action<int, int> _action = (int @in, int @out) =>
                {
                    value = array[@in];

                    array[@in] = array[@out];

                    _action = array.GetShiftDelegate();
                };

                action = (int @in, int @out) => _action(@in, @out);

                postWork = () =>
                {
                    if (offset > 0)

                        setIndex();

                    else
                    {
                        int __offset = length - 1;

                        i = WinCopies.UtilHelpers.GetIndex(start, array.Count, ref __offset);
                    }

                    array[i] = value;
                };
            }

            else // TODO: an optmization can be made: if there is no overlap, there is no need to store the values in a temporary collection. They should be swapped directly.
            {
                IQueueBase queue = EnumerableHelper.GetQueue();
                bool overlap;
                action = (overlap = length > _offset) ? new LosslessShiftOverlapDelegate(array, queue, _offset).Move : array.GetLosslessShiftDelegate(queue);
                postWork = () =>
                {
                    if (offset > 0)
                    {
                        func = () => (i == 0 ? array.Count : i) - 1;
                        setIndex();
                    }

                    else
                    {
                        func = () => (i + 1) % array.Count;
                        i = start;

                        if (overlap)
                        {
                            start = length - (int)_offset;

                            WinCopies.UtilHelpers.SetIndex(ref i, array.Count, ref start);
                        }
                    }

                    Func<int> getIndex = () =>
                    {
                        getIndex = () => i = func();

                        return i;
                    };

                    do

                        array[getIndex()] = queue.Dequeue();

                    while (queue.HasItems);
                };
            }

            if (losslessShiftDelegate(action, out int lOut, out rOut))
            {
                postWork();

                return true;
            }

            return false;
        }

        public static bool LosslessShiftByPosition(this IList array, int inStart, int length, ref int outStart, ref int avoidOverflow, out int offset, out int rOut)
        {
            int _outStart = outStart;
            int _avoidOverflow = avoidOverflow;
            int _offset = 0;

            bool result = array.LosslessShift(inStart, length, _offset = WinCopies.UtilHelpers.GetOffset(inStart, outStart, length), out rOut, (in Action<int, int> action, out int lOut, out int _rOut) => array.Shift(inStart, length, ref _offset, ref _avoidOverflow, action, out lOut, out _rOut));

            outStart = _outStart;
            avoidOverflow = _avoidOverflow;
            offset = _offset;

            return result;
        }

        public static bool LosslessShift(this IList array, int start, int length, ref int offset, ref int avoidOverflow, out int lOut, out int rOut)
        {
            int _offset = offset;
            int _avoidOverflow = avoidOverflow;
            int _lOut = -1;

            bool result = array.LosslessShift(start, length, offset, out rOut, (in Action<int, int> action, out int __lOut, out int _rOut) =>
            {
                bool _result = array.Shift(start, length, ref _offset, ref _avoidOverflow, action, out _lOut, out _rOut);

                __lOut = _lOut;

                return _result;
            });

            offset = _offset;
            avoidOverflow = _avoidOverflow;
            lOut = _lOut;

            return result;
        }
        #endregion Lossless
    }
}
