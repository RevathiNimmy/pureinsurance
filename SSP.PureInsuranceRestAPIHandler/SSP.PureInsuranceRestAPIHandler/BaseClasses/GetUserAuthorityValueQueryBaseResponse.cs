namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUserAuthorityValueQueryBaseResponse : BaseResponseType
    {
        public int UserAuthorityOptionalValue1 { get; set; }
        public bool UserAuthorityOptionalValue1Specified { get; set; }
        public double UserAuthorityOptionalValue2 { get; set; }
        public bool UserAuthorityOptionalValue2Specified { get; set; }
        public string UserAuthorityOptionalValue3 { get; set; }
        public bool UserAuthorityOptionalValue3Specified { get; set; }
        public bool UserAuthorityOptionalValue3Specified_baseAmount { get; set; }
        public double? UserAuthorityOptionalValue3_baseAmount { get; set; }
        public string UserAuthorityValue { get; set; }
    }
}
