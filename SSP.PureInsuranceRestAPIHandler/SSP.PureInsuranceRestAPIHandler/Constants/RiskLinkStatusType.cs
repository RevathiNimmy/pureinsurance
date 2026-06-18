namespace SSP.PureInsuranceRestAPIHandler.Constants
{
    public static class RiskLinkStatusType
    {
        /// <summary>
        /// Risk has been copied/changed status if this is set then there is a record for this insurance file in the risk table
        /// </summary>
        public const string Changed = "C";

        /// <summary>
        /// Risk is unchanged in this version and points to the old risk
        /// </summary>
        public const string Unchanged = "U";

        /// <summary>
        /// Risk has been renewed, still points to the old risk record but has financial data for this version
        /// </summary>
        public const string Renewed = "R";

        /// <summary>
        /// Risk has been deleted in this version, has a records in risk tables
        /// </summary>
        public const string Deleted = "D";
    }

}
