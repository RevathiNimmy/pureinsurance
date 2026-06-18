namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BindQuoteCommandResponse : BindQuoteCommandBaseResponse
    {
        public new BaseGeneralWarningResponseType[] Warnings { get; set; } = new BaseGeneralWarningResponseType[0];
    }
}
