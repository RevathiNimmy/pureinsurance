using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUpdatePartyBankStatusType
    {
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }
        public System.Collections.Generic.List<SAMErrors> Errors { get; set; }
        public int PartyBankKey { get; set; }
        public int RowKey { get; set; }
    }
}
