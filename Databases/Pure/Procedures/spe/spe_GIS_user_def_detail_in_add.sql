SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_user_def_detail_in_add'
GO

CREATE PROCEDURE spe_GIS_user_def_detail_in_add
    @GIS_user_def_detail_id int,
    @GIS_user_def_detail_inds_id int,
    @value char(1)

AS

BEGIN
INSERT INTO GIS_user_def_detail_inds (
    GIS_user_def_detail_id ,
    GIS_user_def_detail_inds_id ,
    value )
VALUES (
    @GIS_user_def_detail_id,
    @GIS_user_def_detail_inds_id,
    @value)
END

GO

