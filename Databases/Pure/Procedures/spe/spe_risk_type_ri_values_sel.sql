SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_risk_type_ri_values_sel'
GO

CREATE PROCEDURE spe_risk_type_ri_values_sel  
    @risk_type_id int,  
    @gis_user_def_header_inds_id1 int,  
    @gis_user_def_header_inds_id2 int,  
    @gis_user_def_header_inds_id3 int,
    @risk_type_ri_limit_version_id int  
AS  
  
BEGIN  
SELECT  risk_type_id,  
    gis_user_def_header_inds_id1,  
    gis_user_def_header_inds_id2,  
    gis_user_def_header_inds_id3,  
    value  
FROM    risk_type_ri_values  
WHERE   risk_type_id = @risk_type_id  
AND gis_user_def_header_inds_id1 = @gis_user_def_header_inds_id1  
AND gis_user_def_header_inds_id2 = @gis_user_def_header_inds_id2  
AND gis_user_def_header_inds_id3 = @gis_user_def_header_inds_id3
AND risk_type_ri_limit_version_id  =@risk_type_ri_limit_version_id
END  

GO

