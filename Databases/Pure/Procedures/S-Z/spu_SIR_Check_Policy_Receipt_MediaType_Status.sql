SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_SIR_Check_Policy_Receipt_MediaType_Status'
GO
--Start - Prakash Varghese - Tech Spec-WPRvb64 Media Type Status
CREATE  PROCEDURE spu_SIR_Check_Policy_Receipt_MediaType_Status
	@Insurance_File_Cnt INT,
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
					LEFT JOIN TransDetail TDL  
      						ON TDL.Transdetail_id=CLI.Transdetail_id
					LEFT JOIN Insurance_File_Payment_Details IFPD  
   						ON TDL.Transdetail_id=IFPD.Transdetail_id
 					LEFT JOIN Document DMT  
						ON DMT.Document_Id=TDL.Document_Id
       					INNER JOIN DocumentType DTP  
     						ON DTP.DocumentType_Id=DMT.DocumentType_Id  
      						 AND DTP.Code ='SRP'
					INNER JOIN Insurance_File IFI
      						ON IFI.Insurance_File_Cnt=IFPD.Insurance_File_Cnt  
				WHERE
					IFI.Insurance_Ref =(SELECT
											Insurance_Ref
										FROM
											Insurance_File
										WHERE
											Insurance_File_Cnt=@Insurance_File_Cnt
									   )	
					AND CLI.MediaType_Status_Id IS NOT NULL 
					AND MTS.CODE='SRPS' 
			)
		SET @IsValid=0
	ELSE
		SET @IsValid=1
END
--End - Prakash Varghese - Tech Spec-WPRvb64 Media Type Status
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


