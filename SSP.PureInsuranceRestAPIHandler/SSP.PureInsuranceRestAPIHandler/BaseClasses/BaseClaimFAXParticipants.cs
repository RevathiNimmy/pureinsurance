using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimFaxParticipants
    {
        public string AccountType { get; set; }
        public RowAction? ActionType { get; set; }
        public string AgreementCode { get; set; }
        public System.Collections.Generic.List<BaseBrokerParticipants> BrokerParticipants { get; set; }
        public float ParticpationPercentage { get; set; }
        public string PartyCode { get; set; }
        public int PartyKey { get; set; }
        public string PartyName { get; set; }
        public double PaymentToDate { get; set; }
        public int RIArrangementLineKey { get; set; }
        public double RecoverToDate { get; set; }
        public bool RecoverToDateSpecified { get; set; }
        public double ReserveToDate { get; set; }
        public double SumInsured { get; set; }
        public double ThisPayment { get; set; }
        public double ThisReserve { get; set; }
    }
}
