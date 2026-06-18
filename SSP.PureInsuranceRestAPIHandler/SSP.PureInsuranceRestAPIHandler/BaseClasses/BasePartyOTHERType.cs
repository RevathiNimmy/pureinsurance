using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePartyOtherType : BasePartyType
    {
        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string TypeCode { get; set; } = string.Empty;

        public string LicenseTypeCode { get; set; } = string.Empty;

        public string LicenseNumber { get; set; } = string.Empty;

        public System.DateTime DateOfBirth { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string DriverStatusCode { get; set; } = string.Empty;

        public string RegNumber { get; set; } = string.Empty;

        public System.Collections.Generic.List<BasePartyOtherTypeConviction> Conviction { get; set; } = null;

        public System.Collections.Generic.List<BaseConvictionType> Convictions { get; set; } = null;

        public System.Collections.Generic.List<BasePartyOtherTypeAccident> Accident { get; set; } = null;

        public bool ActiveIndicator { get; set; }

        public bool ActiveIndicatorSpecified { get; set; }

        public bool AfterHoursIndicator { get; set; }

        public bool AfterHoursIndicatorSpecified { get; set; }

        public int PriorityIndicator { get; set; }

        public bool PriorityIndicatorSpecified { get; set; }

        public System.Collections.Generic.List<BasePartyOtherTypeSupplierBusiness> SupplierBusiness { get; set; } = null;

        public System.Collections.Generic.List<BasePartyOtherTypeBranch> Branches { get; set; }

        public string IsTPASettleDirectly { get; set; } = string.Empty;

        public int LicenseTypeId { get; set; } = 0;
        public int DrivingStatusId { get; set; }
        public int GenderId { get; set; }
        public int TypeId { get; set; }

    }
}
