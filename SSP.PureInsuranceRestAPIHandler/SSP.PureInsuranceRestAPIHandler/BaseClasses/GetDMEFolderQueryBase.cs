namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetDMEFolderQueryBase : BaseRequestType
    {
        public int FolderNum { get; set; }
        public string FolderPath { get; set; }
        public bool IncludeFiles { get; set; }
        public int AgentKey { get; set; }
        public int FilterCategoryId { get; set; }
        public int FilterSubCategoryId { get; set; }
        public string FilterDocName { get; set; }
    }
}
