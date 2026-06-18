using SSP.PureInsuranceRestAPIHandler.BaseClasses;

namespace PureInsurance.REST.Core.Application.Core.Queries.GetDatasetSchema
{
    public class GetDatasetSchemaQueryBase : BaseRequestType
    {
        public string DataModelCode { get; set; }
    }
}
