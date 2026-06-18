SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Risk_Type_ClaimScreen_upd'
GO

CREATE PROCEDURE spu_Risk_Type_ClaimScreen_upd  
    @risk_type_id int,  
    @claim_screen_id int  
AS  
	update risk_type  
	set claims_gis_screen_id = @claim_screen_id  
	where risk_type_id = @risk_type_id  
GO