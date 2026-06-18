SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_SIR_Select_Open_Posting_Periods'
GO

CREATE PROCEDURE spu_SIR_Select_Open_Posting_Periods
	@effective_date  datetime
AS

	SELECT TOP 1 period_id, period_name, period_end_date 
	FROM Period
	WHERE period_end_date > @effective_date
	AND period_end_complete = 0
	UNION
	SELECT TOP 1 period_id, period_name, period_end_date 
	FROM Period
	WHERE period_end_date > GETDATE()
	AND period_end_complete = 0
	ORDER BY period_end_date

GO
