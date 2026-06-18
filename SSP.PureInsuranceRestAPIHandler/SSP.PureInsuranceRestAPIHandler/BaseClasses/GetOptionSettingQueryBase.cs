using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetOptionSettingQueryBase : BaseRequestType
    {
        public int OptionNumber { get; set; }
        public OptionType? OptionType { get; set; }
    }
}
