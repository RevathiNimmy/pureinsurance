SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_GetBroker_Summary_By_RiskIndex'
GO
CREATE  PROCEDURE spu_SAM_GetBroker_Summary_By_RiskIndex  
    @search_object_name VARCHAR(70) = NULL,  
    @search_value VARCHAR(255),  
    @Specials_Type_Filter INT =0,  
    @party_cnt INT =0,  
    @MaxRowsToFetch INT = -1,  
    @AgentKey INT = 0,  
    @SourceID INT = 0  
AS  
BEGIN  
  
    SET NOCOUNT ON  
    DECLARE @SQL               VARCHAR(500) ,  
        @SQL2           VARCHAR(500) ,  
        @SQL3           VARCHAR(500) ,  
        @print              VARCHAR(255) ,  
        @gis_data_model_id  INTEGER ,  
        @object_name        VARCHAR(70) ,  
        @property_name      VARCHAR(70) ,  
        @table_name     VARCHAR(70) ,  
        @column_name        VARCHAR(70) ,  
        @policy_binder_id       INTEGER,  
        @specials_type VARCHAR(10)  
  
 CREATE TABLE #Matches_Found (policy_binder_id int, object_name varchar(70), property_name varchar(70), value varchar(255),Specials_Type Varchar(10))  
  
        DECLARE @gis_data_model_code VARCHAR(10)  
  
        DECLARE c_Data_Models CURSOR FAST_FORWARD FOR  
        Select code  
        FROM    gis_data_model  
        Where   Is_Deleted = 0  AND gis_data_model_type_id<>2  
  
        OPEN c_Data_Models  
  
        FETCH NEXT  FROM c_Data_Models  
        INTO    @gis_data_model_code  
  
        WHILE (@@FETCH_STATUS = 0)  
        BEGIN  
  
        /* First Select the Data Model ID from the Data Model Code */  
  
        SELECT @gis_data_model_id = gis_data_model_id  
        FROM    gis_data_model  
        WHERE   code = @gis_data_model_code  
  
        /* Then Build a Cursor for the Search Properties for this Data Model */  
  
        /* If the Object to Search is known then Limit to that Object */  
  
        IF (@search_object_name IS NULL) OR (@search_object_name = '')  
  
            DECLARE c_search_properties CURSOR FAST_FORWARD FOR  
            SELECT  object_name,  
     table_name,  
     property_name,  
                    column_name,  
     specials_type  
            FROM    gis_object o  
                    INNER JOIN gis_property p ON (o.gis_object_id = p.gis_object_id)  
            WHERE   o.gis_data_model_id = @gis_data_model_id     AND  
         (  
      (@Specials_Type_Filter = 0 AND is_search_property = 1)  
             OR  
          (@Specials_Type_Filter <> 0)  
       )  
       ELSE  
            DECLARE c_search_properties CURSOR FAST_FORWARD FOR  
            SELECT  object_name,  
           table_name,  
     property_name,  
                    column_name,  
     specials_type  
            FROM    gis_object o  
                    INNER JOIN gis_property p ON (o.gis_object_id = p.gis_object_id)  
            WHERE   is_search_property = 1  
              AND   o.gis_data_model_id = @gis_data_model_id  
              AND   o.object_name = @search_object_name  
  
        /* Then Loop Round the Cursor and Do the Searches */  
  
        OPEN c_search_properties  
        FETCH NEXT FROM c_search_properties  
        INTO        @object_name,  
                @table_name ,  
                @property_name,  
                @column_name,  
    @specials_type  
  
        WHILE (@@FETCH_STATUS = 0)  
        BEGIN  
  
            SELECT @SQL = "INSERT INTO #Matches_Found (Policy_Binder_id, object_name, property_name, value, Specials_Type) SELECT " + RTRIM(LTRIM(@gis_data_model_code)) + "_policy_binder_id, '" + RTRIM(LTRIM(@object_name)) + "' , '" +  
        RTRIM(LTRIM(@property_name)) + "' ," + RTRIM(LTRIM(@column_name))  + ", '" + RTRIM(LTRIM(@Specials_Type)) + "'"  
            SELECT @SQL2 = " FROM " + RTRIM(LTRIM(@table_name)) + " WHERE " + RTRIM(LTRIM(@column_name))  
  
            IF RIGHT(@search_value,1) = "%"  
                SELECT @SQL2 = @SQL2 + " LIKE "  
            ELSE  
                IF @specials_type <> 0  
     SELECT @SQL2 = @SQL2 + " LIKE "  
    ELSE  
        SELECT @SQL2 = @SQL2 + " = "  
  
            SELECT @SQL3 = "'" + @search_value + "'"  
  
   EXEC (@SQL + @SQL2 + @SQL3)  
  
            FETCH NEXT FROM c_search_properties  
   INTO        @object_name,  
      @table_name ,        @property_name,  
      @column_name,  
      @specials_type  
  
        END  
  
         CLOSE c_search_properties  
         DEALLOCATE c_search_properties  
  
         FETCH NEXT  FROM c_Data_Models  
   INTO    @gis_data_model_code  
  
      END  
  
    IF @MaxRowsToFetch <>-1  
  
 BEGIN  
  
 SET NOCOUNT ON  
  
    SET ROWCOUNT @MaxRowsToFetch  
  
 END  
  
      SELECT  
   DISTINCT  
         ifi.insurance_file_cnt AS InsuranceFileKey,  
         ifi.insurance_folder_cnt   AS InsuranceFolderKey,  
         ifi.insured_cnt   AS PartyKey,  
         ifi.insurance_ref   AS InsuranceRef,  
         pr.code   AS ProductCode,  
         pr.description   AS ProductDescription,  
         ift.description   AS InsuranceFileTypeDescription,  
         ift.code   AS InsuranceFileTypeCode,  
         p.shortname   AS ClientShortName,  
         p.name   AS ClientName,  
   ISNULL(IFS.Last_Modified,IFS.Date_Created) IssuedDate,  
         ifi.cover_start_date   AS StartDate,  
         ifi.expiry_date   AS ExpiryDate,  
         ISNULL(ifst.code,'LIVE')   AS PolicyStatusCode,  
         ISNULL(ifst.description,'LIVE')   AS PolicyStatusDescription,  
         ifi.Insurance_File_Type_id   AS PolicyTypeId,  
   (CASE  
   WHEN (ifi.insurance_file_cnt = (SELECT MAX(insurance_file_cnt) FROM insurance_file WHERE insurance_folder_cnt = ifi.insurance_folder_cnt AND insurance_file_type_id IN ( 1, 2, 3, 5, 8, 9, 11 ) AND ISNULL(insurance_file_status_id,0) NOT IN ( 1, 2 ))) THEN 1  
   ELSE 0  
   END)  
   AS IsCurrent,  
         ifi.quote_status_id   AS QuoteStatusKey,  
         ifi.quote_version   AS QuoteVersion,  
         ifi.base_insurance_folder_cnt   AS BaseInsuranceFolderKey,  
         pag.trading_name AS AgentName,  
         ifi.lead_agent_cnt  AS AgentKey,  
         ifi.quote_expiry_date   AS QuoteExpiryDate,  
         ifi.Contact_user_id   AS ContactUserId,  
   (CASE WHEN (ifi.insurance_file_status_id IS NULL AND ifi.policy_version > 1) THEN 1 ELSE 0 END) AS RenewedVersion,  
   (CASE WHEN (risk.risk_status_id is null OR risk.risk_status_id =4) THEN 'Unquoted'  
     When risk.risk_status_id = 3   THEN 'Quoted'  
     WHEN risk.risk_status_id = 2  THEN 'Declined'  
     WHEN risk.risk_status_id = 1  THEN 'Referred'  
     ELSE 'Quoted'  
     END ) AS  RiskStatus,  
         ifi.is_marketplace_policy   IsMarketPlacePolicy,  
   0 IsReinstateLink,  
   ISNULL(ifi.base_insurance_file_cnt,0) BaseInsuranceFileKey,  
   risk.risk_number RiskNumber,  
   risk.description RiskDescription,
   NULL As AssociatedClients
  
    FROM    Insurance_File ifi INNER JOIN Insurance_File_System ifs ON ifs.insurance_file_cnt = ifi.insurance_file_cnt  
  
    INNER JOIN Insurance_Folder ifo ON  ifo.insurance_folder_cnt = ifi.insurance_folder_cnt  
  
    Inner JOIN Insurance_File_Type ift ON ift.insurance_file_type_id = ifi.insurance_file_type_id  
  
    INNER JOIN Party p ON p.party_cnt = ifo.insurance_holder_cnt  
  
    INNER JOIN Product pr ON pr.product_id = ifi.product_id  
  
    INNER JOIN insurance_file_risk_link ifrl ON ifrl.insurance_file_cnt = ifi.insurance_file_cnt  
  
    INNER JOIN gis_policy_link gpl ON ifrl.risk_cnt = gpl.risk_id  
  
    INNER JOIN #Matches_Found m ON gpl.gis_policy_link_id = m.policy_binder_id  
  
    INNER JOIN Policy_Type PT ON PT.policy_type_id = ifi.policy_type_id  
  
    LEFT OUTER JOIN Insurance_File_Status ifst ON ifst.insurance_file_status_id=ifi.insurance_file_status_id  
  
    LEFT OUTER JOIN party_agent pag ON ifi.lead_agent_cnt=pag.party_cnt  
  
    INNER JOIN Risk risk ON risk.risk_cnt = ifrl.risk_cnt  
  
    INNER JOIN Risk_type rsktype ON rsktype.risk_type_id = risk.risk_type_id  
  
    WHERE  
  
     ifi.insurance_file_cnt in (SELECT Insurance_File_cnt FROM insurance_file WHERE insurance_folder_cnt=ifi.insurance_folder_cnt)  
  
     AND (ifi.insurance_file_status_id IS NULL  
  
     OR ifi.insurance_file_status_id = 3   --REN hardcoded as 3  
  
     OR ifi.insurance_file_type_id = 8)    --MTACan hardcoded as 8  
  
     AND ifi.policy_ignore IS NULL  
  
     AND  
  
     (  
  
     (@Specials_Type_Filter = 0)  
  
     OR  
  
     (@Specials_Type_Filter <> 0 AND m.specials_type = @Specials_Type_Filter)  
  
     )  
  
     AND (ifi.lead_agent_cnt = @AgentKey OR ISNull(@AgentKey, 0)= 0)  
     AND (ifi.source_id = @SourceID OR IsNull(@SourceID, 0) = 0)  
  
     ORDER BY ifi.insurance_folder_cnt  
  
      IF @MaxRowsToFetch <>-1  
  
 BEGIN  
  
        SET ROWCOUNT 0  
  SET NOCOUNT OFF  
  
       END  
  
 DROP TABLE #Matches_Found  
  
  
    SET NOCOUNT OFF  
  
  
END  