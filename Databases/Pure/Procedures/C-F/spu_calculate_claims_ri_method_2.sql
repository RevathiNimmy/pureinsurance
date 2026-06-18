SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_calculate_claims_ri_method_2'
GO

create PROCEDURE spu_calculate_claims_ri_method_2  
    @claim_id int,  
    @ri_arrangement_id int,  
    @reserve money,  
    @payment money  
AS  
  
    DECLARE  
        -- The total reserve for the band,  
        -- This will be converted to float as it is only used for share percentage calcs  
        @band_total_reserve float,  
        @this_share_percent float,  
        -- The line details for the reinsurance  
        @line_id int,  
        @sum_insured money,  
        @reserve_to_date money,  
        @payment_to_date money,  
        -- The reserves and payments to be allocated to an RI line  
        @this_reserve money,  
        @this_payment money,  
        -- The total of reserves after this allocation  
        @total_reserve money  
  
    -- Get band total reserve for share percentage calculation  
    SELECT  @band_total_reserve = CONVERT(float, reserve + this_reserve)  
    FROM    claim_ri_arrangement  
    WHERE   claim_id = @claim_id  
    AND     ri_arrangement_id = @ri_arrangement_id  
  
    -- See if we have any negative amounts to allocate  
    IF (@reserve < 0) OR (@payment < 0) BEGIN  
        -- Declare the cursor  
        DECLARE RI_Cursor CURSOR FAST_FORWARD READ_ONLY FOR  
            -- Select the treaties with standard priority, method and lines  
            -- Note: We do NOT allocate XOL here  
            SELECT  ri_arrangement_line_id,  
                    sum_insured,  
                    reserve,  
                    payment  
            FROM    claim_ri_arrangement_line  
            WHERE   claim_id = @claim_id  
            AND     ri_arrangement_id = @ri_arrangement_id  
            AND     type IN ('R', 'T', 'F') -- Do not update XOL  
            -- As we are allocating returns we should do this from the bottom up so sort descending  
            ORDER BY  
                    priority DESC, number_of_lines DESC  
  
        -- Open treaty cursor  
        OPEN RI_Cursor  
        FETCH NEXT FROM RI_Cursor  
            INTO @line_id, @sum_insured, @reserve_to_date, @payment_to_date  
  
        WHILE @@FETCH_STATUS = 0 BEGIN  
            -- Reset values  
            SELECT  @this_reserve = 0,  
                    @this_payment = 0  
  
            -- If we have a negative reserve allocate it  
            IF (@reserve < 0) AND (@reserve_to_date > 0) BEGIN  
                -- Decide how much to allocate  
                IF (@reserve + @reserve_to_date > 0)  
                    -- Allocate all the reserve  
                    SELECT  @this_reserve = @reserve,  
                            @reserve = 0  
                ELSE  
                    -- Allocate down to zero  
                    SELECT  @this_reserve = 0 - @reserve_to_date,  
                            @reserve = @reserve - @this_reserve  
            END  
  
            -- If we have reduced payments and have spare reverse allocate them  
            IF (@payment < 0) AND (@payment_to_date > 0) BEGIN  
                -- Decide how much to allocate  
                IF (@payment + @payment_to_date > 0)  
                    -- Alloacte all of the payment  
                    SELECT  @this_payment = @payment,  
                            @payment = 0  
                ELSE  
                    -- Allocate down to zero  
                    SELECT  @this_payment = 0 - @payment_to_date,  
                            @payment = @payment - @this_payment  
            END  
  
            IF (@band_total_reserve = 0)  
  Select @this_share_percent = default_share_percent  
      FROM claim_ri_arrangement_line  
             WHERE claim_id = @claim_id AND ri_arrangement_line_id = @line_id  
     Else  
  Select @this_share_percent = CONVERT(float, reserve + @this_reserve) / @band_total_reserve * 100  
      FROM claim_ri_arrangement_line  
             WHERE claim_id = @claim_id AND ri_arrangement_line_id = @line_id  
  
            -- Update line with new values  
            UPDATE  claim_ri_arrangement_line  
            SET     this_reserve = @this_reserve,  
                    this_payment = @this_payment 
            WHERE   claim_id = @claim_id  
            AND     ri_arrangement_line_id = @line_id  
  
            FETCH NEXT FROM RI_Cursor  
                INTO @line_id, @sum_insured, @reserve_to_date, @payment_to_date  
        END  
  
        CLOSE RI_Cursor  
        DEALLOCATE RI_Cursor  
    END -- (@reserve < 0) OR (@payment < 0)  
  
    -- See if we have any negative amounts to allocate  
    IF (@reserve > 0) OR (@payment > 0) BEGIN  
        -- Declare the cursor  
        DECLARE RI_Cursor CURSOR FAST_FORWARD READ_ONLY FOR  
            -- Select the treaties with standard priority, method and lines  
            -- Note: We do NOT allocate XOL here  
            SELECT  ri_arrangement_line_id,  
                    sum_insured,  
                    reserve,  
                    payment  
            FROM    claim_ri_arrangement_line  
            WHERE   claim_id = @claim_id  
            AND     ri_arrangement_id = @ri_arrangement_id  
            AND     type IN ('R', 'T', 'F') -- Do not update XOL  
            -- As we are allocating additions we should do this from the top down so sort ascending  
            ORDER BY  
                    priority ASC, number_of_lines ASC  
  
        -- Open treaty cursor  
        OPEN RI_Cursor  
        FETCH NEXT FROM RI_Cursor  
            INTO @line_id, @sum_insured, @reserve_to_date, @payment_to_date  
  
        WHILE @@FETCH_STATUS = 0 BEGIN  
            -- Reset values  
            SELECT  @this_reserve = 0,  
                    @this_payment = 0,  
                    @total_reserve = @reserve_to_date  
  
            -- If we have remaining reserve allocate it  
            IF (@reserve > 0) AND (@reserve_to_date < @sum_insured) BEGIN  
                -- Decide how much to allocate  
                IF (@sum_insured - @reserve_to_date) > @reserve  
                    -- Allocate all the reserve  
                    SELECT  @this_reserve = @reserve,  
                            @total_reserve = @reserve_to_date + @this_reserve,  
                            @reserve = 0  
                ELSE  
                    -- Allocate up to sum insured  
                    SELECT  @this_reserve = @sum_insured - @reserve_to_date,  
                            @total_reserve = @reserve_to_date + @this_reserve,  
                            @reserve = @reserve - @this_reserve  
            END  
  
            -- If we have payments and spare reserve allocate them  
            IF (@payment > 0) AND (@payment_to_date < @total_reserve) BEGIN  
                -- Decide how much to allocate  
                IF (@total_reserve - @payment_to_date) > @payment  
                    -- Alloacte all of the payment  
                    SELECT  @this_payment = @payment,  
                            @payment = 0  
                ELSE  
                    -- Allocate up to the full reserve value  
                    SELECT  @this_payment = @total_reserve - @payment_to_date,  
                            @payment = @payment - @this_payment  
            END  
  	    -- Start Girija - PN 55676
            --IF (@band_total_reserve = 0)  
  	     Select @this_share_percent = default_share_percent from claim_ri_arrangement_line  
             WHERE claim_id = @claim_id AND ri_arrangement_line_id = @line_id  
    -- Else  
 -- Select @this_share_percent = CONVERT(float, @this_reserve) / @band_total_reserve * 100  
-- End Girija - PN 55676
  
            -- Update line with new values  
            UPDATE  claim_ri_arrangement_line  
            SET     this_reserve = @this_reserve,  
                    this_payment = @this_payment
            WHERE   claim_id = @claim_id  
            AND     ri_arrangement_line_id = @line_id  
  
            FETCH NEXT FROM RI_Cursor  
                INTO @line_id, @sum_insured, @reserve_to_date, @payment_to_date  
        END  
  
        CLOSE RI_Cursor  
        DEALLOCATE RI_Cursor  
    END -- (@reserve > 0) OR (@payment > 0)  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
