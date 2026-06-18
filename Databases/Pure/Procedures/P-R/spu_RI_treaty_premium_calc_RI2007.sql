SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_RI_treaty_premium_calc_RI2007'
GO
CREATE PROCEDURE spu_RI_treaty_premium_calc_RI2007
@ri_arrangement_id integer AS
BEGIN

-- VARIABLE DECLARATION
DECLARE @gross_premium money
DECLARE @obligatory_premium money
DECLARE @fac_premium money
DECLARE @net_premium money
DECLARE @factor_type varchar(100)
DECLARE @factor_amount money
DECLARE @running_premium money
DECLARE @ret_line_id int
DECLARE @max_iterations int = 100
DECLARE @iteration_count int = 0

DECLARE @gross_SI money
DECLARE @fac_SI money
DECLARE @Obligatory_SI money
DECLARE @net_SI money

--CALCULATE GROSS, FAC AND NET SI
SELECT @gross_SI = ISNULL(sum_insured, 0) FROM ri_arrangement WHERE ri_arrangement_id = @ri_arrangement_id
SELECT @Obligatory_SI = ISNULL(sum_insured, 0) FROM RI_Arrangement_Line WHERE ri_arrangement_id = @ri_arrangement_id and type = 'T' AND ISNULL(Is_Obligatory, 0)= 1
SELECT @fac_SI = ISNULL(SUM(sum_insured), 0) FROM RI_Arrangement_Line
WHERE ri_arrangement_id = @ri_arrangement_id
AND TYPE IN('F', 'FX') 
SELECT @net_SI = ISNULL(@gross_SI, 0) - ISNULL(@Obligatory_SI,0)- ISNULL(@fac_SI, 0)

--CALCULATE GROSS, FAC AND NET PREMIUM
SELECT @gross_premium = ISNULL(premium, 0) FROM ri_arrangement WHERE ri_arrangement_id = @ri_arrangement_id
SELECT @obligatory_premium = ISNULL(SUM(premium_value), 0) FROM RI_Arrangement_Line WHERE ri_arrangement_id = @ri_arrangement_id and TYPE = 'T' AND ISNULL(Is_Obligatory, 0)= 1
SELECT @fac_premium = ISNULL(SUM(premium_value), 0) FROM RI_Arrangement_Line WHERE ri_arrangement_id = @ri_arrangement_id AND TYPE IN('F', 'FX')
SELECT @net_premium = ISNULL(@gross_premium, 0) - ISNULL(@obligatory_premium ,0)- ISNULL(@fac_premium, 0)

SET @running_premium = @net_premium

-- Create temp table for RI premiums
CREATE TABLE #ri_premiums (
  ri_arrangement_line_id int,
  treaty_id int,
  type varchar(100),
  ri_model_line_id int,
  lower_limit money,
  premium_percent decimal(10, 4),
  treaty_type_id int,
  premium_value money,
  this_share_percent decimal(10, 4),
  premium_calculation_basis_id int,
  calculation_factors varchar(255),
  calculated_in_iteration bit DEFAULT 0,
  is_calculated bit DEFAULT 0,
  is_qsr bit DEFAULT 0,
  is_premium_edited bit DEFAULT 0
)

INSERT INTO #ri_premiums
SELECT
ral.ri_arrangement_line_id,
ral.treaty_id,
ral.type,
ral.ri_model_line_id,
ral.lower_limit,
0,
rml.treaty_type_id,
-- For is_premium_edited lines seed with the existing DB premium so it participates
-- correctly in factor derivation; the final UPDATE will skip overwriting it.
CASE WHEN ISNULL(ral.is_premium_edited, 0) = 1 THEN ISNULL(ral.premium_value, 0) ELSE 0 END,
case when ral.type in ('TX', 'TC', 'PX') then ISNULL(rml.ceding_rate, 0)
     when ISNULL(pcb.reinsurance_type_id, 0) = 14 then ISNULL(ral.default_share_percent, 0)  -- QSR: always use default_share_percent
     when ISNULL(ral.this_share_percent, 0) = 0 and @net_SI = 0 then ISNULL(ral.default_share_percent,0)
     ELSE ISNULL(ral.this_share_percent, 0) end as this_share_percent,
