SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_recovery_saa'
GO
create PROCEDURE spu_recovery_saa   
      @peril_id int,    
      @is_salvage tinyint,  
      @claimReceiptId int=0   
  
AS      
IF @claimReceiptId=0  
Begin  
		SELECT    
			wr.recovery_id,    
			wr.claim_peril_id,    
			rt.recovery_type_id,    
 			rt.description as recovery_description,    
	 		c.currency_id,    
 			c.description as currency_description,    
	 		wr.initial_reserve,    
		 	wr.revised_reserve,    
			wr.received_to_date,    
			wr.revision_count,    
			wr.tax_amount,    
			wcp.claim_id,    
			rty.claims_is_post_taxes,    
			wr.base_recovery_id,
			--Start (Arul Stephen) - (Tech Spec WR34 - Claims Recovery Party Link.doc)    
			     rt.code recovery_type_code,    
			     wr.recovery_party_type_id,    
			     wr.recovery_party_cnt ,  
			     p.ShortName,  
			     p.Resolved_Name   
			--End (Arul Stephen) - (Tech Spec WR34 - Claims Recovery Party Link.doc)                
     
			FROM    recovery wr    
			     JOIN    claim_peril wcp    
			             ON wcp.claim_peril_id = wr.claim_peril_id    
			     JOIN    recovery_type rt    
			             ON rt.recovery_type_id = wr.recovery_type_id    
			     JOIN    currency c    
			             ON c.currency_id = wr.currency_id    
			     JOIN    claim wc    
			             ON wc.claim_id = wcp.claim_id    
			      
			     JOIN    risk r    
			             ON r.risk_cnt = wc.risk_type_id    
			     JOIN    risk_type rty    
			             ON rty.risk_type_id = r.risk_type_id   
			   --Start (Arul Stephen) - (Tech Spec WR34 - Claims Recovery Party Link.doc) 
			   left Join   party p -- client details    
			           On p.party_cnt =  wr.recovery_party_cnt
			     
			    left Join  Recovery_Party_Type  rpt on rpt.Recovery_Party_Type_id=wr.recovery_party_type_id  
			 --End (Arul Stephen) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
			  
			     WHERE   wr.claim_peril_id = @peril_id    
			     AND     rt.is_salvage = @is_salvage    
			     ORDER BY    
			             wr.recovery_id    
End  
--Start(Saurabh Agrawal) (Tech Spec QBENZCR004 - Claims Recovery Reinsurance.doc)
Else  
Begin  
		Select   
			wr.recovery_id,    
			wr.claim_peril_id,    
			rt.recovery_type_id,    
			rt.description as recovery_description,    
			c.currency_id,    
			c.description as currency_description,    
			wr.initial_reserve,    
			wr.revised_reserve,    
			wr.received_to_date,    
			wr.revision_count,    
			wr.tax_amount,    
			wcp.claim_id,    
			rty.claims_is_post_taxes,    
			wr.base_recovery_id,    
		--Start (Arul Stephen) - (Tech Spec WR34 - Claims Recovery Party Link.doc)    
			rt.code recovery_type_code,    
			wr.recovery_party_type_id,    
			wr.recovery_party_cnt ,  
			p.ShortName,  
			p.Resolved_Name   
		--End (Arul Stephen) - (Tech Spec WR34 - Claims Recovery Party Link.doc)     
		FROM    recovery wr    
		JOIN    claim_peril wcp    
		ON wcp.claim_peril_id = wr.claim_peril_id    
		JOIN    recovery_type rt    
		ON rt.recovery_type_id = wr.recovery_type_id    
		JOIN    currency c    
		ON c.currency_id = wr.currency_id    
		JOIN    claim wc    
		ON wc.claim_id = wcp.claim_id    
		JOIN    risk r ON r.risk_cnt = wc.risk_type_id    
		JOIN    risk_type rty    
		ON rty.risk_type_id = r.risk_type_id 
		   --Start (Arul Stephen) - (Tech Spec WR34 - Claims Recovery Party Link.doc) 
		   left Join   party p -- client details    
		           On p.party_cnt =  wr.recovery_party_cnt
		     
		    left Join  Recovery_Party_Type  rpt on rpt.Recovery_Party_Type_id=wr.recovery_party_type_id  
		 --End (Arul Stephen) - (Tech Spec WR34 - Claims Recovery Party Link.doc)   
		WHERE   rt.is_salvage = @is_salvage and  wr.claim_peril_id =(select claim_peril_id from claim_receipt where  claim_receipt_id=@claimReceiptId)  
		ORDER BY    
		 wr.recovery_id  
End  
GO
SET ANSI_NULLS ON
GO
--End Saurabh Agrawal (Tech Spec QBENZCR004 - Claims Recovery Reinsurance.doc)  
