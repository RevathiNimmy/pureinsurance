SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_user_def_detail_ra_upd'
GO

CREATE PROCEDURE spe_GIS_user_def_detail_ra_upd
    @GIS_user_def_detail_id int,
    @GIS_user_def_detail_rates_id int,
    @value numeric(19,4)

AS
BEGIN

UPDATE GIS_user_def_detail_rates
    SET
    value=@value

WHERE GIS_user_def_detail_id = @GIS_user_def_detail_id AND GIS_user_def_detail_rates_id = @GIS_user_def_detail_rates_id

END

GO

