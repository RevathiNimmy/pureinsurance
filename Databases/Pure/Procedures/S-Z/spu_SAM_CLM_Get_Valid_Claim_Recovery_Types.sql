SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Get_Valid_Claim_Recovery_Types'
GO

CREATE PROCEDURE spu_SAM_CLM_Get_Valid_Claim_Recovery_Types  
  
AS  
  
Select recovery_type_id, code, description from recovery_type  
WHERE is_deleted = 0  



GO
