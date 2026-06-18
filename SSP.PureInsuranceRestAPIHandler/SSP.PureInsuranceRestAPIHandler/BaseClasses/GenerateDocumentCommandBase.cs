namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GenerateDocumentCommandBase : BaseRequestType
    {

        public string ArchiveDocFileName { get; set; }
        public int ClaimKey { get; set; }
        public bool ClaimKeySpecified { get; set; }
        public string DestinationFileName { get; set; }
        public string DocumentRef { get; set; }

        public string DocumentTemplateCode { get; set; }
        public int InsuranceFileKey { get; set; }
        public int InsuranceFolderKey { get; set; }
        public bool IsSuppressArchive { get; set; }
        public int Mode { get; set; }
        public bool OutputAsHTML { get; set; }
        public bool OutputAsPDF { get; set; }
        public bool OutputAsTXT { get; set; }
        public string ParameterXML { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The PartyKey field is required")]
        //
        public int PartyKey { get; set; }
        public bool SkipArchiveOnEdit { get; set; }
        public bool SpoolDocumentOnly { get; set; }
        public bool SpoolDocumentOnlySpecified { get; set; }
        public int SourceID { get; set; }
    }
}
