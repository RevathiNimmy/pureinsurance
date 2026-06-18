namespace SSP.PureInsuranceRestAPIHandler.Constants
{
    public static class ErrorMessage
    {
        public const string BadRequest = "Bad Request";
        public const string UnAuthorisedParty = "Security check failed {0} does not have permission to access party {1}";
        public const string UnAuthorisedInsuranceFile = "Security check failed {0} does not have permission to access policy {1}";
        public const string UnAuthorisedSource = "Security check failed {0} does not have permission to access source {1}";
    }
}
