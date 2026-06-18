namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseAgentAdditionalDetailsType : BasePartyDetailsAdditionalDetailsType
    {
        public bool Cancelled { get; set; }
        public bool IsInTransferMode { get; set; }
        public string TransferToBusinessTypeCode { get; set; }
    }

}
