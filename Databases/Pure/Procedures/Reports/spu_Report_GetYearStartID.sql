SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_GetYearStartID'
GO

CREATE PROCEDURE spu_Report_GetYearStartID
	@sub_branch_id int, --AMJ
         @SelectedPeriodID int,
         @YearStartPeriodID int OUTPUT
AS
/**********************************************************************************************************************************
** Created by Jude Killip
** 18/12/2001
** v1.00
**
** NAME:        spu_Report_GetYearStartID
**
** PARAMETERS:  @SelectedPeriodID int
**              @YearStartPeriodID int  OUTPUT
**
** USAGE:       DECLARE @YearStartPeriodID int
**              EXECUTE @SelectedPeriodID, sp_Report_GetYearStartID @YearStartPeriodID OUTPUT
**
** DESCRIPTION: Gets the first Period ID in the same year as the SelectedPeriodID parameter

**********************************************************************************************************************************
**
** 01/08/2002	AMJ	- sub branch specific change
**
***********************************************************************************************************************************/

SELECT @YearStartPeriodID = min(period_id)
FROM period
WHERE year_name =
(	SELECT year_name
        FROM period
        WHERE period_id = @SelectedPeriodID
)
and sub_branch_id = @sub_branch_id

GO

