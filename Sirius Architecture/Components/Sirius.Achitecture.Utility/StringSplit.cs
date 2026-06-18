using System;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// Functions for parsing strings by splitting them in different ways.
    /// </summary>
    /// <remarks>
    /// Please note that these functions are intended for very low-level parsing of arbitrary text that could
    /// come from anywhere. If you are parsing whole streams or structured files, it would be more appropriate
    /// to use an API at a higher level of abstraction.
    /// </remarks>
    public static class StringSplit {

        #region Public Static Methods - HeadTail(char[])

        /// <overloads>
        /// Split text into two parts either side of the specified separator.
        /// </overloads>
        /// <summary>
        /// Split text into two parts either side of the specified separator.
        /// </summary>
        /// <param name="text">The text to split.</param>
        /// <returns>The text to the left of the separator (the head).</returns>
        /// <param name="tail">The text to the right of the separator (the tail).</param>
        /// <param name="separators">Array of possible separator characters to search for.</param>
        public static string HeadTail(string text, out string tail, char[] separators) {

            return HeadTail(text, out tail, separators, StringSplitHeadTailOptions.None);
        }

        /// <summary>
        /// Split text into two parts either side of the specified separator.
        /// </summary>
        /// <param name="text">The text to split.</param>
        /// <returns>The text to the left of the separator (the head).</returns>
        /// <param name="tail">The text to the right of the separator (the tail).</param>
        /// <param name="separators">Array of possible separator characters to search for.</param>
        /// <param name="options">Fine-tune the behaviour of the method.</param>
        public static string HeadTail(string text, out string tail, char[] separators, StringSplitHeadTailOptions options) {

            int separatorPos = 0;
            if((options & StringSplitHeadTailOptions.SearchFromEnd) != 0) {
                separatorPos = text.LastIndexOfAny(separators);
            } else {
                separatorPos = text.IndexOfAny(separators);
            }

            return HeadTail(text, out tail, separatorPos, 1, options);
        }

        #endregion

        #region Public Static Methods - HeadTail(string)

        /// <summary>
        /// Split text into two parts either side of the specified separator.
        /// </summary>
        /// <param name="text">The text to split.</param>
        /// <returns>The text to the left of the separator (the head).</returns>
        /// <param name="tail">The text to the right of the separator (the tail).</param>
        /// <param name="separator">Separator text to search for.</param>
        public static string HeadTail(string text, out string tail, string separator) {

            return HeadTail(text, out tail, separator, StringComparison.CurrentCulture, StringSplitHeadTailOptions.None);
        }

        /// <summary>
        /// Split text into two parts either side of the specified separator.
        /// </summary>
        /// <param name="text">The text to split.</param>
        /// <returns>The text to the left of the separator (the head).</returns>
        /// <param name="tail">The text to the right of the separator (the tail).</param>
        /// <param name="separator">Separator text to search for.</param>
        /// <param name="comparisonType">The comparison rules to use for the search.</param>
        public static string HeadTail(string text, out string tail, string separator, StringComparison comparisonType) {

            return HeadTail(text, out tail, separator, comparisonType, StringSplitHeadTailOptions.None);
        }

        /// <summary>
        /// Split text into two parts either side of the specified separator.
        /// </summary>
        /// <param name="text">The text to split.</param>
        /// <returns>The text to the left of the separator (the head).</returns>
        /// <param name="tail">The text to the right of the separator (the tail).</param>
        /// <param name="separator">Separator text to search for.</param>
        /// <param name="options">Fine-tune the behaviour of the method.</param>
        public static string HeadTail(string text, out string tail, string separator, StringSplitHeadTailOptions options) {

            return HeadTail(text, out tail, separator, StringComparison.CurrentCulture, options);
        }

        /// <summary>
        /// Split text into two parts either side of the specified separator.
        /// </summary>
        /// <param name="text">The text to split.</param>
        /// <returns>The text to the left of the separator (the head).</returns>
        /// <param name="tail">The text to the right of the separator (the tail).</param>
        /// <param name="separator">Separator text to search for.</param>
        /// <param name="comparisonType">The comparison rules to use for the search.</param>
        /// <param name="options">Fine-tune the behaviour of the method.</param>
        public static string HeadTail(string text, out string tail, string separator, StringComparison comparisonType, StringSplitHeadTailOptions options) {

            int separatorPos = 0;
            if((options & StringSplitHeadTailOptions.SearchFromEnd) != 0) {
                separatorPos = text.LastIndexOf(separator, comparisonType);
            } else {
                separatorPos = text.IndexOf(separator, comparisonType);
            }

            return HeadTail(text, out tail, separatorPos, separator.Length, options);
        }

        #endregion

        #region Private Static Methods - HeadTail

        /// <summary>
        /// Factored-out internal implementation of the actual splitting code.
        /// </summary>
        /// <param name="text">The text to split.</param>
        /// <returns>The text to the left of the separator (the head).</returns>
        /// <param name="tail">The text to the right of the separator (the tail).</param>
        /// <param name="separatorPos">Start position of the separator.</param>
        /// <param name="separatorLength">Length of the separator.</param>
        /// <param name="options">Fine-tune the behaviour of the method.</param>
        private static string HeadTail(string text, out string tail, int separatorPos, int separatorLength, StringSplitHeadTailOptions options) {

            string head = null;

            if(separatorPos > -1) {
                head = text.Substring(0, separatorPos);
                tail = text.Substring(separatorPos + separatorLength);
            } else if((options & StringSplitHeadTailOptions.DefaultToTail) != 0) {
                head = string.Empty;
                tail = text;
            } else {
                head = text;
                tail = string.Empty;
            }

            if((options & StringSplitHeadTailOptions.RemoveWhitespace) != 0) {
                head = head.Trim();
                tail = tail.Trim();
            }

            return head;
        }

        #endregion

        #region Public Static Methods - HeadTailCsv

        /// <summary>
        /// Extract the first field from a comma-separated list, returning the rest of the fields by reference.
        /// Fields are separated by commas surrounded by white space. If a field is enclosed in double quotes,
        /// its contents are taken literally. To include a double quote within a quoted field, double it up.
        /// </summary>
        /// <param name="text">The text to split.</param>
        /// <param name="tail">The text minus the first CSV field (the tail).</param>
        /// <returns>The first CSV field (the head).</returns>
        public static string HeadTailCsv(string text, out string tail) {

            // Remove leading and trailing whitespace to make all the separator tests simpler.
            text = text.Trim();

            // Find the next separator character.
            int nextCommaPos = text.IndexOf(',');
            int nextQuotePos = text.IndexOf('"');

            string head;

            if(nextCommaPos == -1 && nextQuotePos == -1) {
                // No more separators found. Return whole string as the head.
                head = text;
                tail = string.Empty;

            } else if(nextCommaPos > -1 && (nextQuotePos == -1 || nextCommaPos < nextQuotePos)) {
                // There are multiple list items, and at least the first one has no quotes in it.
                // Therefore we just split at the first comma.
                head = text.Substring(0, nextCommaPos).Trim();
                tail = text.Substring(nextCommaPos + 1).Trim();

            } else if(nextQuotePos > 0) {
                // First item is quoted, but the quote isn't the first character. This is illegal, so we
                // assume there is a comma immediately before the first quote and treat that as the second item.
                head = text.Substring(0, nextQuotePos - 1).Trim();
                tail = text.Substring(nextQuotePos);

            } else {
                // First item starts with a quote. Therefore we scan through to find the finishing quote,
                // taken doubled quotes into account along the way. If the finishing quote is not followed by a
                // comma, assume there is one.
                int headStartPos = nextQuotePos + 1;
                int headEndPos = -1;

                for(; ; ) {
                    // Find next quote.
                    nextQuotePos = text.IndexOf('"', nextQuotePos + 1);
                    // If next quote is not found, then assume the end of the text is the end of the item.
                    if(nextQuotePos == -1) {
                        headEndPos = text.Length - 1;
                        break;
                    }
                    // If next quote is at the end of the text, then we have found the end of the item.
                    if(nextQuotePos == text.Length - 1) {
                        headEndPos = text.Length - 1;
                        break;
                    }
                    // If next quote is not doubled, then we have found the end of the item.
                    if(text[nextQuotePos + 1] != '"') {
                        headEndPos = nextQuotePos;
                        break;
                    }
                    // Next quote is doubled, skip over it and continue searching.
                    nextQuotePos += 1;
                }

                // Safety check: headEndPos will never be -1 at this point!
                System.Diagnostics.Debug.Assert(headEndPos > -1);

                // Extract the head and decode any doubled quotes within it.
                head = text.Substring(headStartPos, headEndPos - headStartPos).Replace(@"""""", @"""");

                // Extract the tail and remove the starting comma (if it exists).
                tail = text.Substring(headEndPos + 1).Trim();
                if(tail.StartsWith(",")) {
                    tail = tail.Substring(1).Trim();
                }
            }

            return head;
        }

        #endregion
    }
}
