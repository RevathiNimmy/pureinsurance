using System.Collections.Generic;
namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ReplacePartyContactCommandBase : BaseRequestType
    {

        public int UserKey { get; set; }

        public List<BaseContactType> Contacts { get; set; }

        //[Range(1, int.MaxValue, ErrorMessage = "The PartyKey field is required")]
        //[DefaultValue(0)]
        public int PartyKey { get; set; }
    }
}
