SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_PLICO_gis_search_property_find
GO
CREATE PROCEDURE spu_PLICO_gis_search_property_find    
 @PartyCNT int,    
 @SystemDT DateTime = NULL  
AS    
DECLARE @SQL                          NVARCHAR(4000),    
 @print                            VARCHAR(255),    
 @gis_data_model_code              VARCHAR(10),    
 @object_name                      VARCHAR(70),    
 @property_name                    VARCHAR(70),    
 @table_name                       VARCHAR(70),    
 @column_name                      VARCHAR(70),    
 @policy_binder_id                 INTEGER,    
 @PrimaryPolicy_ColumnName    VARCHAR(50),    
 @PrimaryPolicy_TableName    VARCHAR(50),    
 @Deleted_ColumnName         VARCHAR(50),    
 @Deleted_TableName      VARCHAR(50),    
    @ModelObjectName                  VARCHAR(50),    
    @Risk_Type                        VARCHAR(50),    
    @Col_Policy_Binder                VARCHAR(50),    
    @Current_Policy_Binder            INT    
IF @SystemDT IS NULL OR @SystemDT = ''    
 SELECT @SystemDT = GETDATE()    
CREATE TABLE #Matches_Found (ID int IDENTITY, policy_binder_id int,    
  object_name varchar(70), property_name varchar(70),    
  value varchar(255),Specials_Type Varchar(10),    
  Primary_Policy tinyint, Is_Deleted tinyint,    
  risk_type_code varchar(50))    
CREATE Table #Policy_Binders    
      (Policy_Binders_Fld INT)    
DECLARE c_search_properties CURSOR FAST_FORWARD FOR    
SELECT  object_name,    
        table_name,    
        property_name,    
        column_name,    
        GDM.Code    
FROM    gis_object o    
        INNER JOIN gis_property p    
              ON (o.gis_object_id = p.gis_object_id)    
        INNER JOIN gis_data_model GDM    
              ON (GDM.gis_data_model_id = O.gis_data_model_id)    
WHERE   Specials_Type = 3    
AND     GDM.gis_data_model_type_id=1    
OPEN c_search_properties    
FETCH NEXT FROM c_search_properties    
INTO       @object_name,    
           @table_name ,    
           @property_name,    
           @column_name,    
           @gis_data_model_code    
WHILE (@@FETCH_STATUS = 0)    
BEGIN    
 SELECT @SQL = "INSERT INTO #Matches_Found (Policy_Binder_id, object_name, property_name, value, Specials_Type, risk_type_code) "    
 SELECT @SQL = @SQL + "SELECT " + RTRIM(@gis_data_model_code) + "_policy_binder_id, '" + @object_name + "' , '" + @property_name + "' ," + @column_name  + ", 3,"    
 SELECT @SQL = @SQL + "(SELECT RT.code FROM risk_type RT INNER JOIN Risk R ON R.risk_type_id = RT.risk_type_id INNER JOIN GIS_POLICY_LINK GPL ON GPL.risk_id = r.risk_cnt WHERE GPL.gis_policy_link_id = " + RTRIM(@gis_data_model_code) + "_policy_binder_id)"
  
    
    SELECT @SQL = @SQL + " FROM " + @table_name + " WHERE " + @column_name + " = " + CONVERT(VARCHAR(100), @PartyCNT)    
 EXEC (@SQL)    
 SELECT @Col_Policy_Binder = RTRIM(@gis_data_model_code) + '_policy_binder_id',    
        @Risk_type = RTRIM(risk_type_code)    
           FROM  #Matches_Found    
    WHERE  ID = @@Identity    
    SELECT @SQL = "INSERT INTO #Policy_Binders "    
    SELECT @SQL = @SQL  + " SELECT " + RTRIM(@gis_data_model_code) + "_policy_binder_id"    
    SELECT @SQL = @SQL  + " FROM " + RTRIM(@table_name)    
    SELECT @SQL = @SQL  + " WHERE " + RTRIM(@table_name) + "."  + RTRIM(@column_name)  + "=" + CONVERT(VARCHAR(100),@PartyCNT)    
    DELETE FROM #Policy_Binders    
    EXEC(@SQL)    
    IF @Risk_type = 'PHY'    
       SELECT  @ModelObjectName = 'PHY'    
    ELSE IF    @Risk_type = 'ANC'    
  SELECT @ModelObjectName = 'ANC'    
    ELSE IF    @Risk_type = 'ENT'    
  SELECT @ModelObjectName = 'CORP'    
    ELSE IF    @Risk_type = 'ERSLOT'    
  SELECT @ModelObjectName = 'ERS'    
    ELSE IF    @Risk_type = 'RES'    
  SELECT @ModelObjectName = 'RES'    
 EXEC spu_get_gis_property_column_name    
        @gis_data_model_code  = @gis_data_model_code,    
        @gis_object_name  = @ModelObjectName,    
        @property_name   = 'Primary_Policy',    
        @table_name  = @PrimaryPolicy_TableName OUTPUT,    
        @Column_Name  = @PrimaryPolicy_ColumnName OUTPUT    
 EXEC spu_get_gis_property_column_name    
        @gis_data_model_code  = @gis_data_model_code,    
        @gis_object_name  = @ModelObjectName,    
        @property_name   = 'Deleted',    
      @table_name  = @Deleted_TableName OUTPUT,    
        @Column_Name  = @Deleted_ColumnName OUTPUT    
 DECLARE c_policy_binders CURSOR FAST_FORWARD FOR    
    SELECT  Policy_Binders_Fld    
    FROM    #Policy_Binders    
    OPEN c_policy_binders    
    FETCH NEXT FROM c_policy_binders    
          INTO @Current_Policy_Binder    
    WHILE (@@FETCH_STATUS = 0)    
 BEGIN    
  SELECT @SQL = "UPDATE #Matches_Found SET "    
 IF NOT ISNULL(@PrimaryPolicy_ColumnName,'')=''       
 BEGIN    
   SELECT @SQL = @SQL + " Primary_Policy = (Select " + @PrimaryPolicy_ColumnName  + " FROM " + @PrimaryPolicy_TableName    
   SELECT @SQL = @SQL + " WHERE " + RTRIM(@gis_data_model_code) + "_policy_binder_id = "    
      SELECT @SQL = @SQL + CONVERT(VARCHAR(50),@Current_Policy_Binder) + ")"    
 END    
  IF NOT ISNULL(@PrimaryPolicy_ColumnName,'')=''    
     SELECT @SQL = @SQL + ","    
  SELECT @SQL = @SQL + " Is_Deleted = CASE WHEN EXISTS(SELECT 1 from insurance_file_risk_link ifrl "    
  SELECT @SQL = @SQL + " JOIN gis_policy_link gpl ON gpl.risk_id=ifrl.risk_cnt "    
  SELECT @SQL = @SQL + " JOIN " + RTRIM(@gis_data_model_code) + "_policy_binder dmpb "    
  SELECT @SQL = @SQL + " ON gpl.gis_policy_link_id=dmpb.gis_policy_link_id "    
  SELECT @SQL = @SQL + " WHERE ifrl.status_flag='D' AND dmpb.gis_policy_link_id= "    
  SELECT @SQL = @SQL + CONVERT(VARCHAR(50),@Current_Policy_Binder) + ")"    
  SELECT @SQL = @SQL + " THEN 1 ELSE 0 END "    
  SELECT @SQL = @SQL + " WHERE Policy_Binder_id = " + CONVERT(VARCHAR(50),@Current_Policy_Binder)    
  EXEC (@SQL)    
      FETCH NEXT FROM c_policy_binders    
           INTO @Current_Policy_Binder    
 END    
    CLOSE c_policy_binders    
    DEALLOCATE c_policy_binders    
 FETCH NEXT FROM c_search_properties    
       INTO    @object_name,    
               @table_name ,    
               @property_name,    
               @column_name,    
               @gis_data_model_code    
