SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_calculate_claims_ri_method_0_full'
GO

CREATE PROCEDURE spu_calculate_claims_ri_method_0_full  
    @claim_id int,  
    @ri_arrangement_id int,  
    @total_reserve money,  
    @total_payment money  
AS  
    Declare @sum_insured money  
    -- Update the treaty ri lines  
    -- Note: Always using default percentages and ignore XOL lines  
    UPDATE  claim_ri_arrangement_line    
    SET     this_share_percent = ISNULL(this_share_percent, default_share_percent),    
            this_reserve = CONVERT(money, (ISNULL(this_share_percent, default_share_percent) / 100 * @total_reserve)),    
            this_payment = CONVERT(money, (ISNULL(this_share_percent, default_share_percent) / 100 * @total_payment))       
    WHERE   claim_id = @claim_id    
    AND     ri_arrangement_id = @ri_arrangement_id    
    AND     type In ('R', 'T', 'F', 'FX', 'TX')   
   
  
 Select @sum_insured = cra.sum_insured  from claim_ri_arrangement cra    
    WHERE   cra.claim_id = @claim_id    
    AND     cra.ri_arrangement_id = @ri_arrangement_id  
  
 IF @sum_insured = 0.0        
 BEGIN        
   UPDATE  claim_ri_arrangement_line  
    SET     this_share_percent = default_share_percent,  
            this_reserve = CONVERT(money, (default_share_percent / 100 * @total_reserve)),  
            this_payment = CONVERT(money, (default_share_percent / 100 * @total_payment))  
    WHERE   claim_id = @claim_id  
    AND     ri_arrangement_id = @ri_arrangement_id  
    AND     type In ('R', 'T', 'F', 'FX', 'TX')        
 END   



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
