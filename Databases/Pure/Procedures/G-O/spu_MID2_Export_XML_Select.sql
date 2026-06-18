SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure spu_MID2_Export_XML_Select
GO

CREATE Procedure spu_MID2_Export_XML_Select
	@batch_id int = NULL,
	@Rule_ID int = NULL,
	@branch_id INT = 0,
	@new_batch bit = 0
AS
	DECLARE @Curr_File_Seq_Num INT,
			@Supplier_Type varchar(25),
			@Supplier_ID INT,
			@Site_Number INT,
			@Test_Indicator BIT

	If (@new_batch = 1)
	BEGIN
		DECLARE @Mid_Policies table (Policy_Id varchar(128))
		INSERT into @Mid_Policies
		SELECT mid_policy_id FROM	mid_policy MP
			INNER JOIN insurance_file inf on MP.insurance_file_cnt = inf.insurance_file_cnt
		WHERE	MP.batch_id IS NULL
			AND	inf.insurance_file_type_id IN (2, 5, 6, 8, 9, 11) -- Only Live policies
			AND inf.source_id  = @branch_id
			AND MP.mid_type = 'MID2'

		UPDATE mid_policy
		SET		batch_id = @batch_id
		FROM	mid_policy MP
		WHERE	MP.mid_policy_id IN (SELECT policy_id from @Mid_Policies)

		--Set policy and vehicle records to generated
		UPDATE mid_policy
		SET		mid_status_id = (SELECT mid_status_id FROM MID_Status WHERE code='GENERATED')
		WHERE	mid_policy_id IN (SELECT policy_id from @Mid_Policies)

		UPDATE	mid_vehicle
		SET		mid_status_id = (SELECT mid_status_id FROM MID_Status WHERE code='GENERATED')
		FROM	mid_vehicle mfv
			INNER JOIN mid_policy MP ON	mfv.mid_policy_id = MP.mid_policy_id
		WHERE	MP.mid_policy_id IN (SELECT policy_id from @Mid_Policies)

		--Increment PPCC and set policy update status on policies being exported
		DECLARE @mid_policy_id int
		DECLARE @insurance_folder_cnt int

		DECLARE policy_cursor CURSOR FOR
		SELECT	mid_policy_id, MP.insurance_folder_cnt
		FROM	mid_policy MP
		WHERE	MP.mid_policy_id IN (SELECT policy_id from @Mid_Policies)
		ORDER BY mid_policy_id

		OPEN policy_cursor
		FETCH NEXT FROM policy_cursor INTO
		@mid_policy_id, @insurance_folder_cnt

		WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @last_mid_id int
			DECLARE @last_mid_status_code varchar(10)
			DECLARE @last_mid_errors varchar(255)
			/*@LastLiveMID_Policy_Id = MIDPolicyID for last LIVE version of the policy weather exported or unexported */
			DECLARE @LastLiveMID_Policy_Id INT
			SET @LastLiveMID_Policy_Id = ISNULL((SELECT MAX(mid_policy_id) FROM mid_policy MP INNER JOIN Insurance_File INF ON MP.insurance_file_cnt = INF.insurance_file_cnt 
											WHERE MP.insurance_folder_cnt = @insurance_folder_cnt AND MP.mid_policy_id < @mid_policy_id AND INF.insurance_file_type_id IN (2, 5, 6, 8, 9)),0) 

			SELECT	@last_mid_id = MAX(mid_policy_id)
			FROM	mid_policy
			WHERE insurance_folder_cnt = @insurance_folder_cnt
				AND	mid_policy_id < @mid_policy_id -- Only get older versions
				AND	batch_id IS NOT NULL --Only get exported records

			SELECT	@last_mid_status_code = Code,
					@last_mid_errors = MP.reject_error_codes
			FROM	mid_status ms
				INNER JOIN	mid_policy MP ON	ms.mid_status_id = MP.mid_status_id
			WHERE 	mid_policy_id = @last_mid_id

			--Case 1, First version of policy
			IF (@last_mid_id IS NULL)
			BEGIN
				UPDATE	mid_policy
				SET		ppcc = 1,update_type = 'N'
				WHERE	mid_policy_id = @mid_policy_id
					AND	insurance_folder_cnt NOT IN (SELECT insurance_folder_cnt FROM mid_policy WHERE batch_id != @batch_id)
			END
			ELSE
			BEGIN
				IF ((@last_mid_status_code != 'ERROR') OR
					(@last_mid_status_code = 'ERROR' AND @last_mid_errors like '%W%' AND @last_mid_errors not like '%E%'))
				BEGIN
					--Case 2, Copy and incremented off last sucessful version
					UPDATE	mid_policy
					SET		ppcc = (SELECT ppcc + 1 FROM mid_policy WHERE mid_policy_id = @last_mid_id), Update_Type = 'A'
					WHERE	mid_policy_id = @mid_policy_id

					UPDATE mid_vehicle SET update_type = 'N'    
  						WHERE @LastLiveMID_Policy_Id > 0 AND MID_policy_id = @Mid_Policy_ID    
   						AND Registration NOT IN (SELECT Registration from mid_vehicle where mid_policy_id = @LastLiveMID_Policy_Id )  
						AND NOT (update_type = 'A'  
						AND  EXISTS (SELECT 1 FROM mid_vehicle AS mv  
    				INNER JOIN mid_policy AS mp   
       					ON mp.mid_policy_id = mv.mid_policy_id  
    					WHERE mp.insurance_folder_cnt = (SELECT insurance_folder_cnt FROM mid_policy WHERE mid_policy_id = @Mid_Policy_ID)  
      					AND EXISTS (SELECT 1 FROM mid_vehicle AS v0  
            			WHERE v0.mid_policy_id = @Mid_Policy_ID AND v0.Registration = mv.Registration)  
      					AND mv.mid_policy_id <> @Mid_Policy_ID))

					--Updating update_type(policy update status) to 'R' on renewal policies being exported
					UPDATE	MiP
					SET		MiP.Update_Type = 'R'
					FROM mid_policy AS MiP
						INNER JOIN insurance_file AS InF	ON MiP.insurance_file_cnt = InF.insurance_file_cnt
						INNER JOIN insurance_file_system AS IFS		ON InF.insurance_file_cnt = IFS.insurance_file_cnt
						LEFT OUTER JOIN transaction_type AS TT		ON IFS.last_trans_type_id = TT.transaction_type_id
						LEFT OUTER JOIN insurance_file_type AS IFT		ON InF.insurance_file_type_id = IFT.insurance_file_type_id
					WHERE (((InF.insurance_file_status_id IS NULL AND TT.code = 'REN')
							AND ((SELECT insurance_file_cnt FROM mid_policy WHERE mid_policy_id = @last_mid_id) != InF.insurance_file_cnt))
							OR (((SELECT TOP 1 insurance_file_status.code FROM insurance_file
									INNER JOIN mid_policy ON insurance_file.insurance_file_cnt = mid_policy.insurance_file_cnt
									LEFT OUTER JOIN insurance_file_status ON insurance_file.insurance_file_status_id = insurance_file_status.insurance_file_status_id
									WHERE mid_policy_id = @last_mid_id)  = 'REN' )
									AND (IFT.code = 'WRITTEN' ))
							OR
							EXISTS(SELECT renewal_status_cnt FROM Renewal_Status
								WHERE renewal_insurance_file_cnt = InF.insurance_file_cnt
									AND renewal_status_type_id = (SELECT renewal_status_type_id from renewal_status_type where LTRIM(RTRIM(code)) = 'Written')))
							AND (MiP.mid_policy_id = @mid_policy_id)

					--Updating update_type(policy update status) to 'D' on policy cancellation)
					/*UPDATE	MiP
					SET		MiP.Update_Type = 'D'
					FROM mid_policy AS MiP
						INNER JOIN insurance_file AS InF	ON MiP.insurance_file_cnt = InF.insurance_file_cnt
						INNER JOIN insurance_file_system AS IFS		ON InF.insurance_file_cnt = IFS.insurance_file_cnt
						LEFT OUTER JOIN transaction_type AS TT		ON IFS.last_trans_type_id = TT.transaction_type_id
						LEFT OUTER JOIN insurance_file_type AS IFT		ON InF.insurance_file_type_id = IFT.insurance_file_type_id
					WHERE (((InF.insurance_file_status_id IS NULL AND TT.code = 'MTC')
							AND ((SELECT insurance_file_cnt FROM mid_policy WHERE mid_policy_id = @last_mid_id) != InF.insurance_file_cnt))
							OR ( ((SELECT TOP 1 insurance_file_status.code FROM insurance_file INNER JOIN mid_policy ON insurance_file.insurance_file_cnt = mid_policy.insurance_file_cnt
									LEFT OUTER JOIN insurance_file_status ON insurance_file.insurance_file_status_id = insurance_file_status.insurance_file_status_id
									WHERE mid_policy_id = @last_mid_id)  = 'CAN' )
									--AND (IFT.code = 'MTACAN' )
									))
						AND (MiP.mid_policy_id = @mid_policy_id)*/

				END
				ELSE
				BEGIN
					--Case 3, Copy off failed last version?
					UPDATE	mid_policy
					SET		ppcc = (SELECT ppcc FROM mid_policy WHERE mid_policy_id = @last_mid_id),Update_Type = 'A'
					WHERE mid_policy_id = @mid_policy_id
				END
			END

			FETCH NEXT FROM policy_cursor INTO
			@mid_policy_id, @insurance_folder_cnt

		END

		CLOSE policy_cursor
		DEALLOCATE policy_cursor

		SELECT	@Curr_File_Seq_Num = R.Current_File_Seq_Num,
				@Supplier_Type = S.description,
				@Supplier_ID = R.Supplier_id,
				@Site_Number = R.Site_Number,
				@Test_Indicator = R.Test_Indicator
		FROM	MID_Rule R INNER JOIN Supplier_Type S ON R.Supplier_Type_id = S.Supplier_Type_id
		WHERE	MID_Rule_id = @Rule_ID
		UPDATE	MID_Rule set Current_File_Seq_Num = RIGHT(RTRIM('000000'+CAST((Current_File_Seq_Num + 1)AS VARCHAR)),6) where MID_Rule_id = @Rule_ID

	END
	ELSE
	BEGIN

		SELECT	@Curr_File_Seq_Num = B.batch_ref, @branch_id = B.company_id
		FROM Batch B
		WHERE Batch_id = @batch_id

		SELECT @rule_id =  MID_Rule_id
		FROM MID_Rule R
		WHERE	R.Is_Deleted <> 1
			AND R.MID_Type = 'MID2'
			AND CONVERT(DATE, GETDATE()) BETWEEN CONVERT(date, R.Start_Date) AND CONVERT(DATE, R.Expiry_Date)
			AND R.Source_id = @branch_id

		SELECT	@Supplier_Type = S.description,
				@Supplier_ID = R.Supplier_id,
				@Site_Number = R.Site_Number,
				@Test_Indicator = R.Test_Indicator
		FROM	MID_Rule R INNER JOIN Supplier_Type S ON R.Supplier_Type_id = S.Supplier_Type_id
		WHERE	MID_Rule_id = @Rule_ID

	END

	SELECT	1 As Tag,
		Null     As Parent,
		'http://www.siriusfs.com/SFI/Export/MID2_Export/20060420' As [EXPORT_HEADER!1!xmlns],
		'http://www.w3.org/2001/XMLSchema-instance'    As [EXPORT_HEADER!1!xmlns:xsi],
		'http://www.siriusfs.com/SFI/Export/MID2_Export/20060420MID2_Export.xsd' As [EXPORT_HEADER!1!xsi:schemaLocation],
		'MID2_Export' As [EXPORT_HEADER!1!interface_name],
		GetDate()  As [EXPORT_HEADER!1!date_exported],
		@batch_id  As [EXPORT_HEADER!1!batch_id],
		B.batch_ref  As [EXPORT_HEADER!1!batch_reference],
		@Curr_File_Seq_Num  As [EXPORT_HEADER!1!file_Seq_Number],
		@Supplier_Type  As [EXPORT_HEADER!1!supplier_type],
		@Supplier_ID  As [EXPORT_HEADER!1!supplier_id],
		@Site_Number  As [EXPORT_HEADER!1!site_number],
		@Test_Indicator  As [EXPORT_HEADER!1!test_indicator],
		0    As [EXPORT_HEADER!1!total_transactions],
		0   As [EXPORT_HEADER!1!total_amount],
		NULL		  As [EXPORT_POLICY!2!mid_policy_key],
		Null          As [EXPORT_POLICY!2!insurance_file_key],
		Null          As [EXPORT_POLICY!2!insurance_file_version],
		Null          As [EXPORT_POLICY!2!insured_key],
		Null          As [EXPORT_POLICY!2!insurance_ref],
		Null          As [EXPORT_POLICY!2!insurance_file_type_code],
		Null          As [EXPORT_POLICY!2!insurance_file_status_code],
		Null          As [EXPORT_POLICY!2!update_type],
		Null          As [EXPORT_POLICY!2!ppcc],
		Null          As [EXPORT_POLICY!2!insured_name],
		Null          As [EXPORT_POLICY!2!insured_address1],
		Null          As [EXPORT_POLICY!2!insured_address2],
		Null          As [EXPORT_POLICY!2!insured_address3],
		Null          As [EXPORT_POLICY!2!insured_address4],
		Null          As [EXPORT_POLICY!2!insured_postcode],
		Null          As [EXPORT_POLICY!2!insured_telephone],
		Null          As [EXPORT_POLICY!2!cover_start_date],
		Null          As [EXPORT_POLICY!2!cover_end_date],
		NULL          As [EXPORT_POLICY!2!lapsed_reason],
		Null          As [EXPORT_POLICY!2!reject_reference],
		Null          As [EXPORT_POLICY!2!reject_code],
		Null          As [EXPORT_POLICY!2!insurer_id],
		Null          As [EXPORT_POLICY!2!delegated_authority_id],
		Null          As [EXPORT_POLICY!2!da_branch_id],
		Null	As [EXPORT_VEHICLE!3!mid_vehicle_key],
		Null   As [EXPORT_VEHICLE!3!update_type],
		Null   As [EXPORT_VEHICLE!3!registration],
		Null   As [EXPORT_VEHICLE!3!is_foreign_registration],
		Null   As [EXPORT_VEHICLE!3!is_trade_registration],
		Null   As [EXPORT_VEHICLE!3!make],
		Null   As [EXPORT_VEHICLE!3!model],
		Null   As [EXPORT_VEHICLE!3!on_date],
		Null   As [EXPORT_VEHICLE!3!off_date],
		Null   As [EXPORT_VEHICLE!3!reject_reference],
		Null   As [EXPORT_VEHICLE!3!reject_code],
		NULL   As [EXPORT_VEHICLE!3!permitted_drivers],
		NULL   As [EXPORT_VEHICLE!3!class_use],
		Null   As [EXPORT_VEHICLE!3!insurer_id],
		Null   As [EXPORT_VEHICLE!3!delegated_authority_id],
		Null   As [EXPORT_VEHICLE!3!da_branch_id]
	FROM batch B
	WHERE (ISNULL(B.batch_id,0) <> 0 and B.batch_id = @batch_id)

	UNION ALL  ---2nd

	SELECT 2	As [Tag],
		1 As [Parent],
		'http://www.siriusfs.com/SFI/Export/MID2_Export/20060420' As [EXPORT_HEADER!1!xmlns],
		'http://www.w3.org/2001/XMLSchema-instance'      As [EXPORT_HEADER!1!xmlns:xsi],
		'http://www.siriusfs.com/SFI/Export/MID2_Export/20060420MID2_Export.xsd'  As [EXPORT_HEADER!1!xsi:schemaLocation],
		'MID2_Export'  As [EXPORT_HEADER!1!interface_name],
		GetDate()   As [EXPORT_HEADER!1!date_exported],
		@batch_id   As [EXPORT_HEADER!1!batch_id],
		B.batch_ref   As [EXPORT_HEADER!1!batch_reference],
		@Curr_File_Seq_Num  As [EXPORT_HEADER!1!file_Seq_Number],
		@Supplier_Type  As [EXPORT_HEADER!1!supplier_type],
		@Supplier_ID  As [EXPORT_HEADER!1!supplier_id],
		@Site_Number  As [EXPORT_HEADER!1!site_number],
		@Test_Indicator  As [EXPORT_HEADER!1!test_indicator],
		0    As [EXPORT_HEADER!1!total_transactions],
		0   As [EXPORT_HEADER!1!total_amount],
		MP.mid_policy_id    As [EXPORT_POLICY!2!mid_policy_key],
		MP.insurance_file_cnt  As [EXPORT_POLICY!2!insurance_file_key],
		INF.policy_Version As [EXPORT_POLICY!2!insurance_file_version],
		INF.insured_cnt    As [EXPORT_POLICY!2!insured_key],
		INF.Insurance_Ref  As [EXPORT_POLICY!2!insurance_ref],
		IFT.Code           As [EXPORT_POLICY!2!insurance_file_type_code],
		ISNULL(IFS.Code,'Live')           As [EXPORT_POLICY!2!insurance_file_status_code],
		MP.Update_Type     As [EXPORT_POLICY!2!update_type],
		ISNULL(MP.PPCC,'')            As [EXPORT_POLICY!2!ppcc],
		INF.Insured_Name   As [EXPORT_POLICY!2!insured_name],
		AD.Address1          As [EXPORT_POLICY!2!insured_address1],
		AD.Address2          As [EXPORT_POLICY!2!insured_address2],
		AD.Address3          As [EXPORT_POLICY!2!insured_address3],
		AD.Address4          As [EXPORT_POLICY!2!insured_address4],
		AD.Postal_Code       As [EXPORT_POLICY!2!insured_postcode],
		ISNull(CONT.Number,'')          As [EXPORT_POLICY!2!insured_telephone],
		INF.cover_start_date As [EXPORT_POLICY!2!cover_start_date],
		INF.expiry_date   As [EXPORT_POLICY!2!cover_end_date],
		LR.description	As [EXPORT_POLICY!2!lapsed_reason],
		MP.reject_reference  As [EXPORT_POLICY!2!reject_reference],
		MP.reject_error_codes As [EXPORT_POLICY!2!reject_code],
		R.Insurer_id          As [EXPORT_POLICY!2!insurer_id],
		R.Delegated_Authority_id          As [EXPORT_POLICY!2!delegated_authority_id],
		MP.DA_Branch_id          As [EXPORT_POLICY!2!da_branch_id],
		Null   As [EXPORT_VEHICLE!3!mid_vehicle_key],
		Null   As [EXPORT_VEHICLE!3!update_type],
		Null   As [EXPORT_VEHICLE!3!registration],
		Null   As [EXPORT_VEHICLE!3!is_foreign_registration],
		Null   As [EXPORT_VEHICLE!3!is_trade_registration],
		Null   As [EXPORT_VEHICLE!3!make],
		Null   As [EXPORT_VEHICLE!3!model],
		Null   As [EXPORT_VEHICLE!3!on_date],
		Null   As [EXPORT_VEHICLE!3!off_date],
		Null   As [EXPORT_VEHICLE!3!reject_reference],
		Null   As [EXPORT_VEHICLE!3!reject_code],
		NULL   As [EXPORT_VEHICLE!3!permitted_drivers],
		NULL   As [EXPORT_VEHICLE!3!class_use],
		Null   As [EXPORT_VEHICLE!3!insurer_id],
		Null   As [EXPORT_VEHICLE!3!delegated_authority_id],
		Null   As [EXPORT_VEHICLE!3!da_branch_id]
	FROM	Mid_Policy MP
		JOIN Batch B	On MP.Batch_id=B.Batch_ID
		INNER JOIN	Insurance_File INF	ON	MP.insurance_file_cnt = inf.insurance_file_cnt
		LEFT OUTER JOIN	Insurance_File_Status IFS	ON	INF.insurance_file_status_id = IFS.insurance_file_status_id
		LEFT OUTER JOIN	Insurance_File_Type IFT	ON	INF.insurance_file_type_id = IFT.insurance_file_type_id
		INNER JOIN	Party PTY	ON	INF.insured_cnt = PTY.party_cnt
		LEFT OUTER JOIN	Party_Address_Usage PAU	ON	PAU.Party_Cnt = PTY.Party_Cnt
		LEFT OUTER JOIN	Address AD	ON	AD.Address_Cnt = PAU.Address_Cnt
		LEFT OUTER JOIN	Contact_Address_Usage CAU	ON	CAU.Address_Cnt = AD.Address_Cnt
		LEFT OUTER JOIN	Contact CONT	ON	CONT.Contact_Cnt = CAU.Contact_Cnt
		LEFT OUTER JOIN	Lapsed_Reason LR	ON	INF.lapsed_reason_id = LR.lapsed_reason_id
		LEFT OUTER JOIN MID_Rule R	ON inf.source_id = R.Source_id
	WHERE	MP.batch_id = @batch_id
		AND R.MID_Rule_id = @Rule_ID
		AND PAU.address_usage_type_id = 4

	UNION ALL  ---3rd

	SELECT 3	As [Tag],
		2	As [Parent],
		'http://www.siriusfs.com/SFI/Export/MID2_Export/20060420' As [EXPORT_HEADER!1!xmlns],
		'http://www.w3.org/2001/XMLSchema-instance'      As [EXPORT_HEADER!1!xmlns:xsi],
		'http://www.siriusfs.com/SFI/Export/MID2_Export/20060420MID2_Export.xsd'  As [EXPORT_HEADER!1!xsi:schemaLocation],
		'MID2_Export'  As [EXPORT_HEADER!1!interface_name],
		GetDate()   As [EXPORT_HEADER!1!date_exported],
		@batch_id   As [EXPORT_HEADER!1!batch_id],
		B.batch_ref   As [EXPORT_HEADER!1!batch_reference],
		@Curr_File_Seq_Num  As [EXPORT_HEADER!1!file_Seq_Number],
		@Supplier_Type  As [EXPORT_HEADER!1!supplier_type],
		@Supplier_ID  As [EXPORT_HEADER!1!supplier_id],
		@Site_Number  As [EXPORT_HEADER!1!site_number],
		@Test_Indicator  As [EXPORT_HEADER!1!test_indicator],
		0    As [EXPORT_HEADER!1!total_transactions],
		0   As [EXPORT_HEADER!1!total_amount],
		MP.mid_policy_id    As [EXPORT_POLICY!2!mid_policy_key],
		MP.insurance_file_cnt  As [EXPORT_POLICY!2!insurance_file_key],
		INF.policy_Version As [EXPORT_POLICY!2!insurance_file_version],
		INF.insured_cnt    As [EXPORT_POLICY!2!insured_key],
		INF.Insurance_Ref  As [EXPORT_POLICY!2!insurance_ref],
		IFT.Code           As [EXPORT_POLICY!2!insurance_file_type_code],
		ISNULL(IFS.Code,'Live')           As [EXPORT_POLICY!2!insurance_file_status_code],
		MP.Update_Type     As [EXPORT_POLICY!2!update_type],
		ISNULL(MP.PPCC,'')            As [EXPORT_POLICY!2!ppcc],
		INF.Insured_Name   As [EXPORT_POLICY!2!insured_name],
		AD.Address1          As [EXPORT_POLICY!2!insured_address1],
		AD.Address2          As [EXPORT_POLICY!2!insured_address2],
		AD.Address3          As [EXPORT_POLICY!2!insured_address3],
		AD.Address4          As [EXPORT_POLICY!2!insured_address4],
		AD.Postal_Code       As [EXPORT_POLICY!2!insured_postcode],
		ISNull(CONT.Number,'')          As [EXPORT_POLICY!2!insured_telephone],
		INF.cover_start_date As [EXPORT_POLICY!2!cover_start_date],
		INF.expiry_date   As [EXPORT_POLICY!2!cover_end_date],
		LR.description	As [EXPORT_POLICY!2!lapsed_reason],
		MP.reject_reference  As [EXPORT_POLICY!2!reject_reference],
		MP.reject_error_codes As [EXPORT_POLICY!2!reject_code],
		R.Insurer_id          As [EXPORT_POLICY!2!insurer_id],
		R.Delegated_Authority_id          As [EXPORT_POLICY!2!delegated_authority_id],
		MP.DA_Branch_id          As [EXPORT_POLICY!2!da_branch_id],
		ISNULL(MV.Mid_Vehicle_ID,'')  As [EXPORT_VEHICLE!3!mid_vehicle_key],
		ISNULL(MV.Update_Type,'')   As [EXPORT_VEHICLE!3!update_type],
		ISNULL(MV.Registration,'')   As [EXPORT_VEHICLE!3!registration],
		ISNULL(MV.Is_Foreign_Registration,'')   As [EXPORT_VEHICLE!3!is_foreign_registration],
		ISNULL(MV.Is_Trade_Registration,'')   As [EXPORT_VEHICLE!3!is_trade_registration],
		ISNULL(MV.Make,'')   As [EXPORT_VEHICLE!3!make],
		ISNULL(MV.Model,'')   As [EXPORT_VEHICLE!3!model],
		ISNULL(MV.On_Date,'')   As [EXPORT_VEHICLE!3!on_date],
		ISNULL(MV.Off_Date,'')   As [EXPORT_VEHICLE!3!off_date],
		ISNULL(MV.Reject_Reference,'')   As [EXPORT_VEHICLE!3!reject_reference],
		ISNULL(MV.Reject_Error_Codes,'')  As [EXPORT_VEHICLE!3!reject_code],
		ISNULL(MV.permitted_drivers,'')   As [EXPORT_VEHICLE!3!permitted_drivers],
		ISNULL(MV.class_use,'')   As [EXPORT_VEHICLE!3!class_use],
		R.Insurer_id   As [EXPORT_VEHICLE!3!insurer_id],
		R.Delegated_Authority_id   As [EXPORT_VEHICLE!3!delegated_authority_id],
		MP.DA_Branch_id   As [EXPORT_VEHICLE!3!da_branch_id]
	FROM	Mid_Policy MP
		JOIN Batch B	On	MP.Batch_id=B.Batch_ID
		INNER JOIN	Insurance_File INF	ON	MP.insurance_file_cnt = inf.insurance_file_cnt
		LEFT OUTER JOIN	Insurance_File_Status IFS	ON	INF.insurance_file_status_id = IFS.insurance_file_status_id
		LEFT OUTER JOIN	Insurance_File_Type IFT	ON	INF.insurance_file_type_id = IFT.insurance_file_type_id
		INNER JOIN	Party PTY	ON	INF.insured_cnt = PTY.party_cnt
		INNER JOIN	Mid_Vehicle MV	ON	MV.Mid_Policy_ID=MP.Mid_Policy_ID
		LEFT OUTER JOIN	Party_Address_Usage PAU	ON	PAU.Party_Cnt = PTY.Party_Cnt
		LEFT OUTER JOIN	Address AD	ON	AD.Address_Cnt = PAU.Address_Cnt
		LEFT OUTER JOIN	Contact_Address_Usage CAU	ON	CAU.Address_Cnt = AD.Address_Cnt
		LEFT OUTER JOIN	Contact CONT	ON	CONT.Contact_Cnt = CAU.Contact_Cnt
		LEFT OUTER JOIN	Lapsed_Reason LR	ON	INF.lapsed_reason_id = LR.lapsed_reason_id
		LEFT OUTER JOIN MID_Rule R	ON inf.source_id = R.Source_id
	WHERE	MP.batch_id = @batch_id
		AND UPPER(IFT.code) <> 'WRITTEN'
		AND inf.source_id = @branch_id
		AND R.MID_Rule_id = @Rule_ID
		AND PAU.address_usage_type_id = 4

	order by
	[EXPORT_HEADER!1!batch_id],
	[EXPORT_POLICY!2!mid_policy_key],
	[EXPORT_VEHICLE!3!mid_vehicle_key]

	FOR XML EXPLICIT
GO