SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Get_Dates'
GO


CREATE PROCEDURE spu_Report_Get_Dates
    @dtTodayStart datetime OUTPUT,
    @dtThisYearStart datetime OUTPUT
AS

/**********************************************************************************************************************************
** Created by Jude Killip
** 12/12/2000
**
** NAME:        sp_Report_Get_Dates
**
** PARAMETERS:  @dtTodayStart datetime OUTPUT           - Today's date without hrs mins secs
**              @dtThisYearStart datetime OUTPUT        - 1st January this year without hrs mins secs
**
** USAGE:       DECLARE @dtThisYearStart datetime, @dtTodayStart datetime
**              EXECUTE spu_Report_Get_Dates @dtTodayStart OUTPUT, @dtThisYearStart OUTPUT
**
** E.G.:        @dtThisYearStart            @dtTodayStart
**              --------------------------- ---------------------------
**              2000-01-01 00:00:00.000     2000-12-12 00:00:00.000
**
**
**********************************************************************************************************************************
**
***********************************************************************************************************************************/
DECLARE @dtToday datetime,
        @iDayToday int,
        @iMonthToday int,
        @dtTodayMY datetime

SELECT @dtToday = getdate(),
        @dtThisYearStart = '1/1/' + Convert(char(4), Year(@dtToday)),
        @iDayToday = day(@dtToday),
        @iMonthToday = month(@dtToday),
        @dtTodayMY = dateadd(month, @iMonthToday-1, @dtThisYearStart),
        @dtTodayStart = dateadd(day, @iDayToday-1, @dtTodayMY)
GO


