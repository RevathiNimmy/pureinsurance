EXECUTE DDLDropProcedure 'spu_SIR_AcceptRenewal'
GO

CREATE PROCEDURE spu_SIR_AcceptRenewal

	@old_insurance_file_cnt int,    
	@new_insurance_file_cnt int,  
	@old_insurance_file_status_id int,    
	@new_insurance_file_type_id int,   

	@new_expiry_date datetime = NULL,   
	@new_insurance_ref varchar(30) = NULL,  
	@new_cover_start_date datetime = NULL,  

	@FailureMessage VARCHAR(500) OUTPUT,
	@ReturnValue INT OUTPUT 
AS  


DECLARE @InsuranceFileStatusID INT,
@REPInsuranceFileStatusId INT,
@Insurance_File_Type_Id INT,
@PM_FAILED_RENEWAL_STATUS INT,
@is_midnight_renewal INT,
@Lead_Agent_Cnt INT,
@new_renewal_date DateTime,
@alternate_reference_mandatory INT,
@alternate_reference_for_each_transaction INT,
@alternate_reference VARCHAR(80),
@is_update_status TINYINT

SET @PM_FAILED_RENEWAL_STATUS = 60132

SELECT @REPInsuranceFileStatusId = insurance_file_status_id FROM Insurance_File_Status WHERE code = 'REP'
SELECT @Insurance_File_Type_Id = Insurance_File_Type_Id FROM Insurance_File_Type WHERE code = 'POLICY'

SELECT @InsuranceFileStatusID = Insurance_file_status_ID From Insurance_File 
WHERE Insurance_File_CNT  = @old_insurance_file_cnt


SELECT @is_midnight_renewal = p.is_midnight_renewal, @Lead_Agent_Cnt = ifi.Lead_agent_Cnt ,
		@alternate_reference = IFI.alternate_reference
		FROM Insurance_File ifi JOIN Product p ON ifi.product_id = p.product_id 
		WHERE ifi.insurance_file_cnt = @new_insurance_file_cnt


IF Not Exists (SELECT NULL FROM Renewal_Status WHERE  renewal_insurance_file_cnt = @new_insurance_file_cnt)
BEGIN
	SET @FailureMessage = 'FAILED_RENEWAL_STATUS'
	SET @ReturnValue = @PM_FAILED_RENEWAL_STATUS
END
IF @new_expiry_date IS NOT NULL
BEGIN
	IF @is_midnight_renewal = 1 
		SET @new_renewal_date =DATEADD(d,1,@new_expiry_date)
	ELSE
		SET @new_renewal_date =@new_expiry_date
END

SELECT @is_update_status =0
IF ISNULL(@Lead_Agent_Cnt,0) > 0 
BEGIN
	SELECT @alternate_reference_mandatory = ISNULL(alternate_reference_mandatory,0), 
		   @alternate_reference_for_each_transaction  = ISNULL(alternate_reference_for_each_transaction,0) 	 
	FROM Party_Agent WHERE party_cnt = @Lead_Agent_cnt

	If @alternate_reference_mandatory <> 0 And @alternate_reference_for_each_transaction <> 0
		BEGIN
		IF @alternate_reference =''
			BEGIN
				SET @FailureMessage = 'The Alternate Reference must be entered for this renewal policy. You must amend the renewal before the renewal can be accepted.'
				SELECT @is_update_status =1
			END
		END	

	UPDATE renewal_status SET renewal_status_type_id = 1 WHERE renewal_insurance_file_cnt = @New_Insurance_File_Cnt      
END

IF @is_update_status = 0
BEGIN
Update Insurance_File SET Insurance_File_Status_ID = @REPInsuranceFileStatusId WHERE Insurance_File_Cnt =@old_insurance_file_cnt
Update IFL SET 
IFL.Insurance_File_Status_ID = NULL, 
IFL.Insurance_File_Type_Id = @Insurance_File_Type_Id,
IFL.Insurance_ref = (case when COALESCE(@new_insurance_ref, '0') = '0' then IFL.insurance_ref else @new_insurance_ref end),
IFL.cover_start_date = (case when COALESCE(@new_cover_start_date, 0) = 0 then IFL.cover_start_date else @new_cover_start_date end), 
IFL.expiry_date = (case when COALESCE(@new_expiry_date, 0) = 0 then IFL.expiry_date else @new_expiry_date end),  
IFL.renewal_date = (case when COALESCE(@new_renewal_date, 0) = 0 then IFL.renewal_date else @new_renewal_date end)  

FROM Insurance_File IFL 
WHERE  IFL.Insurance_File_cnt = @new_insurance_file_cnt 
END


