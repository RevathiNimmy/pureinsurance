SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_GetPeriodEnd_Date'
GO


CREATE PROCEDURE spu_Report_GetPeriodEnd_Date
    @sub_branch_id int,
    @dtThisPeriodEnd datetime OUTPUT
AS

/**********************************************************************************************************************************
** Created by Jude Killip
** 26/04/2001
**
** NAME:        sp_Report_GetPeriodEnd_Date
**
** PARAMETERS:  @dtThisPeriodEnd datetime       OUTPUT
**
** USAGE:       DECLARE @dtThisPeriodEnd datetime
**              EXECUTE spu_Report_GetPeriodEnd_Date @dtThisPeriodEnd OUTPUT
***********************************************************************************************************************************/
DECLARE @current_period_id int

SELECT @current_period_id = current_period_id
FROM   ledger
WHERE  ledger_short_name = 'SA'
AND    sub_branch_id = @sub_branch_id


SELECT @dtThisPeriodEnd = period_end_date
FROM   period
WHERE  Period_id = @current_period_id

GO


