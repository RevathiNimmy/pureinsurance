SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_user_def_header_ra_add'
GO

CREATE PROCEDURE spe_GIS_user_def_header_ra_add
    @GIS_user_def_header_id int,
    @GIS_user_def_header_rates_id int,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime
AS

BEGIN
INSERT INTO GIS_user_def_header_rates (
    GIS_user_def_header_id ,
    GIS_user_def_header_rates_id ,
    caption_id ,
    code ,
    description ,
    is_deleted ,
    effective_date )
VALUES (
    @GIS_user_def_header_id,
    @GIS_user_def_header_rates_id,
    @caption_id,
    @code,
    @description,
    @is_deleted,
    @effective_date)
END

GO

