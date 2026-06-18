SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Check_AllowYearEnd'
GO

CREATE PROCEDURE spu_ACT_Check_AllowYearEnd
(
    @period_id INT
)
AS

DECLARE @company_id INT
DECLARE @sub_branch_id  INT

/*Get branch and sub branch from the current_id passed in*/
SELECT  
    @company_id = company_id,
    @sub_branch_id = sub_branch_id
FROM period
WHERE period_id = @period_id

/*Select the current period if the year end button should be displayed*/
SELECT p.period_id
FROM period p
WHERE company_id = @company_id
AND sub_branch_id = @sub_branch_id
/*Year end has not already been created*/
AND
(
    SELECT MAX(period_end_date)
    FROM period
    WHERE period_end_date < p.period_end_date
    AND company_id = @company_id
    AND sub_branch_id = @sub_branch_id
)
NOT IN
(
    SELECT document_date
    FROM document
    WHERE comment LIKE 'Year End Retained Profit%'
    AND company_id = @company_id
    AND sub_branch_id = @sub_branch_id
)
/*Period is the current one*/
AND period_end_date =  
(
    SELECT MIN(period_end_date)
    FROM period
    WHERE period_end_complete = 0
    AND company_id = @company_id
    AND sub_branch_id = @sub_branch_id
    AND period_end_date >
    (
        SELECT MAX(period_end_date)
        FROM Period
        WHERE period_end_complete = 1
        AND company_id = @company_id
        AND sub_branch_id = @sub_branch_id
    )
)
/*Period is the first one of the year*/
AND period_end_date IN
(
    SELECT MIN(period_end_date)
    FROM period
    WHERE company_id = @company_id
    AND sub_branch_id = @sub_branch_id
    GROUP BY year_name
)
/*Period is not the very first one on the system*/
AND period_end_date NOT IN
(
    SELECT MIN(period_end_date)
    FROM period
    WHERE company_id = @company_id
    AND sub_branch_id = @sub_branch_id
)


GO
