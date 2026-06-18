namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetListResponseType
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDeleted { get; set; }
        public int Key { get; set; }
        public int? ParentKey { get; set; }
        public bool ParentKeySpecified { get; set; }
    }
}
