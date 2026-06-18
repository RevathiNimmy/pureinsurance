SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_gis_get_retroactivedate_property'
GO

CREATE PROCEDURE spu_gis_get_retroactivedate_property        
   @insurance_file_cnt   INT,
   @risk_cnt			 INT =NULL
AS
BEGIN

DECLARE @SQL                  VARCHAR(2000),  
    	@object_name          VARCHAR(70) ,  
    	@property_name        VARCHAR(70) ,  
    	@table_name           VARCHAR(70) ,  
    	@column_name          VARCHAR(70) ,  
		@gis_data_model_code  VARCHAR(10) ,  
    	@policy_binder_id     INT,
		@gis_data_model_id    INT,
        @risk_id 	      	  INT,
        @insurance_folder_cnt INT
	
CREATE TABLE #Matches_Found (policy_binder_id int, object_name varchar(70), property_name varchar(70), value varchar(255))  

SELECT  @gis_data_model_id = gs.gis_data_model_id, 
        @risk_id = rsk.risk_cnt, 
		@insurance_folder_cnt = inf.insurance_folder_cnt,
		@gis_data_model_code = RTRIM(gdm.code)
FROM gis_data_model gdm
JOIN gis_screen gs
    ON gs.gis_data_model_id = gdm.gis_data_model_id
JOIN risk rsk
    ON rsk.gis_screen_id = gs.gis_screen_id
JOIN insurance_file_risk_link infrl
    ON infrl.risk_cnt = rsk.risk_cnt
JOIN insurance_file inf
    ON inf.insurance_file_cnt = infrl.insurance_file_cnt
AND inf.insurance_file_cnt = @insurance_file_cnt
AND infrl.risk_cnt = ISNULL(@risk_cnt,infrl.risk_cnt)

DECLARE c_gis_objects CURSOR FAST_FORWARD FOR  
SELECT [object_name],  
       table_name,  
       property_name,  
       column_name  
FROM   gis_object o  
       INNER JOIN gis_property p ON (o.gis_object_id = p.gis_object_id)  
WHERE  o.gis_data_model_id = @gis_data_model_id 
and [object_name]= 'S4IDEFAULT'  

/* Then Loop Round the Cursor and Do the Searches */  
OPEN c_gis_objects  

FETCH NEXT FROM c_gis_objects  
INTO    @object_name,  
        @table_name ,  
        @property_name,  
        @column_name  

WHILE (@@FETCH_STATUS = 0)  
BEGIN  
    SELECT @SQL = 'INSERT INTO #Matches_Found (Policy_Binder_id, object_name, property_name, value) SELECT ' + @gis_data_model_code + '_policy_binder_id, ' + '''' + @object_name + '''' +' , ' + '''' + @property_name + '''' + ' ,' + @column_name  
    SELECT @SQL = @SQL + ' FROM ' + @table_name 
	EXEC(@SQL)
    FETCH NEXT FROM c_gis_objects  
    INTO  @object_name,  
		  @table_name ,  
    	  @property_name,  
          @column_name  
END

CLOSE c_gis_objects  
DEALLOCATE c_gis_objects  

SELECT m.value , rsk.inception_date
FROM gis_policy_link gpl 
JOIN #Matches_Found m
    ON gpl.gis_policy_link_id = m.policy_binder_id
JOIN risk rsk
	ON gpl.risk_id=rsk.risk_cnt
WHERE 
    gpl.insurance_file_cnt = @insurance_folder_cnt
    AND gpl.risk_id = @risk_id
    AND gpl.gis_data_model_id= @gis_data_model_id
    AND m.property_name = 'Retroactive_Date'
    AND m.object_name = 'S4IDEFAULT'

DROP TABLE #Matches_Found 

END        
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
