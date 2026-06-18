namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetDocumentListQueryBase : BaseRequestType
    {
        public int InsuranceFolderKey { get; set; }
        public int AgentKey { get; set; }
        public int FilterCategoryId { get; set; }
        public int FilterSubCategoryId { get; set; }
        public string FilterDocName { get; set; }
    }
}
