SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CDT_Transaction_Reserve_Update'
GO

CREATE PROCEDURE spu_SAM_CDT_Transaction_Reserve_Update  

@base_claim_peril_id int, 
@version_id  int, 
@type_code varchar(50),  
@this_revision money  
  
AS  

BEGIN

UPDATE Reserve SET
	this_revision = ISNULL(@this_revision,0), 
	revised_reserve = ISNULL(revised_reserve, 0) + ISNULL(@this_revision,0), 

	-- if a payment has already been made in this 
	-- session dont increment revision count
	-- otherwise do
	revision_count = 
		CASE WHEN ISNULL(this_payment,0) = 0 THEN
			ISNULL(revision_count,0) + 1 
		ELSE
			ISNULL(revision_count,0)
		END, 

      	average =  
		CASE WHEN ISNULL(sum_insured,0) <> 0 THEN  
	     		((ISNULL(initial_reserve,0) + ISNULL(revised_reserve,0) + @this_revision) / ISNULL(sum_insured,0)) * 100  
		ELSE 0  
		END

WHERE reserve_id IN (SELECT reserve_id FROM reserve 
		     WHERE reserve_type_id = (SELECT reserve_type_id FROM reserve_type WHERE name = @type_code) 
                     AND claim_peril_id in (SELECT claim_peril_id FROM Claim_Peril WHERE base_claim_peril_id = @base_claim_peril_id And version_id = @version_id))

END


GO
