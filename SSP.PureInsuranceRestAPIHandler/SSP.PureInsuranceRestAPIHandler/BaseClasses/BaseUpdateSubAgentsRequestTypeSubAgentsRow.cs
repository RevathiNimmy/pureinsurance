namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUpdateSubAgentsRequestTypeSubAgentsRow
    {
        public double Amount { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The PartyKey field is required")]
        //
        public int PartyKey { get; set; }
        public double Percentage { get; set; }
    }
}
