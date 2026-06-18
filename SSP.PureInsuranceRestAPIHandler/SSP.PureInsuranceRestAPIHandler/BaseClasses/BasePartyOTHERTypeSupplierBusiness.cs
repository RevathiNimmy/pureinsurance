
namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePartyOtherTypeSupplierBusiness
    {
        public int BusinessId { get; set; }
        public int SpecialityId { get; set; }
        public string BusinessCode { get; set; } = string.Empty;
        public string SpecialityCode { get; set; } = string.Empty;
    }
}
