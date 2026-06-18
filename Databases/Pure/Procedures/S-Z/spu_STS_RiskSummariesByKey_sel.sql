EXECUTE DDLDropProcedure 'spu_STS_RiskSummariesByKey_sel'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_STS_RiskSummariesByKey_sel  
  
    @insurance_file_cnt INT,  
    @ProductCode VARCHAR(30) = 'noproductcode',  
    @isviaSAM INT=0,  
    @IncludeGISRetroactiveDate INT=0     
AS  
DECLARE @SQL VARCHAR(2000),  
        @gis_data_model_code  VARCHAR(10) ,  
        @gis_data_model_id    INT,  
        @risk_id              INT  
  
CREATE TABLE #Matches_Found (risk_id INT, value VARCHAR(255))  
  
IF @IncludeGISRetroactiveDate=1  
BEGIN  
  
DECLARE c_gis_objects CURSOR FAST_FORWARD FOR   
SELECT  RTRIM(gdm.code),  
        rsk.risk_cnt  
  FROM  gis_data_model gdm  
  JOIN  gis_screen gs  
    ON  gs.gis_data_model_id = gdm.gis_data_model_id  
  JOIN risk rsk  
    ON rsk.gis_screen_id = gs.gis_screen_id  
  JOIN insurance_file_risk_link infrl  
    ON infrl.risk_cnt = rsk.risk_cnt  
   AND infrl.insurance_file_cnt = @insurance_file_cnt  
  
  OPEN c_gis_objects   
  FETCH NEXT FROM c_gis_objects  
  INTO  @gis_data_model_code,  
        @risk_id  
    
    WHILE (@@FETCH_STATUS = 0)  
      BEGIN  
       SELECT @SQL = 'INSERT INTO #Matches_Found (risk_id, value) SELECT risk_id, retroactive_date '   
       SELECT @SQL = @SQL + ' FROM ' + @gis_data_model_code + '_S4IDefault g JOIN ' + @gis_data_model_code +                                   '_policy_binder pb on pb.' + @gis_data_model_code + '_policy_binder_id=g.' + @gis_data_model_code +                        '_policy_binder_id '  
  
       SELECT @SQL = @SQL + ' JOIN gis_policy_link gpl' + ' on pb.gis_policy_link_id=gpl.gis_policy_link_id WHERE                               risk_id='+ CONVERT(VARCHAR,@risk_id)  
   
       EXEC(@SQL)  
  
  FETCH NEXT FROM c_gis_objects  
          INTO    @gis_data_model_code,  
                  @risk_id  
    END  
  
CLOSE c_gis_objects   
DEALLOCATE c_gis_objects  

END  
-- If there is no product code passed in then we default it to the product code FROM insurance_file  
  
IF (@ProductCode = 'noproductcode')  
BEGIN  
        SELECT @ProductCode = p.code  
          FROM product as p  
    INNER JOIN insurance_file as insfile on insfile.product_id = p.product_id  
         WHERE insfile.insurance_file_cnt = @insurance_file_cnt  
END 
  
   SELECT   risk.risk_cnt 'RiskCnt',  
   risk.risk_folder_cnt 'RiskFolderCnt',  
   rt.code 'RiskTypeCode',  
   risk.description 'Description',  
   risk.total_sum_insured 'TotalSumInsured',  
   risk.total_annual_premium 'Premium',  
   (CASE WHEN (insurance_file_risk_link.Status_flag ='D' AND @isviaSAM = 1)
         THEN 'DELETED'  
         ELSE  rs.code  
          END   ) 'Status',  
   Cast(m.value as datetime) 'retroactive_date',   
   risk.inception_date,
   (CASE WHEN(SELECT COUNT(DISTINCT clm.claim_id) FROM claim clm LEFT OUTER JOIN Risk R ON R.RISK_CNT= clm.Risk_type_id WHERE R.risk_folder_cnt=   risk.risk_folder_cnt)>0 
			THEN 1
			ELSE 0
	END) 'HasClaimLink', Risk.risk_number,
	insurance_file_risk_link.status_flag 'risk_link_status_flag',
	[LastVersion].cover_start_date 'risk_link_change_date',
	insurance_file_risk_link.original_Risk_cnt,
	CASE WHEN EXISTS (Select Null From insurance_file_risk_link ifrl Inner Join risk r On r.risk_cnt = ifrl.risk_cnt AND r.risk_folder_cnt = risk.risk_folder_cnt AND ifrl.insurance_file_cnt = insurance_file.Base_Insurance_File_Cnt Where ISNull(is_risk_edited, 0) = 1) AND insurance_file_risk_link.status_flag <> 'D'
	THEN 1 ELSE 0 END 'is_edited', -- relevant to OOS only; check in base version
	risk.is_risk_selected 'Is_Risk',
	CASE 
		WHEN EXISTS(SELECT 
					1
					FROM
					RI_Arrangement RIA
					LEFT OUTER JOIN RI_Arrangement_Line RAL
					ON RAL.RI_Arrangement_ID=RIA.RI_Arrangement_ID
					WHERE
					RIA.Risk_Cnt IN(insurance_file_risk_link.Risk_Cnt,insurance_file_risk_link.Original_Risk_Cnt)
					AND RAL.Type IN ('F','FX')
				   ) THEN 1
		ELSE 0
		END AS HasFacProp,
		CASE WHEN ISNULL((Select top 1 is_amended from Rating_Section where risk_cnt=risk.risk_cnt and is_amended=1),0)=0 Then 1 Else 0 End as Is_Auto_Rated
FROM insurance_file
   INNER JOIN insurance_file_risk_link 
		INNER JOIN Risk 
	INNER JOIN Risk_Type AS rt 
	ON risk.risk_type_id = rt.risk_type_id
	ON insurance_file_risk_link.risk_cnt = risk.risk_cnt
	ON insurance_file.insurance_file_cnt = insurance_file_risk_link.insurance_file_cnt					
	INNER JOIN insurance_file_risk_link [LastChangeVerionLink]
	INNER JOIN insurance_file [LastVersion]
	ON [LastVersion].insurance_file_cnt = [LastChangeVerionLink].insurance_file_cnt
	ON [LastChangeVerionLink].risk_cnt = insurance_file_risk_link.risk_cnt 
	AND ([LastChangeVerionLink].status_flag = 'C' OR [LastChangeVerionLink].status_flag = 'D')
	LEFT OUTER JOIN Risk_status AS rs ON risk.risk_status_id = rs.risk_status_id
	LEFT JOIN #Matches_Found m  ON risk.risk_cnt = m.risk_id
WHERE insurance_file.insurance_file_cnt = @insurance_file_cnt
ORDER BY risk.risk_number
GO
SET QUOTED_IDENTIFIER OFF 
GO


