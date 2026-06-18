namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetMidFileDetailsResponseTypeRowRow
    {
        public bool IsForeignReg { get; set; }
        public bool IsTradeReg { get; set; }
        public int MIDPolicyKey { get; set; }
        public int MIDVehicleKey { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public System.DateTime OffDate { get; set; }
        public System.DateTime OnDate { get; set; }
        public string Registration { get; set; }
        public string RejectErrorCodes { get; set; }
        public string RejectReference { get; set; }
        public string StatusCode { get; set; }
        public string UpdateType { get; set; }
    }
}
