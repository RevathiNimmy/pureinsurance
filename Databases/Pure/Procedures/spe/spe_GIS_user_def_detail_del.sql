SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_user_def_detail_del'
GO

CREATE PROCEDURE spe_GIS_user_def_detail_del
    @GIS_user_def_detail_id int
AS

DELETE FROM GIS_user_def_detail

WHERE GIS_user_def_detail_id = @GIS_user_def_detail_id

GO

