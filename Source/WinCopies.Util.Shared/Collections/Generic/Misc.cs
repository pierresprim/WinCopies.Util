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

#if WinCopies2

using System.Collections.Generic;

using WinCopies.Collections.DotNetFix.Generic;

using static WinCopies.Util.Util;

namespace WinCopies.Collections.Generic
{
    public interface ITreeEnumerableProviderEnumerable<out T> : IEnumerable<T>
    {
        IEnumerator<ITreeEnumerable<T>> GetRecursiveEnumerator();
    }

    public interface ITreeEnumerable<out T> : ITreeEnumerableProviderEnumerable<T>
    {
        T Value { get; }
    }

    public class TreeEnumerator<T> : Enumerator<ITreeEnumerable<T>, T>
    {
        private Stack<IEnumerator<ITreeEnumerable<T>>> _stack = new Stack<IEnumerator<ITreeEnumerable<T>>>();

        // private bool _firstLaunch = true;
        private bool _completed = false;

        // private Func<string, IEnumerable<string>> _enumerateFunc;

        public TreeEnumerator(IEnumerable<ITreeEnumerable<T>> enumerable) : base(enumerable ?? throw GetArgumentNullException(nameof(enumerable)))
        {
            // Left empty.
        }

        public TreeEnumerator(ITreeEnumerableProviderEnumerable<T> enumerable) : base(new Enumerable<ITreeEnumerable<T>>(() => (enumerable ?? throw GetArgumentNullException(nameof(enumerable))).GetRecursiveEnumerator()))
        {
            // Left empty.
        }

        protected override bool MoveNextOverride()
        {
            if (_completed) return false;

            //                void _markAsCompleted()
            //                {
            //                    _stack = null;

            //                    _completed = true;
            //                }

            //                bool dequeueDirectory()
            //                {
            IEnumerator<ITreeEnumerable<T>> enumerator;

            void push(in ITreeEnumerable<T> enumerable)
            {
                enumerator = enumerable.GetRecursiveEnumerator();

                Current = enumerable.Value;

                _stack.Push(enumerator);
            }

            //new FileSystemEntryEnumerator(
            //#if DEBUG
            //                    Current,
            //#endif
            //_enumerateDirectoriesFunc(Current.Path), _enumerateFilesFunc(Current.Path)));

            while (true)
            {
                if (_stack.Count == 0)
                {
                    //                            if (_directories == null)
                    //                            {
                    //                                _directories = null;

                    //                                _markAsCompleted();

                    //                                return false;
                    //                            }

                    if (InnerEnumerator.MoveNext())
                    {
                        push(InnerEnumerator.Current);

                        return true;
                    }

                    _completed = true;

                    return false;

                    //                            if (_directories.Count == 0)

                    //                                _directories = null;
                }

                enumerator = _stack.Peek();

                //                        //#if DEBUG
                //                        //                        SimulationParameters?.WriteLogAction($"Peeked enumerator: {enumerator.PathInfo.Path}");
                //                        //#endif

                if (enumerator.MoveNext())
                {
                    //Current = enumerator.Current;

                    //                            //#if DEBUG
                    //                            //                            SimulationParameters?.WriteLogAction($"Peeked enumerator: {enumerator.PathInfo.Path}; Peeked enumerator current: {enumerator.Current.Path}");
                    //                            //#endif

                    //if (enumerator.Current.IsDirectory)

                    push(enumerator.Current);

                    return true;
                }

                else

                    _ = _stack.Pop();

                //                        //#if DEBUG
                //                        //                        SimulationParameters?.WriteLogAction($"Peeked enumerator: {enumerator.PathInfo.Path}; Peeked enumerator move next failed.");
                //                        //#endif

                //                        _ = _stack.Pop();
            }

            //}

            //if (_firstLaunch)
            //{
            //_firstLaunch = false;

            //IPathInfo path;

            //while (InnerEnumerator.MoveNext())
            //{
            //    path = InnerEnumerator.Current;

            //    (path.IsDirectory ? _directories : _files).Enqueue(path);
            //}

            //if (_files.Count == 0)
            //{
            //    _files = null;

            //    if (_directories.Count == 0)
            //    {
            //        _directories = null;

            //        _markAsCompleted();

            //        return false;
            //    }

            //    _ = dequeueDirectory();

            //    return true;
            //}

            //if (_directories.Count == 0)

            //    _directories = null;

            //Current = _files.Dequeue();

            //if (_files.Count == 0)

            //    _files = null;

            //return true;
            //}

            //if (_files == null)
            //{
            //    if (_directories == null && _stack.Count == 0)
            //    {
            //        _markAsCompleted();

            //        return false;
            //    }

            //    if (dequeueDirectory())

            //        return true;

            //    _markAsCompleted();

            //    return false;
            //}

            //Current = _files.Dequeue();

            //if (_files.Count == 0)

            //    _files = null;

            //return true;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                // {
                _stack = null;

            // _enumerateFunc = null;
            // }
        }
    }

    public interface IEnumeratorInfo<out T> : System.Collections.Generic.IEnumerator<T>, IEnumeratorInfo
    {
        // Left empty.
    }

    public interface IDisposableEnumeratorInfo<out T> : IEnumeratorInfo<T>, IDisposableEnumeratorInfo
    {
        // Left empty.
    }

    public interface ICountableEnumeratorInfo<out T> : IEnumeratorInfo<T>, ICountableEnumerator<T>
    {
        // Left empty.
    }

    public interface ICountableDisposableEnumeratorInfo<out T> : ICountableDisposableEnumerator<T>, IDisposableEnumeratorInfo<T>, ICountableEnumeratorInfo<T>
    {
        // Left empty.
    }
}

#endif
