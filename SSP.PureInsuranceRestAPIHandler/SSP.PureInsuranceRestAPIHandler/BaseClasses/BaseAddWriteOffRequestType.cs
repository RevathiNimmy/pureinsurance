namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseAddWriteOffRequestType : BaseRequestType
    {

        ////(1, int.MaxValue, ErrorMessage = "The DocumentKey field is required")]
        //
        public int DocumentKey { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The AccountKey field is required")]
        //
        public int AccountKey { get; set; }

        ////(1, double.MaxValue, ErrorMessage = "The WriteOffAmount field is required")]
        //
        public decimal WriteOffAmount { get; set; }

        public int WriteOffAccKey { get; set; }

        public int TransDetailKey { get; set; } = 0;
    }
}
