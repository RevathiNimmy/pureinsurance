
EXECUTE DDLDropProcedure 'spu_Report_GetCurrent24MonthPeriod_SFU'
GO
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
**              EXECUTE sp_Report_GetCurrent24MonthPeriod @24MonthPeriodID OUTPUT, @dt24MonthPeriodEnd OUTPUT
**
** DESCRIPTION: Gets the Period ID and End Date for a period 24 months previous to the Current Period
**
**              NB - Release "Orion_For_Broking" version
**********************************************************************************************************************************
**
***********************************************************************************************************************************/

CREATE PROCEDURE spu_Report_GetCurrent24MonthPeriod_SFU
        (
         @24MonthPeriodID int OUTPUT,
         @dt24MonthPeriodEnd datetime OUTPUT
         )
AS

DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
EXECUTE spu_Report_GetCurrentPeriod_SFU @CurrentPeriodID OUTPUT,  @dtCurrentPeriodEnd OUTPUT

SELECT @dt24MonthPeriodEnd = period_end_date,
        @24MonthPeriodID = period_id
FROM period
WHERE  datediff(month, period_end_date, @dtCurrentPeriodEnd) = 24

IF @24MonthPeriodID is NULL
    SELECT @dt24MonthPeriodEnd = min(period_end_date),
            @24MonthPeriodID = min(period_id)
    FROM period
GO