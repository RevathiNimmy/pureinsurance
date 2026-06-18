namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUpdateTaskGroupTasksRequestTypeRow
    {
        public int DisplaySequence { get; set; }
        public bool DisplaySequenceSpecified { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The TaskCode field is required")]
        //
        public int TaskCode { get; set; }
    }
}
