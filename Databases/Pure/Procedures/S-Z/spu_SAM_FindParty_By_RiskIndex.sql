SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure spu_SAM_FindParty_By_RiskIndex 
GO

CREATE  PROCEDURE spu_SAM_FindParty_By_RiskIndex  
    @search_object_name VARCHAR(70) = NULL,  
    @search_value VARCHAR(255),  
    @is_insurance_ref_reqd tinyint = NULL,  
    @Specials_Type_Filter INT =0,
    --Start (Prakash C Varghese) - (Agent Group Association)
    @agent_group_cnt INT =0,  
    --End (Prakash C Varghese) - (Agent Group Association)
    @MaxRowsToFetch INT = -1 ,
	@sPartyType varchar(20) = '',
	@UserID INT = -1  
AS  
BEGIN  
    /********************************************************************************************************/  
    --Modified Stored procedure spu_gis_search_property_find_two and created this for SAM PN Issue:42259  
    /********************************************************************************************************/  
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
        SELECT top 1 @gis_data_model_id = gis_data_model_id  
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
            WHERE   o.gis_data_model_id = @gis_data_model_id  
     AND  
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
      
            --IF RIGHT(@search_value,1) = "%"  
			 IF CHARINDEX('%', @search_value)>0
                SELECT @SQL2 = @SQL2 + " LIKE "  
            ELSE  
                IF @specials_type <> 0  
         SELECT @SQL2 = @SQL2 + " LIKE "  
         ELSE  
      SELECT @SQL2 = @SQL2 + " = "  
      
    
            SELECT @SQL3 = "'" + @search_value + "'"  
      
    --      SELECT @PRINT = (@SQL + @SQL2 + @SQL3)  
    --      PRINT @PRINT  
      
            EXEC (@SQL + @SQL2 + @SQL3)  
      
            FETCH NEXT FROM c_search_properties  
        INTO        @object_name,  
                    @table_name ,  
                    @property_name,  
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
          p.party_cnt PartyKey,  
          RTrim(Ltrim(p.shortname)) ShortName,  
          p.resolved_name ResolvedName,  
          (select top 1  address1  
          from party_Address_usage PAU, Address AD  
          where  
              PAU.Party_cnt   = P.Party_Cnt  
             and PAU.Address_cnt = AD.Address_cnt  
             and PAU.address_usage_type_id = 4  
         ) AddressLine1,  
         (select top 1  postal_code  
         from party_Address_usage PAU, Address AD  
         where  
             PAU.Party_cnt   = P.Party_Cnt  
             and PAU.Address_cnt = AD.Address_cnt  
             and PAU.address_usage_type_id = 4  
         ) PostCode,  
         RTrim(Ltrim((select top 1 (Ltrim(Rtrim(isnull(area_code,''))) + ' ' +  LTrim(RTrim(isnull(Number,''))) + ' ' +  LTrim(Rtrim(isnull(Extension,''))))  
         from party_contact_usage PCU, contact CON  
         where  
          PCU.Party_cnt   = P.Party_Cnt  
         and PCU.Contact_cnt = CON.Contact_cnt  
         and contact_type_id = 1  
         ))) ContactTelephoneNumber,  
         PL.date_of_birth DateOfBirth,  
         p.agent_cnt  AgentKey ,
		 Party_Type.Code as Type,
		 SL.Code as ServiceLevelCode,
		 SL.Description as ServiceLevelDescription,
         p.file_code FileCode
    FROM  
        #Matches_Found m
        INNER JOIN gis_policy_link gpl
            ON gpl.gis_policy_link_id = m.policy_binder_id
            AND  
            (  
              (@Specials_Type_Filter = 0)  
              OR  
              (@Specials_Type_Filter <> 0 AND m.specials_type = @Specials_Type_Filter)  
             )   
        INNER JOIN insurance_file_risk_link ifrl
            ON ifrl.risk_cnt = gpl.risk_id  
        INNER JOIN Insurance_File ifi
            ON ifrl.insurance_file_cnt = ifi.insurance_file_cnt  
        INNER JOIN Insurance_File_System ifs
            ON ifs.insurance_file_cnt = ifi.insurance_file_cnt  
            AND (ifi.insurance_file_status_id IS NULL  
                OR ifi.insurance_file_status_id = 3   --REN hardcoded as 3  
				OR ifi.insurance_file_type_id = 2	  --Lapse hardcoded as 2
                OR ifi.insurance_file_type_id = 8)    --MTACan hardcoded as 8 
            AND ifi.policy_ignore IS NULL  
        INNER JOIN insurance_File_Type ift
            ON ift.insurance_file_type_id = ifi.insurance_file_type_id  
        INNER JOIN Product pr
            ON pr.product_id = ifi.product_id  
        INNER JOIN Policy_Type PT
            ON PT.policy_type_id = ifi.policy_type_id
        INNER JOIN Insurance_Folder ifo
            ON ifo.insurance_folder_cnt = ifi.insurance_folder_cnt     
        INNER JOIN Party p
            ON p.party_cnt = ifo.insurance_holder_cnt   
		INNER JOIN Party_Type ON P.party_type_id = Party_Type.party_type_id
        LEFT OUTER JOIN party_Lifestyle PL 
            ON P.party_cnt = PL.party_cnt       
		LEFT OUTER JOIN Service_Level SL
			on P.Service_Level_id = SL.Service_Level_id
    --Start (Prakash C Varghese) - (Agent Group Association)
    WHERE
        (@agent_group_cnt=0
         OR (
             p.Agent_Cnt IN (
                             SELECT 
                                 party_cnt 
                             FROM 
                                 party_agent  
                             WHERE 
                                 linked_account_group =@agent_group_cnt
                            ) 
             OR ifi.lead_agent_cnt IN (
                                       SELECT 
                                           party_cnt 
                                       FROM 
                                           party_agent  
                                       WHERE 
                                           linked_account_group =@agent_group_cnt
                                      )                                    
            )
        ) and (@sPartyType='' or party_type.code=@sPartyType)              
    --End (Prakash C Varghese) - (Agent Group Association)
       AND p.source_id IN (SELECT source_id  
							FROM Source WHERE ISNULL(is_deleted,0) <> 1  
							AND source_id NOT IN (SELECT source_id FROM pmuser_source WHERE user_id = @UserID )) 
	  
    IF @MaxRowsToFetch <>-1
	BEGIN
        SET ROWCOUNT 0      
	SET NOCOUNT OFF    
      	END	
	DROP TABLE #Matches_Found  
      
    SET NOCOUNT OFF  
      
END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

