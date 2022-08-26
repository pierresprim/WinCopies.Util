using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using WinCopies.Linq;
using WinCopies.Util;

using static WinCopies.ThrowHelper;
using static WinCopies.UtilHelpers;

using Enumerable = System.Linq.Enumerable;

namespace WinCopies.Data
{
    public abstract class XMLPropertyGroupNode : IPropertyGroup<string>
#if CS8
        , Collections.DotNetFix.Generic.IEnumerable<IProperty>
#endif
    {
        protected XmlNode Node { get; }

        public abstract string Group { get; }

        public XMLPropertyGroupNode(in XmlNode node) => Node = node;

        protected abstract IProperty GetProperty(XmlNode node);

        public IEnumerator<IProperty> GetEnumerator() => Node.Enumerate().Select(GetProperty).GetEnumerator();
#if !CS8
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
#endif
    }

    public abstract class XMLPropertyNode<TValue, TGroup> : INode<TValue
#if CS9
            ?
#endif
            >
#if CS8
        , Collections.DotNetFix.Generic.IEnumerable<INode<TValue
#if CS9
            ?
#endif
            >>
#endif
        where TValue : IEnumerable<IPropertyGroup<TGroup>>
    {
        protected XmlNode Node { get; }

        public TValue
#if CS9
            ?
#endif
            Value => PerformActionIfNotNull(GetNodes(GroupNodePredicate, GetPropertyGroupNode), GetPropertyGroupEnumerable, Delegates.GetDefault<IEnumerable<IPropertyGroup<TGroup>>, TValue>);

        public XMLPropertyNode(in XmlNode node) => Node = GetOrThrowIfNull(node, nameof(node));

        protected abstract bool IsCategoryNode(XmlNode node);
        protected abstract bool GroupNodePredicate(XmlNode node);

        protected IEnumerable<T>
#if CS8
            ?
#endif
            GetNodes<T>(in Predicate<XmlNode> predicate, in Converter<XmlNode, T> converter)
        {
            XmlNode
#if CS8
                ?
#endif
                categoryNode = Node.Enumerate().FirstOrDefaultValuePredicate(predicate);

            return categoryNode?.Enumerate().SelectConverter(converter);
        }

        protected abstract INode<TValue
#if CS9
            ?
#endif
            > GetPropertyNode(XmlNode node);
        protected abstract IPropertyGroup<TGroup> GetPropertyGroupNode(XmlNode node);
        protected abstract TValue GetPropertyGroupEnumerable(IEnumerable<IPropertyGroup<TGroup>> propertyGroups);

        public IEnumerator<INode<TValue
#if CS9
            ?
#endif
            >> GetEnumerator() => GetNodes(IsCategoryNode, GetPropertyNode)?.GetEnumerator() ?? GetEmptyEnumerator<INode<TValue
#if CS9
            ?
#endif
            >>();

#if !CS8
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
#endif
    }

    public abstract class XMLPropertyEnumerable<TValue, TGroup> : PropertyEnumerable<TValue, TGroup> where TValue : IEnumerable<IPropertyGroup<TGroup>>
    {
        protected XmlDocument Document { get; }

        public bool EnumerateAsRooted { get; set; }

        public XMLPropertyEnumerable(in XmlDocument document) => Document = GetOrThrowIfNull(document, nameof(document));

        protected abstract XMLPropertyNode<TValue,TGroup> GetPropertyNode(XmlNode node);

        public override IEnumerator<INode<TValue
#if CS9
            ?
#endif
            >> GetEnumerator()
        {
            XmlNode
#if CS8
                ?
#endif
                first = Document.DocumentElement;

            if (first == null)

                return GetEmptyEnumerator<INode<TValue
#if CS9
            ?
#endif
            >>();

            INode<TValue
#if CS9
                ?
#endif
                > rootNode = GetPropertyNode(first);

            return (EnumerateAsRooted ? Enumerable.Repeat(rootNode, 1) : rootNode).GetEnumerator();
        }
    }
}
