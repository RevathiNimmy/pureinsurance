SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_GetCurrent24MonthPeriod'
GO


CREATE PROCEDURE spu_Report_GetCurrent24MonthPeriod
    @sub_branch_id int,
    @24MonthPeriodID int OUTPUT,
    @dt24MonthPeriodEnd datetime OUTPUT
AS

/**********************************************************************************************************************************
** Created by Jude Killip
** 11/06/2001
**
** NAME:        sp_Report_GetCurrent24MonthPeriod
**
** PARAMETERS:  @24MonthPeriodID int            OUTPUT
**              @dt24MonthPeriodEnd datetime    OUTPUT
**
** USAGE:       DECLARE @24MonthPeriodID int, @dt24MonthPeriodEnd datetime
**              EXECUTE spu_Report_GetCurrent24MonthPeriod @24MonthPeriodID OUTPUT, @dt24MonthPeriodEnd OUTPUT
**
** DESCRIPTION: Gets the Period ID and End Date for a period 24 months previous to the Current Period
***********************************************************************************************************************************/
DECLARE 
    @CurrentPeriodID int, 
    @dtCurrentPeriodEnd datetime

EXECUTE spu_Report_GetCurrentPeriod 
    @sub_branch_id,
    @CurrentPeriodID OUTPUT,  
    @dtCurrentPeriodEnd OUTPUT

SELECT   @dt24MonthPeriodEnd = period_end_date,
         @24MonthPeriodID = period_id
FROM     period
WHERE    datediff(month, period_end_date, @dtCurrentPeriodEnd) > 24
AND      sub_branch_id = @sub_branch_id
ORDER BY period_end_date ASC
       

-- If we have 
IF @24MonthPeriodID is NULL
    SELECT @dt24MonthPeriodEnd = min(period_end_date),
           @24MonthPeriodID = min(period_id)
    FROM   period

GO


