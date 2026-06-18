SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_Act_Get_InsuranceFileCnt'
GO

CREATE PROCEDURE spu_Act_Get_InsuranceFileCnt
	@premiumfinance_cnt INT,
	@premiumfinance_version INT
AS
SELECT insurance_file_cnt 
	FROM pfpremiumfinance 
	WHERE pfprem_finance_cnt=@premiumfinance_cnt AND pfprem_finance_version=@premiumfinance_version
GO