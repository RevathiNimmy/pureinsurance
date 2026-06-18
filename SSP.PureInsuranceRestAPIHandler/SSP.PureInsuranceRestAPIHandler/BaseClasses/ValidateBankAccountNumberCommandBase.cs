using System.ComponentModel;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.ValidateBankAccountNumber
{
    public class ValidateBankAccountNumberCommandBase : BaseRequestType
    {
        
        public string AccountNumber { get; set; }
        public string BIC { get; set; }
        
        //(1, int.MaxValue, ErrorMessage = "The BankCountryKey field is required")]
        
        public int BankCountryKey { get; set; }
        public string BankMediaCode { get; set; }
        
        //(1, int.MaxValue, ErrorMessage = "The BankMediaKey field is required")]
        
        public int BankMediaKey { get; set; }
        public string BankName { get; set; }
        public string IBAN { get; set; }
    }
}
