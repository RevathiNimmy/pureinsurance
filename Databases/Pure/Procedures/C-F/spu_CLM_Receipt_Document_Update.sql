SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Receipt_Document_Update'
GO

CREATE PROCEDURE spu_CLM_Receipt_Document_Update    
	@stats_folder_cnt int        
AS    
    
BEGIN    
    
 UPDATE cr    
  SET cr.document_id = doc.document_id    
    
 FROM claim_receipt cr    
 INNER JOIN stats_folder sf ON    
   sf.receipt_id = cr.claim_receipt_id    
    
   INNER JOIN document doc ON    
    sf.document_ref = doc.document_ref    
   AND  sf.source_id = doc.company_id    
    
 WHERE sf.stats_folder_cnt =@stats_folder_cnt    
 
 UPDATE cp    
  SET cp.document_id = doc.document_id    
    
 FROM claim_payment cp   
 INNER JOIN stats_folder sf ON    
   sf.loss_id = cp.claim_id    
    
   INNER JOIN document doc ON    
    sf.document_ref = doc.document_ref    
   AND  sf.source_id = doc.company_id    
    
 WHERE sf.stats_folder_cnt =@stats_folder_cnt AND cp.base_claim_payment_id = cp.claim_payment_id AND 
 cp.claim_payment_id IN
 (
	SELECT MAX(claim_payment_id) FROM claim_payment WHERE Claim_id = cp.claim_id 
 )
 
END    
  
  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
