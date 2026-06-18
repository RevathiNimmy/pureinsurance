SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_check_GIS_user_def_detail'
GO


CREATE PROCEDURE spu_check_GIS_user_def_detail
    @GIS_user_def_header_id int,
    @code varchar(20)
AS


SELECT  Gis_user_def_detail_id
FROM    Gis_user_def_detail
WHERE   GIS_user_def_header_id = @GIS_user_def_header_id
AND code = @code
GO


