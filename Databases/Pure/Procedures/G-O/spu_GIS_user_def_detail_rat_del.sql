SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_user_def_detail_rat_del'
GO


CREATE PROCEDURE spu_GIS_user_def_detail_rat_del
    @GIS_user_def_detail_id int
AS


DELETE FROM GIS_user_def_detail_rates

WHERE GIS_user_def_Detail_id = @GIS_user_def_Detail_id
GO


