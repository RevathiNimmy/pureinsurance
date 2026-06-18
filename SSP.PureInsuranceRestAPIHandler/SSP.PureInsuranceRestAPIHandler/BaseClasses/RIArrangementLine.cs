namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RIArrangementLine
    {
        public string RiName { get; set; } = string.Empty;
        public double DefaultSharePercent { get; set; }
        public double ThisSharePercent { get; set; }
        public decimal SumInsured { get; set; }
        public decimal Reserve { get; set; }
        public decimal ThisReserve { get; set; }
        public decimal Payment { get; set; }
        public decimal ThisPayment { get; set; }
        public decimal Balance { get; set; }
        public string AgreementCode { get; set; } = string.Empty;
        public int RiArrangementLineId { get; set; }
        public string Type { get; set; } = string.Empty;
        public int TreatyId { get; set; }
        public int PartyCnt { get; set; }
        public int XolArrangementId { get; set; }
        public int Priority { get; set; }
        public decimal NumberofLines { get; set; }
        public decimal LineLimit { get; set; }
        public int Layer { get; set; }
        public int CatastropheCodeId { get; set; }
        public string CatastropheCode { get; set; } = string.Empty;
        public int RIModelId { get; set; }
        public int NextLayerModelId { get; set; }
        public decimal NextLayerTriggerAmount { get; set; }
    }

}
