namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseFindInsuranceFileRequestType : BaseRequestType
    {

        public string ClientShortName { get; set; }
        public int CoverNoteSheetNumber { get; set; }
        public bool CoverNoteSheetNumberSpecified { get; set; }
        public System.DateTime InForceFrom { get; set; }
        public bool InForceFromSpecified { get; set; }
        public System.DateTime InForceTo { get; set; }
        public bool InForceToSpecified { get; set; }
        public string InsuranceRef { get; set; }

        public System.DateTime LossDate { get; set; }
        public int MaxRowsToFetch { get; set; }
        public bool MaxRowsToFetchSpecified { get; set; }
        public string PostCode { get; set; }
        public bool RetrieveAssociates { get; set; }
        public string RiskIndex { get; set; }
        public System.DateTime SearchDate { get; set; }
        public int AgentKey { get; set; }
        public int SourceId { get; set; }
    }
}
