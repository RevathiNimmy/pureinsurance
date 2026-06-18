SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_CashDeposit_Add'
GO

--Start - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
CREATE  PROCEDURE spu_CashDeposit_Add
	@CashDeposit_ID INT OUTPUT,
	@CashDeposit_Ref VARCHAR(30),
	@Account_ID INT,
	@Party_ID INT,
	@Is_SinglePolicy TINYINT,					
	@User_ID SMALLINT
AS 
BEGIN
	DECLARE @NextNumber AS INT
	
	--Start - Prakash - PN 65596
	DECLARE @CurrentCharIndex AS INT
	DECLARE @LastCharIndex AS INT
	--End - Prakash - PN 65596

	INSERT INTO CashDeposit (
							CashDeposit_Ref ,
							Account_ID,
							Party_ID,
							Is_SinglePolicy,					
							Is_Deleted,
							Date_Created,
							User_ID
						   )					 
	VALUES (
			@CashDeposit_Ref ,
			@Account_ID,
			@Party_ID,
			@Is_SinglePolicy,					
			0,
			GETDATE(),
			@User_ID
		   )
		
	SET @CashDeposit_ID=@@IDENTITY

	--Start - Prakash - PN 65596
	--Get the last position of '-' in the string
	SET @CurrentCharIndex=CHARINDEX('-',@CashDeposit_Ref)
	WHILE @CurrentCharIndex>0
	BEGIN
		SET @LastCharIndex=@CurrentCharIndex
		set @CurrentCharIndex=CHARINDEX('-',@CashDeposit_Ref,@LastCharIndex+1)
	END

	--Get the number after the '-CD'
	SET @NextNumber=CONVERT(INT,SUBSTRING(@CashDeposit_Ref,@LastCharIndex+3,LEN(@CashDeposit_Ref)))
	--End - Prakash - PN 65596
	
	IF @NextNumber=1
		INSERT INTO 
			CashDepositNumber (
								Party_ID,
								Next_Number
							  )
		VALUES (
				@Party_ID,
				@NextNumber
			   )
	ELSE
		UPDATE 
			CashDepositNumber
		SET
			Next_Number=@NextNumber
		WHERE
			Party_ID=@Party_ID
END
--End - Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

