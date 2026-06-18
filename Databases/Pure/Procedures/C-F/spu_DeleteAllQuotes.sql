SET QUOTED_IDENTIFIER OFF 
GO 

SET ANSI_NULLS ON 
GO 

EXECUTE Ddldropprocedure 'spu_DeleteAllQuotes' 
GO

CREATE PROCEDURE spu_DeleteAllQuotes
(
	@strInsuranceFileCnts VARCHAR(MAX) 
)
AS 
/*
	Creation Date:	19 Sep 2018
	Created By:		George Harris
	Description:	Meant to replace the sp spu_DeleteQuote whereas this sp takes in a csv string of incurance_file_cnt's and will remove all records pertaining

*/
	BEGIN 

		DECLARE @nReturnValue  INT, 
				@RiskCnt       INT, 
				@RiskFolderCnt INT 

		SELECT @nReturnValue = 0 

		--Function returns the csv string as a table
		SELECT ID INTO #tmpInsCounts
		FROM [dbo].[sf_CSVToTable](@strInsuranceFileCnts)

		SELECT risk_cnt 
		INTO #tmpRisks
		FROM   insurance_file_risk_link 
		WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts)

		BEGIN TRY 

			BEGIN TRANSACTION UpdateQuotedDate

			DELETE ri_arrangement_line_broker_participants 
			FROM   ri_arrangement_line_broker_participants rbp 
				INNER JOIN ri_arrangement_line ral ON rbp.ri_arrangement_line_id = ral.ri_arrangement_line_id 
				INNER JOIN ri_arrangement rra ON ral.ri_arrangement_id = rra.ri_arrangement_id 
				INNER JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt = rra.risk_cnt 
			WHERE  ifrl.insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts) 
				AND ifrl.status_flag IN ( 'C', 'D' ) 

			DELETE ri_arrangement_line 
			FROM   ri_arrangement_line ral 
				INNER JOIN ri_arrangement rra ON ral.ri_arrangement_id = rra.ri_arrangement_id 
				INNER JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt = rra.risk_cnt 
			WHERE  ifrl.insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts)
				AND ifrl.status_flag IN ( 'C', 'D' ) 

			DELETE ri_arrangement 
			FROM   ri_arrangement rra 
			INNER JOIN insurance_file_risk_link ifrl ON rra.risk_cnt = ifrl.risk_cnt 
			WHERE  ifrl.insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts) 
				AND ifrl.status_flag IN ( 'C', 'D' ) 

			DELETE FROM tax_calculation 
			WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts)

			DELETE FROM policy_fee_u 
			WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts)

			DELETE FROM policy_fee 
			WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts) 

			DELETE FROM insurance_file_pt_log 
			WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts)

			DELETE FROM insurance_file_pt_ri_usage 
			WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts)

			DELETE FROM mta_insurance_file_link 
			WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts)

			DELETE FROM insurance_file_clone_log 
			WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts) 

			DELETE FROM insurance_file_cloned_ri_usage 
			WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts) 

			IF (
					SELECT Count(1) 
					FROM   insurance_file_risk_link 
					WHERE  risk_cnt in (SELECT risk_cnt FROM #tmpRisks)
				)= 1
		 
			 BEGIN 

				DELETE FROM insurance_file_risk_link 
				WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts)
					AND risk_cnt IN (SELECT risk_cnt FROM #tmpRisks)

				DELETE tax_calculation 
				WHERE  risk_cnt IN (SELECT risk_cnt FROM #tmpRisks)

				DELETE accumulation_values 
				WHERE  risk_cnt IN (SELECT risk_cnt FROM #tmpRisks)

				DELETE peril 
				WHERE  risk_cnt IN (SELECT risk_cnt FROM #tmpRisks)

				DELETE rating_section 
				WHERE  risk_cnt IN (SELECT risk_cnt FROM #tmpRisks)

				IF (
						SELECT Count(r.risk_cnt) 
						FROM   risk r 
						INNER JOIN risk r2 ON r.risk_folder_cnt = r2.risk_folder_cnt 
						WHERE  r.risk_cnt IN (SELECT risk_cnt FROM #tmpRisks)
					) = 1 
				BEGIN 

					SELECT @RiskFolderCnt = risk_folder_cnt 
					FROM   risk 
					WHERE  risk_cnt IN (SELECT risk_cnt FROM #tmpRisks) 

					DELETE risk 
					WHERE  risk_cnt IN (SELECT risk_cnt FROM #tmpRisks)

					DELETE risk_folder 
					WHERE  risk_folder_cnt = @RiskFolderCnt 

				END 
				ELSE 
				BEGIN 
					DELETE risk 
					WHERE  risk_cnt IN (SELECT risk_cnt FROM #tmpRisks)
				END 
			END 
			ELSE 
				BEGIN 
					DELETE FROM insurance_file_risk_link 
					WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts) 
							AND risk_cnt IN (SELECT risk_cnt FROM #tmpRisks)
				END  

			DELETE agent_commission 
			WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts) 

			DELETE FROM insurance_file_agent 
			WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts) 

			DELETE FROM policy_standard_wording 
			WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts) 

			DELETE FROM document_spooler 
			WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts) 

			DELETE FROM event_log 
			WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts) 

			DELETE FROM insurance_file_deferred_ri_usage 
			WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts) 

			DELETE FROM batch_renewal_job_runs 
			WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts) 

			DELETE FROM renewal_report 
			WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts) 

			DELETE FROM insurance_file_system 
			WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts) 

			DELETE FROM insurance_file 
			WHERE  insurance_file_cnt IN (SELECT ID FROM #tmpInsCounts) 

			COMMIT TRANSACTION UpdateQuotedDate
			RETURN @nReturnValue 

      END TRY

	  BEGIN CATCH
		ROLLBACK TRANSACTION UpdateQuotedDate
		SELECT @nReturnValue = -1 
	  END CATCH

  END 

