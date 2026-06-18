SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_tax_band_rate_saa'
GO

CREATE PROCEDURE spu_tax_band_rate_saa
    @Tax_Band_id int
AS

    SELECT  tbr.tax_band_id,
        	tbr.tax_band_rate_id,
        	tbr.code,
        	tbr.caption_id,
        	tbr.description,
        	tbr.effective_date,
        	tbr.is_deleted,
        	tbr.is_value,
        	tbr.rate,
        	tbr.Calc_Basis,
        	tbr.Basis_Value,
        	tbr.NB,
        	tbr.AMTA,
        	tbr.RMTA,
        	tbr.CANC,
        	tbr.REN, 
            tbr.sum_insured_rounded,
            tbr.currency_id,
            tbr.allow_tax_credit,
            tbr.country_id,
            tbr.state_id,
            tbr.class_of_business_id,
            cob.description,
            c.description,
            s.description,
            tbr.TTRI,
            tbr.TTRIC,
            tbr.TTAC,
            tbr.TTF,
            tbr.TTCP,
            tbr.TTCS,
            tbr.TTCR,
            tbr.TTIC,
            tbr.MTA_Threshold_date,
            tbr.Is_passed_to_insurer,
            tbr.TTI,
            tbr.TTE,
            tbr.risk_group_id,
            tbr.risk_code_id,
            tbr.COB_rating_section_id,
            rg.description,
            rc.description,
            crs.description,
            tbr.use_for_refund_when_expired,
            tbr.use_for_backdated_nb,
            tbr.TTRIPR
    FROM    tax_band_rate tbr
    LEFT JOIN class_of_business cob ON cob.class_of_business_id = tbr.class_of_business_id
    LEFT JOIN country c ON c.country_id = tbr.country_id
    LEFT JOIN state s ON s.state_id = tbr.state_id
    LEFT JOIN risk_group rg ON rg.risk_group_id = tbr.risk_group_id
    LEFT JOIN risk_code rc ON rc.risk_code_id = tbr.risk_code_id
    LEFT JOIN COB_rating_section crs ON crs.COB_rating_section_id = tbr.COB_rating_section_id
    WHERE   tbr.tax_band_id = @tax_band_id
    ORDER BY tbr.effective_date DESC


GO

