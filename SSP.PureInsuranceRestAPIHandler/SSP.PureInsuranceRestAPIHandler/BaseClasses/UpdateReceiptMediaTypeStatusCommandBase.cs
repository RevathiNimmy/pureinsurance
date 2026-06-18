namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.UpdateReceiptMediaTypeStatus
{
    public class UpdateReceiptMediaTypeStatusCommandBase : BaseRequestType
    {
        
        public System.Collections.Generic.List<BaseUpdateReceiptMediaTypeStatusRequestTypeRow> CashListItems { get; set; }
    }
}
