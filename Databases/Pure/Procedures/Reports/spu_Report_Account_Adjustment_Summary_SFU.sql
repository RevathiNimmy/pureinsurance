SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Account_Adjustment_Summary_SFU'
GO

CREATE PROCEDURE spu_Report_Account_Adjustment_Summary_SFU
    @cashlist_id INT
   
AS

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
    D.description 'drawer_name',
    C.list_date,
    A.adjustment_date,
--    U.username ,
    A.amount,
    AM.description 'adjustment_method',
    A.reason,
    @confirm_user 'confirm_user',
    @confirm2_user 'confirm2_user'


FROM 
    CashList_Drawer D,
    CashList C,
    CashList_Adjustment A,
    CashList_Adjustment_Method AM--,
--    PMUser U

-- Join
WHERE 
    D.cashlist_drawer_id = C.cashlist_drawer_id 
AND 
    C.cashlist_id = A.cashlist_id 
--AND
--    A.pmuser_id = U.user_id
-- Filter
AND
    C.cashlist_id = @cashlist_id