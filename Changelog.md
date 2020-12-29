WinCopies-framework
===================

The WinCopies® software framework

CHANGELOG
=========

Updates
-------

??/??/???? 3.2.0.0-preview
==========================

- Supports .Net Framework 4.0 and .Net 5. (All features are not available in the .Net Framework 4.0 version.)
- Depends on Microsoft.CodeAnalysis.NetAnalyzers. (.Net 5 version.)
- Changes:
	- WinCopies.InvalidEnumArgumentException is now in the WinCopies.Util package.
	- Some comparison-related types in the WinCopies.Collections namespace have moved to the WinCopies.Util package, and are still in the same namespace.
	- Enum throw methods are now in the WinCopies.ThrowHelper class of the WinCopies.Util package.

WinCopies.Collections 3.2.0.0-preview
-------------------------------------

- Changes:
	- Fixes #15
	- Fixes #22
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
	- Update (I)(ReadOnly)LinkedList(Node)\<T> read-only issues.
- Removals:
	- UIntCountableEnumerable<T> class.

WinCopies.Util.Desktop 3.2.0.0-preview
--------------------------------------

- Changes:
	- ValueConverters inherit from new generic abstract types.
	- IconToImageSourceConverter now takes System.Drawing.Icon values.
- Additions:
	- BitmapToImageSourceConverter
	- Bitmap-, Icon- and ImageSource-related extension methods.

12/09/2020 3.1.0.0-preview
==========================

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

11/01/2020 3.0.0-preview
========================

- Changes:
	- WinCopies.Util namespace to WinCopies (WinCopies.Util.Data namespace has not changed because of WinCopies.Data package).
	- IBackgroundWorker interface moved to WinCopies.Util.Desktop package. Some items and names have changed.
	- Some obsolete types and members have been removed.

WinCopies.Util 3.0.0-preview
----------------------------

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

WinCopies.Util.Desktop 3.0.0-preview
------------------------------------

- Additions:
	- IPausableBackgroundWorker interface.
	- PausableBackgroundWorker extension methods.
- Removals:
	- Properties:
		- WinCopies.Util.Commands.ApplicationCommands.CloseWindow

??/??/???? 2.7
==============

WinCopies.Util (2.7)
--------------------

- Supports .Net 5.0.
- Additions:
	- WinCopies.Collections.Generic.DictionaryBuilder/Merger
	- Add static methods.
- Bug fixes:
	- WinCopies.Collections.EqualityComparer\<T> now implements WinCopies.Collections.IEqualityComparer\<in T>

12/09/2020 2.6.1
================

WinCopies.Util (2.6.1)
----------------------

- Some bug fixes.
- Additions:
	- Add new (Bit)Array static and extension methods.
	- ILinkedList3
	- ISortable
	- Static methods:
		- Array moving and swapping methods.
		- Collection exception helper methods.
		- Other exception helper methods.

WinCopies.Util.Desktop (2.6.1)
------------------------------

Add CollectionViewModel\<T> as a INotifyCollectionChanged Collection\<T> wrapper.

08/14/2020 2.6
==============

- New extension and static throwing methods.

WinCopies.Util (2.6)
--------------------

- Additions:
	- WinCopies.Collections:
		- I(UInt)Countable
		- I(Disposable)EnumeratorInfo
	- WinCopies.Collections(.Generic):
		- Stack
		- Queue
	- WinCopies.Collections.DotNetFix:
		- IUIntCountableEnumerable (generic and non-generic).
	- WinCopies.Collections.DotNetFix(.Generic):
		- (I)(ReadOnly)(Enumerable)SimpleLinkedList(Node)
		- (I)(ReadOnly)(Enumerable)Stack
		- (I)(ReadOnly)(Enumerable)Queue
	- WinCopies.Collections.DotNetFix.Generic:
		- ICountable(Disposable)Enumerator
		- ArrayEnumerator
	- WinCopies.Collections.Generic:
		- Interfaces:
			- IRecursive(EnumerableProvider)Enumerable
			- I(Countable)(Disposable)EnumeratorInfo
		- RecursiveEnumerator

WinCopies.Util.Desktop (2.6)
-------------------------------

Add WinCopies.Util.DotNetFix.I(Pausable)BackgroundWorker interfaces and extension methods related to these interfaces.

07/17/2020 2.5.9
================

WinCopies.Util (2.5.9)
----------------------

- Additions:
	- Classes & Interfaces:
		- (I)EqualityComparer
		- InvalidArgumentException class
	- ForEach methods for EmptyCheckEnumerator class (generic and non-generic version)
