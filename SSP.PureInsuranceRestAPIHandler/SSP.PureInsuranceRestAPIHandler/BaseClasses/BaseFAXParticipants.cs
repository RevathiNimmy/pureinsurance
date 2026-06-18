using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseFaxParticipants
    {
        public string AccountType { get; set; }
        public RowAction? ActionType { get; set; }
        public string AgreementCode { get; set; }
        public System.Collections.Generic.List<BaseBrokerParticipants> BrokerParticipants { get; set; }
        public double CommissionPercent { get; set; }
        public double CommissionTax { get; set; }
        public bool CommissionTaxSpecified { get; set; }
        public double CommissionValue { get; set; }
        public float ParticpationPercentage { get; set; }
        public string PartyCode { get; set; }
        public int PartyKey { get; set; }
        public string PartyName { get; set; }
        public double PremiumTax { get; set; }
        public bool PremiumTaxSpecified { get; set; }
        public double PremiumValue { get; set; }
        public int RIArrangementLineKey { get; set; }
        public double SumInsured { get; set; }
    }
}
