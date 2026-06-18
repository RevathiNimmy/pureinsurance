
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Risks_Cloned_RI_Cloned_Claim_Sel'
GO

CREATE PROCEDURE spu_Risks_Cloned_RI_Cloned_Claim_Sel  
@product_id int=0,  
@source_id int=0  
AS  

 SELECT Claim_Payment.Claim_id ,claim.base_claim_id  INTO #TmpCLPT FROM    Claim_Payment    
    JOIN Claim On   Claim_Payment.claim_id = Claim.Claim_id  
    WHERE  is_dirty=0 AND
     (Claim_Payment.amount NOT IN (0)) AND (Claim_Payment.is_referred = 1)    
  
SELECT  DISTINCT
      inf.insurance_ref,  
      par.shortname,  
      par.[name],  
      ccru.claim_cloned_RI_usage_id,  
      ccru.Old_insurance_file_cnt,  
      ccru.New_insurance_file_cnt,  
      ccru.Old_Risk_Cnt,  
      ccru.New_Risk_Cnt,  
      ccru.Status,c.claim_id  
FROM Claim_Cloned_RI_Usage ccru  
      INNER JOIN  
      Claim c ON c.Risk_type_id = ccru.Old_Risk_Cnt  
      INNER JOIN  
      Insurance_File inf ON ccru.Old_insurance_file_cnt = inf.insurance_file_cnt  
      INNER JOIN  
      Risk r ON r.risk_cnt = c.Risk_type_id  
      LEFT JOIN  
      Party par ON par.party_cnt = inf.insured_cnt  
      LEFT JOIN #TmpCLPT on #TmpCLPT.claim_id=c.claim_id  
WHERE ccru.status = 1  and c.Claim_id=c.base_claim_id
   AND #TmpCLPT.claim_id IS NULL    
   AND (inf.product_id=@product_id or isnull(@product_id,0)=0)  
   AND (inf.source_id=@source_id or isnull(@source_id,0)=0)  
   AND is_dirty = 0
  
UNION  
  
SELECT  DISTINCT
      inf.insurance_ref,  
      par.shortname,  
      par.[name],  
      0,  
      inf.insurance_file_cnt,  
      c.policy_id,  
      r.Risk_Cnt,  
      c.Risk_type_id,  
      1, c.base_claim_id  
FROM  
      Claim c  
      INNER JOIN  
      Insurance_File inf ON C.POLICY_ID = inf.insurance_file_cnt  
      INNER JOIN  
      Risk r ON r.risk_cnt = c.risk_type_id  
      INNER JOIN  
      ri_arrangement ri ON r.risk_cnt = ri.risk_cnt  
      INNER JOIN  
      claim_ri_arrangement cri ON cri.claim_id = c.Claim_id  
      LEFT JOIN  
      Party par ON par.party_cnt = inf.insured_cnt  
      INNER Join  
      RI_Model As rim ON cri.ri_model_id = rim.ri_model_id  
      INNER JOIN  
      RI_Model As xrim ON cri.xol_ri_model_id = xrim.ri_model_id  
      LEFT JOIN #TmpCLPT on #TmpCLPT.claim_id=c.claim_id  
      WHERE cri.Cloned=1 AND ri.original_flag=0  
      AND (rim.ri_model_type NOT IN (4) AND xrim.ri_model_type NOT IN (4))  
      AND #TmpCLPT.claim_id IS NULL    
      AND (inf.product_id=@product_id or isnull(@product_id,0)=0)  
      AND (inf.source_id=@source_id or isnull(@source_id,0)=0)  
	  AND is_dirty = 0
  
	  DROP TABLE #TmpCLPT  
  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO