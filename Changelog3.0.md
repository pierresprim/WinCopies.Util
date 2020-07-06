WinCopies-framework
===================

The WinCopies® software framework

CHANGELOG
=========

Updates
-------

??/??/???? 3.0.0
================

- Changes:
	- WinCopies.Util namespace to WinCopies (WinCopies.Util.Data namespace has not changed because of WinCopies.Data package.
	- IBackgroundWorker interface moved to WinCopies.Util.Desktop package. Some items and names have changed.
	- Some obsolete types have been removed.

WinCopies.Util 3.0.0
--------------------

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
	- Classes:
		- WinCopies.Util.Util => WinCopies.UtilHelpers
	- Enums:
		- WinCopies.Util.Util.ComparisonType => WinCopies.Diagnostics.ComparisonType
		- WinCopies.Util.Util.ComparisonMode => WinCopies.Diagnostics.ComparisonMode
		- WinCopies.Util.Util.Comparison => WinCopies.Diagnostics.Comparison
	- Static and extension methods:
		- 'If' methods are now in the WinCopies.Diagnostics.IfHelpers namespace.
		- Get- and ThrowException methods are now all in the WinCopies.ThrowHelper class.
	- Misc:
		- StringParameterEmptyOrWhiteSpaces resource to StringParameterEmptyOrWhiteSpace

WinCopies.Util.Desktop 3.0.0
----------------------------

- Additions:
	- IPausableBackgroundWorker interface.
	- PausableBackgroundWorker extension methods.
- Removals:
	- Properties:
		- WinCopies.Util.Commands.ApplicationCommands.CloseWindow

Project link
------------

[https://github.com/pierresprim/WinCopies-framework](https://github.com/pierresprim/WinCopies-framework)

License
-------

See [LICENSE](https://github.com/pierresprim/WinCopies-framework/blob/master/LICENSE) for the license of the WinCopies framework.

This framework uses some external dependencies. Each external dependency is integrated to the WinCopies framework under its own license, regardless of the WinCopies framework license.
