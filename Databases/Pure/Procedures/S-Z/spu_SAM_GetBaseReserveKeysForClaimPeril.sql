SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_GetBaseReserveKeysForClaimPeril'
GO

CREATE PROCEDURE spu_SAM_GetBaseReserveKeysForClaimPeril  
 
@claim_peril_id int  
  
AS  
  
SELECT reserve.base_reserve_id, reserve.reserve_type_id, reserve_type.name   
FROM reserve   
  
 INNER JOIN reserve_type ON   
  reserve_type.reserve_type_id = reserve.reserve_type_id  
  
WHERE claim_peril_id = @claim_peril_id  
  
  


GO
