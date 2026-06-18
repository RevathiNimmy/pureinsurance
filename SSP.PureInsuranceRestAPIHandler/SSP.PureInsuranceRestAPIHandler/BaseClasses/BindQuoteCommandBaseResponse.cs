using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BindQuoteCommandBaseResponse : BaseTransactResponseType
    {
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];
        public System.Collections.Generic.List<BaseGeneralWarningResponseType> Warnings { get; set; }
        public BaseTaxesAndFeesType PolicyLevelTaxesAndFees { get; set; }
    }
}
