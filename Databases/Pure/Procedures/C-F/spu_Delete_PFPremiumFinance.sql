SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXEC DDLDropProcedure 'spu_Delete_PFPremiumFinance'
GO

CREATE PROCEDURE spu_Delete_PFPremiumFinance
    @nInsurance_file_cnt INT
AS
BEGIN
	IF EXISTS(SELECT NULL FROM PFPremiumFinance WHERE insurance_file_cnt = @ninsurance_file_cnt)
	BEGIN
		DELETE PFPremiumFinance
		WHERE  insurance_file_cnt = @ninsurance_file_cnt
	END
END
GO