- Bug fixes

WinCopies.Util.Desktop (2.5.9)
------------------------------

- Package supports .Net Framework v4.8.
- Additions:
	- Classes:
		- ThrowHelper
		- MultiStringConverter
		- Clipboard static class
	- (I)BackgroundWorker extension methods.

07/02/2020 2.5-preview8
=======================

WinCopies.Util (2.5-preview8)
-----------------------------

- Additions:
	- Delegates:
		- LoopIteration delegate (generic and non-generic version).
	- Extension methods:
		- ForEach generic and non-generic version.
		- Join methods for enumerables strings.
	- Types:
		- WinCopies.Util.ThrowHelper class.
		- WinCopies.Collections:
			- Classes:
				- Enumerable\<T>
				- Enumerator\<T>
				- Enumerator\<TSource, TDestination>
				- JoinEnumerator\<T>
				- EmptyCheckEnumerator\<T>
				- EmptyEnumerator\<T>
			- Interfaces:
				- IEnumerableEnumerator generic and non-generic versions.
	- WinCopies.Util.DotNetFix.PausableBackgroundWorker
- Changes:
	- WinCopies.Collections.Enumerator<TSource, TDestination>:
		- MoveNext resets Current to default when MoveNextOverride returns false.
		- Dispose methods: the '!IsDisposed' check is not performed in the Dispose() method anymore, because it has moved to the Dispose(bool) method.
	- Update SplitFactories
	- WinCopies.NullableEntries are now in the WinCopies.Collections namespace.
	- NullableEntries equality and comparison static methods are now in the new static NullableValue/RefEntryHelper classes.
- Bug fixes:
	- String 'Split' methods do not return any value when there is value but no separator in the given string.
	- Bugs fixes with the CheckedUInt64's IsNaN property and CompareTo(CheckedUInt64) method.

06/14/2020 2.5-preview7
=======================

WinCopies.Util (2.5-preview7)
-----------------------------

- Changes:
	- Some breaking changes in (ReadOnly)(Observable)Stack/Queue/LinkedCollections and relative code. Fixes #8.
- Additions:
	- Split extension methods.
	- Constructors:
		- WinCopies.Collections.DotNetFix.LinkedList\<T>:
			- LinkedList()
			- LinkedList(IEnumerable\<T> collection)
			- LinkedList(SerializationInfo info, StreamingContext context)
- Removals:
	- Unnecessary types in WinCopies.Collections.DotNetFix.Extensions.

06/02/2020 2.4-preview6.1
=========================

WinCopies.Util (2.4-preview6.1)
-------------------------------

Bug fixes and improvements for the latest features.

06/02/2020 2.4.0-preview6
=========================

WinCopies.Util (2.4.0-preview6)
-------------------------------

- Existing items behavior updates:
	- WinCopies.Linq.Extensions.Where has been renamed to WinCopies.Linq.Extensions.WherePredicate. Fixes #4.
	- The WinCopies.Util.Util.StartProcessNetCore method returns the process that is launched.

- Additions:
	- (Observable)Stack/Queue/LinkedCollection classes. Fixes #5.
	- WinCopies.Util.Util.NotApplicable const.

05/21/2020 2.3.0-preview5
=========================

WinCopies.Util (2.3.0-preview5)
-------------------------------

- Additions:
	- Interfaces:
		- ICountableEnumerable (generic and non-generic).
	- Extension methods:
		- RemoveAndGetFirstValue and RemoveAndGetLastValue for System.Collection.Generic.LinkedList and WinCopies framework's ILinkedList.
		- RemoveAll methods.
		- Char and String Repeat methods.
	- Static methods:
		- StartProcessNetCore(in string url) and StartProcessNetCore(in System.Uri url);
		- Math class:
			- IsAdditionResultInRange, TryAdd, IsMultiplicationResultInRange and TryMultiply for unsigned types.
	- Method parameters:
		- startIndex to applicable methods of ArrayBuilder.
	- Structs:
		- CheckedUInt64

- Existing items behavior updates:
	- The WinCopies.UtiL.Util.AddRangeIfNotContains(this System.Collections.ICollection collection, params object[] values) now has the following signature: AddRangeIfNotContains(this System.Collections.IList collection, params object[] values). The old method is still supported.
	- The WinCopies.Util.Util.RemoveRangeIfContains<T>(this ICollection<T> collection, params T[] values) now has the following signature: WinCopies.Util.Util.RemoveRangeIfContains<T>(this IList<T> collection, params T[] values). The old method is still supported.
	- The WinCopies.Util.Util.RemoveRangeIfContains<T>(this ICollection<T> collection, in System.Collections.Generic.IEnumerable<T> values) now has the following signature: RemoveRangeIfContains<T>(this IList<T> collection, in System.Collections.Generic.IEnumerable<T> values). The old method is still supported.
	- The WinCopies.Collections.DotNetFix.IReadOnlyLinkedList interface implements WinCopies.Collections.DotNetFix.ICountableEnumerable.

