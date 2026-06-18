SET QUOTED_IDENTIFIER OFF 
Go

EXECUTE DDLDropProcedure 'spu_sir_agent_commission_rev'
GO

CREATE PROCEDURE spu_sir_agent_commission_rev
    @OldInsuranceFileCnt int,
    @NewInsuranceFileCnt int
AS

	DECLARE @agent_commission_cnt INT,
		@original_agent_commission_cnt INT,
        @tax_currency_amount numeric(19, 4),
        @Commission_value numeric(19, 4),
        @is_value tinyint,
        @Lead_Commission_band int,		
        @sub_commission_band int,
		@Product_id int,
        @Risk_Type_id int,
        @premium numeric(19, 4),
        @annual_premium numeric(19, 4),
        @xBaseRate numeric(19, 8),
        @xAccountRate numeric(19, 8),
        @insuranceFolderCnt INT,
		@OriginalInsuranceFileCnt INT,
		@tax_group_id INT,
		@nBusiness_type INT,
		@peril_type_id INT,@class_of_business_id INT

		-- Read the commission display level option (5264: Display Commission at Commission Band Level)
	DECLARE @display_band_level BIT = 0

	SELECT @display_band_level = ISNULL(
		(SELECT CAST(so.value AS BIT) 
		 FROM system_options so
		 INNER JOIN insurance_file ifile ON ifile.insurance_file_cnt = @NewInsuranceFileCnt
		 INNER JOIN source s ON s.source_id = ifile.source_id
		 WHERE so.branch_id = s.source_id 
		 AND so.option_number = 5264
		 AND so.value = '1'), 0)
	EXEC spu_sir_agent_commission_del @Insurance_file_cnt=@NewInsuranceFileCnt
	
	Select @nBusiness_type = business_type_id  FROM insurance_file WHERE insurance_file_cnt =  @NewInsuranceFileCnt
	SELECT @insuranceFolderCnt = insurance_folder_cnt FROM insurance_file WHERE insurance_file_cnt = @OldInsuranceFileCnt

  IF @nbusiness_type <> 1 BEGIN

  IF @display_band_level <> 1 BEGIN
	
	-- Declare the cursor to get the premiums for each commission band
	DECLARE Lead_Peril_Cursor Cursor FAST_FORWARD For
		SELECT lead_commission_band,
		r.Risk_type_id,
		CONVERT(numeric(19, 4), SUM(this_Premium)),
		CONVERT(numeric(19, 4), SUM(annual_Premium)),
		p.peril_type_id,
		p.class_of_business_id
		FROM Peril p
		Inner Join Risk r On p.risk_cnt = r.risk_cnt
		Inner Join insurance_file_risk_link ifr On r.risk_cnt = ifr.risk_cnt
		WHERE ifr.Insurance_file_cnt = @NewInsuranceFileCnt
		AND ifr.status_flag NOT IN ('U','R')
		AND ISNULL(p.is_levy_tax,0)=0
		AND r.is_risk_selected=1
		GROUP BY lead_commission_band, r.Risk_type_id, p.peril_type_id, p.class_of_business_id
		HAVING SUM(this_Premium) <> 0
	
	DECLARE Sub_Peril_Cursor Cursor FAST_FORWARD For
		SELECT sub_commission_band,
		r.Risk_type_id,
		CONVERT(numeric(19, 4), SUM(this_Premium)),
		CONVERT(numeric(19, 4), SUM(annual_Premium)),
		p.peril_type_id,
		p.class_of_business_id
		FROM Peril p
		Inner Join Risk r On p.risk_cnt = r.risk_cnt
		Inner Join insurance_file_risk_link ifr On r.risk_cnt = ifr.risk_cnt
		WHERE ifr.Insurance_file_cnt = @NewInsuranceFileCnt
		AND ifr.status_flag NOT IN ('U','R')
		AND ISNULL(p.is_levy_tax,0)=0
		AND r.is_risk_selected=1
		GROUP BY sub_commission_band, r.Risk_type_id, p.peril_type_id, p.class_of_business_id
		HAVING SUM(this_Premium) <> 0

	-- Process for lead_commission_band
	Open Lead_Peril_Cursor
	Fetch Next FROM Lead_Peril_Cursor Into @Lead_Commission_band, @Risk_Type_id, @Premium, @annual_premium, @peril_type_id, @class_of_business_id

	While @@Fetch_Status = 0 Begin
		Select @OriginalInsuranceFileCnt = 0
		-- starting FROM passed original iFile identify comm % for risk type if any
		Select @OriginalInsuranceFileCnt = ISNULL(MAX(MIFL.original_linked_insurance_file_cnt), @OldInsuranceFileCnt)
			FROM mta_insurance_file_link mifl
				Inner Join insurance_file ifi On ifi.insurance_file_cnt = mifl.insurance_file_cnt
				Inner Join Agent_Commission ac On ac.insurance_file_cnt = mifl.original_linked_insurance_file_cnt AND ifi.lead_agent_cnt = ac.party_cnt
				Inner Join insurance_file ifi2 On ifi2.insurance_file_cnt = mifl.original_linked_insurance_file_cnt
			WHERE ifi.insurance_folder_cnt = @insuranceFolderCnt AND ifi2.insurance_file_cnt <= @OldInsuranceFileCnt
				AND ac.risk_type_id = @Risk_Type_id AND ac.commission_band_id = @Lead_Commission_band
				AND peril_type_id=@peril_type_id
					AND class_of_business_id=@class_of_business_id
				AND ifi2.cover_start_date <= (Select cover_start_date FROM insurance_file with(nolock) WHERE insurance_file_cnt=@NewInsuranceFileCnt)

		SELECT @Commission_value = 0
		SELECT @Commission_value =
			Case is_value
				When 1 Then commission_value * -1
			Else (@premium * commission_percentage) / 100.00 End,
				@xBaseRate =
					Case commission_value
						When 0 Then 1
						Else base_commission_value / commission_value End,
				@xAccountRate =
					Case commission_value
						When 0 Then 1
						Else account_commission_value / commission_value End
		FROM    agent_commission
		WHERE   insurance_file_cnt = @OriginalInsuranceFileCnt
			AND commission_band_id = @Lead_Commission_band
			AND risk_type_id = @Risk_Type_id
			AND peril_type_id=@peril_type_id
			AND class_of_business_id=@class_of_business_id

		If @xAccountRate = 0
			Set @xAccountRate = 1

		If @xBaseRate = 0
			Set @xBaseRate = 1

		If @Commission_value <> 0 BEGIN

			INSERT INTO agent_commission (
				insurance_file_cnt,	is_lead_agent, party_cnt, risk_type_id,	commission_band_id,	premium, commission_percentage,	commission_value,
				is_amended, account_currency_id,
				account_commission_value, 
				base_currency_id,
				base_commission_value,
				tax_group_id, tax_amount, tax_account_amount, tax_base_amount, override_reason,
				calculated_commission_value,
				maximum_rate, is_value,	is_tax_amended,	peril_type_id, class_of_business_id)
			SELECT  @NewInsuranceFileCnt, is_lead_agent, party_cnt,	risk_type_id, commission_band_id, @Premium,	commission_percentage,
				Case is_value When 1 Then commission_value * -1	Else (@premium * commission_percentage) / 100.00 End,
				is_amended,	account_currency_id,
				Case is_value When 1 Then account_commission_value * -1	Else ((@premium * commission_percentage) / 100.00) * @xAccountRate End,  -- establish xRate here only as was applied originally
				base_currency_id,
				Case is_value When 1 Then base_commission_value * -1 Else ((@premium * commission_percentage) / 100.00)	* @xBaseRate End,  -- establish xRate here only as was applied originally
				tax_group_id, tax_amount * -1, tax_account_amount * -1,	tax_base_amount * -1, override_reason,
				Case is_value When 1 Then commission_value * -1	Else (@premium * commission_percentage) / 100.00 End,
				maximum_rate, is_value,	is_tax_amended, peril_type_id, class_of_business_id
			FROM    agent_commission
			WHERE   insurance_file_cnt = @OriginalInsuranceFileCnt
				AND commission_band_id = @Lead_Commission_band
				AND risk_type_id = @Risk_Type_id
				AND peril_type_id=@peril_type_id
				AND class_of_business_id=@class_of_business_id
				

			SELECT @agent_commission_cnt=@@IDENTITY

			SELECT @original_agent_commission_cnt = 0,
					@tax_group_id = 0

			SELECT @original_agent_commission_cnt = agent_commission_cnt,
				@tax_group_id = tax_group_id
			FROM Agent_Commission
			WHERE   insurance_file_cnt = @OriginalInsuranceFileCnt
				AND commission_band_id = @Lead_Commission_band
				AND risk_type_id = @Risk_Type_id

			SELECT @Commission_value = ISNULL(commission_value, 0) FROM Agent_Commission
			WHERE agent_commission_cnt = @agent_commission_cnt

			IF NOT EXISTS (SELECT NULL FROM tax_calculation WHERE agent_commission_cnt = @original_agent_commission_cnt)
			AND @tax_group_id > 0
			BEGIN
				-- Identify previous iFile WHERE comm tax was charged for the risk type
				Select @original_agent_commission_cnt = ac.agent_commission_cnt
				FROM Agent_Commission ac
				Inner Join insurance_file ifi On ifi.insurance_file_cnt = ac.insurance_file_cnt
				Inner Join Tax_Calculation tc On tc.insurance_file_cnt = ifi.insurance_file_cnt AND tc.agent_commission_cnt = ac.agent_commission_cnt
				WHERE ifi.insurance_folder_cnt = @insuranceFolderCnt
				AND ifi.insurance_file_cnt < @OriginalInsuranceFileCnt
				AND ac.commission_band_id = @Lead_Commission_band
				AND ac.risk_type_id = @Risk_Type_id
				AND ac.peril_type_id=@peril_type_id
				AND ac.class_of_business_id=@class_of_business_id
			END

			INSERT INTO tax_calculation (
				tax_band_id, premium, percentage,
				value,
				is_value, is_manually_changed, Calc_Basis, Basis_Value, currency_id, allow_tax_credit, country_id, state_id,
				class_of_business_id, tax_group_id, sequence, insurance_file_cnt, transtype, agent_commission_cnt,
				is_not_applied_to_client, include_tax_in_instalments, spread_tax_across_instalments, tax_band_rate_id, is_suspended)
			SELECT
				tax_band_id, @Commission_value, percentage,
				Case is_value When 1 Then value * -1 Else (@Commission_value * percentage) / 100.00 End,
				is_value, is_manually_changed, Calc_Basis, Basis_Value, currency_id, allow_tax_credit, country_id, state_id,
				class_of_business_id, tax_group_id, sequence, @NewInsuranceFileCnt, transtype, @agent_commission_cnt,
				is_not_applied_to_client, include_tax_in_instalments, spread_tax_across_instalments, tax_band_rate_id, is_suspended
			FROM    tax_calculation
			WHERE   agent_commission_cnt = @original_agent_commission_cnt

			SELECT @tax_currency_amount = value
				FROM Tax_Calculation WHERE agent_commission_cnt = @agent_commission_cnt

			/* Revise Tax amounts */
			UPDATE Agent_Commission
			SET tax_amount = @tax_currency_amount,
				tax_account_amount = ROUND(@tax_currency_amount * @xAccountRate, 2),
				tax_base_amount = @tax_currency_amount * @xBaseRate
			WHERE agent_commission_cnt=@agent_commission_cnt
		END

		Fetch Next FROM Lead_Peril_Cursor Into @Lead_Commission_band, @Risk_Type_id, @Premium, @annual_premium, @peril_type_id,@class_of_business_id

	END -- end of cursor

	-- Close and Deallocate
	Close Lead_Peril_Cursor
	Deallocate Lead_Peril_Cursor

	-- Process for sub_commission_band
	Open Sub_Peril_Cursor
	Fetch Next FROM Sub_Peril_Cursor Into @Sub_Commission_band, @Risk_Type_id, @Premium, @annual_premium, @peril_type_id,@class_of_business_id

	While @@Fetch_Status = 0 Begin
		Select @OriginalInsuranceFileCnt = 0
		-- starting FROM passed original iFile identify comm % for risk type if any
		Select @OriginalInsuranceFileCnt = ISNULL(MAX(MIFL.original_linked_insurance_file_cnt), @OldInsuranceFileCnt)
			FROM mta_insurance_file_link mifl
				Inner Join insurance_file ifi On ifi.insurance_file_cnt = mifl.insurance_file_cnt
				Inner Join Agent_Commission ac On ac.insurance_file_cnt = mifl.original_linked_insurance_file_cnt AND ifi.lead_agent_cnt = ac.party_cnt
				Inner Join insurance_file ifi2 On ifi2.insurance_file_cnt = mifl.original_linked_insurance_file_cnt
			WHERE ifi.insurance_folder_cnt = @insuranceFolderCnt AND ifi2.insurance_file_cnt <= @OldInsuranceFileCnt
				AND ac.risk_type_id = @Risk_Type_id AND ac.commission_band_id = @Sub_Commission_band
				AND peril_type_id=@peril_type_id
					AND class_of_business_id=@class_of_business_id
				AND ifi2.cover_start_date <= (Select cover_start_date FROM insurance_file with(nolock) WHERE insurance_file_cnt=@NewInsuranceFileCnt)

		SELECT @Commission_value = 0
		SELECT @Commission_value =
			Case is_value
				When 1 Then commission_value * -1
			Else (@premium * commission_percentage) / 100.00 End,
				@xBaseRate =
					Case commission_value
						When 0 Then 1
						Else base_commission_value / commission_value End,
				@xAccountRate =
					Case commission_value
						When 0 Then 1
						Else account_commission_value / commission_value End
		FROM    agent_commission
		WHERE   insurance_file_cnt = @OriginalInsuranceFileCnt
			AND commission_band_id = @Sub_Commission_band
			AND risk_type_id = @Risk_Type_id
			AND peril_type_id=@peril_type_id
			AND class_of_business_id=@class_of_business_id

		If @xAccountRate = 0
			Set @xAccountRate = 1

		If @xBaseRate = 0
			Set @xBaseRate = 1

		If @Commission_value <> 0 BEGIN
			INSERT INTO agent_commission (
				insurance_file_cnt, is_lead_agent, party_cnt, risk_type_id, commission_band_id, premium, commission_percentage,
				commission_value,
				is_amended, account_currency_id,
				account_commission_value,
				base_currency_id,
				base_commission_value,
				tax_group_id, tax_amount, tax_account_amount, tax_base_amount, override_reason,
				calculated_commission_value,
				maximum_rate, is_value, is_tax_amended, peril_type_id, class_of_business_id)
			SELECT
				@NewInsuranceFileCnt, is_lead_agent, party_cnt, risk_type_id, commission_band_id, @Premium, commission_percentage,
				Case is_value When 1 Then commission_value * -1 Else (@premium * commission_percentage) / 100.00 End,
				is_amended, account_currency_id,
				Case is_value When 1 Then account_commission_value * -1 Else ((@premium * commission_percentage) / 100.00) * @xAccountRate End,  -- establish xRate here only as was applied originally
				base_currency_id,
				Case is_value When 1 Then base_commission_value * -1 Else ((@premium * commission_percentage) / 100.00) * @xBaseRate End,  -- establish xRate here only as was applied originally
				tax_group_id, tax_amount * -1, tax_account_amount * -1, tax_base_amount * -1, override_reason,
				Case is_value When 1 Then commission_value * -1 Else (@premium * commission_percentage) / 100.00 End,
				maximum_rate, is_value, is_tax_amended,peril_type_id,class_of_business_id
			FROM    agent_commission
			WHERE   insurance_file_cnt = @OriginalInsuranceFileCnt
				AND commission_band_id = @Sub_Commission_band
				AND risk_type_id = @Risk_Type_id
				AND peril_type_id=@peril_type_id
				AND class_of_business_id=@class_of_business_id


			SELECT @agent_commission_cnt=@@IDENTITY

			SELECT @original_agent_commission_cnt = 0,
					@tax_group_id = 0

			SELECT @original_agent_commission_cnt = agent_commission_cnt,
				@tax_group_id = tax_group_id
			FROM Agent_Commission
			WHERE   insurance_file_cnt = @OriginalInsuranceFileCnt
				AND commission_band_id = @Sub_Commission_band
				AND risk_type_id = @Risk_Type_id

			SELECT @Commission_value = ISNULL(commission_value, 0) FROM Agent_Commission
			WHERE agent_commission_cnt = @agent_commission_cnt

			IF NOT EXISTS (SELECT NULL FROM tax_calculation WHERE agent_commission_cnt = @original_agent_commission_cnt)
			AND @tax_group_id > 0
			BEGIN
				-- Identify previous iFile WHERE comm tax was charged for the risk type
				Select @original_agent_commission_cnt = ac.agent_commission_cnt
				FROM Agent_Commission ac
				Inner Join insurance_file ifi On ifi.insurance_file_cnt = ac.insurance_file_cnt
				Inner Join Tax_Calculation tc On tc.insurance_file_cnt = ifi.insurance_file_cnt AND tc.agent_commission_cnt = ac.agent_commission_cnt
				WHERE ifi.insurance_folder_cnt = @insuranceFolderCnt
				AND ifi.insurance_file_cnt < @OriginalInsuranceFileCnt
				AND ac.commission_band_id = @Sub_Commission_band
				AND ac.risk_type_id = @Risk_Type_id
				AND ac.peril_type_id=@peril_type_id
				AND ac.class_of_business_id=@class_of_business_id
			END

			INSERT INTO tax_calculation (
				tax_band_id, premium, percentage,
				value,
				is_value, is_manually_changed, Calc_Basis, Basis_Value, currency_id, allow_tax_credit, country_id, state_id,
				class_of_business_id, tax_group_id, sequence, insurance_file_cnt, transtype, agent_commission_cnt,
				is_not_applied_to_client, include_tax_in_instalments, spread_tax_across_instalments, tax_band_rate_id, is_suspended)
			SELECT
				tax_band_id, @Commission_value, percentage,
				Case is_value When 1 Then value * -1 Else (@Commission_value * percentage) / 100.00 End,
				is_value, is_manually_changed, Calc_Basis, Basis_Value, currency_id, allow_tax_credit, country_id, state_id,
				class_of_business_id, tax_group_id, sequence, @NewInsuranceFileCnt, transtype, @agent_commission_cnt,
				is_not_applied_to_client, include_tax_in_instalments, spread_tax_across_instalments, tax_band_rate_id, is_suspended
			FROM tax_calculation
			WHERE agent_commission_cnt = @original_agent_commission_cnt

			SELECT @tax_currency_amount = value
				FROM Tax_Calculation WHERE agent_commission_cnt = @agent_commission_cnt

			/* Revise Tax amounts */
			UPDATE  Agent_Commission
				SET     tax_amount = @tax_currency_amount,
				tax_account_amount = ROUND(@tax_currency_amount * @xAccountRate, 2),
				tax_base_amount = @tax_currency_amount * @xBaseRate
			WHERE   agent_commission_cnt=@agent_commission_cnt
		END

		Fetch Next FROM Sub_Peril_Cursor Into @Sub_Commission_band, @Risk_Type_id, @Premium, @annual_premium, @peril_type_id,@class_of_business_id

	END -- end of cursor

	-- Close and Deallocate
	Close Sub_Peril_Cursor
	Deallocate Sub_Peril_Cursor

	END

	ELSE
		 BEGIN
	
	-- Declare the cursor to get the premiums for each commission band
	DECLARE Lead_Peril_Cursor Cursor FAST_FORWARD For
		SELECT lead_commission_band,
		r.Risk_type_id,
		CONVERT(numeric(19, 4), SUM(this_Premium)),
		CONVERT(numeric(19, 4), SUM(annual_Premium))
		FROM Peril p
		Inner Join Risk r On p.risk_cnt = r.risk_cnt
		Inner Join insurance_file_risk_link ifr On r.risk_cnt = ifr.risk_cnt
		WHERE ifr.Insurance_file_cnt = @NewInsuranceFileCnt
		AND ifr.status_flag NOT IN ('U','R')
		AND ISNULL(p.is_levy_tax,0)=0
		AND r.is_risk_selected=1
		GROUP BY lead_commission_band, r.Risk_type_id
		HAVING SUM(this_Premium) <> 0
	
	DECLARE Sub_Peril_Cursor Cursor FAST_FORWARD For
		SELECT sub_commission_band,
		r.Risk_type_id,
		CONVERT(numeric(19, 4), SUM(this_Premium)),
		CONVERT(numeric(19, 4), SUM(annual_Premium))
		FROM Peril p
		Inner Join Risk r On p.risk_cnt = r.risk_cnt
		Inner Join insurance_file_risk_link ifr On r.risk_cnt = ifr.risk_cnt
		WHERE ifr.Insurance_file_cnt = @NewInsuranceFileCnt
		AND ifr.status_flag NOT IN ('U','R')
		AND ISNULL(p.is_levy_tax,0)=0
		AND r.is_risk_selected=1
		GROUP BY sub_commission_band, r.Risk_type_id
		HAVING SUM(this_Premium) <> 0

	-- Process for lead_commission_band
	Open Lead_Peril_Cursor
	Fetch Next FROM Lead_Peril_Cursor Into @Lead_Commission_band, @Risk_Type_id, @Premium, @annual_premium

	While @@Fetch_Status = 0 Begin
		Select @OriginalInsuranceFileCnt = 0
		-- starting FROM passed original iFile identify comm % for risk type if any
		Select @OriginalInsuranceFileCnt = ISNULL(MAX(MIFL.original_linked_insurance_file_cnt), @OldInsuranceFileCnt)
			FROM mta_insurance_file_link mifl
				Inner Join insurance_file ifi On ifi.insurance_file_cnt = mifl.insurance_file_cnt
				Inner Join Agent_Commission ac On ac.insurance_file_cnt = mifl.original_linked_insurance_file_cnt AND ifi.lead_agent_cnt = ac.party_cnt
				Inner Join insurance_file ifi2 On ifi2.insurance_file_cnt = mifl.original_linked_insurance_file_cnt
			WHERE ifi.insurance_folder_cnt = @insuranceFolderCnt AND ifi2.insurance_file_cnt <= @OldInsuranceFileCnt
				AND ac.risk_type_id = @Risk_Type_id AND ac.commission_band_id = @Lead_Commission_band
				
				AND ifi2.cover_start_date <= (Select cover_start_date FROM insurance_file with(nolock) WHERE insurance_file_cnt=@NewInsuranceFileCnt)

		SELECT @Commission_value = 0
		SELECT @Commission_value =
			Case is_value
				When 1 Then commission_value * -1
			Else (@premium * commission_percentage) / 100.00 End,
				@xBaseRate =
					Case commission_value
						When 0 Then 1
						Else base_commission_value / commission_value End,
				@xAccountRate =
					Case commission_value
						When 0 Then 1
						Else account_commission_value / commission_value End
		FROM    agent_commission
		WHERE   insurance_file_cnt = @OriginalInsuranceFileCnt
			AND commission_band_id = @Lead_Commission_band
			AND risk_type_id = @Risk_Type_id
			

		If @xAccountRate = 0
			Set @xAccountRate = 1

		If @xBaseRate = 0
			Set @xBaseRate = 1

		If @Commission_value <> 0 BEGIN

			INSERT INTO agent_commission (
				insurance_file_cnt,	is_lead_agent, party_cnt, risk_type_id,	commission_band_id,	premium, commission_percentage,	commission_value,
				is_amended, account_currency_id,
				account_commission_value, 
				base_currency_id,
				base_commission_value,
				tax_group_id, tax_amount, tax_account_amount, tax_base_amount, override_reason,
				calculated_commission_value,
				maximum_rate, is_value,	is_tax_amended)
			SELECT  @NewInsuranceFileCnt, is_lead_agent, party_cnt,	risk_type_id, commission_band_id, @Premium,	commission_percentage,
				Case is_value When 1 Then commission_value * -1	Else (@premium * commission_percentage) / 100.00 End,
				is_amended,	account_currency_id,
				Case is_value When 1 Then account_commission_value * -1	Else ((@premium * commission_percentage) / 100.00) * @xAccountRate End,  -- establish xRate here only as was applied originally
				base_currency_id,
				Case is_value When 1 Then base_commission_value * -1 Else ((@premium * commission_percentage) / 100.00)	* @xBaseRate End,  -- establish xRate here only as was applied originally
				tax_group_id, tax_amount * -1, tax_account_amount * -1,	tax_base_amount * -1, override_reason,
				Case is_value When 1 Then commission_value * -1	Else (@premium * commission_percentage) / 100.00 End,
				maximum_rate, is_value,	is_tax_amended
			FROM    agent_commission
			WHERE   insurance_file_cnt = @OriginalInsuranceFileCnt
				AND commission_band_id = @Lead_Commission_band
				AND risk_type_id = @Risk_Type_id
				

			SELECT @agent_commission_cnt=@@IDENTITY

			SELECT @original_agent_commission_cnt = 0,
					@tax_group_id = 0

			SELECT @original_agent_commission_cnt = agent_commission_cnt,
				@tax_group_id = tax_group_id
			FROM Agent_Commission
			WHERE   insurance_file_cnt = @OriginalInsuranceFileCnt
				AND commission_band_id = @Lead_Commission_band
				AND risk_type_id = @Risk_Type_id

			SELECT @Commission_value = ISNULL(commission_value, 0) FROM Agent_Commission
			WHERE agent_commission_cnt = @agent_commission_cnt

			IF NOT EXISTS (SELECT NULL FROM tax_calculation WHERE agent_commission_cnt = @original_agent_commission_cnt)
			AND @tax_group_id > 0
			BEGIN
				-- Identify previous iFile WHERE comm tax was charged for the risk type
				Select @original_agent_commission_cnt = ac.agent_commission_cnt
				FROM Agent_Commission ac
				Inner Join insurance_file ifi On ifi.insurance_file_cnt = ac.insurance_file_cnt
				Inner Join Tax_Calculation tc On tc.insurance_file_cnt = ifi.insurance_file_cnt AND tc.agent_commission_cnt = ac.agent_commission_cnt
				WHERE ifi.insurance_folder_cnt = @insuranceFolderCnt
				AND ifi.insurance_file_cnt < @OriginalInsuranceFileCnt
				AND ac.commission_band_id = @Lead_Commission_band
				AND ac.risk_type_id = @Risk_Type_id
				
			END

			INSERT INTO tax_calculation (
				tax_band_id, premium, percentage,
				value,
				is_value, is_manually_changed, Calc_Basis, Basis_Value, currency_id, allow_tax_credit, country_id, state_id,
				class_of_business_id, tax_group_id, sequence, insurance_file_cnt, transtype, agent_commission_cnt,
				is_not_applied_to_client, include_tax_in_instalments, spread_tax_across_instalments, tax_band_rate_id, is_suspended)
			SELECT
				tax_band_id, @Commission_value, percentage,
				Case is_value When 1 Then value * -1 Else (@Commission_value * percentage) / 100.00 End,
				is_value, is_manually_changed, Calc_Basis, Basis_Value, currency_id, allow_tax_credit, country_id, state_id,
				class_of_business_id, tax_group_id, sequence, @NewInsuranceFileCnt, transtype, @agent_commission_cnt,
				is_not_applied_to_client, include_tax_in_instalments, spread_tax_across_instalments, tax_band_rate_id, is_suspended
			FROM    tax_calculation
			WHERE   agent_commission_cnt = @original_agent_commission_cnt

			SELECT @tax_currency_amount = value
				FROM Tax_Calculation WHERE agent_commission_cnt = @agent_commission_cnt

			/* Revise Tax amounts */
			UPDATE Agent_Commission
			SET tax_amount = @tax_currency_amount,
				tax_account_amount = ROUND(@tax_currency_amount * @xAccountRate, 2),
				tax_base_amount = @tax_currency_amount * @xBaseRate
			WHERE agent_commission_cnt=@agent_commission_cnt
		END

		Fetch Next FROM Lead_Peril_Cursor Into @Lead_Commission_band, @Risk_Type_id, @Premium, @annual_premium

