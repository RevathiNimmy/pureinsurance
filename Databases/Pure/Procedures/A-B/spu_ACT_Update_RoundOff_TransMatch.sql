SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_ACT_Update_RoundOff_TransMatch'
GO
CREATE PROC spu_ACT_Update_RoundOff_TransMatch    
@transdetailid_roundoff INT,    
@transdetailid_snd INT,  
@roundoff_amount MONEY    
    
AS    
DECLARE @match_id INT,    
 @allocationdetail_id INT    
    
SELECT  @match_id=match_id,@allocationdetail_id=allocationdetail_id    
FROM Transmatch     
WHERE transdetail_id=@transdetailid_snd     
    
UPDATE  Transmatch     
SET match_id=@match_id,allocationdetail_id=@allocationdetail_id    
WHERE  transdetail_id=@transdetailid_roundoff    
  
UPDATE AllocationDetail  
SET round_off_amount=@roundoff_amount,alloc_base_amount=alloc_base_amount+(@roundoff_amount),alloc_ccy_amount=alloc_ccy_amount+(@roundoff_amount)  
WHERE  transdetail_id=@transdetailid_snd     
  
UPDATE  Transmatch     
SET base_match_amount=base_match_amount+(@roundoff_amount),  
currency_match_amount=currency_match_amount+(@roundoff_amount),  
account_match_amount=account_match_amount+(@roundoff_amount),  
system_match_amount=system_match_amount+(@roundoff_amount)  
WHERE  transdetail_id=@transdetailid_snd    
  
UPDATE TransDetail   
SET outstanding_amount =0,  
outstanding_currency_amount=0,  
outstanding_account_amount=0,  
outstanding_system_amount=0  
WHERE  transdetail_id=@transdetailid_roundoff  
  
GO

