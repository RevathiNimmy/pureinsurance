SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
Execute DDLDropProcedure 'spu_Risk_Single_Tax_Select'
GO

 CREATE PROCEDURE spu_Risk_Single_Tax_Select
    @Tax_Calculation_cnt int,
    @Apply_Tax_By int,
    @TransType varchar(4)
AS

    DECLARE
        @effective_date datetime,
        @currency_id smallint,
        @company_id smallint,
        @original_risk_cnt int,
        @type_of_rate tinyint,
        @base_currency_id smallint,
        @insurance_file_cnt int,
        @risk_cnt int,
        @tax_band_id int,
        @tax_group_id int

  DECLARE
  @system_option VARCHAR(20)

  -- Get Risk_Cnt
  SELECT @risk_cnt = risk_cnt,
         @tax_band_id = tax_band_id,
         @tax_group_id = tax_group_id
  FROM Tax_Calculation
  WHERE Tax_Calculation_cnt = @Tax_Calculation_cnt

  -- Delete any existing risk taxes
  DELETE  Tax_Calculation
  WHERE   Tax_Calculation_cnt = @Tax_Calculation_cnt
  And @risk_cnt is not null

 SELECT  @system_option=value
 FROM System_Options
 WHERE option_number=1007 AND branch_id=1

 -- Skip if tax switched off
 IF (@system_option='0')
  RETURN

  -- Note: This is safe as tax will only be applied to edited risks
  -- (thus new risk cnt)
        SELECT  @insurance_file_cnt = ifi.insurance_file_cnt,
                @currency_id = ifi.currency_id,
                @company_id = ifi.source_id,
                @effective_date = ifi.cover_start_date,
                @original_risk_cnt = CASE WHEN ISNULL(ifrl.original_risk_cnt, 0) = 0 THEN
                                         ifrl.renewed_risk_cnt
                                     ELSE
                                         ifrl.original_risk_cnt
                                     END
        FROM    insurance_file ifi
        JOIN    insurance_file_risk_link ifrl on ifrl.insurance_file_cnt = ifi.insurance_file_cnt
        WHERE   ifrl.risk_cnt = @risk_cnt

  IF @Apply_Tax_By='0' BEGIN
    -- Effective date should be transaction date rather than cover start date
    SELECT @effective_date = GetDate()
  END

  -- (RC) Effective date should be risk inception date
  IF @Apply_Tax_By='2' BEGIN
    SELECT @effective_date = inception_date from risk where risk_cnt = @risk_cnt
  END

        -- Determine which branch to use
        EXEC spu_ACT_GetTypeOfRates @type_of_rate OUTPUT
        IF @type_of_rate = 1
            SELECT @company_id = 1

        -- Get base currency
        SELECT @base_currency_id = base_currency_id
        FROM   source
        WHERE  source_id = @company_id

        -- If we are looking for an mta then we should check for return or additional
        IF (@TransType = 'MTA')
            SELECT  @TransType = CASE WHEN SUM(p.this_premium) < 0 THEN 'RMTA' ELSE 'AMTA' END
                FROM    peril p
                WHERE   p.risk_cnt = @risk_cnt
                AND     p.is_premium = 1

        -- Notes:
        -- This uses a lot of trickery to achieve the required results, if you are not 100%
        -- confifident of what it is doing contact me (Peter Finney) for assistance.
        --
        -- Basically,
        --   The query does not access any tables directly as they are all useless in their
        -- native states. Instead it build up 5 virtual tables:
        --     bands    - Lists all tax bands applicable to the current risk
        --     prem     - Lists the premiums applicable to all bands referenced in the current risk
        --     si       - Lists the sum_insured values applicable to all bands referenced in the current risk
        --     osi      - Lists the original sum_insured values applicable to all bands referenced in the current risk
        --     rates    - Lists the current applicable rate for each tax band
        -- These are then brought together on the tax_band_id to calculate the appropriate values

        -- Insert all new taxes
        INSERT INTO Tax_Calculation (
                insurance_file_cnt,
                risk_cnt,
                tax_band_id,
                premium,
                percentage,
                value,
                is_value,
                is_manually_changed,
                calc_basis,
                basis_value,
                sum_insured,
                sum_insured_rounded,
                currency_id,
                allow_tax_credit,
                original_sum_insured,
                country_id,
                state_id,
                class_of_business_id,
                tax_group_id,
                sequence,
                transtype,
                include_tax_in_instalments,
                spread_tax_across_instalments,
                is_not_applied_to_client,
                apply_tax_by)
        SELECT  @insurance_file_cnt,
                @risk_cnt,
                bands.tax_band_id,
                ISNULL(prem.this_premium, 0),
                rates.rate,
                0, -- tax value now calculated by business object!
                rates.is_value,
                0,
                rates.calc_basis,
                rates.basis_value,
                ISNULL(si.sum_insured, 0),
                rates.sum_insured_rounded,
                @currency_id, -- Currency of taxes have changed to match insurance file
                rates.allow_tax_credit,
                ISNULL(osi.sum_insured, 0),
                bands.country_id,
                bands.state_id,
                bands.class_of_business_id,
                bands.tax_group_id,
                bands.sequence,
                'TTR',
                bands.is_include_tax_in_instalments,
                bands.is_spread_tax_across_instalments,
                bands.is_not_applied_to_client,
                @Apply_Tax_By

        -- Get all active tax_bands
        FROM   (SELECT  tgtb.tax_band_id,
                        tgtb.tax_group_id,
                        tgtb.sequence,
                        rs.country_id,
                        rs.state_id,
                        p.class_of_business_id,
                       (Select distinct is_include_tax_in_instalments from tax_type tax_t where tax_t.tax_type_id = tt.tax_type_id) is_include_tax_in_instalments,
                       (Select distinct is_spread_tax_across_instalments from tax_type tax_t where tax_t.tax_type_id = tt.tax_type_id) is_spread_tax_across_instalments,
                       (Select distinct is_not_applied_to_client from tax_type tax_t where tax_t.tax_type_id = tt.tax_type_id) is_not_applied_to_client

                FROM    peril p
                JOIN    tax_group_tax_band tgtb ON tgtb.tax_group_id  = p.tax_group
                JOIN    tax_band tb ON tb.tax_band_id = tgtb.tax_band_id
                JOIN    tax_type tt ON tt.tax_type_id = tb.tax_type_id
                JOIN    rating_section rs ON rs.risk_cnt = p.risk_cnt
                                         AND rs.rating_section_id = p.rating_section_id
                WHERE   p.risk_cnt = @risk_cnt
                AND    (p.is_premium = 1 OR p.is_sum_insured = 1 OR p.is_levy_tax = 1)
                AND     tt.tax_basis = 1
                -- ensure only active tax types are used
                AND     tt.effective_date =(SELECT  MAX(effective_date)
                                            FROM    tax_type
                                            WHERE   tax_type_id = tt.tax_type_id
                                                AND tt.tax_basis = 1
                                                AND effective_date <= @effective_date
                                                AND is_deleted = 0)
              AND     tb.is_deleted=0
                GROUP BY
                        tgtb.tax_band_id,
                        tgtb.tax_group_id,
                        tgtb.sequence,
                        rs.country_id,
                        rs.state_id,
                        p.class_of_business_id,
                        tt.tax_type_id) bands

        -- Get all premium values by tax band and class of business
        JOIN   (SELECT  tgtb.tax_band_id,
                        tgtb.tax_group_id,
                        tgtb.sequence,
                        p.class_of_business_id,
                        sum(p.this_premium) [this_premium]
                FROM    peril p
                JOIN    tax_group_tax_band tgtb ON tgtb.tax_group_id  = p.tax_group
                WHERE   p.risk_cnt = @risk_cnt
                AND    (p.is_premium = 1 or p.is_levy_tax = 1)
                GROUP BY
                        tgtb.tax_band_id,
                        tgtb.tax_group_id,
                        tgtb.sequence,
                        p.class_of_business_id) prem
            ON  prem.tax_band_id = bands.tax_band_id
            AND prem.tax_group_id = bands.tax_group_id
            AND prem.sequence = bands.sequence
            AND prem.class_of_business_id = bands.class_of_business_id

        -- Get all sum insured values
        LEFT JOIN
               (SELECT  tgtb.tax_band_id,
                        tgtb.tax_group_id,
                        tgtb.sequence,
                        -- Get sum insured from rating section as individual perils aren't broken down
                       (SELECT  SUM(rs.sum_insured)
                        FROM    rating_section rs
                        WHERE   rs.rating_section_id IN (SELECT  rating_section_id
                                                 FROM    peril p2
                                                         JOIN    tax_group_tax_band tgtb3 ON tgtb3.tax_group_id  = p2.tax_group
               WHERE   tgtb3.tax_band_id = tgtb.tax_band_id
                                                         AND     tgtb3.tax_group_id = tgtb.tax_group_id
                                                         AND     p2.risk_cnt = @risk_cnt)
                        AND     rs.risk_cnt = @risk_cnt
                        AND     rs.original_flag = 0) [sum_insured]
                FROM    peril p
                JOIN    tax_group_tax_band tgtb ON tgtb.tax_group_id  = p.tax_group
                WHERE   p.risk_cnt = @risk_cnt
                AND    (p.is_sum_insured = 1 OR p.is_levy_tax = 1)
                GROUP BY
                        tgtb.tax_band_id,
                        tgtb.tax_group_id,
                        tgtb.sequence) si
            ON  si.tax_band_id = bands.tax_band_id
            AND si.tax_group_id = bands.tax_group_id
            AND si.sequence = bands.sequence

        -- Get all original sum insured values
        LEFT JOIN
               (SELECT  tgtb.tax_band_id,
                        tgtb.tax_group_id,
                        tgtb.sequence,
                        -- Get sum insured from rating section as individual perils aren't broken down
                       (SELECT  SUM(rs.sum_insured)
                        FROM    rating_section rs
                        WHERE   rs.rating_section_id IN (SELECT  rating_section_id
                                                         FROM    peril p2
                                                         JOIN    tax_group_tax_band tgtb3 ON tgtb3.tax_group_id  = p2.tax_group
                                                         WHERE   tgtb3.tax_band_id = tgtb.tax_band_id
                                                         AND     tgtb3.tax_group_id = tgtb.tax_group_id
                                                         AND     p2.risk_cnt = @original_risk_cnt)
                        AND     rs.risk_cnt = @original_risk_cnt
                        AND     rs.original_flag = 0) [sum_insured]
                FROM    peril p
                JOIN    tax_group_tax_band tgtb ON tgtb.tax_group_id  = p.tax_group
                WHERE   p.risk_cnt = @original_risk_cnt
                AND    (p.is_sum_insured = 1 OR p.is_levy_tax = 1)
                GROUP BY
                        tgtb.tax_band_id,
                        tgtb.tax_group_id,
                        tgtb.sequence) osi
            ON  osi.tax_band_id = bands.tax_band_id
            AND osi.tax_group_id = bands.tax_group_id
            AND osi.sequence = osi.sequence

        -- Get all "current" tax band rates
        JOIN
               (SELECT
                    tbr.tax_band_id,
                    tbr.is_value,
                    tbr.calc_basis,
                    tbr.sum_insured_rounded,
                    tbr.allow_tax_credit,
                    tbr.country_id,
                    tbr.state_id,
                    cob.class_of_business_id,
                    basis_value =
                       (SELECT  CASE
                                  WHEN tbr.currency_id = @currency_id THEN tbr.basis_value
                                  WHEN tbr.currency_id = @base_currency_id THEN tbr.basis_value / CRFromBase.rate_against_base
                                  WHEN @currency_id = @base_currency_id THEN tbr.basis_value * CRToBase.rate_against_base
                                  ELSE (tbr.basis_value * CRToBase.rate_against_base) / CRFromBase.rate_against_base
                  END
                        FROM    CurrencyRate CRToBase
                        JOIN    CurrencyRate CRFromBase ON CRFromBase.company_id = CRToBase.company_id
                        WHERE   CRToBase.company_id = @company_id
                        AND     CRToBase.currency_id = ISNULL(tbr.currency_id , @currency_id)
                        AND     CRToBase.effective_from IN (SELECT  MAX(effective_from)
                                      FROM  CurrencyRate
                                                            WHERE   effective_from <= @effective_date
                                                            AND     currency_id = CRToBase.currency_id
                                                            AND     company_id = CRToBase.company_id)
                        AND     CRFromBase.currency_id = @currency_id
                        AND     CRFromBase.effective_from IN (SELECT  MAX(effective_from)
                                                              FROM    CurrencyRate
                                                              WHERE   effective_from <= @effective_date
                                                              AND     currency_id = CRFromBase.currency_id
                                                              AND     company_id = CRFromBase.company_id)),
                    rate=
                        (SELECT  CASE
                             WHEN tbr.is_value = 0 THEN tbr.rate -- Rate holds a percentage
                              WHEN tbr.currency_id = @currency_id THEN tbr.rate
                              WHEN tbr.currency_id = @base_currency_id THEN tbr.rate / CRFromBase.rate_against_base
                              WHEN @currency_id = @base_currency_id THEN tbr.rate * CRToBase.rate_against_base
                              ELSE (tbr.rate * CRToBase.rate_against_base) / CRFromBase.rate_against_base
                          END

                         FROM    CurrencyRate CRToBase
                         JOIN    CurrencyRate CRFromBase ON CRFromBase.company_id = CRToBase.company_id
                         WHERE   CRToBase.company_id = @company_id
                         AND     CRToBase.currency_id = ISNULL(tbr.currency_id, @currency_id)
                         AND     CRToBase.effective_from IN (SELECT  MAX(effective_from)
                                                             FROM    CurrencyRate
                                                             WHERE   effective_from <= @effective_date
                                                             AND     currency_id = CRToBase.currency_id
                                                             AND     company_id = CRToBase.company_id)
                         AND     CRFromBase.currency_id = @currency_id
                         AND     CRFromBase.effective_from IN (SELECT  MAX(effective_from)
                                         FROM    CurrencyRate
                                                               WHERE   effective_from <= @effective_date
                                                               AND     currency_id = CRFromBase.currency_id
                                                               AND     company_id = CRFromBase.company_id))


                FROM    tax_band_rate tbr
                        -- If the class_of_business is null ('any') we need to cross join
                        -- to any class_of_business values without a defined rate.
                JOIN    class_of_business cob
                ON  cob.class_of_business_id = tbr.class_of_business_id
                        OR (tbr.class_of_business_id IS NULL
                        AND cob.class_of_business_id NOT IN (SELECT  class_of_business_id
                                                             FROM    tax_band_rate tbr3
                                                             WHERE   tbr3.tax_band_id = tbr.tax_band_id
                                                             AND     ISNULL(tbr3.country_id, 0) = ISNULL(tbr.country_id, 0)
                                                             AND     ISNULL(tbr3.state_id, 0) = ISNULL(tbr.state_id, 0)
                                                             AND     tbr3.class_of_business_id IS NOT NULL))
                        -- ensure most recent effective rates
                        WHERE   tbr.effective_date = (SELECT    MAX(effective_date)
                                 FROM     tax_band_rate tbr2
                                 WHERE    tbr2.tax_band_id = tbr.tax_band_id
                                  AND ISNULL(tbr2.country_id, 0) = ISNULL(tbr.country_id, 0)
                                  AND ISNULL(tbr2.state_id, 0) = ISNULL(tbr.state_id, 0)
                                  AND ISNULL(tbr2.class_of_business_id, 0) = ISNULL(tbr.class_of_business_id, 0)
                                  AND tbr2.effective_date <= @effective_date
                                  AND is_deleted = 0
                                      -- check tax usage
                                  AND CASE @TransType
                                         WHEN 'NB'  THEN tbr.NB
                                         WHEN 'AMTA' THEN tbr.AMTA
                                         WHEN 'RMTA' THEN tbr.RMTA
                                         WHEN 'MTC' THEN tbr.CANC
                                         WHEN 'REN' THEN tbr.REN
                                         ELSE 1
                                      END > 0)

                        -- check tax usage
                AND     CASE @TransType
                            WHEN 'NB'  THEN tbr.NB
                            WHEN 'AMTA' THEN tbr.AMTA
                            WHEN 'RMTA' THEN tbr.RMTA
                            WHEN 'MTC' THEN tbr.CANC
                            WHEN 'REN' THEN tbr.REN
                            ELSE 1
                        END > 0
                AND     tbr.is_deleted = 0) rates
                ON  rates.tax_band_id = bands.tax_band_id
                AND ISNULL(rates.country_id, 0) = ISNULL(bands.country_id, 0)
                AND ISNULL(rates.state_id, 0) = ISNULL(bands.state_id, 0)
                AND ISNULL(rates.class_of_business_id, 0) = ISNULL(bands.class_of_business_id, 0)
                AND bands.tax_band_id = @tax_band_id
                AND bands.tax_group_id = @tax_group_id

    -- Return our taxes
    SELECT  t.risk_cnt,
            t.tax_band_id,
            t.premium,
            t.percentage,
            t.value,
            t.is_value,
            t.is_manually_changed,
            tb.description,
            tt.is_not_applied_to_client,
            0, -- is deleted
            t.basis_value,
            t.calc_basis,
            t.sum_insured,
            t.sum_insured_rounded,
            t.currency_id,
            c.description,
            t.allow_tax_credit,
            t.original_sum_insured,
            t.sum_insured - t.original_sum_insured,
            tg.tax_group_id,
            tg.description,
            t.sequence,
            ct.country_id,
            ct.description,
            s.state_id,
            s.description,
            cob.class_of_business_id,
            cob.description,
            0, -- running total
            t.tax_calculation_cnt,
            t.transtype,
            t.is_not_applied_to_client,
            t.include_tax_in_instalments,
            t.spread_tax_across_instalments,
            t.apply_tax_by

    FROM    Tax_Calculation t
    JOIN    tax_band tb ON tb.tax_band_id = t.tax_band_id
    JOIN    tax_type tt ON tt.tax_type_id = tb.tax_type_id
    JOIN    currency c  ON c.currency_id = t.currency_id
    LEFT JOIN
            tax_group tg ON tg.tax_group_id = t.tax_group_id
    LEFT JOIN
            country ct ON ct.country_id = t.country_id
    LEFT JOIN
            state s ON s.state_id = t.state_id
    LEFT JOIN
            class_of_business cob ON cob.class_of_business_id = t.class_of_business_id
    WHERE   t.Tax_Calculation_cnt = @@IDENTITY
    AND     tt.tax_basis = 1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

