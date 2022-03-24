using System;

using static WinCopies.
#if WinCopies3
    ThrowHelper
#else
    Util.Util
#endif
    ;

namespace WinCopies
#if !WinCopies3
    .Util
#endif
{
    public interface IForLoopAction : DotNetFix.IDisposable
    {
        void Loop(int index, int count, Func<int, bool> action);
    }

    public interface IForLoopFunc<T> : DotNetFix.IDisposable
    {
        T Loop(int index, int count, FuncOut<int, T, bool> func, out bool ok);
    }

    internal class _ForLoop : DotNetFix.IDisposable
    {
        protected class PredicateClass
        {
            private readonly FuncIn<int, bool> _defaultPredicate;

            public bool Continue { get; set; } = true;

            public FuncIn<int, bool> Predicate { get; private set; }

            public PredicateClass(FuncIn<int, bool> defaultPredicate)
            {
                _defaultPredicate = defaultPredicate;

                Predicate = (in int i) =>
                {
                    Predicate = (in int _i) => Continue && _defaultPredicate(_i);

                    return _defaultPredicate(i);
                };
            }
        }

        protected FuncIn<int, int, bool> Predicate { get; private set; }

        protected ActionRef<int> Updater { get; private set; }

        public bool IsDisposed => Updater == null;

        public _ForLoop(in FuncIn<int, int, bool> predicate, in ActionRef<int> updater)
        {
            Predicate = predicate;
            Updater = updater;
        }

        protected PredicateClass GetPredicate(int count) => new
#if !CS9
            PredicateClass
#endif
            ((in int index) => Predicate(index, count));

        public void Dispose() => Updater = null;
    }

    internal class ForLoopAction : _ForLoop, IForLoopAction
    {
        public ForLoopAction(in FuncIn<int, int, bool> predicate, in ActionRef<int> updater) : base(predicate, updater) { /* Left empty. */ }

        public void Loop(int index, int count, Func<int, bool> action)
        {
            ThrowIfNull(action, nameof(action));
            ThrowIfDisposed(this);

            PredicateClass predicate = GetPredicate(count);

            for (; predicate.Predicate(index); Updater(ref index))

                predicate.Continue = action(index);
        }
    }

    internal class ForLoopFunc<T> : _ForLoop, IForLoopFunc<T>
    {
        public ForLoopFunc(in FuncIn<int, int, bool> predicate, in ActionRef<int> updater) : base(predicate, updater) { /* Left empty. */ }

        public T Loop(int index, int count, FuncOut<int, T, bool> func, out bool ok)
        {
            ThrowIfNull(func, nameof(func));
            ThrowIfDisposed(this);

            PredicateClass predicate = GetPredicate(count);
            T item = default;

            for (; predicate.Predicate(index); Updater(ref index))

                predicate.Continue = func(index, out item);

            // We can't just return item because it would be possible that func did set item to a value different from the default one and returned true at the end.

            return (ok = !predicate.Continue) ? item : default;
        }
    }

    public static class ForLoop
    {
        private static void LoopAction(in int index, in int count, in Func<int, bool> action, in Func<IForLoopAction> forLoopFunc) => forLoopFunc().Loop(index, count, action);

        public static IForLoopAction GetForLoopActionASC() => new ForLoopAction((in int _index, in int _count) => _index < _count, (ref int _index) => _index++);

        public static void LoopActionASC(in int index, in int count, in Func<int, bool> action) => LoopAction(index, count, action, GetForLoopActionASC);

        public static IForLoopAction GetForLoopActionDESC() => new ForLoopAction((in int _index, in int _count) => _index >= _count, (ref int _index) => _index--);

        public static void LoopActionDESC(in int index, in int count, in Func<int, bool> action) => LoopAction(index, count, action, GetForLoopActionDESC);



        private static T LoopFunc<T>(in int index, in int count, out bool ok, in FuncOut<int, T, bool> func, in Func<IForLoopFunc<T>> forLoopFunc) => forLoopFunc().Loop(index, count, func, out ok);

        public static IForLoopFunc<T> GetForLoopFuncASC<T>() => new ForLoopFunc<T>((in int _index, in int _count) => _index < _count, (ref int _index) => _index++);

        public static T LoopFuncASC<T>(in int index, in int count, out bool ok, in FuncOut<int, T, bool> func) => LoopFunc(index, count, out ok, func, GetForLoopFuncASC<T>);

        public static IForLoopFunc<T> GetForLoopFuncDESC<T>() => new ForLoopFunc<T>((in int _index, in int _count) => _index >= _count, (ref int _index) => _index--);

        public static T LoopFuncDESC<T>(in int index, in int count, out bool ok, in FuncOut<int, T, bool> func) => LoopFunc(index, count, out ok, func, GetForLoopFuncDESC<T>);
    }
}
