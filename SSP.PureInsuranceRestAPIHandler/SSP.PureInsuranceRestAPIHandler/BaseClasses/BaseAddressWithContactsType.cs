using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseAddressWithContactsType : BaseAddressType
    {

        public System.Collections.Generic.List<BaseContactType> Contacts { get; set; }
    }
}
