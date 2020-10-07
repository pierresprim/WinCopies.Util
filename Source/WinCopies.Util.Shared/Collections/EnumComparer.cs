/* Copyright © Pierre Sprimont, 2019
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

#if WinCopies2

using System;
using System.Collections.Generic;
using WinCopies.Util;
using static WinCopies.Util.Util;

namespace WinCopies.Collections
{
    public class EnumComparer : System.Collections.Generic.Comparer<Enum>
    {
        public virtual int CompareToObject(Enum x, object y)
        {
            if (IsNumber(y))
            {
                object o = x.GetNumValue();

                if (o is sbyte sb) return sb.CompareTo(y);

                else if (o is byte b) return b.CompareTo(y);

                else if (o is short s) return s.CompareTo(y);

                else if (o is ushort us) return us.CompareTo(y);

                else if (o is int i) return i.CompareTo(y);

                else if (o is uint ui) return ui.CompareTo(y);

                else if (o is long l) return l.CompareTo(y);

                else if (o is ulong ul) return ul.CompareTo(y);

                else

                    // We shouldn't reach this point.

                    return 0;
            }

            else

                throw new ArgumentException("'y' is not from a numeric type.");
        }

        public virtual int CompareToEnum(object x, Enum y)
        {
            if (IsNumber(x))
            {
                object o = y.GetNumValue();

                if (o is sbyte sb) return -sb.CompareTo(x);

                else if (o is byte b) return -b.CompareTo(x);

                else if (o is short s) return -s.CompareTo(x);

                else if (o is ushort us) return -us.CompareTo(x);

                else if (o is int i) return -i.CompareTo(x);

                else if (o is uint ui) return -ui.CompareTo(x);

                else if (o is long l) return -l.CompareTo(x);

                else if (o is ulong ul) return -ul.CompareTo(x);

                else

                    // We shouldn't reach this point.

                    return 0;
            }

            else

                throw new ArgumentException("'x' is not from a numeric type.");
        }

        public override int Compare(Enum x, Enum y) => x.CompareTo(y);
    }

    public class CustomizableSortingTypeEnumComparer : EnumComparer, IComparer<Enum>
    {
        public SortingType SortingType { get; set; }

        protected virtual int CompareToObjectOverride(Enum x, object y) => base.CompareToObject(x, y);

        public sealed override int CompareToObject(Enum x, object y)
        {
            int result = CompareToObjectOverride(x, y);

            return SortingType == SortingType.Ascending ? result : -result;
        }

        protected virtual int CompareToEnumOverride(object x, Enum y) => base.CompareToEnum(x, y);

        public sealed override int CompareToEnum(object x, Enum y)
        {
            int result = CompareToEnumOverride(x, y);

            return SortingType == SortingType.Ascending ? result : -result;
        }

        protected virtual int CompareOverride(Enum x, Enum y) => base.Compare(x, y);

        public sealed override int Compare(Enum x, Enum y)
        {
            int result = CompareOverride(x, y);

            return SortingType == SortingType.Ascending ? result : -result;
        }
    }
}

#endif
