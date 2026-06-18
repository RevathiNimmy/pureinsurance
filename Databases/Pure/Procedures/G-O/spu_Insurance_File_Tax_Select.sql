SET QUOTED_IDENTIFIER ON
GO
Execute DDLDropProcedure 'spu_Insurance_File_Tax_Select'
GO

 CREATE PROCEDURE spu_Insurance_File_Tax_Select
   @Insurance_File_Cnt int,  
    @Mode int,  
    @TransType varchar(4),  
 @RecalculateTaxes INT =1  
AS  
  
DECLARE  
    @effective_date datetime,  
    @currency_id smallint,  
    @company_id smallint,  
    @base_currency_id smallint,  
    @type_of_rate tinyint,  
    @total_fee_amount money,  
    @use_latest_nb_rate tinyint  
DECLARE  
 @system_option VARCHAR(20)  
  
SET @use_latest_nb_rate=0    

IF EXISTS(SELECT * FROM tax_calculation WHERE Insurance_File_Cnt=@Insurance_File_Cnt AND @mode=0 AND (is_manually_changed=1 OR @RecalculateTaxes=0) AND transtype = 'TTIF')  
BEGIN  
 -- Return our taxes  
    SELECT  t.insurance_file_cnt,  
            t.tax_band_id,  
            prem.this_premium,  
            t.percentage,  
            t.value,  
            t.is_value,  
            t.is_manually_changed,  
            tb.description,  
            tt.is_not_applied_to_client,  
            0, -- is deleted  
            t.basis_value,  
            t.calc_basis,  
            si.sum_insured,  
            t.sum_insured_rounded,  
            t.currency_id,  
            c.description,  
            t.allow_tax_credit,  
            t.original_sum_insured,  
            si.sum_insured - t.original_sum_insured,  
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
   t.apply_tax_by ,  
   tb.code 'TaxBandCode'  
  
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
        JOIN   (SELECT  tgtb.tax_band_id,  
                        tgtb.tax_group_id,  
                        tgtb.sequence,  
                        p.class_of_business_id,  
                        SUM(p.this_premium) [this_premium]  
                FROM    insurance_file_risk_link ifrl  
                JOIN    risk r ON r.risk_cnt = ifrl.risk_cnt  
                JOIN    peril p ON p.risk_cnt = r.risk_cnt  
                JOIN    tax_group_tax_band tgtb ON tgtb.tax_group_id  = p.tax_group  
                WHERE   ifrl.insurance_file_cnt = @insurance_file_cnt  
                AND     ifrl.status_flag <> 'U'  
                AND     r.is_risk_selected = 1  
                AND    (p.is_premium = 1 or p.is_levy_tax = 1)  
                GROUP BY  
                        tgtb.tax_band_id,  
                        tgtb.tax_group_id,  
                        tgtb.sequence,  
                        p.class_of_business_id) prem  
            ON  prem.tax_band_id = t.tax_band_id  
            AND prem.tax_group_id = t.tax_group_id  
    AND prem.sequence = t.sequence  
            AND prem.class_of_business_id = t.class_of_business_id  
        -- Get all sum insured values  
        LEFT JOIN  
               (SELECT  tgtb.tax_band_id,  
                        tgtb.tax_group_id,  
    tgtb.sequence,  
                        -- Get sum insured from rating section as individual perils aren't broken down  
                       (SELECT  SUM(rs2.sum_insured)  
                        FROM    insurance_file_risk_link ifrl2  
                        JOIN    risk r2 ON r2.risk_cnt = ifrl2.risk_cnt  
                        JOIN    rating_section rs2 ON rs2.risk_cnt = r2.risk_cnt  
          WHERE   rs2.rating_section_id IN (SELECT  rating_section_id  
                                                          FROM    peril p3  
                                                          JOIN    tax_group_tax_band tgtb3 ON tgtb3.tax_group_id = p3.tax_group  
                                                          WHERE   p3.risk_cnt = r2.risk_cnt  
                                                          AND     tgtb3.tax_band_id = tgtb.tax_band_id  
                                                          AND     tgtb3.tax_group_id = tgtb.tax_group_id)  
  
                        AND     ifrl2.insurance_file_cnt = @insurance_file_cnt  
                        AND     ifrl2.status_flag <> 'U'  
                        AND     r2.is_risk_selected = 1  
                        AND     rs2.original_flag = 0) [sum_insured]  
                FROM    insurance_file_risk_link ifrl  
                JOIN    risk r ON r.risk_cnt = ifrl.risk_cnt  
                JOIN    peril p ON p.risk_cnt = r.risk_cnt  
                JOIN    tax_group_tax_band tgtb ON tgtb.tax_group_id  = p.tax_group  
                WHERE   ifrl.insurance_file_cnt = @insurance_file_cnt  
                AND     ifrl.status_flag <> 'U'  
                AND     r.is_risk_selected = 1  
                AND    (p.is_sum_insured = 1 OR p.is_levy_tax = 1)  
                GROUP BY  
                        tgtb.tax_band_id,  
                        tgtb.tax_group_id,  
                        tgtb.sequence) si  
            ON  si.tax_band_id = t.tax_band_id  
            AND si.tax_group_id = t.tax_group_id  
            AND si.sequence = t.sequence  
  
    WHERE   t.Insurance_File_Cnt = @Insurance_File_Cnt  
    AND     tt.tax_basis = 2  
    AND     t.transtype = 'TTIF'  
 ORDER BY  
            t.tax_group_id,  
   cob.class_of_business_id,  
            t.sequence  
  
