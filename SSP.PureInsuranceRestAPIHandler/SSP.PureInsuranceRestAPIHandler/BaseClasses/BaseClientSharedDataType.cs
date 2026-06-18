
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public partial class BaseClientSharedDataType
    {
        public string ShortName { get; set; } = string.Empty;
        public string ServiceLevelCode { get; set; } = string.Empty;
        public string AreaCode { get; set; } = string.Empty;
        public int LeadAgentKey { get; set; }
        public bool LeadAgentKeySpecified { get; set; }
        public bool IsProspect { get; set; }
        public bool IsProspectSpecified { get; set; }
        public bool IsAgent { get; set; }
        public bool IsAgentSpecified { get; set; }
        public string CorrespondenceCode { get; set; } = string.Empty;
        public string PaymentCode { get; set; } = string.Empty;
        public string ReminderCode { get; set; } = string.Empty;
        public string PaymentTermCode { get; set; } = string.Empty;
        public string RenewalStopCode { get; set; } = string.Empty;
        public string LoyaltyNumber { get; set; } = string.Empty;
        public string SeasonalGiftCode { get; set; } = string.Empty;
        public System.Collections.Generic.List<BaseAssociateType> Associates { get; set; }
        public System.Collections.Generic.List<BaseConvictionType> Convictions { get; set; }
        public decimal CountyCourtJudgments { get; set; }
        public bool CountyCourtJudgmentsSpecified { get; set; }
        public System.Collections.Generic.List<BaseClientSharedDataTypeLoyaltyScheme> LoyaltyScheme { get; set; }
        public string AgentReference { get; set; } = string.Empty;
        public int CurrentIntermediaryKey { get; set; }
        public bool CurrentIntermediaryKeySpecified { get; set; }
        public string StrengthCode { get; set; } = string.Empty;
        public string StatusCode { get; set; } = string.Empty;
        public int PreviousInsurerKey { get; set; }
        public bool PreviousInsurerKeySpecified { get; set; }
        public int PreviousBrokerKey { get; set; }
        public bool PreviousBrokerKeySpecified { get; set; }
        public System.Collections.Generic.List<BaseClientSharedDataTypeProspectPolicies> ProspectPolicies { get; set; }
        public string CurrentIntermediaryName { get; set; } = string.Empty;
        public string LeadAgentCode { get; set; } = string.Empty;
        public string LeadAgentName { get; set; } = string.Empty;
        public string PreviousInsurerCode { get; set; } = string.Empty;
        public string PreviousInsurerName { get; set; } = string.Empty;
        public string PreviousBrokerCode { get; set; } = string.Empty;
        public string PreviousBrokerName { get; set; } = string.Empty;
        public decimal AccountBalance { get; set; }
        public bool AccountBalanceSpecified { get; set; }
        public decimal YearToDateTurnover { get; set; }
        public bool YearToDateTurnoverSpecified { get; set; }
        public decimal LastYearTurnover { get; set; }
        public bool LastYearTurnoverSpecified { get; set; }
        public string ResolvedName { get; set; } = string.Empty;
        public string BlacklistReasonCode { get; set; } = string.Empty;
    }

}
