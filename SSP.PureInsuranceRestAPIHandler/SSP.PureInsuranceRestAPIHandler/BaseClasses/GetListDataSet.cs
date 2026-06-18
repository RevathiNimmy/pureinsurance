namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetListDataSet
    {
        //[DBCol("id")]
        public int Id { get; set; }

        //[DBCol("caption")]
        public string Caption { get; set; }

        //[DBCol("code")]
        public string Code { get; set; }

        //[DBCol("effective_date")]
        public System.DateTime EffectiveDate { get; set; }

        //[DBCol("is_deleted")]
        public bool IsDeleted { get; set; }

        //[DBCol("parent_id")]
        public int ParentId { get; set; }

        //[DBCol("Description")]
        public string Description { get; set; }

        //[DBCol("IsDefault")]
        public bool IsDefault { get; set; }

        //[DBCol("Key")]
        public int Key { get; set; }

        //[DBCol("ParentKey")]
        public int ParentKey { get; set; }

        //[DBCol("ParentKeySpecified")]
        public bool ParentKeySpecified { get; set; }
    }
}
