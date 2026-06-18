SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_user_def_header_del'
GO

CREATE PROCEDURE spe_GIS_user_def_header_del
    @GIS_user_def_header_id int
AS

DELETE FROM GIS_user_def_header

WHERE GIS_user_def_header_id = @GIS_user_def_header_id

GO

