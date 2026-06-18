SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_risk_claim_sel'
GO


CREATE PROCEDURE spu_risk_claim_sel
    @insurance_file_cnt int
AS


SELECT
    insurance_file_cnt,
    is_buildings,
    record_no,
    claim_date,
    claim_value
 FROM risk_claim
WHERE insurance_file_cnt = @insurance_file_cnt
ORDER BY record_no
GO


