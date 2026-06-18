SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_user_def_header_ind_del'
GO


CREATE PROCEDURE spu_GIS_user_def_header_ind_del
    @GIS_user_def_header_id int,
	@user_id int,
	@unique_id varchar(50),
	@screen_hierarchy varchar(500)
AS

	UPDATE gis 
                SET ScreenHierarchy = CONCAT(@screen_hierarchy, '/Indicator(', LTRIM(RTRIM(gis.code)), ')'),  
                    UniqueId = @unique_id,  
					UserId = @user_id  
                from GIS_user_def_header_inds gis
                WHERE gis.gis_user_def_header_id = @GIS_user_def_header_id 

DELETE FROM GIS_user_def_header_inds

WHERE GIS_user_def_header_id = @GIS_user_def_header_id
GO


