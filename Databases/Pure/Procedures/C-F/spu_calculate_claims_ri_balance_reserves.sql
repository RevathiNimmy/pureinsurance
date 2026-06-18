SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_calculate_claims_ri_balance_reserves'
GO

CREATE PROCEDURE spu_calculate_claims_ri_balance_reserves  
    @claim_id int,  
    @ri_arrangement_id int  
AS    
    -- This just removes the original allocations from 'full' this_xxx ones to give true this_xxx values.  
    UPDATE  claim_ri_arrangement  
    SET     this_reserve = payment - reserve,  
            this_payment = 0  
    WHERE   claim_id = @claim_id  
    AND     ri_arrangement_id = @ri_arrangement_id  
  
    UPDATE  claim_ri_arrangement_line  
    SET     this_reserve = payment - reserve,  
            this_payment = 0  
    WHERE   claim_id = @claim_id  
    AND     ri_arrangement_id = @ri_arrangement_id  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
