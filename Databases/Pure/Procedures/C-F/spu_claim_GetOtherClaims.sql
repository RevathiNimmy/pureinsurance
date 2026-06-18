
SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_claim_GetOtherClaims'
GO

CREATE PROCEDURE spu_claim_GetOtherClaims  
@party_cnt INT  
AS  
BEGIN  

SET NOCOUNT ON

DECLARE @SQL             VARCHAR(255) ,    
@gis_data_model_id  INTEGER ,    
@object_name        VARCHAR(70) ,    
@property_name      VARCHAR(70) ,    
@table_name     VARCHAR(70) ,    
@column_name        VARCHAR(70) ,    
@gis_data_model_code    VARCHAR(70) ,    
@policy_binder_id       INTEGER,
@product_id             INTEGER,
@claim_id               INTEGER    
    
CREATE TABLE #Matches_Found (policy_binder_id int)
CREATE TABLE #Temp (claim_id int,claim_status_id int,info_only int,policy_number varchar(250),DESCRIPTION_claim varchar(250),claim_number varchar(250),product_id int,
DESCRIPTION_policy varchar(250),loss_from_date datetime,claim_status varchar(250),total_indemnity numeric,total_expense numeric,total_excecc numeric,
case_number varchar(250),insurance_file_cnt integer,is_other_claim integer,risk_type_id integer)    
/* First Select the Data Model ID from the Data Model Code */    
DECLARE c_search_properties CURSOR FAST_FORWARD FOR    
    
SELECT o.object_name,    
    o.table_name,    
    p.property_name,    
    p.column_name,    
    Rtrim(gdm.code)    
FROM  gis_object o    
INNER JOIN gis_property p    
ON ( o.gis_object_id = p.gis_object_id )    
INNER JOIN gis_data_model gdm    
ON ( gdm.gis_data_model_id = o.gis_data_model_id )    
WHERE  p.data_type = 2    
   AND p.specials_type = 3    
   AND o.gis_data_model_id IN(SELECT gis_data_model_id    
       FROM gis_data_model gdm    
       INNER JOIN gis_data_model_type gdmt    
        ON gdm.gis_data_model_type_id = gdmt.gis_data_model_type_id    
       WHERE object_name not in ('Work_Claim_Peril','Work_Claim') AND gdmt.is_deleted = 0)    
    
OPEN c_search_properties    
    
FETCH NEXT FROM c_search_properties INTO @object_name, @table_name, @property_name, @column_name, @gis_data_model_code    
    
WHILE ( @@FETCH_STATUS = 0 )    
BEGIN    
 SELECT @SQL = 'INSERT INTO #Matches_Found (Policy_Binder_id)     
 SELECT ' + @gis_data_model_code + '_policy_binder_id  FROM ' + @table_name +     
 ' WHERE ' + @column_name + '=' + CONVERT(VARCHAR(20), @party_cnt)      
 EXEC (@SQL)    
    
FETCH NEXT FROM c_search_properties INTO @object_name, @table_name, @property_name, @column_name, @gis_data_model_code    
END    
    
CLOSE c_search_properties    
    
DEALLOCATE c_search_properties    
   
DECLARE exact_match CURSOR FAST_FORWARD FOR 
SELECT DISTINCT
    
  ifi.product_id,
  clm.claim_id    
    
 FROM claim  clm    
 INNER JOIN claim_status cstat    
  ON clm.claim_status_id = cstat.claim_status_id    
 INNER JOIN (SELECT MAX(claim_id)as claim_id, MAX(version_id) as version_id, base_claim_id    
    FROM claim    
    WHERE is_dirty = 0    
    GROUP by base_claim_id) claim_versions    
  ON clm.claim_id = claim_versions.claim_id    
    INNER JOIN gis_policy_link gpl    
  ON clm.risk_type_id=gpl.risk_id    
 INNER JOIN #Matches_Found m    
  ON gpl.gis_policy_link_id= m.policy_binder_id    
    INNER JOIN insurance_file_risk_link ifrl    
     ON ifrl.risk_cnt = gpl.risk_id    
 INNER JOIN insurance_file ifi    
  ON clm.policy_id=ifi.insurance_file_cnt    
  INNER JOIN product p    
  ON ifi.product_id=p.product_id      
WHERE ifi.insured_cnt<>@party_cnt    
 AND ifrl.status_flag<>'D'      
 end
 
 OPEN exact_match    
    
FETCH NEXT FROM exact_match INTO @product_id, @claim_id
 WHILE ( @@FETCH_STATUS = 0 )    
