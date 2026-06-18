SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_Convert_Policy_Amount_To_Base_Currency'
GO

--Start - Renuka - (WPR85_Cash_Deposit_Process)
CREATE  PROCEDURE spu_Convert_Policy_Amount_To_Base_Currency
	@Insurance_File_Cnt INT,
	@Policy_Amount MONEY,
	@Base_Amount MONEY OUTPUT,
	@Base_Currency_ID INT OUTPUT,
	@Base_Currency_Code VARCHAR(20) OUTPUT,
	@Transaction_Currency_ID INT OUTPUT,
	@Transaction_Currency_Code VARCHAR(20) OUTPUT
AS 
BEGIN
	DECLARE @Source_ID INT
	DECLARE @Transaction_Currency_Base_Xrate FLOAT
	DECLARE @Return_Status INT
	DECLARE @Branch_Base_Currency_ID INT
	DECLARE @Branch_Base_Currency_Code VARCHAR(20)

	-- Get details from insurance file
    SELECT  
		@Source_ID = Source_ID,
        @Transaction_Currency_ID = Currency_ID,
        @Transaction_Currency_Base_Xrate = Currency_Base_Xrate
    FROM    
		Insurance_FIle
    WHERE   
		Insurance_File_Cnt = @Insurance_File_Cnt

	-- Get transaction currency code
    SELECT  
		@Transaction_Currency_Code = Code
    FROM    
		Currency
    WHERE   
		Currency_id = @Transaction_Currency_ID

	--Get Branch Base currency details
	SELECT  
		@Branch_Base_Currency_ID=S.Base_Currency_ID,
		@Branch_Base_Currency_Code=C.Code
	FROM
		Source S
		INNER JOIN Currency C
			ON C.Currency_ID=S.Base_Currency_ID
	WHERE
		S.Source_ID=@Source_ID

	IF @Branch_Base_Currency_ID=@Transaction_Currency_ID
	BEGIN
		SELECT 
			@Base_Amount=@Policy_Amount,
			@Base_Currency_ID=@Branch_Base_Currency_ID,
			@Base_Currency_Code=@Branch_Base_Currency_Code
	END
	ELSE
	BEGIN
		-- Get amounts in base currency
		EXEC spu_ACT_Do_Currency_Conversion
				@Company_ID = @Source_ID,
				@Currency_ID = @Transaction_Currency_ID,
				@Currency_Amount_Unrounded = @Policy_Amount,
				@Mode = '1',
				@Base_Amount = @Base_Amount OUTPUT,
				@Base_Currency_ID=@Base_Currency_ID OUTPUT,
				@Base_Currency_Code=@Base_Currency_Code OUTPUT,
				@Currency_Base_Xrate = @Transaction_Currency_Base_Xrate OUTPUT,
				@Return_Status = @Return_Status OUTPUT
	END
END
--End - Renuka - (WPR85_Cash_Deposit_Process)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO