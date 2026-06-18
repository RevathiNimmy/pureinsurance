SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SIR_Check_Mandatory_Que'
GO

CREATE PROCEDURE spu_SIR_Check_Mandatory_Que  
 @que_type AS VARCHAR(10),  
 @risk_cnt INT  
AS  
BEGIN  
   DECLARE @risk_screen_id  INT, @new_que_type VARCHAR(10)
    Select @risk_screen_id = gis_screen_id from risk r where r.risk_cnt = @risk_cnt
    
    SET @new_que_type = @que_type
    
    IF @new_que_type='pre_quote'
		SELECT o.table_name, p.column_name, sd.PMFormat, p.data_type   
		FROM gis_screen_detail sd JOIN gis_object o on sd.gis_object_id = o.gis_object_id 
		JOIN gis_property p ON (sd.gis_property_id = p.gis_property_id AND p.gis_object_id = o.gis_object_id)
		WHERE sd.gis_screen_id IN 
		(Select gis_screen_id FROM gis_screen WHERE 
		   gis_screen_id = @risk_screen_id  OR parent_id =@risk_screen_id)    
		AND sd.pre_quote_requirement = 2
		ORDER BY table_name,column_name
   ELSE IF @new_que_type='post_quote'  
   		SELECT o.table_name, p.column_name, sd.PMFormat, p.data_type   
		FROM gis_screen_detail sd JOIN gis_object o on sd.gis_object_id = o.gis_object_id 
		JOIN gis_property p ON (sd.gis_property_id = p.gis_property_id AND p.gis_object_id = o.gis_object_id)
		WHERE sd.gis_screen_id IN 
		(Select gis_screen_id FROM gis_screen WHERE 
		   gis_screen_id = @risk_screen_id  OR parent_id =@risk_screen_id)    
		AND sd.post_quote_requirement = 2
		ORDER BY table_name,column_name
	ELSE IF @new_que_type='purchase'		
		SELECT o.table_name, p.column_name, sd.PMFormat, p.data_type   
		FROM gis_screen_detail sd JOIN gis_object o on sd.gis_object_id = o.gis_object_id 
		JOIN gis_property p ON (sd.gis_property_id = p.gis_property_id AND p.gis_object_id = o.gis_object_id)
		WHERE sd.gis_screen_id IN 
		(Select gis_screen_id FROM gis_screen WHERE 
		   gis_screen_id = @risk_screen_id  OR parent_id =@risk_screen_id)    
		AND sd.purchase_requirement = 2
		ORDER BY table_name,column_name
END  