END    
CLOSE c_search_properties    
DEALLOCATE c_search_properties    
SELECT ifi.insurance_file_id,    
 ifi.source_id ins_file_source_id,    
 ifi.insurance_file_cnt,    
 ifi.insurance_ref,    
 ifo.description insurance_folder_code,    
 ift.code type_code,    
 p.name insured_name,    
 p.shortname insured_shortname,    
 p.party_id,    
 p.source_id party_source_id,    
 ifs.last_modified,    
 ifo.insurance_holder_cnt,    
 ifo.insurance_folder_cnt,    
 ifi.product_id,    
 pr.code,    
 pr.description caption,    
 ifi.lead_agent_cnt,    
 ifs.date_created,    
 m.Object_Name,    
 m.Property_Name,    
 m.Value,    
 ifi.renewal_date,    
 ifi.this_premium,    
 ifi.policy_type_id,    
 ift.description AS type_desc,    
 (SELECT TOP 1 shortname    
 FROM Party P    
 WHERE ifi.lead_agent_cnt  = P.party_cnt) Lead_Agent,    
 PT.description AS policy_type,    
 ift.insurance_file_type_id    
    FROM    Insurance_File ifi    
 INNER JOIN Insurance_File_System ifs    
 ON ifs.insurance_file_cnt = ifi.insurance_file_cnt    
 INNER JOIN Insurance_Folder ifo    
 ON ifo.insurance_folder_cnt = ifi.insurance_folder_cnt    
 INNER JOIN Insurance_File_Type ift    
 ON ift.insurance_file_type_id = ifi.insurance_file_type_id    
 INNER JOIN Party p    
 ON p.party_cnt = ifo.insurance_holder_cnt    
 INNER JOIN Product pr    
 ON pr.product_id = ifi.product_id    
 INNER JOIN insurance_file_risk_link ifrl    
 ON ifrl.insurance_file_cnt = ifi.insurance_file_cnt    
 INNER JOIN gis_policy_link gpl    
 ON ifrl.risk_cnt = gpl.risk_id    
 INNER JOIN #Matches_Found m    
 ON gpl.gis_policy_link_id = m.policy_binder_id    
 INNER JOIN Policy_Type PT    
 ON PT.policy_type_id = ifi.policy_type_id    
 WHERE  ifi.policy_ignore IS NULL    
  AND ISNULL(m.Is_Deleted,0) = 0    
  AND ifi.policy_version=(SELECT MAX(policy_version) FROM insurance_file ifi1    
     WHERE  ifi.insurance_ref=ifi1.insurance_ref    
     AND insurance_file_type_id  IN (2,5,6,9)    
     AND (ISNULL(insurance_file_status_id, 3) = 3 OR insurance_file_status_id = 4))    
  AND (DATEDIFF(DAY,expiry_date,@SystemDT)<=0)    
DROP TABLE #Matches_Found    
DROP TABLE #Policy_Binders

SET QUOTED_IDENTIFIER OFF  
GO
SET ANSI_NULLS ON
GO
