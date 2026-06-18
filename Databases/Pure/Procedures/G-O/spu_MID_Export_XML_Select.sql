SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_MID_Export_XML_Select'
GO

CREATE Procedure spu_MID_Export_XML_Select        
	@batch_id int = NULL,
	@rule_id int = NULL,
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
	UPDATE MP
	SET	batch_id = @batch_id
	FROM mid_policy MP
		INNER JOIN  insurance_file inf on MP.insurance_file_cnt = inf.insurance_file_cnt
	WHERE  MP.batch_id IS NULL
		AND inf.insurance_file_type_id IN (2, 5, 6, 8, 9) -- Only Live policies POLICY
		AND inf.source_id  = @branch_id
		AND MP.mid_type = 'MID1'

	--Set all records to generated
	UPDATE	mid_policy
	SET	mid_status_id = (SELECT mid_status_id FROM MID_Status WHERE code='GENERATED')
	WHERE	batch_id = @batch_id

	UPDATE	mid_vehicle
	SET	mid_status_id = (SELECT mid_status_id FROM MID_Status WHERE code='GENERATED')
	FROM mid_vehicle mfv
		INNER JOIN	mid_policy MP ON	mfv.mid_policy_id = MP.mid_policy_id
	WHERE MP.batch_id = @batch_id

	--Increment PPCC and set policy update status on policies being exported
	DECLARE @mid_policy_id int
	DECLARE @insurance_folder_cnt int
	DECLARE @insurance_file_type_id int

	DECLARE policy_cursor CURSOR FOR
	SELECT	mid_policy_id, MP.insurance_folder_cnt, INF.insurance_file_type_id
	FROM   mid_policy MP
		INNER JOIN  insurance_file INF on MP.insurance_file_cnt = INF.insurance_file_cnt
	WHERE  MP.batch_id = @batch_id
	ORDER BY  mid_policy_id

	OPEN policy_cursor
	FETCH NEXT FROM policy_cursor INTO 	@mid_policy_id, @insurance_folder_cnt, @insurance_file_type_id

	WHILE @@FETCH_STATUS = 0
	BEGIN

    DECLARE @last_mid_id int
    DECLARE @last_mid_status_code varchar(10)
    DECLARE @last_mid_errors varchar(255)
	DECLARE	@Prev_Mid_Policy_Id INT
	DECLARE @IsChangeInPolicyVehRecord BIT = 0

	SELECT @Prev_Mid_Policy_Id = MAX(Mid_policy_id)
	FROM MID_Policy MP
		INNER JOIN MID_status MS ON MP.Mid_status_id = MS.MID_status_id
		INNER JOIN Insurance_File INF on INF.insurance_file_cnt = MP.insurance_file_cnt
	WHERE MP.insurance_folder_cnt = @insurance_folder_cnt
		AND Mid_policy_id < @mid_policy_id
		AND INF.insurance_file_type_id IN (2, 5, 6, 8, 9, 11)
		AND INF.source_id  = @branch_id
		AND MP.mid_type = 'MID1'

	-- Look for a change in data only for MTA file types
	IF ISNULL(@Prev_Mid_Policy_Id,0) <> 0 AND @insurance_file_type_id IN (5,6)
	BEGIN
		----Check Whether There is a Change In Policy/ PolicyHolder Information Or Not
		IF EXISTS (SELECT MP.Policyholder_name,MP.Policyholder_DOB,MP.Address1,MP.Address2,MP.Address3,MP.Address4,MP.Address5, MP.Address6,MP.PostCode,MP.Policyholder_DrivingOtherVehicles, CONVERT(DATE,INF.cover_start_date),CONVERT(DATE,INF.expiry_date)
		FROM mid_policy MP
			INNER JOIN Insurance_File INF on INF.insurance_file_cnt = MP.insurance_file_cnt
		WHERE MP.Mid_policy_id = @Prev_Mid_Policy_Id
		EXCEPT
		SELECT MP.policyholder_name,MP.Policyholder_DOB,MP.Address1,MP.Address2,MP.Address3,MP.Address4,MP.Address5, MP.Address6,MP.PostCode,MP.Policyholder_DrivingOtherVehicles, CONVERT(DATE,INF.cover_start_date),CONVERT(DATE,INF.expiry_date)
		FROM mid_policy MP
			INNER JOIN Insurance_File INF on INF.insurance_file_cnt = MP.insurance_file_cnt
		WHERE MP.Mid_policy_id = @mid_policy_id)
			SET @IsChangeInPolicyVehRecord = 1

		----Check Whether There is a Change In Vehicle Information Or Not
		IF EXISTS (SELECT ISNULL(MV.is_foreign_registration,0) AS is_foreign_registration,MV.Registration,MV.Make,MV.Model,CONVERT(DATE,MV.On_date),CONVERT(DATE,MV.Off_date),MV.permitted_drivers,MV.class_use,MV.VIN,MV.Cover_Type
		FROM mid_policy MP
			LEFT JOIN MID_Vehicle MV on mp.Mid_policy_id=mv.MID_policy_id
		WHERE MP.Mid_policy_id= @Prev_Mid_Policy_Id AND MV.update_type <> 'D'
		EXCEPT
		SELECT ISNULL(MV.is_foreign_registration,0) AS is_foreign_registration,MV.Registration,MV.Make,MV.Model,CONVERT(DATE,MV.On_date),CONVERT(DATE,MV.Off_date),MV.permitted_drivers,MV.class_use,MV.VIN,MV.Cover_Type
		FROM mid_policy MP
		LEFT JOIN MID_Vehicle MV on mp.Mid_policy_id=mv.MID_policy_id
		WHERE MP.Mid_policy_id=@mid_policy_id)
			SET @IsChangeInPolicyVehRecord = 1

		IF EXISTS (SELECT ISNULL(MV.is_foreign_registration,0) AS is_foreign_registration,MV.Registration,MV.Make,MV.Model,CONVERT(DATE,MV.On_date),CONVERT(DATE,MV.Off_date),MV.permitted_drivers,MV.class_use,MV.VIN,MV.Cover_Type
		FROM mid_policy MP
		LEFT JOIN MID_Vehicle MV on mp.Mid_policy_id=mv.MID_policy_id
		WHERE MP.Mid_policy_id=@mid_policy_id
		EXCEPT
		SELECT ISNULL(MV.is_foreign_registration,0) AS is_foreign_registration,MV.Registration,MV.Make,MV.Model,CONVERT(DATE,MV.On_date),CONVERT(DATE,MV.Off_date),MV.permitted_drivers,MV.class_use,MV.VIN,MV.Cover_Type
		FROM mid_policy MP
			LEFT JOIN MID_Vehicle MV on mp.Mid_policy_id=mv.MID_policy_id
		WHERE MP.Mid_policy_id= @Prev_Mid_Policy_Id AND MV.update_type <> 'D')
			SET @IsChangeInPolicyVehRecord = 1

		--Check Whether There is a Change In Vehicle Information (Specifically for Registration Number)
		IF EXISTS (SELECT MV.Registration
		FROM mid_policy MP
			LEFT JOIN MID_Vehicle MV on mp.Mid_policy_id=mv.MID_policy_id
		WHERE MP.Mid_policy_id= @Prev_Mid_Policy_Id AND MV.update_type <> 'D'
		EXCEPT
		SELECT MV.Registration
		FROM mid_policy MP
			LEFT JOIN MID_Vehicle MV on mp.Mid_policy_id=mv.MID_policy_id
		WHERE MP.Mid_policy_id=@mid_policy_id)
			SET @IsChangeInPolicyVehRecord = 1

		--If No data is changed from previous version, Set Mid Policy to be excluded from Export
		IF @IsChangeInPolicyVehRecord <> 1
		BEGIN
			UPDATE mid_policy SET ExcludeFromExport = 1
			WHERE mid_policy_id = @mid_policy_id
		END
	END

    SELECT @last_mid_id = MAX(mid_policy_id)
    FROM mid_policy
    WHERE insurance_folder_cnt = @insurance_folder_cnt
		AND mid_policy_id < @mid_policy_id -- Only get older versions
		AND batch_id IS NOT NULL --Only get exported records

    SELECT @last_mid_status_code = Code,
		   @last_mid_errors = MP.reject_error_codes
    FROM   mid_status mps
		INNER JOIN mid_policy MP  ON mps.mid_status_id = MP.mid_status_id
    WHERE mid_policy_id = @last_mid_id

    --Case 1, First version of policy
    IF (@last_mid_id IS NULL)
    BEGIN
		UPDATE  mid_policy
		SET   ppcc = 1, update_type = (CASE WHEN update_type IS NULL THEN 'N' ELSE update_type END)
		WHERE mid_policy_id = @mid_policy_id
			AND insurance_folder_cnt NOT IN (SELECT insurance_folder_cnt FROM mid_policy WHERE batch_id != @batch_id)
    END
    ELSE
    BEGIN
		IF ((@last_mid_status_code != 'ERROR') OR
		(@last_mid_status_code = 'ERROR' AND @last_mid_errors like '%W%' AND @last_mid_errors not like '%E%'))
		BEGIN
			--Case 2, Copy and incremented off last sucessful version
			UPDATE mid_policy
			SET	ppcc = (SELECT ppcc + 1 FROM mid_policy WHERE mid_policy_id = @last_mid_id),Update_Type = 'A'
			WHERE mid_policy_id = @mid_policy_id

		END
		ELSE
		BEGIN
			--Case 3, Copy off failed last version?
			UPDATE	mid_policy
			SET	ppcc = (SELECT ppcc FROM mid_policy WHERE mid_policy_id = @last_mid_id),Update_Type = 'A'
			WHERE mid_policy_id = @mid_policy_id
		END
	END

	FETCH NEXT FROM policy_cursor INTO  @mid_policy_id, @insurance_folder_cnt, @insurance_file_type_id
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

	UPDATE	MID_Rule set Current_File_Seq_Num = RIGHT(RTRIM('000000'+CAST((Current_File_Seq_Num + 1)AS VARCHAR)),6)  where MID_Rule_id = @Rule_ID

