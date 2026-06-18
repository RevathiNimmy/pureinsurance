EXECUTE DDLDROPPROCEDURE spu_recalculate_RI_Quote  
GO
CREATE PROCEDURE spu_recalculate_RI_Quote
    @nInsurance_file_cnt INTEGER, 
    @dtTransfer_date DATETIME,
    @nIs_valid INT = NULL OUTPUT

AS    
    
BEGIN    
    
    DECLARE @risk_status INTEGER,    
            @risk_cnt INTEGER,    
			@status_flag VARCHAR,
			@gis_policy_link_id INTEGER,    
			@rating_section_id INTEGER,
			@transactionType VARCHAR(20),
			@insurance_folder_cnt INTEGER  

	SELECT @nIs_valid=1	
	SELECT @insurance_folder_cnt=insurance_folder_cnt  FROM insurance_file
	WHERE insurance_file_cnt=@nInsurance_file_cnt

	SELECT @transactionType=ISNULL(Code,'NB') FROM Insurance_File_System ifs JOIN Transaction_Type t
	ON ifs.last_trans_type_id = t.transaction_type_id
	WHERE ifs.insurance_file_cnt=@nInsurance_file_cnt

    DECLARE c_risk CURSOR FAST_FORWARD FOR    
        SELECT r.risk_cnt, ifrl.status_flag    
        FROM risk r    
			INNER JOIN insurance_file_risk_link ifrl    
            ON r.risk_cnt = ifrl.risk_cnt    
        WHERE insurance_file_cnt = @nInsurance_file_cnt and ifrl.status_flag = 'C'      
    
    OPEN c_risk    
    FETCH NEXT FROM c_risk INTO @risk_cnt,@status_flag    
    WHILE @@FETCH_STATUS = 0    
		BEGIN    
			IF EXISTS (SELECT NULL 
                From    risk r  
                JOIN    ri_arrangement ra  
                        ON ra.risk_cnt = r.risk_cnt  
				JOIN    risk_type_ri_model_usage u  
                        ON u.risk_type_id = r.risk_type_id  
                        AND u.ri_band = ra.ri_band_id  
                        AND u.ri_model_id = ra.ri_model_id  
				LEFT JOIN  risk_type_ri_model_usage u2  
                        ON u2.portfolio_transfer_from_cnt = u.risk_type_ri_model_usage_cnt  
             WHERE   r.risk_cnt = @risk_cnt  
				   AND u2.portfolio_transfer_from_cnt IS NOT NULL -- Risk is against a replaced RI model  
				   AND IsNull(u.expiry_date,'29-Dec-1899') <= @dtTransfer_date  
				   AND ra.original_flag = 0 -- "live" RI model  
				   AND u.is_deleted = 0  
				   AND u2.is_deleted = 0)  
				BEGIN

					UPDATE risk SET risk_status_id=8 WHERE risk_cnt=@risk_cnt and risk_status_id=3
	
					EXEC Spu_ri_arrangement_refresh_ri2007 @insurance_file_cnt=@nInsurance_file_cnt,@risk_cnt=@risk_cnt,@trans_type=@transactionType, @copy_fac_risk_cnt=NULL,@version_id=1, @TransferDate= @dtTransfer_date
 
					IF NOT EXISTS (SELECT NULL FROM 
								(SELECT ri_arrangement_id, abs(round(sum_insured,2)) SI,abs(round(premium,2)) Prem FROM RI_Arrangement WHERE risk_cnt=@risk_cnt and version_id=1 and original_flag=0) Total 
								JOIN
								 (SELECT ri_arrangement_id, abs(round(sum(sum_insured),2)) SI,abs(round(sum(premium_value),2)) Prem FROM RI_Arrangement_Line WHERE ri_arrangement_id in (SELECT ri_arrangement_id FROM RI_Arrangement WHERE risk_cnt=@risk_cnt and version_id=1 and original_flag=0) group by ri_arrangement_id) Single
									ON Total.ri_arrangement_id=Single.ri_arrangement_id
									WHERE abs(Total.SI-Single.SI)>0.02 or abs(Total.Prem-Single.Prem)>0.02)
							BEGIN
								UPDATE risk SET risk_status_id=3 WHERE risk_cnt=@risk_cnt and risk_status_id=8
								INSERT INTO insurance_file_pt_log (insurance_folder_cnt , insurance_file_cnt,risk_cnt,Status_id,Effective_Date )
								VALUES (@insurance_folder_cnt ,@nInsurance_file_cnt,@risk_cnt,2 ,@dtTransfer_date  )
							END
					ELSE
							BEGIN
								SET @nIs_valid = 0
								INSERT INTO  insurance_file_pt_log (insurance_folder_cnt , insurance_file_cnt,risk_cnt,Status_id,Effective_Date )
								VALUES (@insurance_folder_cnt ,@nInsurance_file_cnt,@risk_cnt,1,@dtTransfer_date  )
							END
									
		
	END
	ELSE
	BEGIN
			Insert into insurance_file_pt_log (insurance_folder_cnt , insurance_file_cnt,risk_cnt,Status_id,Effective_Date )
				values (@insurance_folder_cnt ,@ninsurance_file_cnt,@risk_cnt,1,@dttransfer_date  )
				END
			FETCH NEXT FROM c_risk INTO @risk_cnt,@status_flag    
			END    
			CLOSE c_risk
			DEALLOCATE c_risk
		END   
