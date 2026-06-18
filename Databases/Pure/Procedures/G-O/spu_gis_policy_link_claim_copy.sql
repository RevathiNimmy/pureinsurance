EXECUTE DDLDROPPROCEDURE spu_gis_policy_link_claim_copy  
GO

CREATE PROCEDURE spu_gis_policy_link_claim_copy  
    @gis_policy_link_id INT ,  
    @old_claim_id INT ,  
    @new_claim_id INT   
  
AS 
 
BEGIN  
  
SET IDENTITY_INSERT GIS_Policy_Link ON  
  
INSERT INTO GIS_Policy_Link(  
	 gis_policy_link_id, 
	 quote_ref,
	 quote_ref_password,
	 gis_data_model_id ,  
	 claim_id)  
SELECT  
	gis_policy_link_id,  
	quote_ref,
	quote_ref_password,
	gis_data_model_id,  
	claim_id= @new_claim_id  
FROM  GIS_Policy_Link  
WHERE gis_policy_link_id=@gis_policy_link_id  
AND claim_id=@old_claim_id  
  
END  
  
SET IDENTITY_INSERT GIS_Policy_Link OFF  
GO