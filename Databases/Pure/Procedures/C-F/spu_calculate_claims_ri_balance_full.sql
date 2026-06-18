SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_calculate_claims_ri_balance_full'
GO

CREATE PROCEDURE spu_calculate_claims_ri_balance_full  
    @claim_id int,  
    @ri_arrangement_id int  
AS  
  
    -- This just removes the original allocations from 'full' this_xxx ones to give true this_xxx values.  
    UPDATE  claim_ri_arrangement_line  
    SET     this_reserve = this_reserve - reserve  
    WHERE   claim_id = @claim_id  
    AND     ri_arrangement_id = @ri_arrangement_id  
    AND this_reserve <> 0 

    UPDATE  claim_ri_arrangement_line  
    SET     this_payment = this_payment - payment  
    WHERE   claim_id = @claim_id  
    AND     ri_arrangement_id = @ri_arrangement_id  
    AND this_payment <> 0 






GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
