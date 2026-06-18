--Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claims Recovery Reinsurance
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_all_Receipts_for_claim'
GO

CREATE PROCEDURE spu_get_all_Receipts_for_claim       
   @claim_id INT,    
   @RecoveryType Int,  
   @RecoveryID INT,
   @nSalvageAndTPRecoveryReceipts TINYINT     
AS      
BEGIN  
IF @nSalvageAndTPRecoveryReceipts = 0 
BEGIN     
   SELECT      
       cr.claim_Receipt_id,      
       cr.date_of_Receipt,      
       ISNULL(pty.resolved_name,'Claim Receipt Suspense'),      
       cr.PayeeName,      
       r.this_Receipt,      
       c.description,      
       r.this_Receipt*ISNULL(r.Receipt_loss_xrate,1) [loss amount],      
       r.this_Receipt*ISNULL(r.currency_base_xrate,1) [base amount],      
       r.currency_id [Receipt currency],      
       cl.currency_id [loss currency],      
       i.base_currency_id,      
       ISNULL(r.tax_amount,0)     
    
       FROM  Claim_Receipt_Item r      
      
       INNER JOIN Claim_Receipt cr ON      
       r.claim_Receipt_id = cr.claim_Receipt_id      
      
       JOIN Currency c ON      
        c.currency_id=r.currency_id      
      
       LEFT JOIN Party pty ON      
        pty.party_cnt=cr.party_cnt      
      
       JOIN Claim cl ON      
        cl.claim_id=cr.claim_id      
      
       JOIN Insurance_File i ON      
        i.insurance_file_cnt= cl.policy_id      
 JOIN Recovery re ON r.recovery_id = re.Recovery_id  
 JOIN recovery_type rt ON r.recovery_type_id = rt.recovery_type_id  
      
      WHERE cr.claim_id= @claim_id       
          And rt.Is_Salvage=isNull(@Recoverytype,rt.is_salvage)  And r.Recovery_id=isNull(@Recoveryid,r.Recovery_Id)  
     AND r.this_Receipt<> 0  
END    
ELSE
BEGIN
   SELECT      
       cr.claim_Receipt_id,      
       cr.date_of_Receipt,      
       ISNULL(pty.resolved_name,'Claim Receipt Suspense'),      
       cr.PayeeName,      
       r.this_Receipt,      
       c.description,      
       r.this_Receipt*ISNULL(r.Receipt_loss_xrate,1) [loss amount],      
       r.this_Receipt*ISNULL(r.currency_base_xrate,1) [base amount],      
       r.currency_id [Receipt currency],      
       cl.currency_id [loss currency],      
       i.base_currency_id,      
       ISNULL(r.tax_amount,0)     
    
       FROM  Claim_Receipt_Item r      
      
       INNER JOIN Claim_Receipt cr ON      
       r.claim_Receipt_id = cr.claim_Receipt_id      
      
       JOIN Currency c ON      
        c.currency_id=r.currency_id      
      
       LEFT JOIN Party pty ON      
        pty.party_cnt=cr.party_cnt      
      
       JOIN Claim cl ON      
        cl.claim_id=cr.claim_id      
      
       JOIN Insurance_File i ON      
        i.insurance_file_cnt= cl.policy_id      
 JOIN Recovery re ON r.recovery_id = re.Recovery_id  
 JOIN recovery_type rt ON r.recovery_type_id = rt.recovery_type_id  
      
      WHERE cr.claim_id= @claim_id       
		  And r.Recovery_id=isNull(@Recoveryid,r.Recovery_Id)  
		  AND r.this_Receipt<> 0  
END  
END  
