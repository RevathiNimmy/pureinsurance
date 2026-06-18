SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_CashListItem_claim_link_add'
GO
CREATE PROCEDURE spu_CashListItem_claim_link_add  
    @claim_payment_id int = NULL,  
    @claim_receipt_id int = NULL,
    @cashlistitem_id int  
AS  
  IF Exists( Select 1 FROM cashlistitem_claim_link WHERE claim_payment_id = @claim_payment_id AND cashlistitem_id = @cashlistitem_id 
			UNION 
			Select 1 FROM cashlistitem_claim_link WHERE claim_receipt_id = @claim_receipt_id AND cashlistitem_id = @cashlistitem_id	
			)
		RETURN 
		
    INSERT INTO cashlistitem_claim_link
    (
        claim_payment_id,
        claim_receipt_id,
        cashlistitem_id,
        is_deleted
    )
    values
    (
        @claim_payment_id,  
        @claim_receipt_id,
        @cashlistitem_id,  
        0
    )

GO
