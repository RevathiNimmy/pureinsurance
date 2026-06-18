SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Is_Instalment'
GO

CREATE PROCEDURE spu_Is_Instalment 
	@InsuranceFileCnt int
AS

SELECT insurance_file_cnt FROM PFPremiumFinance WHERE insurance_file_cnt = @InsuranceFileCnt
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
