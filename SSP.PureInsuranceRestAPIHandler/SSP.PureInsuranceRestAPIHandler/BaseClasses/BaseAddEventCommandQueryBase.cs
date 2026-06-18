namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseAddEventCommandQueryBase : BaseRequestType
    {
        public int ClaimKey { get; set; }
        public string Document_Path { get; set; }
        public System.DateTime EventDate { get; set; }
        public int EventLogSubjectKey { get; set; }
        public int EventTypeKey { get; set; }
        public int InsuranceFileKey { get; set; }
        public int InsuranceFolderKey { get; set; }
        public bool IsManualDescription { get; set; }
        public int PartyKey { get; set; }
        public string Priority { get; set; }
        public string RtfText { get; set; }
        public bool StatusKey { get; set; }
        public string UserName { get; set; }
    }
}
