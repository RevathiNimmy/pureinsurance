SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_SIR_Is_Case_Screen_Data_Model_Changed'
GO

CREATE PROCEDURE spu_SIR_Is_Case_Screen_Data_Model_Changed  
 @case_id INT,  
 @previous_data_model_id int OUTPUT,  
 @gis_policy_link_id int OUTPUT 
 
AS  
BEGIN  
    --Fetch previous model code from GIS_POLICY_LINK  
    SELECT @previous_data_model_id = GPL.gis_data_model_id,  
           @gis_policy_link_id = GPL.gis_policy_link_id  
    FROM GIS_Policy_Link GPL  
    INNER JOIN GIS_Data_Model GDM ON GPL.gis_data_model_id=GDM.gis_data_model_id  
    INNER JOIN gis_data_model_type GDMT on GDMT.gis_data_model_type_id = GDM.gis_data_model_type_id  
    WHERE GPL.case_id = @case_id AND GDMT.code = 'CASE'  
  
    If (@previous_data_model_id IS NULL)  
       SET @previous_data_model_id = 0  
    Else  
        --Check if existing party type model is different  
	IF EXISTS (SELECT GDM.gis_data_model_id FROM system_options SO   
	           INNER JOIN GIS_Screen GS ON GS.gis_screen_id = cast(SO.value AS INT)
		   AND SO.option_number = 5035
	           LEFT JOIN GIS_Data_Model GDM ON GDM.gis_data_model_id = GS.gis_data_model_id  
	           WHERE GDM.gis_data_model_id = @previous_data_model_id)  
	BEGIN
	    SET @previous_data_model_id = 0  
	END  

END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


