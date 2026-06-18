namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetSharepointFileListResponseTypeItemList
    {
        public System.DateTime CreatedDate { get; set; }
        public string DocumentTemplateGroup { get; set; }
        public string DocumentTemplateSubGroup { get; set; }
        public string Filename { get; set; }
        public bool InternalOnly { get; set; }
        public string ItemType { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public string PureUser { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
    }
}
