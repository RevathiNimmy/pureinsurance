SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure spu_MID2_Vehicle_Import_Failiure
GO

CREATE PROCEDURE spu_MID2_Vehicle_Import_Failiure
	@Batch_Ref    Varchar(20),
	@InsuranceRef as Varchar(30),
	@Registration_Number Varchar(50),
	@Error_Ref    Varchar(35),
	@Errors     Varchar(255),
	@source_id int = NULL
As
BEGIN

	DECLARE	@Batch_ID   Int,
			@Mid_Status_ID  Int,
			@Mid_Batch_Type_Id int

	SELECT @Mid_Batch_Type_Id = batch_type_id
	FROM batch_type 
	WHERE code =  'MID2'

	SELECT @Batch_ID= batch_ID 
	FROM Batch 
	WHERE  RIGHT(RTRIM('000000'+ISNULL(batch_ref,'')),6) =  RIGHT(RTRIM('000000'+ISNULL(@Batch_Ref,'')),6)
		And batch_type_id = @Mid_Batch_Type_Id
		And company_id = @source_id
		AND interface_code LIKE'%EXPORT%'

	SELECT @Mid_Status_ID= Mid_Status_ID
	FROM MID_Status
	WHERE Code='ERROR'

	 DECLARE @MId_Policy_Id INT

	Update MV
	SET reject_reference = @Error_Ref,
		reject_error_codes = @Errors,
		Mid_Status_ID = @Mid_Status_ID,
		@MId_Policy_Id=MP.mid_policy_id  
	FROM MID_Policy as MP inner join insurance_file as InF on MP.insurance_file_cnt = InF.insurance_file_cnt
		inner join MID_Vehicle as MV on MP.MID_Policy_Id = MV.MID_Policy_Id
	WHERE MP.Batch_Id = @Batch_ID
		And InF.insurance_ref = @InsuranceRef
		And MV.registration = @Registration_Number

	UPDATE MID_Policy
	set Mid_Status_ID = @Mid_Status_ID WHERE Mid_policy_id = @MId_Policy_Id

End
GO