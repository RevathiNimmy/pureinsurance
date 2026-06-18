SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_DeletePlanForOneInsFile'
GO


CREATE PROCEDURE spu_DeletePlanForOneInsFile
		@insurance_file_cnt int
AS

DELETE FROM PFInstalments where pfprem_finance_cnt IN 
	(SELECT pfprem_finance_cnt FROM PFPremiumFinance 
	 WHERE Insurance_File_Cnt = @insurance_file_cnt)

DELETE FROM PFPremiumFinance
WHERE insurance_file_cnt = @insurance_file_cnt


GO
