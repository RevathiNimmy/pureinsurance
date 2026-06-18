SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Do_Period_GenerateDefaultYear'
GO

CREATE PROCEDURE spu_ACT_Do_Period_GenerateDefaultYear
    @company_id INT,
    @sub_branch_id INT,
    @current_period_id INT OUTPUT
AS

DECLARE @CurrentDate DATETIME
DECLARE @EndDate DATETIME
DECLARE @CurrentYear INT
DECLARE @period_id INT

BEGIN TRANSACTION

/* Get 1 Jan for the current year */
SELECT @CurrentYear=YEAR(GETDATE())
SELECT @CurrentDate=CONVERT(DATETIME,'1/1/'+CONVERT(VARCHAR,@CurrentYear))

WHILE Year(@CurrentDate)=@CurrentYear
BEGIN
    
    SELECT @EndDate=DATEADD(Day,-1,DATEADD(Month,1,@CurrentDate))
    INSERT INTO Period (company_id, sub_branch_id, year_name, period_name, period_end_date, period_end_complete)
    VALUES (@company_id, @sub_branch_id, CONVERT(VARCHAR,@CurrentYear),
        'Period '+CONVERT(VARCHAR,Month(@CurrentDate)),@EndDate,0)

	SET @period_id=@@IDENTITY
    /* Are we in the current period? */
    IF GetDate()>=@CurrentDate AND GetDate()<=@EndDate
        SELECT @current_period_id=@period_id

    /* Advance a month */
    SELECT @CurrentDate=DATEADD(Month,1,@CurrentDate)
END

COMMIT TRANSACTION
GO
