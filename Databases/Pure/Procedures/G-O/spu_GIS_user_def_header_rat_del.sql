SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_user_def_header_rat_del'
GO


CREATE PROCEDURE spu_GIS_user_def_header_rat_del
    @GIS_user_def_header_id int
AS


DELETE FROM GIS_user_def_header_rates

WHERE GIS_user_def_header_id = @GIS_user_def_header_id
GO


