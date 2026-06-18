SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_CLM_NetOf_Claim_Peril_Reserve
GO

CREATE PROCEDURE spu_CLM_NetOf_Claim_Peril_Reserve    
 @claim_id INT,
 @claim_peril_id INT,
 @PrePortfolioTransfer BIT = 0,
 @this_revision MONEY OUTPUT
 AS    
    
	DECLARE @reserve_id INT,
	@ThisRevision MONEY,
	@recovery_id INT,
	@ThisReceipt MONEY,
	@this_receipt MONEY
	
	SET @this_revision = 0
	SET @ThisRevision = 0
	
	DECLARE claim_reserve_cursor CURSOR STATIC FOR    
	SELECT reserve_id    
	FROM reserve WITH(NOLOCK)    
	WHERE claim_peril_id = @claim_peril_id  
  
	OPEN claim_reserve_cursor    
  
	FETCH NEXT FROM claim_reserve_cursor    
	INTO @reserve_id    
	      
	WHILE @@FETCH_STATUS = 0    
	BEGIN          
		-- in case of negative???? should be < zero
		IF @PrePortfolioTransfer = 1
		BEGIN
			UPDATE reserve   
			SET 			
			this_revision = paid_to_date - (initial_reserve + revised_reserve) ,
			revised_reserve = paid_to_date - (initial_reserve + revised_reserve) ,
			Initial_reserve = -1*(paid_to_date - (initial_reserve + revised_reserve)),
			Paid_to_date = 0,
			revision_count = revision_count + 1  
			WHERE reserve_id = @reserve_id
			
			SELECT @ThisRevision = this_revision
			FROM reserve
			WHERE reserve_id = @reserve_id
			IF @ThisRevision <> 0
			BEGIN
				SET @this_revision = @this_revision + @ThisRevision
			END
			UPDATE Claim_Payment_Item SET this_payment = 0 WHERE reserve_id = @reserve_id									
		END
		ELSE
		BEGIN
			UPDATE reserve   
			SET 			
			this_revision = -1* revised_reserve, --initial_reserve + revised_reserve - paid_to_date,  
			revised_reserve = revised_reserve + (-1* revised_reserve), --revised_reserve + (initial_reserve + revised_reserve - paid_to_date),  
			revision_count = revision_count + 1  
			WHERE reserve_id = @reserve_id
			
			SELECT @ThisRevision = this_revision
			FROM reserve
			WHERE reserve_id = @reserve_id
			IF @ThisRevision <> 0
			BEGIN
				SET @this_revision = @this_revision + @ThisRevision
			END						 						
		END					
	FETCH NEXT FROM claim_reserve_cursor    
	INTO @reserve_id         
	END  
	CLOSE claim_reserve_cursor
	DEALLOCATE claim_reserve_cursor
	-- For Recovery
	DECLARE claim_recovery_cursor CURSOR STATIC FOR    
	SELECT recovery_id    
	FROM recovery WITH(NOLOCK)    
	WHERE claim_peril_id = @claim_peril_id  
  
	OPEN claim_recovery_cursor    
  
	FETCH NEXT FROM claim_recovery_cursor    
	INTO @recovery_id    
	      
	WHILE @@FETCH_STATUS = 0    
	BEGIN          		
		IF @PrePortfolioTransfer = 1
		BEGIN
			UPDATE Recovery   
			SET 			
			--this_receipt_net = received_to_date - (initial_reserve + revised_reserve) ,
			revised_reserve = received_to_date - (initial_reserve + revised_reserve) ,
			Initial_reserve = -1*(received_to_date - (initial_reserve + revised_reserve)),
			received_to_date = 0,
			revision_count = revision_count + 1  
			WHERE Recovery_id = @recovery_id
			
			SELECT @ThisReceipt = this_receipt_net
			FROM recovery
			WHERE recovery_id = @recovery_id
			IF @ThisReceipt <> 0
			BEGIN
				SET @this_receipt = @this_receipt + @ThisReceipt
			END						
		END
		ELSE
		BEGIN
			UPDATE Recovery   
			SET 			
			--this_receipt_net = -1* revised_reserve, 
			revised_reserve = revised_reserve + (-1* revised_reserve),
			revision_count = revision_count + 1  
			WHERE Recovery_id = @recovery_id
			
			SELECT @ThisReceipt = this_receipt_net
			FROM Recovery
			WHERE recovery_id = @recovery_id
			IF @ThisReceipt <> 0
			BEGIN
				SET @this_receipt = @this_receipt + @ThisReceipt
			END						 						
		END					
	FETCH NEXT FROM claim_recovery_cursor    
	INTO @recovery_id         
	END
	CLOSE  claim_recovery_cursor
	DEALLOCATE claim_recovery_cursor
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
