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
using System.Diagnostics;
using System.Linq;

using WinCopies.Collections;

using IfCT = WinCopies.Diagnostics.ComparisonType;

#if WinCopies3
using WinCopies.Collections.Generic;

using static WinCopies.ThrowHelper;
#else
using WinCopies.Util;

using static WinCopies.Util.Util;

using InvalidEnumArgumentException = WinCopies.Util.InvalidEnumArgumentException;
#endif

namespace WinCopies.Extensions // To avoid name conflicts.
{
    public static class Extensions
    {
#if !WinCopies3
        internal static void ThrowIfNotValidEnumValue(in string argumentName, in Enum @enum)
        {
            if (!@enum.IsValidEnumValue()) throw new InvalidEnumArgumentException(argumentName, @enum);

            // .GetType().IsEnumDefined(@enum)
        }
#endif

        public static bool HasFlag(this Enum @enum, System.Collections.Generic.IEnumerable<Enum> values)
        {
            foreach (Enum value in values)

                if (@enum.HasFlag(value))

                    return true;

            return false;
        }

        public static bool HasFlag(this Enum @enum, params Enum[] values) => @enum.HasFlag((System.Collections.Generic.IEnumerable<Enum>)values);

        public static bool HasAllFlags(this Enum @enum, System.Collections.Generic.IEnumerable<Enum> values)
        {
            foreach (Enum value in values)

                if (!@enum.HasFlag(value))

                    return false;

            return true;
        }

        public static bool HasAllFlags(this Enum @enum, params Enum[] values) => @enum.HasAllFlags((System.Collections.Generic.IEnumerable<Enum>)values);

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

        public static System.Collections.Generic.IEnumerable<Type> GetDirectInterfaces(this Type t, bool ignoreGenerics, bool directTypeOnly, bool ignoreFirstTypesWithoutInterfaces = true, Predicate<Type> predicate = null)
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

            Predicate<Type> __predicate;

            if (predicate == null)

                __predicate = _predicate;

            else

                __predicate = _t => predicate(_t) && _predicate(_t);

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

            return ((System.Collections.Generic.IEnumerable<Type>)interfaces).Except(subInterfaces);
        }
    }
}

#endif
