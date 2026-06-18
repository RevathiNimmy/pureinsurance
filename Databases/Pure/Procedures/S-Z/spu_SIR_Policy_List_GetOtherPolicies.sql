SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Policy_List_GetOtherPolicies'
GO

  
CREATE PROCEDURE spu_SIR_Policy_List_GetOtherPolicies    
    @party_cnt INT    
AS    
BEGIN    
/********************************************************************************************************/    
/* Stored Procedure spu_SIR_Policy_List_GetOtherPolicies, Finds the GIS_Policy_link by doing selects on the*/    
/*                  Properties that are marked as specials_type =3 and data_type=2.              */    
/********************************************************************************************************/    
/* Revision             Description of Modification                                     Date        Who */    
/* --------             ---------------------------                                     ----        --- */    
/* 1.0                  Original                            05/10/2000  RFC */    
/********************************************************************************************************/    
SET NOCOUNT ON    
    
DECLARE @SQL             VARCHAR(255) ,    
    @gis_data_model_id  INTEGER ,    
    @table_name     VARCHAR(70) ,    
    @column_name        VARCHAR(70) ,    
    @gis_data_model_code    VARCHAR(70) ,    
    @policy_binder_id       INTEGER    
    
CREATE TABLE #Matches_Found (Sr_No INT IDENTITY Primary Key, policy_binder_id INT)  
  
    /* First Select the Data Model ID from the Data Model Code */    
DECLARE c_search_properties CURSOR FAST_FORWARD FOR    
SELECT  o.table_name,    
        p.column_name  ,    
        rtrim(gdm.code)    
        FROM gis_object o WITH (NOLOCK)  
 INNER JOIN gis_property p  WITH (NOLOCK) ON o.gis_object_id = p.gis_object_id   
        INNER JOIN gis_data_model gdm  WITH (NOLOCK) ON gdm.gis_data_model_id = o.gis_data_model_id  
        WHERE   p.data_type=2 and p.specials_type=3 AND p.specials_type_reference in ('1','2','4') 
        AND   o.gis_data_model_id in(    
   SELECT DISTINCT gpl.gis_data_model_id from gis_policy_link gpl WITH (NOLOCK)   
    INNER JOIN Insurance_File ifi  WITH (NOLOCK) ON ifi.insurance_folder_cnt = gpl.insurance_file_cnt AND gpl.gis_policy_link_id > 0)  
    
    /* Then Loop Round the Cursor and Do the Searches */    
    
    OPEN c_search_properties    
    
    FETCH NEXT FROM c_search_properties    
    INTO    @table_name ,    
            @column_name,    
       @gis_data_model_code    
    
    WHILE (@@FETCH_STATUS = 0)    
    BEGIN    
    
        SELECT @SQL = 'INSERT INTO #Matches_Found (Policy_Binder_id) SELECT ' + @gis_data_model_code + '_policy_binder_id  FROM ' + @table_name + ' WHERE ' + @column_name + '=' + convert(varchar(20),@party_cnt)    
  EXEC (@SQL)    
    
        FETCH NEXT FROM c_search_properties    
        INTO    @table_name ,    
                @column_name,    
                @gis_data_model_code    
    END    
    
    CLOSE c_search_properties    
    DEALLOCATE c_search_properties    
  
SELECT    
    ifi.insurance_file_cnt,    
    ifi.insurance_ref,    
    ifi.insurance_folder_cnt,    
    pr.code,    
    pr.description caption,    
    ifi.anniversary_copy,    
    noofclaims,  
 inft.code AS type_code,    
 infs.code AS status_code,  
 clm.claim_number  
    INTO #Policy_List     
FROM Insurance_File ifi WITH (NOLOCK)  
    INNER JOIN Product pr WITH (NOLOCK)  
     ON pr.product_id = ifi.product_id    
    INNER JOIN insurance_file_risk_link ifrl WITH (NOLOCK)  
     ON ifrl.insurance_file_cnt = ifi.insurance_file_cnt    
    INNER JOIN (SELECT insurance_file_cnt    
             FROM Insurance_File AS ifi_1    
             WHERE ifi_1.insurance_file_type_id IN (1,2,3,5,6,9)    
             AND ifi_1.policy_ignore IS NULL                     
   ) AS PL ON ifi.insurance_file_cnt = PL.insurance_file_cnt  
    LEFT JOIN (SELECT  policy_number,    
               COUNT(*) AS noofclaims    
               FROM claim WITH (NOLOCK)  
               GROUP BY policy_number) Claim    
        ON ifi.Insurance_Ref = Claim.Policy_Number    
 INNER JOIN Insurance_File_Type inft WITH (NOLOCK)  
  ON inft.insurance_file_type_id = ifi.insurance_file_type_id   
 LEFT OUTER JOIN Insurance_File_Status infs WITH (NOLOCK)  
  ON ifi.insurance_file_status_id = infs.insurance_file_status_id  
 LEFT JOIN claim clm WITH (NOLOCK)  
  ON ifi.Insurance_Ref = clm.Policy_Number  
WHERE ifrl.risk_cnt in(SELECT risk_id   
     FROM gis_policy_link WITH (NOLOCK)  
     WHERE gis_policy_link_id in (SELECT m.policy_binder_id   
    FROM #Matches_Found m Where m.sr_no > 0)              )    
    AND ifi.insured_cnt <> @party_cnt   
  
UPDATE  p SET status_code='REP' FROM #Policy_List p INNEr JOIN insurance_file i ON i.insurance_ref=p.insurance_ref WHERE i.insurance_file_status_id=4
SELECT * FROM #Policy_List  WHERE insurance_file_cnt IN (SELECT MAX(insurance_file_cnt)  
                        FROM #Policy_List   GROUP BY insurance_ref ,insurance_folder_cnt)  
  
  
DROP TABLE #Matches_Found    
DROP TABLE #Policy_List   
SET NOCOUNT OFF    
END    
  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
