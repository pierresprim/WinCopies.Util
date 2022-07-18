using System.Collections;
using System.Collections.Generic;

namespace WinCopies
{
    public interface IItemsChangedEventArgs
    {
        IEnumerable
#if CS8
            ?
#endif
            OldItems
        { get; }

        IEnumerable
#if CS8
            ?
#endif
            NewItems
        { get; }
    }

    public abstract class ItemsChangedAbstractEventArgs<T>
    {
        public T
#if CS9
            ?
#endif
            OldItems
        { get; }

        public T
#if CS9
            ?
#endif
            NewItems
        { get; }

        public ItemsChangedAbstractEventArgs(in T
#if CS9
            ?
#endif
            oldItems, in T
#if CS9
            ?
#endif
            newItems)
        {
            OldItems = oldItems;
            NewItems = newItems;
        }
    }

    public class ItemsChangedEventArgs : ItemsChangedAbstractEventArgs<IEnumerable>, IItemsChangedEventArgs
    {
        public ItemsChangedEventArgs(in IEnumerable oldItems, in IEnumerable newItems) : base(oldItems, newItems) { /* Left empty. */ }
    }

    public class ItemsChangedEventArgs<T> : ItemsChangedAbstractEventArgs<IEnumerable<T>>, IItemsChangedEventArgs
    {
        IEnumerable
#if CS8
            ?
#endif
            IItemsChangedEventArgs.OldItems => OldItems;

        IEnumerable
#if CS8
            ?
#endif
            IItemsChangedEventArgs.NewItems => NewItems;

        public ItemsChangedEventArgs(in IEnumerable<T>
#if CS8
            ?
#endif
            oldItems, in IEnumerable<T>
#if CS8
            ?
#endif
            newItems) : base(oldItems, newItems) { /* Left empty. */ }
    }

    public delegate void ItemsChangedEventHandler(object sender, ItemsChangedEventArgs e);

    public delegate void ItemsChangedEventHandler<T>(object sender, ItemsChangedEventArgs<T> e);
}
