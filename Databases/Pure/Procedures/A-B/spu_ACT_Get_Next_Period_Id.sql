SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_Next_Period_Id'
GO


CREATE PROCEDURE spu_ACT_Get_Next_Period_Id
    @period_id int,
    @next_period_id int OUTPUT,
    @period_end_date datetime
AS

-- DD 30/07/2002: Altered for multi-branch accounting

/*
SELECT @next_period_id = period_id
FROM Period
WHERE period_end_date =
(
    SELECT MIN(period_end_date)
    FROM Period
    WHERE period_end_date > @period_end_date
)
*/

DECLARE @company_id INT
DECLARE @sub_branch_id INT

SELECT @company_id=company_id, @sub_branch_id=sub_branch_id
FROM Period
WHERE period_id=@period_id

SELECT @next_period_id = period_id
FROM Period
WHERE sub_branch_id=@sub_branch_id
and period_end_date = (
    SELECT MIN(period_end_date)
    FROM Period
    WHERE sub_branch_id=@sub_branch_id and period_end_date > @period_end_date
)

GO


