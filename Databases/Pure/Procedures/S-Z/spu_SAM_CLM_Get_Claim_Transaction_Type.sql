SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Get_Claim_Transaction_Type'
GO

CREATE PROCEDURE spu_SAM_CLM_Get_Claim_Transaction_Type 
  
@claim_id integer  
  
AS  
  
SELECT  transaction_type_id
FROM claim 
WHERE claim_id=@claim_id  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
