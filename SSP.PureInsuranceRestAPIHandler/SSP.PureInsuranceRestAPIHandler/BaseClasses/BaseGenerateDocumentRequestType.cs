namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGenerateDocumentRequestType : BaseRequestType
    {
        public string DocumentTemplateCode { get; set; }

        public int Mode { get; set; }

        public int PartyKey { get; set; }

        public int InsuranceFileKey { get; set; }

        public int InsuranceFolderKey { get; set; }

        public string ParameterXML { get; set; }

        public bool OutputAsHTML { get; set; }

        public bool OutputAsPDF { get; set; }

        public int ClaimKey { get; set; }

        public bool ClaimKeyFieldSpecified { get; set; }

        public bool SpoolDocumentOnly { get; set; }

        public string DocumentRef { get; set; }

        public string ArchiveDocFileName { get; set; }
        public bool IutputAsTXT { get; set; }
        public bool SkipArchiveOnEdit { get; set; }
        public bool OutputAsTXT { get; set; }
        public string DestinationFileName { get; set; }
        public bool IsSuppressArchive { get; set; }
        public int SourceID { get; set; }
    }
}
