SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_gis_search_property_risk'
GO

CREATE PROCEDURE spu_gis_search_property_risk        
    @gis_data_model_code VARCHAR(10),        
    @search_object_name VARCHAR(70) = NULL,        
    @search_value VARCHAR(255),
    --Start (Prakash C Varghese) - (Agent Group Association)
    @agent_group_cnt INT =0  
    --End (Prakash C Varghese) - (Agent Group Association)  
AS        
BEGIN        
/********************************************************************************************************/        
/* Stored Procedure spu_gis_search_property_find, Finds the GIS_Policy_link by doing selects on the      */        
/*                  Properties that are marked as search properties within the Data Model.              */        
/********************************************************************************************************/        
/* Revision             Description of Modification                                     Date        Who */        
/* --------             ---------------------------                                     ----        --- */        
/* 1.0                  Original                            05/10/2000  RFC */        
/* 1.1                  insurance_file_cnt on policy binder holds the folder cnt    12/04/2001  Tom */        
/* 1.2                  Need to cater for claims data models..       03/02/2004  RVH */        
/* 1.3   Added check for WORK_CLAIM_PERIL to        18/02/2004  RVH */        
/*   match the check for WORK_CLAIM          */        
/********************************************************************************************************/        
SET NOCOUNT ON        
        
DECLARE @SQL                 VARCHAR(512) ,        
        @SQL2                VARCHAR(512) ,        
        @SQL3                VARCHAR(512) ,        
        @print               VARCHAR(512) ,        
        @gis_data_model_id   INTEGER ,        
        @object_name         VARCHAR(70) ,        
        @property_name       VARCHAR(70) ,        
        @table_name          VARCHAR(70) ,        
        @column_name         VARCHAR(70) ,        
        @policy_binder_id    INTEGER,        
  @gis_data_model_type CHAR(10),        
        @orig_table_name        VARCHAR(70)        
        
    CREATE TABLE #Matches_Found (policy_binder_id int, object_name varchar(70), property_name varchar(70), value varchar(255))        
        
    /* First Select the Data Model ID from the Data Model Code */        
    SELECT  @gis_data_model_id = gis_data_model_id,        
     @gis_data_model_type = upper(gdmt.code)        
    FROM    gis_data_model  gdm,        
     gis_data_model_type gdmt        
    WHERE   gdm.code = @gis_data_model_code        
    AND     gdm.gis_data_model_type_id = gdmt.gis_data_model_type_id        
        
    /* Then Build a Cursor for the Search Properties for this Data Model */        
        
    /* If the Object to Search is known then Limit to that Object */        
    IF (@search_object_name IS NULL) OR (@search_object_name = '')        
        DECLARE c_search_properties CURSOR FAST_FORWARD FOR        
        SELECT  object_name,        
            table_name,        
            property_name,        
                column_name        
        FROM    gis_object o        
                INNER JOIN gis_property p ON (o.gis_object_id = p.gis_object_id)        
        WHERE   is_search_property = 1        
          AND   o.gis_data_model_id = @gis_data_model_id        
    ELSE        
        DECLARE c_search_properties CURSOR FAST_FORWARD FOR        
        SELECT  object_name,        
            table_name,        
                        property_name,        
                column_name        
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
            @column_name        
    WHILE (@@FETCH_STATUS = 0)        
    BEGIN        
      
      
 select @orig_table_name = @table_name        
        
