using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateCoinsuranceValuesCommandBase : BaseRequestType
    {

        public System.Collections.Generic.List<BaseUpdateCoinsuranceValuesRequestTypeRow> CoInsurers { get; set; }
        public int DefaultId { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The InsuranceFileKey field is required")]
        //
        public int InsuranceFileKey { get; set; }
        public bool IsRecovered { get; set; }
        public bool IsSurcharged { get; set; }

        //[Minength(1, ErrorMessage = "The ApiTimeStamp field cannot be empty")]
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
    }
}
