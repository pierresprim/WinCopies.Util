#if CS5
using System;
using System.Collections;
using System.Collections.Generic;

using WinCopies.Util;

namespace WinCopies.Collections.Generic
{
    public unsafe interface IUnsafeReadOnlyList<T> : IReadOnlyList<T> where T : unmanaged
    {
        new T* this[int index] { get; }
    }

    public unsafe class NumericReadOnlyArray<T> : IUnsafeReadOnlyList<T>
#if CS8
        , DotNetFix.Generic.IEnumerable<T>
#endif
        where T : unmanaged
    {
        protected byte[] Array { get; }
        protected ConverterIn<int, T> Converter { get; private set; }

        public bool IsUnsigned { get; }

        public T this[int index] => GetAt(index);

        T* IUnsafeReadOnlyList<T>.this[int index] => GetPointerAt(index);

        public int Count => GetCount();

        public unsafe NumericReadOnlyArray(in byte[] array)
        {
            int size = (Array = array) == null ? throw new ArgumentNullException(nameof(array)) : sizeof(T);
            Type type = typeof(T);

            bool isType(params Type[] types) => type.IsType(true, types);

            if ((array.Length % size == 0 ? size.Between(1, 8) : throw new ArgumentException("Inconsistent range of values.")) && (isType(typeof(sbyte), typeof(short), typeof(int), typeof(long)) || (IsUnsigned = isType(typeof(ushort), typeof(uint), typeof(ulong)))))
            {
                void setConverter<TSigned, TUnsigned>(in Func<byte[], int, TSigned> x, in Func<byte[], int, TUnsigned> y, TypeCode typeCode)
                {
                    Func<byte[], int, object> func;

                    Func<byte[], int, object> getFunc<TResult>(Func<byte[], int, TResult> _func) => (_x, _y) => _func(_x, _y);

                    if (IsUnsigned)
                    {
                        func = getFunc(y);

                        typeCode++;
                    }

                    else

                        func = getFunc(x);

                    Converter = (in int i) => (T)System.Convert.ChangeType(func(Array, i), typeCode);
                }

                switch (sizeof(T))
                {
                    case 1:

                        Converter = (in int i) => (T)System.Convert.ChangeType(Array[i], TypeCode.SByte);

                        break;

                    case 2:

                        setConverter(BitConverter.ToInt16, BitConverter.ToUInt16, TypeCode.Int16);

                        break;

                    case 4:

                        setConverter(BitConverter.ToInt32, BitConverter.ToUInt32, TypeCode.Int32);

                        break;

                    case 8:

                        setConverter(BitConverter.ToInt64, BitConverter.ToUInt64, TypeCode.Int64);

                        break;
                }
            }

            else

                throw new InvalidOperationException($"{nameof(T)} is {type}, but this type is not supported.");
        }

        private unsafe T* GetPointerAt(int index)
        {
            fixed (byte* result = &Array[index])

                return (T*)result;
        }

        private unsafe T GetAt(in int index) => Converter(index * sizeof(T));

        private unsafe int GetCount() => Array.Length / sizeof(T);

        public IEnumerator<T> GetEnumerator() => new DotNetFix.Generic.ArrayEnumerator<T>(this);
#if !CS8
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
#endif
    }
}
#endif
