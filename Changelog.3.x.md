WinCopies-framework
===================

The WinCopies® software framework

CHANGELOG
---------

### 09/26/2021 3.14-preview

- Re-design Action/SelectorDictionary.

### 08/27/2021 3.13-preview

 - Re-design EnumerableQueue/StackCollection
 - Change view models return type from 'void' to 'bool'.

### 08/13/2021 3.12-preview

- Move I(ReadOnlyList)LoopEnumerator to WinCopies.Collections namespace.

### 08/02/2021 3.11-preview

- Update ViewModels.

### 07/19/2021 3.10.1-preview

- ViewModelBase: some parameters were added to update helper methods and OnPropertyChanged(in System.ComponentModel.PropertyChangedEventArgs e, in object oldValue, in object newValue) is called by all other update helper methods.

### 07/17/2021 3.10-preview

- Add optional parameter 'startIndex' to ArrayEnumerator<T> constructor.
- Update RecursiveEnumerator to let the user choose how to add items.
- Some enumerables and enumerators and types and methods that implement these features which previously had IEnumeratorInfo2 as generic type constraint, generic type parameter or return type now work with IEnumeratorInfo instead of IEnumeratorInfo2 because of compatibility issues.
- IReadOnlyList indexer has been removed from versions for .Net for CS 7 and newer.

### 06/24/2021 3.9.1-preview

- IObservableLinkedListEnumerable\<T> implements INotifyPropertyChanged.
- ObservableLinkedListEnumerable\<TItems, TList> and ObservableLinkedCollectionEnumerable<TItems, TList>: TList must implement INotifyPropertyChanged
- ObservableLinkedCollectionEnumerable<TItems, TList>:
	- PropertyChanged event is implemented explicitly.
	- Add a protected virtual method to raise CollectionChanged event.
- TConversionOptions type parameter was removed from converters.
- Add new routed commands.

### 06/21/2021 3.9-preview

- Add new types and static methods.
- WinCopies.Collections.DotNetFix.Generic.LinkedList<T>:
	- AddFirst/Last return type is now WinCopies.Collections.DotNetFix.Generic.LinkedList<T>.LinkedListNode.
	- Memory leak fixed.
- WinCopies.Collections.Generic.ILinkedList(Node)Enumerable have moved to WinCopies.Collections.
- ILinkedListEnumerable:
	- implements IUIntCountable.
	- remove LinkedCollectionChangedAction.Remove event handling.
- Rewrite LinkedTreeNode-related interfaces and classes.
- Rewrite WPF Converters options, MultiConverters and MultiValueConversionAttribute.
- Bug fixed in WinCopies.Collections.AbstractionInterop.Generic.AbstractionTypes<TSource, TDestination>.ReadOnlyQueue/Stack<TQueue> implementations of ISimpleLinkedList<TDestination>.Peek() method.

### 05/24/2021 3.8-preview

- ILinkedListEnumerable implements WinCopies.DotNetFix.IDisposable
- Bug fixed in ObservableLinkedCollectionEnumerable<TItems, TList>. The Remove action was not handled.
- The LoopIteration delegate was removed because it was redundant with Predicate.
- Add Command property to ICommandSource\<T> and default interface implementation for ICommandSource.
- Add out modifier to the parameter 'T' of the Converter delegate.

### 05/06/2021 3.7.2-preview

- Bug fixed in LinkedList.Enumerator.
- EqualsConverter is in the WinCopies.Util.Data namespace.

### 05/05/2021 3.7.1-preview

- Add indexer to IArrayEnumerable<T>.
- Remove ILinkedListExtensions. Use the LinkedListEnumerator class instead.
- Rewrite LinkedList\<T>.LinkedListNode and LinkedList\<T>.UIntCountableEnumeratorInfo.
- LinkedTreeNode\<T>.Enumerator inherits from Enumerator<ILinkedListNode<LinkedTreeNode<T>>, IEnumeratorInfo2<ILinkedListNode<LinkedTreeNode<T>>>, LinkedTreeNode<T>>.
- IArray\<T> implements IReadOnlyArray\<T>.
- Update generic MultiConverterBase class.

