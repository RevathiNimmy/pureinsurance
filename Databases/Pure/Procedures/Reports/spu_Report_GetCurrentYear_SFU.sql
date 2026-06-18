
EXECUTE DDLDropProcedure 'spu_Report_GetCurrentYear_SFU'
GO
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
**              EXECUTE sp_Report_GetCurrentYear @CurrentYearStartPeriodID OUTPUT, @dtLastYearPeriodEndDate OUTPUT
**
** DESCRIPTION: Gets the first Period ID in the same year as the Current Period and Period end date of Last Year
**              i.e. the first period in the current year
**
**              NB - Release "Orion_For_Broking" version
**********************************************************************************************************************************
**
***********************************************************************************************************************************/

CREATE PROCEDURE spu_Report_GetCurrentYear_SFU
(
         @CurrentYearStartPeriodID INT OUTPUT,
         @dtLastYearPeriodEndDate DATETIME OUTPUT,
		 @company_id INT = 1
)
AS

DECLARE @CurrentPeriodID INT
DECLARE @PeriodCompanyID INT
DECLARE @PeriodYearName VARCHAR(20)

EXECUTE spu_Report_GetCurrentPeriod_SFU @CurrentPeriodID OUTPUT, NULL, @company_id

/*Get company and year of current period*/
SELECT
	@PeriodCompanyID = company_id,
	@PeriodYearName = year_name
FROM period
WHERE period_id = @CurrentPeriodID

/*Get the first period id for the current year*/
SELECT @CurrentYearStartPeriodID = period_id
FROM period
WHERE year_name = @PeriodYearName
AND company_id = @PeriodCompanyID
AND period_end_date = 
	(
		SELECT MIN(period_end_date)
		FROM period
		WHERE year_name = @PeriodYearName
		AND company_id = @PeriodCompanyID
	)

/*Get the end date of the period before the first period of this year. Null if this is first year.*/
SELECT @dtLastYearPeriodEndDate = MAX(period_end_date)
FROM period
WHERE company_id = @PeriodCompanyID
AND period_end_date < 
	(
		SELECT MIN(period_end_date)
		FROM period
		WHERE year_name = @PeriodYearName
		AND company_id = @PeriodCompanyID
	)

GO