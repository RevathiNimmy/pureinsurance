SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Account_Balance_Summary_SFU'
GO

CREATE PROCEDURE spu_Report_Account_Balance_Summary_SFU
    @cashlist_id INT
   
AS

DECLARE @total_adjustment CURRENCY
SELECT @total_adjustment =(
    SELECT
        SUM(amount)
    FROM 
        CashList_Adjustment
    WHERE
        cashlist_id = @cashlist_id
    )

DECLARE @confirm_user VARCHAR(255)
SELECT @confirm_user =(
    SELECT
        username
    FROM 
        pmuser U,
        cashlist C
    WHERE
        C.confirm_pmuser_id = U.user_id
    AND
        cashlist_id = @cashlist_id
    )

DECLARE @confirm2_user VARCHAR(255)
SELECT @confirm2_user =(
    SELECT
        username
    FROM 
        pmuser U,
        cashlist C
    WHERE
        C.confirm2_pmuser_id = U.user_id
    AND
        cashlist_id = @cashlist_id
    )


SELECT 
    I.cashlist_id,
    SUM(I.Amount) AS Total,
    SUM(CASE MV.code WHEN 'CASH' THEN Amount ELSE 0 END) 'Total CASH',
    SUM(CASE MV.code WHEN 'CHQ' THEN Amount ELSE 0 END) 'Total CHQ',
    SUM(CASE MV.code WHEN 'CC' THEN Amount ELSE 0 END) 'Total CC',
    @total_adjustment 'Total Adjustment',
    C.cash_float_amount,
    @confirm_user 'confirm_user',
    @confirm2_user 'confirm2_user',
    C.date_approved,    
    C.list_date
--INTO 
--    #tmp
FROM 
    CashList C, 
    CashListItem I, 
    MediaType M,
    MediaType_Validation MV
-- Join
WHERE
    C.cashlist_id = I.cashlist_id
AND
    I.mediatype_id = M.mediatype_id  
AND   
    M.mediatype_validation_id = MV.mediatype_validation_id  
-- Filter
AND
    I.cashlist_id = @cashlist_id

GROUP BY 
    I.cashlist_id,
    C.cash_float_amount,
    C.date_approved,
    C.list_date

GO