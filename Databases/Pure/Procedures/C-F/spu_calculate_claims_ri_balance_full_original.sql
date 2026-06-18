SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_calculate_claims_ri_balance_full_original'
GO

CREATE PROCEDURE spu_calculate_claims_ri_balance_full_original
    @claim_id int,
    @ri_arrangement_id int,
	@claim_allocation_type int = 2
AS

	DECLARE @current_this_reserve MONEY,@current_this_payment MONEY

    -- This just removes the original allocations from 'full' this_xxx ones to give true this_xxx values.
	IF ISNULL(@claim_allocation_type,0) =0 
	BEGIN
	
		UPDATE  claim_ri_arrangement_line
		SET     this_reserve = this_reserve - reserve
		WHERE   claim_id = @claim_id
		AND     ri_arrangement_id = @ri_arrangement_id
		
		UPDATE  claim_ri_arrangement_line
		SET     this_payment = this_payment - payment
		WHERE   claim_id = @claim_id
		AND     ri_arrangement_id = @ri_arrangement_id
		
	END

	ELSE
	BEGIN
		--IF ISNULL(@current_this_reserve,0)<>0 
		BEGIN
		UPDATE  claim_ri_arrangement_line
		SET     this_reserve = this_reserve - reserve
		WHERE   claim_id = @claim_id
		AND     ri_arrangement_id = @ri_arrangement_id
		AND type NOT IN ('F','R')
		END
		--IF ISNULL(@current_this_payment,0)<0
		BEGIN
		UPDATE  claim_ri_arrangement_line
		SET     this_payment = this_payment - payment
		WHERE   claim_id = @claim_id
		AND     ri_arrangement_id = @ri_arrangement_id
		AND type NOT IN ('F','R')
		END
	END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
