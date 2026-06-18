using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindAddressQueryBaseResponse : BasePagedResponse
    {
        public List<BaseAddressLookupType> AddressLookup { get; set; }
    }
}
