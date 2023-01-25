/*
Original source code: Fredrik Hedblad (https://meleak.wordpress.com/2011/08/28/onewaytosource-binding-for-readonly-dependency-property/)

This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org> */

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;

using WinCopies.Diagnostics;

using static WinCopies.Util.Desktop.UtilHelpers;

namespace WinCopies.Util.Data
{
    public class PushBindingManager
    {
        private const string PushBindingsInternal = nameof(PushBindingsInternal);
        private const string StylePushBindings = nameof(StylePushBindings);

        private static readonly DependencyPropertyKey PushBindingsPropertyKey = RegisterAttachedReadOnly<PushBindingCollection, PushBindingManager>(PushBindingsInternal, new UIPropertyMetadata(null));

        public static DependencyProperty PushBindingsProperty = PushBindingsPropertyKey.DependencyProperty;

        public static PushBindingCollection
#if CS8
            ?
#endif
            GetPushBindings(DependencyObject obj)
        {
            if (obj.GetValue(PushBindingsProperty) == null)
            {
                var pushBindings = new PushBindingCollection(obj);

                obj.SetValue(PushBindingsPropertyKey, pushBindings);

                ((INotifyCollectionChanged)pushBindings).CollectionChanged += CollectionChanged;

                return pushBindings;
            }

            return null;
        }

        private static int GetId(PushBindingCollection sourceCollection) => sourceCollection.Count == 1 ? 1 : sourceCollection[
#if CS8
            ^
#else
            sourceCollection.Count -
#endif
            1].Id + 1;

        private static void CollectionChanged(object
#if CS8
            ?
#endif
            sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is PushBindingCollection pushBindings)

                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:

                        if (e.NewItems == null) break;

                        foreach (PushBinding
#if CS8
                            ?
#endif
                            item in e.NewItems)
                        {
                            if (item == null)

                                continue;

                            item.TargetObject = pushBindings.TargetObject;

                            item.Id = GetId(pushBindings);

                            item.SetupTargetBinding();
                        }

                        break;

                    case NotifyCollectionChangedAction.Remove:
                    case NotifyCollectionChangedAction.Reset:

                        if (e.OldItems != null)

                            TryRemoveStylePushBindings(GetStylePushBindings(pushBindings.TargetObject), e.OldItems, true);

                        break;

                    case NotifyCollectionChangedAction.Replace:

                        TryReplaceStylePushBindings(GetStylePushBindings(pushBindings.TargetObject), e.OldItems, e.NewItems, true);

                        break;

                    case NotifyCollectionChangedAction.Move:

                        if (e.OldStartingIndex == e.NewStartingIndex) break;

                        int difference = e.OldStartingIndex - e.NewStartingIndex;
                        int id;
                        FreezableCollection<PushBinding> styleBehaviors = GetStylePushBindings(pushBindings.TargetObject);

                        foreach (PushBinding
#if CS8
                            ?
#endif
                            item in e.OldItems)
                        {
                            if (item == null)

                                continue;

                            id = item.Id;
                            item.Id -= difference;

                            foreach (PushBinding styleItem in styleBehaviors)

                                if (styleItem.Id == id)

                                    styleItem.Id = item.Id;
                        }

                        void updateId(int startIndex, int length)
                        {
                            int count = length + 1;

                            for (int i = startIndex; i < count; i++)
                            {
                                id = pushBindings[startIndex].Id;
                                pushBindings[startIndex].Id += startIndex + 1;

                                foreach (PushBinding styleItem in styleBehaviors)

                                    if (styleItem.Id == id)

                                        styleItem.Id = pushBindings[startIndex].Id;
                            }
                        }

                        if (e.NewStartingIndex < e.OldStartingIndex)

                            updateId(e.NewStartingIndex + e.OldItems.Count, e.OldStartingIndex + e.OldItems.Count);

                        else

                            updateId(e.OldStartingIndex, e.NewStartingIndex);

