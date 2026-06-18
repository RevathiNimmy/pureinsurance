SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_user_def_detail_ra_add'
GO

CREATE PROCEDURE spe_GIS_user_def_detail_ra_add
    @GIS_user_def_detail_id int,
    @GIS_user_def_detail_rates_id int,
    @value numeric(19,4)

AS

BEGIN
INSERT INTO GIS_user_def_detail_rates (
    GIS_user_def_detail_id ,
    GIS_user_def_detail_rates_id ,
    value )
VALUES (
    @GIS_user_def_detail_id,
    @GIS_user_def_detail_rates_id,
    @value)
END

GO

