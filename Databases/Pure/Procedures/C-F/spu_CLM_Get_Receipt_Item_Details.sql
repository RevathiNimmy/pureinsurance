--Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery reinsurance    
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_CLM_Get_Receipt_Item_Details'  
Go
CREATE PROCEDURE spu_CLM_Get_Receipt_Item_Details      
 @claim_Receipt_id INT      
      
AS      
      
BEGIN


DECLARE      @enumAgent             CHAR(1) = '1'
            ,@enumClient             CHAR(1) = '2'
            ,@enumInsurer           CHAR(1) = '3'
            , @enumParty            CHAR(1) = '4'

DECLARE      @enumAgentCode   NVARCHAR(2) = 'AG'
            ,@enumClientCode  NVARCHAR(2) = 'PC'
            ,@enumInsurerCode NVARCHAR(2) = 'IN'


 SELECT
 r.recovery_id,
 rt.description,
 cri.this_receipt,
 cri.tax_amount,
 cri.currency_id,
 cri.currency_base_xrate,
 cri.tax_group_id,
 r.initial_reserve + r.revised_reserve as total_reserve,
 r.received_to_date,
 (r.initial_reserve + r.revised_reserve) - r.received_to_date as balance,
 cri.receipt_loss_xrate,
 c.description,
 tg.description,
 tg.is_withholding_tax,
 tg.advanced_tax_script,
 cri.claim_receipt_item_id,
 r.revised_reserve,
   --Start (Arul Stephen) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
   rt.code recovery_type_code,  

   (CASE 
		WHEN pt.Code   = @enumAgentCode   THEN @enumAgent           -- Agent(AG)
		WHEN pt.Code   = @enumClientCode  THEN @enumClient          -- Personal Client(PC)
		WHEN pt.Code   = @enumInsurerCode THEN @enumInsurer         -- Insurer(IN)
		ELSE r.recovery_party_type_id END) recovery_party_type_id,                            

   cr.party_cnt ,
   p.ShortName,
   p.Resolved_Name
  --End (Arul Stephen) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
 FROM claim_Receipt_item cri
  INNER JOIN Claim_Receipt cr ON
       cri.claim_Receipt_id = cr.claim_Receipt_id

  INNER JOIN recovery r ON
   cri.recovery_id = r.recovery_id

  INNER JOIN recovery_type rt ON
   r.recovery_type_id =rt.recovery_type_id

  INNER JOIN Currency c ON
   c.Currency_Id = cri.Currency_Id

  LEFT OUTER JOIN Tax_Group tg ON
   tg.tax_group_id = cri.tax_group_id
        --Start (Arul Stephen) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
     LEFT JOIN   party p -- client details
             ON p.party_cnt =  cr.party_cnt
LEFT  JOIN Party_Type pt ON pt.party_type_id = p.party_type_id

      left Join  Recovery_Party_Type  rpt on rpt.Recovery_Party_Type_id=r.recovery_party_type_id
   --End (Arul Stephen) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

 WHERE cri.claim_receipt_id =@claim_receipt_id

END
--End (Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery reinsurance

  
