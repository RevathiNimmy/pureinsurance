SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Claim_Payment_Item_id_Select'
GO

CREATE PROCEDURE spu_SAM_Claim_Payment_Item_id_Select 
  
@claim_payment_item_id int OUTPUT,  
@claim_payment_id int,  
@reserve_id int  

AS

SELECT @claim_payment_item_id=claim_payment_item_id
FROM claim_payment_item 
WHERE claim_payment_id=@claim_payment_id AND reserve_id=@reserve_id