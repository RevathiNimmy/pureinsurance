namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseApproveCashListItemResponseType : BaseResponseType
    {
        public byte[] ApiTimeStamp { get; set; }
        public bool CheckValidationOnly { get; set; }
    }
}