                        break;
                }
        }

        public static DependencyProperty StylePushBindingsProperty = RegisterAttached<PushBindingCollection, PushBindingManager>(StylePushBindings, new UIPropertyMetadata(null, StylePushBindingsChanged));

        public static PushBindingCollection GetStylePushBindings(DependencyObject obj) => (PushBindingCollection)obj.GetValue(StylePushBindingsProperty);

        public static void SetStylePushBindings(DependencyObject obj, PushBindingCollection value) => obj.SetValue(StylePushBindingsProperty, value);

        private delegate void Action(in PushBindingCollection
#if CS8
            ?
#endif
            pushBindings, in IList
#if CS8
            ?
#endif
            pushBindingsToWorkOn);

        private static void StylePushBindingsChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target == null) return;

            void doWork(in object collection, in ActionIn<INotifyCollectionChanged> action, in Action _action)
            {
                action((INotifyCollectionChanged)collection);

                _action(GetPushBindings(target), collection as FreezableCollection<PushBinding>);
            }

            if (e.OldValue != null)

                doWork(e.OldValue, (in INotifyCollectionChanged collection) => collection.CollectionChanged -= StyleCollectionChanged, (in PushBindingCollection
#if CS8
                    ?
#endif
                    pushBindings, in IList
#if CS8
                    ?
#endif
                    pushBindingsToWorkOn) => TryRemoveStylePushBindings(pushBindings, pushBindingsToWorkOn, false));

            if (e.NewValue != null)

                doWork(e.NewValue, (in INotifyCollectionChanged collection) => collection.CollectionChanged += StyleCollectionChanged, AddStylePushBindings);
        }

        private static void AddStylePushBindings(in PushBindingCollection
#if CS8
            ?
#endif
            pushBindings, in IList
#if CS8
            ?
#endif
            pushBindingsToAdd)
        {
            if (Determine.OneOrMoreNull(pushBindings, pushBindingsToAdd))

                return;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (PushBinding
#if CS8
                ?
#endif
                pushBinding in pushBindingsToAdd)

                if (pushBinding?.Clone() is PushBinding _pushBinding)
                {
                    pushBindings.Add(_pushBinding);

                    pushBinding.Id = _pushBinding.Id;
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        private static void TryRemoveStylePushBindings(in PushBindingCollection
#if CS8
            ?
#endif
            pushBindings, in IList
#if CS8
            ?
#endif
            pushBindingsToRemove, in bool disposePushBindings)
        {
            if (pushBindingsToRemove == null) return;

            if (disposePushBindings)

                foreach (PushBinding
#if CS8
                    ?
#endif
                    pushBinding in pushBindingsToRemove)

                    pushBinding?.Dispose();

            if (pushBindings == null) return;

            for (int i = 0; i < pushBindingsToRemove.Count; i++)

                for (int j = 0; j < pushBindings.Count; j++)

                    if ((pushBindingsToRemove[i] as PushBinding)?.Id == pushBindings[j].Id)

                        pushBindings.RemoveAt(j);
        }

        private static void TryReplaceStylePushBindings(PushBindingCollection
#if CS8
            ?
#endif
            pushBindings, IList
#if CS8
            ?
#endif
            oldPushBindings, IList
#if CS8
            ?
#endif
            newPushBindings, bool disposePushBindings)
        {
            if (oldPushBindings == null) return;

            int i = 0;
            PushBinding
#if CS8
                ?
#endif
                oldItem = null;
            PushBinding
#if CS8
                ?
#endif
                newItem = null;

            void doWork()
            {
                if (disposePushBindings)

                    oldItem.Dispose();
            }

            void __doWork(in PushBinding clonedPushBinding)
            {
                for (int j = 0; j < pushBindings.Count; j++)

                    if (pushBindings[j].Id == oldItem.Id)

                        pushBindings[j] = clonedPushBinding;
            }

            void _doWork(in ActionIn<PushBinding> _action)
            {
                if ((newItem = newPushBindings[i] as PushBinding) != null)
                {
                    newItem.Id = oldItem.Id;

                    if (newItem.Clone() is PushBinding clonedPushBinding)

                        _action(clonedPushBinding);
                }
            }

            System.Action action;

            if (newPushBindings == null)

                action = doWork;

            else
            {
                ActionIn<PushBinding> _action = pushBindings == null
                    ?
#if !CS9
                    (ActionIn<PushBinding>)
#endif
                    __doWork
                    : (in PushBinding clonedPushBinding) =>
                {
                    newItem.TargetObject = clonedPushBinding.TargetObject = pushBindings.TargetObject;

                    __doWork(clonedPushBinding);
                };

                action = () =>
                {
                    _doWork(_action);

                    doWork();
                };
            }

            for (; i < oldPushBindings.Count; i++)

                if ((oldItem = oldPushBindings[i] as PushBinding) != null)

                    action();
        }

        static void StyleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var pushBindings = (PushBindingCollection)sender;

            void doWork(in IList
#if CS8
                ?
#endif
                pushBindingsToWorkOn, in Action action) => action(GetPushBindings(pushBindings.TargetObject), pushBindingsToWorkOn);

            switch (e.Action)
            {
                //when an item(s) is added we need to set the Owner property implicitly
                case NotifyCollectionChangedAction.Add:

                    doWork(e.NewItems, AddStylePushBindings);

                    break;

                //when an item(s) is removed we should Dispose the BehaviorBinding
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Reset:

                    doWork(e.OldItems, (in PushBindingCollection
#if CS8
                        ?
#endif
                        _pushBindings, in IList
#if CS8
                        ?
#endif
                        pushBindingsToWorkOn) => TryRemoveStylePushBindings(_pushBindings, pushBindingsToWorkOn, false));

                    break;

                //here we have to set the owner property to the new item and unregister the old item
                case NotifyCollectionChangedAction.Replace:

                    doWork(e.OldItems, (in PushBindingCollection
#if CS8
                        ?
#endif
                        _pushBindings, in IList
#if CS8
                        ?
#endif
                        pushBindingsToWorkOn) => TryReplaceStylePushBindings(_pushBindings, pushBindingsToWorkOn, e.NewItems, false));

                    break;

                case NotifyCollectionChangedAction.Move:

                    throw new InvalidOperationException($"{nameof(NotifyCollectionChangedAction.Move)} is not supported for style behaviors.");
            }
        }
    }
}
