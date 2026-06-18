using System.Xml.Serialization;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimPaymentTaxesResponseType : BaseResponseType
    {
        public BaseClaimPaymentTaxItemType[] TaxItems { get; set; }

        public BaseClaimPerilReservePaymentType[] Reserves { get; set; }

        [XmlElement("PaymentItems")]
        public BaseGetClaimPaymentTaxesResponseTypePaymentItems[] PaymentItems { get; set; }
    }
}
