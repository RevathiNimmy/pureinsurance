SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_user_def_detail_ind_del'
GO


CREATE PROCEDURE spu_GIS_user_def_detail_ind_del
    @GIS_user_def_detail_id int
AS

/* End of screen generator stored procedures */

/* Star of user defined lookup maintenance routines */
DELETE FROM GIS_user_def_detail_inds

WHERE GIS_user_def_detail_id = @GIS_user_def_detail_id
GO


