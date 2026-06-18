SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Fee_amounts_sel'
GO

-- AMB 03/10/2003: Accident Management RFC changes
-- DC190504 PN9416 added tax code so as to check if IPTable
-- DC140205 : PN18685 : Is Fee Chargeable ?
CREATE PROCEDURE spu_Fee_amounts_sel
    @party_cnt      int,
    @risk_group_id  int,
    @extra_scheme_id  int
AS

SELECT
    fa.party_cnt,
    fa.risk_group_id,
    fa.fee_percentage,
    fa.fee_amount,
    fa.commission_percentage,
    fa.commission_amount,
    fa.display_on_quotes,
    UPPER(tr.code),
    pe.fee_charge,
    fa.include_fee_in_instalments,
    fa.spread_fee_across_instalments,
    fa.tax_group_id,
    fa.fsa_type_of_sale_id
FROM 
    Fee_amounts fa
left JOIN tax_rates tr ON fa.tax_rates_id = tr.tax_rates_id
left join party_extra pe on fa.party_cnt = pe.party_cnt
WHERE 
    fa.party_cnt = @party_cnt 
AND 
    fa.risk_group_id = @risk_group_id
AND
(    fa.extra_scheme_id = @extra_scheme_id
     OR  (@extra_scheme_id = 0
	  AND fa.extra_scheme_id is null)
)

GO


