/* Copyright © Pierre Sprimont, 2020
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
#if !WinCopies3
using WinCopies.Util.DotNetFix;
using static WinCopies.Util.Desktop.Resources.ExceptionMessages;
#else
using WinCopies.DotNetFix;

using static WinCopies.Desktop.Resources.ExceptionMessages;
#endif

namespace WinCopies.Util.Desktop
{
    /// <summary>
    /// A static class with methods to get and throw exceptions.
    /// </summary>
    public static class ThrowHelper
    {
        /// <summary>
        /// Returns a new <see cref="InvalidOperationException"/> with <see cref="BackgroundWorkerIsBusy"/> as error message.
        /// </summary>
        /// <returns>A new <see cref="InvalidOperationException"/> with <see cref="BackgroundWorkerIsBusy"/> as error message.</returns>
        public static InvalidOperationException GetBackgroundWorkerIsBusyException() => new InvalidOperationException(BackgroundWorkerIsBusy);

        /// <summary>
        /// Throws the exception returned by the <see cref="GetBackgroundWorkerIsBusyException"/> method.
        /// </summary>
        [Obsolete("Use the ThrowIfBackgroundWorkerIsBusy method overloads instead.")]
        public static void ThrowBackgroundWorkerIsBusyException() => throw GetBackgroundWorkerIsBusyException();

        ///// <summary>
        ///// Throws the exception returned by the <see cref="GetBackgroundWorkerIsBusyException"/> method.
        ///// </summary>
        public static void ThrowIfBackgroundWorkerIsBusy(in System.ComponentModel.BackgroundWorker backgroundWorker)
        {
            if (backgroundWorker.IsBusy)

                throw GetBackgroundWorkerIsBusyException();
        }

        public static void ThrowIfBackgroundWorkerIsBusy(in IBackgroundWorker backgroundWorker)
        {
            if (backgroundWorker.IsBusy)

                throw GetBackgroundWorkerIsBusyException();
        }

        /// <summary>
        /// Returns a new <see cref="InvalidOperationException"/> with <see cref="BackgroundWorkerDoesNotSupportCancellation"/> as error message.
        /// </summary>
        /// <returns>A new <see cref="InvalidOperationException"/> with <see cref="BackgroundWorkerDoesNotSupportCancellation"/> as error message.</returns>
        public static InvalidOperationException GetBackgroundWorkerDoesNotSupportCancellationException() => new InvalidOperationException(BackgroundWorkerDoesNotSupportCancellation);

        /// <summary>
        /// Throws the exception returned by the <see cref="GetBackgroundWorkerDoesNotSupportCancellationException"/> method.
        /// </summary>
        [Obsolete("Use the ThrowIfBackgroundWorkerDoesNotSupportCancellation method overloads instead.")]
        public static void ThrowBackgroundWorkerDoesNotSupportCancellationException() => throw GetBackgroundWorkerDoesNotSupportCancellationException();

        ///// <summary>
        ///// Throws the exception returned by the <see cref="GetBackgroundWorkerDoesNotSupportCancellationException"/> method.
        ///// </summary>
        public static void ThrowIfBackgroundWorkerDoesNotSupportCancellation(in System.ComponentModel.BackgroundWorker backgroundWorker)
        {
            if (!backgroundWorker.WorkerSupportsCancellation)

                throw GetBackgroundWorkerDoesNotSupportCancellationException();
        }

        public static void ThrowIfBackgroundWorkerDoesNotSupportCancellation(in IBackgroundWorker backgroundWorker)
        {
            if (!backgroundWorker.WorkerSupportsCancellation)

                throw GetBackgroundWorkerDoesNotSupportCancellationException();
        }

        /// <summary>
        /// Returns a new <see cref="InvalidOperationException"/> with <see cref="BackgroundWorkerDoesNotSupportProgressionNotification"/> as error message.
        /// </summary>
        /// <returns>A new <see cref="InvalidOperationException"/> with <see cref="BackgroundWorkerDoesNotSupportProgressionNotification"/> as error message.</returns>
        public static InvalidOperationException GetBackgroundWorkerDoesNotSupportProgressionNotificationException() => new InvalidOperationException(BackgroundWorkerDoesNotSupportProgressionNotification);

        /// <summary>
        /// Throws the exception returned by the <see cref="GetBackgroundWorkerDoesNotSupportProgressionNotificationException"/> method.
        /// </summary>
        [Obsolete("Use the ThrowIfBackgroundWorkerDoesNotSupportProgressionNotification method overloads instead.")]
        public static void ThrowBackgroundWorkerDoesNotSupportProgressionNotificationException() => throw GetBackgroundWorkerDoesNotSupportProgressionNotificationException();

        ///// <summary>
        ///// Throws the exception returned by the <see cref="GetBackgroundWorkerDoesNotSupportProgressionNotificationException"/> method.
        ///// </summary>
        public static void ThrowIfBackgroundWorkerDoesNotSupportProgressionNotification(in System.ComponentModel.BackgroundWorker backgroundWorker)
        {
            if (!backgroundWorker.WorkerReportsProgress)

                throw GetBackgroundWorkerDoesNotSupportProgressionNotificationException();
        }

        public static void ThrowIfBackgroundWorkerDoesNotSupportProgressionNotification(in IBackgroundWorker backgroundWorker)
        {
            if (!backgroundWorker.WorkerReportsProgress)

                throw GetBackgroundWorkerDoesNotSupportProgressionNotificationException();
        }

        /// <summary>
        /// Returns a new <see cref="InvalidOperationException"/> with <see cref="BackgroundWorkerDoesNotSupportPausing"/> as error message.
        /// </summary>
        /// <returns>A new <see cref="InvalidOperationException"/> with <see cref="BackgroundWorkerDoesNotSupportPausing"/> as error message.</returns>
        public static InvalidOperationException GetBackgroundWorkerDoesNotSupportPausingException() => new InvalidOperationException(BackgroundWorkerDoesNotSupportPausing);

        /// <summary>
        /// Throws the exception returned by the <see cref="GetBackgroundWorkerDoesNotSupportPausingException"/> method.
        /// </summary>
        [Obsolete("Use the ThrowIfBackgroundWorkerDoesNotSupportPausing method instead.")]
        public static void ThrowBackgroundWorkerDoesNotSupportPausingException() => throw GetBackgroundWorkerDoesNotSupportPausingException();

#if CS7
        /// <summary>
        /// Throws the exception returned by the <see cref="GetBackgroundWorkerDoesNotSupportPausingException"/> method.
        /// </summary>
        public static void ThrowIfBackgroundWorkerDoesNotSupportPausing(in PausableBackgroundWorker backgroundWorker)
        {
            if (!backgroundWorker.WorkerSupportsPausing)

                throw GetBackgroundWorkerDoesNotSupportPausingException();
        }
#endif
    }
}
