using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetProductsForUserBranchResponseTypeProductsRow
    {
        //[DBCol("Code")]
        public string ProductCode { get; set; }
        //[DBCol("description")]
        public string ProductDescription { get; set; }
        //[DBCol("Product_id")]
        public int ProductKey { get; set; }
        public System.Collections.Generic.List<BaseBranchType> Branches { get; set; }
    }
}
