SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CDT_Transaction_Recovery_Update'
GO

CREATE PROCEDURE spu_SAM_CDT_Transaction_Recovery_Update  

@base_claim_peril_id int, 
@version_id  int, 
@type_code varchar(50),  
@this_revision money  
  
AS  

BEGIN

UPDATE recovery SET
	revised_reserve = ISNULL(revised_reserve, 0) + ISNULL(@this_revision,0), 
	revision_count =ISNULL(revision_count,0) + 1 
		
WHERE recovery_id IN (SELECT recovery_id FROM recovery 
		     WHERE recovery_type_id = (SELECT recovery_type_id FROM recovery_type WHERE code = @type_code) 
                     AND claim_peril_id in (SELECT claim_peril_id FROM Claim_Peril WHERE base_claim_peril_id = @base_claim_peril_id And version_id = @version_id))

END


GO
