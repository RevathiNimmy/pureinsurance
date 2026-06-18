EXECUTE DDLDROPPROCEDURE spu_recalculate_RI    
GO
CREATE PROCEDURE spu_recalculate_RI    
    @nInsurance_file_cnt integer,    
    @dtTransfer_date datetime,    
    @dPro_rata_rate float=1,    
    @nIs_PT int = 0,    
    @nIs_valid INT = NULL OUTPUT  ,    
    @nIs_Amend INT = 0,    
    @nSkip_posting INT=0    
    
AS    
    
BEGIN    
    
    DECLARE @risk_status INTEGER,    
            @risk_cnt INTEGER,    
   @status_flag VARCHAR,    
   @gis_policy_link_id INTEGER,    
   @rating_section_id INTEGER,    
   @transactionType varchar(20),    
   @old_pro_rata_rate float,    
   @insurance_folder_cnt int,    
   @version_id INT=1,    
   @status_id INT    
    
 SELECT @nIs_valid=1    
    
 SELECT  @insurance_folder_cnt=insurance_folder_cnt    
 FROM insurance_file    
 WHERE insurance_file_cnt=@nInsurance_file_cnt    
    
 IF @nSkip_posting=1    
  SELECT @status_id=2    
 ELSE    
  SELECT @status_id=3    
    
 SELECT @transactionType=ISNULL(Code,'NB')    
 FROM Insurance_File_System ifs JOIN Transaction_Type t    
 ON ifs.last_trans_type_id = t.transaction_type_id    
 WHERE ifs.insurance_file_cnt=@nInsurance_file_cnt    
    
    DECLARE c_risk CURSOR FAST_FORWARD FOR    
         SELECT r.risk_cnt, ifrl.status_flag, isnull(r.pro_rata_rate,1)    
         FROM risk r    
   INNER JOIN insurance_file_risk_link ifrl    
         ON r.risk_cnt = ifrl.risk_cnt    
         WHERE insurance_file_cnt = @nInsurance_file_cnt    
    
    OPEN c_risk    
    FETCH NEXT FROM c_risk INTO @risk_cnt,@status_flag,@old_pro_rata_rate    
    WHILE @@FETCH_STATUS = 0    
    BEGIN    
     IF EXISTS (SELECT *    
                FROM    risk r    
                JOIN    ri_arrangement ra    
                        ON ra.risk_cnt = r.risk_cnt    
                JOIN    risk_type_ri_model_usage u    
                        ON u.risk_type_id = r.risk_type_id    
                        AND u.ri_band = ra.ri_band_id    
                        AND u.ri_model_id = ra.ri_model_id    
    LEFT JOIN risk_type_ri_model_usage u2    
                        ON u2.portfolio_transfer_from_cnt = u.risk_type_ri_model_usage_cnt    
    LEFT JOIN  RI_Band_version rb on rb.ri_band_id =ra.ri_band_id    
    Where   r.risk_cnt = @risk_cnt    
        AND u2.portfolio_transfer_from_cnt is not null -- Risk is against a replaced RI model    
        AND IsNull(u.expiry_date,'29-Dec-1899') <= @dtTransfer_date    
        AND ra.original_flag = 0 -- "live" RI model    
        AND u.is_deleted = 0    
        AND u2.is_deleted = 0    
        AND rb.Proportional_RI_Cal_Method =2 )    
   BEGIN    
    
    SELECT @version_id = MAX(version_id) FROM RI_Arrangement WHERE risk_cnt = @risk_cnt    
    
    IF @version_id >= 1    
    SET @version_id =@version_id +1    
    
    IF  @nIs_PT = 0    
     EXEC Spu_ri_arrangement_refresh_ri2007 @insurance_file_cnt=@nInsurance_file_cnt,@risk_cnt=@risk_cnt,@trans_type=@transactionType, @version_id=@version_id, @TransferDate= @dtTransfer_date    
    ELSE    
     EXEC Spu_ri_arrangement_refresh_ri2007 @insurance_file_cnt=@nInsurance_file_cnt,@risk_cnt=@risk_cnt,@trans_type='PT', @version_id=@version_id, @TransferDate= @dtTransfer_date    
    
    IF @nIs_PT=1    
     BEGIN    
        UPDATE RI_Arrangement SET premium=@dPro_rata_rate*premium/@old_pro_rata_rate,pro_rata_rate=@dPro_rata_rate WHERE risk_cnt=@risk_cnt and version_id=@version_id    
    
        UPDATE RI_Arrangement_Line SET  premium_value=@dPro_rata_rate*premium_value/@old_pro_rata_rate,    
        premium_tax=@dPro_rata_rate*premium_tax/@old_pro_rata_rate, commission_tax=@dPro_rata_rate*commission_tax/@old_pro_rata_rate,    
        commission_value=@dPro_rata_rate*commission_value/@old_pro_rata_rate WHERE ri_arrangement_id in    
        (SELECT ri_arrangement_id  FROM RI_Arrangement WHERE risk_cnt=@risk_cnt and version_id=@version_id)    
     END    
    
    IF NOT EXISTS (SELECT NULL FROM    
        (SELECT ri_arrangement_id, abs(round(sum_insured,2)) SI,abs(round(premium,2)) Prem    
        FROM RI_Arrangement WHERE risk_cnt=@risk_cnt and version_id=@version_id and original_flag=0) Total    
        JOIN    
        (SELECT ri_arrangement_id, abs(round(sum(sum_insured),2)) SI,abs(round(sum(premium_value),2)) Prem    
        FROM RI_Arrangement_Line WHERE ri_arrangement_id in (SELECT ri_arrangement_id FROM RI_Arrangement WHERE risk_cnt=@risk_cnt and version_id=@version_id and original_flag=0) group by ri_arrangement_id) Single    
        ON Total.ri_arrangement_id=Single.ri_arrangement_id    
        WHERE abs(Total.SI-Single.SI)>0.02 or abs(Total.Prem-Single.Prem)>0.02)    
      BEGIN    
        INSERT INTO insurance_file_pt_log (insurance_folder_cnt , insurance_file_cnt,risk_cnt,Status_id,Effective_Date )    
        VALUES (@insurance_folder_cnt ,@nInsurance_file_cnt,@risk_cnt,@status_id,@dtTransfer_date  )    
      END    
    ELSE    
      BEGIN    
       SET @nIs_valid = 0    
       INSERT INTO insurance_file_pt_log (insurance_folder_cnt , insurance_file_cnt,risk_cnt,Status_id,Effective_Date )    
       VALUES (@insurance_folder_cnt ,@nInsurance_file_cnt,@risk_cnt,1,@dtTransfer_date  )    
       IF @nIs_Amend=0 AND @nIs_PT <> 0    
        BREAK    
      END    
    
   END    
 FETCH NEXT FROM c_risk INTO @risk_cnt,@status_flag ,@old_pro_rata_rate    
 END    
 CLOSE c_risk    
 DEALLOCATE c_risk    
END 
GO
