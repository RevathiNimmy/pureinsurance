
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetHeaderAndRiskFeesByKeyResponseTypeRow
    {
        
        public string FeeName { get; set; }

        
        public string CurrencyCode { get; set; }

      
        public string AppliedTo { get; set; }


        public double Premium { get; set; }

        public double Rate { get; set; }


        public double FeeAmount { get; set; }

        public double TaxAmount { get; set; }

        public double TotalAmount { get; set; }

    
        public string TaxGroup { get; set; }

   
        public short IncludeInInstallment { get; set; }

     
        public short SpreadAcrossInstallment { get; set; }

    
        public bool IsValue { get; set; }

      
        public int RiskFeeKey { get; set; }

        
        public short CalculationBasis { get; set; }

        public int DoPaymentTermsId { get; set; }

     
        public int MakeLiveOptionsId { get; set; }

     
        public short IsProrated { get; set; }

       
        public double ProRataRate { get; set; }
    }
}
