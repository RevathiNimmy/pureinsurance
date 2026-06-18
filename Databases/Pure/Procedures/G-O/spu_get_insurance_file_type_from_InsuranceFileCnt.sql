SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_get_insurance_file_type_from_InsuranceFileCnt'
GO

CREATE PROCEDURE spu_get_insurance_file_type_from_InsuranceFileCnt
 @nInsuranceFileCnt INT
AS
BEGIN
	SELECT IFT.code,Ins_file.insurance_file_type_id,cover_start_date  FROM Insurance_File Ins_file JOIN Insurance_File_Type IFT
	ON IFT.insurance_file_type_id=Ins_file.insurance_file_type_id 
	WHERE Ins_file.insurance_file_cnt=@nInsuranceFileCnt
END
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO
