Execute DDLDropProcedure 'spu_SAM_CLM_Get_Claim_Risk_Screen_Details'
GO
CREATE  PROCEDURE spu_SAM_CLM_Get_Claim_Risk_Screen_Details
    @BaseClaim_id INT,
    @Claim_id INT = 0,
	@bIgnoreIsDirty TINYINT =0
AS
SELECT  TOP 1
        claim_id,
        risk.risk_type_id,
        claim.gis_screen_id,
        GIS_Data_Model.code 'gis_data_model_code',
        claim.transaction_type_id,
        transaction_type.code 'transaction_type_code',
        insurance_file_cnt
        FROM claim
        INNER JOIN risk ON claim.risk_type_id = risk.risk_cnt
        INNER JOIN insurance_file_risk_link ON risk.risk_cnt = insurance_file_risk_link.risk_cnt
        INNER JOIN gis_screen ON claim.gis_screen_id = gis_screen.gis_screen_id
        INNER JOIN GIS_Data_Model ON gis_screen.gis_data_model_id = GIS_Data_Model.gis_data_model_id
        INNER JOIN transaction_type ON claim.transaction_type_id = transaction_type.transaction_type_id
WHERE  ( 
          (base_claim_id = @BaseClaim_id AND @Claim_id=0) OR  (@Claim_id<>0 AND Claim_id=@Claim_id)
		)         
          And ((@bIgnoreIsDirty=1 AND is_dirty=0) OR @bIgnoreIsDirty=0)

ORDER BY version_id DESC       
  
GO




