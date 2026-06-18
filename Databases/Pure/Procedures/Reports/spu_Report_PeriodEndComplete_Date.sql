SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_PeriodEndComplete_Date'
GO


CREATE PROCEDURE spu_Report_PeriodEndComplete_Date
    @sub_branch_id int,
    @dtPEndComplete datetime OUTPUT
AS

/**********************************************************************************************************************************
** Created by Jude Killip
** 03/05/2001
**
** NAME:        sp_Report_PeriodEndComplete_Date
**
** PARAMETERS:  @dtPEndComplete datetime       OUTPUT

**
** USAGE:       DECLARE @dtPEndComplete datetime
**              EXECUTE spu_Report_PeriodEndComplete_Date @dtPEndComplete OUTPUT
***********************************************************************************************************************************/

EXECUTE spu_Report_GetPeriodEndComplete_Date
    @sub_branch_id,
    @dtPEndComplete OUTPUT


GO


