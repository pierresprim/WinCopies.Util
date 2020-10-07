/*
The MIT License (MIT)
Copyright (c) 2015 Clay Anderson
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System.Globalization;
using System.Runtime.Serialization;

namespace System
{
    [Serializable]
    public struct Date : IComparable, IFormattable, ISerializable, IComparable<Date>, IEquatable<Date>
    {
        private DateTime _dt;

        public static readonly Date MaxValue = new Date(DateTime.MaxValue);
        public static readonly Date MinValue = new Date(DateTime.MinValue);

        public Date(int year, int month, int day) => _dt = new DateTime(year, month, day);

        public Date(DateTime dateTime) => _dt = dateTime.AddTicks(-dateTime.Ticks % TimeSpan.TicksPerDay);

        private Date(SerializationInfo info, StreamingContext context) => _dt = DateTime.FromFileTime(info.GetInt64("ticks"));

        public static TimeSpan operator -(Date d1, Date d2) => d1._dt - d2._dt;

        public static Date operator -(Date d, TimeSpan t) => new Date(d._dt - t);

        public static bool operator !=(Date d1, Date d2) => d1._dt != d2._dt;

        public static Date operator +(Date d, TimeSpan t) => new Date(d._dt + t);

        public static bool operator <(Date d1, Date d2) => d1._dt < d2._dt;

        public static bool operator <=(Date d1, Date d2) => d1._dt <= d2._dt;

        public static bool operator ==(Date d1, Date d2) => d1._dt == d2._dt;

        public static bool operator >(Date d1, Date d2) => d1._dt > d2._dt;

        public static bool operator >=(Date d1, Date d2) => d1._dt >= d2._dt;

        public static implicit operator DateTime(Date d) => d._dt;

        public static explicit operator Date(DateTime d) => new Date(d);

        public int Day => _dt.Day;

        public DayOfWeek DayOfWeek => _dt.DayOfWeek;

        public int DayOfYear => _dt.DayOfYear;

        public int Month => _dt.Month;

        public static Date Today => new Date(DateTime.Today);

        public int Year => _dt.Year;

        public long Ticks => _dt.Ticks;

        public Date AddDays(int value) => new Date(_dt.AddDays(value));

        public Date AddMonths(int value) => new Date(_dt.AddMonths(value));

        public Date AddYears(int value) => new Date(_dt.AddYears(value));

        public static int Compare(Date d1, Date d2) => d1.CompareTo(d2);

        public int CompareTo(Date value) => _dt.CompareTo(value._dt);

        public int CompareTo(object value) => _dt.CompareTo(value);

        public static int DaysInMonth(int year, int month) => DateTime.DaysInMonth(year, month);

        public bool Equals(Date value) => _dt.Equals(value._dt);

        public override bool Equals(object value) => value is Date && _dt.Equals(((Date)value)._dt);

        public override int GetHashCode() => _dt.GetHashCode();

        public static bool Equals(Date d1, Date d2) => d1._dt.Equals(d2._dt);

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) => info.AddValue("ticks", _dt.Ticks);

        public static bool IsLeapYear(int year) => DateTime.IsLeapYear(year);

        public static Date Parse(string s) => new Date(DateTime.Parse(s));

        public static Date Parse(string s, IFormatProvider provider) => new Date(DateTime.Parse(s, provider));

        public static Date Parse(string s, IFormatProvider provider, DateTimeStyles style) => new Date(DateTime.Parse(s, provider, style));

        public static Date ParseExact(string s, string format, IFormatProvider provider) => new Date(DateTime.ParseExact(s, format, provider));

        public static Date ParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style) => new Date(DateTime.ParseExact(s, format, provider, style));

        public static Date ParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style) => new Date(DateTime.ParseExact(s, formats, provider, style));

        public TimeSpan Subtract(Date value) => this - value;

        public Date Subtract(TimeSpan value) => this - value;

        public string ToLongString => _dt.ToLongDateString();

        public string ToShortString() => _dt.ToShortDateString();

        public override string ToString() => ToShortString();

        public string ToString(IFormatProvider provider) => _dt.ToString(provider);

        public string ToString(string format) => format == "O" || format == "o" || format == "s" ? ToString("yyyy-MM-dd") : _dt.ToString(format);

        public string ToString(string format, IFormatProvider provider) => _dt.ToString(format, provider);

        public static bool TryParse(string s, out Date result)
        {
            bool success = DateTime.TryParse(s, out DateTime d);
            result = new Date(d);
            return success;
        }

        public static bool TryParse(string s, IFormatProvider provider, DateTimeStyles style, out Date result)
        {
            bool success = DateTime.TryParse(s, provider, style, out DateTime d);
            result = new Date(d);
            return success;
        }

        public static bool TryParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style, out Date result)
        {
            bool success = DateTime.TryParseExact(s, format, provider, style, out DateTime d);
            result = new Date(d);
            return success;
        }

        public static bool TryParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style, out Date result)
        {
            bool success = DateTime.TryParseExact(s, formats, provider, style, out DateTime d);
            result = new Date(d);
            return success;
        }
    }

    public static class DateTimeExtensions
    {
        public static Date ToDate(this DateTime dt) => new Date(dt);
    }
}
