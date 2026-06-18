SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXEC DDLDropProcedure 'spu_CLM_Check_Policy_Receipt_MediaType_Status'
GO

--Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
CREATE  PROCEDURE spu_CLM_Check_Policy_Receipt_MediaType_Status
	@Insurance_File_Cnt INT,
	@Claim_Loss_Date DATETIME,
	@IsValid SMALLINT OUTPUT
AS
BEGIN

	IF EXISTS(
			 	SELECT 
					1
				FROM 
					CashListItem CLI
					INNER JOIN CashList CLS
						ON CLS.CashList_Id=CLI.CashList_Id
					INNER JOIN CashListType CLT
						ON CLT.CashListType_Id=CLS.CashListType_id
						AND CLT.Code='R'
					INNER JOIN MediaType MTP
						ON MTP.MediaType_Id=CLI.MediaType_Id
					LEFT JOIN MediaType_Status MTS
						ON MTS.MediaType_Status_Id=CLI.MediaType_Status_Id
					INNER JOIN AllocationDetail ADT
						ON ADT.CashListItem_Id=CLI.CashListItem_Id
					INNER JOIN AllocationDetail ADT1
						ON ADT1.Allocation_Id=ADT.Allocation_Id
					INNER JOIN DocumentType DTP
						ON DTP.DocumentType_Id=ADT1.DocumentType_Id
						AND DTP.Code='SRP'
					INNER JOIN TransDetail TDL
						ON TDL.TransDetail_Id=ADT.TransDetail_ID
					INNER JOIN Document DMT
						ON DMT.Document_Id=TDL.Document_Id
					INNER JOIN Insurance_File IFI
						ON IFI.Insurance_File_Cnt=DMT.Insurance_File_Cnt
				WHERE
					IFI.Insurance_Ref =(SELECT
											Insurance_Ref
										FROM
											Insurance_File
										WHERE
											Insurance_File_Cnt=@Insurance_File_Cnt
									   )	
					AND CLI.MediaType_Status_Id IS NOT NULL 
					AND MTS.CODE<>'SRPC'
					AND IFI.Cover_Start_Date<=@Claim_Loss_Date	  
			)
		SET @IsValid=0
	ELSE
		SET @IsValid=1
END
--End - Sankar - (WPRvb64 Media Type Status) - Paralleling
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

