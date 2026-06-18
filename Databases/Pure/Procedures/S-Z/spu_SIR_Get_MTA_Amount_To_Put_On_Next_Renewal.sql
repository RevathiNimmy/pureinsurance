SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Get_MTA_Amount_To_Put_On_Next_Renewal'
GO

CREATE PROCEDURE spu_SIR_Get_MTA_Amount_To_Put_On_Next_Renewal

	@insurance_file_cnt int,
	@amount_to_finance money output 
AS

BEGIN

	DECLARE @product_is_true_monthly int
	DECLARE @is_broker_business int
	DECLARE @outstanding_mta_amount_to_be_put_on_next_instalment money
	DECLARE @insurance_folder_cnt int

	-- determine whether this policy is based on a true monthly policy product
	SELECT 
		@product_is_true_monthly = is_true_monthly_policy,
		@insurance_folder_cnt = insurance_folder_cnt
	FROM insurance_file ifile
		INNER JOIN product p ON
			ifile.product_id  = p.product_id
	WHERE insurance_file_cnt = @insurance_file_cnt

	-- if this product is a true monthly policy
	IF ISNULL(@product_is_true_monthly,0) = 1
	BEGIN 

		-- determine if the transaction are stored against an agent
		SELECT  @is_broker_business = Count(*)  
		FROM    insurance_file i  
			JOIN    business_type bt ON bt.business_type_id = i.business_type_id  
			JOIN    party_agent pa ON pa.party_cnt = i.lead_agent_cnt  
		WHERE   pa.party_agent_type_id = 1 -- Broker Agent  
		AND     bt.business_type_id <> 1 -- Not Direct  
		AND     i.insurance_file_cnt = @insurance_file_cnt  
	  
		-- if the transactions are stored against an agent
		IF @is_broker_business > 0  
			BEGIN
				-- get the outstanding amount to be put on next renewal
				SELECT @outstanding_mta_amount_to_be_put_on_next_instalment = SUM(td.outstanding_amount)
				FROM Insurance_file i  
					INNER JOIN account Ac ON 
						ac.account_key = i.lead_agent_cnt -- Link to lead_agent_cnt  
					INNER JOIN document doc ON 
						doc.insurance_file_cnt = i.insurance_file_cnt  
					INNER JOIN transdetail td ON 
						td.document_id = doc.document_id  
			                    AND td.account_id = ac.account_id  
					LEFT JOIN pftransaction_id pft ON 
						pft.pftransaction_id = td.transdetail_id
				
				WHERE i.insurance_folder_cnt = @insurance_folder_cnt
				AND i.put_on_next_instalment_renewal = 1
				AND pftransaction_id IS NULL
				AND td.spare<>'comm'
			END
		ELSE
		-- this is direct business so use the transactions stored against the insured
			BEGIN		
				SELECT @outstanding_mta_amount_to_be_put_on_next_instalment = SUM(td.outstanding_amount)
				FROM Insurance_file i
					INNER JOIN account Ac ON
						Ac.account_key = i.insured_cnt -- Link to insured_cnt  
					INNER JOIN document doc ON 
						doc.insurance_file_cnt = i.insurance_file_cnt  
					INNER JOIN transDetail td ON 
						td.document_id = doc.document_id  
		    		            AND td.account_id = ac.account_id  
					LEFT JOIN pftransaction_id pft ON 
						pft.pftransaction_id = td.transdetail_id
				
				WHERE i.insurance_folder_cnt = @insurance_folder_cnt
				AND i.put_on_next_instalment_renewal = 1
				AND pftransaction_id IS NULL
			END
	
		-- ensure that the amount we have is valid
		SELECT @outstanding_mta_amount_to_be_put_on_next_instalment  = ISNULL(@outstanding_mta_amount_to_be_put_on_next_instalment,0) 
	
	END
	ELSE
	BEGIN
		-- if this is not a policy based on a true monthly product 
		-- default the amount to zero
		SELECT @outstanding_mta_amount_to_be_put_on_next_instalment = 0
	END 

	-- return the amount to finance
	SET @amount_to_finance = @outstanding_mta_amount_to_be_put_on_next_instalment
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
