SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure spu_SAM_FindParty_By_PartyIndex 
GO

CREATE  PROCEDURE spu_SAM_FindParty_By_PartyIndex
    @sSearch_value VARCHAR(255),  
    @nMaxRowsToFetch INT = -1 ,
	@sPartyType varchar(20) = '',
	@sLoggedInUserName varchar(255)
AS  
BEGIN  
    /********************************************************************************************************/  
    --Modified Stored procedure spu_gis_search_property_find_two and created this for SAM 
    /********************************************************************************************************/  
    SET NOCOUNT ON  
      
    DECLARE 
		@sSQL			VARCHAR(500) ,  
        @sSQL2				VARCHAR(500) ,  
        @sSQL3			    VARCHAR(500) ,  
        @sPrint		        VARCHAR(255) ,  
        @nGis_data_model_id  INTEGER ,  
        @sObject_name        VARCHAR(70) ,  
        @sProperty_name      VARCHAR(70) ,  
        @sTable_name			VARCHAR(70) ,  
        @sColumn_name        VARCHAR(70) ,  
        @nPolicy_binder_id   INTEGER,  
        @sSpecials_type		VARCHAR(10),
		@optionvalue as char(2);

	-- System Option - Accounts access limited to user's branch access
	SELECT DISTINCT @optionvalue = ISNULL(value,0)
	FROM System_Options
	WHERE option_number =5152;
      
	CREATE TABLE #Matches_Found (policy_binder_id INT, object_name VARCHAR(70), property_name VARCHAR(70), value VARCHAR(255),Specials_Type VARCHAR(10))  
      
	    DECLARE @gis_data_model_code VARCHAR(10)  
      
	    DECLARE c_Data_Models CURSOR FAST_FORWARD FOR  
        SELECT code  
        FROM    gis_data_model  
        WHERE   Is_Deleted = 0  AND gis_data_model_type_id=4
      
        OPEN c_Data_Models  
        FETCH NEXT  FROM c_Data_Models  
        INTO    @gis_data_model_code  
      
        WHILE (@@FETCH_STATUS = 0)  
        BEGIN  
        /* First Select the Data Model ID from the Data Model Code */  
        SELECT @nGis_data_model_id = gis_data_model_id  
        FROM    gis_data_model  
        WHERE   code = @gis_data_model_code  
      
	    DECLARE c_search_properties CURSOR FAST_FORWARD FOR  
           SELECT  object_name,  
                table_name,  
                property_name,  
                column_name,  
				specials_type        
					FROM    gis_object o  
                    INNER JOIN gis_property p ON (o.gis_object_id = p.gis_object_id)  
					WHERE   o.gis_data_model_id = @nGis_data_model_id  
					 AND   is_search_property = 1      
           
        /* Then Loop Round the Cursor and Do the Searches */  
        OPEN c_search_properties  
      
        FETCH NEXT FROM c_search_properties  
        INTO    @sObject_name,  
                @sTable_name ,  
                @sProperty_name,  
                @sColumn_name,  
				@sSpecials_type  
      
        WHILE (@@FETCH_STATUS = 0)  
        BEGIN  
      
     SELECT @sSQL = "INSERT INTO #Matches_Found (Policy_Binder_id, object_name, property_name, value, Specials_Type) SELECT " + RTRIM(LTRIM(@gis_data_model_code)) + "_policy_binder_id, '" + RTRIM(LTRIM(@sObject_name)) + "' , '" +
		RTRIM(LTRIM(@sProperty_name)) + "' ," + RTRIM(LTRIM(@sColumn_name))  + ", '" + RTRIM(LTRIM(@sSpecials_Type)) + "'"  
        SELECT @sSQL2 = " FROM " + RTRIM(LTRIM(@sTable_name)) + " WHERE " + RTRIM(LTRIM(@sColumn_name))  
      
	        IF CHARINDEX('%', @sSearch_value ) > 0 
                SELECT @sSQL2 = @sSQL2 + " LIKE "  
            ELSE  
                IF @sSpecials_type <> 0  
					SELECT @sSQL2 = @sSQL2 + " LIKE "  
				ELSE  
					SELECT @sSQL2 = @sSQL2 + " = "  
    
            SELECT @sSQL3 = "'" +REPLACE(REPLACE(@sSearch_value,'"',''),"'",'') + "'"
      
          --SELECT @PRINT = (@SQL + @SQL2 + @SQL3)  
          --PRINT @PRINT  
      
            --EXEC (@sSQL + @sSQL2 + @sSQL3)  
      
    	Declare @query as nvarchar(max)
		Set @query = @sSQL + @sSQL2 + '@sSearch_value'
		EXEC sp_executesql @query, N'@sSearch_value VARCHAR(255)', @sSearch_value 
      
    FETCH NEXT FROM c_search_properties  
        INTO        @sObject_name,  
                    @sTable_name ,  
                    @sProperty_name,  
                    @sColumn_name,  
					@sSpecials_type  
        END  
      
    CLOSE c_search_properties  
    DEALLOCATE c_search_properties  
      
    FETCH NEXT  FROM c_Data_Models  
    INTO    @gis_data_model_code  
    
    END  
    
	IF @nMaxRowsToFetch <>-1
		BEGIN      
		SET NOCOUNT ON    
      	SET ROWCOUNT @nMaxRowsToFetch
	END     
   