/*        if (@gis_data_model_type = 'CLAIM')        
        begin        
      
  if (upper(@object_name, 10) = 'WORK_CLAIM')       
  begin        
   select @table_name = @table      
  end        
        
  if (upper(@object_name) = 'WORK_CLAIM')        
  begin        
        select @object_name = 'CLAIM'        
  end        
      
  if (upper(right(@table_name, 16)) = 'WORK_CLAIM_PERIL')        
  begin        
   select @table_name = substring(@table_name, 1, len(@table_name) - 16) + 'CLAIM_PERIL'        
  end        
        
  if (upper(@object_name) = 'WORK_CLAIM_PERIL')        
  begin        
   select @object_name = 'CLAIM_PERIL'        
  end        
 end        
*/       
      
 if ((upper(right(@object_name, 10)) = 'WORK_CLAIM') or (upper(right(@object_name, 16)) = 'WORK_CLAIM_PERIL'))        
 begin        
      
  SET @SQL = 'INSERT INTO #Matches_Found (Policy_Binder_id, object_name, property_name, value)'    
  SET @SQL = @SQL + ' SELECT (select ' + @gis_data_model_code + '_policy_binder_id from gis_policy_link gpl, ' + @gis_data_model_code + '_policy_binder cpb '     
  SET @SQL = @SQL + ' WHERE gpl.claim_id = ' + @table_name + '.claim_id and gpl.gis_policy_link_id = cpb.gis_policy_link_id), ''' + @object_name + ''' , ''' + @property_name + ''' ,''' + @column_name  + ''''  
  SET @SQL2 = ' FROM ' + @table_name + ' WHERE ' + @column_name        
        
  --IF RIGHT(@search_value,1) = '%'        
   IF CHARINDEX('%', @search_value)>0
  SET @SQL2 = @SQL2 + ' LIKE '        
  ELSE        
  SET @SQL2 = @SQL2 + ' = '        
  SET @SQL3 = '''' + replace(@search_value, '''', '''''') + ''''        
 end        
 else        
 begin        
  SET @SQL = 'INSERT INTO #Matches_Found (Policy_Binder_id, object_name, property_name, value) SELECT ' + @gis_data_model_code + '_policy_binder_id, '' + @object_name + '' , '' + @property_name + '' ,' + @column_name        
  SET @SQL2 = ' FROM ' + @table_name + ' WHERE ' + @column_name        
        
  --IF RIGHT(@search_value,1) = '%'        
   IF CHARINDEX('%', @search_value)>0
  SET @SQL2 = @SQL2 + ' LIKE '        
  ELSE        
  SET @SQL2 = @SQL2 + ' = '        
  SET @SQL3 = '''' + replace(@search_value, '''', '''''') + ''''        
 end        
        
        SET @PRINT = (@SQL + @SQL2 + @SQL3)        
        
 PRINT @PRINT        
        
        EXEC (@SQL + @SQL2 + @SQL3)        
        
        FETCH NEXT FROM c_search_properties        
        INTO        @object_name,        
                @table_name ,        
                @property_name,        
                @column_name        
    END        
    CLOSE c_search_properties        
    DEALLOCATE c_search_properties        
        
    if (@gis_data_model_type = 'CLAIM')        
    begin       
    
 --select * from #matches_Found     
    
 SELECT  DISTINCT        
         clm.policy_id,        
         gpl.gis_policy_link_id,        
         mf.object_name,        
         ' ',  
         ' ',  
  clm.claim_id        
 FROM    gis_policy_link gpl,        
         #Matches_Found mf,        
         claim clm,
         --Start (Prakash C Varghese) - (Agent Group Association)
         insurance_file ifi,
         party p  
         --End (Prakash C Varghese) - (Agent Group Association)  
 WHERE   gpl.gis_policy_link_id = mf.policy_binder_id        
 AND     gpl.claim_id = clm.claim_id        
 --Start (Prakash C Varghese) - (Agent Group Association)
   AND ifi.insurance_file_cnt=clm.policy_ID
   AND p.party_cnt=ifi.insured_cnt
   AND (@agent_group_cnt=0
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
       )              
    --End (Prakash C Varghese) - (Agent Group Association)    
  

    end        
    else        
    begin        

        SELECT  DISTINCT        
                clm.policy_id,        
                gpl.gis_policy_link_id,        
                mf.object_name,        
                ' ',  
                ' ',
		clm.claim_id    
        FROM    gis_policy_link gpl,        
                #Matches_Found mf,        
                claim clm,
                --Start (Prakash C Varghese) - (Agent Group Association)
                insurance_file ifi,
                party p  
                --End (Prakash C Varghese) - (Agent Group Association)
        WHERE   gpl.gis_policy_link_id = mf.policy_binder_id        
        AND     gpl.risk_id = clm.risk_type_id        
        --Start (Prakash C Varghese) - (Agent Group Association)
       AND ifi.insurance_file_cnt=clm.policy_ID
       AND p.party_cnt=ifi.insured_cnt
       AND (@agent_group_cnt=0
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
           )              
        --End (Prakash C Varghese) - (Agent Group Association)    
    end        
        
    --Start (Prakash C Varghese)
    DROP TABLE #Matches_Found
     --End (Prakash C Varghese)
SET NOCOUNT OFF        
END        
      
    
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
