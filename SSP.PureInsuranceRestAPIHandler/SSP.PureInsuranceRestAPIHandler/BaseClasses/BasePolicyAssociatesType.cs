using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePolicyAssociatesType
    {
        public RowAction ActionType { get; set; }
        public string AssociationDetail { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The AssociationTypeKey field is required")]
        //
        public int AssociationTypeKey { get; set; }

        public System.DateTime DateAttached { get; set; }
        public bool DateAttachedSpecified { get; set; }
        public System.DateTime DateRemoved { get; set; }
        public bool DateRemovedSpecified { get; set; }
        public int InsuranceFileAssociatesKey { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The InsuranceFileKey field is required")]
        //
        public int InsuranceFileKey { get; set; }
        public int InsuranceFolderCnt { get; set; }
        public bool IsAddUnConfirmed { get; set; }
        public bool IsDelUnConfirmed { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDeletedSpecified { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The PartyKey field is required")]
        //
        public int PartyKey { get; set; }
        public int RowKey { get; set; }
    }
}
