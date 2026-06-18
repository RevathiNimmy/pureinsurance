SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Import_Reference_Import'
GO


CREATE PROCEDURE spu_ACT_Import_Reference_Import  
    @batch_id int,  
    @transdetail_id int,  
    @media_ref varchar(100),  
    @items_affected int output  
AS  
  
    -- Update cashlistitem table  
    UPDATE  cashlistitem  
    SET     batch_id = @batch_id,  
            media_ref = @media_ref  
    WHERE   transdetail_id = @transdetail_id  
  
    -- Update cashlistitem table  
    UPDATE  transdetail  
    SET     spare = @media_ref  
    WHERE   transdetail_id = @transdetail_id  
  
    -- Get affected record count  
    SELECT  @items_affected = @@RowCount  

    IF ISNULL(@items_affected,0) <> 0 
	BEGIN 
		UPDATE Claim_Payment
		SET media_ref = @media_ref
		FROM Claim_Payment 

			LEFT JOIN Document ON 
				claim_payment.document_id = document.document_id
		
				LEFT JOIN TransDetail transdetailmatched ON
					transdetailmatched.document_id = document.document_id

					LEFT JOIN AllocationDetail allocationdetailmatched ON 
						transdetailmatched.transdetail_id = allocationdetailmatched.transdetail_id

						LEFT JOIN AllocationDetail ON
							allocationdetail.allocation_id = allocationdetailmatched.allocation_id
						    AND allocationdetail.transdetail_id <> allocationdetailmatched.transdetail_id 
		
							LEFT JOIN Transdetail ON 
								allocationdetail.transdetail_id = transdetail.transdetail_id

		WHERE transdetail.transdetail_id = @transdetail_id
	END 


GO
