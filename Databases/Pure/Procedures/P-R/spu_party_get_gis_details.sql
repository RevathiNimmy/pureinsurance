SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_party_get_gis_details'
GO

CREATE PROCEDURE spu_party_get_gis_details    
 @party_cnt INT,      
 @party_type_id int OUTPUT,     
 @gis_screen_id INT OUTPUT,     
 @data_model_code varchar(10) OUTPUT,     
 @data_model_type_id int OUTPUT,   
 @party_type_code varchar(10) OUTPUT  
       
AS      
       
SELECT      
 @gis_screen_id = party_type.gis_screen_id,      
 @party_type_id = party.party_type_id,      
 @data_model_type_id = gis_data_model.gis_data_model_type_id,     
 @data_model_code = gis_data_model.code,   
 @party_type_code = party_type.code    
    
FROM party      
    
 INNER JOIN party_type ON     
  party.party_type_id = party_type.party_type_id    
      
 INNER JOIN gis_screen  ON     
  gis_screen.gis_screen_id = party_type.gis_screen_id      
    
 LEFT JOIN gis_data_model ON     
  gis_data_model.gis_data_model_id = gis_screen.gis_data_model_id      
    
WHERE party.party_cnt = @party_cnt     
AND gis_screen.is_deleted = 0      
    
  


GO
