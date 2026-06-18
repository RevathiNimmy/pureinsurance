SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_Risk_OverrideTax_Select'
GO

CREATE PROCEDURE spu_Risk_OverrideTax_Select
    @risk_cnt int,
    @Mode int,
    @TransType varchar(4) ,
    @insurance_file_cnt int
AS
 
 SELECT  t.risk_cnt,
            t.tax_band_id,
            prem.this_premium,
            t.percentage,
            t.value,
            t.is_value,
            t.is_manually_changed,
            tb.description,
            tt.is_not_applied_to_client,
            0 [is_deleted], -- is deleted
            t.basis_value,
            t.calc_basis,
            si.sum_insured,
            t.sum_insured_rounded,
            t.currency_id,
            c.description,
            t.allow_tax_credit,
            t.original_sum_insured,
            si.sum_insured - t.original_sum_insured [sum_insured],
            tg.tax_group_id,
            tg.description,
            t.sequence,
            ct.country_id,
            ct.description,
            s.state_id,
            s.description,
            cob.class_of_business_id,
            cob.description,
            0 [running_total], -- running total
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
                        sum(p.this_premium) [this_premium]
                FROM    peril p
                JOIN    tax_group_tax_band tgtb ON tgtb.tax_group_id  = p.tax_group
				join rating_section rs on rs.rating_section_id = p.rating_section_id and rs.risk_cnt = p.risk_cnt
                WHERE   p.risk_cnt = @risk_cnt
                AND    (p.is_premium = 1 or p.is_levy_tax = 1)
				and rs.original_flag = 0
                GROUP BY
                        tgtb.tax_band_id,
                        tgtb.tax_group_id,
                        tgtb.sequence,
                        p.class_of_business_id) prem
            ON  prem.tax_band_id = t.tax_band_id
            AND prem.tax_group_id = t.tax_group_id
            AND prem.sequence = t.sequence
            AND prem.class_of_business_id = t.class_of_business_id
            JOIN (SELECT  tgtb.tax_band_id,
                        tgtb.tax_group_id,
                        min(tgtb.sequence) [sequence],
                       (SELECT  SUM(rs.sum_insured)
                        FROM    rating_section rs
                    WHERE   rs.rating_section_id IN (SELECT  rating_section_id
                                                         FROM    peril p2
                                                         JOIN    tax_group_tax_band tgtb3 ON tgtb3.tax_group_id  = p2.tax_group
                                                         WHERE   tgtb3.tax_band_id = tgtb.tax_band_id
                                                         AND     tgtb3.tax_group_id = tgtb.tax_group_id
                                                         AND     p2.risk_cnt = @risk_cnt)
                        AND     rs.risk_cnt = @risk_cnt
                        AND     rs.original_flag = 0) [sum_insured],
                        p.class_of_business_id
                FROM    peril p
                JOIN    tax_group_tax_band tgtb ON tgtb.tax_group_id  = p.tax_group
                WHERE   p.risk_cnt = @risk_cnt
                AND    (p.is_premium = 1 OR p.is_sum_insured = 1 OR p.is_levy_tax = 1)
                GROUP BY
                        tgtb.tax_band_id,
                        tgtb.tax_group_id,
                        tgtb.sequence,
                        p.class_of_business_id) si
                ON     si.tax_band_id = t.tax_band_id
               AND     si.tax_group_id = t.tax_group_id
               AND     si.sequence = t.sequence
               AND     si.class_of_business_id = t.class_of_business_id
    WHERE    t.risk_cnt = @risk_cnt
    AND      tt.tax_basis = 1
    AND      t.transtype = 'TTR'
    AND      t.insurance_file_cnt = @insurance_file_cnt -- EM renewal risk change
	AND t.tax_calculation_cnt = (SELECT max(tax_calculation_cnt)
									 FROM Tax_Calculation tc 
									 WHERE tc.risk_cnt = t.risk_cnt
										   and tc.transtype = t.transtype
										   and tc.insurance_file_cnt = t.insurance_file_cnt)
    ORDER BY t.tax_group_id,cob.class_of_business_id,  t.sequence

