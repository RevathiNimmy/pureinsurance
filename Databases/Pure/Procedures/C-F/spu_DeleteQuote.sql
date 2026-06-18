--*********************************************************************************************************  
-- Name : spu_DeleteQuote 
--   
-- Desc : delete this version of the quote and its dependanies   
--**********************************************************************************************************  
SET quoted_identifier OFF 

go 

SET ansi_nulls ON 

go 

EXECUTE Ddldropprocedure 
  'spu_DeleteQuote' 

go 

CREATE PROCEDURE Spu_deletequote @nInsuranceFileCnt INT 
AS 
  BEGIN 
      DECLARE @nReturnValue  INT, 
              @RiskCnt       INT, 
              @RiskFolderCnt INT 

      SELECT @nReturnValue = 0 

      BEGIN TRANSACTION 

      DELETE ri_arrangement_line_broker_participants 
      FROM   ri_arrangement_line_broker_participants rbp 
             JOIN ri_arrangement_line ral 
               ON rbp.ri_arrangement_line_id = ral.ri_arrangement_line_id 
             JOIN ri_arrangement rra 
               ON ral.ri_arrangement_id = rra.ri_arrangement_id 
             JOIN insurance_file_risk_link ifrl 
               ON ifrl.risk_cnt = rra.risk_cnt 
      WHERE  ifrl.insurance_file_cnt = @nInsuranceFileCnt 
             AND ifrl.status_flag IN ( 'C', 'D' ) 

      IF @@ERROR <> 0 
        GOTO error_label 

      DELETE ri_arrangement_line 
      FROM   ri_arrangement_line ral 
             JOIN ri_arrangement rra 
               ON ral.ri_arrangement_id = rra.ri_arrangement_id 
             JOIN insurance_file_risk_link ifrl 
               ON ifrl.risk_cnt = rra.risk_cnt 
      WHERE  ifrl.insurance_file_cnt = @nInsuranceFileCnt 
             AND ifrl.status_flag IN ( 'C', 'D' ) 

      IF @@ERROR <> 0 
        GOTO error_label 

      DELETE ri_arrangement 
      FROM   ri_arrangement rra 
             JOIN insurance_file_risk_link ifrl 
               ON rra.risk_cnt = ifrl.risk_cnt 
      WHERE  ifrl.insurance_file_cnt = @nInsuranceFileCnt 
             AND ifrl.status_flag IN ( 'C', 'D' ) 

      IF @@ERROR <> 0 
        GOTO error_label 

      DELETE FROM tax_calculation 
      WHERE  insurance_file_cnt = @nInsuranceFileCnt 

      IF @@ERROR <> 0 
        GOTO error_label 

      DELETE FROM policy_fee_u 
      WHERE  insurance_file_cnt = @nInsuranceFileCnt 

      IF @@ERROR <> 0 
        GOTO error_label 

      DELETE FROM policy_fee 
      WHERE  insurance_file_cnt = @nInsuranceFileCnt 

      IF @@ERROR <> 0 
        GOTO error_label 

      DELETE FROM insurance_file_pt_log 
      WHERE  insurance_file_cnt = @nInsuranceFileCnt 

      IF @@ERROR <> 0 
        GOTO error_label 

      DELETE FROM insurance_file_pt_ri_usage 
      WHERE  insurance_file_cnt = @nInsuranceFileCnt 

      IF @@ERROR <> 0 
        GOTO error_label 

      DELETE FROM mta_insurance_file_link 
      WHERE  insurance_file_cnt = @nInsuranceFileCnt 

      IF @@ERROR <> 0 
        GOTO error_label 

      DELETE FROM insurance_file_clone_log 
      WHERE  insurance_file_cnt = @nInsuranceFileCnt 

      IF @@ERROR <> 0 
        GOTO error_label 

      DELETE FROM insurance_file_cloned_ri_usage 
      WHERE  insurance_file_cnt = @nInsuranceFileCnt 

      IF @@ERROR <> 0 
        GOTO error_label 

      DECLARE cursorrisk CURSOR FAST_FORWARD FOR 
        SELECT risk_cnt 
        FROM   insurance_file_risk_link 
        WHERE  insurance_file_cnt = @nInsuranceFileCnt 

      OPEN cursorrisk 

      FETCH next FROM cursorrisk INTO @RiskCnt 

      WHILE @@FETCH_STATUS = 0 
        BEGIN 
            IF (SELECT Count(*) 
                FROM   insurance_file_risk_link 
                WHERE  risk_cnt = @RiskCnt 
                GROUP  BY risk_cnt) = 1 
              BEGIN 
                  DELETE FROM insurance_file_risk_link 
                  WHERE  insurance_file_cnt = @nInsuranceFileCnt 
                         AND risk_cnt = @RiskCnt 

                  IF @@ERROR <> 0 
                    GOTO error_label 

                  DELETE tax_calculation 
                  WHERE  risk_cnt = @RiskCnt 

                  IF @@ERROR <> 0 
                    GOTO error_label 

                  DELETE accumulation_values 
                  WHERE  risk_cnt = @RiskCnt 

                  IF @@ERROR <> 0 
                    GOTO error_label 

                  DELETE peril 
                  WHERE  risk_cnt = @RiskCnt 

                  IF @@ERROR <> 0 
                    GOTO error_label 

                  DELETE rating_section 
                  WHERE  risk_cnt = @RiskCnt 

                  IF @@ERROR <> 0 
                    GOTO error_label 

                  IF (SELECT Count(r.risk_cnt) 
                      FROM   risk r 
                             JOIN risk r2 
                               ON r.risk_folder_cnt = r2.risk_folder_cnt 
                      WHERE  r.risk_cnt = @RiskCnt) = 1 
                    BEGIN 
                        SELECT @RiskFolderCnt = risk_folder_cnt 
                        FROM   risk 
                        WHERE  risk_cnt = @RiskCnt 

                        DELETE risk 
                        WHERE  risk_cnt = @RiskCnt 

                        IF @@ERROR <> 0 
                          GOTO error_label 

                        DELETE risk_folder 
                        WHERE  risk_folder_cnt = @RiskFolderCnt 

                        IF @@ERROR <> 0 
                          GOTO error_label 
                    END 
                  ELSE 
                    BEGIN 
                        DELETE risk 
                        WHERE  risk_cnt = @RiskCnt 

                        IF @@ERROR <> 0 
                          GOTO error_label 
                    END 
              END 
            ELSE 
              BEGIN 
                  DELETE FROM insurance_file_risk_link 
                  WHERE  insurance_file_cnt = @nInsuranceFileCnt 
                         AND risk_cnt = @RiskCnt 

                  IF @@ERROR <> 0 
                    GOTO error_label 
              END 

            FETCH next FROM cursorrisk INTO @RiskCnt 
        END 

      DELETE agent_commission 
      WHERE  insurance_file_cnt = @nInsuranceFileCnt 

      IF @@ERROR <> 0 
        GOTO error_label 

      DELETE FROM insurance_file_agent 
      WHERE  insurance_file_cnt = @nInsuranceFileCnt 

      IF @@ERROR <> 0 
        GOTO error_label 

      DELETE FROM policy_standard_wording 
      WHERE  insurance_file_cnt = @nInsuranceFileCnt 

      IF @@ERROR <> 0 
        GOTO error_label 

      DELETE FROM document_spooler 
      WHERE  insurance_file_cnt = @nInsuranceFileCnt 

      IF @@ERROR <> 0 
        GOTO error_label 

      DELETE FROM event_log 
      WHERE  insurance_file_cnt = @nInsuranceFileCnt 

      IF @@ERROR <> 0 
        GOTO error_label 

      DELETE FROM insurance_file_deferred_ri_usage 
      WHERE  insurance_file_cnt = @nInsuranceFileCnt 

      IF @@ERROR <> 0 
        GOTO error_label 

      DELETE FROM batch_renewal_job_runs 
      WHERE  insurance_file_cnt = @nInsuranceFileCnt 

      DELETE FROM renewal_report 
      WHERE  insurance_file_cnt = @nInsuranceFileCnt 

      DELETE FROM insurance_file_system 
      WHERE  insurance_file_cnt = @nInsuranceFileCnt 

      IF @@ERROR <> 0 
        GOTO error_label 

	  DELETE FROM mid_vehicle  
      WHERE  mid_policy_id IN (SELECT mid_policy_id FROM mid_policy  
      WHERE  insurance_file_cnt = @nInsuranceFileCnt ) 

      DELETE FROM mid_policy  
      WHERE  insurance_file_cnt = @nInsuranceFileCnt  
  
      IF @@ERROR <> 0  
        GOTO error_label 

      DELETE FROM insurance_file 
      WHERE  insurance_file_cnt = @nInsuranceFileCnt 

      IF @@ERROR <> 0 
        GOTO error_label 

      COMMIT TRANSACTION 

      GOTO end_label 

      ERROR_LABEL: 

      ROLLBACK TRANSACTION 

      SELECT @nReturnValue = -1 

      END_LABEL: 

      CLOSE cursorrisk 

      DEALLOCATE cursorrisk 

      RETURN @nReturnValue 
  END 

