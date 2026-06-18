using System;
using System.ComponentModel;
using System.Text;
using System.Xml;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// Provides helper methods for processing XML data.
    /// </summary>
    public static class XmlUtility {

        #region Private Constants

        private const string AddElementWarning = "Use LINQ to XML instead.";

        #endregion
        
        #region Public Static Methods - ToXPath

        /// <overloads>
        /// Format a value as an XPath literal value.
        /// </overloads>
        /// <summary>
        /// Format a value as an XPath literal value.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>The XPath text representing the value.</returns>
        public static string ToXPath(bool value) {

            return XmlConvert.ToString(value);
        }

        /// <summary>
        /// Format a value as an XPath literal value.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>The XPath text representing the value.</returns>
        public static string ToXPath(long value) {

            return XmlConvert.ToString(value);
        }

        /// <summary>
        /// Format a value as an XPath literal value.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>The XPath text representing the value.</returns>
        public static string ToXPath(string value) {

            // If the value contains only single or double quotes, construct an XPath literal.
            if(!value.Contains("\"")) {
                return "\"" + value + "\"";
            }
            if(!value.Contains("'")) {
                return "'" + value + "'";
            }

            // If the value contains both single and double quotes, construct an expression
            // that concatenates all non-double-quote substrings with the quotes, e.g.
            // concat("foo", '"', "bar").
            StringBuilder expr = new StringBuilder();
            expr.Append("concat(");
            string[] substrings = value.Split('\"');
            for(int i = 0; i < substrings.Length; i++) {
                bool needComma = (i > 0);
                if(substrings[i] != "") {
                    if(i > 0) {
                        expr.Append(", ");
                    }
                    expr.Append("\"");
                    expr.Append(substrings[i]);
                    expr.Append("\"");
                    needComma = true;
                }
                if(i < substrings.Length - 1) {
                    if(needComma) {
                        expr.Append(", ");
                    }
                    expr.Append("'\"'");
                }

            }
            expr.Append(")");
            return expr.ToString();
        }

        #endregion

        #region Public Static Methods - XmlEncode

        /// <summary>
        /// Replace XML standard entity references with their defined values.
        /// </summary>
        /// <param name="value">XML-escaped text to decode</param>
        /// <returns>Raw text</returns>
        public static string XmlDecode(string value) {

            StringBuilder text = new StringBuilder(value);
            text.Replace("&quot;", @"""");
            text.Replace("&apos;", "'");
            text.Replace("&gt;", ">");
            text.Replace("&lt;", "<");
            text.Replace("&amp;", "&");
            return text.ToString();
        }

        /// <summary>
        /// Replace reserved characters in XML with their corresponding entity references.
        /// </summary>
        /// <param name="value">Raw text to encode</param>
        /// <returns>XML-escaped text</returns>
        public static string XmlEncode(string value) {

            StringBuilder text = new StringBuilder(value);
            text.Replace("&", "&amp;");
            text.Replace("<", "&lt;");
            text.Replace(">", "&gt;");
            text.Replace("'", "&apos;");
            text.Replace(@"""", "&quot;");
            return text.ToString();
        }

        #endregion

        #region Public Static Methods - Add Element [Obsolete]

        /// <overloads>
        /// Create and append a new child element to the specified parent node.
        /// </overloads>
        /// <summary>
        /// Create and append a new child element to the specified parent node.
        /// </summary>
        /// <param name="parentNode">Parent node (must belong to a document).</param>
        /// <param name="name">Qualified name of the new child element.</param>
        /// <returns>The new child element.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(AddElementWarning)]
        public static XmlElement AddElement(XmlNode parentNode, string name) {

            XmlElement childElement = parentNode.OwnerDocument.CreateElement(name);
            parentNode.AppendChild(childElement);
            return childElement;
        }

        /// <summary>
        /// Create and append a new child element to the specified parent node.
        /// </summary>
        /// <param name="parentNode">Parent node (must belong to a document).</param>
        /// <param name="qualifiedName">Qualified name of the new child element.</param>
        /// <param name="namespaceUri">Namespace URI of the new child element.</param>
        /// <returns>The new child element.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(AddElementWarning)]
        public static XmlElement AddElement(XmlNode parentNode, string qualifiedName, string namespaceUri) {

            XmlElement childElement = parentNode.OwnerDocument.CreateElement(qualifiedName, namespaceUri);
            parentNode.AppendChild(childElement);
            return childElement;
        }

        /// <summary>
        /// Create and append a new child element to the specified parent node.
        /// </summary>
        /// <param name="parentNode">Parent node (must belong to a document).</param>
        /// <param name="prefix">Namespace prefix of the new child element.</param>
        /// <param name="localName">Local name of the new child element.</param>
        /// <param name="namespaceUri">Namespace URI of the new child element.</param>
        /// <returns>The new child element.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(AddElementWarning)]
        public static XmlElement AddElement(XmlNode parentNode, string prefix, string localName, string namespaceUri) {

            XmlElement childElement = parentNode.OwnerDocument.CreateElement(prefix, localName, namespaceUri);
            parentNode.AppendChild(childElement);
            return childElement;
        }

        #endregion
    }
}
