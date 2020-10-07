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

// todo:

#if STRINGCOMPARER

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.Util
{
    public class StringComparer : IComparer, IEqualityComparer, IComparer<string>, IEqualityComparer<string>
    {

        public CultureInfo CultureInfo {get;set;}

        public bool Ordinal{get;set;}

        public bool IgnoreCase {get;set;}

        public bool AccentSensitive {get;set;}

        public StringComparer(CultureInfo cultureInfo, bool ordinal, bool ignoreCase, bool accentSensitive)

        {

CultureInfo
            
        }

        public override int Compare(string x, string y)
        {
            throw new NotImplementedException();
        }

        public int Compare(object x, object y)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(string x, string y)
        {
            throw new NotImplementedException();
        }

        public new bool Equals(object x, object y)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode(string obj)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

#endif
