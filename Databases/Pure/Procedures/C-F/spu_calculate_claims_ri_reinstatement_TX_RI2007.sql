-- *****************************************************************************  
-- * PBI 26251: Reinstatement of XOL (Claims)  
-- * Author:   Shipali Sharma  
-- * Date:     2025-03-12  
-- * Purpose:  Apply reinstatement capacity cap for Per-Claim XOL treaties (TX)  
-- *           after standard layer calculation, with multicurrency support.  
-- *           Converts aggregate to treaty currency, checks capacity, triggers  
-- *           reinstatement or caps allocation at remaining capacity.  
-- * Bug 37997: Replace two-step read-check-write with a single atomic conditional  
-- *           UPDATE + @@ROWCOUNT to eliminate race condition under concurrent  
-- *           claim processing.  
-- *           CodeAnt review: @@ROWCOUNT = 0 does not mean no capacity — another  
-- *           session may have incremented the counter first. Re-read live counter  
-- *           before capping to avoid incorrectly capping a claim.  
-- *****************************************************************************  
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_calculate_claims_ri_reinstatement_TX_RI2007'
GO  
CREATE PROCEDURE spu_calculate_claims_ri_reinstatement_TX_RI2007  
    @claim_id               INT,  
    @ri_arrangement_id      INT,  
    @line_id                INT,  
    @ri_model_line_id       INT,  
    @this_reserve           MONEY OUTPUT,  
    @this_payment           MONEY OUTPUT  
