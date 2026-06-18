SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXEC DDLDropProcedure 'spu_ACT_Get_Policy_Status_For_MediaType_Status_Maintenance'
GO

--Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
CREATE  PROCEDURE spu_ACT_Get_Policy_Status_For_MediaType_Status_Maintenance
	@Insurance_File_Cnt INT,
	@IsPolicyCancelled TINYINT OUTPUT,
	@IsClaimPaymentInitiated TINYINT OUTPUT
AS
BEGIN
	SET @IsPolicyCancelled=0
	SET @IsClaimPaymentInitiated=0

	IF EXISTS (
				SELECT
					1
				FROM 
					Insurance_File IFI
					INNER JOIN Insurance_File_Status IFS
						ON IFS.Insurance_File_Status_Id=IFI.Insurance_File_Status_Id
				WHERE
					IFI.Insurance_File_Cnt=@Insurance_File_Cnt
					AND IFS.CODE='CAN'
			  )
		SET @IsPolicyCancelled=1

	IF EXISTS (
				SELECT
					1
				FROM
					Claim_Payment CLP
					INNER JOIN Claim CLM
						ON CLM.Claim_ID =CLP.Claim_ID
				WHERE
					CLM.Policy_Id=@Insurance_File_Cnt
			  )
		SET @IsClaimPaymentInitiated=1
END
--End - Sankar - (WPRvb64 Media Type Status) - Paralleling
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

