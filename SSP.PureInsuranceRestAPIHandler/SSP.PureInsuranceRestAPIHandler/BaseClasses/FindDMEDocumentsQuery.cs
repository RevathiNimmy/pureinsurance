namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindDMEDocumentsQuery : BaseRequestType
    {
        public string PartyCode { get; set; }

        public string PartyName { get; set; }

        public string PolicyNumber { get; set; }

        public string ClaimNumber { get; set; }

        public string RiskIndex { get; set; }

        public string PostCode { get; set; }

        public string DocumentDescription { get; set; }

        public bool IncludeFiles { get; set; }

        public int ParentNum { get; set; }

        public string FolderName { get; set; }
        public int AgentKey { get; set; }
        public int PageNumber { get; set; }
        public int FoldersPageSize { get; set; }
        public int FoldersPageNumber { get; set; }
        public int DocumentsPageNumber { get; set; }
        public int DocumentsPageSize { get; set; }
        public string FoldersSortBy { get; set; }
        public string DocumentsSortBy { get; set; }
        public int FilterCategoryId { get; set; }
        public int FilterSubCategoryId { get; set; }
        public string FilterDocName { get; set; }
    }
}
