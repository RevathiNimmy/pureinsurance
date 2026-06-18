SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_Get_CDReceipts_For_Allocation'
GO

--Start - Renuka - (WPR85_Cash_Deposit_Process)
CREATE  PROCEDURE spu_Get_CDReceipts_For_Allocation
	@CashDeposit_ID INT,
	@Total_Premium MONEY,
	@Is_PrePayment TINYINT=0,
	@Cover_Start_Date DATETIME=NULL,
	@Policy_Issue_Date DATETIME=NULL ,
	@Retrieve_Reciepts TINYINT=1,
	@Is_Valid TINYINT OUTPUT
AS 
BEGIN

	DECLARE @AvailableReceipts TABLE (
										AvailableReceipts_ID INT IDENTITY,
										TransDetail_ID INT
									   )
	DECLARE @SelectedReceipts TABLE (
										 TransDetail_ID INT,
										 Amount_Allocated MONEY
										)

	DECLARE @TotalRowCnt AS INT
	DECLARE @CurrentRowCnt AS INT
	DECLARE @Account_ID AS INT
	DECLARE @IsSelected TINYINT
	DECLARE @TransDetail_ID AS INT

	DECLARE @MinimumRunningBalance AS MONEY
	DECLARE @OutstandingAmount AS MONEY
	DECLARE @AmountAllocated AS MONEY
	DECLARE @CurrentAmount AS MONEY
	DECLARE @RemainingAmount AS MONEY
	
	DECLARE @ReceiptDocumentType_ID AS INT

	--Get the Id for receipts to avoid joining them in subsequent queries which run multiple times
	SELECT 
		@ReceiptDocumentType_ID=DocumentType_ID
	FROM 
		DocumentType
	WHERE
		CODE='SRP'

	--Get the Account ID and Single policy status
	SELECT 
		@Account_ID=Account_ID
	FROM 
		CashDeposit
	WHERE
		CashDeposit_ID=@CashDeposit_ID
		

	--Get the receipts which meets the basic filter conditions to table variable.
	INSERT INTO 
		@AvailableReceipts (
							TransDetail_ID
						   )
	SELECT 
		TDL.TransDetail_ID
	FROM
		TransDetail TDL
		INNER JOIN Document DOC
			ON DOC.Document_ID=TDL.Document_ID
	WHERE
		TDL.Account_ID=@Account_ID
		AND DOC.DocumentType_ID=@ReceiptDocumentType_ID
		AND (TDL.Outstanding_Amount*-1)>0
		AND (@Is_PrePayment=0 OR (  
								  @Is_PrePayment=1  
								  AND (  
									   (DATEDIFF(dd,@Cover_Start_Date,@Policy_Issue_Date)>=0  
										AND DATEDIFF(dd,TDL.Accounting_Date,@Cover_Start_Date)>=0
									   )  
									   OR (  
											DATEDIFF(dd,@Cover_Start_Date,@Policy_Issue_Date)<0  
											AND DATEDIFF(dd,TDL.Accounting_Date,@Policy_Issue_Date)>=0  
										  )  
									  )  
								 )  
			 )   

	SET @TotalRowCnt=@@ROWCOUNT
	SET @CurrentRowCnt=1
	
	SET @RemainingAmount =@Total_Premium
	SET @Is_Valid=0

	WHILE @CurrentRowCnt<=@TotalRowCnt AND @RemainingAmount>0
	BEGIN
		SET @IsSelected=0
		SET @MinimumRunningBalance=0
		SET @OutstandingAmount=0

		SELECT 
			@TransDetail_ID=TransDetail_ID
		FROM
			@AvailableReceipts
		WHERE
			AvailableReceipts_ID=@CurrentRowCnt
		
		--Get the current outstanding amount of transaction
		SELECT 
			@OutstandingAmount=OutStanding_Amount*-1
		FROM
			TransDetail 
		WHERE TransDetail_ID=@TransDetail_ID

		--If prepayment get the minimun running balance availabe between cover start date and policy issue date
		IF @Is_PrePayment=1 
		BEGIN

			SELECT
				@MinimumRunningBalance= MIN(ADT.New_OS_Base_Amount)*-1
			FROM		
				AllocationDetail ADT
				INNER JOIN TransDetail TDT
					ON TDT.TransDetail_ID=ADT.TransDetail_ID
				INNER JOIN Document DOC
					ON DOC.Document_ID=TDT.Document_ID
			WHERE
				TDT.TransDetail_ID=@TransDetail_ID
				AND DOC.DocumentType_ID=@ReceiptDocumentType_ID
				AND (
					 (DATEDIFF(dd,@Cover_Start_Date,@Policy_Issue_Date)>=0 
					  AND DATEDIFF(dd,@Cover_Start_Date,ADT.Accounting_Date)>= 0
					  AND DATEDIFF(dd,ADT.Accounting_Date,@Policy_Issue_Date)>=0
					 )
					 OR (
						 DATEDIFF(dd,ADT.Accounting_Date,@Cover_Start_Date )>=0
						 AND DATEDIFF(dd,@Policy_Issue_Date,ADT.Accounting_Date)>=0
						)
					)
			--If @MinimumRunningBalance=NULL then no entry there in allocation detail. 
			--So set the minimum balance to outstanding amount
			IF @MinimumRunningBalance IS NULL
				SET @MinimumRunningBalance=ISNULL(@OutstandingAmount,0)
		END
		ELSE
		BEGIN
			--Prepayment is off. set minumum balance to outstanding amount
			SET @MinimumRunningBalance=ISNULL(@OutstandingAmount,0) 
		END

		IF ISNULL(@MinimumRunningBalance,0) >0
			SET @IsSelected=1
		ELSE 
			SET @IsSelected=0
		
		
		IF @IsSelected=1
		BEGIN
			SELECT 
				@AmountAllocated=CASE 
									 WHEN @MinimumRunningBalance<=@OutstandingAmount THEN
										 @MinimumRunningBalance
									 ELSE
										 @OutstandingAmount
									 END
			SELECT
				@AmountAllocated=CASE 
									 WHEN @RemainingAmount<=@AmountAllocated THEN
										 @RemainingAmount
									 ELSE
										 @AmountAllocated
									 END

			SET @RemainingAmount=@RemainingAmount-@AmountAllocated

 
			INSERT INTO
				@SelectedReceipts (
									TransDetail_ID,
									Amount_Allocated
								  )
			VALUES( 
					@TransDetail_ID,
					@AmountAllocated
				  )
		END

		SET @CurrentRowCnt=@CurrentRowCnt+1
	END
			
	IF @RemainingAmount>0
		SELECT 
			@Is_Valid=0
	ELSE
	BEGIN
		SET @Is_Valid=1
		IF @Retrieve_Reciepts=1
		BEGIN
			SELECT 
				@Account_ID AS Account_ID,
				TransDetail_ID,
				Amount_Allocated
			FROM
				@SelectedReceipts
			ORDER BY TransDetail_ID
		END
	END
END
--End - Renuka - (WPR85_Cash_Deposit_Process)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