### 05/02/2021 3.7-preview

- Update ILinkedList-like interfaces.
- LinkedList implements the new interface ILinkedListExtensions.
- Constructors of some (UInt)CountableEnumerator-like classes of the WinCopies.Collections.Enumeration namespace are public.

### 04/25/2021 3.6-preview

- Bug fixed in LinkedObservableCollection.

#### WinCopies.Collections

- Additions:
	- Classes:
		- New classes and interfaces for enumeration.
		- SubReadOnlyList\<T> classes.
		- ReversedReadOnlyList\<TItems, TList> ReversedReadOnlyList\<T> ReversedArray\<T> classes.
	- New static methods.
	- EnumeratorInfoBase.GetOrThrowIfDisposed\<T> method.
	- (I)IncrementableIReadOnlyList\<T> and IDecrementableIReadOnlyList\<T> interfaces and classes.
- ISimpleLinkedList\<T> implements ISimpleLinkedList.
- ILinkedList3\<T>, IEnumerableInfoLinkedList\<T>, IReadOnlyEnumerableInfoLinkedList\<T> and ILinkedTreeNode\<T>: add new default implementations.
- LinkedList\<T>.LinkedListNode is now defined as: LinkedListNode : ILinkedListNode\<T, LinkedListNode, LinkedList\<T>>, ILinkedListNode\<T>.
- Redefinition of I(UInt)CountableEnumerable/Enumerator-related interfaces.
- The non-generic interface WinCopies.Collections.Generic.IReadOnlyList is now in the WinCopies.Collections namespace.
- RepeatEnumerator:
	- inherits from ConditionalEnumerator\<T>.
	- has now protected constructors only. Instances can be created outside the class inheritance hierarchy from new static methods.
- Bug fixed in ArrayEnumerator\<T> and ArrayMerger\<T>.
- Removed WinCopies.Collections.Generic.IReadOnlyList2\<out T> interface.

#### WinCopies.Util.Extensions

- Additions:
	- EnumerableExtensions class with the Contains\<T> method.
	- StringExtensions class.
- Update some EventAndQueryDelegates so they are now based on delegates and add constructor for a custom default value.
- Bug fixed in ValueManager.

### 04/05/2021

#### WinCopies.Collections

