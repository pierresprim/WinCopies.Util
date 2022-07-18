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
using System.Linq;
using System.Reflection;

using WinCopies.Collections;
using WinCopies.Util;

using IfCT = WinCopies.Diagnostics.ComparisonType;

#if WinCopies3
using WinCopies.Collections.Generic;

using static WinCopies.ThrowHelper;
#else
using static WinCopies.Util.Util;

using InvalidEnumArgumentException = WinCopies.Util.InvalidEnumArgumentException;
#endif

namespace WinCopies.Extensions // To avoid name conflicts.
{
    public class EmbeddedResourcesDictionary : IReadOnlyDictionary<string, System.IO.Stream>
#if WinCopies3 && CS8
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
#if WinCopies3
        public static IReadOnlyList<Type> GetRealGenericParameters(this Type type)
        {
            Type[] array = type.GetGenericArguments();

            int length = type.GetRealGenericTypeParameterLength();

            return new SubReadOnlyList<Type>(array, length == 0 ? 0 : array.Length - length);
        }
#else
        internal static void ThrowIfNotValidEnumValue(in string argumentName, in Enum @enum)
        {
            if (!@enum.IsValidEnumValue()) throw new InvalidEnumArgumentException(argumentName, @enum);

            // .GetType().IsEnumDefined(@enum)
        }
#endif

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

        public static IEnumerable<Type> GetDirectInterfaces(this Type t, bool ignoreGenerics, bool directTypeOnly, bool ignoreFirstTypesWithoutInterfaces = true, Predicate<Type>
#if CS8
            ?
#endif
            predicate = null)
        {
            var interfaces = new ArrayBuilder<Type>();
            var subInterfaces = new ArrayBuilder<Type>();

            void _addInterface(in Type i) => _ = interfaces.AddLast(i);

            void _addSubInterface(in Type i) => _ = subInterfaces.AddLast(i);

            void addNonGenericSubInterfaces(Type _t)
            {
                foreach (Type i in _t.GetInterfaces())

                    if (!i.IsGenericType)

                        _addSubInterface(i);
            }

            void addNonGenericInterfaces()
            {
                foreach (Type i in t.GetInterfaces())

                    if (!i.IsGenericType)
                    {
                        _addInterface(i);

                        addNonGenericSubInterfaces(i);
                    }
            }

            void addSubInterfaces(Type _t)
            {
                foreach (Type i in _t.GetInterfaces())

                    _addSubInterface(i);
            }

            void addInterfaces()
            {
                foreach (Type i in t.GetInterfaces())
                {
                    _addInterface(i);

                    addSubInterfaces(i);
                }
            }

            bool _predicate(Type _t) => _t.GetDirectInterfaces(ignoreGenerics, true, false).GetEnumerator().MoveNext();

            Predicate<Type> __predicate = predicate == null ?
#if !CS9
                (Predicate<Type>)
#endif
                _predicate : (_t => predicate(_t) && _predicate(_t));

            void addBaseTypesInterfaces(Action<Type> _action)
            {
                Type _t;

                if (ignoreFirstTypesWithoutInterfaces)
                {
                    _t = t;

                    do
                    {
                        if (__predicate(_t))
                        {
                            _t = _t.BaseType;

                            break;
                        }
                    }
                    while ((_t = _t.BaseType) != null);
                }

                else

                    _t = t.BaseType;

                while (_t != null)
                {
                    _action(_t);

                    _t = _t.BaseType;
                }
            }

            if (ignoreGenerics)
            {
                addNonGenericInterfaces();

                if (directTypeOnly)

                    addBaseTypesInterfaces(addNonGenericSubInterfaces);
            }

            else
            {
                addInterfaces();

                if (directTypeOnly)

                    addBaseTypesInterfaces(addSubInterfaces);
            }

            return ((IEnumerable<Type>)interfaces).Except(subInterfaces);
        }
    }
}
#endif
