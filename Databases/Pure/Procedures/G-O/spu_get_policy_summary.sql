SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_policy_summary'
GO

CREATE PROCEDURE spu_get_policy_summary
    @insurance_file_cnt int
AS

DECLARE 
    @tax_value money,
    @tax_value1 money,
    @tax_value2 money,
    @fee_value money

-- Get tax
SELECT  @tax_value1 = SUM(value) 
FROM    Tax_Calculation 
WHERE   insurance_file_cnt = @insurance_file_cnt
AND		risk_cnt IS NULL

SELECT  @tax_value2 = SUM(value) 
FROM    Tax_Calculation rt
JOIN    insurance_file_risk_link ifrl      ON ifrl.risk_cnt = rt.risk_cnt
JOIN    risk r                             ON r.risk_cnt = rt.risk_cnt
WHERE   ifrl.insurance_file_cnt = @insurance_file_cnt
AND     ifrl.status_flag <> 'U'
AND     r.is_risk_selected = 1
AND		rt.risk_cnt IS NOT NULL

SELECT  @tax_value = ISNULL(@tax_value1, 0) + ISNULL(@tax_value2, 0)


-- Get fee
SELECT  @fee_value = SUM(currency_amount)
FROM    policy_fee_u 
WHERE   insurance_file_cnt = @insurance_file_cnt


-- Return data
SELECT  i.insured_name,
        ISNULL(agent.resolved_name,'Direct Business'),
        i.inception_date_tpi,
        i.cover_start_date,
        i.expiry_date,
        ISNULL(i.this_premium,0),
        ISNULL(@tax_value,0),
        ISNULL(@fee_value,0),
        ISNULL(i.this_premium,0) + ISNULL(@tax_value,0) + ISNULL(@fee_value,0), -- total
        c.description
FROM    insurance_file i
LEFT JOIN 
        party agent    ON i.lead_agent_cnt = agent.party_cnt
LEFT JOIN 
        currency c     ON i.currency_id = c.currency_id
WHERE   i.insurance_file_cnt = @insurance_file_cnt

GO