END
	-- Close and Deallocate
	Close Lead_Peril_Cursor
	Deallocate Lead_Peril_Cursor

	-- Process for sub_commission_band
	Open Sub_Peril_Cursor
	Fetch Next FROM Sub_Peril_Cursor Into @Sub_Commission_band, @Risk_Type_id, @Premium, @annual_premium

	While @@Fetch_Status = 0 Begin
		Select @OriginalInsuranceFileCnt = 0
		-- starting FROM passed original iFile identify comm % for risk type if any
		Select @OriginalInsuranceFileCnt = ISNULL(MAX(MIFL.original_linked_insurance_file_cnt), @OldInsuranceFileCnt)
			FROM mta_insurance_file_link mifl
				Inner Join insurance_file ifi On ifi.insurance_file_cnt = mifl.insurance_file_cnt
				Inner Join Agent_Commission ac On ac.insurance_file_cnt = mifl.original_linked_insurance_file_cnt AND ifi.lead_agent_cnt = ac.party_cnt
				Inner Join insurance_file ifi2 On ifi2.insurance_file_cnt = mifl.original_linked_insurance_file_cnt
			WHERE ifi.insurance_folder_cnt = @insuranceFolderCnt AND ifi2.insurance_file_cnt <= @OldInsuranceFileCnt
				AND ac.risk_type_id = @Risk_Type_id AND ac.commission_band_id = @Sub_Commission_band
				
				AND ifi2.cover_start_date <= (Select cover_start_date FROM insurance_file with(nolock) WHERE insurance_file_cnt=@NewInsuranceFileCnt)

		SELECT @Commission_value = 0
		SELECT @Commission_value =
			Case is_value
				When 1 Then commission_value * -1
			Else (@premium * commission_percentage) / 100.00 End,
				@xBaseRate =
					Case commission_value
						When 0 Then 1
						Else base_commission_value / commission_value End,
				@xAccountRate =
					Case commission_value
						When 0 Then 1
						Else account_commission_value / commission_value End
		FROM    agent_commission
		WHERE   insurance_file_cnt = @OriginalInsuranceFileCnt
			AND commission_band_id = @Sub_Commission_band
			AND risk_type_id = @Risk_Type_id
			

		If @xAccountRate = 0
			Set @xAccountRate = 1

		If @xBaseRate = 0
			Set @xBaseRate = 1

		If @Commission_value <> 0 BEGIN
			INSERT INTO agent_commission (
				insurance_file_cnt, is_lead_agent, party_cnt, risk_type_id, commission_band_id, premium, commission_percentage,
				commission_value,
				is_amended, account_currency_id,
				account_commission_value,
				base_currency_id,
				base_commission_value,
				tax_group_id, tax_amount, tax_account_amount, tax_base_amount, override_reason,
				calculated_commission_value,
				maximum_rate, is_value, is_tax_amended)
			SELECT
				@NewInsuranceFileCnt, is_lead_agent, party_cnt, risk_type_id, commission_band_id, @Premium, commission_percentage,
				Case is_value When 1 Then commission_value * -1 Else (@premium * commission_percentage) / 100.00 End,
				is_amended, account_currency_id,
				Case is_value When 1 Then account_commission_value * -1 Else ((@premium * commission_percentage) / 100.00) * @xAccountRate End,  -- establish xRate here only as was applied originally
				base_currency_id,
				Case is_value When 1 Then base_commission_value * -1 Else ((@premium * commission_percentage) / 100.00) * @xBaseRate End,  -- establish xRate here only as was applied originally
				tax_group_id, tax_amount * -1, tax_account_amount * -1, tax_base_amount * -1, override_reason,
				Case is_value When 1 Then commission_value * -1 Else (@premium * commission_percentage) / 100.00 End,
				maximum_rate, is_value, is_tax_amended
			FROM    agent_commission
			WHERE   insurance_file_cnt = @OriginalInsuranceFileCnt
				AND commission_band_id = @Sub_Commission_band
				AND risk_type_id = @Risk_Type_id
				


			SELECT @agent_commission_cnt=@@IDENTITY

			SELECT @original_agent_commission_cnt = 0,
					@tax_group_id = 0

			SELECT @original_agent_commission_cnt = agent_commission_cnt,
				@tax_group_id = tax_group_id
			FROM Agent_Commission
			WHERE   insurance_file_cnt = @OriginalInsuranceFileCnt
				AND commission_band_id = @Sub_Commission_band
				AND risk_type_id = @Risk_Type_id

			SELECT @Commission_value = ISNULL(commission_value, 0) FROM Agent_Commission
			WHERE agent_commission_cnt = @agent_commission_cnt

			IF NOT EXISTS (SELECT NULL FROM tax_calculation WHERE agent_commission_cnt = @original_agent_commission_cnt)
			AND @tax_group_id > 0
			BEGIN
				-- Identify previous iFile WHERE comm tax was charged for the risk type
				Select @original_agent_commission_cnt = ac.agent_commission_cnt
				FROM Agent_Commission ac
				Inner Join insurance_file ifi On ifi.insurance_file_cnt = ac.insurance_file_cnt
				Inner Join Tax_Calculation tc On tc.insurance_file_cnt = ifi.insurance_file_cnt AND tc.agent_commission_cnt = ac.agent_commission_cnt
				WHERE ifi.insurance_folder_cnt = @insuranceFolderCnt
				AND ifi.insurance_file_cnt < @OriginalInsuranceFileCnt
				AND ac.commission_band_id = @Sub_Commission_band
				AND ac.risk_type_id = @Risk_Type_id
				
			END

			INSERT INTO tax_calculation (
				tax_band_id, premium, percentage,
				value,
				is_value, is_manually_changed, Calc_Basis, Basis_Value, currency_id, allow_tax_credit, country_id, state_id,
				class_of_business_id, tax_group_id, sequence, insurance_file_cnt, transtype, agent_commission_cnt,
				is_not_applied_to_client, include_tax_in_instalments, spread_tax_across_instalments, tax_band_rate_id, is_suspended)
			SELECT
				tax_band_id, @Commission_value, percentage,
				Case is_value When 1 Then value * -1 Else (@Commission_value * percentage) / 100.00 End,
				is_value, is_manually_changed, Calc_Basis, Basis_Value, currency_id, allow_tax_credit, country_id, state_id,
				class_of_business_id, tax_group_id, sequence, @NewInsuranceFileCnt, transtype, @agent_commission_cnt,
				is_not_applied_to_client, include_tax_in_instalments, spread_tax_across_instalments, tax_band_rate_id, is_suspended
			FROM tax_calculation
			WHERE agent_commission_cnt = @original_agent_commission_cnt

			SELECT @tax_currency_amount = value
				FROM Tax_Calculation WHERE agent_commission_cnt = @agent_commission_cnt

			/* Revise Tax amounts */
			UPDATE  Agent_Commission
				SET     tax_amount = @tax_currency_amount,
				tax_account_amount = ROUND(@tax_currency_amount * @xAccountRate, 2),
				tax_base_amount = @tax_currency_amount * @xBaseRate
			WHERE   agent_commission_cnt=@agent_commission_cnt
		END

		Fetch Next FROM Sub_Peril_Cursor Into @Sub_Commission_band, @Risk_Type_id, @Premium, @annual_premium

	END -- end of cursor

	-- Close and Deallocate
	Close Sub_Peril_Cursor
	Deallocate Sub_Peril_Cursor

	END
END