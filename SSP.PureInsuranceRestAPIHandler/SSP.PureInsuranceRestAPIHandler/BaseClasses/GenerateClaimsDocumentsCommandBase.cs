namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GenerateClaimsDocumentsCommandBase : BaseRequestType
    {
        public int ClaimKey { get; set; }
        public int Mode { get; set; }
        public bool OutputAsHTML { get; set; }
        public bool OutputAsPDF { get; set; }
        public string ParameterXML { get; set; } = null;
        public string TransactionType { get; set; } = null;
    }
}
