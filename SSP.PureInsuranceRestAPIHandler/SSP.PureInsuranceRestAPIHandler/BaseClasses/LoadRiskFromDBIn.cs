namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class LoadRiskFromDBIn
    {
        //[DBCol("XMLDataSetDef")]
        public string XMLDataSetDef { get; set; }

        //[DBCol("XMLDataSet")]
        public string XMLDataSet { get; set; }

        //[DBCol("DataModelCode")]
        public string DataModelCode { get; set; }

        //[DBCol("RiskID")]
        public int RiskId { get; set; }

        //[DBCol("InsuranceFileCnt")]
        public int InsuranceFileCount { get; set; }

        //[DBCol("InsuranceFolderCnt")]
        public int InsuranceFolderCount { get; set; }
    }
}
