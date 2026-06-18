namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class SAMErrorInvalidData
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public string FieldName { get; set; }

        public string SuppliedValue { get; set; }
        public string Reason { get; set; }
    }
}