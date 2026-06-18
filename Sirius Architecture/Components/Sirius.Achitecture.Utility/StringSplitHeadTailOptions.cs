using System;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// Controls the behaviour of the <see cref="StringSplit.HeadTail"/> methods.
    /// </summary>
    [Flags]
    public enum StringSplitHeadTailOptions {

        /// <summary>
        /// The default behaviour. Do not use in combination with any other flag.
        /// </summary>
        None = 0,

        /// <summary>
        /// If true, search backwards from the end of the text.
        /// If false (the default), search forwards from the start of the text.
        /// </summary>
        SearchFromEnd = 1,

        /// <summary>
        /// If true, and a separator is not found, return the whole text as the tail.
        /// If false (the default), and a separator is not found, return the whole text as the head.
        /// </summary>
        DefaultToTail = 2,

        /// <summary>
        /// If true, the head and tail parts are trimmed before being returned.
        /// If false (the default), the head and tail parts are returned as is.
        /// </summary>
        RemoveWhitespace = 4
    }
}
