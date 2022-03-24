using System.Collections;
using System.Collections.Generic;

namespace WinCopies
{
    public interface IItemsChangedEventArgs
    {
        IEnumerable OldItems { get; }

        IEnumerable NewItems { get; }
    }

    public abstract class ItemsChangedAbstractEventArgs<T>
    {
        public T OldItems { get; }

        public T NewItems { get; }

        public ItemsChangedAbstractEventArgs(in T oldItems, in T newItems)
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
        IEnumerable IItemsChangedEventArgs.OldItems => OldItems;

        IEnumerable IItemsChangedEventArgs.NewItems => NewItems;

        public ItemsChangedEventArgs(in IEnumerable<T> oldItems, in IEnumerable<T> newItems) : base(oldItems, newItems) { /* Left empty. */ }
    }

    public delegate void ItemsChangedEventHandler(object sender, ItemsChangedEventArgs e);

    public delegate void ItemsChangedEventHandler<T>(object sender, ItemsChangedEventArgs<T> e);
}
