using SSP.PureInsuranceRestAPIHandler.Enums;
using System;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimReceiptType
    {
        public int BaseClaimKey { get; set; }
        public int BaseClaimPerilKey { get; set; }

        public int ClaimId { get; set; }
        public int ClaimKey { get; set; }

        public int ClaimPerilId { get; set; }
        public int ClaimPerilKey { get; set; }
        public string ClaimVersionDescription { get; set; }

        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }

        public int ClaimReceiptId { get; set; }
        public int DoNotCreateClaimVersionOnSalvageReceipt { get; set; }
        public bool IsSalvageRecovery { get; set; }
        public int PartyKey { get; set; }

        public bool IsGetClaimReceiptTaxesType { get; set; }

        public bool IsAdvancedTaxSystemOptionOn { get; set; }

        public int VersionId { get; set; }

        public int TransactionTypeId { get; set; }

        public string TransactionTypeCode
        {
            get
            {
                string _transactionTypeCode = "C_RV";
                if (IsSalvageRecovery)
                {
                    _transactionTypeCode = "C_SA";
                }
                return _transactionTypeCode;
            }
        }

        public System.DateTime TransactionDate { get; set; } = DateTime.Now;

        public decimal ReceiptToLossXRate { get; set; }

        public int BaseCurrencyId { get; set; }

        public decimal BaseAmount { get; set; }

        public int AccountCurrencyId { get; set; }

        public decimal AccountAmount { get; set; }

        public int SystemCurrencyId { get; set; }

        public decimal SystemAmount { get; set; }

        public decimal CurrencyToBaseXRate { get; set; }

        public System.DateTime CurrencyToBaseDate { get; set; }

        public decimal AccountToBaseXRate { get; set; }

        public System.DateTime AccountToBaseDate { get; set; }

        public decimal SystemToBaseXRate { get; set; }

        public System.DateTime SystemToBaseDate { get; set; }

        public decimal TotalReceiptAmountGross
        {
            get
            {
                decimal _totalReceiptAmountGross = 0m;
                if (ReceiptItem is null)
                {
                    return _totalReceiptAmountGross;
                }

                foreach (BaseClaimReceiptItemType ClaimReceiptItem in ReceiptItem)
                {
                    if (ClaimReceiptItem.IsTPSalvageExcludeTax)
                    {
                        _totalReceiptAmountGross += ClaimReceiptItem.ReceiptAmount + ClaimReceiptItem.TotalTaxAmount;
                    }
                    else
                    {
                        _totalReceiptAmountGross += ClaimReceiptItem.ReceiptAmount;
                    }
                }
                return _totalReceiptAmountGross;
            }
        }

        public decimal TotalReceiptAmountNet
        {
            get
            {
                decimal _totalReceiptAmountNet = 0m;
                if (ReceiptItem is null)
                {
                    return _totalReceiptAmountNet;
                }

                foreach (BaseClaimReceiptItemType ClaimReceiptItem in ReceiptItem)
                {
                    if (ClaimReceiptItem.IsTPSalvageExcludeTax)
                    {
                        _totalReceiptAmountNet += ClaimReceiptItem.ReceiptAmount;
                    }
                    else
                    {
                        _totalReceiptAmountNet += ClaimReceiptItem.ReceiptAmount - ClaimReceiptItem.TotalTaxAmount;
                    }
                }

                return _totalReceiptAmountNet;
            }
        }

        public decimal TotalReceiptTaxAmount
        {
            get
            {
                decimal _totalReceiptTaxAmount = 0m;

                if (ReceiptItem is null)
                {
                    return _totalReceiptTaxAmount;
                }
                foreach (BaseClaimReceiptItemType ClaimReceiptItem in ReceiptItem)
                    _totalReceiptTaxAmount += ClaimReceiptItem.TotalTaxAmount;
                return _totalReceiptTaxAmount;
            }
        }


        public System.Collections.Generic.List<BaseInsurerType> Coinsurers { get; set; }

        public System.Collections.Generic.List<BaseInsurerType> Reinsurers { get; set; }

        public BaseAdditionalClaimRelatedDetails AdditionalDetails { get; set; }
        public BaseClaimPayeeType Payee { get; set; }
        public BaseClaimReceiptAdvancedTaxDetailsType AdvancedTaxDetails { get; set; }
        public System.Collections.Generic.List<BaseClaimReceiptItemType> ReceiptItem { get; set; }
        public ClaimReceiptPartyTypeType ReceiptPartyType { get; set; }
        public BaseClaimReceiptPayeeType ReceiptPayee { get; set; }

        public BaseReceiptCashListType ReceiptcashList { get; set; }
    }
}
