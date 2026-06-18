SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_Current_Period'
GO


CREATE PROCEDURE spu_ACT_Get_Current_Period
    @company_id int,
    @sub_branch_id int=NULL
AS

DECLARE @period_id INT

/* DD 23/08/2002 */
/* Get the Product Option for multi-tree accounting */
/*PSL 29/07/2003 Fixed 5615 */
/*now looks at the earliest period not period-ended, that is after the last period that WAS period-ended*/
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

/* Determine the current period for this company and sub-branch */
SELECT @period_id=ISNULL(MIN(period_id),1)
FROM Period
WHERE company_id = @company_id and sub_branch_id=@sub_branch_id and period_end_date =
           (SELECT MIN(period_end_date)
            FROM period
            WHERE   company_id = @company_id and
                    sub_branch_id=@sub_branch_id and
                    period_end_complete = 0
            AND period_end_date >= (SELECT ISNULL(MIN(period_end_date) ,GETDATE())   
                                   FROM period    
                                   WHERE   company_id = @company_id and    
                                           sub_branch_id=@sub_branch_id and    
                                           period_end_complete = 1))

/* Get the details */
EXEC spu_ACT_Select_Period @period_id=@period_id