SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_fee_amounts_sel'
GO

CREATE PROCEDURE spe_fee_amounts_sel
	@language_id INT,
    @party_cnt INT

AS

SELECT
	fa.risk_group_id,
	pc.caption,
	fa.fee_percentage,
	fa.fee_amount,
	fa.commission_percentage,
	fa.Commission_amount,
	fa.display_on_quotes,
	fa.transaction_type_id,
	fa.tax_group_id,
	fa.extra_scheme_id,
	es.description,
	fa.currency_id,
	c.description,
	fa.commission_tax_group_id,
	fa.fsa_type_of_sale_id
FROM fee_amounts fa
LEFT JOIN risk_group rg
	ON rg.risk_group_id = fa.risk_group_id
LEFT JOIN pmcaption pc
	ON pc.caption_id = rg.caption_id
LEFT JOIN extra_scheme es 
	ON es.extra_scheme_id = fa.extra_scheme_id
LEFT JOIN currency c
	ON c.currency_id = fa.currency_id
WHERE party_cnt = @party_cnt
AND pc.language_id = @language_id

GO

