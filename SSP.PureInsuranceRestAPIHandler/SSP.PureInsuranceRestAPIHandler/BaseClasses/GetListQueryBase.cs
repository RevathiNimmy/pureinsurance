using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetListQueryBase : BaseRequestType
    {
        public System.DateTime EffectiveDate { get; set; }

        public bool EffectiveDateSpecified { get; set; }
        public bool ExcludeDeletedRecords { get; set; }
        public bool ExcludeEffectiveDate { get; set; }
        public GetListFilterType FilterType { get; set; }
        public string FilterValue { get; set; }

        public string ListCode { get; set; }

        public STSListType ListType { get; set; }
        public string ParentFieldName { get; set; }
        public int ParentFieldValue { get; set; }

        public bool ParentFieldValueSpecified { get; set; }

        public string SpuICCSName { get; set; }
        public System.Collections.Generic.List<IccsParam> SpuICCSParameters { get; set; }
        public bool UseCache { get; set; }
        public int Version { get; set; }

        public bool VersionSpecified { get; set; }
        public System.Collections.Generic.List<string> WhereColumnName { get; set; }
        public System.Collections.Generic.List<string> WhereOperator { get; set; }
        public System.Collections.Generic.List<string> WhereValue { get; set; }
    }
}
