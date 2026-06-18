SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_recalculate_RI_Quote_For_Deferred'
GO

CREATE PROCEDURE spu_recalculate_RI_Quote_For_Deferred  
    @nInsurance_file_cnt INTEGER,  
    @nIs_valid INT = NULL OUTPUT  
AS  
  
BEGIN  
  
    DECLARE @risk_status INTEGER,  
            @risk_cnt INTEGER,  
            @status_flag VARCHAR,  
            @gis_policy_link_id INTEGER,  
		    @rating_section_id INTEGER,  
            @transactionType VARCHAR(20)  ,  
            @insurance_folder_cnt INTEGER ,  
		    @version_id INTEGER,  
		    @pro_rata_rate FLOAT,  
		    @old_pro_rata_rate  FLOAT  
  
 SELECT @insurance_folder_cnt = insurance_folder_cnt FROM insurance_file WHERE insurance_file_cnt = @nInsurance_file_cnt  
 SELECT @nIs_valid=1  
  
    DECLARE c_risk CURSOR FAST_FORWARD FOR  
         SELECT r.risk_cnt, ifrl.status_flag, isnull(r.pro_rata_rate,1)  
           FROM risk r  
      INNER JOIN insurance_file_risk_link ifrl  
             ON r.risk_cnt = ifrl.risk_cnt  
          WHERE insurance_file_cnt = @nInsurance_file_cnt and ifrl.status_flag IN ('C'  ,'D')  
  
    OPEN c_risk  
    FETCH NEXT FROM c_risk INTO @risk_cnt,@status_flag  , @old_pro_rata_rate  
    WHILE @@FETCH_STATUS = 0  
    BEGIN  
		UPDATE risk SET risk_status_id=8 WHERE risk_cnt=@risk_cnt AND risk_status_id in (3,9)   
  
    EXEC Spu_ri_arrangement_refresh @nInsurance_file_cnt,@risk_cnt,'DRI' 
  
   
  
    IF NOT EXISTS (SELECT NULL FROM  
      (SELECT ri_arrangement_id, ABS(ROUND(sum_insured,2)) SI,ABS(ROUND(premium,2)) Prem FROM RI_Arrangement WHERE risk_cnt=@risk_cnt AND original_flag=0) Total  
      JOIN  
      (SELECT ri_arrangement_id, ABS(ROUND(SUM(sum_insured),2)) SI,ABS(ROUND(SUM(premium_value),2)) Prem FROM RI_Arrangement_Line WHERE ri_arrangement_id IN (SELECT ri_arrangement_id FROM RI_Arrangement WHERE risk_cnt=@risk_cnt AND original_flag=0) group 
BY ri_arrangement_id) Single  
      ON Total.ri_arrangement_id=Single.ri_arrangement_id  
      WHERE ABS(Total.SI-Single.SI)>0.02 OR ABS(Total.Prem-Single.Prem)>0.02)  
         BEGIN  
       UPDATE risk SET risk_status_id=3 WHERE risk_cnt=@risk_cnt AND risk_status_id=8  
   
         END  
 
 SELECT @nIs_valid  
  
 FETCH NEXT FROM c_risk INTO @risk_cnt,@status_flag  , @old_pro_rata_rate  
    END  
 CLOSE c_risk  
 DEALLOCATE c_risk  
  
END  
GO


