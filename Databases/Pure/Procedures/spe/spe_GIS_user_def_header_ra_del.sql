SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_user_def_header_ra_del'
GO

CREATE PROCEDURE spe_GIS_user_def_header_ra_del
    @GIS_user_def_header_id int,
    @GIS_user_def_header_rates_id int
AS

DELETE FROM GIS_user_def_header_rates

WHERE GIS_user_def_header_id = @GIS_user_def_header_id AND GIS_user_def_header_rates_id = @GIS_user_def_header_rates_id

GO

