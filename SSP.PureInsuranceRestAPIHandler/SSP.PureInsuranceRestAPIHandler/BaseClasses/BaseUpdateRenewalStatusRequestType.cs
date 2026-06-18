namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUpdateRenewalStatusRequestType : BaseRequestType
    {
        public int RenewalStatusKey { get; set; }
        public string OldRenewalStatusTypeCode { get; set; }
        public int InsuranceFileKey { get; set; }
        public string RenewalStatusCode { get; set; }
        public byte[] QuoteTimeStamp { get; set; }
        public int InsuranceFolderKey { get; set; }
        public int RenewalStatusTypeKey { get; set; }
    }
}
