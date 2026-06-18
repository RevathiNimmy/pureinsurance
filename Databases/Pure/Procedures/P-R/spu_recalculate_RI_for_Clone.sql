EXECUTE DDLDropProcedure 'spu_recalculate_RI_For_Clone'
GO
CREATE  PROCEDURE spu_recalculate_RI_For_Clone  
    @nInsurance_file_cnt INTEGER,  
    @nIs_valid INTEGER = NULL OUTPUT  
AS  
  
BEGIN  
  
    DECLARE @risk_status INTEGER,  
            @risk_cnt INTEGER,  
			@status_flag VARCHAR,  
			@gis_policy_link_id INTEGER,  
			@rating_section_id INTEGER,  
			@transactionType VARCHAR(20)  ,  
			@insurance_folder_cnt INTEGER ,    
			@version_id INT, 
			@pro_rata_rate FLOAT, 
			@old_pro_rata_rate  FLOAT  ,
			@ri_arrangement_id INT  

    SELECT @insurance_folder_cnt = insurance_folder_cnt FROM insurance_file WHERE insurance_file_cnt = @nInsurance_file_cnt 
	SELECT @nIs_valid=1
    
	DECLARE c_risk CURSOR FAST_FORWARD FOR  
    SELECT r.risk_cnt, ifrl.status_flag, ISNULL(r.pro_rata_rate,1)  
    FROM risk r INNER JOIN insurance_file_risk_link ifrl  
    ON r.risk_cnt = ifrl.risk_cnt  
	WHERE insurance_file_cnt = @nInsurance_file_cnt 
	      
    OPEN c_risk  
    FETCH NEXT FROM c_risk INTO @risk_cnt,@status_flag  , @old_pro_rata_rate  
    WHILE @@FETCH_STATUS = 0  
		BEGIN  
			IF EXISTS(SELECT  NULL FROM ri_arrangement WHERE  risk_cnt = @risk_cnt AND  cloned = 1)  
				BEGIN  
					SELECT @version_id = ISNULL(MAX(version_id),1) from RI_Arrangement where risk_cnt=@risk_cnt and original_flag=0  
					SELECT @pro_rata_rate=ISNULL(pro_rata_rate,1) from RI_Arrangement where risk_cnt=@risk_cnt and original_flag=0  and  version_id = @version_id  
     
					INSERT INTO RI_Arrangement_line_Broker_Participants_Archive  
						(ri_arrangement_line_id,  
						 ri_party_cnt,  
						 participation_percent)  
					SELECT RALBP.ri_arrangement_line_id,  
						 RALBP.ri_party_cnt,  
						 RALBP.participation_percent 
					FROM RI_Arrangement_line_Broker_Participants RALBP 
					JOIN RI_Arrangement_Line  RAL ON RALBP.ri_arrangement_line_id =RAL.ri_arrangement_line_id 
					JOIN RI_Arrangement RA ON RA.ri_arrangement_id= RAL.ri_arrangement_id 
					WHERE  RA.risk_cnt =@risk_cnt AND RA.version_id =@version_id  
  
        			INSERT INTO  RI_Arrangement_Line_Archive  
						 (ri_arrangement_line_id,  
						 ri_arrangement_id,  
						 type,  
						 treaty_id,  
						 party_cnt,  
						 default_share_percent,  
						 this_share_percent,  
						 premium_percent,  
						 commission_percent,  
						 agreement_code,  
						 priority,  
						 number_of_lines,  
						 line_limit,  
						 sum_insured,  
						 premium_value,  
						 commission_value,  
						 premium_tax,  
						 commission_tax,  
						 is_commission_modified,  
						 retained,  
						 lower_limit,  
						 participation_percent,  
						 grouping,  
						 ri_model_line_id,  
						 Is_Obligatory,
						 ri_arrangement_line_Version_id,
						 created_date)  
					SELECT ri_arrangement_line_id,  
						 ri_arrangement_id,  
						 type,  
						 treaty_id,  
						 party_cnt,  
						 default_share_percent,  
						 this_share_percent,  
						 premium_percent,  
						 commission_percent,  
						 agreement_code,  
						 priority,  
						 number_of_lines,  
						 line_limit,  
						 sum_insured,  
						 premium_value,  
						 commission_value,  
						 premium_tax,  
						 commission_tax,  
						 is_commission_modified,  
						 retained,  
						 lower_limit,  
						 participation_percent,  
						 grouping,  
						 ri_model_line_id,  
						 Is_Obligatory,
						 (SELECT MAX(ISNULL(ri_arrangement_line_Version_id,0))+1 FROM RI_Arrangement_Line_Archive WHERE  ri_arrangement_id in 
						 (SELECT ri_arrangement_id FROM RI_Arrangement WHERE risk_cnt =@risk_cnt AND version_id =@version_id)),
						 GETDATE()
					FROM RI_Arrangement_Line  
					WHERE ri_arrangement_id IN (SELECT ri_arrangement_id FROM RI_Arrangement WHERE risk_cnt =@risk_cnt AND version_id =@version_id )  
								AND type Not In ('F','FX')  
  
					IF @version_id>1  
						BEGIN 
							UPDATE RI_Arrangement_Line SET  premium_value= @old_pro_rata_rate*premium_value/@pro_rata_rate,  
								   premium_tax= @old_pro_rata_rate*premium_tax/@pro_rata_rate, commission_tax= @old_pro_rata_rate*commission_tax/@pro_rata_rate ,  
								   commission_value= @old_pro_rata_rate*commission_value/@pro_rata_rate 
							WHERE ri_arrangement_id IN (select ri_arrangement_id  from RI_Arrangement where risk_cnt=@risk_cnt and version_id=@version_id and original_flag=0)  
								AND type IN ('FX')
						END

					EXEC Spu_ri_arrangement_refresh_ri2007 @nInsurance_file_cnt,@risk_cnt,'DRI' ,@version_id=@version_id  
  
					IF @version_id>1  
						BEGIN  
							UPDATE RI_Arrangement SET premium=@pro_rata_rate*premium/@old_pro_rata_rate,pro_rata_rate=@pro_rata_rate where risk_cnt=@risk_cnt and version_id=@version_id and original_flag=0  
  
						    UPDATE RI_Arrangement_Line SET  premium_value=@pro_rata_rate*premium_value/@old_pro_rata_rate,  
						   premium_tax=@pro_rata_rate*premium_tax/@old_pro_rata_rate, commission_tax=@pro_rata_rate*commission_tax/@old_pro_rata_rate,  
						   commission_value=@pro_rata_rate*commission_value/@old_pro_rata_rate where ri_arrangement_id in  
						   (select ri_arrangement_id  from RI_Arrangement where risk_cnt=@risk_cnt and version_id=@version_id and original_flag=0)  
  
						END  
      
					IF NOT EXISTS (SELECT NULL FROM  
						(SELECT ri_arrangement_id, abs(round(sum_insured,2)) SI,abs(round(premium,2)) Prem from RI_Arrangement where risk_cnt=@risk_cnt  and original_flag=0 and version_id=@version_id) Total  
						JOIN  
						(SELECT ri_arrangement_id, abs(round(sum(sum_insured),2)) SI,abs(round(sum(premium_value),2)) Prem from RI_Arrangement_Line WHERE ri_arrangement_id in (SELECT ri_arrangement_id from RI_Arrangement where risk_cnt=@risk_cnt and original_flag=0 And version_id=@version_id) group by ri_arrangement_id) 
							Single    
						ON Total.ri_arrangement_id=Single.ri_arrangement_id  
						WHERE abs(Total.SI-Single.SI)>0.02 or abs(Total.Prem-Single.Prem)>0.02)  
							BEGIN  
     							EXEC spu_Update_RIArrangement_ClonedStatus @risk_cnt=@risk_cnt,@cloned=2  
								INSERT INTO insurance_file_clone_log (insurance_file_cnt,insurance_folder_cnt,risk_cnt ,Status_id )  
								VALUES (@nInsurance_file_cnt,@insurance_folder_cnt,@risk_cnt,3)  
  							END  
					ELSE  
						BEGIN  
							 SET @nIs_valid = 0  
							 INSERT INTO insurance_file_clone_log (insurance_file_cnt,insurance_folder_cnt ,risk_cnt ,Status_id )  
							 VALUES (@nInsurance_file_cnt,@insurance_folder_cnt ,@risk_cnt,1)  
						END  
				END  

			DECLARE c_ri_lines CURSOR FAST_FORWARD FOR  
			SELECT ri_arrangement_id  
			FROM ri_arrangement  
			WHERE risk_cnt=@risk_cnt and version_id=@version_id and original_flag=1
			OPEN c_ri_lines  
			FETCH NEXT FROM c_ri_lines INTO @ri_arrangement_id 
			WHILE @@FETCH_STATUS = 0  
				BEGIN  
					EXEC spu_upd_Premium_Percent_RI2007 @ri_arrangement_id
					FETCH NEXT FROM c_ri_lines INTO @ri_arrangement_id 
				END	
			CLOSE c_ri_lines  
			DEALLOCATE c_ri_lines  
			SELECT @nIs_valid    
 
	 FETCH NEXT FROM c_risk INTO @risk_cnt,@status_flag  , @old_pro_rata_rate  
     END  
	 CLOSE c_risk  
	 DEALLOCATE c_risk  
  
END   
