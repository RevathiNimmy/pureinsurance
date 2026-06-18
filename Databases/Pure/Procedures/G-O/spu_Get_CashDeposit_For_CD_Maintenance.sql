SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_Get_CashDeposit_For_CD_Maintenance'
GO

--Start - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
CREATE  PROCEDURE spu_Get_CashDeposit_For_CD_Maintenance
	@Party_Code VARCHAR(20),
	@CashDeposit_Ref VARCHAR(30) = NULL,
	@Bank_Code VARCHAR(10)=NULL,
	@MaxRowsToFetch INT=500
AS
BEGIN
	
	DECLARE @TotalBranchCnt INT
	DECLARE @TotalProductCnt INT
	DECLARE @TotalBankCnt INT
	DECLARE @TotalRowCnt INT
	DECLARE @CurrentRowCnt INT
	DECLARE @CurrentCD_ID INT
	DECLARE @Bank_Name VARCHAR(50)

	DECLARE @SelectedCDs TABLE (
								SelectedCds_ID INT IDENTITY,
								CashDeposit_ID INT,
								Bank_Name VARCHAR(50)
								)

	INSERT INTO @SelectedCDs (
							  CashDeposit_ID
							 )
	SELECT DISTINCT
		CashDeposit_ID
	FROM 
		CashDeposit CDT
		INNER JOIN Party PTY
			ON PTY.Party_Cnt=CDT.Party_ID
		INNER JOIN Account ACT
			ON ACT.Account_ID=CDT.Account_ID
	WHERE
		PTY.ShortName LIKE LTRIM(RTRIM(@Party_Code))
		AND (@CashDeposit_Ref IS NULL 
			 OR CDT.CashDeposit_Ref LIKE LTRIM(RTRIM(@CashDeposit_Ref))
			)
		AND (@Bank_Code IS NULL
			 OR @Bank_Code IN (SELECT DISTINCT
								   CLB.Code
							   FROM 
								  CashListItem_Bank CLB
								  INNER JOIN CashListItem CLI
									  ON CLI.CashlistItem_Bank_Id=CLB.CashListItem_Bank_Id
								  INNER JOIN TransDetail TDL
									  ON TDL.TransDetail_ID=CLI.TransDetail_ID
								  INNER JOIN Document DOC
									  ON DOC.Document_ID=TDL.Document_ID
								  INNER JOIN DocumentType DMT
									  ON DMT.DocumentType_ID=DOC.DocumentType_ID
								  INNER JOIN CashDeposit CDT1
									  ON CDT1.Account_ID=TDL.Account_ID
								  WHERE
									  DMT.Code='SRP'	
									  AND CDT1.CashDeposit_ID=CDT.CashDeposit_ID
								)	
			)

	SET @TotalRowCnt=@@ROWCOUNT
	SET @CurrentRowCnt=1

	WHILE @CurrentRowCnt<=@TotalRowCnt
	BEGIN

		DECLARE @Banks TABLE (
							  Banks_ID INT IDENTITY,
							  Bank_Name VARCHAR(50)
							 )
								
		SELECT 
			@CurrentCD_ID=CashDeposit_ID
		FROM
			@SelectedCDs
		WHERE
			SelectedCDs_ID=@CurrentRowCnt

		INSERT INTO
			@Banks (Bank_Name)
		SELECT DISTINCT
			CLB.Description
		FROM
			CashListItem_Bank CLB
			INNER JOIN CashListItem CLI
				ON CLI.CashlistItem_Bank_Id=CLB.CashListItem_Bank_Id
			INNER JOIN TransDetail TDL
				ON TDL.TransDetail_ID=CLI.TransDetail_ID
			INNER JOIN Document DOC
				ON DOC.Document_ID=TDL.Document_ID
			INNER JOIN DocumentType DMT
				ON DMT.DocumentType_ID=DOC.DocumentType_ID
			INNER JOIN CashDeposit CDT
				ON CDT.Account_ID=TDL.Account_ID
			WHERE
				DMT.Code='SRP'	
				AND CDT.CashDeposit_ID=@CurrentCD_ID

		SET @TotalBankCnt=@@ROWCOUNT

		SELECT 
			@Bank_Name= CASE @TotalBankCnt
								  WHEN 0 THEN
									  NULL
								  WHEN 1 THEN
									  (SELECT TOP 1 
										   Bank_Name
									   FROM @Banks
									  )
								  ELSE
									  'MULTIPLE'
								  END
		
		UPDATE @SelectedCds
			SET Bank_Name=@Bank_Name
		WHERE
			SelectedCds_ID=@CurrentRowCnt
	
		SET @CurrentRowCnt=@CurrentRowCnt+1
	END

	SELECT
		@TotalBranchCnt=COUNT(Source_ID)
	FROM
		Source
	WHERE 
		Is_Deleted<>1

	SELECT
		@TotalProductCnt=COUNT(Product_ID)
	FROM
		Product
	WHERE
		Is_Deleted<>1

	SET NOCOUNT ON
	SET ROWCOUNT @MaxRowsToFetch

	SELECT DISTINCT
		CDT.CashDeposit_ID,
		CDT.Account_ID,
		CDT.Party_ID,
		SCD.Bank_Name,
		CDT.CashDeposit_Ref,
		(SELECT
			 SUM(amount)*-1
		 FROM 
			 TransDetail TDT
			 INNER JOIN Document DOC
				 ON DOC.Document_ID=TDT.Document_ID
			 INNER JOIN DocumentType DMT
				 ON DMT.DocumentType_ID=DOC.DocumentType_ID
			 WHERE
				 TDT.Account_ID=CDT.Account_ID
				 AND DMT.Code='SRP'			 
		)AS Amount,
		(SELECT
			 SUM(outstanding_amount)*-1
		 FROM 
			 TransDetail TDT
			 INNER JOIN Document DOC
				 ON DOC.Document_ID=TDT.Document_ID
			 INNER JOIN DocumentType DMT
				 ON DMT.DocumentType_ID=DOC.DocumentType_ID
			 WHERE
				 TDT.Account_ID=CDT.Account_ID
				 AND DMT.Code='SRP'	
		)AS Available_Balance,
		PTY.Resolved_Name AS Party_Name,
		CASE (  SELECT 
				COUNT(CPL.Product_ID) 
			FROM 
				CashDeposit_Product_Link CPL
				INNER JOIN Product PDT
					ON PDT.Product_ID=CPL.Product_ID 
			WHERE 
				CPL.CashDeposit_ID=CDT.CashDeposit_ID
				AND PDT.Is_Deleted<>1
		 )
		 
		WHEN 0 THEN
			NULL
		WHEN 1 THEN
			(SELECT TOP 1 
				 Description
			 FROM 
				 Product PDT 
				 INNER JOIN CashDeposit_Product_Link CPL 
					 ON CPL.Product_ID=PDT.Product_ID
			 WHERE 
				 CPL.CashDeposit_ID=CDT.CashDeposit_ID
			)
		WHEN @TotalProductCnt THEN
			'ALL'
		ELSE
			'MULTIPLE'
		END AS Product,
	CASE (	SELECT 
				COUNT(CBL.Branch_ID) 
			FROM 
				CashDeposit_Branch_Link CBL
				INNER JOIN Source SRC
					ON SRC.Source_ID=CBL.Branch_ID 
			WHERE 
				CBL.CashDeposit_ID=CDT.CashDeposit_ID
				AND SRC.Is_Deleted<>1
		 )
		WHEN 0 THEN
			NULL
		WHEN 1 THEN
			(
			 SELECT TOP 1 
		 		 Description
			 FROM 
				 Source SRC 
				 INNER JOIN CashDeposit_Branch_Link CBL 
					 ON CBL.Branch_ID=SRC.Source_ID 
			 WHERE 
				 CBL.CashDeposit_ID=CDT.CashDeposit_ID
			)
		WHEN @TotalBranchCnt THEN
			'ALL'
		ELSE
			'MULTIPLE'
		END AS Branch ,
		CDT.Is_SinglePolicy,
		CDT.Is_Deleted,
  USR.UserName,
  PTY.ShortName,  
  (SELECT  TOP 1 Curr.Code 
   FROM  Currency Curr
   INNER JOIN 
    TransDetail TDT  ON TDT.amount_currency_id  = Curr.currency_id 
    INNER JOIN Document DOC  
     ON DOC.Document_ID=TDT.Document_ID  
    INNER JOIN DocumentType DMT  
     ON DMT.DocumentType_ID=DOC.DocumentType_ID  
    WHERE  
     TDT.Account_ID=CDT.Account_ID  
     AND DMT.Code='SRP'  
  )AS CurrencyCode 

	FROM 
		@SelectedCDs SCD
		INNER JOIN CashDeposit CDT
			ON CDT.CashDeposit_ID=SCD.CashDeposit_ID
		INNER JOIN Party PTY
			ON PTY.Party_Cnt=CDT.Party_ID
		INNER JOIN Account ACT
			ON ACT.Account_ID=CDT.Account_ID
		INNER JOIN PMUser USR
			ON USR.User_ID=CDT.User_ID
	SET ROWCOUNT 0	
	SET NOCOUNT OFF

END
--End - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
