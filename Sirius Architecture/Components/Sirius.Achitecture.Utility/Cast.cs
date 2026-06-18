using System.Diagnostics;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// Safe typecasting functions that handle all possible methods of expressing a database <c>NULL</c> value.
    /// This class deliberately does NOT handle conversions from string to non-string because there is no safe
    /// default way of doing this. Use one of the <c>Convert</c> or <c>Format</c> classes instead.
    /// </summary>
    [DebuggerStepThrough]
    public static partial class Cast {
    }
}
