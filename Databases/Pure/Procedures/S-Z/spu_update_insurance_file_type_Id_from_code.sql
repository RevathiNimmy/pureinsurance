SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_update_insurance_file_type_Id_from_code'
GO
CREATE PROCEDURE spu_update_insurance_file_type_Id_from_code
 @nInsuranceFileCnt INT,
 @sInsuranceFileTypeCode VARCHAR(10)
AS
BEGIN

DECLARE @InsuranceFileTypeID INT

	SELECT @InsuranceFileTypeID=insurance_file_type_id FROM Insurance_file_type 
	WHERE code=@sInsuranceFileTypeCode

	UPDATE Insurance_File set insurance_file_type_id=@InsuranceFileTypeID 
	WHERE insurance_file_cnt=@nInsuranceFileCnt
END
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO