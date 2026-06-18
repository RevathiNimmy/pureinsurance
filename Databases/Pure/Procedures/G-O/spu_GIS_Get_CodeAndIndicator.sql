SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXEC DDLDropProcedure 'spu_GIS_Get_CodeAndIndicator'
GO

CREATE PROCEDURE spu_GIS_Get_CodeAndIndicator  
	@LookupValue  int
AS  

SELECT d.code, null
FROM GIS_user_def_detail d,
     GIS_User_Def_Header GUDH
WHERE d.GIS_user_def_detail_id = @LookupValue 
AND d.gis_user_def_header_inds_id IS NULL
AND d.gis_user_def_header_id = GUDH.gis_user_def_header_id

UNION
SELECT d.code, h.code
FROM GIS_user_def_detail d,
     GIS_user_def_header_inds h,
     GIS_User_Def_Header GUDH
WHERE d.GIS_user_def_detail_id =  @LookupValue 
AND d.GIS_user_def_header_id = h.GIS_user_def_header_id
AND d.gis_user_def_header_inds_id = h.gis_user_def_header_inds_id
AND d.gis_user_def_header_id = GUDH.gis_user_def_header_id

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO