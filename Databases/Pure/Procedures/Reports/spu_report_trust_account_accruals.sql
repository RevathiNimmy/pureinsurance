SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_report_trust_account_accruals'
GO


CREATE PROCEDURE spu_report_trust_account_accruals
	@branch_id INT,
	@end_date DATETIME

AS
 
DECLARE @ClientBankTotal money
DECLARE @PremiumFinanceTotal money
DECLARE @TransactedCommissionTotal money
DECLARE @EarnedCommissionTotal money
DECLARE @UnearnedCommissionTotal money
DECLARE @InsurerDebt money
DECLARE @InsurerCredit money
DECLARE @ClientDebt money
DECLARE @ClientCredit money
DECLARE @FSAdisabled tinyint

SELECT @FSAdisabled = 0

IF NOT EXISTS
    (
        SELECT NULL
        FROM hidden_options
        WHERE option_number = 61
        AND value = '1'
    )
BEGIN
    --FSA option is disabled so prevent use of the report PN22417
    SELECT NULL 'Client Cash',
	   NULL 'Premium Finance Debt',
	   NULL 'Transacted Commission',
	   NULL 'Earned Commission',
	   NULL 'Unearned Commission',
	   NULL 'Due from Insurer',
	   NULL 'Due to Insurer',
	   NULL 'Client Debt',
	   NULL 'Client Credit',
	   1    'FSAdisabled'
RETURN	
END

If @branch_id = 0
	SELECT @branch_id = NULL

/* Calculate money in the client bank account */
SELECT @ClientBankTotal = (SELECT isnull(sum(amount),0) from Transdetail
		   	  WHERE  account_id in (SELECT DISTINCT BA.account_id
						FROM BankAccount BA
 						JOIN Transdetail T ON T.account_id = BA.account_id
						JOIN Transdetail T2 ON T2.document_id = T.document_id
						JOIN Account A ON A.account_id = T2.account_id
						JOIN Ledger L ON L.ledger_id = A.ledger_id
						WHERE L.ledger_short_name = 'SA'
						AND T.transdetail_id <> T2.transdetail_id
						)
			AND ref_date <= @end_date
			AND company_id = IsNull(@branch_id,Company_id)
			)

 

/* Calculate money owed from Finance Providers */
SELECT @PremiumFinanceTotal = ( SELECT ISNULL(sum(T.amount),0) 	
				FROM Transdetail T
		   	  	JOIN Account A ON A.account_id = T.account_id  
 				JOIN Ledger L ON L.ledger_id = A.ledger_id
				WHERE L.ledger_short_name = 'RF' 
			      	AND T.ref_date <= @end_date
				AND T.company_id = IsNull(@branch_id,T.Company_id)
				) * -1


/* Calculate Unearned Commission */
SELECT @TransactedCommissionTotal = 
				( SELECT isnull(sum(T.amount),0) 
				FROM Transdetail T
		   	  	JOIN Account A ON A.account_id = T.account_id  
 				JOIN Ledger L ON L.ledger_id = A.ledger_id
				WHERE L.ledger_short_name = 'CO'
				AND T.spare in ('BROK','BROK ADJ')
			      	AND T.ref_date <= @end_date
				AND T.company_id = IsNull(@branch_id,T.Company_id)
				)

/* Calculate Unearned Commission */
SELECT @EarnedCommissionTotal = 
				( SELECT isnull(sum(TM.base_match_amount),0) 
				FROM Transdetail T
				JOIN Transmatch TM ON TM.transdetail_id = T.transdetail_id
		   	  	JOIN Account A ON A.account_id = T.account_id  
 				JOIN Ledger L ON L.ledger_id = A.ledger_id
				WHERE L.ledger_short_name = 'CO' 
				AND T.spare in ('BROK','BROK ADJ')			      	
				AND T.ref_date <= @end_date
				AND T.company_id = IsNull(@branch_id,T.Company_id)
				)

/* Calculate Unearned Commission */
SELECT @UnearnedCommissionTotal =  @TransactedCommissionTotal - @EarnedCommissionTotal  
 

EXEC spu_report_trust_account_accruals_creditors_debtors @end_date=@end_date,@branch_id=@branch_id,
					    @insurer_debt=@InsurerDebt OUTPUT,
					    @insurer_credit=@InsurerCredit OUTPUT,					    @client_debt=@ClientDebt OUTPUT,
					    @client_credit=@ClientCredit OUTPUT 


SELECT 	@ClientBankTotal  'Client Cash',
	@PremiumFinanceTotal * -1  'Premium Finance Debt',
	@TransactedCommissionTotal * -1 'Transacted Commission',
	@EarnedCommissionTotal * -1 'Earned Commission',
	@UnearnedCommissionTotal * -1 'Unearned Commission',
	@InsurerDebt   'Due from Insurer',
	@InsurerCredit * -1 'Due to Insurer',
	@ClientDebt 'Client Debt',
	@ClientCredit * -1 'Client Credit',
	@FSAdisabled 'FSAdisabled'