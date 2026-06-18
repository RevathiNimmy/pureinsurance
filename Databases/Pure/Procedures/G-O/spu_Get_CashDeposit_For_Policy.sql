SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_Get_CashDeposit_For_Policy'
GO

--Start - Renuka - (WPR85_Cash_Deposit_Process)
CREATE  PROCEDURE spu_Get_CashDeposit_For_Policy
	@Party_Cnt INT,
	@Product_ID INT,
	@Branch_ID INT,
	@Total_Premium MONEY,
	@Policy_ID INT,
	@Is_PrePayment TINYINT=0,
	@Cover_Start_Date DATETIME=NULL,
	@Policy_Issue_Date DATETIME=NULL 
AS 
BEGIN
	DECLARE @SelectedCDs TABLE (
									SelectedCDs_ID INT IDENTITY,
									CashDeposit_ID INT,
									Account_ID INT,
									Is_Single_Policy INT
								  )
	DECLARE @AmountDetails TABLE (
									CashDeposit_ID INT,
									Amount MONEY,
									Available_Balance MONEY
									)

	DECLARE @TotalRowCnt AS INT
	DECLARE @CurrentRowCnt AS INT
	DECLARE @CashDeposit_ID AS INT
	DECLARE @Account_ID AS INT
	DECLARE @Is_Single_Policy AS TINYINT
	DECLARE @IsSelected TINYINT

	DECLARE @ReceiptDocumentType_ID AS INT
	DECLARE @CancelledPolicyStatus_ID AS INT
	DECLARE @CancelledPolicyType_ID AS INT

	--Get the Ids for receipts, cancelled policy status, cancelled policy type
	--to avoid joining them in subsequent queries which run multiple times
	SELECT 
		@ReceiptDocumentType_ID=DocumentType_ID
	FROM 
		DocumentType
	WHERE
		CODE='SRP'

	SELECT 
		@CancelledPolicyStatus_ID=Insurance_File_Status_ID
	FROM 
		Insurance_File_Status
	WHERE
		CODE='CAN'

	SELECT 
		@CancelledPolicyType_ID=Insurance_File_Type_ID
	FROM
		Insurance_File_Type
	WHERE
		CODE='MTACAN'

	--Get the cashdeposits which meets the basic filter conditions to table variable.
	INSERT INTO 
		@SelectedCDs (
					  CashDeposit_ID,
					  Account_ID,
					  Is_Single_Policy
					 )
	SELECT DISTINCT
		CDT.CashDeposit_ID,
		CDT.Account_ID,
		CDT.Is_SinglePolicy
	FROM
		CashDeposit CDT
		INNER JOIN CashDeposit_Product_Link CPL
			ON CPL.CashDeposit_ID=CDT.CashDeposit_ID
		INNER JOIN CashDeposit_Branch_Link CBL
			ON CBL.CashDeposit_ID=CDT.CashDeposit_ID
	WHERE
		CDT.Party_ID= @Party_Cnt
		AND CBL.Branch_ID=@Branch_ID
		AND CPL.Product_ID=@Product_ID
		AND CDT.Is_Deleted<>1
		AND (@Is_PrePayment=0 OR (  
								  @Is_PrePayment=1  
								  AND (  
									   (DATEDIFF(dd,@Cover_Start_Date,@Policy_Issue_Date)>=0  
										AND DATEDIFF(dd,CDT.Date_Created,@Cover_Start_Date)>=0  
									   )  
									   OR (  
											DATEDIFF(dd,@Cover_Start_Date,@Policy_Issue_Date)<0  
											AND DATEDIFF(dd,CDT.Date_Created,@Policy_Issue_Date)>=0
										  )  
									  )  
								 )  
			 )   

	SET @TotalRowCnt=@@ROWCOUNT
	SET @CurrentRowCnt=1

	WHILE @CurrentRowCnt<=@TotalRowCnt
	BEGIN
		SET @IsSelected=1
		SELECT 
			@CashDeposit_ID=CashDeposit_ID,
			@Account_ID=Account_ID,
			@Is_Single_Policy=Is_Single_Policy
		FROM
			@SelectedCDs
		WHERE
			SelectedCDs_ID=@CurrentRowCnt

		--Perform single policy validation
		IF  @Is_Single_Policy=1
		BEGIN
			--Check This CD has been used before
			IF EXISTS (
						SELECT
							1
						FROM
							CashDeposit_Policy_Link
						WHERE
							CashDeposit_ID=@CashDeposit_ID
					  )
			BEGIN
				--This CD is used before, Check this policy version belongs to the same policy
				IF EXISTS (
							SELECT 
								1
							FROM 
								CashDeposit_Policy_Link CPL
								INNER JOIN Insurance_File IFI
									ON IFI.Insurance_File_Cnt=CPL.Insurance_File_Cnt
							WHERE
								CPL.CashDeposit_ID=@CashDeposit_ID
								AND IFI.Insurance_Folder_Cnt=( SELECT
																   IFI1.Insurance_Folder_Cnt
															   FROM
																   Insurance_File IFI1
															   WHERE
																   IFI1.Insurance_File_Cnt=@Policy_ID
															 )
							)
				BEGIN
					SET @IsSelected=1
				END
				ELSE
				BEGIN
					--CD has been used before for a different policy. 
					--Now check that policy has been cancelled from the inception date
					IF EXISTS (
								SELECT 
									1
								FROM
									CashDeposit_Policy_Link CPL
									INNER JOIN Insurance_File IFI
										ON IFI.Insurance_File_Cnt=CPL.Insurance_File_Cnt
								WHERE 
									CPL.CashDeposit_ID=@CashDeposit_ID
									AND IFI.Insurance_File_Status_ID=@CancelledPolicyStatus_ID
									AND EXISTS (
												SELECT 
													1
												FROM 
													Insurance_File IFI1
												WHERE
													IFI1.Insurance_Folder_Cnt=IFI.Insurance_Folder_Cnt
													AND IFI1.Insurance_File_Type_ID=@CancelledPolicyType_ID
													AND DATEDIFF(dd,IFI1.Inception_Date_Tpi,IFI1.Cover_Start_Date)=0
											    )
							   )
					BEGIN
						SET @IsSelected=1
					END
					ELSE
					BEGIN
						SET @IsSelected=0
					END				
				END																					
			END
			ELSE
			BEGIN
				--This CD has not been used before, so select it 	
				SET @IsSelected=1
			END
		END

		IF @IsSelected=1
		BEGIN

			INSERT INTO @AmountDetails (CashDeposit_ID,
										Amount,
										Available_Balance
									   )
			SELECT 
					@CashDeposit_ID,
					(SELECT
						 SUM(amount)*-1
					 FROM 
						 TransDetail TDT
						 INNER JOIN Document DOC
							 ON DOC.Document_ID=TDT.Document_ID
					 WHERE
						 TDT.Account_ID=@Account_ID
						 AND DOC.DocumentType_ID=@ReceiptDocumentType_ID
					),
					(SELECT
						SUM(outstanding_amount)*-1
					 FROM 
						 TransDetail TDT
						 INNER JOIN Document DOC
				   			 ON DOC.Document_ID=TDT.Document_ID
					 WHERE
						 TDT.Account_ID=@Account_ID
						 AND DOC.DocumentType_ID=@ReceiptDocumentType_ID
					) 
		END

		SET @CurrentRowCnt=@CurrentRowCnt+1
	END

	SELECT 
		CDT.CashDeposit_ID,
		CDT.Account_ID,
		CDT.Party_ID,
		CDT.CashDeposit_Ref,
		ADT.Amount,
		ADT.Available_Balance,
		CDT.Date_Created
	FROM 
		CashDeposit CDT
		INNER JOIN @AmountDetails ADT
			ON ADT.CashDeposit_ID=CDT.CashDeposit_ID

END
--End - Renuka - (WPR85_Cash_Deposit_Process)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