SELECT DISTINCT Party.party_cnt AS PartyKey,
PMCaption.caption,
Party.shortname AS ShortName,
Party.resolved_name AS ResolvedName,
Address.address1 AS AddressLine1,
(CASE WHEN Address.postal_code = CONVERT(VARCHAR(20), Address.address_id) THEN '' ELSE Address.postal_code END) AS PostCode,--postal_code ,
(Select code from source where source_id = Party.source_id) source_id,
Party.party_id,
'' AS area_code,
'' AS number,
Party.is_prospect,
Party.invariant_key,
'Sirius',
' ',
CASE party_type.code WHEN 'AG' THEN 'Agent' WHEN 'CC' THEN 'Corporate Client' WHEN 'GC' THEN 'Group Client' WHEN 'PC' THEN 'Personal Client' ELSE ' ' END,
Party.file_code,
(SELECT TOP 1 date_of_birth FROM Party_Lifestyle WITH(NOLOCK) WHERE party_cnt = Party.party_cnt AND category = 1) AS DateOfBirth,
Party.swift_party_id,
Address.address2 AddressLine2,
 (CASE source.is_deleted WHEN 1 THEN Rtrim(source.description) + ' (Closed)' WHEN 0 THEN source.description END) AS description ,
Party.Agent_Cnt AgentKey,
record_status,
rtrim(party_type.code) AS Type,
Party_Agent.date_cancelled,
party_net_data.online_status,
(CASE WHEN Party.Party_type_id='1' THEN Party.name ELSE '' END)AS name,
(CASE WHEN Party.Party_type_id='1' THEN Party_Personal_Client.forename ELSE '' END)AS forename,
Address.address3,
Address.address4,
SL.Code as ServiceLevelCode,
SL.Description as ServiceLevelDescription
FROM 
#Matches_Found 
inner join gis_policy_link gpl ON gpl.gis_policy_link_id=#Matches_Found.Policy_Binder_id
inner join Party WITH(NOLOCK) ON Party.party_cnt=gpl.[party_cnt] 
INNER join Party_Type WITH(NOLOCK) ON Party.party_type_id = Party_Type.party_type_id
LEFT OUTER join source WITH(NOLOCK) ON Party.source_id = source.source_id
INNER join PMCaption WITH(NOLOCK) ON Party_Type.caption_id = PMCaption.caption_id
LEFT OUTER join (Party_Address_Usage INNER JOIN Address WITH(NOLOCK)ON Party_Address_Usage.address_cnt = Address.address_cnt 
INNER JOIN Address_Usage_Type WITH(NOLOCK) ON Party_Address_Usage.address_usage_type_id = Address_Usage_Type.address_usage_type_id
 AND Address_Usage_Type.code = '3131 XCO') ON Party.party_cnt = Party_Address_Usage.party_cnt
LEFT OUTER join Party_Agent WITH(NOLOCK) ON Party.party_cnt = Party_Agent.party_cnt
LEFT OUTER join party_net_data WITH(NOLOCK) ON Party.party_cnt = party_net_data.party_cnt
LEFT OUTER join Party_Personal_Client ON Party.party_cnt = Party_Personal_Client.party_cnt
LEFT OUTER join Insurance_File ON (insured_cnt = party.party_cnt OR collection_from_cnt = party.party_cnt)
LEFT OUTER JOIN Service_Level SL on Party.Service_Level_id = SL.Service_Level_id
WHERE Party.is_deleted = 0 and (@sPartyType='' or party_type.code=@sPartyType)
AND ((@optionvalue = 1 AND party.source_id IN (SELECT s.source_id
							FROM Source s left join PMUser_Source ps
							ON s.source_id=ps.source_id
							AND ps.user_id = (select u.user_id from pmuser u where u.username = @sLoggedInUserName)
							WHERE ps.source_id is	null))
	 OR (@optionvalue = 0))
ORDER BY Party.shortname        
          
      IF @nMaxRowsToFetch <>-1
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

