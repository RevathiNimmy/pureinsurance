using System;

namespace Sirius.Architecture.Data {

    /// <summary>
    /// Flags that alter the default behaviour of a <see cref="SiriusCommand"/> before execution.
    /// </summary>
    [Flags]
    public enum SiriusCommandBehaviour {

        /// <summary>
        /// The default behaviour (the command is not prepared before execution, every parameter is strictly checked for type and size).
        /// Do not use in combination with any other flag.
        /// </summary>
        None = 0,

        /// <summary>
        /// Prepare the command before execution. This is only relevant for SQL statement batches, not stored procedure RPC calls.
        /// </summary>
        Prepare = 1,

        /// <summary>
        /// Allow string values for numeric and datetime parameter types. This flag is only for backward compatibility,
        /// because it is not safe for globalisation. DO NOT use it in new code.
        /// </summary>
        [Obsolete("Do not pass string values to non-string parameters.")]
        AllowStringValuesForNonStringTypes = 2,

        // <summary>
        // Truncate parameter values that exceed the defined size instead of throwing an exception. Only set this if you are sure that you want it.
        // </summary>
        // SilentlyTruncateOverMaximumSize = 4
    }
}
