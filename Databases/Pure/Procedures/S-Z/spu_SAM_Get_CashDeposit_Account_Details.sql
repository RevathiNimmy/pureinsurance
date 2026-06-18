SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
EXEC DDLDropProcedure 'spu_SAM_Get_CashDeposit_Account_Details'
GO
--Start - Prakash Varghese - Tech Spec-WPR85
CREATE  PROCEDURE spu_SAM_Get_CashDeposit_Account_Details
@CashDeposit_Ref VARCHAR(30)
AS
BEGIN
	SELECT 
		CDT.CashDeposit_ID,
		CDT.Party_ID,
		CDT.Account_ID,
		AST.Code AS Account_Status
	FROM
		CashDeposit CDT
		LEFT OUTER JOIN Account ACT
			ON ACT.Account_ID=CDT.Account_ID
		LEFT OUTER JOIN AccountStatus AST
			ON AST.AccountStatus_ID=ACT.AccountStatus_ID
	WHERE
		CDT.CashDeposit_Ref=@CashDeposit_Ref
END
--End - Prakash Varghese - Tech Spec-WPR85
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
