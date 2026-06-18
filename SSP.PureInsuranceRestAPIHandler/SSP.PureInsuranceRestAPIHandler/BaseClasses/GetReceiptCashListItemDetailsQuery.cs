namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetReceiptCashListItemDetails
{
    public class GetReceiptCashListItemDetailsQuery : GetReceiptCashListItemDetailsQueryBase //GetReceiptCashListItemDetailsQueryResponse>
    {
        public int PoliciesPageSize { get; set; }
        public int PoliciesPageNumber { get; set; }
        public int InstalmentPlanDetailsPageNumber { get; set; }
        public int InstalmentPlanDetailsPageSize { get; set; }
        public string PoliciesSortBy { get; set; }
        public string InstalmentPlanDetailsSortBy { get; set; }
    }
}
