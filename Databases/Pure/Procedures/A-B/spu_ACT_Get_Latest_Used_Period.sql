SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Get_Latest_Used_Period'
GO

CREATE PROCEDURE spu_ACT_Get_Latest_Used_Period

    @period_id INT,
    @period_end_date DATETIME OUTPUT
    
AS

DECLARE @company_id INT
DECLARE @sub_branch_id INT

/*Get the branch ids from the passed in period*/
SELECT 
    @company_id = company_id,
    @sub_branch_id = sub_branch_id
FROM period
WHERE period_id = @period_id

/*Get the last use period*/
SELECT
    @period_end_date = MAX(p.period_end_date)
FROM period p
WHERE p.company_id = @company_id
AND p.sub_branch_id = @sub_branch_id
AND EXISTS
    (
        SELECT
            NULL
        FROM transdetail
        WHERE period_id = p.period_id
    )


GO



