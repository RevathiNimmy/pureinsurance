EXECUTE DDLDROPPROCEDURE spu_Calculate_Claims_Incurred_to_date
GO

CREATE PROCEDURE spu_Calculate_Claims_Incurred_to_date      
@claim_id INT,      
@ri_arrangement_id INT,      
@IsCreated INT = 0      
AS      

DECLARE @current_reserve_cr MONEY, @total_reserve MONEY, @ri_model_id INT, @xol_ri_model_id INT,
        @base_claim_id INT, @prop_ri_calculation_method INT, @ri_band_id INT,
        @IsPortfolioTransferred TINYINT = 0, @DecimalPlaces TINYINT,
        @cover_start_date_ForRi DATETIME, @Insurance_file_cnt INT

-- Get basic claim info
SELECT @Insurance_file_cnt = Policy_id, @base_claim_id = base_claim_id 
FROM claim WHERE claim_id = @claim_id

SELECT @cover_start_date_ForRi = inception_date_tpi
FROM insurance_file (NOLOCK)
WHERE Insurance_file_cnt = @Insurance_file_cnt

-- Check portfolio transfer
IF EXISTS(SELECT 1 FROM Claim_pt_log CPT 
          INNER JOIN claim clm ON CPT.base_claim_id = clm.base_claim_id 
          WHERE clm.Claim_id = @claim_id) OR @IsCreated = 1
    SET @IsPortfolioTransferred = 1

-- Initialize arrangement data
UPDATE Claim_RI_Arrangement 
SET incurred_to_date = ISNULL(reserve_to_date,0) + ISNULL(salvage_to_date,0) + 
                      ISNULL(recovery_to_date,0) + ISNULL(this_reserve,0) + 
                      ISNULL(this_salvage,0) + ISNULL(this_recovery,0)
WHERE claim_id = @claim_id AND ri_arrangement_id = @ri_arrangement_id

UPDATE Claim_RI_Arrangement_Line 
SET claim_incurred_to_date = 0
WHERE claim_id = @claim_id AND ri_arrangement_id = @ri_arrangement_id AND is_pt_archive = 0

-- Get arrangement details
SELECT @current_reserve_cr = CASE 
           WHEN ISNULL(this_reserve,0) <> 0 
               THEN ISNULL(this_reserve,0) - ISNULL(this_payment,0)
           ELSE ISNULL(reserve_to_date,0) - ISNULL(payment_to_date,0)
                    - ISNULL(salvage_to_date,0) - ISNULL(recovery_to_date,0)
           END,
       @total_reserve = ISNULL(reserve_to_date,0) + ISNULL(this_reserve,0),
       @ri_model_id = ri_model_id, 
       @xol_ri_model_id = xol_ri_model_id
FROM Claim_RI_Arrangement 
WHERE claim_id = @claim_id AND ri_arrangement_id = @ri_arrangement_id

-- Get RI band and calculation method
SELECT @ri_band_id = cra.ri_band_id
FROM RI_Arrangement ra 
JOIN Claim_RI_Arrangement cra ON ra.ri_arrangement_id = cra.original_ri_arrangement_id
WHERE cra.claim_ri_arrangement_id = @ri_arrangement_id AND cra.claim_id = @claim_id

SELECT TOP 1 @prop_ri_calculation_method = Proportional_RI_Cal_Method
FROM RI_Band_version
WHERE ri_band_id = @ri_band_id
  AND CONVERT(DATE, effective_date, 23) <= CONVERT(DATE, @cover_start_date_ForRi, 23)
ORDER BY effective_date DESC

-- Use old calc logic if no QS or Surplus
IF NOT EXISTS(SELECT 1 FROM Claim_RI_Arrangement_line 
              WHERE ri_arrangement_id = @ri_arrangement_id AND type IN ('T','TFS'))
    SET @prop_ri_calculation_method = 1

-- Get decimal places for rounding
SELECT @DecimalPlaces = decimal_places
FROM Claim C
JOIN Insurance_File IFL ON IFL.insurance_file_cnt = C.Policy_id
JOIN Currency Cr ON Cr.currency_id = IFL.currency_id
WHERE claim_id = @claim_id

