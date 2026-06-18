SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_user_def_header_in_del'
GO

CREATE PROCEDURE spe_GIS_user_def_header_in_del
    @GIS_user_def_header_id int,
    @GIS_user_def_header_inds_id int
AS

DELETE FROM GIS_user_def_header_inds

WHERE GIS_user_def_header_id = @GIS_user_def_header_id AND GIS_user_def_header_inds_id = @GIS_user_def_header_inds_id

GO

