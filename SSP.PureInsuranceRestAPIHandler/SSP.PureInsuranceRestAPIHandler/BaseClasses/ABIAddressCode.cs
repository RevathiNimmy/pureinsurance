using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public static class AbiAddressCode
    {
        public static string GetABIAddressCode(AddressTypeType eAddressType)
        {
            switch (eAddressType)
            {
                case AddressTypeType.Item3131001:
                    return "3131 001";
                case AddressTypeType.Item3131002:
                    return "3131 002";
                case AddressTypeType.Item31310X9:
                    return "3131 0X9";
                case AddressTypeType.Item31310XR:
                    return "3131 0XR";
                case AddressTypeType.Item3131XBA:
                    return "3131 XBA";
                case AddressTypeType.Item3131XBI:
                    return "3131 XBI";
                case AddressTypeType.Item3131XCO:
                    return "3131 XCO";
                case AddressTypeType.Item3131XPR:
                    return "3131 XPR";
                case AddressTypeType.Item3131XRE:
                    return "3131 XRE";
                case AddressTypeType.Item3131XSA:
                    return "3131 XSA";
                case AddressTypeType.Item3131ECK:
                    return "3131 ECK";
                default:
                    return "UNKNOWN Address Type Enum";
            }
        }

    }
}
