namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CaseLinkUnlinkCommandBaseResponse : BaseResponseType
    {
        public BaseGeneralWarningResponseType[] Warnings { get; set; } = new BaseGeneralWarningResponseType[0];
    }
}