END
ELSE
BEGIN

	SELECT	@Curr_File_Seq_Num = B.batch_ref, @branch_id = B.company_id
	FROM Batch B
	WHERE Batch_id = @batch_id

	SELECT @rule_id =  MID_Rule_id
	FROM MID_Rule R
	WHERE	R.Is_Deleted <> 1
		AND R.MID_Type = 'MID1'
		AND CONVERT(DATE, GETDATE()) BETWEEN CONVERT(date, R.Start_Date) AND CONVERT(DATE, R.Expiry_Date)
		AND R.Source_id = @branch_id

	SELECT	@Supplier_Type = S.description,
			@Supplier_ID = R.Supplier_id,
			@Site_Number = R.Site_Number,
			@Test_Indicator = R.Test_Indicator
	FROM	MID_Rule R INNER JOIN Supplier_Type S ON R.Supplier_Type_id = S.Supplier_Type_id
	WHERE	MID_Rule_id = @Rule_ID

END

--TODO: Use the following (and extend joining to the first correspondance address and first
--telephone number) to create XML matching the XML defined earlier in the specification.
--For the vehicle a join will also have to be made to mid_vehicle

SELECT  1 As Tag,
Null     As Parent,
'http://www.siriusfs.com/SFI/Export/MID_Export/20060420' As [EXPORT_HEADER!1!xmlns],
'http://www.w3.org/2001/XMLSchema-instance'    As [EXPORT_HEADER!1!xmlns:xsi],
'http://www.siriusfs.com/SFI/Export/MID_Export/20060420
MID_Export.xsd' As [EXPORT_HEADER!1!xsi:schemaLocation],
'MID_Export' As [EXPORT_HEADER!1!interface_name],
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
Null          As [EXPORT_POLICY!2!mid_policy_key],
Null          As [EXPORT_POLICY!2!insurance_file_key],
Null          As [EXPORT_POLICY!2!insurance_file_version],
Null          As [EXPORT_POLICY!2!insured_key],
Null          As [EXPORT_POLICY!2!insurance_ref],
Null          As [EXPORT_POLICY!2!insurance_file_type_code],
Null          As [EXPORT_POLICY!2!insurance_file_status_code],
Null          As [EXPORT_POLICY!2!update_type],
Null          As [EXPORT_POLICY!2!ppcc],
Null          As [EXPORT_POLICY!2!insured_name],
NULL		  AS [EXPORT_POLICY!2!policyholder_dob],
NULL		  AS [EXPORT_POLICY!2!Policyholder_DrivingOtherVehicles],
Null          As [EXPORT_POLICY!2!insured_address1],
Null          As [EXPORT_POLICY!2!insured_address2],
Null          As [EXPORT_POLICY!2!insured_address3],
Null          As [EXPORT_POLICY!2!insured_address4],
NULL		  As [EXPORT_POLICY!2!insured_address5],
NULL          As [EXPORT_POLICY!2!insured_address6],
Null          As [EXPORT_POLICY!2!insured_postcode],
Null          As [EXPORT_POLICY!2!insured_telephone],
Null          As [EXPORT_POLICY!2!cover_start_date],
Null          As [EXPORT_POLICY!2!cover_end_date],
Null          As [EXPORT_POLICY!2!reject_reference],
Null          As [EXPORT_POLICY!2!reject_code],
NULL          As [EXPORT_POLICY!2!insurer_id],
NULL          As [EXPORT_POLICY!2!delegated_authority_id],
NULL          As [EXPORT_POLICY!2!da_branch_id],
NULL		As [EXPORT_POLICY!2!party_type_code],
Null   As [EXPORT_POLICY!2!mid_vehicle_key],
--Null   As [EXPORT_POLICY!2!update_type],
Null   As [EXPORT_POLICY!2!registration],
Null   As [EXPORT_POLICY!2!is_foreign_registration],
Null   As [EXPORT_POLICY!2!is_trade_registration],
Null   As [EXPORT_POLICY!2!make],
Null   As [EXPORT_POLICY!2!model],
NULL   As [EXPORT_POLICY!2!Vehicle_CoverType],
NULL   As [EXPORT_POLICY!2!Vehicle_VIN],
Null   As [EXPORT_POLICY!2!on_date],
Null   As [EXPORT_POLICY!2!off_date],
NULL   As [EXPORT_POLICY!2!vehicle_reject_reference],
NULL   As [EXPORT_POLICY!2!vehicle_reject_code],
NULL   As [EXPORT_POLICY!2!permitted_drivers],
NULl   As [EXPORT_POLICY!2!class_use]
FROM batch B
WHERE (ISNULL(B.batch_id,0) <> 0 and B.batch_id = @batch_id)

