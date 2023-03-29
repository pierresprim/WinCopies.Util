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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
#if NETCORE || NET
using System.Runtime.Loader;
#endif

using static WinCopies.UtilHelpers;

namespace WinCopies
{
    public interface IArrayValueProvider<T>
    {
#if CS5
        IReadOnlyList<T>
#else
        T[]
#endif
        Items
        { get; }

        int CurrentIndex { get; }

        T CurrentValue { get; }
    }

    public struct ArrayValueProvider<T> : IArrayValueProvider<T>
    {
        public
#if CS5
            IReadOnlyList<T>
#else
            T[]
#endif
            Items
        { get; }

        public int CurrentIndex { get; private set; }

        public T CurrentValue => Items[CurrentIndex++];

        public ArrayValueProvider(in
#if CS5
            IReadOnlyList<T>
#else
            T[]
#endif
            items, in int startIndex)
        {
            Items = items;
            CurrentIndex = startIndex;
        }

        public ArrayValueProvider(in
#if CS5
            IReadOnlyList<T>
#else
            T[]
#endif
            items) : this(items, 0) { /* Left empty. */ }
    }

    public struct ArrayValueProvider2<T> : IArrayValueProvider<T>
    {
        private ArrayValueProvider<T> _values;

        public
#if CS5
            IReadOnlyList<T>
#else
            T[]
#endif
            Items => _values.Items;

        public int CurrentIndex => _values.CurrentIndex;

        public T CurrentValue
        {
            get
            {
                if (Count < Length)
                {
                    Count++;

                    return _values.CurrentValue;
                }

                throw new IndexOutOfRangeException();
            }
        }

        public int Count { get; private set; }

        public int Length { get; }

        public ArrayValueProvider2(in ArrayValueProvider<T> values, in int length)
        {
            _values = values;
            Count = 0;
            Length = length > values.Items.
#if CS5
                Count
#else
                Length
#endif
                - values.CurrentIndex ? throw new ArgumentOutOfRangeException(nameof(length)) : length;
        }

        public ArrayValueProvider2(in
#if CS5
            IReadOnlyList<T>
#else
            T[]
#endif
            items, in int startIndex, in int length) : this(new ArrayValueProvider<T>(items, startIndex), length) { /* Left empty. */ }

        public ArrayValueProvider2(in
#if CS5
            IReadOnlyList<T>
#else
            T[]
#endif
            items, in int length) : this(new ArrayValueProvider<T>(items), length) { /* Left empty. */ }
    }
#if CS8 && !NETSTANDARD
    public class AssemblyLoadContext : System.Runtime.Loader.AssemblyLoadContext
    {
        protected AssemblyDependencyResolver Resolver { get; }
        public string Path { get; }

        public AssemblyLoadContext(string path, bool isCollectible = false) : base(isCollectible) => Resolver = new AssemblyDependencyResolver(Path = path);

        protected override Assembly Load(AssemblyName assemblyName) => PerformActionIfNotNull(Resolver.ResolveAssemblyToPath(assemblyName), LoadFromAssemblyPath);

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName) => PerformActionIfNull(Resolver.ResolveUnmanagedDllToPath(unmanagedDllName), IntPtr.Zero, LoadUnmanagedDllFromPath);
    }
#endif
    public enum NullableBool : sbyte
    {
        None = -1,
        False = 0,
        True = 1,
    }

    /// <summary>
    /// This enum is designed as an extension of the <see cref="bool"/> type.
    /// </summary>
    public enum Result : sbyte
    {
        /// <summary>
        /// An error as occurred.
        /// </summary>
        Error = -3,

        /// <summary>
        /// The operation has been canceled.
        /// </summary>
        Canceled = -2,

        /// <summary>
        /// The operation did not return any particular value. This value is the same as returning a <see langword="null"/> <see cref="Nullable{Boolean}"/>.
        /// </summary>
        None = NullableBool.None,

        /// <summary>
        /// The operation returned False. This value is the same number as <see langword="false"/>.
        /// </summary>
        False = NullableBool.False,

        /// <summary>
        /// The operation returned True. This value is the same number as <see langword="true"/>.
        /// </summary>
        True = NullableBool.True
    }

    public enum XOrResult : sbyte
    {
        MoreThanOneTrueResult = -1,

        NoTrueResult = 0,

        OneTrueResult = 1
    }

    public abstract class DisposableBase : DotNetFix.IDisposable
    {
        public abstract bool IsDisposed { get; }

        protected abstract void DisposeManaged();
        protected abstract void DisposeUnmanaged();

        public void Dispose()
        {
            if (IsDisposed)

                return;

            DisposeUnmanaged();
            DisposeManaged();

            GC.SuppressFinalize(this);
        }

        ~DisposableBase() => DisposeUnmanaged();
    }

    public abstract class Disposable : DisposableBase
    {
        private bool _isDisposed;

        public override bool IsDisposed => _isDisposed;

        protected override void DisposeManaged() => _isDisposed = true;
    }

    public abstract class DisposableValue<T> : System.IDisposable where T : DotNetFix.IDisposable
    {
        protected T Value { get; }

        protected DisposableValue(T value) => Value = value == null ? throw ThrowHelper.GetArgumentNullException(nameof(value)) : value;

        protected virtual void Dispose(in bool disposing)
        {
            if (!Value.IsDisposed)

                Value.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }

    public interface IPropertyObservable : DotNetFix.IDisposable
    {
        void AddPropertyChangedDelegate(Action<string> action);

        void RemovePropertyChangedDelegate(Action<string> action);
    }

    public interface IAsEquatable<T>
    {
        IEquatable<T
#if CS9
            ?
#endif
            > AsEquatable();
    }

    public interface IValueProviderBase : DotNetFix.IDisposable
    {
#if CS8
        [NotNull]
#endif
        object Value { get; }
    }

    public interface IValueProvider : IValueProviderBase, IEquatable<IValueProviderBase>
    {
        // Left empty.
    }

    public interface IValueProvider<T> : IValueProviderBase, IEquatable<T>
    {
#if CS8
        object IValueProviderBase.Value => Value;

        [NotNull]
#endif
        new T Value { get; }
    }

    public delegate bool TaskAwaiterPredicate(ref bool cancel);

    public interface ISplitFactory<T, U, TContainer>
    {
        TContainer Container { get; }

        int SubCount { get; }

        void SubClear();
    }

    public interface IValueSplitFactory<T, U, TContainer> : ISplitFactory<T, U, TContainer>
    {
        void Add(U enumerable);

        U GetEnumerable();

        void SubAdd(T value);
    }

    public interface IRefSplitFactory<T, U, V, TContainer> : ISplitFactory<T, U, TContainer> where T : class
    {
        void Add(V enumerable);

        V GetEnumerable();

        void SubAdd(U value);

        U GetValueContainer(T value);
    }

    public interface IPopable<TIn, TOut>
    {
        TOut Pop(TIn key);
    }

    public interface IAsEnumerable<
#if CS5
        out
#endif
        T>
    {
        IEnumerable<T> AsEnumerable();
    }

    public interface IAsEnumerableAlt<
#if CS5
        out
#endif
        T>
    {
        IEnumerable<T> AsEnumerableAlt();
    }
}
