namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddDocumentToDocumasterCommandBase : BaseRequestType
    {
        public int ClaimKey { get; set; }
        public string Description { get; set; }
        public byte[] Document { get; set; } = new byte[0];
        public string FileName { get; set; }
        public int FolderNum { get; set; }
        public int InsuranceFolderKey { get; set; }
        public int PartyKey { get; set; }
        public bool VisibleFromWeb { get; set; }
        public int DocumentTemplateGroupId { get; set; }
        public int DocumentTemplateSubGroupId { get; set; }
    }
}
