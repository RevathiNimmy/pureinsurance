SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_risk_type_ri_values_add'
GO

CREATE PROCEDURE spe_risk_type_ri_values_add  
    @risk_type_id int,  
    @gis_user_def_header_inds_id1 int,  
    @gis_user_def_header_inds_id2 int,  
    @gis_user_def_header_inds_id3 int,  
    @value numeric(19,4),
    @risk_type_ri_limit_version_id int,
	@UserId INT = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL  
	
AS  
  
BEGIN  
INSERT INTO risk_type_ri_values (  
    risk_type_id ,  
    gis_user_def_header_inds_id1 ,  
    gis_user_def_header_inds_id2 ,  
    gis_user_def_header_inds_id3 ,  
    value,
    risk_type_ri_limit_version_id,
	UserId,
	UniqueId,
	ScreenHierarchy)  
VALUES (  
    @risk_type_id ,  
    @gis_user_def_header_inds_id1 ,  
    @gis_user_def_header_inds_id2 ,  
    @gis_user_def_header_inds_id3 ,  
    @value,
    @risk_type_ri_limit_version_id,
	@UserId,
	@UniqueId,
	@ScreenHierarchy)  
END  

GO