END  
ELSE  
BEGIN 
    -- Delete any existing risk taxes  
    DELETE  tax_calculation  
    WHERE   insurance_file_cnt = @Insurance_File_Cnt  
    AND     risk_cnt IS NULL  
  AND     transtype = 'TTIF'  
  
 SELECT  @system_option=value  
 FROM System_Options  
 WHERE option_number=1007 AND branch_id=1  
  
 -- Skip if tax switched off  
 IF (@system_option='0')  
  RETURN  
  
    -- If the business is fees underwiting (check this by the transaction)  
   IF @TransType IN ('NB', 'MTA', 'MTC', 'REN')
    BEGIN
        -- Get the total fee amount
        SELECT  @total_fee_amount = SUM(ISNULL(pf.currency_amount,0))
        FROM    policy_fee_u pf
		
        JOIN    fee_amounts fa
                ON  fa.transaction_type_id = pf.transaction_type_id
        
            AND fa.product_id = pf.product_id
            AND fa.party_cnt = pf.party_cnt
        WHERE   pf.insurance_file_cnt = @Insurance_File_Cnt
    AND NOT fa.tax_group_id IS NULL
    END

    IF (@mode = 2 or @mode=0)  
    BEGIN  
  
--NOTE TO DEVELOER – This section has been moved. It was originally further down in the SP.  
  
        -- If we are looking for an mta then we should check for return or additional  
        IF (@TransType = 'MTA')  
        BEGIN  
            SELECT  @TransType = CASE WHEN SUM(p.this_premium) < 0 THEN 'RMTA' ELSE 'AMTA' END  
            FROM    insurance_file_risk_link ifrl  
            JOIN    risk r ON r.risk_cnt = ifrl.risk_cnt  
            JOIN    peril p ON p.risk_cnt = r.risk_cnt  
            WHERE   ifrl.insurance_file_cnt = @insurance_file_cnt  
      AND     ifrl.status_flag <> 'U'  
            AND     r.is_risk_selected = 1  
            AND     p.is_premium = 1  
        END  
  
  SELECT  @system_option='0'  
  
  SELECT  @system_option=value  
  FROM System_Options  
  WHERE option_number=5019 AND branch_id=1  
  
  IF @system_option='1' OR @system_option='2' OR @system_option='3'  
  BEGIN  
         -- Get details from insurance file  
         SELECT  
        @effective_date = CASE WHEN @system_option='3' THEN inception_date_tpi ELSE ifi.cover_start_date END,  
     @currency_id = ifi.currency_id,  
                 @company_id = ifi.source_id  
         FROM    insurance_file ifi  
         WHERE   ifi.insurance_file_cnt = @Insurance_File_Cnt  
  
   -- IPT01 - Insurance Premium Tax  
   -- If this is a return Premium then check for the existence of  
   -- a previous version of the policy and if one exists look for a  
   -- valid tax rate from the effective date of the previous version  
   -- source_id that the tax refunded is correct.  
   IF @TransType = 'RMTA' OR @TransType = 'MTC'  
   BEGIN  
  
    -- Get details from insurance file  
    DECLARE @prev_insurance_file_cnt INT, @curr_risk_cnt  INT , @insurance_folder_cnt    INT  
    DECLARE @prev_effective_date datetime  
  
       SELECT  @prev_effective_date =inception_date_tpi ,@insurance_folder_cnt=insurance_folder_cnt  
       FROM Insurance_File WHERE insurance_file_cnt=  @Insurance_File_Cnt  
  
       SELECT TOP 1 @prev_insurance_file_cnt=insurance_file_cnt From Insurance_File  
       WHERE insurance_folder_cnt=@insurance_folder_cnt AND cover_start_date=@prev_effective_date  
  
    IF EXISTS(SELECT  
        1  
       FROM  
        insurance_file_risk_link ifrl  
       JOIN risk r  
        ON r.risk_cnt = ifrl.risk_cnt  
       JOIN peril p  
        ON p.risk_cnt = r.risk_cnt  
       JOIN tax_group_tax_band tgtb  
        ON tgtb.tax_group_id  = p.tax_group  
       JOIN tax_band tb  
        ON tb.tax_band_id = tgtb.tax_band_id  
       JOIN tax_type tt  
        ON tt.tax_type_id = tb.tax_type_id  
       JOIN tax_band_rate tbr  
        ON tbr.tax_band_id = tb.tax_band_id  
       JOIN Class_Of_Business cob  
        ON cob.class_of_business_id = p.class_of_business_id  
       WHERE ifrl.insurance_file_cnt = @prev_insurance_file_cnt  
  AND tbr.effective_date <= @prev_effective_date  
        AND tbr.use_for_refund_when_expired = 1  
        AND tbr.is_deleted = 0  
        AND tb.is_deleted = 0  
        AND (cob.class_of_business_id = tbr.class_of_business_id  
          OR ISNULL(tbr.class_of_business_id, 0) = 0)  
   AND tt.tax_basis=2  
        )  
     SELECT @effective_date = @prev_effective_date  
     END  
  IF @system_option = '2' OR @system_option='3'  
      BEGIN  
        SELECT @effective_date = ifi.inception_date_tpi,  
                             @currency_id = ifi.currency_id,  
                             @company_id = ifi.source_id  
        FROM   insurance_file ifi  
        WHERE  ifi.insurance_file_cnt = @Insurance_File_Cnt  
      END  
   -- IPT01 - Insurance Premium Tax  
   -- If this is a backdated NB transaction then check if there are any  
   -- rates which should be applied based on use_for_backdated_nb flag.  
   IF @TransType = 'NB' AND @effective_date < GETDATE()  
   BEGIN  
  
    IF EXISTS(SELECT  
        1  
       FROM  
        insurance_file_risk_link ifrl  
       JOIN risk r  
        ON r.risk_cnt = ifrl.risk_cnt  
       JOIN peril p  
        ON p.risk_cnt = r.risk_cnt  
       JOIN tax_group_tax_band tgtb  
        ON tgtb.tax_group_id  = p.tax_group  
       JOIN tax_band tb  
        ON tb.tax_band_id = tgtb.tax_band_id  
       JOIN tax_type tt  
        ON tt.tax_type_id = tb.tax_type_id  
       JOIN tax_band_rate tbr  
        ON tbr.tax_band_id = tb.tax_band_id  
       JOIN Class_Of_Business cob  
        ON cob.class_of_business_id = p.class_of_business_id  
       WHERE ifrl.insurance_file_cnt = @insurance_file_cnt  
  AND tbr.effective_date >= @effective_date AND tbr.effective_date <= GETDATE()  
  AND tbr.use_for_backdated_nb = 1  
        AND tbr.is_deleted = 0  
        AND tb.is_deleted = 0  
        AND (cob.class_of_business_id = tbr.class_of_business_id  
          OR ISNULL(tbr.class_of_business_id, 0) = 0)  
        AND tt.tax_basis=2  
        )  
    BEGIN  
     SELECT @effective_date = GETDATE(), @use_latest_nb_rate = 1  
    END  
  
   END  
  END  
  ELSE  
  BEGIN  
      -- Effective date should be transaction date rather than cover start date  
      SET @effective_date = GetDate()  
  
           -- Get details from insurance file  
           SELECT  
                   @currency_id = ifi.currency_id,  
                   @company_id = ifi.source_id  
           FROM    insurance_file ifi  
           WHERE   ifi.insurance_file_cnt = @Insurance_File_Cnt  
   END  
  
        -- Determine which branch to use  
        EXEC spu_ACT_GetTypeOfRates @type_of_rate OUTPUT  
        IF @type_of_rate =1  
            SELECT @company_id = 1  
  
        -- Get base currency  
        SELECT  @base_currency_id = base_currency_id  
        FROM    source  
        WHERE   source_id =  @company_id  
  
        -- Notes:  
        -- This uses a lot of trickery to achieve the required results, if you are not 100%  
        -- confifident of what it is doing contact me (Peter Finney) for assistance.  
        --  
        -- Basically,  
        --   The query does not access any tables directly as they are all useless in their  
        -- native states. Instead it build up 5 virtual tables:  
        --     bands    - Lists all tax bands applicable to the current policy  
        --     prem     - Lists the premiums applicable to all bands referenced in the current policy  
        --     si       - Lists the sum_insured values applicable to all bands referenced in the current policy  
        --     osi      - Lists the original sum_insured values applicable to all bands referenced in the current policy  
        --     rates    - Lists the current applicable rate for each tax band  
        -- These are then brought together on the tax_band_id to calculate the appropriate values  

