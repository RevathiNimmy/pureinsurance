using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public partial class BasePartyPCTypeLifestyle
    {
        public int LifestyleKey { get; set; }
        public string Name { get; set; } = string.Empty;
        public System.DateTime DateOfBirth { get; set; }
        public bool DateOfBirthSpecified { get; set; }
        public string CategoryCode { get; set; } = string.Empty;
        public GenderCodeType GenderCode { get; set; }
        public bool GenderCodeSpecified { get; set; }
        public string OccupationCode { get; set; } = string.Empty;
        public string SecOccupationCode { get; set; } = string.Empty;
        public bool Smoker { get; set; }
        public bool SmokerSpecified { get; set; }
        public int ProcessingStatus { get; set; }
    }
}
