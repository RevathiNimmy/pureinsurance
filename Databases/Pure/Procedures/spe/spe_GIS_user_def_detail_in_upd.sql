SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_user_def_detail_in_upd'
GO

CREATE PROCEDURE spe_GIS_user_def_detail_in_upd
    @GIS_user_def_detail_id int,
    @GIS_user_def_detail_inds_id int,
    @value char(1)

AS
BEGIN

UPDATE GIS_user_def_detail_inds
    SET
    value=@value

WHERE GIS_user_def_detail_id = @GIS_user_def_detail_id AND GIS_user_def_detail_inds_id = @GIS_user_def_detail_inds_id

END

GO

