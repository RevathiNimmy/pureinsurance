namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseSelectedCashDepositType
    {
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }
        public string CashDepositRef { get; set; }
        public int CashDepositID { get; set; }
        public int PartyKey { get; set; }
        public int CashDepositAccountID { get; set; }
        public decimal TotalPremium { get; set; }
        public object[,] AllocationDetails { get; set; }
    }
}
