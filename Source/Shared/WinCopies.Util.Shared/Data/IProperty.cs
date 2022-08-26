using System.Collections;
using System.Collections.Generic;

namespace WinCopies.Data
{
    public interface INode<T> : IEnumerable<INode<T>>
    {
        T Value { get; }
    }

    public interface IProperty
    {
        string Name { get; }

        object Value { get; }
    }

    public interface IPropertyGroup<T> : IEnumerable<IProperty>
    {
        T Group { get; }
    }

    public abstract class PropertyEnumerable<TValue, TGroup> : IEnumerable<INode<TValue
#if CS9
            ?
#endif
            >> where TValue : IEnumerable<IPropertyGroup<TGroup>>
#if CS8
        ?
#endif
    {
        public abstract IEnumerator<INode<TValue
#if CS9
            ?
#endif
            >> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
