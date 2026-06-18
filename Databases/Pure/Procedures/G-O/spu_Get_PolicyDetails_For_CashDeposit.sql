SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_Get_PolicyDetails_For_CashDeposit'
GO

--Start - Renuka - (WPR85_Cash_Deposit_Process)
CREATE  PROCEDURE spu_Get_PolicyDetails_For_CashDeposit
	@Policy_ID INT
AS 
BEGIN

	SELECT 
		IFI.Insurance_File_Cnt,
		IFI.Insurance_Folder_Cnt,
		IFI.Product_ID,
		BTP.Code AS Business_Type,
		PTC.Party_Cnt AS Party_Cnt,
		PTC.ShortName AS Party_Code,
		PTC.Resolved_Name AS Party_Name,
		PTA.Party_Cnt AS Agent_Cnt,
		PTA.ShortName AS Agent_Code,
		PTA.Resolved_Name AS Agent_Name,
		AGT.Code AS Agent_Type
	FROM
		Insurance_File IFI
		INNER JOIN Business_Type BTP
			ON BTP.Business_Type_ID=IFI.Business_Type_ID
		INNER JOIN Party PTC
			ON PTC.Party_Cnt=IFI.Insured_Cnt
		LEFT JOIN Party PTA
			ON PTA.Party_Cnt=IFI.Lead_Agent_Cnt
		LEFT JOIN Party_Agent PAG
			ON PAG.Party_Cnt=PTA.Party_Cnt
		LEFT JOIN Party_Agent_Type AGT
			ON AGT.Party_Agent_Type_ID=PAG.Party_Agent_Type_ID
	WHERE
		IFI.Insurance_File_Cnt=@Policy_ID

END
--End - Renuka - (WPR85_Cash_Deposit_Process)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
