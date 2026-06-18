
namespace SSP.PureInsuranceRestAPIHandler
{
    public class BuilderDetailsModel
    {
        public int GISScreenId { get; set; }
        public string DataModelCode { get; set; } = string.Empty;
        public int DataModelTypeId { get; set; }
        public int PartyTypeId { get; set; }
        public string PartyTypeCode { get; set; } = string.Empty;
    }
}
