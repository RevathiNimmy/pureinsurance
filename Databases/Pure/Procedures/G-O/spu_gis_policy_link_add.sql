SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_gis_policy_link_add'
GO

CREATE PROCEDURE spu_gis_policy_link_add  
    @gis_policy_link_id INT OUTPUT,  
    @gis_data_model_id INT OUTPUT,  
    @gis_data_model_code CHAR(10),  
    @insurance_file_cnt INT = NULL,  
    @claim_id INT = NULL,  
    @party_cnt INT = NULL,  
    @risk_id INT = NULL,
    @case_id INT = NULL

  
AS  
BEGIN  
  
SELECT  @gis_data_model_id = gis_data_model_id  
FROM    gis_data_model  
WHERE   code = @gis_data_model_code  
  
INSERT INTO GIS_Policy_Link (  
    gis_data_model_id,  
    insurance_file_cnt,  
    claim_id,  
    party_cnt,  
    risk_id,
    case_id)  
VALUES (  
    @gis_data_model_id,  
    @insurance_file_cnt,  
    @claim_id,  
    @party_cnt,  
    @risk_id,
    @case_id)  
SELECT @gis_policy_link_id = @@IDENTITY  
END  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
