using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace WinCopies
{
    public static class Math
    {

        public static ulong Pow(in byte b, in byte power)

        {

            if (power == 0) return 1;

            if (power == 1) return b;

            ulong result = (ulong)(b * b);

            for (byte i = 3; i <= power; i++)

                result *= b;

            return result;

        }

        public static long Pow(in sbyte b, in sbyte power)

        {

            if (power == 0) return 1;

            if (power == 1) return b;

            if (power == -1)

                return (long)(1 / b);

            long result;

            if (power < 0)

            {

                if (power < -1)

                {

                    result = (long)(b * b);

                    for (sbyte i = -3; i >= power; i--)

                        result *= b;

                    result = 1 / result;

                    return result;

                }

            }

            result = b * b;

            for (sbyte i = 3; i <= power; i++)

                result *= b;

            return result;

        }

        public static ulong Pow(in ushort s, in ushort power)

        {

            if (power == 0) return 1;

            if (power == 1) return s;

            ulong result = (ulong)(s * s);

            for (ushort i = 3; i <= power; i++)

                result *= s;

            return result;

        }

        public static long Pow(in short s, in short power)

        {

            if (power == 0) return 1;

            if (power == 1) return s;

            if (power == -1)

                return (long)(1 / s);

            long result;

            if (power < 0)

            {

                if (power < -1)

                {

                    result = (long)(s * s);

                    for (short i = -3; i >= power; i--)

                        result *= s;

                    result = 1 / result;

                    return result;

                }

            }

            result = s * s;

            for (short i = 3; i <= power; i++)

                result *= s;

            return result;

        }

        public static ulong Pow(in uint i, in uint power)

        {

            if (power == 0u) return 1u;

            if (power == 1u) return i;

            ulong result = (ulong)(i * i);

            for (uint _i = 3u; _i <= power; _i++)

                result *= i;

            return result;

        }

        public static long Pow(in int i, in int power)

        {

            if (power == 0) return 1L;

            if (power == 1) return i;

            if (power == -1)

                return (long)(1 / i);

            long result;

            if (power < 0)

            {

                if (power < -1)

                {

                    result = (long)(i * i);

                    for (int _i = -3; _i >= power; _i--)

                        result *= i;

                    result = 1 / result;

                    return result;

                }

            }

            result = i * i;

            for (int _i = 3; _i <= power; _i++)

                result *= i;

            return result;

        }

        public static ulong Pow(in ulong value, in ulong power)

        {

            if (power == 0) return 1UL;

            if (power == 1) return value;

            ulong result = value * value;

            for (ulong i = 3; i <= power; i++)

                result *= value;

            return result;

        }

        public static long Pow(in long value, in long power)

        {

            if (power == 0) return 1L;

            if (power == 1) return value;

            if (power == -1)

                return 1 / value;

            long result;

            if (power < 0)

            {

                if (power < -1)

                {

                    result = value * value;

                    for (long i = -3; i >= power; i--)

                        result *= value;

                    result = 1 / result;

                    return result; 

                }

            }

            result = value * value;

            for (long i = 3; i <= power; i++)

                result *= value;

            return result * value;

        }
    }
}
