using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseContactType
    {
        public string AreaCode { get; set; }
        public BaseContactDetailType ContactDetail { get; set; }
        public ContactTypeType? ContactTypeCode { get; set; }
        public string ContactTypeDescription { get; set; }
        public int ContactTypeId { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        public string OtherContactTypeCode { get; set; }
    }
}
