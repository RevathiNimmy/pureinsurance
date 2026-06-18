SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure spu_MID2_Policy_Import_Failiure
GO

CREATE PROCEDURE spu_MID2_Policy_Import_Failiure
	@InsuranceRef as Varchar(30),
	@Batch_Ref as Varchar(20),
	@PPCC As Int,
	@Expected_PPCC As Int,
	@Error_Ref As Varchar(35),
	@Errors As Varchar(80),
	@source_id int = NULL
As
BEGIN
	Declare	@Batch_ID   Int,
		@Mid_Status_ID  Int,
		@Mid_Batch_Type_Id Int

	select @Mid_Batch_Type_Id = batch_type_id
	from batch_type
	where code =  'MID2'

	Select @Batch_ID= batch_ID
	from Batch 
	Where  RIGHT(RTRIM('000000'+ISNULL(batch_ref,'')),6) =  RIGHT(RTRIM('000000'+ISNULL(@Batch_Ref,'')),6)
		And batch_type_id = @Mid_Batch_Type_Id
		AND company_id = @source_id
		AND interface_code LIKE'%EXPORT%'

	Select @Mid_Status_ID= Mid_Status_ID
	From MID_Status
	Where Code = 'ERROR'

	Update MID
	Set reject_reference=@Error_Ref,
		reject_error_codes=@Errors,
		Mid_Status_ID=@Mid_Status_ID,
		ppcc_expected = @Expected_PPCC
	From MID_Policy as MID
		Inner Join Insurance_file as InF	On MID.insurance_file_cnt = Inf.insurance_file_cnt
	Where MID.batch_id=@Batch_ID
		And MID.ppcc= @PPCC
		And InF.insurance_ref = @InsuranceRef

END
GO