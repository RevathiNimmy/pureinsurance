
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_WPField_Details'
GO
 

CREATE PROCEDURE spu_get_WPField_Details
	@CCMWPFields CCMWPFields READONLY,
	@Insurance_file_cnt INTEGER,  
 	@Claim_id INTEGER = 0  
AS        
        
BEGIN
		SELECT DISTINCT WP.table_name,[SQL],main_group, sub_group,data_model, loop1, loop2, loop3,
           CASE specials_type
                     WHEN  5 THEN 5
                     ELSE 0  end specials_type
		FROM wp_fields WP
		JOIN @CCMWPFields F ON WP.Table_name = F.Table_name
			AND WP.column_name = F.column_name    
		WHERE
			(WP.Data_Model IN (SELECT gdm.Code FROM insurance_file ifl JOIN Insurance_file_risk_link ifrl ON ifl.insurance_file_cnt = ifrl.insurance_file_cnt
			JOIN Risk ON ifrl.risk_cnt = Risk.risk_cnt 
			JOIN risk_type ON Risk.Risk_type_id = risk_type.risk_type_id
			JOIN gis_screen ON risk_type.gis_screen_id= gis_screen.gis_screen_id
			JOIN gis_data_model gdm ON gis_screen.gis_data_model_id= gdm.gis_data_model_id 
			WHERE ifl.insurance_file_cnt = @Insurance_file_cnt) OR Data_Model is NULL)
			or  
			(WP.Data_Model IN (SELECT gdm.Code FROM claim cc   
			JOIN gis_screen ON cc.gis_screen_id= gis_screen.gis_screen_id    
			JOIN gis_data_model gdm ON gis_screen.gis_data_model_id= gdm.gis_data_model_id    
			WHERE cc.claim_id = @Claim_id) OR Data_Model is NULL)   
              
		UNION
		  
		SELECT DISTINCT WP.table_name,[SQL],main_group, sub_group,data_model, loop1, loop2, loop3,
		CASE specials_type
					WHEN  5 THEN 5
					ELSE 0  end specials_type
		FROM wp_fields WP
			JOIN @CCMWPFields F
			ON WP.Table_name = F.Table_name
		WHERE F.Column_name in('FilePath','FileName','SWCODE','SWDESC')
		AND (WP.Data_Model IN (SELECT gdm.Code FROM insurance_file ifl JOIN Insurance_file_risk_link ifrl ON ifl.insurance_file_cnt = ifrl.insurance_file_cnt
			JOIN Risk ON ifrl.risk_cnt = Risk.risk_cnt 
			JOIN risk_type ON Risk.Risk_type_id = risk_type.risk_type_id
			JOIN gis_screen ON risk_type.gis_screen_id= gis_screen.gis_screen_id
			JOIN gis_data_model gdm ON gis_screen.gis_data_model_id= gdm.gis_data_model_id 
			WHERE ifl.insurance_file_cnt = @Insurance_file_cnt) OR Data_Model is NULL)
			or  
			(WP.Data_Model IN (SELECT gdm.Code FROM claim cc   
			JOIN gis_screen ON cc.gis_screen_id= gis_screen.gis_screen_id    
			JOIN gis_data_model gdm ON gis_screen.gis_data_model_id= gdm.gis_data_model_id    
			WHERE cc.claim_id = @Claim_id) OR Data_Model is NULL) 
		----Added Row in case of only single standard wording in the document

          
	
END  
GO