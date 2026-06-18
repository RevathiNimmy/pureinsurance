SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_ACT_Trans_Detail_Rounding
GO


CREATE PROCEDURE spu_ACT_Trans_Detail_Rounding
	@nTransactionExportFolderCnt	INT,
	@nDocumentId					INT,
	@crAmount						NUMERIC(19,4)
AS
-- should only be called when exchange rate = 1

DECLARE	@nReturn AS INT = 0 -- Maps to PMTure(1) , PMFalse(0) no rounding has been done fixed,continue normal processing for rounding
DECLARE @InsuranceFilecnt INTEGER
DECLARE @TransType INTEGER
DECLARE @InsFileType INTEGER
DECLARE @CurrencyCode CHAR(10)
DECLARE @CurrencyID SMALLINT
DECLARE @RoundToPlaces TINYINT

SELECT @InsuranceFilecnt  = Insurance_File_cnt, @CurrencyCode = currency_code 
FROM Transaction_Export_Folder WHERE transaction_export_folder_cnt = @nTransactionExportFolderCnt

SELECT @TransType = last_trans_type_id FROM Insurance_File_System WHERE insurance_file_cnt = @InsuranceFilecnt
SELECT @InsFileType = insurance_file_type_id FROM insurance_file WHERE insurance_file_cnt = @InsuranceFilecnt
SELECT @CurrencyID = currency_id, @RoundToPlaces = round_to_places FROM Currency WHERE code = @CurrencyCode

CREATE TABLE #spu_pmb_select_transaction_data
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[transaction_amount] [numeric](19, 4) NULL	
)

INSERT INTO	#spu_pmb_select_transaction_data
SELECT 
(Case	WHEN @TransType IN(22) AND @InsFileType IN(8)
		THEN ROUND(SUM(transaction_amount * -1), 2)
		ELSE ROUND(SUM(transaction_amount), 2) END) transaction_amount
FROM    Transaction_Export_Folder F
JOIN    Transaction_Export_Detail D ON D.transaction_export_folder_cnt = F.transaction_export_folder_cnt
WHERE   F.transaction_export_folder_cnt = @nTransactionExportFolderCnt
GROUP BY
document_ref, debit_credit, transaction_type_code, document_date, accounting_date,
document_comment, currency_code, business_type_code, insurance_ref, product_code,
branch_code, agent_shortname, insurance_holder_shortname, effective_date,
created_by_user_id, source_id, is_payable_by_instalments, insurance_holder_id,
agent_id, posting_period_number, insurance_file_cnt, reason, transaction_ledger_code,
account_type_code, mapping_code, transaction_account_key, spare, real_insurance_file_cnt,
purchase_order_no, purchase_invoice_no, F.underwriting_year_id,suspended,release_to_income,
release_account_code, transdetail_type_code, tax_group_id, tax_band_id,terms_of_payment_id,
payment_due_date,manually_released,released_on_full_settlement,released_for_whole_posting,released_on_policy_effective , d.fee_type
ORDER BY
MAX(transaction_export_detail_id)




DECLARE @RoundingAmount AS NUMERIC (19, 4)
SELECT @RoundingAmount = SUM(ISNULL(transaction_amount,0)) FROM #spu_pmb_select_transaction_data

-- As per sp spu_pmb_select_transaction_data @RoundingAmount = 0 but we are still getting a rounding via code @crAmount
-- Not handling case where there is rounding as per sp spu_pmb_select_transaction_data and additional rounding via code @crAmount
IF ((ABS(@crAmount) != ABS(@RoundingAmount)) AND (ABS(@RoundingAmount) = 0))
BEGIN

	-- Have only seen single Transdetail for a document_id affected, if more are found then this can be put in a cursor. 
	DECLARE @transdetail_id INT
	DECLARE @amount NUMERIC(19,4)
	DECLARE @amountUpdated NUMERIC(19,4)
	DECLARE @base_amount_unrounded NUMERIC(19,4)
	DECLARE @currency_amount NUMERIC(19,4)

	SELECT
	@transdetail_id = transdetail_id,
	@amount = amount,
	@base_amount_unrounded = base_amount_unrounded,
	@currency_amount = currency_amount
	FROM TransDetail
	WHERE document_id = @nDocumentId
	AND ABS(ISNULL(base_amount_unrounded,0)) > 0
	AND ROUND(ISNULL(AMOUNT,0),@RoundToPlaces) != ROUND(ISNULL(base_amount_unrounded,0),@RoundToPlaces)

	SET @amountUpdated = ROUND(@base_amount_unrounded,@RoundToPlaces)

	UPDATE Transdetail SET
	amount = @amountUpdated,
	Currency_amount = @amountUpdated,
	outstanding_amount =			CASE
										WHEN outstanding_amount = @amount THEN @amountUpdated
										ELSE outstanding_amount
									END,
	outstanding_currency_amount =	CASE
										WHEN outstanding_currency_amount = @amount THEN @amountUpdated
										ELSE outstanding_currency_amount
									END,
	outstanding_account_amount =	CASE
										WHEN outstanding_account_amount = @amount THEN @amountUpdated
										ELSE outstanding_account_amount
									END,
	outstanding_system_amount =		CASE
										WHEN outstanding_system_amount = @amount THEN @amountUpdated
										ELSE outstanding_system_amount
									END
	WHERE Transdetail_id = @transdetail_id

	SET @nReturn = 1
END


SELECT @nReturn



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

