EXECUTE DDLDropProcedure 'spu_Report_Account_Balance_SFU'
GO

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

/**********************************************************************************************************************************
** Created by Jude Killip
** 11/Oct/2002
** Reports - Account_Balance.rpt (was Client_Balances.rpt)
**
**********************************************************************************************************************************
** VER      DATE        WHO     WHAT
** 1.01     21-Oct-02   JMK     Rename from sp_Report_Client_Balances to spu_Report_Account_Balance
**                              Eliminate accounts with ^ prefix
**                              Add account_type parameter
***********************************************************************************************************************************/

CREATE PROCEDURE spu_Report_Account_Balance_SFU
	@account_type VARCHAR(20),
	@branch_id INT,
	@TypeOfCurrency VARCHAR(15),
	@GroupBy VARCHAR(20)
AS

DECLARE @LedgerShortName VARCHAR(2)
DECLARE @SystemCurrencyCode VARCHAR(10)
DECLARE @SystemCurrencyDesc VARCHAR(255)

/*Set @branch_id to NULL when searching for all branches.*/
IF @branch_id = 0
BEGIN
	SELECT @branch_id = NULL
END

/*Get the type of ledger we are reporting on.*/
SELECT @LedgerShortName = 
	CASE @account_type
        WHEN 'CLIENT' THEN 'SA'
        WHEN 'REINSURER' THEN 'IN'
        WHEN 'AGENT' THEN 'AG'
        WHEN 'SUBAGENT' THEN 'UB'
        WHEN 'PURCHASE' THEN 'PU'
	END

/*Get System Currency Details*/
SELECT
	@SystemCurrencyCode = c.iso_code,
	@SystemCurrencyDesc = c.description
FROM PMSystem pms
JOIN currency c
	ON c.currency_id = pms.currency_id
WHERE pms.system_id = 1

CREATE TABLE #Report_Accounts
(
	Sort VARCHAR(30),
	short_code VARCHAR(30),
	account_name VARCHAR(60),
	Amount MONEY,
	CompanyCode VARCHAR(10),
	CompanyDesc VARCHAR(255),
	CurrencyCode VARCHAR(10),
	CurrencyDesc VARCHAR(255)
)

INSERT INTO #Report_Accounts
SELECT
    CASE
    	WHEN ASCII(LEFT(a.short_code,1)) BETWEEN 65 AND 90
    		OR ASCII(LEFT(a.short_code,1)) BETWEEN 95 AND 12 THEN LEFT(a.short_code,1)
    	ELSE '...'
    END,
    a.short_code,
    a.account_name,
	CASE @TypeOfCurrency
		WHEN 'Account' THEN ISNULL(SUM(ROUND(t.account_amount,2)), 0.0)
		WHEN 'Base' THEN ISNULL(SUM(ROUND(t.amount,2)), 0.0)
		WHEN 'System' THEN ISNULL(SUM(ROUND(t.system_amount,2)), 0.0)
		WHEN 'Transaction' THEN ISNULL(SUM(ROUND(t.currency_amount,2)), 0.0)
	END Amount,
	c.code,
	c.description,
	CASE @TypeOfCurrency
		WHEN 'Account' THEN ca.iso_code
		WHEN 'Base' THEN cb.iso_code
		WHEN 'System' THEN @SystemCurrencyCode
		WHEN 'Transaction' THEN ct.iso_code
	END,
	CASE @TypeOfCurrency
		WHEN 'Account' THEN ca.description
		WHEN 'Base' THEN cb.description
		WHEN 'System' THEN @SystemCurrencyDesc
		WHEN 'Transaction' THEN ct.description
	END
FROM transdetail t
JOIN account a
	ON a.account_id = t.account_id
JOIN ledger l
	ON l.ledger_id = a.ledger_id
JOIN company c
	ON c.company_id = t.company_id
JOIN currency ca /*Account Currency*/
	ON ca.currency_id = a.currency_id
JOIN currency cb /*Base Currency*/
	ON cb.currency_id = c.base_currency
JOIN currency ct /*Transaction Currency*/
	ON ct.currency_id = t.currency_id
WHERE l.ledger_short_name = @LedgerShortName
AND ASCII(LEFT(a.short_code,1)) <> 94       -- eliminate accounts with ^ prefix
AND t.company_id = ISNULL(@branch_id, t.company_id)
GROUP BY
	a.short_code,
	a.account_name,
	c.code,
	c.description,
	CASE @TypeOfCurrency
		WHEN 'Account' THEN ca.iso_code
		WHEN 'Base' THEN cb.iso_code
		WHEN 'System' THEN @SystemCurrencyCode
		WHEN 'Transaction' THEN ct.iso_code
	END,
	CASE @TypeOfCurrency
		WHEN 'Account' THEN ca.description
		WHEN 'Base' THEN cb.description
		WHEN 'System' THEN @SystemCurrencyDesc
		WHEN 'Transaction' THEN ct.description
	END
HAVING SUM(amount) <> 0


SELECT 
	*,
	CASE @GroupBy
		WHEN 'Branch' THEN CompanyCode
		WHEN 'Branch and Currency' THEN CompanyCode
		WHEN 'Currency' THEN CurrencyCode
		ELSE ''
	END 'GroupByCode',
	CASE @GroupBy
		WHEN 'Branch' THEN CompanyDesc
		WHEN 'Branch and Currency' THEN CompanyDesc
		WHEN 'Currency' THEN CurrencyDesc
		ELSE ''
	END 'GroupByDesc'
FROM #Report_Accounts

DROP TABLE #Report_Accounts

GO

