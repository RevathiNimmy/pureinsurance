EXECUTE DDLDropProcedure 'spu_Get_Previous_InsuranceFileKey'
GO

CREATE PROCEDURE spu_Get_Previous_InsuranceFileKey 
      @nInsuranceFileKey INT,
      @nPreviousInsuranceFileKey INT OUTPUT
AS  
BEGIN

	DECLARE @nInsuranceFolderCnt INT

	SELECT @nInsuranceFolderCnt = insurance_folder_cnt 
	FROM insurance_file 
	WHERE insurance_file_cnt =@nInsuranceFileKey

	SELECT @nPreviousInsuranceFileKey=Max(insurance_file_cnt) FROM insurance_file  
	Left Join insurance_file_Type ON insurance_file_Type.insurance_file_Type_id=insurance_file.insurance_file_Type_id      
	WHERE insurance_folder_cnt = @nInsuranceFolderCnt 
	AND insurance_file_cnt < @nInsuranceFileKey          
	AND Insurance_File_Type.code in ('POLICY','MTA PERM','MTACAN','MTAREINS')

END  
GO



