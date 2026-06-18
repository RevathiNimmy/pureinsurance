SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Reinsurance_Screen_sel'
GO


CREATE PROCEDURE spu_Reinsurance_Screen_sel
(
	@IRiskType int
)
AS
	SELECT rt.display_reinsurance_screen 
	FROM risk_type rt 
	JOIN risk rr ON rt.risk_type_id=rr.risk_type_id 
	WHERE rr.risk_cnt = @IRiskType 
	AND rt.is_deleted = 0
GO

