using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace WinCopies.Tests.Util.Util
{
    [Flags]
    public enum NumericTypes
    {
        Byte = 1,
        UShort = 2,
        UInt = 4,
        ULong = 8
    }

    [TestClass]
    public class Math
    {
        private void AssertAdditionResultInRange(ulong left, ulong right, ulong maxValue, bool shouldSucceed, ulong expectedResult, NumericTypes numericTypes)
        {
            bool result;

            if (numericTypes.HasFlag(NumericTypes.ULong))
            {
                result = WinCopies.Math.IsAdditionResultInRange(left, right, maxValue);
                ulong? resultULong = WinCopies.Math.TryAdd(left, right, maxValue);

                if (shouldSucceed)
                {
                    Assert.IsTrue(result);

                    Assert.IsTrue(resultULong.HasValue);
                    Assert.AreEqual(expectedResult, resultULong.Value);
                }
                else
                {
                    Assert.IsFalse(result);

                    Assert.IsFalse(resultULong.HasValue);
                }
            }

            if (numericTypes.HasFlag(NumericTypes.UInt))
            {
                var _left = (uint)left;
                var _right = (uint)right;
                var _maxValue = (uint)maxValue;

                result = WinCopies.Math.IsAdditionResultInRange(_left, _right, _maxValue);
                uint? resultUInt = WinCopies.Math.TryAdd(_left, _right, _maxValue);

                if (shouldSucceed)
                {
                    Assert.IsTrue(result);

                    Assert.IsTrue(resultUInt.HasValue);
                    Assert.AreEqual((uint)expectedResult, resultUInt.Value);
                }
                else
                {
                    Assert.IsFalse(result);

                    Assert.IsFalse(resultUInt.HasValue);
                }
            }

            if (numericTypes.HasFlag(NumericTypes.UShort))
            {
                var _left = (ushort)left;
                var _right = (ushort)right;
                var _maxValue = (ushort)maxValue;

                result = WinCopies.Math.IsAdditionResultInRange(_left, _right, _maxValue);
                ushort? resultUShort = WinCopies.Math.TryAdd(_left, _right, _maxValue);

                if (shouldSucceed)
                {
                    Assert.IsTrue(result);

                    Assert.IsTrue(resultUShort.HasValue);
                    Assert.AreEqual((ushort)expectedResult, resultUShort.Value);
                }
                else
                {
                    Assert.IsFalse(result);

                    Assert.IsFalse(resultUShort.HasValue);
                }
            }

            if (numericTypes.HasFlag(NumericTypes.Byte))
            {
                var _left = (byte)left;
                var _right = (byte)right;
                var _maxValue = (byte)maxValue;

                result = WinCopies.Math.IsAdditionResultInRange(_left, _right, _maxValue);
                byte? resultByte = WinCopies.Math.TryAdd(_left, _right, _maxValue);

                if (shouldSucceed)
                {
                    Assert.IsTrue(result);

                    Assert.IsTrue(resultByte.HasValue);
                    Assert.AreEqual((byte)expectedResult, resultByte.Value);
                }
                else
                {
                    Assert.IsFalse(result);

                    Assert.IsFalse(resultByte.HasValue);
                }
            }
        }

        [TestMethod]
        public void AdditionResultInRange()
        {
            var numericTypes = NumericTypes.Byte | NumericTypes.UShort | NumericTypes.UInt | NumericTypes.ULong;

            AssertAdditionResultInRange(0UL, 0UL, 0UL, true, 0UL, numericTypes);

            AssertAdditionResultInRange(0UL, 10UL, 1UL, false, 10UL, numericTypes);
            AssertAdditionResultInRange(10UL, 0UL, 1UL, false, 10UL, numericTypes);

            AssertAdditionResultInRange(0UL, 10UL, 10UL, true, 10UL, numericTypes);
            AssertAdditionResultInRange(10UL, 0UL, 10UL, true, 10UL, numericTypes);

            AssertAdditionResultInRange(0UL, 10UL, 100UL, true, 10UL, numericTypes);
            AssertAdditionResultInRange(10UL, 0UL, 100UL, true, 10UL, numericTypes);



            AssertAdditionResultInRange(30UL, 30UL, 10UL, false, 0UL, numericTypes);
            AssertAdditionResultInRange(10UL, 10UL, 20UL, true, 20UL, numericTypes);
            AssertAdditionResultInRange(10UL, 10UL, 30UL, true, 20UL, numericTypes);

            AssertAdditionResultInRange(20UL, 30UL, 10UL, false, 0UL, numericTypes);
            AssertAdditionResultInRange(30UL, 20UL, 10UL, false, 0UL, numericTypes);

            AssertAdditionResultInRange(10UL, 20UL, 30UL, true, 30UL, numericTypes);
            AssertAdditionResultInRange(20UL, 10UL, 30UL, true, 30UL, numericTypes);

            AssertAdditionResultInRange(10UL, 20UL, 100UL, true, 30UL, numericTypes);
            AssertAdditionResultInRange(20UL, 10UL, 100UL, true, 30UL, numericTypes);



            AssertAdditionResultInRange(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, false, 0UL, NumericTypes.ULong);

            AssertAdditionResultInRange(1UL, ulong.MaxValue - 1, ulong.MaxValue - 1, false, 0UL, NumericTypes.ULong);
            AssertAdditionResultInRange(ulong.MaxValue - 1, 1UL, ulong.MaxValue - 1, false, 0UL, NumericTypes.ULong);

            AssertAdditionResultInRange(1UL, ulong.MaxValue - 1, ulong.MaxValue, true, ulong.MaxValue, NumericTypes.ULong);
            AssertAdditionResultInRange(ulong.MaxValue - 1, 1UL, ulong.MaxValue, true, ulong.MaxValue, NumericTypes.ULong);

            AssertAdditionResultInRange(10UL, 20UL, ulong.MaxValue, true, 30UL, NumericTypes.ULong);
            AssertAdditionResultInRange(20UL, 10UL, ulong.MaxValue, true, 30UL, NumericTypes.ULong);



            AssertAdditionResultInRange(uint.MaxValue, uint.MaxValue, uint.MaxValue, false, 0UL, NumericTypes.UInt);

            AssertAdditionResultInRange(1UL, uint.MaxValue - 1, uint.MaxValue - 1, false, 0UL, NumericTypes.UInt);
            AssertAdditionResultInRange(uint.MaxValue - 1, 1UL, uint.MaxValue - 1, false, 0UL, NumericTypes.UInt);

            AssertAdditionResultInRange(1UL, uint.MaxValue - 1, uint.MaxValue, true, uint.MaxValue, NumericTypes.UInt);
            AssertAdditionResultInRange(uint.MaxValue - 1, 1UL, uint.MaxValue, true, uint.MaxValue, NumericTypes.UInt);

            AssertAdditionResultInRange(10UL, 20UL, uint.MaxValue, true, 30UL, NumericTypes.UInt);
            AssertAdditionResultInRange(20UL, 10UL, uint.MaxValue, true, 30UL, NumericTypes.UInt);



            AssertAdditionResultInRange(ushort.MaxValue, ushort.MaxValue, ushort.MaxValue, false, 0UL, NumericTypes.UShort);

            AssertAdditionResultInRange(1UL, ushort.MaxValue - 1, ushort.MaxValue - 1, false, 0UL, NumericTypes.UShort);
            AssertAdditionResultInRange(ushort.MaxValue - 1, 1UL, ushort.MaxValue - 1, false, 0UL, NumericTypes.UShort);

            AssertAdditionResultInRange(1UL, ushort.MaxValue - 1, ushort.MaxValue, true, ushort.MaxValue, NumericTypes.UShort);
            AssertAdditionResultInRange(ushort.MaxValue - 1, 1UL, ushort.MaxValue, true, ushort.MaxValue, NumericTypes.UShort);

            AssertAdditionResultInRange(10UL, 20UL, ushort.MaxValue, true, 30UL, NumericTypes.UShort);
            AssertAdditionResultInRange(20UL, 10UL, ushort.MaxValue, true, 30UL, NumericTypes.UShort);



            AssertAdditionResultInRange(byte.MaxValue, byte.MaxValue, byte.MaxValue, false, 0UL, NumericTypes.Byte);

            AssertAdditionResultInRange(1UL, byte.MaxValue - 1, byte.MaxValue - 1, false, 0UL, NumericTypes.Byte);
            AssertAdditionResultInRange(byte.MaxValue - 1, 1UL, byte.MaxValue - 1, false, 0UL, NumericTypes.Byte);

            AssertAdditionResultInRange(1UL, byte.MaxValue - 1, byte.MaxValue, true, byte.MaxValue, NumericTypes.Byte);
            AssertAdditionResultInRange(byte.MaxValue - 1, 1UL, byte.MaxValue, true, byte.MaxValue, NumericTypes.Byte);

            AssertAdditionResultInRange(10UL, 20UL, byte.MaxValue, true, 30UL, NumericTypes.Byte);
            AssertAdditionResultInRange(20UL, 10UL, byte.MaxValue, true, 30UL, NumericTypes.Byte);
        }

        private void AssertMultiplicationResultInRange(ulong left, ulong right, ulong maxValue, bool shouldSucceed, ulong expectedResult, NumericTypes numericTypes)
        {
            bool result;

            if (numericTypes.HasFlag(NumericTypes.ULong))
            {
                result = WinCopies.Math.IsMultiplicationResultInRange(left, right, maxValue);
                ulong? resultULong = WinCopies.Math.TryMultiply(left, right, maxValue);

                if (shouldSucceed)
                {
                    Assert.IsTrue(result);

                    Assert.IsTrue(resultULong.HasValue);
                    Assert.AreEqual(expectedResult, resultULong.Value);
                }
                else
                {
                    Assert.IsFalse(result);

                    Assert.IsFalse(resultULong.HasValue);
                }
            }

            if (numericTypes.HasFlag(NumericTypes.UInt))
            {
                var _left = (uint)left;
                var _right = (uint)right;
                var _maxValue = (uint)maxValue;

                result = WinCopies.Math.IsMultiplicationResultInRange(_left, _right, _maxValue);
                uint? resultUInt = WinCopies.Math.TryMultiply(_left, _right, _maxValue);

                if (shouldSucceed)
                {
                    Assert.IsTrue(result);

                    Assert.IsTrue(resultUInt.HasValue);
                    Assert.AreEqual((uint)expectedResult, resultUInt.Value);
                }
                else
                {
                    Assert.IsFalse(result);

                    Assert.IsFalse(resultUInt.HasValue);
                }
            }

            if (numericTypes.HasFlag(NumericTypes.UShort))
            {
                var _left = (ushort)left;
                var _right = (ushort)right;
                var _maxValue = (ushort)maxValue;

                result = WinCopies.Math.IsMultiplicationResultInRange(_left, _right, _maxValue);
                ushort? resultUShort = WinCopies.Math.TryMultiply(_left, _right, _maxValue);

                if (shouldSucceed)
                {
                    Assert.IsTrue(result);

                    Assert.IsTrue(resultUShort.HasValue);
                    Assert.AreEqual((ushort)expectedResult, resultUShort.Value);
                }
                else
                {
                    Assert.IsFalse(result);

                    Assert.IsFalse(resultUShort.HasValue);
                }
            }

            if (numericTypes.HasFlag(NumericTypes.Byte))
            {
                var _left = (byte)left;
                var _right = (byte)right;
                var _maxValue = (byte)maxValue;

                result = WinCopies.Math.IsMultiplicationResultInRange(_left, _right, _maxValue);
                byte? resultByte = WinCopies.Math.TryMultiply(_left, _right, _maxValue);

                if (shouldSucceed)
                {
                    Assert.IsTrue(result);

                    Assert.IsTrue(resultByte.HasValue);
                    Assert.AreEqual((byte)expectedResult, resultByte.Value);
                }
                else
                {
                    Assert.IsFalse(result);

                    Assert.IsFalse(resultByte.HasValue);
                }
            }
        }

        [TestMethod]
        public void MultiplyResultInRange()
        {
            var numericTypes = NumericTypes.Byte | NumericTypes.UShort | NumericTypes.UInt | NumericTypes.ULong;

            AssertMultiplicationResultInRange(0UL, 0UL, 0UL, true, 0UL, numericTypes);

            AssertMultiplicationResultInRange(0UL, 10UL, 0UL, true, 0UL, numericTypes);
            AssertMultiplicationResultInRange(10UL, 0UL, 0UL, true, 0UL, numericTypes);

            AssertMultiplicationResultInRange(0UL, 10UL, 10UL, true, 0UL, numericTypes);
            AssertMultiplicationResultInRange(10UL, 0UL, 10UL, true, 0UL, numericTypes);



            AssertMultiplicationResultInRange(10UL, 10UL, 10UL, false, 0UL, numericTypes);
            AssertMultiplicationResultInRange(10UL, 10UL, 100UL, true, 100UL, numericTypes);
            AssertMultiplicationResultInRange(10UL, 10UL, 110UL, true, 100UL, numericTypes);

            AssertMultiplicationResultInRange(2UL, 100UL, 10UL, false, 0UL, numericTypes);
            AssertMultiplicationResultInRange(100UL, 2UL, 10UL, false, 0UL, numericTypes);

            AssertMultiplicationResultInRange(2UL, 100UL, 200UL, true, 200UL, numericTypes);
            AssertMultiplicationResultInRange(100UL, 2UL, 200UL, true, 200UL, numericTypes);

            AssertMultiplicationResultInRange(2UL, 100UL, 250UL, true, 200UL, numericTypes);
            AssertMultiplicationResultInRange(100UL, 2UL, 250UL, true, 200UL, numericTypes);



            AssertMultiplicationResultInRange(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, false, 0UL, NumericTypes.ULong);

            AssertMultiplicationResultInRange(1UL, ulong.MaxValue, ulong.MaxValue, true, ulong.MaxValue, NumericTypes.ULong);
            AssertMultiplicationResultInRange(ulong.MaxValue, 1UL, ulong.MaxValue, true, ulong.MaxValue, NumericTypes.ULong);

            AssertMultiplicationResultInRange(2UL, ulong.MaxValue / 2, ulong.MaxValue / 2, false, 0UL, NumericTypes.ULong);
            AssertMultiplicationResultInRange(ulong.MaxValue / 2, 2UL, ulong.MaxValue / 2, false, 0UL, NumericTypes.ULong);

            Assert.AreEqual(18446744073709551614, 2 * (ulong.MaxValue / 2));

            AssertMultiplicationResultInRange(2UL, ulong.MaxValue / 2, ulong.MaxValue - 1, true, ulong.MaxValue - 1, NumericTypes.ULong);
            AssertMultiplicationResultInRange(ulong.MaxValue / 2, 2UL, ulong.MaxValue - 1, true, ulong.MaxValue - 1, NumericTypes.ULong);

            AssertMultiplicationResultInRange(10UL, 20UL, ulong.MaxValue, true, 200UL, NumericTypes.ULong);
            AssertMultiplicationResultInRange(20UL, 10UL, ulong.MaxValue, true, 200UL, NumericTypes.ULong);



            AssertMultiplicationResultInRange(uint.MaxValue, uint.MaxValue, uint.MaxValue, false, 0UL, NumericTypes.UInt);

            AssertMultiplicationResultInRange(1UL, uint.MaxValue, uint.MaxValue, true, uint.MaxValue, NumericTypes.UInt);
            AssertMultiplicationResultInRange(uint.MaxValue, 1UL, uint.MaxValue, true, uint.MaxValue, NumericTypes.UInt);

            AssertMultiplicationResultInRange(2UL, uint.MaxValue / 2, uint.MaxValue / 2, false, 0UL, NumericTypes.UInt);
            AssertMultiplicationResultInRange(uint.MaxValue / 2, 2UL, uint.MaxValue / 2, false, 0UL, NumericTypes.UInt);

            AssertMultiplicationResultInRange(2UL, uint.MaxValue / 2, uint.MaxValue - 1, true, uint.MaxValue - 1, NumericTypes.UInt);
            AssertMultiplicationResultInRange(uint.MaxValue / 2, 2UL, uint.MaxValue - 1, true, uint.MaxValue - 1, NumericTypes.UInt);

            AssertMultiplicationResultInRange(10UL, 20UL, uint.MaxValue, true, 200UL, NumericTypes.UInt);
            AssertMultiplicationResultInRange(20UL, 10UL, uint.MaxValue, true, 200UL, NumericTypes.UInt);



            AssertMultiplicationResultInRange(ushort.MaxValue, ushort.MaxValue, ushort.MaxValue, false, 0UL, NumericTypes.UShort);

            AssertMultiplicationResultInRange(1UL, ushort.MaxValue, ushort.MaxValue, true, ushort.MaxValue, NumericTypes.UShort);
            AssertMultiplicationResultInRange(ushort.MaxValue, 1UL, ushort.MaxValue, true, ushort.MaxValue, NumericTypes.UShort);

            AssertMultiplicationResultInRange(2UL, ushort.MaxValue / 2, ushort.MaxValue / 2, false, 0UL, NumericTypes.UShort);
            AssertMultiplicationResultInRange(ushort.MaxValue / 2, 2UL, ushort.MaxValue / 2, false, 0UL, NumericTypes.UShort);

            AssertMultiplicationResultInRange(2UL, ushort.MaxValue / 2, ushort.MaxValue - 1, true, ushort.MaxValue - 1, NumericTypes.UShort);
            AssertMultiplicationResultInRange(ushort.MaxValue / 2, 2UL, ushort.MaxValue - 1, true, ushort.MaxValue - 1, NumericTypes.UShort);

            AssertMultiplicationResultInRange(10UL, 20UL, ushort.MaxValue, true, 200UL, NumericTypes.UShort);
            AssertMultiplicationResultInRange(20UL, 10UL, ushort.MaxValue, true, 200UL, NumericTypes.UShort);



            AssertMultiplicationResultInRange(byte.MaxValue, byte.MaxValue, byte.MaxValue, false, 0UL, NumericTypes.Byte);

            AssertMultiplicationResultInRange(1UL, byte.MaxValue, byte.MaxValue, true, byte.MaxValue, NumericTypes.Byte);
            AssertMultiplicationResultInRange(byte.MaxValue, 1UL, byte.MaxValue, true, byte.MaxValue, NumericTypes.Byte);

            AssertMultiplicationResultInRange(2UL, byte.MaxValue / 2, byte.MaxValue / 2, false, 0UL, NumericTypes.Byte);
            AssertMultiplicationResultInRange(byte.MaxValue / 2, 2UL, byte.MaxValue / 2, false, 0UL, NumericTypes.Byte);

            AssertMultiplicationResultInRange(2UL, byte.MaxValue / 2, byte.MaxValue - 1, true, byte.MaxValue - 1, NumericTypes.Byte);
            AssertMultiplicationResultInRange(byte.MaxValue / 2, 2UL, byte.MaxValue - 1, true, byte.MaxValue - 1, NumericTypes.Byte);

            AssertMultiplicationResultInRange(10UL, 20UL, byte.MaxValue, true, 200UL, NumericTypes.Byte);
            AssertMultiplicationResultInRange(20UL, 10UL, byte.MaxValue, true, 200UL, NumericTypes.Byte);
        }
    }
}
