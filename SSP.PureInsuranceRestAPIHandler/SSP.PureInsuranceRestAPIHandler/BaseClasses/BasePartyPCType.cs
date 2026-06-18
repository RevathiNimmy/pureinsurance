using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePartyPCType : BasePartyType
    {
        public string Surname { get; set; } = string.Empty;
        public string Forename { get; set; } = string.Empty;
        public System.DateTime DateOfBirth { get; set; }
        public bool DateOfBirthSpecified { get; set; }
        public string Title { get; set; } = string.Empty;
        public MaritalStatusCodeType MaritalStatusCode { get; set; }
        public bool MaritalStatusCodeSpecified { get; set; }
        public string GenderCode { get; set; } = string.Empty;
        public string Initials { get; set; } = string.Empty;
        public string OccupationCode { get; set; } = string.Empty;
        public string EmployersBusinessCode { get; set; } = string.Empty;
        public EmploymentStatusCodeType EmploymentStatusCode { get; set; }
        public bool EmploymentStatusCodeSpecified { get; set; }
        public string AlternativeId { get; set; } = string.Empty;
        public BaseClientSharedDataType ClientDetail { get; set; } = null;
        public string TradingName { get; set; } = string.Empty;
        public string SecOccupationCode { get; set; } = string.Empty;
        public string SecEmployersBusinessCode { get; set; } = string.Empty;
        public EmploymentStatusCodeType SecEmploymentStatusCode { get; set; }
        public bool SecEmploymentStatusCodeSpecified { get; set; }
        public string NationalityCode { get; set; } = string.Empty;
        public string AccommodationCode { get; set; } = string.Empty;
        public System.Collections.Generic.List<BasePartyPCTypeLifestyle> Lifestyle { get; set; } = null;
        public string Salutation { get; set; } = string.Empty;
        public bool TPS { get; set; }
        public bool TPSSpecified { get; set; }
        public bool MPS { get; set; }
        public bool MPSSpecified { get; set; }
        public bool EMPS { get; set; }
        public bool EMPSSpecified { get; set; }
        public string Source { get; set; } = string.Empty;
        public bool PetOwner { get; set; }
        public bool PetOwnerSpecified { get; set; }
    }
}
