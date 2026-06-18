
namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ValidatePartyBank
    {
        public bool IsBank { get; set; }
        public bool IsNumberExisting { get; set; }
        public bool IsValidTransaction { get; set; }
        public bool IsValidTransactionParty { get; set; }
    }
}
