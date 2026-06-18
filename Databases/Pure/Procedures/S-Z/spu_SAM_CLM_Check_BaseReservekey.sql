SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Check_BaseReservekey'
GO

CREATE PROCEDURE spu_SAM_CLM_Check_BaseReservekey  
@base_Reserve_Id INT,  
@base_Claim_Peril_Id INT,  
@Count INT = NULL OUTPUT  
  
AS  
  
SELECT @Count = COUNT(Reserve_id)  
FROM Reserve R  
JOIN Claim_Peril CP  
ON CP.Claim_peril_id=R.Claim_peril_id  
  
WHERE base_reserve_id = @Base_reserve_id and CP.Base_claim_Peril_id=@base_Claim_Peril_id  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