-- Net Sum Insured for manually added treaty share % derivation (mirrors CalcRI2007)
DECLARE @Net_SumInsured FLOAT
SELECT @Net_SumInsured = cra.Sum_insured - ISNULL((
    SELECT SUM(ISNULL(cral.Sum_insured,0))
    FROM Claim_RI_Arrangement_line cral
    WHERE cral.ri_arrangement_id = @ri_arrangement_id
      AND ((cral.Type IN ('F','FX') AND ISNULL(cral.retained,0) = 0)
           OR (cral.type = 'T' AND ISNULL(cral.is_obligatory,0) = 1))
), 0)
FROM Claim_RI_Arrangement cra
WHERE cra.claim_id = @claim_id AND cra.ri_arrangement_id = @ri_arrangement_id

-- Cursor variables
DECLARE @ri_arrangement_line_id INT, @type VARCHAR(3), @treaty_id INT, @lower_limit MONEY,
        @line_limit MONEY, @Reserve MONEY, @payment MONEY, @net_payment MONEY,
        @this_share_percent FLOAT, @is_pt_archive TINYINT, @Participation_Percent FLOAT,
        @current_reserve MONEY, @incurred MONEY, @NET_Recovery MONEY, @treaty_Type_id INT, @priority INT,
        @manually_added TINYINT, @sum_insured MONEY


--priority variables
    DECLARE
            @last_priority INT,
            @QsTotal NUMERIC(19, 5),
            @number_of_lines FLOAT,
            @priority_reserve MONEY,
			@priority_payment MONEY,
			@running_reserve MONEY,
			@running_payment MONEY,
            @first_priority INT,
            @tfs_total_reserve MONEY
            
 -- Set default values
        SELECT
            @last_priority = -666,
            @priority_reserve = 0,
            @tfs_total_reserve = 0;
-- Single cursor for all RI types ordered by priority
DECLARE ri_cursor CURSOR FOR
SELECT cral.ri_arrangement_line_id, cral.type, cral.treaty_id, ISNULL(cral.lower_limit,0), ISNULL(cral.line_limit,0),
       CASE WHEN cral.is_pt_archive = 0 
            THEN ISNULL(cral.Reserve,0) + ISNULL(cral.this_reserve,0) + ISNULL(cral.claim_incurred_to_date,0)
            ELSE ISNULL(cral.Reserve_to_date,0) END,
       CASE WHEN cral.is_pt_archive = 0 
            THEN ISNULL(cral.payment,0) + ISNULL(cral.this_payment,0) + ISNULL(cral.claim_incurred_to_date,0)
            ELSE ISNULL(cral.payment_to_date,0) END,
       ISNULL(cral.Payment,0) + ISNULL(cral.salvage,0) + ISNULL(cral.recovery,0) + 
       ISNULL(cral.this_payment,0) + ISNULL(cral.this_salvage,0) + ISNULL(cral.this_recovery,0),
       ISNULL(cral.this_share_percent,0), cral.is_pt_archive,
       CASE WHEN cral.type = 'FX' THEN cral.Participation_Percent ELSE NULL END,
       case when  ISNULL(cral.manually_added,0)= 1 and cral.type in ('T','TFS') then 1 
			when   ISNULL(cral.manually_added,0)= 1 and cral.type in ('TX') then 2 else  rml.Treaty_Type_id end Treaty_Type_id, cral.priority,
       ISNULL(cral.manually_added,0), ISNULL(cral.sum_insured,0)
FROM claim_ri_arrangement_line cral
LEFT JOIN ri_model_line rml ON cral.ri_model_line_id = rml.ri_model_line_id
WHERE cral.claim_id = @claim_id AND cral.ri_arrangement_id = @ri_arrangement_id
ORDER BY CASE WHEN cral.type = 'R' THEN 1 ELSE 0 END ASC, cral.priority ASC, cral.number_of_lines ASC, cral.line_limit ASC

