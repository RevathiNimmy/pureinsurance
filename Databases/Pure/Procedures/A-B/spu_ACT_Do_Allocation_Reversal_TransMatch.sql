SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Do_Allocation_Reversal_TransMatch'
GO


CREATE PROCEDURE spu_ACT_Do_Allocation_Reversal_TransMatch

                                        @v_iAllocationDetailID int,
                                        @v_iTransDetailID int,
                                        @v_nAllocCurrencyAmount    numeric (19,4),
                                        @v_nAllocBaseAmount        numeric (19,4)

AS

    DECLARE @iOrigTransMatchId int
    DECLARE @nOrigCurrencyAmount    numeric (19,4)
    DECLARE @nOrigBaseAmount        numeric (19,4)
    DECLARE @nOrigAccountAmount        numeric (19,4)
    DECLARE @nOrigSystemAmount        numeric (19,4)
    DECLARE @nRevOrigCurrencyAmount    numeric (19,4)
	DECLARE @nRevOrigBaseAmount        numeric (19,4)
	DECLARE @nRevOrigAccountAmount        numeric (19,4)
    DECLARE @nRevOrigSystemAmount        numeric (19,4)
    DECLARE @nNewCurrencyAmount    numeric (19,4)
	DECLARE @nNewBaseAmount        numeric (19,4)
	DECLARE @nNewAccountAmount        numeric (19,4)
    DECLARE @nNewSystemAmount        numeric (19,4)
    DECLARE @nAllocAccountAmount    numeric (19,4)
    DECLARE @nAllocSystemAmount        numeric (19,4)
    DECLARE @account_id INT
	DECLARE @company_id INT
	DECLARE @currency_id INT
	DECLARE @currency_base_xrate FLOAT
	DECLARE @account_base_xrate FLOAT
	DECLARE @system_base_xrate FLOAT
	DECLARE @return_status INT
	DECLARE @match_id INT
	DECLARE @match_date DATETIME

    DECLARE @iReturnCode int

    declare @iErrorCode int
    declare @k_iErrorCode_UnexpectedRowCount int
    SET     @k_iErrorCode_UnexpectedRowCount = -1
    declare @k_iErrorCode_MismatchingTotals int
    SET     @k_iErrorCode_MismatchingTotals = -2



    -- ===================================================================	
    -- get the details from the transmatch row that is to be reversed
    -- ===================================================================	

    SELECT  @iOrigTransMatchId = transmatch_id,
            @nOrigBaseAmount = base_match_amount,
            @nOrigCurrencyAmount = currency_match_amount,
            @nOrigAccountAmount = account_match_amount,
            @nOrigSystemAmount = system_match_amount,
            @nRevOrigCurrencyAmount = currency_match_amount * -1,    
			@nRevOrigBaseAmount = base_match_amount * -1,        
			@nRevOrigAccountAmount = account_match_amount * -1,        
			@nRevOrigSystemAmount = system_match_amount * -1,
			@currency_base_xrate = currency_match_xrate,
			@currency_id = currency_id,
			@match_id = match_id
    FROM    Transmatch tm
       
    WHERE   tm.allocationdetail_id = @v_iAllocationDetailID
      AND   tm.transdetail_id = @v_iTransDetailID
      AND   ISNULL(tm.is_reversed,0) = 0   

    -- there should only be one    
    IF (@@ROWCOUNT > 1) BEGIN
        print 'More than one transmatch row detected with is_reversed not set'
        RETURN @k_iErrorCode_UnexpectedRowCount
    END


    BEGIN TRANSACTION


    -- ===================================================================	
    -- add reversing entries into transmatch - for the full original match amount but with the sign reversed
    -- ===================================================================	

    INSERT INTO transmatch
	(
		currency_id,
		allocationdetail_id,
		transdetail_id,
		match_id,
		base_match_amount,
		currency_match_amount,
		currency_match_xrate,
		is_reversed,
		account_match_amount,
		system_match_amount,
		allocation_reversed_date
	)
	SELECT  currency_id,
			allocationdetail_id,
			transdetail_id,
			match_id,
			-1 * base_match_amount,        
			-1 * currency_match_amount,    
			currency_match_xrate,
			1,
			-1 * account_match_amount,
			-1 * system_match_amount,
			(SELECT TOP 1 accounting_date FROM allocationdetail where
			allocationdetail_id=@v_iAllocationDetailID)
	FROM    transmatch tm
	WHERE   tm.transmatch_id = @iOrigTransMatchId
    
    
    /*Should never be NULL but just in case*/
    IF @v_iAllocationDetailID IS NOT NULL
	BEGIN
		/*Update the transdetail record with new outstanding amounts*/
		EXEC spu_ACT_Update_TransDetailOutstanding @v_iTransDetailID, 
				@nRevOrigCurrencyAmount, @nRevOrigBaseAmount, @nRevOrigAccountAmount, @nRevOrigSystemAmount 
	END


    -- ===================================================================	
    -- flag the original row that is being reversed  
    -- ===================================================================	

    UPDATE  transmatch
	SET     
            is_reversed=1,
            allocation_reversed_date=(SELECT TOP 1 accounting_date FROM allocationdetail where
			allocationdetail_id=@v_iAllocationDetailID)
	WHERE   
            transmatch_id = @iOrigTransMatchId
                          


    -- ===================================================================	
    -- add new entry into transmatch to replace the original - but only if not fully reversed
    -- This should use the net match amount 
    -- ===================================================================	

    IF ( @nOrigBaseAmount <> (@v_nAllocBaseAmount)) BEGIN

		/*Get the account and company IDs from the transdetail record.*/
		SELECT	@account_id = account_id,
				@company_id = company_id,
				@account_base_xrate = account_base_xrate,
				@system_base_xrate = system_base_xrate
		FROM	TransDetail
		WHERE	transdetail_id=@v_iTransDetailID
		
		/*Get the match date from the matchgroup record*/
		SELECT @match_date = match_date
		FROM MatchGroup
		WHERE match_id = @match_id

		/*Get the converted amounts*/
		EXECUTE spu_ACT_Do_Currency_Conversion
			@account_id = @account_id,
			@company_id = @company_id,
			@currency_id = @currency_id,
			@currency_amount_unrounded = @v_nAllocCurrencyAmount,
			@mode = 'ALL',
			@account_amount = @nAllocAccountAmount OUTPUT,
			@system_amount = @nAllocSystemAmount OUTPUT,
			@currency_base_xrate = @currency_base_xrate OUTPUT,
			@currency_base_date = @match_date OUTPUT,
			@account_base_xrate = @account_base_xrate OUTPUT,
			@system_base_xrate = @system_base_xrate OUTPUT,
			@return_status = @return_status OUTPUT

        INSERT INTO transmatch
		(
			currency_id,
			allocationdetail_id,
			transdetail_id,
			match_id,
			base_match_amount,
			currency_match_amount,
			currency_match_xrate,
			account_match_amount,
			system_match_amount
		)
		SELECT  currency_id,
				allocationdetail_id,
				transdetail_id,
				match_id,
				base_match_amount - @v_nAllocBaseAmount,            -- reduce the match amount by the allocation amount just reversed
				currency_match_amount - @v_nAllocCurrencyAmount,    -- reduce the match amount by the allocation amount just reversed
				currency_match_xrate,
				account_match_amount - @nAllocAccountAmount,
				system_match_amount - @nAllocSystemAmount
		FROM    transmatch tm
		WHERE   tm.transmatch_id = @iOrigTransMatchId
            
		/*Should never be NULL but just in case*/
		IF @v_iAllocationDetailID IS NOT NULL
		BEGIN
			
			SELECT @nNewCurrencyAmount = @nOrigCurrencyAmount - @v_nAllocCurrencyAmount
			SELECT @nNewBaseAmount = @nOrigBaseAmount - @v_nAllocBaseAmount
			SELECT @nNewAccountAmount = @nOrigAccountAmount - @nAllocAccountAmount
			SELECT @nNewSystemAmount = @nOrigSystemAmount - @nAllocSystemAmount
		
			/*Update the transdetail record with new outstanding amounts*/
			EXEC spu_ACT_Update_TransDetailOutstanding @v_iTransDetailID, 
					@nNewCurrencyAmount, @nNewBaseAmount, @nNewAccountAmount, @nNewSystemAmount
		END

    END



    -- ===================================================================	
    -- now does it balance
    -- ===================================================================	

    SELECT  COUNT(*)
    FROM    transmatch tm
    WHERE   
            tm.allocationdetail_id = @v_iAllocationDetailID
      AND   tm.transdetail_id = @v_iTransDetailID
    HAVING 
            SUM(base_match_amount)     <> @nOrigBaseAmount - @v_nAllocBaseAmount
        OR  SUM(currency_match_amount) <> @nOrigCurrencyAmount - @v_nAllocCurrencyAmount

    IF (@@ROWCOUNT > 0) BEGIN
        print 'Transmatch totals do not balance as expected'
        ROLLBACK TRANSACTION
        RETURN @k_iErrorCode_MismatchingTotals
    END


    -- ===================================================================	
    -- ALL DONE
    -- ===================================================================	
    COMMIT TRANSACTION
GO