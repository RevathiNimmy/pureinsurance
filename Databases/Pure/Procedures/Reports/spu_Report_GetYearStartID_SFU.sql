
EXECUTE DDLDropProcedure 'spu_Report_GetYearStartID_SFU'
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO

/**********************************************************************************************************************************
** Created by Jude Killip
** 18/12/2001
** v1.00
**
** NAME:        sp_Report_GetYearStartID
**
** PARAMETERS:  @SelectedPeriodID int
**              @YearStartPeriodID int  OUTPUT
**
** USAGE:       DECLARE @YearStartPeriodID int
**              EXECUTE @SelectedPeriodID, sp_Report_GetYearStartID @YearStartPeriodID OUTPUT
**
** DESCRIPTION: Gets the first Period ID in the same year as the SelectedPeriodID parameter
**
**********************************************************************************************************************************
**
***********************************************************************************************************************************/

CREATE PROCEDURE spu_Report_GetYearStartID_SFU
        (
         @SelectedPeriodID int,
         @YearStartPeriodID int OUTPUT
         )
AS

SELECT @YearStartPeriodID = min(period_id)
FROM period
WHERE year_name =
    (SELECT year_name
        FROM period
        WHERE period_id = @SelectedPeriodID)


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO

