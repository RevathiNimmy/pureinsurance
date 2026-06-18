SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_calculate_claims_ri_xol_return_0'
GO

CREATE PROCEDURE spu_calculate_claims_ri_xol_return_0  
    @claim_id int,  
    @ri_arrangement_id int,  
    @total_reserve money,  
    @original_reserve money,  
    @original_payment money,  
    @revised_reserve money,  
    @revised_payment money  
AS  
  
    Declare  
        @reserve_ratio float,  
        @payment_ratio float  
  
    -- Get ratios for changed amounts  
    -- Note: Conversion is needed to ensure suitable precision!  
    Select  @reserve_ratio = Case When @original_reserve = 0 Then 0 Else  
                Convert(float, @revised_reserve) / @original_reserve End,  
            @payment_ratio = Case When @original_payment = 0 Then 0 Else  
                Convert(float, @revised_payment) / @original_payment End 

    Set @reserve_ratio = ISNULL(@reserve_ratio,0)
    Set @payment_ratio = ISNULL(@payment_ratio,0) 
  
    -- Update the treaty ri lines  
    UPDATE  claim_ri_arrangement_line  
    SET     this_reserve = this_reserve * @reserve_ratio,  
            this_payment = this_payment * @payment_ratio,  
            this_share_percent = Case When @total_reserve = 0 Then 0 Else  
                (this_reserve * @reserve_ratio) / @total_reserve * 100 End  
    WHERE   claim_id = @claim_id  
    AND     ri_arrangement_id = @ri_arrangement_id  
    AND     type = 'R'  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
