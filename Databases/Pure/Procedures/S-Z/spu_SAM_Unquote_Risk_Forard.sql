SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Unquote_Risk_Forard'
GO

CREATE PROCEDURE spu_SAM_Unquote_Risk_Forard
	@risk_cnt INT
AS  
DECLARE @insurance_file_cnt INT,
		@risk_folder_cnt INT,
		@oos_mta_cancelled_policy_id INT,
		@oos_mta_base_policy_id INT,
		@oos_mta_future_policy_id INT

	Set @oos_mta_cancelled_policy_id = 0  
	Set @oos_mta_base_policy_id = 0
	Set @oos_mta_future_policy_id = 0

	Select @risk_folder_cnt = ISNULL(r.risk_folder_cnt, 0), 
			@insurance_file_cnt = ISNULL(ifrl.insurance_file_cnt, 0) From Risk r
				Inner Join insurance_file_risk_link ifrl ON ifrl.risk_cnt = r.risk_cnt
	Where ifrl.Risk_cnt = @risk_cnt

    Select @oos_mta_cancelled_policy_id = ISNull(cancelled_linked_insurance_file_cnt, 0),
			@oos_mta_base_policy_id = ISNull(insurance_file_cnt, 0)
					From mta_insurance_file_link with (nolock) 
						Where new_linked_insurance_file_cnt = @insurance_file_cnt
	
	If (@oos_mta_cancelled_policy_id > 0 AND @oos_mta_base_policy_id > 0)
	Begin
	 	DECLARE c_iFile CURSOR FAST_FORWARD FOR
			Select new_linked_insurance_file_cnt
						From mta_insurance_file_link with (nolock) 
							Where insurance_file_cnt = @oos_mta_base_policy_id 
									AND new_linked_insurance_file_cnt > @insurance_file_cnt
										AND cancelled_linked_insurance_file_cnt IS NOT NULL

 		OPEN c_iFile
    		FETCH NEXT FROM c_iFile INTO @oos_mta_future_policy_id
    		WHILE @@FETCH_STATUS = 0
    			BEGIN
	        		UPDATE risk SET risk_status_id = 4
							From risk r
									Inner Join insurance_file_risk_link ifrl ON r.risk_cnt = ifrl.risk_cnt
							Where ifrl.insurance_file_cnt = @oos_mta_future_policy_id
												AND r.risk_folder_cnt = @risk_folder_cnt
 													AND ifrl.status_flag <> 'U'
        			FETCH NEXT FROM c_iFile INTO @oos_mta_future_policy_id
    			END
		CLOSE c_iFile
    	DEALLOCATE c_iFile
	End 
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
