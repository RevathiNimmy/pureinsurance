EXEC DDLDropProcedure 'spu_Party_Get_GIS_CustomData'
GO

CREATE PROCEDURE spu_Party_Get_GIS_CustomData       
  @party_cnt INT,        
  @gis_screen_id INT,      
  @gis_policy_link_id INT,      
  @ifound INT OUTPUT        
AS        
        
/*        
 Return the associated GIS_CustomData for the Party,Screen Id and gis_policy_id passed in.        
 If the custom data is found then return 1 else 0      
*/        
        
DECLARE @gis_data_model_id INT    
DECLARE @code VARCHAR(10)      
DECLARE @main_Table_Name VARCHAR(100)      
DECLARE @Table_Name VARCHAR(100)      
DECLARE @Field_Name VARCHAR(255)      
DECLARE @sSQL NVARCHAR(1000)      
DECLARE @nCount INT      
DECLARE @policy_binder_id INT      
      
SET NOCOUNT ON    
    
SELECT @gis_data_model_id = gis_data_model_id from GIS_Screen where gis_screen_id = @gis_screen_id      
      
SELECT @code=code from GIS_Data_Model where gis_data_model_id = @gis_data_model_id      
      
SELECT @Main_Table_Name = RTRIM(@code)+ '_Policy_Binder'      
      
SELECT @Field_Name = RTRIM(@code)+ '_Policy_Binder_Id'      
      
SELECT @policy_binder_id = @gis_policy_link_id        
      
DECLARE curTable CURSOR FAST_FORWARD FOR      
SELECT table_name FROM GIS_Object       
WHERE gis_data_model_id = @gis_data_model_id and is_selectable_for_screen = 1      
    
OPEN curTable      
      
SELECT @ifound = 0       
      
FETCH NEXT FROM curTable INTO @Table_Name      
      
WHILE @@FETCH_STATUS = 0      
BEGIN      
    SELECT @sSQL = N'SELECT @nCount = count(1) FROM ' + @Table_Name +  
    N' WHERE ' + @Field_Name + N' = ' + CONVERT(VARCHAR(10),@policy_binder_id)  
      
    EXEC sp_executesql @sSQL,N'@nCount INT OUTPUT', @nCount OUTPUT      
       
    if @nCount = 1      
        SELECT @ifound=1      
       
    FETCH NEXT FROM curTable INTO @Table_Name      
END      
      
CLOSE curTable      
DEALLOCATE curTable      
    
SET NOCOUNT OFF    
      
SELECT @ifound

GO