ISNULL(rml.premium_calculation_basis_id, 0),
ISNULL(pcb.calculation_Factors, ''),
-- is_premium_edited lines are pre-seeded so mark as already calculated
CASE WHEN ISNULL(ral.is_premium_edited, 0) = 1 THEN 1 ELSE 0 END,
CASE WHEN ISNULL(ral.is_premium_edited, 0) = 1 THEN 1 ELSE 0 END,
CASE WHEN ISNULL(pcb.reinsurance_type_id, 0) = 14 THEN 1 ELSE 0 END,
ISNULL(ral.is_premium_edited, 0)
FROM ri_arrangement_line ral
JOIN ri_model_line rml ON ral.ri_model_line_id = rml.ri_model_line_id
JOIN Premium_Calculation_Basis pcb ON pcb.premium_calculation_basis_id = rml.premium_calculation_basis_id
WHERE ral.ri_arrangement_id = @ri_arrangement_id
AND ral.Type IN ('R', 'T', 'TX', 'TC', 'TFS', 'PX')
AND ISNULL(pcb.calculation_Factors, '') <> ''
AND ISNULL(ral.Is_Obligatory,0) = 0                    -- exclude obligatory lines (pre-deducted from gross to derive net_premium)

-- Remove rows where the computed this_share_percent is zero (no premium to calculate)
-- For TX/TC/PX this uses rml.ceding_rate; for others ral.this_share_percent
-- Exclude R type: its premium is set by retained absorption, not iterative calc
-- Exclude QSR lines (is_qsr=1): their premium is split from retained using default_share_percent
DELETE FROM #ri_premiums WHERE ISNULL(this_share_percent, 0) = 0 AND type <> 'R' AND is_qsr = 0

