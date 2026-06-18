namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetAllocationDetails
{
    public class GetAllocationDetailsQueryBaseResponse : BasePagedResponse
    {
        public System.Collections.Generic.List<BaseGetAllocationDetailsResponseTypeRow> Row { get; set; }
    }
}
