SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_risk_type_screenID'
GO

CREATE PROCEDURE spu_get_risk_type_screenID  
 @risk_type_id int  
AS  
	SELECT Claims_Gis_screen_id  
	FROM risk_type  
	INNER JOIN risk  
	ON risk_type.risk_type_id = risk.risk_type_id  
	and risk_cnt=@risk_type_id  
GO