using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.Constants
{
    public static class LockNameString
    {
        public static string LockNameProperty(LockName lockName)
        {
            switch (lockName)
            {
                case LockName.InsuranceFileCnt:
                    return "insurance_file_cnt";
                case LockName.InsuranceFolderCnt:
                    return "insurance_folder_cnt";
                case LockName.PartyCnt:
                    return "party_cnt";
                case LockName.ClaimId:
                    return "claim_id";
                case LockName.TaskInstanceCnt:
                    return "pmwrk_task_instance_cnt";
                case LockName.UserGroupCnt:
                    return "pmuser_group_id";
                case LockName.TaskGroupCnt:
                    return "pmwrk_task_group_id";
                case LockName.CoverNoteBookId:
                    return "cover_note_book_id";
                case LockName.BGId:
                    return "bg_id";
                case LockName.RenewalProcess:
                    return "renewal_status_cnt";
                case LockName.TransDetailKey:
                    return "transdetail_id";
                case LockName.CashListItemID:
                    return "cashlistitem_id";
                case LockName.ClaimPaymentCnt:
                    return "claim_payment_id";
                case LockName.RiskKey:
                    return "risk_cnt";
                case LockName.CashDepositKey:
                    return "cashdeposit_id";
                case LockName.ClaimPayment:
                    return "claim_payment";
                case LockName.PFPremFinanceCnt:
                    return "pfprem_finance_cnt";
                default:
                    return string.Empty;
            }
        }
    }
}

