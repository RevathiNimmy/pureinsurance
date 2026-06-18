
EXECUTE DDLDropProcedure 'spu_Report_GetCurrent12MonthPeriod_SFU'
GO
/**********************************************************************************************************************************
** Created by Jude Killip
** 11/06/2001
**
** NAME:        sp_Report_GetCurrent12MonthPeriod
**
** PARAMETERS:  @12MonthPeriodID int            OUTPUT
**              @dt12MonthPeriodEnd datetime    OUTPUT
**
** USAGE:       DECLARE @12MonthPeriodID int, @dt12MonthPeriodEnd datetime
**              EXECUTE sp_Report_GetCurrent12MonthPeriod @12MonthPeriodID OUTPUT, @dt12MonthPeriodEnd OUTPUT
**
** DESCRIPTION: Gets the Period ID and End Date for a period 12 months previous to the Current Period
**
**              NB - Release "Orion_For_Broking" version
**********************************************************************************************************************************
**
***********************************************************************************************************************************/

CREATE PROCEDURE spu_Report_GetCurrent12MonthPeriod_SFU
        (
         @12MonthPeriodID int OUTPUT,
         @dt12MonthPeriodEnd datetime OUTPUT
         )
AS

DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
EXECUTE spu_Report_GetCurrentPeriod_SFU @CurrentPeriodID OUTPUT,  @dtCurrentPeriodEnd OUTPUT

SELECT @dt12MonthPeriodEnd = period_end_date,
        @12MonthPeriodID = period_id
FROM period
WHERE  datediff(month, period_end_date, @dtCurrentPeriodEnd) = 12

IF @12MonthPeriodID is NULL
    SELECT @dt12MonthPeriodEnd = min(period_end_date),
            @12MonthPeriodID = min(period_id)
    FROM period
GO