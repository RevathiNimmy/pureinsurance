SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_Trans_For_Allocation'
GO

create PROCEDURE spu_ACT_Select_Trans_For_Allocation
    @account_id INT,
	@company_id INT=NULL,
    @receipt_transdetail_id INT = 0,
	@base_balance MONEY=0 OUTPUT,
	@base_currency_count SMALLINT=0 OUTPUT,
	@account_balance MONEY=0 OUTPUT,
	@account_currency_count SMALLINT=0 OUTPUT,
	@DNGIND INT
AS
BEGIN
	DECLARE @TypeOfRates TINYINT

	EXEC spu_ACT_GetTypeOfRates @TypeOfRates

	IF @TypeOfRates = 1
	BEGIN
		SELECT @company_id = NULL
	END

	/* First check whether there are multiple base currencies on the account
	   if there are then auto-allocation is not allowed  */

	SELECT 	@base_currency_count=COUNT(DISTINCT amount_currency_id)
	FROM 	transdetail
	WHERE 	account_id = @account_id
	AND 	company_id=ISNULL(@company_id,company_id)		
	AND 	outstanding_amount<>0
	GROUP BY amount_currency_id
	

	IF @base_currency_count=1 BEGIN
		/* Now check whether there are multiple account currencies on the account
		   if there are then auto-allocation is not allowed  */
		IF @DNGIND =0 BEGIN
		SELECT  @base_balance=SUM(outstanding_amount),
				@account_balance=SUM(outstanding_account_amount)
		FROM	TransDetail
		WHERE 	account_id=@account_id
		AND 	company_id=ISNULL(@company_id,company_id)	
		END
		ELSE
		BEGIN
			SELECT  @base_balance=SUM(outstanding_amount),
					@account_balance=SUM(outstanding_account_amount)
			FROM	TransDetail t INNER JOIN DOCUMENT d on d.document_id=t.document_id
			WHERE 	account_id=@account_id
			AND 	t.company_id=ISNULL(@company_id,t.company_id) AND d.document_ref not like 'I%%'
		END

		SELECT 	@account_currency_count=COUNT(DISTINCT account_currency_id)
		FROM 	transdetail
		WHERE 	account_id = @account_id
		AND 	outstanding_account_amount<>0
		AND 	company_id=ISNULL(@company_id,company_id)		
		GROUP BY account_currency_id
	
		IF @DNGIND =0 BEGIN
	    SELECT
			t.transdetail_id,
	        t.outstanding_amount,
			t.amount_currency_id,
	        t.outstanding_account_amount,
			t.account_currency_id,
			c.iso_code,  
			d.document_ref  
		FROM transdetail t
		INNER JOIN Currency c ON c.currency_id=t.amount_currency_id
		INNER JOIN Document d ON d.document_id = t.document_id 
		WHERE t.account_id = @account_id
		AND t.company_id=ISNULL(@company_id,t.company_id)
		AND t.outstanding_amount<>0
        AND t.transdetail_id <> @receipt_transdetail_id
	END
	ELSE BEGIN
			 SELECT
				t.transdetail_id,
				t.outstanding_amount,
				t.amount_currency_id,
				t.outstanding_account_amount,
				t.account_currency_id,
				c.iso_code
		FROM transdetail t
		INNER JOIN Currency c ON c.currency_id=t.amount_currency_id
			INNER JOIN DOCUMENT d on d.document_id=t.document_id and d.document_ref not like 'I%%'
		WHERE t.account_id = @account_id
			AND t.company_id=ISNULL(@company_id,t.company_id)
			AND t.outstanding_amount<>0
		END
	END
		ELSE BEGIN
				 SELECT
					t.transdetail_id,
					t.outstanding_amount,
					t.amount_currency_id,
					t.outstanding_account_amount,
					t.account_currency_id,
					c.iso_code,  
					d.document_ref
				FROM transdetail t
				INNER JOIN Currency c ON c.currency_id=t.amount_currency_id
				INNER JOIN DOCUMENT d on d.document_id=t.document_id and d.document_ref not like 'I%%'
				WHERE t.account_id = @account_id
				AND t.company_id=ISNULL(@company_id,t.company_id)
				AND t.outstanding_amount <> 0
			END
	END
GO
