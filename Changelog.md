WinCopies-framework
===================

The WinCopies® software framework

CHANGELOG
=========

Updates
-------

??/??/???? 2.2.0-preview5
=========================

WinCopies.Util (2.2.0-preview5)
-------------------------------

- Additions:
	- Interfaces:
		- ICountableEnumerable (generic and non-generic).

WinCopies.Util.Desktop (2.2.0-preview5)
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
