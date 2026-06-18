namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class PolicyProcessV2CommandBaseResponse : BaseResponseType
    {
        public BasePolicyProcessV2ResponseTypeInsured Insured { get; set; }
        public BasePolicyProcessV2ResponseTypePolicy Policy { get; set; }
    }
}
