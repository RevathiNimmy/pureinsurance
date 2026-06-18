SET QUOTED_IDENTIFIER OFF
GO

EXECUTE DDLDropProcedure 'spu_CLM_Get_ThisReceipt_Item'  
GO

CREATE PROCEDURE spu_CLM_Get_ThisReceipt_Item      
	@nClaim_Receipt_Id INT 
AS    
BEGIN      
   
	DECLARE @option_number INT , @option_value VARCHAR(255)

	SET @option_number = 5067
	SET @option_value = '0'

	SELECT @option_value = value FROM System_Options WHERE option_number =  @option_number

	IF @option_value = '1'
	BEGIN
		SELECT claim_receipt_item_id, this_receipt + ISNULL(tax_amount,0), tax_amount 
		FROM Claim_Receipt_Item 
		WHERE claim_receipt_id =  @nClaim_Receipt_Id
	END
	ELSE
	BEGIN
		SELECT claim_receipt_item_id, this_receipt ,tax_amount 
		FROM Claim_Receipt_Item 
		WHERE claim_receipt_id =  @nClaim_Receipt_Id
	END
      
END      

    
    
  
