SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_Latest_period'
GO


CREATE PROCEDURE spu_ACT_Select_Latest_period
    @company_id int,
    @period_end_date datetime,
    @sub_branch_id int = NULL
AS

DECLARE @period_id int

/* DD 23/08/2002 */
/* Get the Product Option for multi-tree accounting */
DECLARE @Value VARCHAR(20)
SELECT
    @Value=Value
FROM
    Hidden_options
WHERE
    option_number=16

/*
    If Null/0 then there is only one tree.
    Hardcoded for performance reasons
*/
IF @Value IS NULL OR @Value=0
BEGIN
    SELECT @company_id=1
    SELECT @sub_branch_id=1
END
ELSE
BEGIN
    IF @sub_branch_id IS NULL
        EXEC spu_sub_branch_default @source_id=@company_id, @sub_branch_id=@sub_branch_id OUTPUT
END

-- Get the earliest dates period id
SELECT   @period_id = period_id
FROM     period
WHERE    company_id = @company_id
AND      period_end_date > @period_end_date
AND      period_end_complete = 0
AND      sub_branch_id = @sub_branch_id
ORDER BY period_end_date DESC

-- Return the period id
Select @period_id period_id

GO


