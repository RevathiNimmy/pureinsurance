using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseRiskRIArrangementLineType
    {
        public RowAction? ActionType { get; set; }
        public string AgreementCode { get; set; }
        public System.Collections.Generic.List<BaseBrokerParticipants> BrokerParticipants { get; set; }
        public bool CedePremiumOnly { get; set; }
        public double CommissionPercent { get; set; }
        public double CommissionTax { get; set; }
        public bool CommissionTaxSpecified { get; set; }
        public double CommissionValue { get; set; }
        public double DefaultSharePercent { get; set; }
        public System.Collections.Generic.List<BaseFaxParticipants> FAXParticipants { get; set; }
        public int Grouping { get; set; }
        public bool GroupingSpecified { get; set; }
        public bool IsCommissionModified { get; set; }
        public bool IsDomiciledForTax { get; set; }
        public bool IsObligatory { get; set; }
        public bool IsRIBroker { get; set; }
        public double LineLimit { get; set; }
        public double LowerLimit { get; set; }
        public bool LowerLimitSpecified { get; set; }
        public decimal NumberOfLines { get; set; }
        public double ParticipationPercent { get; set; }
        public bool ParticipationPercentSpecified { get; set; }
        public int PartyKey { get; set; }
        public bool PartyKeySpecified { get; set; }
        public double PremiumPercent { get; set; }
        public double PremiumTax { get; set; }
        public bool PremiumTaxSpecified { get; set; }
        public double PremiumValue { get; set; }
        public int Priority { get; set; }
        public int RIArrangementKey { get; set; }
        public int RIArrangementLineKey { get; set; }
        public string RIName { get; set; }
        public string RIPlacement { get; set; }
        public string ReinsuranceTypeCode { get; set; }
        public double Retained { get; set; }
        public bool RetainedSpecified { get; set; }
        public int RiOverrideReasonId { get; set; }
        public double SumInsured { get; set; }
        public double ThisSharePercent { get; set; }
        public string TreatyCode { get; set; }

        public int TreatyId { get; set; }
        public string Type { get; set; }
        public int TreatyTypeID { get; set; }
        public bool IsPortfolioTransferred { get; set; }
        public int TreatyPremiumType { get; set; }
        public string CalculationFactors { get; set; }
    
        public double FACPropPremiumPerc { get; set; }
        public int FACPremiumType { get; set; }
        public bool? ManuallyAdded { get; set; }
        public bool IsEditedDB { get; set; }
        public bool IsPremiumEdited { get; set; }
    }
}
