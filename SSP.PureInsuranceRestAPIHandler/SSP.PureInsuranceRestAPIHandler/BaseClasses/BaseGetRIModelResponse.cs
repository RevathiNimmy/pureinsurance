namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetRIModelResponse : BaseResponseType
    {
        public int ClaimAllocationType { get; set; }
        public string Code { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public System.DateTime ExpiryDate { get; set; }
        public string FACPremiums { get; set; }
        public int RIModelKey { get; set; }
        public string RIModelType { get; set; }
        public int Currecyid { get; set; }
    }

}
