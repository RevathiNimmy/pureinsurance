using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimRiskRIArrangementLineType
    {
        public RowAction? ActionType { get; set; }
        public string AgreementCode { get; set; }
        public double Balance { get; set; }
        public bool BalanceSpecified { get; set; }
        public List<BaseBrokerParticipants> BrokerParticipants { get; set; }
        public bool CedePremiumOnly { get; set; }
        public double DefaultSharePercent { get; set; }
        public List<BaseClaimFaxParticipants> FAXParticipants { get; set; }
        public int Grouping { get; set; }
        public bool GroupingSpecified { get; set; }
        public double Incurred { get; set; }
        public bool IncurredSpecified { get; set; }
        public bool IsDomiciledForTax { get; set; }
        public bool IsDomiciledForTaxSpecified { get; set; }
        public bool IsObligatory { get; set; }
        public bool IsRIBroker { get; set; }
        public bool IsRIBrokerSpecified { get; set; }
        public double LineLimit { get; set; }
        public double LowerLimit { get; set; }
        public bool LowerLimitSpecified { get; set; }
        public decimal NumberOfLines { get; set; }
        public int PartyKey { get; set; }
        public bool PartyKeySpecified { get; set; }
        public double PaymentToDate { get; set; }
        public int Priority { get; set; }

        //[Range(1, int.MaxValue, ErrorMessage = "The RIArrangementKey field is required")]
        //[DefaultValue(0)]
        public int RIArrangementKey { get; set; }
        public int RIArrangementLineKey { get; set; }
        public string RIName { get; set; }
        public string RIPlacement { get; set; }
        public double RecoverToDate { get; set; }
        public bool RecoverToDateSpecified { get; set; }
        public string ReinsuranceTypeCode { get; set; }
        public double ReserveToDate { get; set; }
        public double Retained { get; set; }
        public bool RetainedSpecified { get; set; }
        public double SumInsured { get; set; }
        public double ThisPayment { get; set; }
        public double ThisReserve { get; set; }
        public double ThisSharePercent { get; set; }
        public string TreatyCode { get; set; }
        public int TreatyID { get; set; }

        public string Type { get; set; }
    }
}