BEGIN 
if  @product_id<>6
Begin
insert into #Temp
SELECT 
DISTINCT clm.claim_id,    
  clm.claim_status_id,    
  clm.info_only,    
  clm.policy_number,    
  clm.DESCRIPTION,    
  clm.claim_number,    
  ifi.product_id,   
  p.DESCRIPTION,    
  clm.loss_from_date,    
  cstat.DESCRIPTION                              'status',   
  Isnull((SELECT SUM(( r.initial_reserve + r.revised_reserve ) - paid_to_date)    
  FROM   claim_peril cp    
   JOIN   reserve r    
    ON cp.claim_peril_id = r.claim_peril_id    
   JOIN   reserve_type rt    
    ON r.reserve_type_id = rt.reserve_type_id    
   AND rt.is_indemnity = 1    
   WHERE  cp.claim_id = clm.claim_id), 0) 'total_indemnity',    
  Isnull((SELECT SUM(( r.initial_reserve + r.revised_reserve ) - paid_to_date)    
  FROM   claim_peril cp    
   JOIN   reserve r    
    ON cp.claim_peril_id = r.claim_peril_id    
   JOIN   reserve_type rt    
    ON r.reserve_type_id = rt.reserve_type_id    
   AND rt.is_expense = 1    
   WHERE  cp.claim_id = clm.claim_id), 0) 'total_expense',    
  Isnull((SELECT SUM(( r.initial_reserve + r.revised_reserve ) - paid_to_date)    
  FROM   claim_peril CP    
   JOIN   reserve R    
    ON CP.claim_peril_id = r.claim_peril_id    
   JOIN   reserve_type RT    
    ON r.reserve_type_id = rt.reserve_type_id    
   AND rt.is_excess = 1    
   WHERE  CP.claim_id = clm.claim_id), 0) 'total_excess',    
  (SELECT case_number    
  FROM   [case]    
  WHERE  case_id = clm.base_case_id)            'case_number',    
  ifi.insurance_file_cnt,    
  1                                              'is_other_claim',    
  risk_type_id    
 FROM claim  clm    
 INNER JOIN claim_status cstat    
  ON clm.claim_status_id = cstat.claim_status_id    
 INNER JOIN (SELECT MAX(claim_id)as claim_id, MAX(version_id) as version_id, base_claim_id    
    FROM claim    
    WHERE is_dirty = 0    
    GROUP by base_claim_id) claim_versions    
  ON clm.claim_id = claim_versions.claim_id    
    INNER JOIN gis_policy_link gpl    
  ON clm.risk_type_id=gpl.risk_id    
 INNER JOIN #Matches_Found m    
  ON gpl.gis_policy_link_id= m.policy_binder_id    
    INNER JOIN insurance_file_risk_link ifrl    
     ON ifrl.risk_cnt = gpl.risk_id    
 INNER JOIN insurance_file ifi    
  ON clm.policy_id=ifi.insurance_file_cnt    
  INNER JOIN product p    
  ON ifi.product_id=p.product_id      
WHERE ifi.insured_cnt<>@party_cnt    
 AND ifrl.status_flag<>'D'
 AND clm.claim_id = @claim_id      
End  
else
Begin
insert into #Temp
SELECT DISTINCT clm.claim_id,    
  clm.claim_status_id,    
  clm.info_only,    
  clm.policy_number,    
  clm.DESCRIPTION,    
  clm.claim_number,    
  ifi.product_id,    
  p.DESCRIPTION,    
  clm.loss_from_date,    
  cstat.DESCRIPTION                              'status',    
  Isnull((SELECT SUM(( r.initial_reserve + r.revised_reserve ) - paid_to_date)    
  FROM   claim_peril cp    
   JOIN   reserve r    
    ON cp.claim_peril_id = r.claim_peril_id    
   JOIN   reserve_type rt    
    ON r.reserve_type_id = rt.reserve_type_id    
   AND rt.is_indemnity = 1    
   WHERE  cp.claim_id = clm.claim_id), 0) 'total_indemnity',    
  Isnull((SELECT SUM(( r.initial_reserve + r.revised_reserve ) - paid_to_date)    
  FROM   claim_peril cp    
   JOIN   reserve r    
    ON cp.claim_peril_id = r.claim_peril_id    
   JOIN   reserve_type rt    
    ON r.reserve_type_id = rt.reserve_type_id    
   AND rt.is_expense = 1    
   WHERE  cp.claim_id = clm.claim_id), 0) 'total_expense',    
  Isnull((SELECT SUM(( r.initial_reserve + r.revised_reserve ) - paid_to_date)    
  FROM   claim_peril CP    
   JOIN   reserve R    
    ON CP.claim_peril_id = r.claim_peril_id    
   JOIN   reserve_type RT    
    ON r.reserve_type_id = rt.reserve_type_id    
   AND rt.is_excess = 1    
   WHERE  CP.claim_id = clm.claim_id), 0) 'total_excess',    
  (SELECT case_number    
  FROM   [case]    
  WHERE  case_id = clm.base_case_id)            'case_number',    
  ifi.insurance_file_cnt,    
  1                                              'is_other_claim',    
  risk_type_id    
 FROM claim  clm    
 INNER JOIN claim_status cstat    
  ON clm.claim_status_id = cstat.claim_status_id    
 INNER JOIN (SELECT MAX(claim_id)as claim_id, MAX(version_id) as version_id, base_claim_id    
    FROM claim    
    WHERE is_dirty = 0    
    GROUP by base_claim_id) claim_versions    
  ON clm.claim_id = claim_versions.claim_id    
    INNER JOIN gis_policy_link gpl    
  ON clm.risk_type_id=gpl.risk_id    
 INNER JOIN #Matches_Found m    
  ON gpl.gis_policy_link_id= m.policy_binder_id    
    INNER JOIN insurance_file_risk_link ifrl    
     ON ifrl.risk_cnt = gpl.risk_id    
 INNER JOIN insurance_file ifi    
  ON clm.policy_id=ifi.insurance_file_cnt    
  INNER JOIN product p    
  ON ifi.product_id=p.product_id    
 JOIN GIS_Policy_Link GPLC    
 on GPLC.claim_id=clm.Claim_id    
 Join CB_ERSLOT CRO     
 on CRO.CB_Policy_binder_id=GPLC.gis_policy_link_id    
WHERE ifi.insured_cnt<>@party_cnt    
 AND ifrl.status_flag<>'D'     
 AND CRO.Find_Doctor =@party_cnt and CRO.Claim_Owner=1
 AND clm.claim_id = @claim_id 
End  

FETCH NEXT FROM exact_match INTO @product_id, @claim_id   
END  
  
select distinct * from #Temp     
 DROP TABLE #Matches_Found  
  DROP TABLE #Temp   
  
SET NOCOUNT OFF   

GO