UNION ALL

SELECT 2          As [Tag],
1                 As [Parent],
'http://www.siriusfs.com/SFI/Export/MID_Export/20060420' As [EXPORT_HEADER!1!xmlns],
'http://www.w3.org/2001/XMLSchema-instance'      As [EXPORT_HEADER!1!xmlns:xsi],
'http://www.siriusfs.com/SFI/Export/MID_Export/20060420
MID_Export.xsd'  As [EXPORT_HEADER!1!xsi:schemaLocation],
'MID_Export'  As [EXPORT_HEADER!1!interface_name],
GetDate()   As [EXPORT_HEADER!1!date_exported],
@batch_id   As [EXPORT_HEADER!1!batch_id],
B.batch_ref   As [EXPORT_HEADER!1!batch_reference],
@Curr_File_Seq_Num  As [EXPORT_HEADER!1!file_Seq_Number],
@Supplier_Type  As [EXPORT_HEADER!1!supplier_type],
@Supplier_ID  As [EXPORT_HEADER!1!supplier_id],
@Site_Number  As [EXPORT_HEADER!1!site_number],
@Test_Indicator  As [EXPORT_HEADER!1!test_indicator],
0    As [EXPORT_HEADER!1!total_transactions],
0    As [EXPORT_HEADER!1!total_amount],
MP.mid_policy_id    As [EXPORT_POLICY!2!mid_policy_key],
MP.insurance_file_cnt  As [EXPORT_POLICY!2!insurance_file_key],
INF.policy_Version As [EXPORT_POLICY!2!insurance_file_version],
INF.insured_cnt    As [EXPORT_POLICY!2!insured_key],
INF.Insurance_Ref  As [EXPORT_POLICY!2!insurance_ref],
IFT.Code           As [EXPORT_POLICY!2!insurance_file_type_code],
ISNULL(IFS.Code,'Live')           As [EXPORT_POLICY!2!insurance_file_status_code],
MV.Update_Type     As [EXPORT_POLICY!2!update_type],
ISNULL(MP.PPCC,'')            As [EXPORT_POLICY!2!ppcc],
ISNULL(MP.Policyholder_Name,INF.Insured_Name)   As [EXPORT_POLICY!2!insured_name],
ISNULL(MP.Policyholder_DOB,PLS.date_of_birth)  AS [EXPORT_POLICY!2!policyholder_dob],
ISNULL(MP.Policyholder_DrivingOtherVehicles, (SELECT CASE WHEN LTRIM(RTRIM(PT.code)) = 'PC' THEN 'Y' ELSE ' ' END ))
										 AS [EXPORT_POLICY!2!Policyholder_DrivingOtherVehicles],
