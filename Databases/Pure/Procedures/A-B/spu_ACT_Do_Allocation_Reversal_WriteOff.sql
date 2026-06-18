SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Do_Allocation_Reversal_WriteOff'
GO

-- RAW 12/03/2003 : ISS2893 : created
-- RAW 19/05/2003 : CQ954 : added handling of the other transaction in the allocationdetail that triggered the writeoff



CREATE PROCEDURE spu_ACT_Do_Allocation_Reversal_WriteOff
    @v_iDocumentID INT,
    @v_nWriteOffCurrencyAmount numeric (19,4),
    @v_nWriteOffBaseAmount numeric (19,4)
AS

    declare @iAllocationdetailId int
    DECLARE @iTransMatchId INT
    DECLARE @iTransDetailId INT
    declare @iWriteOffTransdetailId int
    declare @iOtherTransdetailId int
    declare @nWriteOffAccountAmount numeric (19,4)
    declare @nWriteOffSystemAmount numeric (19,4)
    declare @nWriteOffRevBaseAmount numeric (19,4)
    declare @nWriteOffRevCurrencyAmount numeric (19,4)
    declare @nWriteOffRevAccountAmount numeric (19,4)
    declare @nWriteOffRevSystemAmount numeric (19,4)
    declare @iAuditSetID int    

    declare @iErrorCode int
    declare @k_iErrorCode_DocumentIdMissing int
    SET     @k_iErrorCode_DocumentIdMissing = -1
    declare @k_iErrorCode_DocumentIdInvalid int
    SET     @k_iErrorCode_DocumentIdInvalid = -2
    
    DECLARE @account_id INT
    DECLARE @company_id INT
    DECLARE @currency_id INT
    DECLARE @currency_base_xrate FLOAT
    DECLARE @account_base_xrate FLOAT
    DECLARE @system_base_xrate FLOAT
    DECLARE @return_status INT


    -- ===================================================================  
    -- VALIDATE PARAMS AND SET DEFAULTS WHERE MISSING
    -- ===================================================================  

    IF (@v_iDocumentID IS NULL) BEGIN
        print 'DocumentID has not been provided'
        RETURN @k_iErrorCode_DocumentIdMissing
    END

    -- Is DocumentID valid
    SELECT  1
    FROM    Document d
    WHERE   d.Document_id = @v_iDocumentID
      AND   d.documenttype_id IN (14,49)        -- Write Off (or Currency Differences) document type

    IF (@@ROWCOUNT <> 1) BEGIN
        print 'DocumentID is invalid'
        RETURN @k_iErrorCode_DocumentIdInvalid
    END



    -- capture and store details about the write off
    SELECT 
        @iTransDetailId = td.transdetail_id,
        @iTransMatchId = tm.transmatch_id,
        @iAllocationdetailId = ad.allocationdetail_id,
        @iWriteOffTransdetailId = td.transdetail_id       
    FROM transdetail td
    JOIN transmatch tm
        ON tm.transdetail_id = td.transdetail_id
    JOIN allocationdetail ad
        ON ad.allocationdetail_id = tm.allocationdetail_id
        AND ad.transdetail_id <> tm.transdetail_id
    WHERE td.document_id = @v_iDocumentID
    AND ISNULL(tm.is_reversed,0) = 0   -- only reverse a row once
    AND tm.base_match_amount <> 0


    SELECT
            @iOtherTransdetailId = tm.transdetail_id
    FROM 
            transmatch tm
    WHERE   
            tm.allocationdetail_id = @iAllocationdetailId
      AND   tm.transdetail_id <> @iWriteOffTransdetailId

    -- RAW 19/05/2003 : CQ954 : end



    -- ==========================================================================================================
    -- write off amounts cause a problem because there is no separate row for them in allocationdetail
    -- but there are in transmatch and transdetail

    -- Note - partial allocations do not apply to write off documents 
    --     - one write off document is only ever associated with one allocationdetail
    -- ==========================================================================================================

    BEGIN TRANSACTION


    /*Flag transmatch record as being reversed*/
    UPDATE transmatch
    SET is_reversed=1
    WHERE transmatch_id = @iTransMatchId
    AND ISNULL(is_reversed,0) = 0
 
    /*Add transmatch reversing entry*/
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
        system_match_amount
    )
    SELECT  
        currency_id,
        allocationdetail_id,
        transdetail_id,
        match_id,
        -1 * base_match_amount,
        -1 * currency_match_amount,
        currency_match_xrate,
        is_reversed,
        -1 * account_match_amount,
        -1 * system_match_amount
    FROM transmatch
    WHERE transmatch_id = @iTransMatchId
    AND ISNULL(is_reversed,0) = 0   -- only reverse a row once
    
    SELECT 
        @nWriteOffRevCurrencyAmount = -1 * currency_match_amount, 
        @nWriteOffRevBaseAmount = -1 * base_match_amount, 
        @nWriteOffRevAccountAmount = -1 * account_match_amount, 
        @nWriteOffRevSystemAmount = -1 * system_match_amount
    FROM transmatch
    WHERE transmatch_id = @iTransMatchId

    /*Update the transdetail record with new outstanding amounts*/
    EXEC spu_ACT_Update_TransDetailOutstanding 
            @iTransDetailId, 
            @nWriteOffRevCurrencyAmount, 
            @nWriteOffRevBaseAmount, 
            @nWriteOffRevAccountAmount, 
            @nWriteOffRevSystemAmount 
   
    --Get details that were used by the match we are reversing
    SELECT
        @currency_base_xrate = currency_match_xrate
    FROM transmatch
    WHERE transdetail_id = @iOtherTransdetailId
    AND allocationdetail_id = @iAllocationdetailId
    
    SELECT 
        @account_id = account_id,
        @company_id = company_id,
        @currency_id = currency_id,
        @account_base_xrate = account_base_xrate,
        @system_base_xrate = system_base_xrate
    FROM transdetail
    WHERE transdetail_id = @iOtherTransdetailId

    EXECUTE spu_ACT_Do_Currency_Conversion
        @account_id = @account_id,
        @company_id = @company_id,
        @currency_id = @currency_id,
        @currency_amount_unrounded = @v_nWriteOffCurrencyAmount,
        @mode = 'ALL',
        @account_amount = @nWriteOffAccountAmount OUTPUT,
        @system_amount = @nWriteOffSystemAmount OUTPUT,
        @currency_base_xrate = @currency_base_xrate OUTPUT,
        @account_base_xrate = @account_base_xrate OUTPUT,
        @system_base_xrate = @system_base_xrate OUTPUT,
        @return_status = @return_status OUTPUT

    -- ===================================================================  
    -- Write off amounts are also included in the totals for the transaction that triggered the write off
    -- so remove it from there as well
    -- ===================================================================  

    UPDATE  transmatch
    SET      
            base_match_amount = base_match_amount - @v_nWriteOffBaseAmount,
            currency_match_amount = currency_match_amount - @v_nWriteOffCurrencyAmount,
            account_match_amount = account_match_amount - @nWriteOffAccountAmount,
            system_match_amount = system_match_amount - @nWriteOffSystemAmount
    WHERE
            transdetail_id = @iOtherTransdetailId    
      AND   allocationdetail_id = @iAllocationdetailId


    /*Should never be NULL but just in case*/
    IF @iAllocationdetailId IS NOT NULL
    BEGIN
        SELECT
            @nWriteOffRevBaseAmount = -1 * @v_nWriteOffBaseAmount,
            @nWriteOffRevCurrencyAmount = -1 * @v_nWriteOffCurrencyAmount,
            @nWriteOffRevAccountAmount = -1 * @nWriteOffAccountAmount,
            @nWriteOffRevSystemAmount = -1 * @nWriteOffSystemAmount
    
        /*Update the transdetail record with new outstanding amounts*/
        EXEC spu_ACT_Update_TransDetailOutstanding @iOtherTransdetailId, 
                    @nWriteOffRevCurrencyAmount, @nWriteOffRevBaseAmount, @nWriteOffRevAccountAmount, @nWriteOffRevSystemAmount 
    END

