SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_sel_NonBusinessDaysCount'
GO

CREATE PROCEDURE spu_SIR_sel_NonBusinessDaysCount
    @startdate DATETIME,
    @enddate DATETIME
AS
SELECT count(*)
FROM   Non_Business_Days
WHERE  (effective_date >= @startDate and effective_date <= @enddate and is_repeating=0)
OR     (DAY(effective_date) >= DAY(@startdate) AND
       DAY(effective_date) <= DAY(@enddate) AND
       MONTH(effective_date) >= MONTH(@startdate) AND
       MONTH(effective_date) <= MONTH(@enddate) AND
       is_repeating=1)
GO
