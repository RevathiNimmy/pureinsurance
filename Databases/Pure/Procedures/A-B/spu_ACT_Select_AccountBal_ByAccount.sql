SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_AccountBal_ByAccount'
GO

/*If bank then use currency amount*/
/*If only one base currency then use base amount*/
/*Otherwise use account amount, as transactions for account could be accross two branches with different base rates.*/

CREATE PROCEDURE spu_ACT_Select_AccountBal_ByAccount
    @account_id INT,
    @postingstatus_id SMALLINT,
    @company_id INT = NULL
AS

DECLARE @isbank TINYINT
DECLARE @TypeOfRates TINYINT

-- Check if this is a bankaccount
SELECT  @isbank = COUNT(*)
FROM    bankaccount
WHERE   account_id = @account_id

EXEC spu_ACT_GetTypeOfRates @TypeOfRates

-- Do we need to restrict by company?
IF ISNULL(@company_id, 0) = 0
BEGIN
    SELECT  CASE
    			WHEN @isbank > 0 THEN ISNULL(SUM(ROUND(outstanding_currency_amount, 2)), 0)
    			WHEN @TypeOfRates = 1 THEN ISNULL(SUM(ROUND(outstanding_amount, 2)), 0)
                ELSE ISNULL( SUM( ROUND(outstanding_account_amount, 2)), 0)
            END AS sum_amount
    FROM    transdetail td
    WHERE   td.account_id = @account_id
    AND     td.postingstatus_id = @postingstatus_id
END
ELSE
BEGIN
    SELECT  CASE
    			WHEN @isbank > 0 THEN ISNULL(SUM(ROUND(outstanding_currency_amount, 2)), 0)
    			ELSE ISNULL(SUM(ROUND(outstanding_amount, 2)), 0)
            END AS sum_amount
    FROM    transdetail td
    WHERE   td.account_id = @account_id
    AND     td.postingstatus_id = @postingstatus_id
    AND     td.company_id = @company_id
END

GO


