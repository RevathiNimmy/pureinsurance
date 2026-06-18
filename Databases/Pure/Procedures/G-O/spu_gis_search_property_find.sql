SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_gis_search_property_find'
GO

CREATE PROCEDURE spu_gis_search_property_find    
    @gis_data_model_code VARCHAR(10),    
    @search_object_name VARCHAR(70) = NULL,    
    @search_value VARCHAR(255),    
    @is_insurance_ref_reqd tinyint = NULL,    
    --Start (Prakash C Varghese) - (Agent Group Association)    
    @agent_group_cnt INT =0 ,    
    --End (Prakash C Varghese) - (Agent Group Association)    
	@insurance_ref VARCHAR(70) = ''   
  
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
/* 1.2   Need to cater for claim datamodels       03/02/2004  RVH */    
/* 1.3   Added check for WORK_CLAIM_PERIL to        18/02/2004  RVH */    
/*   match the check for WORK_CLAIM          */    
/********************************************************************************************************/    
SET NOCOUNT ON    
    
DECLARE @SQL            VARCHAR(512) ,    
 @SQL2             VARCHAR(512) ,    
 @SQL3             VARCHAR(512) ,    
 @print                VARCHAR(512) ,    
 @gis_data_model_id    INTEGER ,    
 @object_name          VARCHAR(70) ,    
 @property_name        VARCHAR(70) ,    
 @table_name       VARCHAR(70) ,    
 @column_name          VARCHAR(70) ,    
 @policy_binder_id      INTEGER,    
 @gis_data_model_type  CHAR(10),    
 @orig_table_name VARCHAR(70),    
 @is_non_gis  INTEGER    
    
 CREATE TABLE #Matches_Found (policy_binder_id int, object_name varchar(70), property_name varchar(70), value varchar(255),Policy_ID Bigint, Claim_number varchar(30))    
    
 IF ISNULL(@insurance_ref,'')=''
 BEGIN
	SET @insurance_ref='%'
 END
 ELSE IF CHARINDEX('%',@insurance_ref)=0
 BEGIN
	SET @insurance_ref=@insurance_ref+'%'
 END
 
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
          column_name,    
   ISNULL(is_non_gis, 0)    
  FROM    gis_object o    
          INNER JOIN gis_property p ON (o.gis_object_id = p.gis_object_id)    
  WHERE   is_search_property = 1    
    AND   o.gis_data_model_id = @gis_data_model_id    
 ELSE    
  DECLARE c_search_properties CURSOR FAST_FORWARD FOR    
  SELECT  object_name,    
     table_name,    
   property_name,    
          column_name,    
   ISNULL(is_non_gis, 0)    
  FROM    gis_object o    
          INNER JOIN gis_property p ON (o.gis_object_id = p.gis_object_id)    
  WHERE   is_search_property = 1    
    AND   o.gis_data_model_id = @gis_data_model_id    
    AND   o.object_name = @search_object_name    
    
/* Then Loop Round the Cursor and Do the Searches */    
 OPEN c_search_properties    
    
 FETCH NEXT FROM c_search_properties    
 INTO    
 @object_name,    
 @table_name ,    
 @property_name,    
 @column_name,    
 @is_non_gis    
    
 WHILE (@@FETCH_STATUS = 0)    
 BEGIN    
    
 select @orig_table_name = @table_name    
    
 if (@gis_data_model_type = 'CLAIM')    
 begin    
 --if (upper(right(@table_name, 10)) = 'CLAIM')    
 --begin    
  --select @table_name = substring(@table_name, 1, len(@table_name) - 10) + 'CLAIM'    
 --end    
    
 if (@is_non_gis = 4)    
 begin    
  select @object_name = 'CLAIM'    
 end    
    
 --if (upper(right(@table_name, 16)) = 'CLAIM_PERIL')    
 --begin    
 -- select @table_name = substring(@table_name, 1, len(@table_name) - 16) + 'CLAIM_PERIL'    
 --end    
    
 if (@is_non_gis = 5)    
 begin    
  select @object_name = 'CLAIM_PERIL'    
 end    
    
 end    
    
