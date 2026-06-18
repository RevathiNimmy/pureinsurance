SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_Period_Total'
GO


CREATE PROCEDURE spu_ACT_Get_Period_Total
    @period_id int,
    @account_id int,
    @amount numeric(19,4) OUTPUT
AS

/**********************************************************************************************/
/* Author : Christopher Field */
/* */
/* History: 22/09/1998 CF - Added account_id Parameter */
/**********************************************************************************************/
DECLARE @year_name varchar(20)
SELECT
@year_name = year_name
FROM Period
WHERE period_id = @period_id
SELECT
@amount = SUM(amount)
FROM TransDetail,
    Period
WHERE Period.year_name = @year_name
AND TransDetail.Period_id = Period.period_id
AND TransDetail.Account_id = @account_id
GO


