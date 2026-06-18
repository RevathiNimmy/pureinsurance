SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_GetPeriodEndComplete_Date'
GO


CREATE PROCEDURE spu_Report_GetPeriodEndComplete_Date
    @sub_branch_id int,
    @dtPEndComplete datetime OUTPUT
AS

/**********************************************************************************************************************************
** Created by Jude Killip
** 03/05/2001
**
** NAME:        sp_Report_GetPeriodEndComplete_Date
**
** PARAMETERS:  @dtPEndComplete datetime       OUTPUT
**
** USAGE:       DECLARE @dtPEndComplete datetime
**              EXECUTE spu_Report_GetPeriodEndComplete_Date sub_branch_id, @dtPEndComplete OUTPUT
***********************************************************************************************************************************/
DECLARE 
    @CurrentPeriodID int,
    @CurrentEndDate datetime

EXECUTE spu_Report_GetCurrentPeriod
    @sub_branch_id,
    @CurrentPeriodID OUTPUT, 
    @CurrentEndDate OUTPUT

-- PWF 30/07/2002 More reliable method to get previous end date
SELECT @dtPEndComplete = Max(period_end_date)
FROM   Period
WHERE  sub_branch_id = @sub_branch_id
AND    period_end_date < @CurrentEndDate

GO

