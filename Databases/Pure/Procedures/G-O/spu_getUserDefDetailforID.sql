SET QUOTED_IDENTIFIER ON
GO
Execute DDLDropProcedure 'spu_getUserDefDetailforID'
GO
/*******************************************************************************************************/
/* spu_getUserDefDetailforID    */                                                                              
/*******************************************************************************************************/
CREATE PROCEDURE spu_getUserDefDetailforID
    @sCode VARCHAR(10),
    @sHeaderID VARCHAR(10)

AS

SELECT d.gis_user_def_detail_id
            FROM GIS_user_def_detail d 
			     JOIN GIS_user_def_header h 
            ON d.GIS_user_def_header_id = h.GIS_user_def_header_id
            WHERE d.Code = @sCode 
		          AND d.gis_user_def_header_id = @sHeaderID

SET QUOTED_IDENTIFIER OFF
GO

