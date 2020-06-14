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
namespace WinCopies.Util
#else
namespace WinCopies
#endif
{
#if WinCopies2
    [Obsolete("Use the TaskCompletedEventHandler delegate instead.")]
    public delegate void SucceededEventHandler(object sender, SucceededEventArgs e);

    [Obsolete("Use the TaskCompletedEventArgs class instead.")]
    public class SucceededEventArgs
    {
            public bool Succeeded { get; } = false;

        public SucceededEventArgs(bool succeeded) => Succeeded = succeeded;
            }
#endif

    public delegate void TaskCompletedEventHandler(object sender, TaskCompletedEventArgs e);

    public class TaskCompletedEventArgs
    {
        public bool Succeeded { get; } = false;

        public TaskCompletedEventArgs(bool succeeded) => Succeeded = succeeded;
    }
}
