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

#if WinCopies2
using System.ComponentModel;
using System.Threading;

namespace WinCopies.Util
{
    /// <summary>
    /// Represents a BackgroundWorker that runs in a MTA thread by default and automatically stops on background when reports progress.
    /// </summary>
    public interface IBackgroundWorker : IComponent
    {
        /// <summary>
        /// Gets the <see cref="System.Threading.ApartmentState"/> of this thread.
        /// </summary>
        ApartmentState ApartmentState { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the thread must try to cancel before finished the background tasks.
        /// </summary>
        bool CancellationPending { get; }

        /// <summary>
        /// Gets a value that indicates whether the thread is busy.
        /// </summary>
        bool IsBusy { get; }

        /// <summary>
        /// Gets a value that indicates whether the working is cancelled.
        /// </summary>
        bool IsCancelled { get; }

        /// <summary>
        /// Gets the current progress of the working in percent.
        /// </summary>
        int Progress { get; }

        /// <summary>
        /// Gets or sets a value that indicates whether the thread can notify of the progress.
        /// </summary>
        bool WorkerReportsProgress { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the thread supports cancellation.
        /// </summary>
        bool WorkerSupportsCancellation { get; set; }

        /// <summary>
        /// Cancels the working.
        /// </summary>
        void Cancel();

        /// <summary>
        /// Cancels the working.
        /// </summary>
        void Cancel(object stateInfo = null);

        /// <summary>
        /// Cancels the working asynchronously.
        /// </summary>
        void CancelAsync();

        /// <summary>
        /// Cancels the working asynchronously.
        /// </summary>
        void CancelAsync(object stateInfo = null);

        /// <summary>
        /// Notifies of the progress.
        /// </summary>
        /// <param name="percentProgress">
        /// Progress percentage.
        /// </param>
        void ReportProgress(int percentProgress);

        /// <summary>
        /// Notifies of the progress.
        /// </summary>
        /// <param name="percentProgress">
        /// Progress percentage.
        /// </param>
        /// <param name="userState">
        /// User object.
        /// </param>
        void ReportProgress(int percentProgress, object userState);

        /// <summary>
        /// Suspends the current thread.
        /// </summary>
        void Suspend();

        /// <summary>
        /// Resumes the current thread.
        /// </summary>
        void Resume();

        /// <summary>
        /// <para>Called when the background thread starts. Put your background working code here.</para>
        /// <para>The event handler is running in the background thread.</para>
        /// </summary>
        event DoWorkEventHandler DoWork;

        /// <summary>
        /// <para>Called when the background thread reports progress.</para>
        /// <para>The event handler is running in the main thread.</para>
        /// </summary>
        event ProgressChangedEventHandler ProgressChanged;

        /// <summary>
        /// <para>Called when the background thread has finished working.</para>
        /// <para>The event handler is running in the background thread.</para>
        /// </summary>
        event RunWorkerCompletedEventHandler RunWorkerCompleted;
    }
}
#endif
