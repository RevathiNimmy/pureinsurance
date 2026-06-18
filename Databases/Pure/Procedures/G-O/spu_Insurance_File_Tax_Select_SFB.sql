SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Insurance_File_Tax_Select_SFB'
GO

CREATE PROCEDURE spu_Insurance_File_Tax_Select_SFB  
    @Insurance_File_Cnt int,  
    @Mode int,  
    @TransType varchar(4)  
AS  
  
    DECLARE  
        @effective_date datetime,  
        @currency_id smallint,  
        @company_id smallint,  
        @base_currency_id smallint,  
        @type_of_rate tinyint,  
        @total_fee_amount money  
  
    -- If the business is fees underwiting (check this by the transaction)  
/*    IF @TransType IN ('NB', 'MTA', 'MTC', 'REN')  
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
*/  
    IF (@mode = 2)  
    BEGIN  
        -- Get details from insurance file  
        SELECT  @effective_date = ifi.Cover_Start_Date,  
         @currency_id = ifi.currency_id,  
         @company_id = ifi.source_id  
        FROM    insurance_file ifi  
        WHERE   ifi.insurance_file_cnt = @Insurance_File_Cnt  
  
        -- Determine which branch to use  
        EXEC spu_ACT_GetTypeOfRates @type_of_rate OUTPUT  
        IF @type_of_rate =1  
            SELECT @company_id = 1  
  
        -- Get base currency  
        SELECT  @base_currency_id = base_currency_id  
        FROM    source  
        WHERE   source_id =  @company_id  
  
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
  
        -- Delete any existing risk taxes  
        DELETE  tax_calculation  
        WHERE   insurance_file_cnt = @Insurance_File_Cnt  
        AND risk_cnt IS NULL  
	AND transtype = 'TTIF'
  
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
  
        -- Insert all new taxes  
  INSERT INTO Tax_Calculation (  
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
	        insurance_section_id)  
     	SELECT  @insurance_file_cnt,  
       		bands.tax_band_id,  
                ISNULL(prem.this_premium, 0),  
                rates.rate,  
                0, -- tax value now calculated by business object!  
                rates.is_value,  
                0,  
                rates.calc_basis,  
                rates.basis_value,  
                0,  
                rates.sum_insured_rounded,  
                @currency_id, -- Currency of taxes have changed to match insurance file  
                rates.allow_tax_credit,  
                0,  
                bands.country_id,  
                bands.state_id,  
                bands.class_of_business_id,  
                bands.tax_group_id,  
                bands.sequence,  
    		'TTIF',
		bands.insurance_section_id  
        -- Get all active tax_bands  
  
        FROM   (SELECT  tgtb.tax_band_id,  
                        tgtb.tax_group_id,  
                        tgtb.sequence,  
                        a.country_id,  
                        NULL 'state_id',  
                        1 'class_of_business_id',
			iCOBs.insurance_section_id 'insurance_section_id'
  
                FROM    insurance_file i
		JOIN	insurance_COB_section iCOBs ON iCOBS.insurance_file_cnt = i.insurance_file_cnt 
                JOIN    tax_group_tax_band tgtb ON tgtb.tax_group_id  = iCOBs.tax_group_id  
                JOIN    tax_band tb ON tb.tax_band_id = tgtb.tax_band_id 
		JOIN 	tax_band_rate tbr ON tbr.tax_band_id = tb.tax_band_id 
                JOIN    tax_type tt ON tt.tax_type_id = tb.tax_type_id
		JOIN    party_address_usage pau ON pau.party_cnt = i.insured_cnt
		JOIN    address a ON a.address_cnt = pau.address_cnt
		JOIN	address_usage_type aut ON aut.address_usage_type_id = pau.address_usage_type_id
                WHERE   i.insurance_file_cnt = @Insurance_file_Cnt
		AND     aut.code = '3131 XCO'  
                AND     tt.tax_basis = 2  
                -- ensure only active tax types are used  
                AND     tt.effective_date = (SELECT  MAX(effective_date)  
                                             FROM    tax_type tt2  
                                             WHERE   tt2.tax_type_id = tt.tax_type_id  
                                                 AND tt2.tax_basis = 2  
                                                 AND tt2.effective_date <= @effective_date 
                                                 AND tt2.is_deleted = 0)  
    		AND  tb.is_deleted=0  
                GROUP BY  
                        tgtb.tax_band_id,  
                        tgtb.tax_group_id,  
                        tgtb.sequence,  
                        a.country_id,
		        iCOBs.insurance_section_id) bands  
        -- Get all premium values by tax band and class of business  
        JOIN   (SELECT  tgtb.tax_band_id,  
                        tgtb.tax_group_id,  
                        tgtb.sequence,
			1 'class_of_business_id',  
                        SUM(iCOBS.premium_excluding_tax) [this_premium]  
                FROM    insurance_COB_Section iCOBs  
                JOIN    tax_group_tax_band tgtb ON tgtb.tax_group_id  = iCOBS.tax_group_id  
                WHERE   iCOBS.insurance_file_cnt = @insurance_file_cnt  
 
                GROUP BY  
                        tgtb.tax_band_id,  
                        tgtb.tax_group_id,  
                        tgtb.sequence ) prem  
            ON  prem.tax_band_id = bands.tax_band_id  
            AND prem.tax_group_id = bands.tax_group_id  
            AND prem.sequence = bands.sequence  
            AND prem.class_of_business_id = bands.class_of_business_id  
 
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
                    tbr.class_of_business_id,  
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
                        AND     CRToBase.currency_id = tbr.currency_id  
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
                        AND     CRToBase.currency_id = tbr.currency_id  
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
                        -- ensure most recent effective rates  
                WHERE   tbr.effective_date = (SELECT  MAX(effective_date)  
                                               FROM    tax_band_rate tbr2  
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
                    AND CASE @TransType  
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
    END -- (@mode = 2)  
    -- Return our taxes  
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
      t.transtype  
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
            t.sequence  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
