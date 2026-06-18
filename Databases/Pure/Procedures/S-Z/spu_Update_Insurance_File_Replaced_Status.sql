SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_Update_Insurance_File_Replaced_Status'
GO

CREATE PROCEDURE spu_Update_Insurance_File_Replaced_Status
	@nInsuranceFileCnt int,
	@nCancelled tinyint
AS  
BEGIN  
	Update insurance_file
		Set out_of_sequence_replaced = @ncancelled
				WHERE insurance_file_cnt = @nInsuranceFileCnt
END  
