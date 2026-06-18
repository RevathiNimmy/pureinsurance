SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_Previous_Period_Id'
GO


CREATE PROCEDURE spu_ACT_Get_Previous_Period_Id
    @period_id int,
    @period_end_date datetime,
    @previous_period_id int OUTPUT
AS


BEGIN
/*
SELECT @previous_period_id = period_id
FROM Period
WHERE period_end_date =
(
    SELECT MAX(period_end_date)
    FROM Period
    WHERE period_end_date < @period_end_date
)

END
*/

/* DD 31/07/2002: Altered to work with multi-branch accounting */

DECLARE @company_id INT
DECLARE @sub_branch_id INT

SELECT @company_id=company_id, @sub_branch_id=sub_branch_id
FROM Period
WHERE period_id=@period_id

SELECT @previous_period_id = period_id
FROM Period
WHERE company_id=@company_id and sub_branch_id=@sub_branch_id
and period_end_date = (
    SELECT MAX(period_end_date)
    FROM Period
    WHERE company_id=@company_id and sub_branch_id=@sub_branch_id and period_end_date < @period_end_date
)

END

GO


