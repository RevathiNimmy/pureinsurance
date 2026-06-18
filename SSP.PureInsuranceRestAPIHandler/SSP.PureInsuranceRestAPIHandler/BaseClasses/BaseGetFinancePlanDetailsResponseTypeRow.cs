namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetFinancePlanDetailsResponseTypeRow
    {
        public double Amount { get; set; }
        public System.DateTime DueDate { get; set; }
        public int InstalmentNumber { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public bool PaymentDateSpecified { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }
}
