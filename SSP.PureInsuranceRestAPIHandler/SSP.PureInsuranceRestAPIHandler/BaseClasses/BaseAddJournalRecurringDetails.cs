namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseAddJournalRecurringDetails
    {
        public int Occurs { get; set; }
        public int PerPeriodOnDay { get; set; }
        public bool PerPeriodOnDaySpecified { get; set; }
        public int PerPeriodOnMonth { get; set; }
        public bool PerPeriodOnMonthSpecified { get; set; }
        public int PerQuarterOnDay { get; set; }
        public bool PerQuarterOnDaySpecified { get; set; }
    }
}