namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUpdateCoinsuranceValuesRequestTypeRow
    {
        public string ArrangementRef { get; set; }
        public int CoInsurerKey { get; set; }
        public double CommissionPerc { get; set; }
        public double SharePerc { get; set; }
    }
}