CREATE TABLE #TAXCALC
(
	 ID								INT IDENTITY Primary Key,
       insurance_file_cnt INT,  
                tax_band_id INT,  
                premium NUMERIC(19, 4),  
                percentage NUMERIC(19, 4),  
                value INT,  
                is_value INT,  
                is_manually_changed INT,  
                calc_basis INT,  
                basis_value INT,  
                sum_insured  NUMERIC(19, 4),  
                sum_insured_rounded  NUMERIC(19, 4),  
                currency_id INT,  
                allow_tax_credit INT,  
                original_sum_insured NUMERIC(19, 4),  
                country_id INT,  
                state_id INT,  
                class_of_business_id INT,  
                tax_group_id INT,  
                sequence INT,  
                transtype VARCHAR(10),  
                include_tax_in_instalments INT,  
           spread_tax_across_instalments INT,  
            is_not_applied_to_client INT,  
                apply_tax_by INT
	
)
 
        INSERT INTO #TAXCALC (  
		
                insurance_file_cnt,  
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
                apply_tax_by) -- (RC)  
        SELECT  @insurance_file_cnt,  
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
                'TTIF',  
              bands.is_include_tax_in_instalments,  
            bands.is_spread_tax_across_instalments,  
            bands.is_not_applied_to_client,  
            @system_option   -- (RC)  
  
        -- Get all active tax_bands  
        FROM   (SELECT  tgtb.tax_band_id,  
                        tgtb.tax_group_id,  
                        tgtb.sequence,  
                        rs.country_id,  
                        rs.state_id,  
                        p.class_of_business_id,  
        (Select distinct is_include_tax_in_instalments from tax_type tax_t where tax_t.tax_type_id = tt.tax_type_id) is_include_tax_in_instalments,    --GAURAV  
   (Select distinct is_spread_tax_across_instalments from tax_type tax_t where tax_t.tax_type_id = tt.tax_type_id) is_spread_tax_across_instalments, --GAURAV  
                (Select distinct is_not_applied_to_client from tax_type tax_t where tax_t.tax_type_id = tt.tax_type_id) is_not_applied_to_client  
                FROM    insurance_file_risk_link ifrl  
                JOIN    risk r ON r.risk_cnt = ifrl.risk_cnt  
                JOIN    peril p ON p.risk_cnt = r.risk_cnt  
                JOIN    tax_group_tax_band tgtb ON tgtb.tax_group_id  = p.tax_group  
                JOIN    tax_band tb ON tb.tax_band_id = tgtb.tax_band_id  
                JOIN    tax_type tt ON tt.tax_type_id = tb.tax_type_id  
                JOIN    rating_section rs ON rs.risk_cnt = p.risk_cnt  
                                         AND rs.rating_section_id = p.rating_section_id  
                WHERE   ifrl.insurance_file_cnt = @Insurance_File_Cnt  
                AND     ifrl.status_flag <> 'U'  
                AND     r.is_risk_selected = 1  
                AND    (p.is_premium = 1 OR p.is_sum_insured = 1 OR p.is_levy_tax = 1)  
  AND  rs.original_flag IN(1,0)  
                AND     tt.tax_basis = 2  
                -- ensure only active tax types are used  
                AND     tt.effective_date = (SELECT  MAX(effective_date)  
                                             FROM    tax_type tt2  
                                             WHERE   tt2.tax_type_id = tt.tax_type_id  
                                                 AND tt2.tax_basis = 2  
                                                 AND tt2.effective_date <=  @effective_date  
                                                 AND tt2.is_deleted = 0)  
                AND     tb.is_deleted = 0  
    GROUP BY  
                        tgtb.tax_band_id,  
                        tgtb.tax_group_id,  
                        tgtb.sequence,  
                        rs.country_id,  
                        rs.state_id,  
                        p.class_of_business_id,  
   tt.tax_type_id) bands --GAURAV  
  
        -- Get all premium values by tax band and class of business  
        JOIN   (SELECT  tgtb.tax_band_id,  
                        tgtb.tax_group_id,  
                        tgtb.sequence,  
                        p.class_of_business_id,  
                        SUM(p.this_premium) [this_premium],  
      rs.country_id,  
                        rs.state_id  
                FROM    insurance_file_risk_link ifrl  
                JOIN    risk r ON r.risk_cnt = ifrl.risk_cnt  
                JOIN    peril p ON p.risk_cnt = r.risk_cnt  
    JOIN    rating_section rs ON rs.risk_cnt = p.risk_cnt  
                                         AND rs.rating_section_id = p.rating_section_id  
                JOIN    tax_group_tax_band tgtb ON tgtb.tax_group_id  = p.tax_group  
                WHERE   ifrl.insurance_file_cnt = @insurance_file_cnt  
                AND     ifrl.status_flag <> 'U'  
                AND     r.is_risk_selected = 1  
                AND    (p.is_premium = 1 or p.is_levy_tax = 1)  
                GROUP BY  
                        tgtb.tax_band_id,  
                        tgtb.tax_group_id,  
                        tgtb.sequence,  
                        p.class_of_business_id,  
                        rs.country_id,  
                        rs.state_id) prem  
            ON  prem.tax_band_id = bands.tax_band_id  
            AND prem.tax_group_id = bands.tax_group_id  
   AND prem.sequence = bands.sequence  
            AND prem.class_of_business_id = bands.class_of_business_id  
   AND ISNULL(prem.country_id, 0) = ISNULL(bands.country_id, 0)  
            AND ISNULL(prem.state_id, 0) = ISNULL(bands.state_id, 0)  
  
        -- Get all sum insured values  
        LEFT JOIN  
               (SELECT  tgtb.tax_band_id,  
                        tgtb.tax_group_id,  
                        tgtb.sequence,  
                        0[sum_insured],  
            p.class_of_business_id,  
                 rs.country_id,  
                        rs.state_id  
                FROM    insurance_file_risk_link ifrl  
                JOIN    risk r ON r.risk_cnt = ifrl.risk_cnt  
                JOIN    peril p ON p.risk_cnt = r.risk_cnt  
                JOIN    rating_section rs ON rs.risk_cnt = p.risk_cnt  
                                         AND rs.rating_section_id = p.rating_section_id  
                JOIN    tax_group_tax_band tgtb ON tgtb.tax_group_id  = p.tax_group  
                WHERE   ifrl.insurance_file_cnt = @insurance_file_cnt  
                AND     ifrl.status_flag <> 'U'  
                AND     r.is_risk_selected = 1  
                AND    (p.is_sum_insured = 1 OR p.is_levy_tax = 1)  
                GROUP BY  
                        tgtb.tax_band_id,  
                        tgtb.tax_group_id,  
                        tgtb.sequence,  
            p.class_of_business_id,  
                        rs.country_id,  
                        rs.state_id) si  
            ON  si.tax_band_id = bands.tax_band_id  
            AND si.tax_group_id = bands.tax_group_id  
            AND si.sequence = bands.sequence  
            AND si.class_of_business_id = bands.class_of_business_id  
            AND ISNULL(si.country_id, 0) = ISNULL(bands.country_id, 0)  
            AND ISNULL(si.state_id, 0) = ISNULL(bands.state_id, 0)  
        -- Get all original sum insured values  
        LEFT JOIN  
               (SELECT  tgtb.tax_band_id,  
                        tgtb.tax_group_id,  
                        tgtb.sequence,  
                        0 [sum_insured],  
            p.class_of_business_id,  
                 rs.country_id,  
                        rs.state_id                         --RC PN37141  
                FROM    insurance_file_risk_link ifrl  
                JOIN    risk r ON (r.risk_cnt = ifrl.original_risk_cnt OR r.risk_cnt = ifrl.renewed_risk_cnt)  
         JOIN    peril p ON p.risk_cnt = r.risk_cnt  
                JOIN    rating_section rs ON rs.risk_cnt = p.risk_cnt  
                                         AND rs.rating_section_id = p.rating_section_id  
           JOIN    tax_group_tax_band tgtb ON tgtb.tax_group_id  = p.tax_group  
                WHERE   ifrl.insurance_file_cnt = @insurance_file_cnt  
                AND     ifrl.status_flag <> 'U'  
                AND     r.is_risk_selected = 1  
                AND    (p.is_sum_insured = 1 OR p.is_levy_tax = 1)  
                GROUP BY  
      tgtb.tax_band_id,  
                        tgtb.tax_group_id,  
                        tgtb.sequence,  
   p.class_of_business_id,  
                        rs.country_id,  
                        rs.state_id) osi  
            ON  osi.tax_band_id = bands.tax_band_id  
            AND osi.tax_group_id = bands.tax_group_id  
            AND osi.sequence = bands.sequence  
            AND osi.class_of_business_id = bands.class_of_business_id  
            AND ISNULL(osi.country_id, 0) = ISNULL(bands.country_id, 0)  
            AND ISNULL(osi.state_id, 0) = ISNULL(bands.state_id, 0)  
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
                                                              AND     company_id = CRFromBase.company_id)),  
                    rate =  
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
                                                            AND  company_id = CRToBase.company_id)  
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
                WHERE   tbr.effective_date = (SELECT  MAX(effective_date)  
                                              FROM    tax_band_rate tbr2  
                                              WHERE   tbr2.tax_band_id = tbr.tax_band_id  
                                                  AND ISNULL(tbr2.country_id, 0) = ISNULL(tbr.country_id, 0)  
                                                  AND ISNULL(tbr2.state_id, 0) = ISNULL(tbr.state_id, 0)  
                                                  AND ISNULL(tbr2.class_of_business_id, 0) = ISNULL(tbr.class_of_business_id, 0)  
                                                  AND tbr2.effective_date <= @effective_date  
                                                  AND is_deleted = 0  
                                                  AND ((@use_latest_nb_rate = isnull(tbr2.use_for_backdated_nb,0)) OR (@use_latest_nb_rate = 0))  
                                                      -- check tax usage  
                                                  AND CASE @TransType  
                                                          WHEN 'NB'  THEN tbr.NB  
                                                          WHEN 'AMTA' THEN tbr.AMTA  
                                                          WHEN 'RMTA' THEN tbr.RMTA  
                                                          WHEN 'MTC' THEN tbr.CANC  
                                                          WHEN 'REN' THEN tbr.REN  
                                                          WHEN 'MTR' THEN tbr.NB  
                                                          ELSE 1  
                                                      END > 0)  
                        -- check tax usage  
                AND     CASE @TransType  
                            WHEN 'NB'  THEN tbr.NB  
                            WHEN 'AMTA' THEN tbr.AMTA  
           WHEN 'RMTA' THEN tbr.RMTA  
                            WHEN 'MTC' THEN tbr.CANC  
                            WHEN 'REN' THEN tbr.REN  
                            WHEN 'MTR' THEN tbr.NB  
                            ELSE 1  
                        END > 0  
                AND     tbr.is_deleted = 0) rates  
        ON  rates.tax_band_id = bands.tax_band_id  
            AND (ISNULL(rates.country_id, 0) = ISNULL(bands.country_id, 0) OR rates.country_id IS NULL )  
            AND ISNULL(rates.state_id, 0) = ISNULL(bands.state_id, 0)  
            AND ISNULL(rates.class_of_business_id, 0) = ISNULL(bands.class_of_business_id, 0)  
    END -- (@mode = 2)  


		Declare @premium as NUMERIC(19, 4)
	
		Declare @sum_insured as NUMERIC(19, 4)
		Declare @sum_insured_rounded as NUMERIC(19, 4)

		 
	DELETE from #TAXCALC WHERE id in
		(SELECT id FROM
		( 
		SELECT id, tax_band_id,ROW_NUMBER() OVER ( PARTITION BY tax_band_id ORDER BY Is_value DESC) row_num   FROM #TAXCALC WHERE is_value=1
		) TEMPTAX WHERE row_num >1
		) 

		
		
		Declare @OriginalSumInsured numeric(20,4)
		SELECT  @sum_insured= SUM(rs2.sum_insured)  
                        FROM    insurance_file_risk_link ifrl2  
                        JOIN    risk r2 ON r2.risk_cnt = ifrl2.risk_cnt  
                        JOIN    rating_section rs2 ON rs2.risk_cnt = r2.risk_cnt  
           
                  
                WHERE   ifrl2.insurance_file_cnt = @insurance_file_cnt  
                AND     ifrl2.status_flag <> 'U'  
                AND     r2.is_risk_selected = 1  and rs2.original_flag = 0   
				
				SELECT  @OriginalSumInsured= SUM(rs2.sum_insured)  
                        FROM    insurance_file_risk_link ifrl2  
                        JOIN    risk r2 ON r2.risk_cnt = ifrl2.risk_cnt  
                        JOIN    rating_section rs2 ON rs2.risk_cnt = r2.risk_cnt  
           
                  
                WHERE   ifrl2.insurance_file_cnt = @insurance_file_cnt  
                AND     ifrl2.status_flag <> 'U'  
                AND     r2.is_risk_selected = 1  and rs2.original_flag = 1
				
	
		
		SELECT  @premium=sum(premium ),@sum_insured_rounded=sum(sum_insured_rounded) from #TAXCALC where is_value=1

		Update  #TAXCALC set  sum_insured =@sum_insured,premium=@premium ,sum_insured_rounded =@sum_insured_rounded ,class_of_business_id=Null WHERE id in
		(SELECT id FROM
		( 
		SELECT id, tax_band_id,ROW_NUMBER() OVER ( PARTITION BY tax_band_id ORDER BY Is_value DESC) row_num   FROM #TAXCALC WHERE is_value=1
		) TEMPTAX WHERE row_num > 0
		) 
		 
        -- Insert all new taxes 
INSERT INTO Tax_Calculation(  
                insurance_file_cnt,  
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
  SELECT        insurance_file_cnt,  
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
                ISNULL(@OriginalSumInsured, 0), 
                country_id,  
                state_id,  
                class_of_business_id,  
                tax_group_id,  
                sequence,  
                transtype,  
                include_tax_in_instalments,  
           spread_tax_across_instalments,  
            is_not_applied_to_client,  
                apply_tax_by from #TAXCALC 

	DROP TABLE #TAXCALC

    SELECT  t.insurance_file_cnt,  
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
   t.apply_tax_by ,  
   tb.code 'TaxBandCode'  
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
    WHERE   t.Insurance_File_Cnt = @Insurance_File_Cnt  
    AND     tt.tax_basis = 2  
    AND     t.transtype = 'TTIF'  
    ORDER BY  
            t.tax_group_id,  
   cob.class_of_business_id,  
            t.sequence  
END   
  
GO
SET QUOTED_IDENTIFIER OFF
GO
