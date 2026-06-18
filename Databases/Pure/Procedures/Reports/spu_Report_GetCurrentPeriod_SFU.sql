
EXECUTE DDLDropProcedure 'spu_Report_GetCurrentPeriod_SFU'
GO
/**********************************************************************************************************************************
** Created by Jude Killip
** 12/06/2001
**
** NAME:        sp_Report_GetCurrentPeriod
**
** PARAMETERS:  @CurrentPeriodID int                OUTPUT
**              @dtCurrentPeriodEnd datetime        OUTPUT
**
** USAGE:       DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
**              EXECUTE sp_Report_GetCurrentPeriod @CurrentPeriodID OUTPUT,  @dtCurrentPeriodEnd OUTPUT
**
** DESCRIPTION: Gets the Current Period ID and Current Period end date
**              i.e. the period which transactions are currently being booked against
**
**              NB - Release "Orion_For_Broking" version
**********************************************************************************************************************************
**
***********************************************************************************************************************************/

CREATE PROCEDURE spu_Report_GetCurrentPeriod_SFU
(
	@CurrentPeriodID INT OUTPUT,
	@dtCurrentPeriodEnd DATETIME OUTPUT,
	@company_id INT = 1
)
AS

IF NOT EXISTS(SELECT NULL FROM ledger WHERE company_id > 1)
BEGIN
	SELECT @company_id = 1
END

SELECT @CurrentPeriodID = current_period_id
FROM ledger
WHERE ledger_short_name = 'SA'
AND company_id = @company_id

SELECT @dtCurrentPeriodEnd = period_end_date
FROM period
WHERE  Period_id = @CurrentPeriodID


GO
