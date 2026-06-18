SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_Insert_Standard_Wording'
GO
/*********************************************************************************************************/
/* spu_SAM_Insert_Standard_Wording - Inserts the Standard_Wording entries for a given property 		 */
/*                                                                                                       */
/* RDT 7/6/2007                                                                                          */
/*********************************************************************************************************/
CREATE PROCEDURE spu_SAM_Insert_Standard_Wording      
    @gis_datamodel_code varchar(30),      
    @gis_policy_binder_id int,      
    @gis_object_name varchar(255),      
    @gis_property_name varchar(255),      
    @document_template_code varchar(255),      
    @Sequence int = 0,      
    @parent_key_name varchar(255) = NULL,      
    @parent_key_value varchar(255) = NULL,
    @documenttemplateid INT=0      
      
AS      
BEGIN      
      
DECLARE @SQL varchar(1000)      
DECLARE @gis_object_id int      
DECLARE @gis_property_id int      
DECLARE @document_template_id int      
DECLARE @NSQL NVARCHAR(1000)          
SELECT @gis_datamodel_code = RTRIM(@gis_datamodel_code)      
      
IF @documenttemplateid = 0 BEGIN  
SELECT @document_template_id = document_template_id FROM Document_Template WHERE code = @document_template_code AND is_deleted <> 1 
END
ELSE BEGIN
SELECT @document_template_id = @documenttemplateid
END

IF  @document_template_id<0  
 BEGIN  
  IF (@documenttemplateid<>0)  
   BEGIN  
    DECLARE @RecordFound INT  
    SELECT @NSQL = N'SELECT @iCount=COUNT( ' + @gis_datamodel_code + '_policy_binder_id) FROM ' + @gis_datamodel_code + '_standard_wording '  
    SELECT @NSQL = @NSQL + 'WHERE ' + @gis_datamodel_code + '_policy_binder_id = ' + CONVERT(VARCHAR,@gis_policy_binder_id)  
    EXEC sp_executesql  
    @query = @NSQL,  
    @params = N'@iCount INT OUTPUT',  
    @iCount = @RecordFound OUTPUT  
    IF @RecordFound=0  
     BEGIN  
      SELECT @document_template_id = @documenttemplateid  
     END  
    ELSE  
     BEGIN  
      SELECT @NSQL = N'SELECT @Doc_id=MIN(sw.document_template_id) FROM ' + @gis_datamodel_code + '_standard_wording sw'  
      SELECT @NSQL = @NSQL + ' LEFT OUTER JOIN Document_Template dt ON dt.document_template_id = sw.document_template_id '  
         SELECT @NSQL = @NSQL + ' WHERE dt.code=''' + @document_template_code + ''' AND sw.' + @gis_datamodel_code + '_policy_binder_id='+ CONVERT(VARCHAR,@gis_policy_binder_id) + ' AND sw.document_template_id=' + CONVERT(VARCHAR,@documenttemplateid)   
      EXEC sp_executesql  
      @query = @NSQL,  
      @params = N'@Doc_id INT OUTPUT',  
      @Doc_id = @document_template_id OUTPUT  
     END  
    IF ISNULL(@document_template_id,'')=''  
    BEGIN  
     SELECT @document_template_id= @documenttemplateid  
    END  
    ELSE  
    BEGIN  
    IF  @document_template_id> @documenttemplateid  
      BEGIN  
       SELECT @SQL = 'UPDATE ' + @gis_datamodel_code + '_standard_wording SET DOCUMENT_TEMPLATE_ID=' + convert(varchar(10), @documenttemplateid)  
       SELECT @SQL = @SQL + ' WHERE  ' + @gis_datamodel_code + '_policy_binder_id = ' + CONVERT(VARCHAR,@gis_policy_binder_id)  
       SELECT @SQL = @SQL + ' AND DOCUMENT_TEMPLATE_ID=' + convert(varchar(10), @document_template_id)  
       EXEC (@SQL)  
       SELECT @document_template_id=@documenttemplateid  
      END  
    END  
   END  
  ELSE  
   BEGIN  
    SELECT @document_template_id = MIN(document_template_id) FROM Document_Template WHERE code = @document_template_code  
   END  
 END  
  
SELECT @gis_object_id = GIS_Object.gis_object_id,  
       @gis_property_id = GIS_Property.gis_property_id  
FROM  
       GIS_Property  
INNER JOIN GIS_Object ON GIS_Property.gis_object_id = GIS_Object.gis_object_id  
INNER JOIN GIS_Data_Model ON GIS_Object.gis_data_model_id = GIS_Data_Model.gis_data_model_id  
WHERE RTRIM(GIS_Object.object_name) = RTRIM(@gis_object_name)  
 AND RTRIM(GIS_Data_Model.code) = RTRIM(@gis_datamodel_code)  
 AND RTRIM(GIS_Property.property_name) = RTRIM(@gis_property_name)  
  
IF (@parent_key_name IS NULL) OR (@parent_key_name = '')  
BEGIN  
 SELECT @SQL = 'INSERT INTO ' + @gis_datamodel_code + '_standard_wording '  
 SELECT @SQL = @SQL + '(' + @gis_datamodel_code + '_policy_binder_id, sequence_id, document_template_id, gis_property_id, gis_object_id) '  
 SELECT @SQL = @SQL + 'VALUES (' + convert(varchar(10), @gis_policy_binder_id) + ', ' + convert(varchar(10), @Sequence) + ', ' + convert(varchar(10), @document_template_id) + ', ' + convert(varchar(10), @gis_property_id) + ', ' + convert(varchar(10), @gis_object_id) + ')'  
END  
ELSE  
BEGIN  
 SELECT @SQL = 'INSERT INTO ' + @gis_datamodel_code + '_standard_wording '  
 SELECT @SQL = @SQL + '(' + @gis_datamodel_code + '_policy_binder_id, sequence_id, document_template_id, gis_property_id, gis_object_id, child,' +  @parent_key_name + ') '  
 SELECT @SQL = @SQL + 'VALUES (' + convert(varchar(10), @gis_policy_binder_id) + ', ' + convert(varchar(10), @Sequence) + ', ' + convert(varchar(10), @document_template_id) + ', ' + convert(varchar(10), @gis_property_id) + ', ' + convert(varchar(10), @gis_object_id) + ', 1, ' +  @parent_key_value + ')'  
END  
EXEC (@SQL)  
--SELECT @SQL  
END  

