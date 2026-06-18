--Start (Arul Stephen) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_recovery_add'
GO

CREATE PROCEDURE spu_recovery_add
		@recovery_id int output,
		@claim_peril_id int,
    		@recovery_type_id int,
    		@currency_id int,
    		@initial_reserve money,
    		@revised_reserve money,
    		@received_to_date money,
    		@revision_count int, 
    		@tax_amount money, 
    		@recovery_party_type_id INT = NULL,  
    		@recovery_party_cnt  INT = NULL  
AS 
 
	DECLARE @version_id int 
 
	EXEC spu_CLM_Get_Claim_Version 
			@claim_peril_id = @claim_peril_id, 
			@version_id = @version_id OUTPUT 
 
	INSERT INTO recovery ( 
		claim_peril_id , 
		recovery_type_id, 
		currency_id, 
		initial_reserve , 
		revised_reserve, 
		received_to_date, 
		revision_count, 
		tax_amount, 
		version_id, 
		recovery_party_type_id, 
		recovery_party_cnt 
	) 
	VALUES ( 
		@claim_peril_id, 
		@recovery_type_id, 
		@currency_id, 
		@initial_reserve, 
		@revised_reserve, 
		@received_to_date, 
		@revision_count, 
		@tax_amount, 
		@version_id, 
		@recovery_party_type_id, 
		@recovery_party_cnt 
	) 
 
	SELECT @recovery_id = @@IDENTITY 
 
	UPDATE recovery 
	SET base_recovery_id = @recovery_id 
	WHERE recovery_id = @recovery_id  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--End (Arul Stephen) - (Tech Spec WR34 - Claims Recovery Party Link.doc)

