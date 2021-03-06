﻿using System;

namespace Microsoft.Language.Xml
{
    using InternalSyntax;

    public class XmlNameSyntax : XmlNodeSyntax
    {
        internal new class Green : XmlNodeSyntax.Green
        {
            readonly XmlPrefixSyntax.Green xmlPrefix;
            readonly XmlNameTokenSyntax.Green localName;

            internal XmlPrefixSyntax.Green Prefix => xmlPrefix;
            internal XmlNameTokenSyntax.Green LocalName => localName;

            internal Green(XmlPrefixSyntax.Green xmlPrefix, XmlNameTokenSyntax.Green localName)
                : base(SyntaxKind.XmlName)
            {
                this.SlotCount = 2;
                this.xmlPrefix = xmlPrefix;
                AdjustWidth(xmlPrefix);
                this.localName = localName;
                AdjustWidth(localName);
            }

            internal override SyntaxNode CreateRed(SyntaxNode parent, int position) => new XmlNameSyntax(this, parent, position);

            internal override GreenNode GetSlot(int index)
            {
                switch (index)
                {
                    case 0: return xmlPrefix;
                    case 1: return localName;
                }

                throw new InvalidOperationException();
            }

            internal override GreenNode Accept(InternalSyntax.SyntaxVisitor visitor)
            {
                return visitor.VisitXmlName(this);
            }
        }

        internal new Green GreenNode => (Green)base.GreenNode;

        XmlPrefixSyntax prefix;
        XmlNameTokenSyntax localName;

        public XmlPrefixSyntax PrefixNode => GetRed(ref prefix, 0);
        public XmlNameTokenSyntax LocalNameNode => GetRed(ref localName, 1);

        internal XmlNameSyntax(Green green, SyntaxNode parent, int position)
            : base(green, parent, position)
        {

        }

        public override SyntaxNode Accept(SyntaxVisitor visitor)
        {
            return visitor.VisitXmlName(this);
        }

        public string LocalName => LocalNameNode?.Text;
        public string Prefix => PrefixNode?.Name?.Text;

        public string FullName => (PrefixNode != null ? (PrefixNode.Name?.Text ?? string.Empty) + ":" : string.Empty) + (LocalNameNode?.Text ?? string.Empty);

        public override string ToString()
        {
            return $"XmlNameSyntax {FullName}";
        }

        internal override SyntaxNode GetCachedSlot(int index)
        {
            switch (index)
            {
                case 0: return prefix;
                case 1: return localName;
                default: return null;
            }
        }

        internal override SyntaxNode GetNodeSlot(int slot)
        {
            switch (slot)
            {
                case 0: return PrefixNode;
                case 1: return LocalNameNode;
                default: return null;
            }
        }

        public XmlNameSyntax Update(XmlPrefixSyntax prefix, XmlNameTokenSyntax localName)
        {
            if (prefix != this.PrefixNode || localName != this.LocalNameNode)
            {
                var newNode = SyntaxFactory.XmlName(prefix, localName);
                return newNode;
            }

            return this;
        }

        public XmlNameSyntax WithPrefix(XmlPrefixSyntax prefix)
        {
            return Update(prefix, LocalNameNode);
        }

        public XmlNameSyntax WithLocalName(XmlNameTokenSyntax localName)
        {
            return Update(PrefixNode, localName);
        }
    }
}