AS  
BEGIN  
    DECLARE @current_aggregate      DECIMAL(18,2)  
    DECLARE @available_capacity     DECIMAL(18,2)  
    DECLARE @current_reinstatement_count INT  
    DECLARE @max_reinstatements     INT  
    DECLARE @treaty_id              INT  
    DECLARE @treaty_limit           MONEY  
    DECLARE @treaty_currency_id     INT  
    DECLARE @treaty_period_start    DATETIME  
    DECLARE @treaty_period_end      DATETIME  
    DECLARE @claim_to_treaty_rate   FLOAT  
  
    -- Get treaty configuration  
    SELECT   
        @treaty_id = t.treaty_id,  
        @treaty_limit = ISNULL(t.treaty_limit,0),   
        @treaty_currency_id = ISNULL(t.currency_id, 0),  
        @current_reinstatement_count = ISNULL(t.current_reinstatement_count, 0),  
        @max_reinstatements = ISNULL(t.reinstatements, 0),  
        @treaty_period_start = t.effective_date,  
        @treaty_period_end = t.expiry_date  
    FROM Claim_RI_Arrangement_Line cral  
    INNER JOIN ri_model_line rml ON cral.ri_model_line_id = rml.ri_model_line_id  
    INNER JOIN Treaty t ON rml.treaty_id = t.treaty_id  
    WHERE cral.ri_arrangement_line_id = @line_id  
  
    IF @treaty_id IS NULL OR ISNULL(@treaty_limit,0) = 0     
        RETURN  
  
    -- Calculate aggregate for treaty period with multicurrency conversion  
    SELECT @current_aggregate = ISNULL(SUM(  
        (ISNULL(cral.reserve, 0) +   
         ISNULL(cral.payment, 0) +   
         ISNULL(cral.salvage, 0) +   
         ISNULL(cral.recovery, 0))  
        * CASE   
            WHEN @treaty_currency_id = 0 OR @treaty_currency_id = inf.currency_id THEN 1.0  
            ELSE ISNULL(NULLIF(inf.currency_base_xrate, 0), 1.0)  
                 / ISNULL(NULLIF(cr_treaty.rate_against_base, 0), 1.0)  
          END  
    ), 0)  
    FROM Claim_RI_Arrangement_Line cral  
    INNER JOIN Claim c ON cral.claim_id = c.claim_id  
    INNER JOIN insurance_file inf ON c.policy_id = inf.insurance_file_cnt  
    OUTER APPLY (  
        SELECT TOP 1 cr2.rate_against_base  
        FROM CurrencyRate cr2  
        WHERE cr2.currency_id = @treaty_currency_id  
        AND cr2.effective_from <= c.loss_from_date  
        ORDER BY cr2.effective_from DESC  
    ) cr_treaty  
    WHERE cral.treaty_id = @treaty_id  
    AND cral.type = 'TX'  
    AND c.loss_from_date >= @treaty_period_start  
    AND (@treaty_period_end IS NULL OR c.loss_from_date <= @treaty_period_end)  
  
    -- Determine claim-to-treaty currency conversion rate  
    SET @claim_to_treaty_rate = 1.0  
  
    IF @treaty_currency_id > 0  
    BEGIN  
        DECLARE @claim_policy_currency_id SMALLINT  
        DECLARE @claim_policy_rate FLOAT  
        DECLARE @treaty_rate FLOAT  
        DECLARE @claim_loss_date DATETIME  
        DECLARE @claim_source_id INT  
          
        SELECT @claim_policy_currency_id = inf.currency_id,  
               @claim_policy_rate = inf.currency_base_xrate,  
               @claim_loss_date = c.loss_from_date,  
               @claim_source_id = inf.source_id  
        FROM Claim c  
        INNER JOIN insurance_file inf ON c.policy_id = inf.insurance_file_cnt  
        WHERE c.claim_id = @claim_id  
  
        IF ISNULL(@claim_policy_rate, 0) = 0  
         EXECUTE Spu_act_get_currency_rate  
                @claim_policy_currency_id, @claim_source_id,  
                @claim_loss_date, @claim_policy_rate OUTPUT  
  
        IF @claim_policy_currency_id <> @treaty_currency_id  
        BEGIN  
            SET @treaty_rate = 0  
            EXECUTE Spu_act_get_currency_rate  
                @treaty_currency_id, @claim_source_id,  
                @claim_loss_date, @treaty_rate OUTPUT  
  
            IF ISNULL(@treaty_rate, 0) > 0   
                SET @claim_to_treaty_rate = ISNULL(@claim_policy_rate, 1.0) / @treaty_rate  
        END  
    END  
  
    -- Available capacity in treaty currency  
    SET @available_capacity = @treaty_limit * (@current_reinstatement_count + 1)  
  
    -- Project aggregate using layer-calculated values  
    DECLARE @projected_aggregate MONEY  
    SET @projected_aggregate = @current_aggregate   
        + (@this_reserve + @this_payment) * @claim_to_treaty_rate  
  
    -- Check if reinstatement needed  
    IF @projected_aggregate > @available_capacity  
    BEGIN  
        -- BUG 37997 FIX: Atomic conditional UPDATE eliminates the TOCTOU race condition.  
        -- The WHERE clause acts as both the guard and the increment in one statement.  
        -- Only one concurrent session can satisfy the predicate; the other gets @@ROWCOUNT = 0.  
        -- CodeAnt: use ISNULL in the increment so a NULL column becomes 1, not NULL,
        -- preventing the < @max_reinstatements predicate from matching indefinitely.
        UPDATE Treaty  
        SET current_reinstatement_count = ISNULL(current_reinstatement_count, 0) + 1  
        WHERE treaty_id = @treaty_id  
          AND ISNULL(current_reinstatement_count, 0) < @max_reinstatements  
  
        IF @@ROWCOUNT = 1  
        BEGIN  
            SET @current_reinstatement_count = @current_reinstatement_count + 1  
            SET @available_capacity = @treaty_limit * (@current_reinstatement_count + 1)  
        END  
        ELSE  
        BEGIN  
            -- BUG 37997 (CodeAnt): @@ROWCOUNT = 0 does NOT necessarily mean no capacity remains.
            -- Another concurrent session may have already incremented the counter, so the
            -- pre-update @available_capacity is stale. Re-read the live counter before capping.
            SELECT @current_reinstatement_count = ISNULL(current_reinstatement_count, 0)
            FROM Treaty
            WHERE treaty_id = @treaty_id

            SET @available_capacity = @treaty_limit * (@current_reinstatement_count + 1)

            -- Only cap if the claim still exceeds capacity after refreshing the live counter
            IF @projected_aggregate <= @available_capacity
                RETURN

            -- No reinstatement available - cap at remaining capacity  
            DECLARE @remaining_capacity_treaty MONEY  
            SET @remaining_capacity_treaty = @available_capacity - @current_aggregate  
            IF @remaining_capacity_treaty < 0  
                SET @remaining_capacity_treaty = 0  
  
            DECLARE @remaining_capacity MONEY  
            IF ISNULL(@claim_to_treaty_rate, 0) > 0  
                SET @remaining_capacity = @remaining_capacity_treaty / @claim_to_treaty_rate  
            ELSE  
                SET @remaining_capacity = @remaining_capacity_treaty  
              
            DECLARE @total_allocation MONEY  
            SET @total_allocation = @this_reserve + @this_payment  
              
            IF @total_allocation > @remaining_capacity  
            BEGIN  
                DECLARE @allocation_ratio FLOAT  
                IF @total_allocation > 0  
                    SET @allocation_ratio = @remaining_capacity / @total_allocation  
                ELSE  
                    SET @allocation_ratio = 0  
                      
                SET @this_reserve = @this_reserve * @allocation_ratio  
                SET @this_payment = @this_payment * @allocation_ratio  
            END  
        END  
    END  
END  