
EXECUTE DDLDropProcedure 'spu_Report_GetPeriodEndComplete_Date_SFU'
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

CREATE PROCEDURE spu_Report_GetPeriodEndComplete_Date_SFU
(
	@dtPEndComplete DATETIME OUTPUT,
	@company_id INT = 1
)
AS

DECLARE @CurrentPeriodID INT
DECLARE @PeriodCompanyID INT

EXECUTE spu_Report_GetCurrentPeriodID_SFU @CurrentPeriodID OUTPUT, @company_id

SELECT @dtPEndComplete = period_end_date
FROM period
WHERE Period_id = @CurrentPeriodID - 1

/*Get company of current period*/
SELECT
	@PeriodCompanyID = company_id
FROM period
WHERE period_id = @CurrentPeriodID

/*Get the end date of the period before the first period of this year. Null if this is first year.*/
SELECT @dtPEndComplete = MAX(period_end_date)
FROM period
WHERE company_id = @PeriodCompanyID
AND period_end_date < 
	(
		SELECT period_end_date
		FROM period
		WHERE period_id = @CurrentPeriodID
	)

GO