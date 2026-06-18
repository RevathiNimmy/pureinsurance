SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Clean_Up_Dirty_Cases'
GO


CREATE PROCEDURE spu_CLM_Clean_Up_Dirty_Cases  

@case_id int
  
AS  
  
BEGIN  
  
DECLARE @dirty_case_id int  
DECLARE @status int 
DECLARE @Code VARCHAR(250)
DECLARE @sSQL NVARCHAR(250)
DECLARE @TableName VARCHAR(250)
DECLARE @TableNameExist INT

DECLARE dirty_cases CURSOR FOR  
 SELECT case_id  
 FROM [case]  
 WHERE base_case_id NOT IN (  
   SELECT lock_value  
   FROM pmlock  
   WHERE lock_name = 'case_id')  
 AND is_dirty_case = 1  
 AND base_case_id IN ( 
 	SELECT base_case_id 
	FROM [case] 
	WHERE case_id = @case_id)
  
OPEN dirty_cases  
  
FETCH NEXT FROM dirty_cases  
INTO @dirty_case_id  
  
WHILE @@FETCH_STATUS = 0  
BEGIN  

------------------------------------------  
-- delete associated event log entries  
------------------------------------------ 

DELETE event_log  
 WHERE case_id = @dirty_case_id  

  
IF @@ERROR <> 0  
    GOTO Error_Routine    

------------------------------------------  
-- Delete from  CASE_Policy_Binder
------------------------------------------  

Declare Policy_Binder_cursor cursor for
   SELECT GDM.CODE from gis_data_model GDM,gis_data_model_type GDMT 
        WHERE GDM.gis_data_model_type_id=GDMT.gis_data_model_type_id AND GDMT.CODE='CASE'	

Open Policy_Binder_Cursor  
    Fetch Next From Policy_Binder_Cursor Into @Code
WHILE @@FETCH_STATUS = 0  
BEGIN  
SET @TableName = LTRIM(RTRIM(@Code)) + '_Policy_Binder'
EXEC DDLExistSTable @TableName, @TableNameExist OUTPUT

If ISNULL(@TableNameExist,0)<> 0 
BEGIN
SET @sSQL = 'Delete ' + @TableName +' WHERE gis_policy_link_id=(SELECT gis_policy_link_id 
				FROM gis_policy_link WHERE case_id= '+ LTRIM(RTRIM(@dirty_case_id)) +')'    
EXECUTE sp_executesql @SSQL
END

 Fetch Next From Policy_Binder_Cursor Into @Code 
END  
Close Policy_Binder_Cursor  
Deallocate  Policy_Binder_Cursor

------------------------------------------  
-- Delete from  GIS_Policy_Link
------------------------------------------  

Delete GIS_Policy_Link
  WHERE case_id=@dirty_case_id

------------------------------------------  
-- Delete from case 
------------------------------------------  
DELETE  [case] 
WHERE   case_id = @dirty_case_id  
  
IF @@ERROR <> 0  
    GOTO Error_Routine  
  
  
SELECT @status = 0   
 
  
   -- Get the next author.  
   FETCH NEXT FROM dirty_cases  
   INTO @dirty_case_id  
END  
  
CLOSE dirty_cases  
DEALLOCATE dirty_cases  
  
END  

RETURN  
  
Error_Routine:  
  
    SELECT  @status = -1  
    RETURN  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
