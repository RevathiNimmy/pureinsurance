SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_GetCurrentPeriod'
GO


CREATE PROCEDURE spu_Report_GetCurrentPeriod
    @sub_branch_id int, -- PWF 30/07/2002
    @CurrentPeriodID int OUTPUT,
    @dtCurrentPeriodEnd datetime OUTPUT
AS

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
**              EXECUTE spu_Report_GetCurrentPeriod sub_branch_id, @CurrentPeriodID OUTPUT,  @dtCurrentPeriodEnd OUTPUT
**
** DESCRIPTION: Gets the Current Period ID and Current Period end date
**              i.e. the period which transactions are currently being booked against
***********************************************************************************************************************************/
SELECT @CurrentPeriodID = current_period_id
FROM   Ledger
WHERE  ledger_short_name = 'SA'
AND    sub_branch_id = @sub_branch_id

SELECT @dtCurrentPeriodEnd = period_end_date
FROM   Period
WHERE  Period_id = @CurrentPeriodID

GO


