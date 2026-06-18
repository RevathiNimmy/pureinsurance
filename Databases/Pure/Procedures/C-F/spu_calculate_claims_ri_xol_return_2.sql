SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_calculate_claims_ri_xol_return_2'
GO

CREATE PROCEDURE spu_calculate_claims_ri_xol_return_2  
    @claim_id int,  
    @ri_arrangement_id int,  
    @total_reserve money,  
    @original_reserve money,  
    @original_payment money,  
    @revised_reserve money,  
    @revised_payment money  
AS  
  
    -- Note: This method will NOT allocate negative TOTAL reserve or payment amounts  
  
    DECLARE  
        -- The line details for the reinsurance  
        @line_id int,  
        @sum_insured money,  
        -- The reserves and payments to be allocated to an RI line  
        @this_reserve money,  
        @this_payment money,  
        -- The difference to reallocate to retained  
        @os_reserve money,  
        @os_payment money  
  
    -- Outstanding allocations are just the new revised amounts  
    SELECT  @os_reserve = @revised_reserve,  
            @os_payment = @revised_payment  
  
    -- Declare the cursor  
    DECLARE RI_Cursor CURSOR FAST_FORWARD READ_ONLY FOR  
        -- Reallocate all retained lines against new amount  
        SELECT  ri_arrangement_line_id,  
                sum_insured  
        FROM    claim_ri_arrangement_line  
        WHERE   claim_id = @claim_id  
        AND     ri_arrangement_id = @ri_arrangement_id  
        AND     type = 'R' -- We are only return retained premiums  
        -- As we are allocating additions we should do this from the top down so sort ascending  
        ORDER BY  
                priority ASC, number_of_lines ASC  
  
    -- Open treaty cursor  
    OPEN RI_Cursor  
    FETCH NEXT FROM RI_Cursor  
        INTO @line_id, @sum_insured  
  
    WHILE @@FETCH_STATUS = 0 BEGIN  
        -- Reset values  
        SELECT  @this_reserve = 0,  
                @this_payment = 0  
  
        -- If we have remaining reserve allocate it  
        IF @os_reserve > 0 BEGIN  
            -- Decide how much to allocate  
            IF @sum_insured > @os_reserve  
                -- Allocate all the reserve  
                SELECT  @this_reserve = @os_reserve,  
                        @os_reserve = 0  
            ELSE  
                -- Allocate up to sum insured  
                SELECT  @this_reserve = @sum_insured,  
                        @os_reserve = @os_reserve - @sum_insured  
        END  
  
        -- If we have payments and spare reserve allocate them  
        IF @os_payment > 0 BEGIN  
            -- Decide how much to allocate  
            IF @this_reserve > @os_payment  
                -- Alloacte all of the payment  
                SELECT  @this_payment = @os_payment,  
                        @os_payment = 0  
            ELSE  
                -- Allocate up to the full reserve value  
                SELECT  @this_payment = @this_reserve,  
                        @os_payment = @os_payment - @this_reserve  
        END  
  
        -- Update line with new values  
        UPDATE  claim_ri_arrangement_line  
       SET     this_reserve = (@this_reserve-reserve),
                this_payment = (@this_payment-payment),
                this_share_percent = CASE WHEN @total_reserve > 0 THEN  
                                         CONVERT(float, @this_reserve) / @total_reserve * 100  
                                     ELSE 0 END  
        WHERE   claim_id = @claim_id  
        AND     ri_arrangement_line_id = @line_id  
  
        FETCH NEXT FROM RI_Cursor  
            INTO @line_id, @sum_insured  
    END  
  
    CLOSE RI_Cursor  
    DEALLOCATE RI_Cursor  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
