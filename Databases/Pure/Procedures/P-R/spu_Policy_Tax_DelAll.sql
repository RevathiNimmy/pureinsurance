EXEC DDLDropProcedure 'spu_Policy_Tax_DelAll'
GO

CREATE PROCEDURE spu_Policy_Tax_DelAll
	@InsuranceFileCnt INT
AS BEGIN

DELETE  Tax_Calculation
FROM	Tax_Calculation
WHERE   insurance_file_cnt = @InsuranceFileCnt

END
GO