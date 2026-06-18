namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetAccountIdFromPartyCntQueryBase : BaseRequestType
    {
        public int PartyCnt { get; set; }
        public int CompanyId { get; set; }
    }
}
