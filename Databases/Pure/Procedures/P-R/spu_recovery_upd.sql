--Start (Arul Stephen) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_recovery_upd'
GO

CREATE PROCEDURE spu_recovery_upd
		@recovery_id int,
		@claim_peril_id int,
    		@recovery_type_id int,  
    		@currency_id int,  
    		@initial_reserve money,  
    		@revised_reserve money,  
    		@received_to_date money,  
    		@revision_count int,  
 		@tax_amount money,
		@recovery_party_type_id int = NULL,
		@recovery_party_cnt  int = NULL
AS

    	UPDATE  recovery
    	SET     claim_peril_id = @claim_peril_id ,
            	recovery_type_id = @recovery_type_id,
            	currency_id = @currency_id,
            	initial_reserve = @initial_reserve,
            	revised_reserve = @revised_reserve,
            	received_to_date = @received_to_date,
            	revision_count = @revision_count,
      		tax_amount = @tax_amount,
		recovery_party_type_id = @recovery_party_type_id,
		recovery_party_cnt = @recovery_party_cnt
    WHERE	recovery_id = @recovery_id
 



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--End (Arul Stephen) - (Tech Spec WR34 - Claims Recovery Party Link.doc)