SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Claim_Payment_id_Select'
GO

CREATE PROCEDURE spu_SAM_Claim_Payment_id_Select 
  
@claim_payment_id int OUTPUT,  
@claim_id int,  
@claim_peril_id int  

AS

SELECT @claim_payment_id=claim_payment_id
FROM claim_payment 
WHERE claim_id=@claim_id AND claim_peril_id=@claim_peril_id