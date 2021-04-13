namespace WinCopies.Collections.Generic
{
    public class ReversedArray<T> : WinCopies.Collections.DotNetFix.Generic.ICountableEnumerable<T>, IReadOnlyList<T>
    {
        private T[] _array;

        public int Count => _array.Length;

        private int GetIndex(in int index) => _array.Length - 1 - index;

        public T this[int index] { get => _array[GetIndex(index)]; set => _array[GetIndex(index)] = value; }

        public ArrayEnumerator<T> GetEnumerator() => new ArrayEnumerator<T>(_array, true);

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