WinCopies.Util.Desktop (2.3.0-preview5)
---------------------------------------

- Additions:
	- New commands.
- Existing items behavior updates:
	- The WinCopies.Util.Commands.ApplicationCommands.CloseAllTabs has now the Shift modifier key instead of Alt.

05/04/2020 2.2.0-preview4
=========================

WinCopies.Util (2.2.0-preview4)
-------------------------------

- Additions:
	- Classes:
		- WinCopies.Linq.Extensions

- Removals:
	- WinCopies.Util.IDisposable.IsDisposed property, as WinCopies.Util.IDisposable implements WinCopies.Util.DotNetFix.IDisposable, that implements this property.

WinCopies.Util.Desktop (2.2.0-preview4)
---------------------------------------

- Additions:
	- Classes:
		- IconToImageSourceConverter
	- Extension methods:
		- ToImageSource(this Bitmap bitmap)

- Existing items behavior updates:
	- All command-related items have moved to the WinCopies.Util.Commands namespace.
	- Change the type of the WinCopies.Util.Commands.DelegateCommand.CanExecuteDelegate property for the non-generic class from System.Predicate<object> to WinCopies.Util.Predicate.
	- Add explicit constructors to the WinCopies.Util.Commands.DelegateCommand classes (generic and non-generic).

04/24/2020 2.2.0-preview3
=========================

- Add build property to build symbol packages.
- Supports .Net Framework 4.7.2, .Net Core 3.0 and .Net Standard 2.0.

WinCopies.Util (2.2.0-preview3)
-------------------------------

- Update doc.

- Existing items behavior updates:
	- The ThrowIfDisposingOrDisposed(IDisposable obj, string objectName) static method has changed to ThrowIfDisposingOrDisposed(IDisposable obj).

- Additions:
	- Classes:
		- TypeArgumentException exception.
	- Methods:
		- GetExceptionForInvalidType<T>(in Type objType, in string argumentName); static method.
		- ArrayBuilder (extension) methods and method parameters.
		- Math static methods.
		- Enum static and extension methods.
	- Constructors:
		- Default constructor for the ObjectDisposingException exception.

WinCopies.Util.Desktop (2.0.0-preview3)
---------------------------------------

- Additions:
	- APIs:
		- AttachedCommandBehavior (this API is licensed under a specific license).

03/27/2020 2.2.0-preview2
=========================

- Available for Any CPU configuration only.

03/26/2020 2.2.0-preview1
=========================

- Available for 32- and 64-bit Windows platforms.
- Some types have moved to the WinCopies.Util.Desktop package.

WinCopies.Util (2.2.0-preview1)
-------------------------------

- Existing items behavior updates:
	- The interfaces and enumerators of the WinCopies.Collections namespace for uint-indexed collections have been replaced by the new interfaces for uint-indexed collections in the WinCopies.Collections.DotNetFix namespace and are now obsolete.

- Additions:
	- Interfaces:
		- New interfaces for uint-indexed collections, partially designed like the .Net collection interfaces (WinCopies.Collections.DotNetFix namespace).
	- Extension and static methods:
		- Fix #2

02/09/2020 2.1
==============

WinCopies.Util (2.1)
--------------------

Available for .Net Framework, .Net Core and .Net Standard*

- Existing items behavior updates:
	- This assembly now targets the 4.7.2 version of the .Net Framework instead of version 4.8 for the .Net Framework version of this assembly.

- Additions:
	- Interfaces:
		- IUIntIndexedCollection and IUIntIndexedCollection<T\>
		- UIntIndexedCollectionEnumeratorBase, UIntIndexedCollectionEnumerator and UIntIndexedCollectionEnumerator<T\>
		- WinCopies.Util.DotNetFix.IDisposable

- Removals:
	- WinCopies.Util.Extensions.ToImageSource(this Bitmap bitmap); this method was only available for the 2.0.0 version for the .Net Framework and has now been removed.

\* Some features are not available in the .Net Core and .Net Standard versions since these frameworks do not have the same structure as the .Net Framework. New packages that include these features will be released later.

2.0
===

WinCopies.Util (2.0)
--------------------

