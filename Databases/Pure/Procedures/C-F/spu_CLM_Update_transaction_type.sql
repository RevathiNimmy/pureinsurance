SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Update_transaction_type'
GO

CREATE Procedure spu_CLM_Update_transaction_type 
    @claim_ID int,
    @transaction_type varchar(12)  
AS
  UPDATE Claim  
  SET transaction_type_id = (SELECT transaction_type_id FROM transaction_type WHERE code = @transaction_type)
  WHERE claim_id = @claim_id