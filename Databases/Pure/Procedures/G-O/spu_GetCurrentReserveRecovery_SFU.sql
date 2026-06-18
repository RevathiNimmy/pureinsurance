SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_GetCurrentReserveRecovery_SFU'
GO

CREATE  PROCEDURE spu_GetCurrentReserveRecovery_SFU  
    @ClaimID int  
AS  
  
BEGIN  
 SELECT c.claim_status_id,  
   (SELECT IsNull(SUM(initial_reserve + revised_reserve - paid_to_date),0)  
    FROM Reserve r JOIN Claim_Peril cp ON r.claim_peril_id = cp.claim_peril_id  
    WHERE cp.claim_id = @ClaimID) 'CurrentReserve',  
   (SELECT IsNull(SUM(initial_reserve + revised_reserve - received_to_date),0)  
    FROM Recovery r JOIN Claim_Peril cp ON r.claim_peril_id = cp.claim_peril_id  
    WHERE cp.claim_id = @ClaimID) 'CurrentRecovery',
--Start (Vijayakumar Ramasamy)- (Tech Spec WR25UIIC  Generate Documents  Close Claim.doc)-(6.2.1)
cs.code  
--End (Vijayakumar Ramasamy)- (Tech Spec WR25UIIC  Generate Documents  Close Claim.doc)-(6.2.1)
 FROM Claim c  
--Start (Vijayakumar Ramasamy)- (Tech Spec WR25UIIC  Generate Documents  Close Claim.doc)-(6.2.1)
LEFT JOIN claim_status cs ON c.claim_status_id = cs.claim_status_id
--End (Vijayakumar Ramasamy)- (Tech Spec WR25UIIC  Generate Documents  Close Claim.doc)-(6.2.1)
 WHERE c.claim_id = @ClaimID  
END  


