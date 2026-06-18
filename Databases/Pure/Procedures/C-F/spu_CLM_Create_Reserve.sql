SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Create_Reserve'
GO

CREATE PROCEDURE spu_CLM_Create_Reserve  
 @claim_peril_id int  
AS  
  
BEGIN  
  
 INSERT INTO reserve (claim_peril_id, reserve_type_id, sum_insured)  
  
 SELECT  claim_peril.claim_peril_id,  
         peril_type_reserve_type.reserve_type_id,  
         CASE  
   WHEN is_main_reserve = 0 THEN 0  
     ELSE claim_peril.sum_insured  
  END AS sum_insured  
  
 FROM peril_type_reserve_type  
  
  INNER JOIN claim_peril ON  
   claim_peril.peril_type_id = peril_type_reserve_type.peril_type_id  
  
  LEFT JOIN reserve ON  
   claim_peril.claim_peril_id = reserve.claim_peril_id  
  
 WHERE claim_peril.claim_peril_id = @claim_peril_id  
 AND reserve.reserve_id IS null  
 ORDER BY peril_type_reserve_type.reserve_type_id  
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
