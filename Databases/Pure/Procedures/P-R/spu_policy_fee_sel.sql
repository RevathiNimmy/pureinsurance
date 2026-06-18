EXECUTE DDLDropProcedure 'spu_policy_fee_sel'
GO

CREATE PROCEDURE spu_policy_fee_sel
    @insurance_file_cnt INT

AS

SELECT
    pt.description,
    p.shortname,
    pf.fee_percentage,
    pf.fee_amount,
    pf.party_cnt,
    pf.commission_percentage,
    pf.commission_amount,
    pf.isIPTable,
    pf.extra_scheme_id,
    ex.Description ,
    isnull(pf.tax_amount,0),
    isnull(pf.total_fee,0),
    isnull(pf.commission_tax_amount,0) ,
    isnull(pf.total_commission,0),
    pf.policy_fee_id,pe.fee_charge, 
    	(select 
		isnull(max(extra_amount_basis),0) 
	from 
		fee_amounts 
	where 
		party_cnt=p.party_cnt and 
		((risk_group_id = rc.risk_group_id) or (isnull(rc.risk_group_id,0)=0 and risk_group_id is null)))
FROM policy_fee pf
JOIN party p
    ON p.party_cnt = pf.party_cnt
JOIN party_type pt
    ON pt.party_type_id = p.party_type_id
LEFT JOIN party_extra pe
    ON pe.party_cnt = pf.party_cnt
LEFT JOIN extra_scheme ex
    ON ex.extra_scheme_id = pf.extra_scheme_id
left join insurance_file iff 
	on iff.insurance_file_cnt = @insurance_file_cnt
left join risk_code rc
	on rc.risk_code_id = iff.risk_code_id
WHERE pf.insurance_file_cnt = @insurance_file_cnt
ORDER BY pf.policy_fee_id
go