
EXECUTE DDLDropProcedure 'spu_Report_GetCurrentPeriodID_SFU'
GO
/**********************************************************************************************************************************
** Created by Jude Killip
** 03/05/2001
**
** NAME:        sp_Report_GetPeriodEndComplete_Date
**
** PARAMETERS:  @dtPEndComplete datetime       OUTPUT

**
** USAGE:       DECLARE @dtPEndComplete datetime
**              EXECUTE sp_Report_GetPeriodEndComplete_Date @dtPEndComplete OUTPUT
**
**
**              NB - Release "Orion_For_Broking" version
**********************************************************************************************************************************
**
***********************************************************************************************************************************/

CREATE PROCEDURE spu_Report_GetCurrentPeriodID_SFU
(
	@CurrentPeriodID INT OUTPUT,
	@company_id INT = 1
)
AS

IF NOT EXISTS(SELECT NULL FROM ledger WHERE company_id > 1)
BEGIN
	SELECT @company_id = 1
END

SELECT @CurrentPeriodID = MIN(period_id) 
FROM period 
WHERE period_end_complete = 0
AND company_id = @company_id

GO