- WinCopies.Collections.DotNetFix.Generic:
	- IEnumerableSimpleLinkedList\<T> is now defined as:
		IEnumerableSimpleLinkedList\<T> : ISimpleLinkedList\<T>, IUIntCountableEnumerable\<T>, IEnumerableSimpleLinkedListBase, System.Collections.Generic.IEnumerable\<T>, ICollection (, IReadOnlyCollection\<T> -- only for .Net versions that use CS7 or greater) and contains new methods.
	- Queue and Stack collections have changed to handle non-enumerable queues/stacks, for better compatibility. For enumerable features, the generic types EnumerableQueue/StackCollection have been added.
	- Add ReadOnlyQueue/Stack\<TQueue/Stack, TItems>
	- LinkedCollection implements ILinkedList3 instead of IEnumerableInfoLinkedList.
	- ObservableLinkedCollection implements ILinkedList3\<T> and its constructor takes a ILinkedList3\<T>.
	- ReadOnlyLinkedCollection implements IReadOnlyLinkedList2\<T>.
	- ReadOnlyObservableLinkedCollection:
		- Implements IReadOnlyLinkedList2\<T>.
		- Remove the RaiseCollectionChangedEvent method.
	- ILinkedList\<T> is now defined as :
		ILinkedList\<T> : IReadOnlyLinkedList2\<T>, Collections.Generic.IEnumerable\<ILinkedListNode\<T>>, ICollection\<T>, ICollection
		and has new members.
	- Remove ILinkedList3\<T>.GetNodeEnumerator(EnumerationDirection enumerationDirection) because it was redundant with the new implementation.
	- IEnumerableInfoLinkedList:
		- now implements IEnumerableInfo\<ILinkedListNode\<T>>.
		- has default implementations for C# 8 and higher.
        - the GetEnumerator method now returns an IEnumeratorInfo2\<T>.
	- IReadOnlyLinkedList\<T> is now defined as:
		IReadOnlyLinkedList\<T> : ICollection\<T>, ICollection, IReadOnlyCollection\<T>, IUIntCountable, Collections.Generic.IEnumerable\<T>
	- Remove IReadOnlyLinkedList2\<T>.GetEnumerator(EnumerationDirection enumerationDirection) because it was redundant with the new implementation.
	- IReadOnlyEnumerableInfoLinkedList\<T> is now defined as:
		IReadOnlyEnumerableInfoLinkedList\<T> : IReadOnlyLinkedList2\<T>, IEnumerableInfo\<T>
		and all of its members have been removed because they was redundant with the new implementation.
	- ReadOnlyLinkedList\<T> implements IReadOnlyLinkedList2 instead of IReadOnlyEnumerableInfoLinkedList\<T>
	- IEnumerableSimpleLinkedList\<T> is now defined as:
		IEnumerableSimpleLinkedList\<T> : ISimpleLinkedList\<T>, IUIntCountable, IEnumerableSimpleLinkedListBase, System.Collections.Generic.IEnumerable\<T>, ICollection (, IReadOnlyCollection\<T> -- for C# 7 and higher).
	- IEnumerable interfaces have been entirely redefined.
- EnumerableQueue\<T>: remove IReadOnlyCollection<T>.Count and ICollection.Count.
- ILinkedTreeNode\<T> is now defined as:
	ILinkedTreeNode\<T> : ILinkedListNode\<T>, IEnumerableInfoLinkedList\<T>
- LinkedTreeNode\<T> does not implement IEnumerableInfo\<T> anymore.
- ArrayMerger's constructor takes an ILinkedList3\IUIntCountableEnumerable\<T>>.
- Add types and new static methods.

### 03/01/2021 3.4-preview

Add new classes and static methods.

#### WinCopies.Util.Desktop

Remove the nested types in WinCopies.Util.Data.MultiConverterBase\<TSourceIn, TSourceOut, TParam, TDestination>.

### 02/27/2021 3.3-preview

#### WinCopies.Collections

- Additions:
	- WinCopies.Collections.IEnumerable\<out TItems, out TEnumerator>
- Changes:
	- WinCopies.Collections:
		- Stack/Queue.IsReadOnly are now accessible through the explicit interface implementation of ISimpleLinkedListBase.
		- The following classes are now in the WinCopies.Collections.DotNetFix namespace:
			- INotifyCollectionChanging
			- NotifyCollectionChangingEventHandler
	- WinCopies.Collections.Generic:
		- IEnumerableInfo\<out T>.GetReversedEnumerator() is now in the new IEnumerable\<out TItems, out TEnumerator> interface.
		- IReadOnlyList now implements the new IReadOnlyList interface.
	- WinCopies.Collections.DotNetFix.ISimpleLinkedListBase has changed. Some of its members are now in the new interface WinCopies.Collections.DotNetFix.ISimpleLinkedListBase2.
	- WinCopies.Collections.DotNetFix.Generic:
		- ISimpleLinkedList\<T> now implements WinCopies.Collections.DotNetFix.ISimpleLinkedListBase2.
		- IQueueBase\<T> and IStackBase\<T> now implement WinCopies.Collections.DotNetFix.ISimpleLinkedListBase.
	- WinCopies.Collections.DotNetFix:
		- IEnumerableSimpleLinkedListBase, ReadOnlySimpleLinkedListBase, SimpleLinkedListBase now implement ISimpleLinkedListBase2
	- WinCopies.Collections.DotNetFix.Generic.ArrayEnumerator\<T> is now compatible with any System.Collections.Generic.IReadOnlyList\<T>. Use WinCopies.Collections.Generic.CountableEnumerableArray\<T> to use an array with ArrayEnumerator\<T>.
	- The following methods of WinCopies.Collections.EnumerableExtensions are now in WinCopies.Linq.Extensions:
		- AsObjectEnumerable(this IEnumerable enumerable)
		- As<T>(this System.Collections.IEnumerable enumerable)
		- To<T>(this System.Collections.IEnumerable enumerable)
- WinCopies.Linq.Extensions:
	- Additions:
		- Merge(this System.Collections.Generic.IEnumerable<System.Collections.IEnumerable> enumerable)
	- The name of the following methods of this class have changed:
		- Last<T>(this WinCopies.Collections.Generic.IEnumerable<T> enumerable, Predicate<T> predicate) ==> PredicateLast
		- LastOrDefault<T>(this WinCopies.Collections.Generic.IEnumerable<T> enumerable, Predicate<T> predicate) ==> PredicateLastOrDefault
		- FirstOrDefault<T>(this IEnumerable enumerable, in Predicate<T> predicate) ==> PredicateFirstOrDefault
- WinCopies.Collections.DotNetFix.NotifyCollectionChangedEventArgs:
	- Additions:
		- NotifyCollectionChangedEventArgs(in bool isChangingEvent, in NotifyCollectionChangedAction action)
	- Removals:
		- ResetItems property
		- NotifyCollectionChangedEventArgs constructor
- Bug fixes:
	- WinCopies.Collections.DotNetFix.ReadOnlyEnumerableQueue.TryPeek kept calling itself.
	- #24
- Removals:
	- TEnumDestination generic type parameter from WinCopies.Collections.Generic.Enumerator\<TSource, TEnumSource, TDestination, TEnumDestination>. This class is now defined as Enumerator\<TSource, TEnumSource, TDestination>.
	- ArrayEnumerator.Array property.
	- ArrayEnumeratorBase
	- ListEnumerator. Use the new version of ArrayEnumerator instead.

#### WinCopies.Util.Desktop

- Bug fixed in IsNullConverter.
- Additions:
	- WinCopies.Collections.IObservableCollectionBase
	- WinCopies.Collections.DotNetFix.IObservableCollection\<T> is now in the WinCopies.Collections.DotNetFix.Generic namespace.
	- WinCopies.Collections.DotNetFix.Generic:
		- ICountableEnumerable.Count
		- IObservableCollectionBase\<T>
	- New helper methods to ConverterHelper.
	- Generic MultiConverterBase.
- AlwaysConvertibleOneWayConverter:
	The following properties and methods have now sealed overrides:
	- ConvertBackOptions
	- ConvertBack(TDestination value, TParam parameter, CultureInfo culture)
- AlwaysConvertibleOneWayToSourceConverter:
	The following properties and methods have now sealed overrides:
	- ConvertOptions
	- Convert(TSource value, TParam parameter, CultureInfo culture)

### 02/06/2021 3.2.0.0-preview

- Update WinCopies.Collections.DotNetFix.UIntIndexedListEnumerator and WinCopies.Collections.DotNetFix.Generic.UIntIndexedListEnumerator\<T> constructors.

#### WinCopies.Collections

- Changes:
	- Fixes #15
	- Fixes #22
	- ArrayEnumerator\<T> is now defined as : public class ArrayEnumerator\<T> : Enumerator\<T>, ICountableDisposableEnumeratorInfo\<T>
	- CountableEnumerableArray\<T> is now defined as : public class CountableEnumerableArray<T> : WinCopies.Collections.Generic.IReadOnlyList\<T>
	- WinCopies.Collections.DotNetFix.IEnumerator has now a MoveNext() and a Reset() method.
	- Update LinkedList classes and interfaces in order to have a better interface model regarding the enumerator provider methods.
	- WinCopies.Collections.DotNetFix.SimpleLinkedListBase.ClearItems() is now protected.
	- Some interface for uint indexation have changed in order to implement the non-generic version of the IUIntCountableEnumerable interface and the generic version in a generic context.
	- WinCopies.Collections namespace: All the types below are now in the WinCopies.Collections.Generic namespace:
		- EqualityComparison\<in T>
		- IComparer\<in T>
		- Comparer\<T>
		- IEqualityComparer\<in T>
		- EqualityComparer\<T>
	- UIntIndexedListEnumerator\<T> is now in the WinCopies.Collections.DotNetFix.Generic namespace.
	- ICountableEnumerable\<T> implements System.Collections.Generic.IReadOnlyCollection\<T>.
	- Update (I)(ReadOnly)LinkedList(Node)\<T> to avoid read-only issues.
- Removals:
	- UIntCountableEnumerable<T> class.
	- StringExtensions.StartsWith(this string s, char value) was removed from the .Net Standard 2.0, .Net Core and .Net 5 and later targetting versions because this method is now available in .Net.

#### WinCopies.Util.Desktop

- Changes:
	- ValueConverters inherit from new generic abstract types. The default converters of the WinCopies.Util.Desktop package still work as they did before but are based on new abstract classes.
	- IconToImageSourceConverter now takes System.Drawing.Icon values.
- Additions:
	- BitmapToImageSourceConverter
	- Bitmap-, Icon- and ImageSource-related extension methods.

### 12/14/2020 3.1.0.1-preview

- Supports .Net Framework 4.0 and .Net 5. (All features are not available in the .Net Framework 4.0 version.)
- Depends on Microsoft.CodeAnalysis.NetAnalyzers. (.Net 5 version.)
- Additions:
	- Interfaces and classes.
	- Static exception throwing methods.
	- Other static methods.
	- Extension methods.
- Changes:
	- WinCopies.InvalidEnumArgumentException is now in the WinCopies.Util package.
	- Classes named UtilHelpers, ThrowHelper and Extensions exist in both WinCopies.Util and WinCopies.Util.Extensions packages. Now, those from the WinCopies.Util package are now in the WinCopies namespace, except Extensions which is in the WinCopies.Util namespace, and those from the WinCopies.Util.Extensions package are now in the WinCopies.Extensions namespace in order to avoid name conflicts.
	- Some comparison-related types in the WinCopies.Collections namespace have moved to the WinCopies.Util package, and are still in the same namespace.
	- Enum throw methods are now in the WinCopies.ThrowHelper class of the WinCopies.Util package.
	- WinCopies.Collections.EnumerableExtensions.Join\<T> and AppendValues methods are now in WinCopies.Linq.Extensions.
	- Some types are not supported anymore by the .Net Framework 4.0 targetting version.
	- ToEnumerable\<T>(this T[] array) is now in WinCopies.Collections.ArrayExtensions (WinCopies.Collections package).
	- Some static methods have new return types. These types as compatible with the previous ones, so these methods should remain compatible with older releases, but their new return types extend the capacity of the usage of the returned values.

### 12/09/2020 3.1.0.0-preview

- Bug fixes.
- Changes:
	- WinCopies.Collections.DotNetFix namespace:
		- ICountableEnumerable\<T> and IUIntCountableEnumerable\<T> are now in the WinCopies.Collections.DotNetFix.Generic namespace.
	- WinCopies.Collections.DotNetFix.Generic.ILinkedList\<T>.GetNodeEnumerator(EnumerationDirection enumerationDirection) is now in ILinkedList3\<T> for better compatibility with previous versions.
	- WinCopies.Collections.DotNetFix.Generic.LinkedCollection\<T> now implements ILinkedList3\<T> and has new protected virtual methods.
	- Some changes in linked list-related interfaces (such as IStack\<T>, IQueue\<T>, ...)
	- WinCopies.TypeForDataTemplateAttribute is now sealed.
	- Some extension methods have moved to specific static extension classes.
	- ILinkedList2\<T> is now obsolete.
- Additions:
	- Classes:
		- WinCopies.Collections.Generic.UIntCountableEnumerableArray\<T>
		- WinCopies.Collections.DotNetFix.Generic.UIntCountableEnumerable
		- WinCopies.Collections.Generic.ArrayMerger\<T>
		- WinCopies.Collections.Generic.(I)LinkedTreeNode
		- EnumerableHelper\<T>. This class contains static methods that return linked list-based interfaces that are "simplified" versions of their related "complete" interface versions. These interfaces are useful to work only with the features that are really needed.
		- IEnumerableInfo\<T>
		- IEnumeratorInfo2\<T>
		- Enumerator<TSource, TEnumSource, TDestination, TEnumDestination>
	- Methods:
		- Add GetIf methods in WinCopies.Util.Extensions.
		- Methods to the LinkedList\<T> class and the ILinkedList3\<T> interface.
- Removals:
	- WinCopies.Collections.DotNetFix.Generic.ReadOnlyLinkedList.GetNodeEnumerator(EnumerationDirection enumerationDirection) was removed because the GetNodeEnumerator() is now in ILinkedList3<T> for better compatibility.
	- WinCopies.Collections.DotNetFix.Generic.IReadOnlyLinkedList\<T>.GetNodeEnumerator(EnumerationDirection enumerationDirection) was removed because the GetNodeEnumerator() is now in ILinkedList3<T> for better compatibility.

### 11/01/2020 3.0.0-preview

- Changes:
	- WinCopies.Util namespace to WinCopies (WinCopies.Util.Data namespace has not changed because of WinCopies.Data package).
	- IBackgroundWorker interface moved to WinCopies.Util.Desktop package. Some items and names have changed.
	- Some obsolete types and members have been removed.

#### WinCopies.Util

- Removals:
	- Methods:
		- WinCopies.Util.Util.ThrowIfNull(object, string) method. Please use the generic method instead.
		- WinCopies.Util.Extensions:
			- object\[] AddRangeIfNotContains(this System.Collections.ICollection collection, params object\[] values)
			- T\[] RemoveRangeIfContains\<T>(this ICollection\<T> collection, params T\[] values)
			- T\[] RemoveRangeIfContains\<T>(this ICollection\<T> collection, in IEnumerable\<T> values)
			- bool ContainsOneValue(this IEnumerable array, Comparison\<object> comparison, out bool containsMoreThanOneValue, params object\[] values)
			- object GetNumValue(this Enum @enum, in string enumName)
			- bool Contains(this string s, IEqualityComparer\<char> comparer, string value)
			- bool Contains(this string s, char value, IEqualityComparer\<char> comparer, out int index)
		- WinCopies.Util.Util:
			- (bool propertyChanged, object oldValue) SetPropertyWhenNotBusy<T>(T bgWorker, string propertyName, string fieldName, object newValue, Type declaringType, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, bool throwIfBusy = true) where T : IBackgroundWorker, INotifyPropertyChanged
			- void ThrowIfNull(in object obj, in string argumentName)
			- T\[] ConcatenateLong\<T>(params T[][] arrays)
	- Classes:
		- LinkedList\<T>
		- ReadOnlyLinkedList\<T>
	- Interfaces:
		- WinCopies.Collections:
			- IUIntIndexedCollection
			- IUIntIndexedCollection\<T>
			- UIntIndexedCollectionEnumeratorBase
			- UIntIndexedCollectionEnumerator
			- UIntIndexedCollectionEnumerator\<T>
			- ReadOnlyObservableCollection\<T>
- Changes:
	- All linked lists (Stack and Stack-based, Queue and Queue-based and linked lists and collections) are now completely WinCopies Framework native and have changed consequently.
	- Classes:
		- WinCopies.Util.Util => WinCopies.UtilHelpers
	- Enums:
		- WinCopies.Util.Util.ComparisonType => WinCopies.Diagnostics.ComparisonType
		- WinCopies.Util.Util.ComparisonMode => WinCopies.Diagnostics.ComparisonMode
		- WinCopies.Util.Util.Comparison => WinCopies.Diagnostics.Comparison
	- Static and extension methods:
		- 'If' methods are now in the WinCopies.Diagnostics.IfHelpers namespace.
		- Get- and ThrowException methods are now all in the WinCopies.ThrowHelper class.
		- WinCopies.UtilHelpers.GetNumValue is now generic and the 'enumType' parameter has been removed.
	- Misc:
		- StringParameterEmptyOrWhiteSpaces resource to StringParameterEmptyOrWhiteSpace
- Additions:
	- WinCopies.Collections.Generic.IEnumerable\<T> interface.

#### WinCopies.Util.Desktop

- Additions:
	- IPausableBackgroundWorker interface.
	- PausableBackgroundWorker extension methods.
- Removals:
	- Properties:
		- WinCopies.Util.Commands.ApplicationCommands.CloseWindow
