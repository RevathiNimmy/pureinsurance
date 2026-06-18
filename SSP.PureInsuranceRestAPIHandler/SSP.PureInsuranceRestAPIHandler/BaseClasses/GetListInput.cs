using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetListInput
    {
        public string BranchCode { get; set; }
        public STSListType ListType { get; set; }
        public string ListCode { get; set; }
        public bool ExcludeDeletedRecords { get; set; }
        public bool ExcludeEffectiveDate { get; set; }
        public string ParentFieldName { get; set; }
        public int ParentFieldValue { get; set; }
        public GetListFilterType FilterType { get; set; }
        public string FilterValue { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public int Version { get; set; }
        public Dictionary<string, string> SpuICCSParameters { get; set; }
        public string SpuICCSName { get; set; }
        public bool UseCache { get; set; }
        public System.Collections.ObjectModel.Collection<BaseListFilterOptions> WhereClause { get; set; }
    }
}