Available for .Net Framework, .Net Core and .Net Standard*

- Existing items behavior updates:
	- The view models OnPropertyChanged methods do not update the properties or fields anymore ; this feature has been replaced by the 'Update' methods added in the same classes.
	- WinCopies.Util.BackgroundWorker class:
		- The BackgroundWorker class now resets its properties in background.
		- If a ThreadAbortException is thrown, and is not caught, in the background thread, the BackgroundWorker will consider that a cancellation has occurred.
		- Info can now be passed to the Cancel and CancelAsync methods.
	- The 'If' methods perform a real 'xor' comparison in binary mode and are faster.
	- The 'If' methods now set up the out 'key' parameter with the second value and predicate pair that was checked, if any, instead of the default value for the key type when performing a 'xor' comparison.
	- The ApartmentState, WorkerReportsProgress and WorkerSupportsCancellation properties of the IBackgroundWorker interface are now settable.
	- The IsNullConverter class now uses the 'is' operator instead of '=='.
	- The WinCopies.Util.Data.ValueObject now implements the WinCopies.Util.IValueObject generic interface.**
	- The WinCopies.Util.IValueObject interface implements IDisposable, so all classes that implements the WinCopies.Util.IValueObject are also disposable.
	- The following items have been moved to the WinCopies.Collections.DotNetFix namespace:
		- NotifyCollectionChangedEventArgs
		- ObservableCollection:
			- ObservableCollection classes:
				- Now call base methods for avoinding reentrancy.
				- Now have the Serializable attribute
				- Now implement the IObservableCollection interface
		- ReadOnlyObservableCollection:
			- The ReadOnlyObservableCollection classes:
				- Now have the Serializable attribute.
	- The ConverterArrayParameter and ConverterArrayMultiParametersParameter classes can now be used in XAML.
	- The IsNullConverter now supports setting the parameter of the binding to true to get a reversed boolean.

- Obsolete items:
	- Classes and interfaces:
		- WinCopies.Util.Data.IValueObjects generic and non-generic:
			- are now obsoletes and have been replaced by the WinCopies.Util.IValueObject interfaces.**;
			- now inherit from the WinCopies.Util.IValueObject interfaces.**
		- WinCopies.Util.Data.CheckableObjects generic and non-generic:
			- are now obsoletes and have been replaced by the corresponding models and view models of the new WinCopies.GUI.Models and WinCopies.GUI.ViewModels packages.
		- (I)ReadOnlyArrayList
		- The Generic class is being replaced by the Resources class and will be removed in later versions.
	- Extension methods:
		- static bool ContainsOneValue(this IEnumerable array, Comparison<object\> comparison, out bool containsMoreThanOneValue, params object[] values) IEnumerable extension method (replaced by ContainsOneValue(this IEnumerable array, WinCopies.Collections.Comparison comparison, out bool containsMoreThanOneValue, params object[] values)).
		- static object GetNumValue(this Enum @enum, string enumName) Enum extension method. Replaced by:
			- GetNumValue(this Enum @enum)
			- WinCopies.Util.GetNumValue(Type enumType, string fieldName)
		- static bool Contains(this string s, IEqualityComparer<char\> comparer, string value) (replaced by Contains(this string s, string value, IEqualityComparer<char\> comparer)).
		- static bool Contains(this string s, char value, IEqualityComparer<char\> comparer, out int index) (replaced by array-common methods).
	- Util methods:
		- static (bool propertyChanged, object oldValue) SetPropertyWhenNotBusy<T\>(T bgWorker, string propertyName, string fieldName, object newValue, Type declaringType, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, bool throwIfBusy = true) where T : IBackgroundWorker, INotifyPropertyChanged (replaced by the WinCopies.Util.Extensions.SetBackgroundWorkerProperty method overloads).
		- Static If methods with object-generic delegates have been replaced by ones with new non-generic delegates.
	- Misc:
		- The resources are now available from the new Resources static class.
		- The Microsoft.Shell namespace members are now available from the version 1.1.0 and later of the following NuGet package: https://www.nuget.org/packages/WinCopies.WindowsAPICodePack.Win32Native
		- The WinCopies.Util.Commands.ApplicationCommands.CloseWindow is now obsolete. Please use the System.Windows.Input.ApplicationCommands.Close command instead.

