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
using System.ComponentModel;
using System.Threading;

using static WinCopies.Util.Desktop.ThrowHelper;

#if WinCopies2
namespace WinCopies.Util
#else
using WinCopies.Desktop;

namespace WinCopies
#endif
{
    /// <summary>
    /// Represents a BackgroundWorker that runs in a MTA thread by default and automatically stops on background when reports progress.
    /// </summary>
    public class BackgroundWorker : Component, IBackgroundWorker
    {
        /// <summary>
        /// <para>This event is called when the background thread starts. Put your background working code here.</para>
        /// <para>The event handler is running in the background thread.</para>
        /// </summary>
        public event DoWorkEventHandler DoWork;

        /// <summary>
        /// <para>This event is called when the background thread reports progress.</para>
        /// <para>The event handler is running in the main thread.</para>
        /// </summary>
        public event ProgressChangedEventHandler ProgressChanged;

        /// <summary>
        /// <para>This event is called when the background thread has finished working.</para>
        /// <para>The event handler is running in the background thread.</para>
        /// </summary>
        public event RunWorkerCompletedEventHandler RunWorkerCompleted;



        private Thread _Thread = null;

        private readonly ApartmentState _ApartmentState = ApartmentState.MTA;

        private readonly SynchronizationContext _SyncContext;

        private readonly ManualResetEvent _event = new ManualResetEvent(true);



        /// <summary>
        /// Gets a value that indicates whether the working has been cancelled.
        /// </summary>
        public bool IsCancelled { get; private set; } = false;

        /// <summary>
        /// Gets a value that indicates whether the thread must try to cancel before finished the background tasks.
        /// </summary>
        public bool CancellationPending { get; private set; } = false;

        ///// <para>FR: Obtient une valeur indiquant si le thread est occupé.</para>
        /// <summary>
        /// Gets a value that indicates whether the thread is busy.
        /// </summary>
        public bool IsBusy { get; private set; } = false;

        private readonly bool workerReportsProgress = false;

        /// <summary>
        /// Gets or sets a value that indicates whether the thread can notify of the progress.
        /// </summary>
        public bool WorkerReportsProgress { get => workerReportsProgress; set => this.SetBackgroundWorkerProperty(nameof(WorkerReportsProgress), nameof(workerReportsProgress), value, typeof(BackgroundWorker), true); }

        private readonly bool workerSupportsCancellation = false;

        /// <summary>
        /// Gets or sets a value that indicates whether the thread supports the cancellation.
        /// </summary>
        public bool WorkerSupportsCancellation
        {
            get => workerSupportsCancellation;

            set => this.SetBackgroundWorkerProperty(nameof(WorkerReportsProgress), nameof(workerSupportsCancellation), value, typeof(BackgroundWorker), true);
        }

        /// <summary>
        /// Gets the current progress of the current <see cref="BackgroundWorker"/> in percent.
        /// </summary>
        public int Progress { get; private set; }

        /// <summary>
        /// Gets the <see cref="System.Threading.ApartmentState"/> of this thread.
        /// </summary>
        public ApartmentState ApartmentState
        {
            get => _ApartmentState;

            set
            {
                if (_ApartmentState == value) return;

                else if (value == ApartmentState.Unknown) throw new InvalidArgumentException(nameof(value));

                _ = this.SetBackgroundWorkerProperty(nameof(ApartmentState), nameof(_ApartmentState), value, typeof(BackgroundWorker), true);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundWorker"/> class.
        /// </summary>
        public BackgroundWorker() => _SyncContext = SynchronizationContext.Current ?? new SynchronizationContext();
        // {

        // Reset()

        // _CanCancel = True

        // _ReportProgress = True

        // }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundWorker"/> class with a given <see cref="System.Threading.ApartmentState"/>.
        /// </summary>
        /// <param name="apartmentState">
        /// The <see cref="System.Threading.ApartmentState"/> in which to initialize the thread.
        /// </param>
        public BackgroundWorker(ApartmentState apartmentState)
        {
            _ApartmentState = apartmentState;

            // Reset()

            // _CanCancel = True

            // _ReportProgress = True

            _SyncContext = SynchronizationContext.Current ?? new SynchronizationContext();
        }

        /// <summary>
        /// Re-initializes the local variables.
        /// </summary>
        private void Reset(bool isCancelled)
        {
            CancellationPending = false;

            IsCancelled = isCancelled;

            if (!isCancelled)

                Progress = 0;

            _Thread = null;

            IsBusy = false;
        }

        /// <summary>
        /// Starts the working.
        /// </summary>
        public void RunWorkerAsync() => RunWorkerAsync(null);

        /// <summary>
        /// Starts the working with a custom parameter.
        /// </summary>
        /// <param name="argument">
        /// Argument given for the working.
        /// </param>
        public void RunWorkerAsync(object argument)
        {
            if (IsBusy)

                ThrowBackgroundWorkerIsBusyException();

            Reset(false);

            IsBusy = true;



            //Exception error = null;

            _Thread = new Thread(() => ThreadStart(new DoWorkEventArgs(argument)))
            {
                IsBackground = true
            };

            _Thread.SetApartmentState(_ApartmentState);

            _Thread.Start();
        }

        /// <summary>
        /// Entry point of the thread.
        /// </summary>
        private void ThreadStart(DoWorkEventArgs e)
        {
            bool isCancelled = false;

            Exception error = null;

            try
            {
                DoWork?.Invoke(this, e);
            }

            // _IsRunning = False

            // _IsCancelling = False

            catch (Exception ex)
            {
                error = ex;

                if (ex is ThreadAbortException)

                    isCancelled = true;
            }

            isCancelled = isCancelled || IsCancelled || CancellationPending || e.Cancel;

            Reset(isCancelled);

            _SyncContext.Send(ThreadCompleted, new ValueTuple<object, Exception, bool>(e.Result, error, isCancelled));
        }

        /// <summary>
        /// The method that is called when the thread has finished.
        /// </summary>
        private void ThreadCompleted(object args)
        {
            (object result, Exception ex, bool isCancelled) = (ValueTuple<object, Exception, bool>)args;

            var e = new RunWorkerCompletedEventArgs(result, ex, isCancelled);

            // IsBusy = false;

            // CancellationPending = false;

            // IsCancelled = r.Item3;    

            RunWorkerCompleted?.Invoke(this, e);
        }

        /// <summary>
        /// Cancels the working asynchronously with a custom cancellation info.
        /// </summary>
        /// <param name="stateInfo">A custom cancellation info.</param>
        public void CancelAsync(object stateInfo) => Cancel(false, stateInfo);

        /// <summary>
        /// Cancels the working using a custom cancellation info.
        /// </summary>
        /// <param name="stateInfo">A custom cancellation info.</param>
        public void Cancel(object stateInfo) => Cancel(true, stateInfo);

        /// <summary>
        /// Cancels the working asynchronously.
        /// </summary>
        public void CancelAsync() => Cancel(false, null);

        /// <summary>
        /// Cancels the working.
        /// </summary>
        public void Cancel() => Cancel(true, null);

        private void Cancel(bool abort, object stateInfo)
        {
            if (!WorkerSupportsCancellation)

                ThrowBackgroundWorkerDoesNotSupportCancellationException();



            if (_Thread == null || !IsBusy)

                return;



            if (abort)

                _Thread.Abort(stateInfo);

            //ThreadCompleted(new ValueTuple<object, Exception, bool>(null, null, true));

            else

                CancellationPending = true;
        }

        /// <summary>
        /// Delegate for progress reportting.
        /// </summary>
        /// <param name="args">
        /// Event argument.
        /// </param>
        private void OnProgressChanged(object args) => ProgressChanged?.Invoke(this, args as ProgressChangedEventArgs);

#if WinCopies2

        /// <summary>
        /// Notifies of the progress.
        /// </summary>
        /// <param name="percentProgress">
        /// Progress percentage.
        /// </param>
        public void ReportProgress(int percentProgress) => ReportProgress(percentProgress, null);

        /// <summary>
        /// Notifies of the progress.
        /// </summary>
        /// <param name="percentProgress">
        /// Progress percentage.
        /// </param>
        /// <param name="userState">
        /// User object.
        /// </param>
        public void ReportProgress(int percentProgress, object userState)

#else

        /// <summary>
        /// Notifies of the progress.
        /// </summary>
        /// <param name="progressPercentage">
        /// Progress percentage.
        /// </param>
        public void ReportProgress(int progressPercentage) => ReportProgress(progressPercentage, null);

        /// <summary>
        /// Notifies of the progress.
        /// </summary>
        /// <param name="progressPercentage">
        /// Progress percentage.
        /// </param>
        /// <param name="userState">
        /// User object.
        /// </param>
        public void ReportProgress(int progressPercentage, object userState)

#endif

        {
            if (!WorkerReportsProgress)

                ThrowBackgroundWorkerDoesNotSupportProgressionNotificationException();

            Progress =

#if WinCopies2

                percentProgress

#else

progressPercentage

#endif

                ;

            var e = new ProgressChangedEventArgs(Progress, userState);

            _SyncContext.Send(OnProgressChanged, e);
        }

        /// <summary>
        /// Suspends the current thread.
        /// </summary>
        public void Suspend()
        {
            _ = _event.Reset();

            _ = _event.WaitOne();
        }

        /// <summary>
        /// Resumes the current thread.
        /// </summary>
        public void Resume() => _event.Set();

        /// <summary>
        /// Gets a value that indicates whether the current <see cref="BackgroundWorker"/> is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

#if WinCopies2

        /// <summary>
        /// Releases resources used by the <see cref="BackgroundWorker"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="BackgroundWorker"/> is busy and does not support cancellation.</exception>
        public new void Dispose() => base.Dispose();
#endif

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="BackgroundWorker"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
        /// <exception cref="InvalidOperationException">The <see cref="BackgroundWorker"/> is busy and does not support cancellation.</exception>
        protected override void Dispose(bool disposing)
        {
            if (IsBusy)

                //    throw new InvalidOperationException(BackgroundWorkerIsBusy);

                Cancel(true, null);

            base.Dispose(disposing);

            if (IsDisposed)

                return;

            _event.Dispose();

            IsDisposed = true;
        }
    }

    namespace DotNetFix
    {
        /// <summary>
        /// Executes an operation on a separate thread.
        /// </summary>
        public interface IBackgroundWorker
        {
            #region Properties
            /// <summary>
            /// Gets or sets a value indicating whether the background worker supports asynchronous cancellation.
            /// </summary>
            /// <exception cref="InvalidOperationException">When setting: The background worker is busy.</exception>
            [DefaultValue(false)]
            bool WorkerSupportsCancellation { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the background worker can report progress updates.
            /// </summary>
            /// <exception cref="InvalidOperationException">When setting: The background worker is busy.</exception>
            [DefaultValue(false)]
            bool WorkerReportsProgress { get; set; }

            /// <summary>
            /// Gets a value indicating whether the background worker is running an asynchronous operation.
            /// </summary>
            bool IsBusy { get; }

            /// <summary>
            /// Gets a value indicating whether the application has requested cancellation of a background operation.
            /// </summary>
            bool CancellationPending { get; }
            #endregion

            #region Events
            /// <summary>
            /// Occurs when <see cref="RunWorkerAsync()"/> is called.
            /// </summary>
            event DoWorkEventHandler DoWork;

            /// <summary>
            /// Occurs when <see cref="ReportProgress(int)"/> is called.
            /// </summary>
            event ProgressChangedEventHandler ProgressChanged;

            /// <summary>
            /// Occurs when the background operation has completed, has been canceled, or has raised an exception.
            /// </summary>
            event RunWorkerCompletedEventHandler RunWorkerCompleted;
            #endregion

            #region Methods
            /// <summary>
            /// Requests cancellation of a pending background operation.
            /// </summary>
            /// <exception cref="InvalidOperationException"><see cref="WorkerSupportsCancellation"/> is <see langword="false"/>.</exception>
            void CancelAsync();

            /// <summary>
            /// Raises the <see cref="ProgressChanged"/> event.
            /// </summary>
            /// <param name="percentProgress">The percentage, from 0 to 100, of the background operation that is complete.</param>
            /// <exception cref="InvalidOperationException">The <see cref="WorkerReportsProgress"/> property is set to <see langword="false"/>.</exception>
            void ReportProgress(int percentProgress);

            /// <summary>
            /// Raises the <see cref="ProgressChanged"/> event.
            /// </summary>
            /// <param name="percentProgress">The percentage, from 0 to 100, of the background operation that is complete.</param>
            /// <param name="userState">The state object passed to <see cref="RunWorkerAsync(object)"/>.</param>
            /// <exception cref="InvalidOperationException">The <see cref="WorkerReportsProgress"/> property is set to <see langword="false"/>.</exception>
            void ReportProgress(int percentProgress, object userState);

            /// <summary>
            /// Starts execution of a background operation.
            /// </summary>
            /// <param name="argument">A parameter for use by the background operation to be executed in the <see cref="DoWork"/> event handler.</param>
            /// <exception cref="InvalidOperationException"><see cref="IsBusy"/> is <see langword="true"/>.</exception>
            void RunWorkerAsync(object argument);

            /// <summary>
            /// Starts execution of a background operation.
            /// </summary>
            /// <exception cref="InvalidOperationException"><see cref="IsBusy"/> is <see langword="true"/>.</exception>
            void RunWorkerAsync();
            #endregion
        }

        public interface IPausableBackgroundWorker : IBackgroundWorker
        {
            bool PausePending { get; }

            bool WorkerSupportsPausing { get; set; }

            void PauseAsync();
        }

        public class PausableBackgroundWorker : System.ComponentModel.BackgroundWorker, IPausableBackgroundWorker
        {
            public bool PausePending { get; private set; }

            private bool _workerSupportsPausing = false;

            public bool WorkerSupportsPausing { get => _workerSupportsPausing; set => _workerSupportsPausing = IsBusy ? throw GetBackgroundWorkerIsBusyException() : value; }

            public void PauseAsync()
            {
                if (!_workerSupportsPausing)

                    throw GetBackgroundWorkerDoesNotSupportPausingException();

                if (IsBusy)

                    PausePending = true;
            }

            protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
            {
                base.OnRunWorkerCompleted(e);

                PausePending = false;
            }
        }
    }
}
