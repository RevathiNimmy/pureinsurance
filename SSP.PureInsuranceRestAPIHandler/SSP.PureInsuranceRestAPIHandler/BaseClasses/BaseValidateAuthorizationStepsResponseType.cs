namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseValidateAuthorizationStepsResponseType
    {
        //[DBCol("CurrentStep")]
        public int CurrentStep { get; set; }
        //[DBCol("IsLastStep")]
        public bool IsLastStep { get; set; }
        //[DBCol("PMUserGroup")]
        public string PMUserGroup { get; set; }
        //[DBCol("ValidationMessage")]
        public string ValidationMessage { get; set; }
        //[DBCol("JournalAmount")]
        public decimal JournalAmount { get; set; }
    }
}
