namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class SaveToDB
    {
        // In ONLY Parameters
        public string DataModelCode { get; set; }

        public string BusinessTypeCode { get; set; }

        // In/Outs Parameters
        public string XMLDataset { get; set; }
    }
}
