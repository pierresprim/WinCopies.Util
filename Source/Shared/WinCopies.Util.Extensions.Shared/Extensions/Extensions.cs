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

#if CS7
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using WinCopies.Collections;
using WinCopies.Collections.Generic;
using WinCopies.Linq;
using WinCopies.Util;

using IfCT = WinCopies.Diagnostics.ComparisonType;

using static WinCopies.ThrowHelper;

namespace WinCopies.Extensions // To avoid name conflicts.
{
    public class EmbeddedResourcesDictionary : IReadOnlyDictionary<string, System.IO.Stream>
#if CS8
        , Collections.DotNetFix.Generic.IEnumerable<KeyValuePair<string, System.IO.Stream>>
#endif
    {
        protected Assembly Assembly { get; }

        public System.IO.Stream this[string key] => TryGetValue(key, out System.IO.Stream stream) ? stream : throw new KeyNotFoundException();

        public System.Collections.Generic.IReadOnlyList<string> Keys { get; }

        public System.Collections.Generic.IEnumerable<System.IO.Stream> Values => this.Select(item => item.Value);

        public int Count => Keys.Count;

        System.Collections.Generic.IEnumerable<string> IReadOnlyDictionary<string, System.IO.Stream>.Keys => Keys;

        public EmbeddedResourcesDictionary() => Keys = new ReadOnlyArray<string>(Assembly.GetManifestResourceNames());

        public bool ContainsKey(string key) => TryGetValue(key, out _);
        public IEnumerator<KeyValuePair<string, System.IO.Stream>> GetEnumerator() => Assembly.EnumerateEmbeddedResources().GetEnumerator();
        public System.IO.Stream
#if CS8
            ?
#endif
            TryGetValue(string key) => Assembly.GetManifestResourceStream(key);
        public bool TryGetValue(string key, out System.IO.Stream value) => (value = TryGetValue(key)) != null;
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public static class Extensions
    {
        public static IReadOnlyList<Type> GetRealGenericParameters(this Type type)
        {
            Type[] array = type.GetGenericArguments();

            int length = type.GetRealGenericTypeParameterLength();

            return new SubReadOnlyList<Type>(array, length == 0 ? 0 : array.Length - length);
        }

        public static bool HasFlag(this Enum @enum, IEnumerable<Enum> values)
        {
            foreach (Enum value in values)

                if (@enum.HasFlag(value))

                    return true;

            return false;
        }

        public static bool HasFlag(this Enum @enum, params Enum[] values) => @enum.HasFlag((IEnumerable<Enum>)values);

        public static bool HasAllFlags(this Enum @enum, IEnumerable<Enum> values)
        {
            foreach (Enum value in values)

                if (!@enum.HasFlag(value))

                    return false;

            return true;
        }

        public static bool HasAllFlags(this Enum @enum, params Enum[] values) => @enum.HasAllFlags((IEnumerable<Enum>)values);

        public static bool IsValidFlagsEnumValue<T>(this T value, in IfCT comparisonType, in string argumentName, params T[] values) where T : Enum
        {
            ThrowIfNull(values, nameof(values));

            ThrowIfNotFlagsEnumType<T>(nameof(T));

            ThrowIfNotValidEnumValue(nameof(comparisonType), comparisonType);

            switch (comparisonType)
            {
                case IfCT.And:

                    foreach (T _value in values)

                        if (!value.HasFlag(_value))

                            return false;

                    return true;

                case IfCT.Or:

                    foreach (T _value in values)

                        if (value.HasFlag(_value))

                            return true;

                    return false;

                case IfCT.Xor:

                    bool oneResultFound = false;

                    foreach (T _value in values)

                        if (value.HasFlag(_value))

                            if (oneResultFound)

                                return false;

                            else

                                oneResultFound = true;

                    return oneResultFound;

                default:

                    Debug.Assert(false);

                    return false;
            }
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type, in Predicate<Type>
#if CS8
            ?
#endif
            predicate = null) => WinCopies.UtilHelpers.EnumerateUntilNull(type, Delegates.GetBaseTypeConverter(), predicate);

        public static IEnumerable<Type> GetParentTypes(this Type type, in Predicate<Type>
#if CS8
            ?
#endif
            predicate = null) => WinCopies.UtilHelpers.EnumerateUntilNull(type, Delegates.GetParentTypeConverter(), predicate);

        public static IEnumerable<Type> GetDirectInterfaces(this Type t, bool ignoreGenerics, bool? directTypeOnly, bool ignoreFirstTypesWithoutInterfaces = true, bool doNotExclude = false, Predicate<Type>
#if CS8
            ?
#endif
            predicate = null)
        {
            EnumerableHelper<Type>.IEnumerableLinkedList interfaces = EnumerableHelper<Type>.GetEnumerableLinkedList();
            EnumerableHelper<Type>.IEnumerableLinkedList subInterfaces = EnumerableHelper<Type>.GetEnumerableLinkedList();

            void __addInterface(in Type i, in EnumerableHelper<Type>.IEnumerableLinkedList collection)
            {
                if (collection.None(i))

                    collection.AddLast(i);
            }
            void _addInterface(in Type i) => __addInterface(i, interfaces);
            void _addSubInterface(in Type i) => __addInterface(i, subInterfaces);

            void addInterface(in Type _t, params ActionIn<Type>[] actions)
            {
                ActionIn<Type> action = actions.Length > 1 ? (in Type i) =>
                {
                    foreach (ActionIn<Type> _action in actions)

                        _action(i);
                }
                : actions[0];

                foreach (Type i in _t.GetInterfaces())

                    action(i);
            }

            ActionIn<Type> getNonGenericAssertedAction(params ActionIn<Type>[] actions) => (in Type i) =>
            {
                if (!i.IsGenericType)

                    foreach (ActionIn<Type> action in actions)

                        action(i);
            };

            void addNonGenericSubInterfaces(in Type _t) => addInterface(_t, getNonGenericAssertedAction(_addSubInterface));
            void addNonGenericInterfaces() => addInterface(t, getNonGenericAssertedAction(_addInterface, addNonGenericSubInterfaces));

            void addSubInterfaces(in Type _t) => addInterface(_t, _addSubInterface);
            void addInterfaces() => addInterface(t, _addInterface, addSubInterfaces);

            bool _predicate(Type _t) => _t.GetDirectInterfaces(ignoreGenerics, true, false).GetEnumerator().MoveNext();

            Predicate<Type> __predicate = predicate == null ?
#if !CS9
                (Predicate<Type>)
#endif
                _predicate : (_t => predicate(_t) && _predicate(_t));

            void add(in Action _addInterfaces, in ActionIn<Type> _addSubInterfaces)
            {
                _addInterfaces();

                if (directTypeOnly.HasValue)

                    if (directTypeOnly.Value)
                    {
                        Type
#if CS8
                            ?
#endif
                            _t;

                        if (ignoreFirstTypesWithoutInterfaces)
                        {
                            if (t.GetBaseTypes().FirstOrDefaultPredicate<Type>(__predicate, out _t))

                                _t = _t.BaseType;
                        }

                        else

                            _t = t.BaseType;

                        if (_t != null)

                            foreach (Type __t in _t.GetBaseTypes())

                                _addSubInterfaces(__t);
                    }

                    else if (doNotExclude)

                        foreach (Type _t in t.GetBaseTypes().SelectMany(__t => __t.GetDirectInterfaces(ignoreGenerics, null, ignoreFirstTypesWithoutInterfaces, false, predicate)))

                            if (WinCopies.UtilHelpers.EnumerateUntilNull(subInterfaces.FirstNode, node => node.Next).FirstOrDefault(node => node.Value == _t, out EnumerableHelper<Type>.ILinkedListNode _node))

                                _node.Remove();
            }

            if (ignoreGenerics)

                add(addNonGenericInterfaces, addNonGenericSubInterfaces);

            else

                add(addInterfaces, addSubInterfaces);

            return interfaces.AsFromType<IEnumerable<Type>>().Except(subInterfaces);
        }
    }
}
#endif
