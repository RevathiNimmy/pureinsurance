SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Risk_Type_GIS_Screen_del'
GO


CREATE PROCEDURE spu_Risk_Type_GIS_Screen_del
    @risk_type_id int
AS


DELETE FROM Risk_Type_GIS_Screen

WHERE risk_type_id = @risk_type_id
GO


