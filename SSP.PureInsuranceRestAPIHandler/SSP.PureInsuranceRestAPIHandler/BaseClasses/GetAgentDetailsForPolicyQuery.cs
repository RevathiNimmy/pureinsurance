using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{

    public class GetAgentDetailsForPolicyQuery : GetAgentDetailsForPolicyQueryBase
    {
        public int AddressPageNumber { get; set; }
        public int AddressPageSize { get; set; }
        public int ContactsPageNumber { get; set; }
        public int ContactsPageSize { get; set; }
        public string AddressSortBy { get; set; }
        public string ContactsSortBy { get; set; }
    }
}
