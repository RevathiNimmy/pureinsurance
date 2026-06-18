EXECUTE DDLDropProcedure 'spu_Copy_transExport_for_Reversal_By_DocumentRef'
GO
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
CREATE PROCEDURE spu_Copy_transExport_for_Reversal_By_DocumentRef
@DocumentRef  VARCHAR(50),
@transactiontExportFolderCnt INT
AS
Declare @oldtransExportCnt INT
SELECT @oldtransExportCnt=Transaction_Export_Folder_cnt from Transaction_Export_Folder WHERE document_ref=@DocumentRef

INSERT INTO Transaction_Export_detail
(transaction_export_folder_cnt,
transaction_export_detail_id,
transaction_amount,
transaction_ledger_code,
account_type_code,
transaction_account_key,
ceded_ref,
cover_share_percent,
sum_insured_total,
charges_total,
taxes_total,
recoveries_total,
commission_excluded,
withholding_tax_excluded,
mapping_code,
spare,
purchase_order_no,
purchase_invoice_no,
base_transaction_amount,
base_taxes_amount,
suspended,
release_to_income,
release_account_code,
transdetail_type_code,
tax_group_id,
tax_band_id,
manually_released,
released_on_full_settlement,
released_for_whole_posting,
released_on_policy_effective)
SELECT 
@transactiontExportFolderCnt,
transaction_export_detail_id,
-transaction_amount,
transaction_ledger_code,
account_type_code,
transaction_account_key,
ceded_ref,
cover_share_percent,
sum_insured_total,
-charges_total,
-taxes_total,
recoveries_total,
commission_excluded,
withholding_tax_excluded,
mapping_code,
spare,
purchase_order_no,
purchase_invoice_no,
-base_transaction_amount,
-base_taxes_amount,
suspended,
release_to_income,
release_account_code,
transdetail_type_code,
tax_group_id,
tax_band_id,
manually_released,
released_on_full_settlement,
released_for_whole_posting,
released_on_policy_effective FROM Transaction_Export_Detail WHERE transaction_export_folder_cnt= @oldtransExportCnt