/*
    -- ===================================================================  
    -- Add entry to AuditSet for document
    -- remember we do not need to handle partial document allocations
    -- ===================================================================  
    --SET @iAuditSetID = 1        -- set default
    SELECT @iAuditSetID = ISNULL(MAX(auditset_id),0) + 1 FROM AuditSet

    INSERT INTO AuditSet
        (
            auditset_id,
            company_id,
            posted_date,
            comment,
            document_id,
            auditset_type_id
        )
        SELECT  
                @iAuditSetID,
                d.company_id,            
                getdate(),
                'write off reversed',
                @v_iDocumentID,
                2                -- reversal
        FROM    document d

        WHERE   d.document_id = @v_iDocumentID
    



    -- ===================================================================  
    -- UPDATE Document
    -- remember we do not need to handle partial document allocations
    -- ===================================================================  
    
    UPDATE  document
    SET     auditset_id = @iAuditSetID
    WHERE   document_id = @v_iDocumentID

    
    -- ===================================================================  
    -- UPDATE TransDetail
    -- remember we do not need to handle partial document allocations
    -- ===================================================================  
    
    -- update each transdetail row that is associated with this document_id
    --    note - do not change fully_matched since we are effectively cancelling the transaction
    
    
    
    UPDATE  transdetail
    SET     amount = 0,
            base_amount_unrounded = 0,
            currency_amount = 0,
            currency_amount_unrounded = 0,
            comment = LEFT('Reversed - ' + comment, 60),
            account_amount = 0,
            account_amount_unrounded = 0,
            system_amount = 0,
            system_amount_unrounded = 0,
            outstanding_amount = 0,
            outstanding_currency_amount = 0,
            outstanding_account_amount = 0,
            outstanding_system_amount = 0
    WHERE   document_id = @v_iDocumentID

*/


    
    -- RAW 19/05/2003 : CQ954 : added
    -- ===================================================================  
    -- UPDATE AllocationDetail
    -- remember we do not need to handle partial document allocations
    -- ===================================================================  
    
    -- update the allocationdetail row for the transaction that the write-off trans applies to
    -- ie it has the same allocationdetail_id
    UPDATE  allocationdetail
    SET     
            new_os_base_amount = new_os_base_amount + @v_nWriteOffBaseAmount,
            new_os_ccy_amount = new_os_ccy_amount + @v_nWriteOffCurrencyAmount,
            write_off_reason_id = NULL,
            write_off_amount = NULL
    WHERE   
            transdetail_id = @iOtherTransdetailId    
      AND   allocationdetail_id = @iAllocationdetailId


    
    COMMIT TRANSACTION


GO