SET @SQL = ''    
SET @SQL2 = ''    
    
 if ((@is_non_gis = 4) OR (@is_non_gis = 5))    
 begin    
         SELECT @SQL = 'INSERT INTO #Matches_Found (Policy_Binder_id, object_name, property_name, value) SELECT (select ' + @gis_data_model_code + '_policy_binder_id from gis_policy_link gpl, '    
    + @gis_data_model_code + '_policy_binder cpb where gpl.claim_id = '    
    + @table_name + '.claim_id and gpl.gis_policy_link_id = cpb.gis_policy_link_id), ''' + @object_name + ''' , ''' + @property_name + ''' ,' + @column_name    
    
         SELECT @SQL2 = ' FROM ' + @table_name + ' WHERE ' + @column_name    
    
        -- IF RIGHT(@search_value,1) = '%'    
		 IF CHARINDEX('%', @search_value)>0
             SELECT @SQL2 = @SQL2 + ' LIKE '    
         ELSE    
             SELECT @SQL2 = @SQL2 + ' = '    
    
         SELECT @SQL3 = '''' + @search_value + ''''    
 end    
 else    
 begin    
    
         SELECT @SQL = 'INSERT INTO #Matches_Found (Policy_Binder_id, object_name, property_name, value) SELECT TBL.' + @gis_data_model_code + '_policy_binder_id, ''' + @object_name + ''' , ''' + @property_name + ''' ,' + @column_name    
         SELECT @SQL2 = ' FROM ' + @table_name   + ' TBL '
		 SELECT @SQL2 += ' INNER JOIN ' + @gis_data_model_code + '_policy_binder PB ON PB.' + @gis_data_model_code + '_policy_binder_id = TBL.' + @gis_data_model_code + '_policy_binder_id '
		 SELECT @SQL2 += ' INNER JOIN GIS_Policy_Link GPL ON GPL.gis_policy_link_id= PB.gis_policy_link_id '
		 SELECT @SQL2 += ' LEFT JOIN Claim  ON Claim.Claim_id=GPL.claim_id '
		 SELECT @SQL2 += ' WHERE ' + @column_name    
    
        -- IF RIGHT(@search_value,1) = '%'    
		 IF CHARINDEX('%', @search_value)>0
             SELECT @SQL2 = @SQL2 + ' LIKE '    
         ELSE    
             SELECT @SQL2 = @SQL2 + ' = '    
    
         SELECT @SQL3 = '''' + @search_value + ''' And ISNULL(Claim.is_dirty,0) = CASE WHEN ISNULL(Claim.Claim_id,0) > 0  THEN 0 ELSE ISNULL(Claim.is_dirty,0) END '
    
 end    
    
 --SELECT @PRINT = (@SQL + @SQL2 + @SQL3)  
 --PRINT @PRINT  
    
EXEC (@SQL + @SQL2 + @SQL3)    
    
 FETCH NEXT FROM c_search_properties    
 INTO    @object_name,    
  @table_name ,    
  @property_name,    
  @column_name,    
  @is_non_gis    
 END    
    
 CLOSE c_search_properties    
 DEALLOCATE c_search_properties    
    
