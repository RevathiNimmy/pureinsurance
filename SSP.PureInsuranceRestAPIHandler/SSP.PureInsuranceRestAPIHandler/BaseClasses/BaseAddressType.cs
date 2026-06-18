using Newtonsoft.Json;
using SSP.PureInsuranceRestAPIHandler.Enums;
using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseAddressType
    {
        public string AddressKey { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine10 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string AddressLine4 { get; set; }

        public string AddressLine5 { get; set; }

        public string AddressLine6 { get; set; }

        public string AddressLine7 { get; set; }

        public string AddressLine8 { get; set; }

        public string AddressLine9 { get; set; }
        public AddressTypeType? AddressTypeCode { get; set; } = AddressTypeType.Correspondence;
        
        [JsonProperty(Required = Required.AllowNull)]
        public string CountryCode { get; set; } 
        [JsonProperty(Required = Required.AllowNull)]
        public string PostCode { get; set; }
        [JsonProperty(Required = Required.AllowNull)]
        public Nullable<int> CountryId { get; set; } = 0;
    }
}
