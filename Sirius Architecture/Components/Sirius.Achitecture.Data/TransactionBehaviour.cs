namespace Sirius.Architecture.Data {

    /// <summary>
    /// Specifies the exact behaviour of the begin / commit / rollback transaction methods.
    /// </summary>
    public enum TransactionBehaviour {

        /// <summary>
        /// Native SQL Server behaviour. This is the recommended option for new development because it matches stored procedure behaviour.
        /// <para>
        /// <list type="bullet">
        /// <item><description>Begin - increments the nesting level (and begins a physical transaction if nesting level was zero).</description></item>
        /// <item><description>Commit - decrements the nesting level (and commits the physical transaction if nesting level is now zero). If called again after nesting level is zero, an exception is thrown.</description></item>
        /// <item><description>Rollback - always sets the nesting level to zero and rolls back the physical transaction regardless of the current level. If called again after nesting level is zero, it is silently ignored.</description></item>
        /// <item><description>Close - silently roll back if there is a pending transaction.</description></item>
        /// </list>
        /// </para>
        /// </summary>
        SqlServer,

        /// <summary>
        /// Existing PMDAO behaviour. Use this for porting code from PMDAO without having to change the error-handling logic.
        /// <para>
        /// <list type="bullet">
        /// <item><description>Begin - increments the nesting level (and begins a physical transaction if nesting level was zero).</description></item>
        /// <item><description>Commit - decrements the nesting level (and commits the physical transaction if nesting level is now zero). If called again after nesting level is zero, it is silently ignored.</description></item>
        /// <item><description>Rollback - decrements the nesting level (and rolls back the physical transaction if nesting level is now zero). If called again after nesting level is zero, it is silently ignored.</description></item>
        /// <item><description>Close - silently roll back if there is a pending transaction.</description></item>
        /// </list>
        /// </para>
        /// </summary>
        PMDAO
    }
}