- Additions:
	- Classes:
		- EnumComparer
		- ArrayBuilder class to build arrays, lists and observable collections.
		- Comparer classes and interfaces for sorting support.
		- ValueObjectEqualityComparer
		- TreeNode
		- InterfaceDataTemplateSelector
		- WinCopies.Collections.DotNetFix.LinkedList
		- WinCopies.Collections.LinkedList
		- ReadOnlyLinkedList
		- EnumeratorCollection
		- MergedStylesExtension
		- Resources static class
	- Interfaces:
		- IDeepCloneable
		- IDisposable
		- IObservableCollection
		- IReadOnlyObservableCollection
		- WinCopies.Util.IValueObject
		- ITreeNode
		- IReadOnlyTreeNode
		- IObservableTreeNode
		- IReadOnlyObservableTreeNode
		- ILinkedList
	- Delegates:
		- EqualityComparison
		- FieldValidateValueCallback***
		- FieldValueChangedCallback***
		- PropertyValidateCallback***
		- PropertyValueChangedCallback***
		- ActionParams
		- Func
		- FuncParams
	- Methods:
		- Static methods:
			- 'SetField' static method
			- 'Between' static methods for the other numeric types
			- 'ThrowIfNull' static method
			- 'GetOrThrowIfNotType' static method
			- 'GetIf' methods
			- ThrowOnInvalidCopyToArrayOperation method
		- Extension methods:
			- ToStringWithoutAccents string extension method
			- Extension methods for LinkedLists
			- Extension methods for setting properties in BackgroundWorkers with an is-busy check.
			- Extension method for throwing if an object that implements IDisposable is disposing or disposed.
			- AsObjectEnumerable extension method to yield return items from an IEnumerable as items of an IEnumerable<T\>.
			- FirstOrDefault IEnumerable extension method. This is a generic method that looks for the first item of the given generic type parameter. If none item is found, the method returns the default value for the given generic type parameter.
			- LastOrDefault IEnumerable extension method. Same method as the FirstOrDefault method but to get the last item of an IEnumerable instead of the first one.
		- Misc:
			- Update methods in the view model classes to replace the update feature of the OnPropertyChanged methods of these classes. The OnPropertyChanged methods still exist in these classes, but now just raise the PropertyChanged event.
			- The view model classes now have an OnAutoPropertyChanged method to automatically set an auto-property and raise the PropertyChanged event.
			- The ReadOnlyObservableCollection has now an OnCollectionChanging protected virtual method.
	- Parameters:
		- The WinCopies.Util.Extensions.SetProperty/Field now have multiple new optional parameters to extend the capabilities of these methods.
	- Structures:
		- ValueObjectEnumerator structure
		- WrapperStructure structure

- Bug fixes:
	- BackgroundWorker class: when aborting, the RunWorkerCompleted event was raised twice.
	- BackgroundWorker class: when finalizing, an invalid operation exception was thrown if the BackgroundWorker was busy; now, the BackgroundWorker aborts the working instead of throwing an exception.
	- Is extension method: error when setting the typeEquality parameter to false.
	- String extension methods: unexpected results.

- Removals:
	- The 'performIntegrityCheck' parameter in 'SetProperty' methods has been replaced by the 'throwIfReadOnly' parameter.

- Misc:
	- ReadOnlyObservableCollection's CollectionChanging event has now the protected access modifier.
	- Some code now uses the 'in' parameter modifier.
	- The dependency package System.Windows.Interactivity.WPF has been replaced by the https://www.nuget.org/packages/Microsoft.Xaml.Behaviors.Wpf package.
	- Move resources from Generic.xaml to assembly's resource file.
	- Update doc.

\* Some features are not available in the .Net Core and .Net Standard versions since these frameworks do not have the same structure as the .Net Framework. New packages that include these features will be released later.

\*\* This also applies to the other already existing classes and interfaces, in the previous versions, that inherit from or implement these classes and interfaces.

\*\*\* See WinCopies.Util.Extensions.SetProperty/Field

WinCopies.Data (2.0)
--------------------

First release

WinCoipies.GUI (2.0)
--------------------

First release

WinCopies.GUI.Models (2.0)
--------------------------

First release

WinCopies.GUI.ViewModels (2.0)
------------------------------

First release

WinCopies.GUI.Templates (2.0)
-----------------------------

First release

WinCopies.GUI.Windows (2.0)
---------------------------

First release

Project link
------------

[https://github.com/pierresprim/WinCopies-framework](https://github.com/pierresprim/WinCopies-framework)

License
-------

See [LICENSE](https://github.com/pierresprim/WinCopies-framework/blob/master/LICENSE) for the license of the WinCopies framework.

This framework uses some external dependencies. Each external dependency is integrated to the WinCopies framework under its own license, regardless of the WinCopies framework license.
