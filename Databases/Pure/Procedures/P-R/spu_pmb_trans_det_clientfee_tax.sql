SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmb_trans_det_clientfee_tax'
GO


CREATE PROCEDURE spu_pmb_trans_det_clientfee_tax
    @transaction_export_folder_cnt int,
	@tax_group_code varchar(20),
    @tax_amount numeric(19, 4) 
AS


BEGIN

	DECLARE @transaction_export_detail_id   int,
    		@transaction_amount         	numeric(19, 4),
    		@transaction_ledger_code    	char(2),
    		@account_type_code      	varchar(11),
    		@mapping_code           	varchar(20),
    		@branch_id         		int,
    		@suspended	       		tinyint,
    		@release_to_income 		tinyint,
    		@transdetail_type_code 		varchar(20),
    		@accounting_basis 		int,
		@source_id			int,
		@spare				varchar(20),
		@tax_group_id		int

	/*DC220606 -start -get branch_id to get correct system option */
	SELECT 	@source_id = branch_id
	FROM	transaction_export_folder
	WHERE	transaction_export_folder_cnt = @transaction_export_folder_cnt

	SELECT	@accounting_basis = ISNULL(value, 0)
	FROM	system_options
	WHERE	option_number = 4012
	AND	branch_id = @source_id
	/*DC220606 -end */

	SELECT @transaction_ledger_code = 'NO'
	SELECT @transdetail_type_code = 'FEETAX'
	SELECT @spare = 'FEE TAX'

	/* Set new detail_id */

	SELECT  @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
	FROM    Transaction_Export_Detail
	WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt

	SELECT @tax_group_id = tax_group_id FROM tax_group WHERE code = @tax_group_code

	IF @transaction_export_detail_id IS NULL
    		SELECT @transaction_export_detail_id = 1

	INSERT INTO Transaction_Export_Detail 
		(
    	transaction_export_folder_cnt,
    	transaction_export_detail_id,
    	transaction_amount,
    	transaction_ledger_code,
    	account_type_code,
    	ceded_ref,
    	cover_share_percent,
    	sum_insured_total,
    	charges_total,
    	taxes_total,
    	recoveries_total,
    	commission_excluded,
    	withholding_tax_excluded,
    	mapping_code,
    	transaction_account_key,
    	spare,
    	suspended,
    	release_to_income,
    	release_account_code,
    	transdetail_type_code,
	tax_group_id
		)
	VALUES 
		(
    	@transaction_export_folder_cnt,
    	@transaction_export_detail_id,
    	@tax_amount * -1,
    	@transaction_ledger_code,
    	'TAXOUT',
    	NULL,
    	0,
    	0,
    	0,
    	0,
    	0,
    	0,
    	0,
    	'TAX' + @tax_group_code + 'OUT',
    	0,
	@spare,
	1,
	0,
	'',
	@transdetail_type_code,
	@tax_group_id
	) 

END
RETURN

Err_Add_Trans_ClientFee_Tax:
    BEGIN
        /* Delete all transactions for this folder */

        DELETE FROM Transaction_Export_Detail
            WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

        /* Delete the transactions folder record */
        DELETE FROM Transaction_Export_Folder
            WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

        RETURN
    END
GO
