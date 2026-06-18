namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseDMEFolderType
    {
        public System.DateTime CreateDate { get; set; }
        public string ExternalCode { get; set; }
        public int FolderLevel { get; set; }
        public int FolderNum { get; set; }
        public string Name { get; set; }
        public int ParentNum { get; set; }
    }
}
