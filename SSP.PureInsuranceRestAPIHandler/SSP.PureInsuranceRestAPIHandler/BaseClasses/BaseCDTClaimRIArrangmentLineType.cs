namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCdtClaimRIArrangmentLineType
    {
        public string AgreementCode { get; set; }
        public decimal DefaultSharePercent { get; set; }
        public int Grouping { get; set; }
        public decimal LineLimit { get; set; }
        public decimal LowerLimit { get; set; }
        public decimal NumberOfLines { get; set; }
        public decimal ParticipationPercent { get; set; }
        public int PartyKey { get; set; }
        public decimal Payment { get; set; }
        public int Priority { get; set; }
        public decimal Recovery { get; set; }
        public decimal Reserve { get; set; }
        public decimal Retained { get; set; }
        public int SAMStagingClaimRIArrangementKey { get; set; }
        public int SAMStagingClaimRIArrangementLineKey { get; set; }
        public decimal Salvage { get; set; }
        public decimal SumInsured { get; set; }
        public decimal ThisPayment { get; set; }
        public decimal ThisRecovery { get; set; }
        public decimal ThisReserve { get; set; }
        public decimal ThisSalvage { get; set; }
        public decimal ThisSharePercent { get; set; }
        public string TreatyCode { get; set; }
        public string Type { get; set; }

        public int TreatyID { get; set; }
    }
}
