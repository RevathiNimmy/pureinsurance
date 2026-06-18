using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetAgentDetailsForPolicyResponse : BaseResponseType
    {

        public string Address1 { get; set; }

         public string  Address2 { get; set; }

        public string  Address3 { get; set; }

        public string Address4 { get; set; }

        public int AddressKey { get; set; }

        public int AddressUsageTypeKey { get; set; }
        

        public List<BaseAddressType> Addresses;

        public string AreaCode { get; set; }

        public String Code { get; set; } 

        public int ContactTypeKey { get; set; }


        public List<BaseContactType> Contacts;


        public int CountryKey { get;set; }

        public string Description { get; set; }

        public string Extension { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public string PostalCode { get; set; }

        public string Shortname { get; set; }


    }
}