SET @ret_line_id = (SELECT TOP 1 ri_arrangement_line_id FROM #ri_premiums WHERE TYPE = 'R')

-- Create temp table for factors
CREATE TABLE #factors ( TYPE VARCHAR(500), amount MONEY )

-- Insert initial factors G and G,F
INSERT INTO #factors SELECT 'G', @gross_premium
INSERT INTO #factors SELECT 'G,F', @net_premium

-- START LOOP for iterative calculation
WHILE @iteration_count < @max_iterations
BEGIN
    SET @iteration_count = @iteration_count + 1

    -- Reset calculated_in_iteration flag (keep pre-seeded is_premium_edited lines as calculated)
    UPDATE #ri_premiums SET calculated_in_iteration = 0
    -- On first iteration only, mark is_premium_edited lines so they contribute to factor derivation once
    IF @iteration_count = 1
        UPDATE #ri_premiums SET calculated_in_iteration = 1 WHERE ISNULL(is_premium_edited, 0) = 1 AND is_calculated = 1

    -- Calculate lines whose factor is available AND all contributing types
    -- that have non-zero share (i.e. will actually produce premium) are done
    UPDATE rp
    SET rp.premium_value = f.amount * rp.this_share_percent / 100,
        rp.calculated_in_iteration = 1,
        rp.is_calculated = 1
    FROM #ri_premiums rp
    JOIN #factors f ON rp.calculation_factors = f.type
    WHERE rp.is_calculated = 0
    AND rp.is_qsr = 0
    AND NOT EXISTS (
        SELECT 1 FROM #ri_premiums rp2
        WHERE rp2.is_calculated = 0
        AND rp2.ri_arrangement_line_id <> rp.ri_arrangement_line_id
        AND rp2.this_share_percent > 0  -- only block on lines that will produce premium
        AND (
            (   (rp.calculation_factors = 'P'
              OR rp.calculation_factors LIKE '%,P'
              OR rp.calculation_factors LIKE '%,P,%')
                AND rp2.type IN ('T', 'TFS')
            )
         OR (rp.calculation_factors LIKE '%,TX%' OR rp.calculation_factors = 'TX') AND rp2.type = 'TX'
         OR (rp.calculation_factors LIKE '%,TC%' OR rp.calculation_factors = 'TC') AND rp2.type = 'TC'
         OR (rp.calculation_factors LIKE '%,PX%' OR rp.calculation_factors = 'PX') AND rp2.type = 'PX'
         OR (   (rp.calculation_factors = 'R'
              OR rp.calculation_factors LIKE '%,R'
              OR rp.calculation_factors LIKE '%,R,%')
                AND rp2.type = 'R'
            )
        )
    )

    -- Check if any uncalculated lines remain
    IF NOT EXISTS(SELECT 1 FROM #ri_premiums WHERE is_calculated = 0) BREAK

    -- Create temp table for current iteration factors
    -- Include is_premium_edited lines (already calculated) so their premiums
    -- contribute to derived factors for other lines that depend on them.
    SELECT type, SUM(premium_value) AS total_premium INTO #currentfactor
    FROM #ri_premiums WHERE calculated_in_iteration = 1
    GROUP BY type

    -- Insert new derived factors
    DECLARE factor_cursor CURSOR FOR
    SELECT CASE WHEN cf.type IN ('T', 'TFS') THEN 'P' ELSE cf.type END, cf.total_premium
    FROM #currentfactor cf

    OPEN factor_cursor
    FETCH NEXT FROM factor_cursor INTO @factor_type, @factor_amount
    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Update existing derived factor keys by subtracting the new amount
        UPDATE #factors
        SET amount = amount - @factor_amount
        WHERE type IN (SELECT CONCAT(f.type, ',', @factor_type) FROM #factors f)

        -- Insert new derived factor keys that do not yet exist
        INSERT INTO #factors (type, amount)
        SELECT CONCAT(f.type, ',', @factor_type), f.amount - @factor_amount
        FROM #factors f
        WHERE NOT EXISTS(SELECT 1 FROM #factors WHERE type = CONCAT(f.type, ',', @factor_type))

        FETCH NEXT FROM factor_cursor INTO @factor_type, @factor_amount
    END
    CLOSE factor_cursor
    DEALLOCATE factor_cursor
    DROP TABLE #currentfactor

    -- Conditionally seed R factor once all non-R-dependent lines are calculated
    -- R = the already-calculated premium_value of the R-type line
    IF NOT EXISTS (
        SELECT 1 FROM #ri_premiums
        WHERE is_calculated = 0
        AND calculation_factors NOT LIKE '%R%'
    )
    AND NOT EXISTS (SELECT 1 FROM #factors WHERE type = 'R')
    BEGIN
        DECLARE @r_amount money
        SELECT TOP 1 @r_amount = premium_value
        FROM #ri_premiums
        WHERE type = 'R'

        IF @r_amount IS NOT NULL
        BEGIN
            INSERT INTO #factors (type, amount) VALUES ('R', @r_amount)

            -- Derive R-based combinations with all existing factors (e.g. R,TX  R,P)
            INSERT INTO #factors (type, amount)
            SELECT CONCAT('R,', f.type), @r_amount - f.amount
            FROM #factors f
            WHERE f.type <> 'R'
            AND NOT EXISTS (SELECT 1 FROM #factors WHERE type = CONCAT('R,', f.type))
        END
    END

END

  -- If Retained line exists, absorb any unallocated premium (positive or negative) into it
  IF @ret_line_id IS NOT NULL
  BEGIN
    DECLARE @total_non_ret_premium money
    SELECT @total_non_ret_premium = ISNULL(SUM(premium_value), 0)
    FROM #ri_premiums
    WHERE ri_arrangement_line_id <> @ret_line_id
	
    DECLARE @unallocated_premium money
    SET @unallocated_premium = @net_premium - @total_non_ret_premium -  -- use @net_premium: retained absorbs from net pool only
        (SELECT ISNULL(premium_value, 0) FROM #ri_premiums WHERE ri_arrangement_line_id = @ret_line_id)

    IF @unallocated_premium <> 0
    BEGIN
      UPDATE #ri_premiums
      SET premium_value = premium_value + @unallocated_premium
      WHERE ri_arrangement_line_id = @ret_line_id
    END
  END

  -- Split Retained premium between R and QSR lines (Quota Share Retained, is_qsr = 1)
  -- There can be multiple QSR lines but only one Retained line
  -- E.g. Ret=50%, QSR1=10%, QSR2=40% => total=100%, R gets 50% of premium, QSR1 gets 10%, QSR2 gets 40%
  IF @ret_line_id IS NOT NULL AND EXISTS (SELECT 1 FROM #ri_premiums WHERE is_qsr = 1)
  BEGIN
    DECLARE @r_default_pct decimal(10,4)
    DECLARE @total_pct decimal(10,4)
    DECLARE @retained_premium money
    DECLARE @qsr_line_id int
    DECLARE @qsr_default_pct decimal(10,4)
    DECLARE @qsr_premium money
    DECLARE @qsr_premium_pct decimal(10,4)

    SELECT @r_default_pct = ISNULL(ral.default_share_percent, 0)
    FROM RI_Arrangement_Line ral WHERE ral.ri_arrangement_line_id = @ret_line_id

    -- Total = R default % + sum of all QSR default %
    SELECT @total_pct = @r_default_pct + ISNULL(SUM(ISNULL(ral.default_share_percent, 0)), 0)
    FROM RI_Arrangement_Line ral
    WHERE ral.ri_arrangement_line_id IN (SELECT ri_arrangement_line_id FROM #ri_premiums WHERE is_qsr = 1)

    SELECT @retained_premium = ISNULL(premium_value, 0)
    FROM #ri_premiums WHERE ri_arrangement_line_id = @ret_line_id

    IF @total_pct > 0
    BEGIN
      -- Update R line in temp table with its proportional share
      UPDATE #ri_premiums
      SET premium_value = @retained_premium * @r_default_pct / 100
      WHERE ri_arrangement_line_id = @ret_line_id

      -- Cursor to update each QSR line individually
      DECLARE qsr_cursor CURSOR FOR
      SELECT ri_arrangement_line_id FROM #ri_premiums WHERE is_qsr = 1

      OPEN qsr_cursor
      FETCH NEXT FROM qsr_cursor INTO @qsr_line_id
      WHILE @@FETCH_STATUS = 0
      BEGIN
        SELECT @qsr_default_pct = ISNULL(default_share_percent, 0)
        FROM RI_Arrangement_Line WHERE ri_arrangement_line_id = @qsr_line_id

        SET @qsr_premium = @retained_premium * @qsr_default_pct / 100
        SET @qsr_premium_pct = CASE WHEN @gross_premium > 0 THEN (@qsr_premium / @gross_premium) * 100 ELSE 0 END

        UPDATE RI_Arrangement_Line
        SET premium_value = @qsr_premium,
            premium_percent = @qsr_premium_pct,
            commission_value = @qsr_premium * (ISNULL(commission_percent, 0) / 100.0)
        WHERE ri_arrangement_line_id = @qsr_line_id
        AND ri_arrangement_id = @ri_arrangement_id

        FETCH NEXT FROM qsr_cursor INTO @qsr_line_id
      END
      CLOSE qsr_cursor
      DEALLOCATE qsr_cursor
    END
  END

  -- Calculate premium_percent
  UPDATE #ri_premiums SET premium_percent = CASE WHEN @gross_premium > 0 THEN (premium_value / @gross_premium) * 100 ELSE 0 END

  -- Final update to RI_Arrangement_Line table (skip QSR lines - updated separately in the split above)
  -- Also skip lines where is_premium_edited = 1 (user directly edited the premium);
  -- their premium_value is preserved as-is but commission is still recalculated from it.
  UPDATE ral SET
  ral.premium_value = tmpri.premium_value,
  ral.premium_percent = tmpri.premium_percent,
  ral.commission_value = ISNULL(tmpri.premium_value,0) * (ISNULL(ral.commission_percent, 0) / 100.0)
  FROM RI_Arrangement_Line ral
  JOIN #ri_premiums tmpri ON tmpri.ri_arrangement_line_id = ral.ri_arrangement_line_id
  WHERE ral.ri_arrangement_id = @ri_arrangement_id
  AND tmpri.is_qsr = 0
  AND ISNULL(ral.is_premium_edited, 0) = 0

  -- For is_premium_edited lines: preserve premium_value but recalculate commission from it
  UPDATE ral SET
  ral.commission_value = ISNULL(ral.premium_value, 0) * (ISNULL(ral.commission_percent, 0) / 100.0)
  FROM RI_Arrangement_Line ral
  JOIN #ri_premiums tmpri ON tmpri.ri_arrangement_line_id = ral.ri_arrangement_line_id
  WHERE ral.ri_arrangement_id = @ri_arrangement_id
  AND tmpri.is_qsr = 0
  AND ISNULL(ral.is_premium_edited, 0) = 1

  DROP TABLE #ri_premiums
  DROP TABLE #factors
END