ISNULL(MP.Address1,AD.address1)          As [EXPORT_POLICY!2!insured_address1],
ISNULL(MP.Address2,AD.Address2)          As [EXPORT_POLICY!2!insured_address2],
ISNULL(MP.Address3,AD.Address3)          As [EXPORT_POLICY!2!insured_address3],
ISNULL(MP.Address4,AD.Address4)          As [EXPORT_POLICY!2!insured_address4],
MP.Address5          As [EXPORT_POLICY!2!insured_address5],
MP.Address6          As [EXPORT_POLICY!2!insured_address6],
ISNULL(MP.PostCode,AD.postal_code)       As [EXPORT_POLICY!2!insured_postcode],
ISNull(CONT.Number,'')          As [EXPORT_POLICY!2!insured_telephone],
CASE WHEN (LTRIM(RTRIM(IFT.code)) = 'MTA TEMP' AND CONVERT(DATE, INF.expiry_date) <= CONVERT(DATE, GETDATE()) AND ISNULL(MV.on_date,0) <> 0) OR MV.update_type='D'  
	THEN MV.on_date
	ELSE INF.cover_start_date END As [EXPORT_POLICY!2!cover_start_date],
CASE WHEN (LTRIM(RTRIM(IFT.code)) = 'MTA TEMP' AND CONVERT(DATE, INF.expiry_date) <= CONVERT(DATE, GETDATE()) AND ISNULL(MV.off_date,0) <> 0)  OR MV.update_type='D'  
	THEN MV.off_date
	ELSE INF.expiry_date END As [EXPORT_POLICY!2!cover_end_date],
