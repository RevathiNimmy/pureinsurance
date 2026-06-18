SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Select_Trans_For_Allocation_FilterBy_Ins_File'
GO
CREATE PROCEDURE spu_ACT_Select_Trans_For_Allocation_FilterBy_Ins_File
	@insurance_file_cnt INT,
	@account_code VARCHAR(20),
    	@company_id INT=NULL,
	@account_id INT=0 OUTPUT,
	@base_balance MONEY=0 OUTPUT,
	@base_currency_count SMALLINT=0 OUTPUT,
	@account_balance MONEY=0 OUTPUT,
	@account_currency_count SMALLINT=0 OUTPUT
AS
BEGIN
	DECLARE @TypeOfRates TINYINT

    	-- Get account id
    	SELECT @account_id = account_id
    	FROM account
	WHERE short_code = @account_code

	EXEC spu_ACT_GetTypeOfRates @TypeOfRates

	IF @TypeOfRates = 1
	BEGIN
		SELECT @company_id = NULL
	END

	--/* First check whether there are multiple base currencies on the account
	--   if there are then auto-allocation is not allowed  */

	--SELECT 	@base_currency_count=COUNT(DISTINCT amount_currency_id)
	--FROM 	transdetail
	--WHERE 	account_id = @account_id
	--AND 	company_id=ISNULL(@company_id,company_id)
	--AND 	outstanding_amount<>0
	--GROUP BY amount_currency_id

	--IF @base_currency_count=1 BEGIN
	--	/* Now check whether there are multiple account currencies on the account
	--	   if there are then auto-allocation is not allowed  */

	--	SELECT  @base_balance=SUM(outstanding_amount),
	--		@account_balance=SUM(outstanding_account_amount)
	--	FROM	TransDetail
	--	INNER JOIN Document d ON TransDetail.document_id = d.document_id
	--	WHERE 	d.insurance_file_cnt = @insurance_file_cnt 	
	--	AND 	account_id = @account_id
	--	AND 	TransDetail.company_id=ISNULL(@company_id,TransDetail.company_id)

	--	SELECT 	@account_currency_count=COUNT(DISTINCT account_currency_id)
	--	FROM 	transdetail
	--	INNER JOIN Document d ON TransDetail.document_id = d.document_id
	--	WHERE 	d.insurance_file_cnt = @insurance_file_cnt 	
	--	AND 	account_id = @account_id
	--	AND 	outstanding_account_amount<>0
	--	AND 	TransDetail.company_id=ISNULL(@company_id,TransDetail.company_id)
	--	GROUP BY account_currency_id

	    SELECT
			t.transdetail_id,
	        t.outstanding_amount,
			t.amount_currency_id,
	        t.outstanding_account_amount,
			t.account_currency_id,
			c.iso_code
			INTO #tempReceiptImport
		FROM transdetail t
		INNER JOIN Currency c ON c.currency_id=t.amount_currency_id
		INNER JOIN Document d ON t.document_id = d.document_id
		WHERE 	d.insurance_file_cnt = @insurance_file_cnt 	
		AND 	account_id = @account_id
		AND t.company_id=ISNULL(@company_id,t.company_id)
		AND t.outstanding_amount<>0
	/* First check whether there are multiple base currencies on the account
	   if there are then auto-allocation is not allowed  */

	SELECT 	@base_currency_count=COUNT(DISTINCT amount_currency_id)
	FROM 	#tempReceiptImport	

	IF @base_currency_count=1 BEGIN
		/* Now check whether there are multiple account currencies on the account
		   if there are then auto-allocation is not allowed  */

		SELECT  @base_balance=SUM(outstanding_amount),
			@account_balance=SUM(outstanding_account_amount)
		FROM	#tempReceiptImport		

		SELECT 	@account_currency_count=COUNT(DISTINCT account_currency_id)
		FROM 	#tempReceiptImport		
		SELECT * FROM #tempReceiptImport	   
	END
	Drop TABLE #tempReceiptImport
END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

