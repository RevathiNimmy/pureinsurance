SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_Period'
GO


CREATE PROCEDURE spu_ACT_SelAll_Period
    @company_id int,
    @sub_branch_id int=NULL,
    @year_name varchar(20)
AS

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

SELECT
    period_id,
    company_id,
    sub_branch_id,
    year_name,
    period_name,
    period_end_date,
    period_end_complete
FROM Period
WHERE @company_id = company_id
    AND @sub_branch_id = sub_branch_id
    AND @year_name = year_name
ORDER BY period_end_date

GO


