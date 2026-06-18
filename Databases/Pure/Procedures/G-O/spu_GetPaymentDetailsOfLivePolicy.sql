
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_GetPaymentDetailsOfLivePolicy'
GO

CREATE PROCEDURE spu_GetPaymentDetailsOfLivePolicy
	@insurance_file_cnt INT
AS
BEGIN
	DECLARE @insurance_folder_cnt INT
	DECLARE @iPolicy_Insurance_File_Type_ID INT
	DECLARE @iMTAPerm_Insurance_File_Type_ID INT
	DECLARE @iINS_FILE_CNT INT
	DECLARE @IsInstalment INT

	SELECT @insurance_folder_cnt = insurance_folder_cnt FROM insurance_file WHERE insurance_file_cnt = @insurance_file_cnt
	
	SELECT @iPolicy_Insurance_File_Type_ID = insurance_file_type_id
	                           FROM Insurance_File_Type
	                           WHERE code = 'POLICY'
	
	SELECT @iMTAPerm_Insurance_File_Type_ID = insurance_file_type_id
	                           FROM Insurance_File_Type
	                           WHERE code = 'MTA PERM'
	
	SELECT @iINS_FILE_CNT = (SELECT MAX(insurance_file_cnt)
				FROM insurance_file
				WHERE insurance_folder_cnt = @insurance_folder_cnt
				AND (insurance_file_type_id = @iPolicy_Insurance_File_Type_ID OR
				     insurance_file_type_id = @iMTAPerm_Insurance_File_Type_ID)
				AND insurance_file_Status_id is NULL)
	
	SELECT @IsInstalment = COUNT(*) FROM pfpremiumfinance WHERE insurance_file_cnt=@iINS_FILE_CNT
	
	IF @IsInstalment > 0 
		SELECT CLI.party_bank_id, mediatype_id FROM cashlistitem CLI
				INNER JOIN CashListItem_Instalments CLIIN ON CLIIN.CashListItem_Id = CLI.CashListItem_Id
				INNER JOIN PFInstalments PFI ON PFI.PFInstalments_ID = CLIIN.PFInstalments_ID
				INNER JOIN PFPremiumFinance PFP ON PFP.PFPrem_Finance_cnt = PFI.PFPrem_Finance_cnt
				INNER JOIN Insurance_File INF ON PFP.Insurance_File_cnt = INF.Insurance_File_Cnt
		WHERE INF.insurance_file_cnt=@iINS_FILE_CNT
	ELSE
		SELECT cli.party_bank_id, mediatype_id FROM cashlistitem CLI
			INNER JOIN AllocationDetail AD ON AD.CashListItem_Id = CLI.CashListItem_Id
			INNER JOIN TransDetail TD ON TD.TransDetail_id =AD.TransDetail_id
			INNER JOIN Document D ON D.Document_id=TD.Document_id
			INNER JOIN Insurance_File INF ON D.Insurance_File_Cnt = INF.Insurance_File_Cnt 
		WHERE INF.insurance_file_cnt=@iINS_FILE_CNT
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO