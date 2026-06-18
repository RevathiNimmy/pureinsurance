SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_GetCurrentYear'
GO


CREATE PROCEDURE spu_Report_GetCurrentYear
    @sub_branch_id int,
    @CurrentYearStartPeriodID int OUTPUT,
    @dtLastYearPeriodEndDate datetime OUTPUT
AS

/**********************************************************************************************************************************
** Created by Jude Killip
** 12/06/2001
**
** NAME:        sp_Report_GetCurrentYear
**
** PARAMETERS:  @CurrentYearStartPeriodID int   OUTPUT
**              @LastYearPeriodEndDate datetime      OUTPUT
**
** USAGE:       DECLARE @CurrentYearStartPeriodID int, @dtLastYearPeriodEndDate datetime
**              EXECUTE spu_Report_GetCurrentYear @CurrentYearStartPeriodID OUTPUT, @dtLastYearPeriodEndDate OUTPUT
**
** DESCRIPTION: Gets the first Period ID in the same year as the Current Period and Period end date of Last Year
**              i.e. the first period in the current year
***********************************************************************************************************************************/
DECLARE 
    @CurrentPeriodID int,
    @dtThisYearPeriodStartDate datetime

EXECUTE spu_Report_GetCurrentPeriod 
    @sub_branch_id,
    @CurrentPeriodID OUTPUT, 
    NULL

-- Get the current years start period
SELECT @CurrentYearStartPeriodID = min(period_id)
FROM   period
WHERE  year_name = (SELECT year_name
                    FROM   period
                    WHERE  period_id = @CurrentPeriodID)
AND    sub_branch_id = @sub_branch_id

-- Get the year start period end date
SELECT @dtThisYearPeriodStartDate = period_end_date   
FROM   period
WHERE  period_id = @CurrentYearStartPeriodID

-- Get the last period end from before this year
SELECT @dtLastYearPeriodEndDate = period_end_date
FROM   period
WHERE  period_end_date < @dtThisYearPeriodStartDate
AND    sub_branch_id = @sub_branch_id

GO


