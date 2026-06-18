using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCashListResponseType : BaseResponseType
    {

        public int CashListKey { get; set; }
        public byte[] ApiTimeStamp { get; set; }
        public int Version { get; set; }
        public System.Collections.Generic.List<BaseCashListResponseTypeWarnings> Warnings { get; set; }
    }
}
