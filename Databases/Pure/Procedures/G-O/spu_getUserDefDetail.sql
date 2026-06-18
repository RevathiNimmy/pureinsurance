SET QUOTED_IDENTIFIER ON
GO
Execute DDLDropProcedure 'spu_getUserDefDetail'
GO
/*******************************************************************************************************/
/* spu_getUserDefDetail    */                                                                              
/*******************************************************************************************************/
CREATE PROCEDURE spu_getUserDefDetail
    @sCode VARCHAR(10),
    @sHeaderCode VARCHAR(10)

AS
SELECT d.gis_user_def_detail_id
            FROM GIS_user_def_detail d JOIN
                 GIS_user_def_header h 
            ON d.GIS_user_def_header_id = h.GIS_user_def_header_id
            WHERE d.Code = @sCode  
			      AND h.code = @sHeaderCode

SET QUOTED_IDENTIFIER OFF
GO
