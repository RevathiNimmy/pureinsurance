SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF
GO

DDLDropProcedure 'spu_risks_for_policy_saa'
GO

CREATE PROCEDURE spu_risks_for_policy_saa
    @insurance_file_cnt int,
    @risk_status char
AS
DECLARE @status_flag char
DECLARE @risk_cnt as int
IF @risk_status = 'A'
BEGIN
    SELECT  r.risk_cnt,
        r.description,
        rt.risk_type_id,
        p.report_pointer,
        rt.header_clause_id,
        rt.trailer_clause_id,
		ifrl.original_risk_cnt
    FROM    Risk r,
        Risk_Type rt,
	product p,
	Insurance_file i,
        Insurance_file_risk_link ifrl
    WHERE i.insurance_file_cnt = @insurance_file_cnt
    AND ifrl.insurance_file_cnt = i.insurance_file_cnt
    --AND ifrl.status_flag <> 'D'
    AND ifrl.risk_cnt = r.risk_cnt
    AND r.risk_type_id = rt.risk_type_id
    AND p.product_id = i.product_id
    ORDER BY 
		rt.primary_sort, rt.secondary_sort, r.risk_number
END
IF @risk_status = 'D'
BEGIN
    SELECT  r.risk_cnt,
        r.description,
        rt.risk_type_id,
        p.report_pointer,
        rt.header_clause_id,
        rt.trailer_clause_id
    FROM    Risk r,
        Risk_Type rt,
        product p,
        Insurance_file i,
        Insurance_file_risk_link ifrl
    WHERE i.insurance_file_cnt = @insurance_file_cnt
    AND ifrl.insurance_file_cnt = i.insurance_file_cnt
    AND ifrl.status_flag = 'D'
    AND ifrl.risk_cnt = r.risk_cnt
    AND r.risk_type_id = rt.risk_type_id
    AND p.product_id = i.product_id
    ORDER BY
        rt.primary_sort, rt.secondary_sort, r.risk_number
END
IF @risk_status = 'N'
BEGIN
    SELECT  r.risk_cnt,
        r.description,
        rt.risk_type_id,
        p.report_pointer,
        rt.header_clause_id,
        rt.trailer_clause_id
    FROM    Risk r,
        Risk_Type rt,
        product p,
        Insurance_file i,
        Insurance_file_risk_link ifrl
    WHERE i.insurance_file_cnt = @insurance_file_cnt
    AND ifrl.insurance_file_cnt = i.insurance_file_cnt
    AND ifrl.status_flag = 'C'
    AND ifrl.original_risk_cnt is null
    AND r.risk_cnt = ifrl.risk_cnt
    AND r.risk_type_id = rt.risk_type_id
    AND p.product_id = i.product_id
    ORDER BY
        rt.primary_sort, rt.secondary_sort, r.risk_number
END
IF @risk_status = 'C'
BEGIN
    SELECT  r.risk_cnt,
        r.description,
        rt.risk_type_id,
        p.report_pointer,
        rt.header_clause_id,
        rt.trailer_clause_id
    FROM    Risk r,
        Risk_Type rt,
        product p,
        Insurance_file i,
        Insurance_file_risk_link ifrl
    WHERE i.insurance_file_cnt = @insurance_file_cnt
    AND ifrl.insurance_file_cnt = i.insurance_file_cnt
    AND ifrl.status_flag = 'C'
    AND ifrl.original_risk_cnt is not null
    AND ifrl.is_risk_edited=1
    AND r.risk_cnt = ifrl.risk_cnt
    AND r.risk_type_id = rt.risk_type_id
    AND p.product_id = i.product_id
    ORDER BY
        rt.primary_sort, rt.secondary_sort, r.risk_number
END
GO
