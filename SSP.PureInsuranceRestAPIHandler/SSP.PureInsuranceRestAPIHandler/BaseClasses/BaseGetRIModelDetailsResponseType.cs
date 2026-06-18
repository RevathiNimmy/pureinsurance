using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetRIModelDetailsResponseType : BaseResponseType
    {
        public int ClaimAllocationType { get; set; }

        public string Code { get; set; }

        public string CurrencyCode { get; set; }

        public string Description { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string FACPremiums { get; set; }

        public int RIModelKey { get; set; }

        public string RIModelType { get; set; }
    }
}
