
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
EXEC DDLDropProcedure 'spu_Get_CD_Account_Details_For_Policy'
GO
--Start - Renuka - (WPR85_Cash_Deposit_Process)
CREATE  PROCEDURE spu_Get_CD_Account_Details_For_Policy
@Insurance_File_Cnt INTEGER
AS
BEGIN
	SELECT 
		IFI.Balance_Type,
		ACT.Account_ID AS CD_Account_ID,
		CDT.Party_ID AS Base_Account_Key,
		AST.Code AS Account_Status,
		PAT.Code AS Policy_Agent_Type
	FROM
		Insurance_File IFI
		LEFT JOIN CashDeposit CDT
			ON CDT.Account_ID=IFI.Intermediary_Agent_Account_ID
		LEFT OUTER JOIN Account ACT
			ON ACT.Account_ID=CDT.Account_ID
		LEFT OUTER JOIN AccountStatus AST
			ON AST.AccountStatus_ID=ACT.AccountStatus_ID
		LEFT OUTER JOIN Party_Agent PAG
			ON PAG.Party_Cnt=IFI.Lead_Agent_Cnt
		LEFT OUTER JOIN Party_Agent_Type PAT
			ON PAT.Party_Agent_Type_ID=PAG.Party_Agent_Type_ID
	WHERE
		IFI.Insurance_File_Cnt=@Insurance_File_Cnt
END
--End - Renuka - (WPR85_Cash_Deposit_Process)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


