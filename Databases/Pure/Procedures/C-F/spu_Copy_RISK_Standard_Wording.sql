SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

EXEC DDLDropProcedure 'spu_Copy_RISK_Standard_Wording'
GO

CREATE PROCEDURE spu_Copy_RISK_Standard_Wording  
@data_model VARCHAR(60) ,  
@new_policy_binder INT,  
@old_policy_binder INT,  
@gis_prop_id INT,  
@gis_obj_id INT,  
@doc_template_id INT,  
@new_doc_template_id INT = 0,  
@seq_id INT,  
@isChild INT,  
@childId INT=NULL AS  
DECLARE @RiskAddress_Id INT  
DECLARE @objName VARCHAR(2000)  
DECLARE @field_name VARCHAR(50)  
DECLARE @field_value int  
DECLARE @parmDef NVARCHAR (1000)  
DECLARE @SQL NVARCHAR (2500) 
IF (@new_doc_template_id = 0)  
BEGIN  
SET  
 @new_doc_template_id = @doc_template_id 
END  
CREATE TABLE #clause5 (risk_address int)  
CREATE TABLE #tmp_object (tmp_value int)
If (@isChild =0)  OR (@isChild =1)
BEGIN  
SELECT  
  @objName = Object_Name  
FROM  
  GIS_Object  
WHERE  
  GIS_Object_Id = @gis_obj_id If (ISNULL (@objName, '') <> '')  
BEGIN  
SELECT  
  @field_name = @data_model + '_' + @objName + '_Id'  
SET  
  @parmDef = N'@fvalue int out'  
  --SELECT @SQL = N'Select  @fvalue = '  + @field_name + ' From ' + @data_model + '_standard_wording'    
SELECT  
  @SQL = N'Select  isnull(' + @field_name + ',0) as ' + @field_name + ' From ' + @data_model + '_standard_wording'  
SELECT  
  @SQL = @SQL + ' Where ' + @data_model + '_Policy_binder_id = ' + cast(@old_policy_binder AS varchar)  
SELECT  
  @SQL = @SQL + ' AND Sequence_Id = ' + cast(@seq_id AS varchar) 
  IF @isChild = 1 and (ISNULL (@childId, '') <> '') 
BEGIN  
 
SELECT  
  @SQL = @SQL + ' AND Document_Template_Id = ' + cast(@childId AS varchar) END
  ELSE  
BEGIN
 
SELECT  
  @SQL = @SQL + ' AND Document_Template_Id = ' + cast(@doc_template_id AS varchar) END  
SELECT  
  @SQL = @SQL + ' AND GIS_Object_Id = ' + cast(@gis_obj_id AS varchar)  
SELECT  
  @SQL = @SQL + ' AND GIS_Property_Id = ' + cast(@gis_prop_id AS varchar)  
  --Exec SP_ExecuteSQL @SQL, @ParmDef  ,@fvalue=@field_value out  
 
select @SQL =  + 'INSERT INTO  #clause5  '  +  @SQL
 
  Exec SP_ExecuteSQL @SQL END END

IF (SELECT COUNT(*) FROM #clause5) = 0
    BEGIN
        -- Construct the dynamic SQL for selecting from the original document template
                SELECT @SQL = N'Select TOP 1 isnull(' + @field_name + ',0) as ' + @field_name + ' From ' + @data_model + '_standard_wording'  
                SELECT @SQL = @SQL + ' WHERE Document_Template_Id = (SELECT original_document_template_id FROM Document_Template WHERE document_template_id = ' + CAST(@doc_template_id AS VARCHAR) + ')'
                SELECT @SQL =  + 'INSERT INTO  #clause5  '  +  @SQL
      
        -- Execute the dynamic SQL
        EXEC SP_ExecuteSQL @SQL
    END

DECLARE @isDuplicate int  
DECLARE outer_cursor CURSOR FAST_FORWARD FOR  
SELECT  
  risk_address  
FROM  
  #clause5   
OPEN outer_cursor  
FETCH NEXT  
FROM  
  outer_cursor INTO @RiskAddress_Id WHILE @@FETCH_STATUS = 0  
BEGIN  
SET  
  @SQL = 'Insert into #tmp_object SELECT isnull(COUNT(*),0)'  
SET  
  @SQL = @SQL + ' FROM ' + @data_model + '_standard_wording'  
SET  
  @SQL = @SQL + ' WHERE ' + @data_model + '_Policy_Binder_Id = ' + convert(varchar, @new_policy_binder)  
SET  
  @SQL = @SQL + ' AND Document_Template_Id = ' + convert(varchar, @new_doc_template_id)  
SET  
  @SQL = @SQL + ' AND isnull(' + @field_name + ',0) =' + convert(varchar, @RiskAddress_Id) 
SET
  @SQL= @SQL + ' AND GIS_Property_id = ' + convert(varchar, @gis_prop_id) 
SET
  @SQL= @SQL + ' AND GIS_Object_id = ' + convert(varchar, @gis_obj_id)
SET
@SQL= @SQL + ' AND GIS_Property_id = ' + convert(varchar, @gis_prop_id)
SET 
   @SQL= @SQL + ' AND child = ' + convert(varchar, @isChild)
 
  Exec SP_ExecuteSQL @SQL  
SELECT  
  @isDuplicate = ISNULL (tmp_value, 0)  
FROM  
  #tmp_object 
  IF (convert(int, @isDuplicate) = 0) -- Insert only if no duplicate found  
BEGIN  
SELECT  
  @SQL = 'INSERT INTO ' + RTRIM(@data_model) + '_standard_wording ('  
SELECT  
  @SQL = @SQL + RTRIM(@data_model) + '_Policy_binder_id, sequence_id, document_template_id, gis_property_id, gis_object_id, child' If (@isChild = 1)  
  AND (ISNULL (@objName, '') <> '')  
SELECT  
  @SQL = @SQL + ', ' + @field_name  
SELECT  
  @SQL = @SQL + ') '  
SELECT  
  @SQL = @SQL + 'Values (' + cast(@new_policy_binder AS varchar) + ', ' + cast(@seq_id AS varchar) + ', ' + cast(@new_doc_template_id AS varchar)  
SELECT  
  @SQL = @SQL + ', ' + cast(@gis_prop_id AS varchar) + ', ' + cast(@gis_obj_id AS varchar) + ', ' + cast(@isChild AS varchar) If (@isChild = 1)  
  AND (ISNULL (@objName, '') <> '')  
SELECT  
  @SQL = @SQL + ', ' + cast(@RiskAddress_Id AS varchar)  
SELECT  
  @SQL = @SQL + ')'
Exec SP_ExecuteSQL @SQL END 

DELETE FROM #tmp_object  
FETCH NEXT  
FROM  
  outer_cursor INTO @RiskAddress_Id END  
CLOSE outer_cursor  
DEALLOCATE outer_cursor  
DROP TABLE #clause5
GO


