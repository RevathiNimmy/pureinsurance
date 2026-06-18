namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public  class BaseCoInsurerType
    {
        public string ParticipantCode { get; set; } = "";

        public int InsurerKey { get; set; } = 0;

        public string ArrangementRef { get; set; } = "";

        public double SharePerc { get; set; } = 0d;

        public double CommPerc { get; set; } = 0d;

    }
}
