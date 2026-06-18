SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_SIR_Delete_GIS_Data
GO

CREATE PROCEDURE spu_SIR_Delete_GIS_Data
 @gis_policy_link_id INT  
AS  
BEGIN  
    DECLARE  
     @gis_table   VARCHAR(100),  
     @sSQL      VARCHAR(1000),  
     @gis_policy_binder_id  INT,  
     @data_model_id   INT,  
     @data_model_code  VARCHAR(10)  
  
    SELECT @data_model_id = gis_data_model_id 
    FROM GIS_Policy_Link  
    WHERE GIS_Policy_Link_Id = @gis_policy_link_id  
  
    SELECT @data_model_code = RTRIM(Code) 
    FROM Gis_Data_Model  
    WHERE gis_data_model_id = @data_model_id  
  
    -- Most unlikely just for resilience  
    IF (@data_model_code IS NULL)  
    RETURN
  
    DECLARE c_gis_tables CURSOR FAST_FORWARD FOR  
        SELECT 
            go.table_name 
        FROM 
            gis_object go
            LEFT OUTER JOIN gis_object gp  
                ON go.parent_object_id = gp.gis_object_id 
        WHERE 
            go.gis_data_model_id = @data_model_id 
        ORDER BY 
            go.parent_object_id DESC  
  
    -- Open the Peril Cursor  
    OPEN c_gis_tables  
  
    FETCH NEXT FROM c_gis_tables INTO @gis_table  
  
    WHILE (@@FETCH_STATUS = 0)  
    BEGIN  
        SET @sSQL = 'DELETE FROM ' + @gis_table  
        SET @sSQL = @sSQL + ' WHERE ' + @data_model_code + '_Policy_Binder_Id IN '  
        SET @sSQL = @sSQL + '(Select ' + @data_model_code + '_Policy_Binder_Id FROM '  
        SET @sSQL = @sSQL + @data_model_code + '_Policy_Binder WHERE gis_policy_link_id = '  
        SET @sSQL = @sSQL + CAST(@gis_policy_link_id AS VARCHAR) + ')'  
        
        EXEC (@sSQL)  
  
        -- Fetch Next  
        FETCH NEXT FROM c_gis_tables INTO @gis_table  
    END  
  
    -- Delete GIS Policy Link  
    DELETE FROM GIS_Policy_Link  
    WHERE GIS_Policy_Link_id = @gis_policy_link_id  

END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO