

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_CheckCDUsedForMultiPolicy'
GO

CREATE PROCEDURE spu_CheckCDUsedForMultiPolicy 
	@CashDeposit_ID INT,
	@IsRepeated TINYINT OUTPUT
AS

DECLARE @CancelledPolicyStatus_ID INT
DECLARE @CancelledPolicyType_ID INT
DECLARE @Insurance_Folder_Cnt INT
DECLARE @COUNTER INT

BEGIN
	SET @IsRepeated = 0
	SET @Counter = 0

	SELECT @CancelledPolicyStatus_ID=Insurance_File_Status_ID
	FROM Insurance_File_Status WHERE CODE='CAN'

	SELECT @CancelledPolicyType_ID=Insurance_File_Type_ID
	FROM Insurance_File_Type WHERE CODE='MTACAN'

	DECLARE POLICIES_CUR CURSOR FOR 
		SELECT DISTINCT Insurance_Folder_Cnt FROM Insurance_File IFI
			INNER JOIN CashDeposit_Policy_Link CDPL ON IFI.Insurance_File_Cnt = CDPL.Insurance_File_Cnt
		WHERE CDPL.CashDeposit_ID = @CashDeposit_ID

	OPEN POLICIES_CUR;

	FETCH NEXT FROM POLICIES_CUR INTO @Insurance_Folder_Cnt;
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF NOT EXISTS (SELECT 
			1
		FROM 
			Insurance_File IFI
		WHERE
			IFI.Insurance_Folder_Cnt = @Insurance_Folder_Cnt
			AND IFI.Insurance_File_Type_ID=@CancelledPolicyType_ID
			AND IFI.Insurance_File_Status_ID=@CancelledPolicyStatus_ID
			AND DATEDIFF(dd,IFI.Inception_Date_Tpi,IFI.Cover_Start_Date) = 0)
				SET @Counter = @Counter + 1

		FETCH NEXT FROM POLICIES_CUR INTO @Insurance_Folder_Cnt
	END;
	CLOSE POLICIES_CUR;
	DEALLOCATE POLICIES_CUR;

	IF @Counter > 1 
		SET @IsRepeated = 1
	
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 


