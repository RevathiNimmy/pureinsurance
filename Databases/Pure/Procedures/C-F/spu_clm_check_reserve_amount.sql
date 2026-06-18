SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_clm_check_reserve_amount'
GO

CREATE procedure spu_clm_check_reserve_amount  
@ReserveAmount NUMERIC(19,4) OUT ,  
@TransactionType char(10),  
@ClaimId INT  
as  
  
BEGIN  
  
IF (@TransactionType ='C_CO' or @TransactionType ='C_CR' or @TransactionType ='C_CP')  
SELECT @ReserveAmount = Case @TransactionType  
      WHEN 'C_CO' THEN SUM(ISNUll(Initial_reserve, 0))  
      WHEN 'C_CR' THEN SUM(ISNUll(this_revision, 0))  
      WHEN 'C_CP' THEN SUM(ISNUll(this_payment, 0)) END  
      FROM Reserve r  
      JOIN Claim_Peril cp ON cp.Claim_Peril_id = r.claim_Peril_id  
      WHERE cp.Claim_id = @claimId  
	  GROUP BY  cp.Claim_id   
     
IF (@TransactionType ='C_SA' OR @TransactionType ='C_RV')  
SELECT @ReserveAmount = SUM(ISNULL(r.this_receipt_net, 0))  
	FROM Recovery r 
		JOIN Claim_Peril cp ON cp.Claim_Peril_id = r.claim_Peril_id
		JOIN claim c on cp.claim_id =c.claim_id  
		WHERE CP.claim_id = @ClaimId AND ISNULL(r.this_receipt_net, 0)  <> 0  
		GROUP BY  cp.Claim_id  
  
    
SELECT @ReserveAmount = ISNUll(@ReserveAmount,0)  
END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