MP.reject_reference  As [EXPORT_POLICY!2!reject_reference],
MP.reject_error_codes As [EXPORT_POLICY!2!reject_code],
R.Insurer_id          As [EXPORT_POLICY!2!insurer_id],
R.Delegated_Authority_id          As [EXPORT_POLICY!2!delegated_authority_id],
MP.DA_Branch_id          As [EXPORT_POLICY!2!da_branch_id],
PT.code		As [EXPORT_POLICY!2!party_type_code],
ISNULL(MV.Mid_Vehicle_ID,'')  As [EXPORT_POLICY!2!mid_vehicle_key],
--ISNULL(MV.Update_Type,'')   As [EXPORT_POLICY!2!update_type],
ISNULL(MV.Registration,'')   As [EXPORT_POLICY!2!registration],
ISNULL(MV.Is_Foreign_Registration,'')   As [EXPORT_POLICY!2!is_foreign_registration],
ISNULL(MV.Is_Trade_Registration,'')   As [EXPORT_POLICY!2!is_trade_registration],
ISNULL(MV.Make,'')   As [EXPORT_POLICY!2!make],
ISNULL(MV.Model,'')   As [EXPORT_POLICY!2!model],
ISNULL(RIGHT(RTRIM('00'+ CAST(MV.Cover_Type AS VARCHAR)),2),'  ')  As [EXPORT_POLICY!2!Vehicle_CoverType],
ISNULL(MV.VIN,'')  As [EXPORT_POLICY!2!Vehicle_VIN],
ISNULL(MV.On_Date,'')   As [EXPORT_POLICY!2!on_date],
ISNULL(MV.Off_Date,'')   As [EXPORT_POLICY!2!off_date],
ISNULL(MV.Reject_Reference,'')   As [EXPORT_POLICY!2!vehicle_reject_reference],
ISNULL(MV.Reject_Error_Codes,'')  As [EXPORT_POLICY!2!vehicle_reject_code],
ISNULL(MV.permitted_drivers,'')   As [EXPORT_POLICY!2!permitted_drivers],
ISNULL(MV.class_use,'')   As [EXPORT_POLICY!2!class_use]
FROM  Mid_Policy MP
	JOIN Batch B On MP.Batch_id=B.Batch_ID
	INNER JOIN Insurance_File INF ON MP.insurance_file_cnt = inf.insurance_file_cnt
	LEFT OUTER JOIN Insurance_File_Status IFS ON INF.insurance_file_status_id = IFS.insurance_file_status_id
	LEFT OUTER JOIN Insurance_File_Type IFT ON INF.insurance_file_type_id = IFT.insurance_file_type_id
	INNER JOIN Party PTY ON INF.insured_cnt = PTY.party_cnt
	LEFT OUTER JOIN party_lifestyle PLS ON PTY.party_cnt = PLS.party_cnt
	INNER JOIN Party_Type PT ON PTY.party_type_id=PT.party_type_id
	INNER JOIN Mid_Vehicle MV ON MV.Mid_Policy_ID=MP.Mid_Policy_ID
	LEFT OUTER JOIN Party_Address_Usage PAU ON PAU.Party_Cnt = PTY.Party_Cnt
	LEFT OUTER JOIN Address AD ON AD.Address_Cnt = PAU.Address_Cnt
	LEFT OUTER JOIN Contact_Address_Usage CAU ON CAU.Address_Cnt = AD.Address_Cnt
	LEFT OUTER JOIN Contact CONT ON CONT.Contact_Cnt = CAU.Contact_Cnt
	LEFT OUTER JOIN MID_Rule R	ON inf.source_id = R.Source_id

WHERE	MP.batch_id = @batch_id
	AND inf.source_id = @branch_id
	AND R.MID_Rule_id = @Rule_ID
	AND ISNULL(MP.ExcludeFromExport,0) = 0
	AND PAU.address_usage_type_id = 4

ORDER BY
     [EXPORT_HEADER!1!batch_id],
     [EXPORT_POLICY!2!mid_policy_key],
	 [EXPORT_POLICY!2!mid_vehicle_key]

FOR XML EXPLICIT
GO