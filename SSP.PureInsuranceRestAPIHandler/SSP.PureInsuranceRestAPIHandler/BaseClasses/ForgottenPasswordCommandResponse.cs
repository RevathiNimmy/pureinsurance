namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ForgottenPasswordCommandResponse : BaseResponseType
    {
        public int ClaimKey { get; set; }
        public int Mode { get; set; }
        public bool OutputAsHTML { get; set; }
        public bool OutputAsPDF { get; set; }
        public string ParameterXML { get; set; }
        public string TransactionType { get; set; }
    }
}
