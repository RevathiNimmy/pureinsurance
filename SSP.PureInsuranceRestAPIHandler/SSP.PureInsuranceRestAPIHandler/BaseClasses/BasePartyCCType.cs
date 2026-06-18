namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePartyCCType : BasePartyType
    {
        public string CompanyName { get; set; } = string.Empty;
        public string BusinessCode { get; set; } = string.Empty;
        public string MainContact { get; set; } = string.Empty;
        public int NumberOfOffices { get; set; }
        public bool NumberOfOfficesSpecified { get; set; }
        public string NumberOfEmployees { get; set; } = string.Empty;
        public string TradeCode { get; set; } = string.Empty;
        public BaseClientSharedDataType ClientDetail { get; set; }
        public string CompanyReg { get; set; } = string.Empty;
        public string SICCode { get; set; } = string.Empty;
        public System.DateTime TradingSince { get; set; }
        public bool TradingSinceSpecified { get; set; }
        public decimal WageRoll { get; set; }
        public bool WageRollSpecified { get; set; }
        public string TurnoverCode { get; set; } = string.Empty;
        public System.DateTime FinancialYear { get; set; }
        public bool FinancialYearSpecified { get; set; }
        public string Salutation { get; set; } = string.Empty;
        public bool TPS { get; set; }
        public bool TPSSpecified { get; set; }
        public bool MPS { get; set; }
        public bool MPSSpecified { get; set; }
        public bool EMPS { get; set; }
        public bool EMPSSpecified { get; set; }
        public string Source { get; set; } = string.Empty;
        public string AlternativeId { get; set; } = string.Empty;
    }
}
