SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_Get_Payment_Method
GO

CREATE PROCEDURE spu_Get_Payment_Method
    @insurance_file_cnt INT
AS
BEGIN

	-- If it is the replaced cancelled version of policy/ else select payment method for latest version of policy
	IF EXISTS (SELECT NULL FROM Insurance_File INF 
				WHERE INF.insurance_file_cnt = @insurance_file_cnt
				AND INF.insurance_file_type_id = (SELECT insurance_file_type_id from Insurance_File_Type WHERE code = 'MTACAN')
				AND INF.insurance_file_status_id = (SELECT insurance_file_status_id from Insurance_File_Status WHERE code = 'REP') )
		
		SELECT Payment_Method, DOPaymentTerms_id, CollectionFrequency_id 
		FROM insurance_file 
		WHERE insurance_file_cnt = @insurance_file_cnt
	ELSE
		SELECT Payment_Method, DOPaymentTerms_id, CollectionFrequency_id 
		FROM insurance_file 
		WHERE insurance_file_cnt = (SELECT max(insurance_file_cnt) FROM insurance_file 
									WHERE insurance_folder_cnt = (select insurance_folder_cnt FROM insurance_file 
																	WHERE insurance_file_cnt = @insurance_file_cnt) 
									AND (insurance_file_status_id IS NULL OR insurance_file_status_id = (SELECT insurance_file_status_id from Insurance_File_Status
																										WHERE code = 'REN') ))

END
GO

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS ON 
GO