IF (@gis_data_model_type = 'CLAIM')    
BEGIN    
 UPDATE #Matches_Found SET Policy_ID =    
 (Select Claim.Policy_id FROM gis_policy_link gpl Join Claim ON gpl.Claim_id=Claim.claim_id    
 WHERE gpl.gis_policy_link_id=#Matches_Found.Policy_binder_id AND claim.is_dirty<>1)    
    
 UPDATE #Matches_Found SET Claim_Number=    
 (Select Claim.Claim_Number FROM gis_policy_link gpl    
 Join Claim ON gpl.Claim_id=Claim.claim_id    
 WHERE gpl.gis_policy_link_id=#Matches_Found.Policy_binder_id AND claim.is_dirty<>1)    
    
 DELETE FROM #Matches_Found WHERE POLICY_BINDER_ID <(    
 SELECT MAX(POLICY_BINDER_ID) FROM #Matches_Found M WHERE M.Policy_ID=#Matches_Found.Policy_ID    
 AND M.Claim_Number=#Matches_found.Claim_number )    
    
END    
    
        /*The big thing now is that l.insurance_file_cnt is actually the insurance_folder_cnt    
        so we can instead work through risk_cnt and insurance_file_risk_link    
        It'll go berserk when we have a bunch of MTAs all pointing to the same risk,    
        and we should give some thought as to how we plan to present it then*/    
    
 if (@gis_data_model_type = 'CLAIM')    
  begin    
 IF (@is_insurance_ref_reqd = 1)    
  BEGIN    
    
   SELECT  DISTINCT f.insurance_file_cnt,    
                        0,    
   t.object_name,    
                        ' ',    
                        ' ',    
   f.insurance_ref,    
      c.claim_id    
   FROM    gis_policy_link l,    
   #Matches_Found t,    
   insurance_file f,    
   claim c,    
   --Start (Prakash C Varghese) - (Agent Group Association)    
    party p    
   --End (Prakash C Varghese) - (Agent Group Association)    
   WHERE   l.gis_policy_link_id = t.policy_binder_id    
   AND l.Claim_id = c.claim_id    
   AND f.insurance_file_cnt = c.policy_Id    
      AND c.is_dirty = 0    
      --Start (Prakash C Varghese) - (Agent Group Association)    
      AND p.party_cnt=f.insured_cnt    
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
               OR f.lead_agent_cnt IN (    
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
  END    
 ELSE    
  BEGIN    
    
   SELECT  DISTINCT f.insurance_file_cnt,    
                        0,    
   t.object_name,    
                        ' ',    
                        ' ',    
   NULL insurance_ref,    
      c.claim_id    
   FROM    gis_policy_link l,    
   #Matches_Found t,    
   insurance_file f,    
      claim c,    
      --Start (Prakash C Varghese) - (Agent Group Association)    
      party p    
      --End (Prakash C Varghese) - (Agent Group Association)    
      WHERE   l.gis_policy_link_id = t.policy_binder_id    
      AND l.Claim_id = c.claim_id    
      AND f.insurance_file_cnt = c.policy_Id    
      AND c.is_dirty = 0    
      --Start (Prakash C Varghese) - (Agent Group Association)    
      AND p.party_cnt=f.insured_cnt    
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
               OR f.lead_agent_cnt IN (    
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
     END    
 END    
 else    
    BEGIN    
    IF (@gis_data_model_type = 'CASE')    
  BEGIN    
    
 SELECT  DISTINCT clm.policy_id,    
                0,    
  t.object_name,    
                ' ',    
                ' ',    
  clm.policy_number insurance_ref,    
    clm.claim_id claim_id    
  FROM    gis_policy_link l,    
  #Matches_Found t,    
    [case] c INNER JOIN claim clm on c.base_case_id = clm.base_case_id       
	INNER JOIN
(SELECT MAX(case_version) as case_version,MAX(case_id) as case_id, base_case_id FROM [case]WHERE is_dirty_case=0 GROUP BY base_case_id ) case_version
ON c.case_id = case_version.case_id
    --End (Prakash C Varghese) - (Agent Group Association)    
  WHERE   l.gis_policy_link_id = t.policy_binder_id    
  AND c.case_id = l.case_id    
  AND clm.is_dirty = 0    
  AND clm.claim_id = (Select max(claim_id) from claim where base_case_id=c.base_case_id)
 
  END    
    ELSE    
  begin    
 IF (@is_insurance_ref_reqd = 1)    
  BEGIN    
    
  SELECT  DISTINCT ifrl.insurance_file_cnt,    
                0,    
  t.object_name,    
                ' ',    
                t.value, --PN71493-Sushil Kumar    
  f.insurance_ref,    
    0 claim_id    
  FROM    gis_policy_link l,    
  #Matches_Found t,    
  insurance_file f,    
    insurance_file_risk_link ifrl,    
    --Start (Prakash C Varghese) - (Agent Group Association)    
    party p    
    --End (Prakash C Varghese) - (Agent Group Association)    
    WHERE   l.gis_policy_link_id = t.policy_binder_id    
    AND ifrl.risk_cnt = l.risk_id    
    AND ifrl.insurance_file_cnt = f.insurance_file_cnt  and f.insurance_ref like @insurance_ref 
    --Start (Prakash C Varghese) - (Agent Group Association)    
     AND p.party_cnt=f.insured_cnt    
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
              OR f.lead_agent_cnt IN (    
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
    
  END    
 ELSE    
   BEGIN    
    
  SELECT  DISTINCT ifrl.insurance_file_cnt,    
                0,    
  t.object_name,    
                ' ',    
                ' ',    
  NULL insurance_ref,    
    0 claim_id    
  FROM    gis_policy_link l,    
  #Matches_Found t,    
    insurance_file_risk_link ifrl,    
    --Start (Prakash C Varghese) - (Agent Group Association)    
    insurance_file f,    
    party p    
    --End (Prakash C Varghese) - (Agent Group Association)    
  WHERE   l.gis_policy_link_id = t.policy_binder_id    
  AND ifrl.risk_cnt = l.risk_id    
    --Start (Prakash C Varghese) - (Agent Group Association)    
     AND ifrl.insurance_file_cnt = f.insurance_file_cnt  and f.insurance_ref like @insurance_ref       
     AND p.party_cnt=f.insured_cnt    
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
              OR f.lead_agent_cnt IN (    
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
    
         END    
 end    
 END    
 DROP TABLE #Matches_Found    
 SET NOCOUNT OFF    
    
END 



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