OPEN ri_cursor
FETCH NEXT FROM ri_cursor INTO @ri_arrangement_line_id, @type, @treaty_id, @lower_limit, @line_limit,
      @Reserve, @payment, @net_payment, @this_share_percent, @is_pt_archive, @Participation_Percent, @treaty_Type_id, @priority,
      @manually_added, @sum_insured

	    SELECT @first_priority = @priority;
        SELECT @running_reserve = @current_reserve_cr;

WHILE @@FETCH_STATUS = 0
BEGIN   
		IF ISNULL(@last_priority, -666) <> @priority
		BEGIN
			SET @last_priority = @priority
			SET @priority_reserve = @running_reserve 
		END
    IF @treaty_Type_id = 1 -- Proportional
    BEGIN
        -- For manually added treaties derive share% from sum_insured/Net_SumInsured (mirrors CalcRI2007)
        IF @manually_added = 1 AND @type IN ('T','TFS') AND ISNULL(@Net_SumInsured,0) <> 0
            SET @this_share_percent = (@sum_insured / @Net_SumInsured) * 100.0

        IF @type IN ('T','TFS','F')
            SET @current_reserve = CASE WHEN @is_pt_archive = 0 THEN @priority_reserve * @this_share_percent / 100.0 ELSE 0 END
        ELSE IF @type = 'R'
        BEGIN
            -- Use this_reserve directly (set by spu_calculate_claims_ri_method_2_full_RI2007)
            SELECT @current_reserve = ISNULL(this_reserve, 0)
            FROM Claim_RI_Arrangement_Line
            WHERE ri_arrangement_line_id = @ri_arrangement_line_id
            SET @net_payment = 0
        END

        -- Accumulate TFS; deduct T/F from running_reserve; R handled above
        IF @type = 'TFS'
            SET @tfs_total_reserve = @tfs_total_reserve + @current_reserve
        ELSE IF @type NOT IN ('R')
            SET @running_reserve = @running_reserve - @current_reserve
    END
    ELSE IF @treaty_Type_id = 2 -- Non-Proportional
    BEGIN
        -- Get recovery amounts
        SELECT @NET_Recovery = SUM(ISNULL(salvage,0) + ISNULL(recovery,0) + ISNULL(this_salvage,0) + ISNULL(this_recovery,0))
        FROM Claim_RI_Arrangement_Line
        WHERE claim_id = @claim_id AND ri_arrangement_id = @ri_arrangement_id
       AND (@type IN ('F','FX') OR (@type = 'TX' AND type IN ('R','TX')))
       
        -- Calculate current reserve based on type
      SET @current_reserve = 
      CASE 
        WHEN (@running_reserve + @NET_Recovery) > @lower_limit THEN
            CASE 
                WHEN (@running_reserve + @NET_Recovery) > @line_limit THEN
                    (ISNULL(@Participation_Percent,100) * (@line_limit - @lower_limit) / 100) - @net_payment
                ELSE
                    (ISNULL(@Participation_Percent,100) * (@running_reserve + @NET_Recovery - @lower_limit) / 100) - @net_payment
            END
        ELSE 0 
      END
	     -- Update total reserve
       SET @total_reserve = CASE 
            WHEN @type IN ('F','FX') THEN @total_reserve - (@payment + @running_reserve)
            WHEN @type = 'TX' THEN @total_reserve + ISNULL(@NET_Recovery,0)
            ELSE @total_reserve END
	   SET @running_reserve = @running_reserve -  @current_reserve
     
    END
    
    IF @type = 'R' SET @net_payment = 0
    SET @incurred = ROUND(@current_reserve, @DecimalPlaces) + ROUND(@net_payment, @DecimalPlaces)
    
    UPDATE Claim_RI_Arrangement_Line 
    SET claim_incurred_to_date = @incurred
    WHERE ri_arrangement_line_id = @ri_arrangement_line_id
    
    FETCH NEXT FROM ri_cursor INTO @ri_arrangement_line_id, @type, @treaty_id, @lower_limit, @line_limit,
          @Reserve, @payment, @net_payment, @this_share_percent, @is_pt_archive, @Participation_Percent, @treaty_Type_id, @priority,
          @manually_added, @sum_insured
END

CLOSE ri_cursor
DEALLOCATE ri_cursor 
GO