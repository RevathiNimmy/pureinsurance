using System.Xml.Serialization;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetUserAuthorityValueResponseType : BaseResponseType
    {
        public string UserAuthorityValue { get; set; }

        public int UserAuthorityOptionalValue1 { get; set; }

        [XmlIgnore]
        public bool UserAuthorityOptionalValue1Specified { get; set; }

        public double UserAuthorityOptionalValue2 { get; set; }

        [XmlIgnore]
        public bool UserAuthorityOptionalValue2Specified { get; set; }

        public string UserAuthorityOptionalValue3 { get; set; }

        [XmlIgnore]
        public bool UserAuthorityOptionalValue3Specified { get; set; }

        public double UserAuthorityOptionalValue3_BaseAmount { get; set; }

        [XmlIgnore]
        public bool UserAuthorityOptionalValue3_BaseAmountSpecified { get; set; }
    }
}
