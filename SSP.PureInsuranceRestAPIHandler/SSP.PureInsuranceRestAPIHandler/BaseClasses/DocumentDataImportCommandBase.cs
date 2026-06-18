namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.DocumentDataImport
{
    public class DocumentDataImportCommandBase : BaseRequestType
    {
        public BasePostDocumentRequestType Document { get; set; }
        public int SAMStagingDocumentKey { get; set; }
    }
}
