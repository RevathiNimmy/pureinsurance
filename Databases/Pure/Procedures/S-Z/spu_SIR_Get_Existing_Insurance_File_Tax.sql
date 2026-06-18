SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Get_Existing_Insurance_File_Tax'
GO

CREATE PROCEDURE spu_SIR_Get_Existing_Insurance_File_Tax

	@insurance_file_cnt int

AS

BEGIN

    -- Return the existing taxes 
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
    WHERE   t.Insurance_File_Cnt = @Insurance_File_Cnt  
    AND     tt.tax_basis = 2  
    AND t.transtype = 'TTIF'  
    ORDER BY  
            t.tax_group_id,  
            t.sequence  

END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
