namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateMarketplacePolicyStatusCommandBase : BaseRequestType
    {
		public int InsuranceFileKey { get; set; }
        public bool IsMarketPlacePolicy { get; set; }
    }
    
}